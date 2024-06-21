using Unity.Entities;
using Unity.Mathematics;

namespace EnableComponents.Components
{
    public struct RandomData : IComponentData
    {
        public Random Random;
    }
}