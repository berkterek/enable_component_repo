using EnableComponents.Components;
using EnableComponents.Controllers;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace EnableComponents.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class CreateVisualSoldierSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (soldierTag, soldierVisualObjectData,entity) in SystemAPI.Query<RefRO<SoldierTag>,SoldierVisualObjectData>().WithEntityAccess())
            {
                var soldierObject = Object.Instantiate(soldierVisualObjectData.Object);
                var soldierVisualController = soldierObject.GetComponent<SoldierVisualController>();
                entityCommandBuffer.AddComponent<SoldierVisualReferenceData>(entity, new()
                {
                    Reference = soldierVisualController
                });
                
                entityCommandBuffer.RemoveComponent<SoldierVisualObjectData>(entity);
            }
            
            entityCommandBuffer.Playback(EntityManager);
            entityCommandBuffer.Dispose();
        }
    }
}