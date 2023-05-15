using Unity.Entities;
using Unity.Rendering;

namespace Config
{
    [MaterialProperty("_GlyphId")]
    internal struct GlyphIdFloatOverride : IComponentData
    {
        public float Value;
    }
}
