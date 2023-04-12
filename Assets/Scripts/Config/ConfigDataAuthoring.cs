using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Config
{
    public class ConfigDataAuthoring : MonoBehaviour
    {
        public GameObject damageBubble;
        public class ConfigDataBaker : Baker<ConfigDataAuthoring>
        {
            public override void Bake(ConfigDataAuthoring authoring)
            {
                AddComponent(new ConfigData { DamageBubble = GetEntity(authoring.damageBubble) });
            }
        }
    }
}