using EnableComponents.Components;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace EnableComponents.Authroings
{
    public class SoldierEntityAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public float MoveSpeed;
        
        class SoldierEntityBaker : Baker<SoldierEntityAuthoring>
        {
            public override void Bake(SoldierEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<SoldierTag>(entity);
                AddComponent<MoveTag>(entity);
                AddComponent<IdleTag>(entity);
                AddComponent<AttackTag>(entity);
                AddComponent<SearchTargetTag>(entity);
                AddComponent<TimerData>(entity);
                AddComponent<SoldierTargetData>(entity);

                uint randomSeed = (uint)new System.Random().Next(0, int.MaxValue);
                AddComponent<RandomData>(entity, new()
                {
                    Random = Random.CreateFromIndex(randomSeed)
                });
                
                AddComponent<MoveData>(entity, new ()
                {
                    Speed = authoring.MoveSpeed
                });
                
                AddComponentObject(entity, new SoldierVisualObjectData()
                {
                    Object = authoring.Prefab
                });
            }
        }
    }
}