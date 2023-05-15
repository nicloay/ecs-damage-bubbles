using Unity.Entities;
using UnityEngine;

namespace EcsDamageBubbles.Demo
{
    public class TestSpawnerAuthoring : MonoBehaviour
    {
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public int entitiesPerFrame;
        
        public class TestSpawnerBaker : Baker<TestSpawnerAuthoring>
        {
            public override void Bake(TestSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new TestSpawner()
                {
                    MaxPosition = authoring.maxPosition,
                    MinPosition = authoring.minPosition,
                    EntitiesPerFrame = authoring.entitiesPerFrame
                });
            }
        }
    }
}