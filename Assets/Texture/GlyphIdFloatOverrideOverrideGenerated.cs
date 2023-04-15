using Unity.Entities;

namespace Unity.Rendering
{
    [MaterialProperty("_GlyphId")]
    internal struct GlyphIdFloatOverride : IComponentData
    {
        public float Value;
    }
}