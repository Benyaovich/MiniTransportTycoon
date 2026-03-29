using System;
using UnityEngine;

namespace ScriptableObjects.Vehicles
{
    public abstract class VehicleSO : ScriptableObject
    {
        public Transform prefab;
        public string displayName;
        public float speed;
        public int price;
        public int maintenanceCost;
        public int maxCarryCapacity;
        public abstract Vehicle Create(Grid<ModelGridObject> grid);
        public abstract Type VehicleType { get; }
    }
}
