using Unity.Entities;

namespace DamageInfo
{
    public struct DamageBubble : IComponentData
    {
        public float SpawnTime;
        public float OriginalY;
    }
}