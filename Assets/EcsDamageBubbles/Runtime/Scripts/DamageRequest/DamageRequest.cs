using Unity.Entities;
using Unity.Mathematics;

namespace Damage
{
    public struct DamageRequest : IComponentData
    {
        public int Value;
        public float4 Color;
    }
}