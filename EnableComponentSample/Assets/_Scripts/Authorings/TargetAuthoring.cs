using EnableComponents.Components;
using Unity.Entities;
using UnityEngine;

namespace EnableComponents.Authroings
{
    public class TargetAuthoring : MonoBehaviour
    {
        class TargetBaker : Baker<TargetAuthoring>
        {
            public override void Bake(TargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<TargetTag>(entity);
            }
        }
    }
}