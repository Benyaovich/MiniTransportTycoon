using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 70;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] private float maxCameraSize = 50;
    [SerializeField] private float minCameraSize = 30;

    

    

    private void Update()
    {
        HandleMovement();
        HandleCameraZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,inputVector.y, 0f);

        float moveSpeedBasedOnCameraZoom = mainCamera.orthographicSize / maxCameraSize * moveSpeed;
        transform.position += moveDir * (moveSpeedBasedOnCameraZoom * Time.deltaTime);
    }
    
    private void HandleCameraZoom()
    {
        float scrollAxis = GameInput.Instance.GetScrollAxis();
        float newSize = mainCamera.orthographicSize - scrollAxis * zoomSpeed;
        
        if (newSize >= maxCameraSize)
        {
            mainCamera.orthographicSize = maxCameraSize;
        }
        else if (newSize <= minCameraSize)
        {
            mainCamera.orthographicSize = minCameraSize;
        }
        else
        {
            mainCamera.orthographicSize = newSize;
        }
    }
    
    
}
