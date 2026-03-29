#nullable enable
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Interfaces;
using ScriptableObjects.Vehicles;
using UnityEngine;
using View;

namespace Controller.Vehicles
{
    public class VehicleVisualService
    {
        private Dictionary<Vehicle, Transform> _vehiclesVisuals = new();
        private IVehicleStorage _vehicleStorage;
        private Transform _parentTransform;
        private List<VehicleSO> _vehicleSos;

        public VehicleVisualService(IVehicleStorage vehicleStorage, Transform transform, List<VehicleSO> vehicleSos)
        {
            _vehicleStorage = vehicleStorage;
            _parentTransform = transform;
            _vehicleSos = vehicleSos;
            
            _vehicleStorage.OnVehicleAdd += VehicleStorageOnVehicleAdd;
            _vehicleStorage.OnVehicleRemove += VehicleStorageOnVehicleRemove;
        }

        private void VehicleStorageOnVehicleRemove(object sender, Vehicle vehicle)
        {
            UnityEngine.Object.Destroy(_vehiclesVisuals[vehicle].gameObject);
            _vehiclesVisuals.Remove(vehicle);
        }

        private void VehicleStorageOnVehicleAdd(object sender, Vehicle vehicle)
        {
            Transform transform = UnityEngine.Object.Instantiate(FindVehicleSo(vehicle).prefab,
                new Vector3(-10,0,-10),
                Quaternion.identity, _parentTransform);
            _vehiclesVisuals.TryAdd(vehicle, transform);
            LinkVisualToModel(vehicle, transform);
        }

        private void LinkVisualToModel(Vehicle vehicle, Transform transform)
        {
           transform.TryGetComponent(out VehicleVisual vehicleVisual);
           vehicleVisual.Setup(vehicle);
        }


        private VehicleSO FindVehicleSo(Vehicle e)
        {
            VehicleSO? vehicleSo = _vehicleSos.Find(x => x.VehicleType == e.GetType());
            if (vehicleSo == null){ throw new NullReferenceException("Nincs ilyen VehicleSO létrehozva."); }

            return vehicleSo;
        }
    }
}