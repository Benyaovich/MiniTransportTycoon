using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 70;
    [SerializeField] private float zoomSpeed = 10;
    [SerializeField] private float maxFOV = 90;
    [SerializeField] private float minFOV = 40;
    
    [SerializeField] private CinemachineCamera cinemachineCamera;
    
    private void Update()
    {
        HandleMovement();
        HandleCameraZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 inputDir = new Vector3(inputVector.x, 0, inputVector.y);

        Transform t = transform;
        Vector3 moveDir = t.forward * inputDir.z + t.right * inputDir.x;

        float finalMoveSpeed = moveSpeed * cinemachineCamera.Lens.FieldOfView / maxFOV;
        
        t.position += moveDir * (finalMoveSpeed * Time.deltaTime);
    }

    private void HandleCameraZoom()
    {
        float scrollAxis = GameInput.Instance.GetScrollAxis();
        float newSize = cinemachineCamera.Lens.FieldOfView - scrollAxis * zoomSpeed;
        
        if (newSize >= maxFOV)
        {
            newSize = maxFOV;
        }
        else if (newSize <= minFOV)
        {
            newSize = minFOV;
        }
        
        cinemachineCamera.Lens.FieldOfView = newSize;
    }

    
    
    
    
}
