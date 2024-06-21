using EnableComponents.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace EnableComponents.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(SoldierEntityIdleSystem))]
    public partial class SoldierVisualIdleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (soldierTag, idleTag, entity) in SystemAPI.Query<RefRO<SoldierTag>, RefRO<IdleTag>>().WithEntityAccess())
            {
                if (SystemAPI.ManagedAPI.HasComponent<SoldierVisualReferenceData>(entity))
                {
                    var referenceData = SystemAPI.ManagedAPI.GetComponent<SoldierVisualReferenceData>(entity);
                    
                    referenceData.Reference.IsWalking(false);
                }
            }
        }
    }
}