using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 70;
    [SerializeField] private float zoomSpeed = 10;
    [SerializeField] private float maxFOV = 90;
    [SerializeField] private float minFOV = 40;
    [SerializeField] private float rotationSensitivity = 10;
    [SerializeField] private Vector2 mapEdgeX = new Vector2(0,500);
    [SerializeField] private Vector2 mapEdgeZ = new Vector2(0,500);
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private Vector2 previousMousePos = Vector2.zero;
    
    private void Update()
    {
        if (PlayerState.Instance.IsGameOver) return;
        HandleMovement();
        if (Utils.IsPointerOverBlockingUI()) return;
        HandleCameraZoom();
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (!GameInput.Instance.IsRightClickPressed)
        {
            previousMousePos = GameInput.Instance.GetMousePosition();
            return;
        }

        Vector2 mousePos = GameInput.Instance.GetMousePosition();
        float deltaX = mousePos.x - previousMousePos.x;

        float amountToRotate = deltaX * rotationSensitivity;
        transform.Rotate(Vector3.up, amountToRotate, Space.World);

        previousMousePos = mousePos;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 inputDir = new Vector3(inputVector.x, 0, inputVector.y);

        Transform t = transform;
        Vector3 moveDir = t.forward * inputDir.z + t.right * inputDir.x;

        float finalMoveSpeed = moveSpeed * cinemachineCamera.Lens.FieldOfView / maxFOV;
        
        Vector3 newPosition = t.position + moveDir * (finalMoveSpeed * Time.deltaTime);
        if (newPosition.x <= mapEdgeX.x || newPosition.x >= mapEdgeX.y ||
            newPosition.z <= mapEdgeZ.x || newPosition.z >= mapEdgeZ.y) return;
        t.position = newPosition;
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
