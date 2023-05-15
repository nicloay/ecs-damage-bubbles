using Unity.Entities;
using Unity.Mathematics;

namespace TestSpawner
{
    public struct TestSpawner : IComponentData
    {
        public float3 MinPosition;
        public float3 MaxPosition;
        public int EntitiesPerFrame;
    }
}