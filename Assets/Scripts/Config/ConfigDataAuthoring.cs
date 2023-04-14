using Unity.Entities;
using UnityEngine;

namespace Config
{
    public class ConfigDataAuthoring : MonoBehaviour
    {
        public GameObject glyphPrefab;
        public float verticalOffset = 2f;
        public float movementTime = 2f;
        public float scaleOffset = 1f;
        public float glyphZOffset = 0.001f;
        public float glyphWidth = 0.07f;

        public class ConfigDataBaker : Baker<ConfigDataAuthoring>
        {
            public override void Bake(ConfigDataAuthoring authoring)
            {
                AddComponent(new ConfigData
                {
                    GlyphPrefab = GetEntity(authoring.glyphPrefab),
                    ScaleOffset = authoring.scaleOffset,
                    VerticalOffset = authoring.verticalOffset,
                    MovementTime = authoring.movementTime,
                    GlyphZOffset = authoring.glyphZOffset,
                    GlyphWidth = authoring.glyphWidth
                });
            }
        }
    }
}