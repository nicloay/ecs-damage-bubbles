using EcsDamageBubbles.Config;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDamageBubbles
{
    /// <summary>
    ///     Replace DamageRequest tag with DamageBubble text
    /// </summary>
    public partial struct DamageBubbleSpawnSystem : ISystem
    {
        private NativeArray<float4> _colorConfig;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<DamageBubblesConfig>();
            state.RequireForUpdate<DamageBubbleColorConfig>();
        }

        public void OnDestroy(ref SystemState state)
        {
            _colorConfig.Dispose();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (_colorConfig == default)
            {
                var damageColorConfig = SystemAPI.GetSingletonBuffer<DamageBubbleColorConfig>(true);
                _colorConfig = new NativeArray<float4>(damageColorConfig.Length, Allocator.Persistent);
                for (var i = 0; i < _colorConfig.Length; i++) _colorConfig[i] = damageColorConfig[i].Color;
            }

            var config = SystemAPI.GetSingleton<DamageBubblesConfig>();

            var elapsedTime = (float)SystemAPI.Time.ElapsedTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();

            new ApplyGlyphsJob
            {
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                ElapsedTime = elapsedTime,
                ColorConfig = _colorConfig,
                GlyphEntity = config.GlyphPrefab,
                GlyphZOffset = config.GlyphZOffset,
                GlyphWidth = config.GlyphWidth
            }.ScheduleParallel();
        }


        [BurstCompile]
        [WithNone(typeof(DamageBubble.DamageBubble))]
        public partial struct ApplyGlyphsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;
            [ReadOnly] public Entity GlyphEntity;
            [ReadOnly] public float ElapsedTime;
            [ReadOnly] public float GlyphZOffset;
            [ReadOnly] public float GlyphWidth;
            [ReadOnly] public NativeArray<float4> ColorConfig;

            public void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, in LocalTransform transform,
                in DamageBubbleRequest damageBubbleRequest)
            {
                var number = damageBubbleRequest.Value;
                var color = ColorConfig[damageBubbleRequest.ColorId];
                var glyphTransform = transform;
                var offset = math.log10(number) / 2f * GlyphWidth;
                glyphTransform.Position.x += offset;

                // split to numbers
                // we iterate from  rightmost digit to leftmost
                while (number > 0)
                {
                    var digit = number % 10;
                    number /= 10;
                    var glyph = Ecb.Instantiate(chunkIndex, GlyphEntity);
                    Ecb.SetComponent(chunkIndex, glyph, glyphTransform);
                    glyphTransform.Position.x -= GlyphWidth;
                    glyphTransform.Position.z -= GlyphZOffset;
                    Ecb.AddComponent(chunkIndex, glyph,
                        new DamageBubble.DamageBubble
                            { SpawnTime = ElapsedTime, OriginalY = glyphTransform.Position.y });

                    Ecb.AddComponent(chunkIndex, glyph, new GlyphIdFloatOverride { Value = digit });
                    Ecb.SetComponent(chunkIndex, glyph, new GlyphColorOverride { Color = color });
                }

                Ecb.DestroyEntity(chunkIndex, entity);
            }
        }
    }
}