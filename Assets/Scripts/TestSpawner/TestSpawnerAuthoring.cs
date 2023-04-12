using Unity.Entities;
using UnityEngine;

namespace TestSpawner
{
    public class TestSpawnerAuthoring : MonoBehaviour
    {
        public class TestSpawnerBaker : Baker<TestSpawnerAuthoring>
        {
            public override void Bake(TestSpawnerAuthoring authoring)
            {
                AddComponent(new TestSpawner());
            }
        }
    }
}