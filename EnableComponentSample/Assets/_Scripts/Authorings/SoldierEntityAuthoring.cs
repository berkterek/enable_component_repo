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
                
                
            }
        }
    }
}