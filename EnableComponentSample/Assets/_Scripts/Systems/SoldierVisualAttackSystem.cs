using EnableComponents.Components;
using Unity.Entities;

namespace EnableComponents.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(SoldierEntityAttackSystem))]
    public partial class SoldierVisualAttackSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (soldierTag, attackTag, entity) in SystemAPI.Query<RefRO<SoldierTag>, RefRO<AttackTag>>().WithEntityAccess())
            {
                if (SystemAPI.ManagedAPI.HasComponent<SoldierVisualReferenceData>(entity))
                {
                    var referenceData = SystemAPI.ManagedAPI.GetComponent<SoldierVisualReferenceData>(entity);
                    
                    referenceData.Reference.IsAttacking(true);
                }
            }
        }
    }
}