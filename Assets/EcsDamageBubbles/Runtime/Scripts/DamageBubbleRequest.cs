using Unity.Entities;

namespace EcsDamageBubbles
{
    public struct DamageBubbleRequest : IComponentData
    {
        public int Value;
        public int ColorId;
    }
}