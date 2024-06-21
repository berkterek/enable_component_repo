using EnableComponents.Controllers;
using Unity.Entities;
using UnityEngine;

namespace EnableComponents.Components
{
    public class SoldierVisualObjectData : IComponentData
    {
        public GameObject Object;
    }

    public class SoldierVisualReferenceData : ICleanupComponentData
    {
        public SoldierVisualController Reference;
    }
}