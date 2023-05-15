using Unity.Entities;
using Unity.Mathematics;

namespace EcsDamageBubbles.Config
{
    public struct DamageBubbleColorConfig : IBufferElementData
    {
        public float4 Color;
    }
}