using Unity.Entities;

namespace EcsDamageBubbles
{
    public struct DamageRequest : IComponentData
    {
        public int Value;
        public int ColorId;
    }
}