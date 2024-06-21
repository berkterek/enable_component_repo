using EnableComponents.Components;
using Unity.Entities;
using UnityEngine;

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

                AddComponent<MoveTag>(entity);
                AddComponent<IdleTag>(entity);
                AddComponent<AttackTag>(entity);
                AddComponent<SoldierTag>(entity);
                
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