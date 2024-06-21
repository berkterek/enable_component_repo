using Unity.Entities;
using Unity.Mathematics;

namespace EnableComponents.Components
{
    public struct SoldierTargetData : IComponentData
    {
        public float3 TargetPosition;
    }
}