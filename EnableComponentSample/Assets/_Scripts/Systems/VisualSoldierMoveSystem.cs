using EnableComponents.Components;
using Unity.Entities;
using Unity.Transforms;

namespace EnableComponents.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class VisualSoldierMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var (soldierTag, soldierVisualReferenceData,localTransform, entity) in SystemAPI
                         .Query<RefRO<SoldierTag>, SoldierVisualReferenceData, RefRO<LocalTransform>>().WithEntityAccess())
            {
                soldierVisualReferenceData.Reference.SetPosition(localTransform.ValueRO.Position);
            }
        }
    }
}