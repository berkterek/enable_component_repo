using EnableComponents.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace EnableComponents.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(SoldierEntityMoveSystem))]
    public partial class SoldierVisualMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (soldierTag, moveTag, entity) in SystemAPI.Query<RefRO<SoldierTag>, RefRO<MoveTag>>().WithEntityAccess())
            {
                if (SystemAPI.ManagedAPI.HasComponent<SoldierVisualReferenceData>(entity))
                {
                    var referenceData = SystemAPI.ManagedAPI.GetComponent<SoldierVisualReferenceData>(entity);
                    
                    referenceData.Reference.IsWalking(true);
                }
            }
        }
    }
}