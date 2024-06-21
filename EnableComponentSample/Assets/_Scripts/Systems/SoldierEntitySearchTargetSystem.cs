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
    [UpdateAfter(typeof(SoldierEntityIdleSystem))]
    public partial struct SoldierEntitySearchTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var targetPositionList = new NativeList<float3>(Allocator.Temp);
            foreach (var (targetTag, localTransform) in SystemAPI.Query<RefRO<TargetTag>, RefRO<LocalTransform>>())
            {
                targetPositionList.Add(localTransform.ValueRO.Position);
            }

            var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            var job = new SoldierEntitySearchTargetJob()
            {
                TargetPositions = targetPositionList.ToArray(Allocator.TempJob),
                Ecb = entityCommandBuffer.AsParallelWriter()
            };
            targetPositionList.Dispose();

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
    [WithDisabled(typeof(IdleTag))]
    public partial struct SoldierEntitySearchTargetJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        [ReadOnly,DeallocateOnJobCompletion] public NativeArray<float3> TargetPositions;
        
        [BurstCompile]
        private void Execute(Entity entity, in LocalTransform localTransform, in SearchTargetTag searchTargetTag, ref SoldierTargetData soldierTargetData, ref RandomData randomData,[ChunkIndexInQuery]int sortKey)
        {
            float3 position = GetRandomPosition(ref randomData);
            while (math.distance(position, localTransform.Position) < 1f)
            {
                position = GetRandomPosition(ref randomData);
            }

            soldierTargetData.TargetPosition = position;
            
            Ecb.SetComponentEnabled<SearchTargetTag>(sortKey, entity, false);
            Ecb.SetComponentEnabled<MoveTag>(sortKey, entity, true);
        }

        private float3 GetRandomPosition(ref RandomData randomData)
        {
            int randomIndex = randomData.Random.NextInt(0, TargetPositions.Length);
            return TargetPositions[randomIndex];
        }
    }
}