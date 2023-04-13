using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Rendering
{
    [MaterialProperty("_GlyphId")]
    struct GlyphIdFloatOverride : IComponentData
    {
        public float Value;
    }
}
