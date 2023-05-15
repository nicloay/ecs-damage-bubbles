using Unity.Entities;
using Unity.Rendering;

namespace EcsDamageBubbles.Config
{
    [MaterialProperty("_GlyphId")]
    internal struct GlyphIdFloatOverride : IComponentData
    {
        public float Value;
    }
}