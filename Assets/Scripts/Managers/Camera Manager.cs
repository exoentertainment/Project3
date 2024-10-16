using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    #region --Serialized Fields --
    
    [Header("Camera Target")]
    [SerializeField] private CinemachineCamera vcam;

    [Header("Variables")] 
    [SerializeField] LayerMask cameraLayerMask;
    [SerializeField] private float panSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;
    
    #endregion

    private bool isPanningLeft;
    bool isPanningRight;
    private bool isPanningUp;
    bool isPanningDown;
    private bool isRotatingLeft;
    bool isRotatingRight;
    private bool isZoomingIn;

    int scrollDirection;

    private GameObject currentFocus;
    
    public static CameraManager instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
    
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (isPanningLeft)
        {
            PanCameraLeft();
        }
        
        if (isPanningRight)
        {
            PanCameraRight();
        }

        if (isPanningUp)
        {
            PanCameraUp();
        }

        if (isPanningDown)
        {
            PanCameraDown();
        }

        if (isRotatingLeft)
        {
            RotateCameraLeft();
        }

        if (isRotatingRight)
        {
            RotateCameraRight();
        }

        if (scrollDirection != 0)
        {
            ZoomCamera();
        }
    }

    #region -- Movement Logic --
    
    void PanCameraLeft()
    {
        // Vector3 newPos = cameraTarget.position;
        // newPos.x += panSpeed * Time.deltaTime;
            
        // Vector3 newPos = vcam.transform.position;
        // newPos.x -= panSpeed * Time.deltaTime;
        //
        // vcam.transform.position = newPos;
        
        Vector3 direction = vcam.transform.right;
        direction.y = 0;
        direction.Normalize();
        vcam.transform.Translate(direction * (Time.deltaTime * -panSpeed), Space.World);
    }
    
    void PanCameraRight()
    {
        // Vector3 newPos = cameraTarget.position;
        // newPos.x -= panSpeed * Time.deltaTime;
            
        // Vector3 newPos = vcam.transform.position;
        // newPos.x += panSpeed * Time.deltaTime;
        //
        // vcam.transform.position = newPos;
        
        Vector3 direction = vcam.transform.right;
        direction.y = 0;
        direction.Normalize();
        vcam.transform.Translate(direction * (Time.deltaTime * panSpeed), Space.World);
    }
    
    void PanCameraDown()
    {
        // Vector3 newPos = cameraTarget.position;
        // newPos.z += panSpeed * Time.deltaTime;
        //     
        // cameraTarget.position = newPos;
        
        // Vector3 newPos = vcam.transform.position;
        // newPos.z -= panSpeed * Time.deltaTime;
        // //vcam.transform.Translate(newPos);
        //
        // vcam.transform.position = newPos;
        
        Vector3 direction = vcam.transform.forward;
        direction.y = 0;
        direction.Normalize();
        vcam.transform.Translate(direction * (Time.deltaTime * -panSpeed), Space.World);
    }
    
    void PanCameraUp()
    {
        // Vector3 newPos = cameraTarget.position;
        // newPos.z -= panSpeed * Time.deltaTime;
        //     
        // cameraTarget.position = newPos;
        
        // Vector3 newPos = vcam.transform.position;
        // newPos.z += panSpeed * Time.deltaTime;
        //
        // vcam.transform.position = newPos;
        
        // Vector3 oldRotation = vcam.transform.eulerAngles;
        // vcam.transform.rotation = Quaternion.Euler(0, 0, 0);
        // vcam.transform.position += vcam.transform.forward * (zoomSpeed * Time.deltaTime);
        // vcam.transform.rotation = Quaternion.Euler(oldRotation.x, oldRotation.y, oldRotation.z);
        
        Vector3 direction = vcam.transform.forward;
        direction.y = 0;
        direction.Normalize();
        vcam.transform.Translate(direction * (Time.deltaTime * panSpeed), Space.World);
    }

    void RotateCameraLeft()
    {
        // float newAngle = cameraTarget.eulerAngles.y + (rotateSpeed * Time.deltaTime);
        //
        // cameraTarget.rotation = Quaternion.Euler(0, newAngle, 0);
        
        float newAngle = vcam.transform.eulerAngles.y + (rotateSpeed * Time.deltaTime);
        
        vcam.transform.rotation = Quaternion.Euler(vcam.transform.rotation.eulerAngles.x, newAngle, 0);
    }

    void RotateCameraRight()
    {
        // float newAngle = cameraTarget.eulerAngles.y - (rotateSpeed * Time.deltaTime);
        //
        // cameraTarget.rotation = Quaternion.Euler(0, newAngle, 0);
        
        float newAngle = vcam.transform.eulerAngles.y - (rotateSpeed * Time.deltaTime);
        
        vcam.transform.rotation = Quaternion.Euler(vcam.transform.rotation.eulerAngles.x, newAngle, 0);
    }

    void ZoomCamera()
    {
        if (scrollDirection > 0 && vcam.Lens.FieldOfView > zoomMin)
        {
            vcam.Lens.FieldOfView -= zoomSpeed * Time.deltaTime;
        }
        if (scrollDirection < 0 && vcam.Lens.FieldOfView < zoomMax)
        {
            vcam.Lens.FieldOfView += zoomSpeed * Time.deltaTime;
        }
    }

    void MoveCameraToFocus(RaycastHit hit)
    {
        Vector3 camOffset = new Vector3(0, 2000, -2000);
        vcam.transform.position = hit.collider.transform.position + camOffset;
    }
    
    #endregion
    
    #region --Input Listeners --
    
    public void PanLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningLeft = true;
            RemoveTrackingObject();
        }
        else if (context.canceled)
        {
            isPanningLeft = false;
        }
    }
    
    public void PanRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningRight = true;
            RemoveTrackingObject();
        }
        else if (context.canceled)
        {
            isPanningRight = false;
        }
    }
    
    public void PanUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningUp = true;
            RemoveTrackingObject();
        }
        else if (context.canceled)
        {
            isPanningUp = false;
        }
    }
    
    public void PanDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningDown = true;
            RemoveTrackingObject();
        }
        else if (context.canceled)
        {
            isPanningDown = false;
        }
    }
    
    public void RotateLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotatingLeft = true;
        }
        else if (context.canceled)
        {
            isRotatingLeft = false;
        }
    }
    
    public void RotateRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotatingRight = true;
        }
        else if (context.canceled)
        {
            isRotatingRight = false;
        }
    }
    
    public void ZoomIn(InputAction.CallbackContext context)
    {
        float scrollY = context.ReadValue<float>();

        if (scrollY > 0)
        {
            scrollDirection = 1;
        }    
        else if (scrollY < 0)
        {
            scrollDirection = -1;
        }
        else if (scrollY == 0)
        {
            scrollDirection = 0;
        }
    }

    public void FocusTarget(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cameraLayerMask))
            {
                if (hit.collider != null)
                {
                    if (currentFocus != hit.collider.gameObject)
                    {
                        MoveCameraToFocus(hit);
                        currentFocus = hit.collider.gameObject;
                        vcam.transform.position = hit.collider.transform.position;
                        vcam.Follow = hit.collider.transform;
                    }
                }
            }
        }
    }
    
    #endregion

    public bool IsObjectInView(Transform target)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(target.position);
        
        return (viewPos.x >= 0 && viewPos.y >= 0) && (viewPos.x <= 1 && viewPos.y <= 1) && viewPos.z >= 0;
    }

    void RemoveTrackingObject()
    {
        if (vcam.Follow != null)
        {
            vcam.Follow = null;
            currentFocus = null;
        }
    }
}
