using EnableComponents.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace EnableComponents.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial struct SoldierEntityAttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            var deltaTime = SystemAPI.Time.DeltaTime;

            var job = new SoldierEntityAttackJob()
            {
                Ecb = entityCommandBuffer.AsParallelWriter(),
                DeltaTime = deltaTime
            };

            var jobHandle = job.ScheduleParallel(state.Dependency);
            state.Dependency = jobHandle;
            jobHandle.Complete();
            
            entityCommandBuffer.Playback(state.EntityManager);
            entityCommandBuffer.Dispose();
        }
    }

    [BurstCompile]
    [WithDisabled(typeof(MoveTag))]
    [WithDisabled(typeof(SearchTargetTag))]
    [WithDisabled(typeof(IdleTag))]
    public partial struct SoldierEntityAttackJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(Entity entity, in AttackTag attackTag, ref TimerData timerData, [ChunkIndexInQuery] int sortKey)
        {
            timerData.Timer += DeltaTime;

            if (timerData.Timer < 6f) return;
            timerData.Timer = 0f;
            
            Ecb.SetComponentEnabled<AttackTag>(sortKey, entity, false);
            Ecb.SetComponentEnabled<IdleTag>(sortKey, entity, true);
        }
    }
}