using Unity.Entities;
using UnityEngine;

namespace Config
{
    public class ConfigDataAuthoring : MonoBehaviour
    {
        public GameObject glyphPrefab;
        public class ConfigDataBaker : Baker<ConfigDataAuthoring>
        {
            public override void Bake(ConfigDataAuthoring authoring)
            {
                AddComponent(new ConfigData { GlyphPrefab = GetEntity(authoring.glyphPrefab) });
            }
        }
    }
}