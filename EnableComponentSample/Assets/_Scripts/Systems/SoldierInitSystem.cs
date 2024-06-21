using EnableComponents.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace EnableComponents.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateBefore(typeof(CreateVisualSoldierSystem))]
    public partial struct SoldierInitSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SoldierTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (soldierTag, idleTag, moveTag, searchTag,attackTag, entity) in SystemAPI.Query<RefRO<SoldierTag>, RefRO<IdleTag>, RefRO<MoveTag>, RefRO<SearchTargetTag>, RefRO<AttackTag>>().WithEntityAccess())
            {
                entityCommandBuffer.SetComponentEnabled<MoveTag>(entity, false);
                entityCommandBuffer.SetComponentEnabled<SearchTargetTag>(entity, false);
                entityCommandBuffer.SetComponentEnabled<AttackTag>(entity, false);
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
            entityCommandBuffer.Dispose();
        }
    }
}