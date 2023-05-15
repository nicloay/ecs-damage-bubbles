using Unity.Entities;
using Unity.Mathematics;

namespace EcsDamageBubbles.Demo
{
    public struct TestSpawner : IComponentData
    {
        public float3 MinPosition;
        public float3 MaxPosition;
        public int EntitiesPerFrame;
    }
}