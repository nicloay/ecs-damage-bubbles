using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Config
{
    [MaterialProperty("_GlyphColor")]
    internal struct GlyphColorOverride : IComponentData
    {
        public float4 Color;
    }
}