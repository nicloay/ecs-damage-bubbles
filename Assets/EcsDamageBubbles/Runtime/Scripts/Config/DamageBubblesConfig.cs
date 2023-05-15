﻿using Unity.Entities;

namespace Config
{
    public struct DamageBubblesConfig : IComponentData
    {
        public Entity GlyphPrefab;
        public float VerticalOffset;
        public float MovementTime;
        public float ScaleOffset;
        public float GlyphZOffset;
        public float GlyphWidth;
    }
}