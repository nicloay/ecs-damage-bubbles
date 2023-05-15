using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EcsDamageBubbles.Config
{
    public class DamageBubblesConfigAuthoring : MonoBehaviour
    {
        public GameObject glyphPrefab;
        public float verticalOffset = 2f;
        public float movementTime = 2f;
        public float scaleOffset = 1f;
        public float glyphZOffset = 0.001f;
        public float glyphWidth = 0.07f;
        public Color[] damageColors;

        public class ConfigDataBaker : Baker<DamageBubblesConfigAuthoring>
        {
            public override void Bake(DamageBubblesConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new DamageBubblesConfig
                {
                    GlyphPrefab = GetEntity(authoring.glyphPrefab, TransformUsageFlags.None),
                    ScaleOffset = authoring.scaleOffset,
                    VerticalOffset = authoring.verticalOffset,
                    MovementTime = authoring.movementTime,
                    GlyphZOffset = authoring.glyphZOffset,
                    GlyphWidth = authoring.glyphWidth
                });

                var buffer = AddBuffer<DamageBubbleColorConfig>(entity);
                foreach (var managedColor in authoring.damageColors)
                {
                    var color = new float4(managedColor.r, managedColor.g, managedColor.b, managedColor.a);
                    buffer.Add(new DamageBubbleColorConfig { Color = color });
                }
            }
        }
    }
}