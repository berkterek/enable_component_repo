using EnableComponents.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EnableComponents.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(SoldierEntitySearchTargetSystem))]
    public partial struct SoldierEntityMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            var deltaTime = SystemAPI.Time.DeltaTime;
            var job = new SoldierEntityMoveJob()
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
    [WithDisabled(typeof(SearchTargetTag))]
    [WithDisabled(typeof(IdleTag))]
    public partial struct SoldierEntityMoveJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(Entity entity, in MoveData moveData, in MoveTag moveTag, ref LocalTransform localTransform, in SoldierTargetData soldierTargetData, [ChunkIndexInQuery]int sortKey)
        {
            float3 direction = math.normalize(soldierTargetData.TargetPosition - localTransform.Position);

            localTransform.Position += moveData.Speed * DeltaTime * direction;

            if (math.distance(localTransform.Position, soldierTargetData.TargetPosition) > 0.3f) return;
            
            Ecb.SetComponentEnabled<MoveTag>(sortKey,entity, false);
            Ecb.SetComponentEnabled<AttackTag>(sortKey,entity, true);
        }
    }
}