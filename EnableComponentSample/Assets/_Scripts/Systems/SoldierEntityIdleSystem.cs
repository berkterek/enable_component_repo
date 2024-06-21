using EnableComponents.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace EnableComponents.Systems
{
    [BurstCompile]
    public partial struct SoldierEntityIdleSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

            var job = new SoldierEntityIdleJob()
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
    [WithDisabled(typeof(AttackTag))]
    [WithDisabled(typeof(MoveTag))]
    [WithDisabled(typeof(SearchTargetTag))]
    public partial struct SoldierEntityIdleJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter Ecb;
        
        [BurstCompile]
        private void Execute(Entity entity, in SoldierTag soldierTag, in IdleTag idleTag, ref TimerData timerData, [ChunkIndexInQuery]int sortKey)
        {
            timerData.Timer += DeltaTime;

            if (timerData.Timer < 3f) return;
            timerData.Timer = 0f;
            
            Ecb.SetComponentEnabled<IdleTag>(sortKey,entity, false);
            Ecb.SetComponentEnabled<SearchTargetTag>(sortKey,entity, true);
        }
    }
}