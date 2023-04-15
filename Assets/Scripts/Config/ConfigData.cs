using Unity.Entities;

namespace Config
{
    public struct ConfigData : IComponentData
    {
        public Entity GlyphPrefab;
        public float VerticalOffset;
        public float MovementTime;
        public float ScaleOffset;
        public float GlyphZOffset;
        public float GlyphWidth;
    }
}