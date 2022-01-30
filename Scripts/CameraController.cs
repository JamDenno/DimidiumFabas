using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera playerCam = null;
    private Transform playerCamera = null;
    [SerializeField] public float mouseSensitivity = 2.5f;

    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.025f;
    [SerializeField] float PlayerFieldOFView = 70f;

    [SerializeField] bool lockCursor = true;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    float cameraPitch = 0.0f;

    void Start()
    {
        Screen.SetResolution(1920,1080,true);
        playerCam.fieldOfView = PlayerFieldOFView;
        playerCamera = playerCam.transform;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 targetmouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));



        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetmouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        /*
        if (GetComponent<PlayerMovement>().Player_Gravity != 0)
        {
            playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        }
        else
        {
            transform.RotateAround(transform.right, -currentMouseDelta.y * Time.deltaTime * mouseSensitivity * 2);
            cameraPitch = 0;
        }
        */
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
}
