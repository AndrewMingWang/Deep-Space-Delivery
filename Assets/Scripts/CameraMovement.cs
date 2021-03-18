using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [Header("Sensitivity")]
    public float RotateSensitivity = 0.05f;
    public float ZoomSensitivity = 0.05f;

    private float _verticalRotationAngle;

    // For Panning
    [Header("Panning")]
    public Vector2 XBounds;
    public Vector2 ZBounds;
    private Vector3 screenOrigin;
    private Vector3 worldOrigin;

    // For Zooming
    [Header("Zooming")]
    public Vector2 ZoomBounds;

    // For Rotation
    private KeyCode CWRotationKey = KeyCode.A;
    private KeyCode CCWRotationKey = KeyCode.D;
    // public KeyCode VerticalRotationUpKey = KeyCode.W;
    // public KeyCode VerticalRotationDownKey = KeyCode.S;
    float _startCameraDist;

    [Header("Permissions")]
    public bool allowPan = true;
    public bool allowZoom = true;
    public bool allowRotation = true;

    void Start()
    {
        _startCameraDist = transform.localPosition.magnitude;
        _verticalRotationAngle = 45;
    }


    void Update() {
        if (allowPan) Panning();
        if (allowZoom) Zoom();
        if (allowRotation) Rotation();
    }


    public void Panning()
    {
        Vector2 mouseLocation = Input.mousePosition;
        if (mouseLocation.x < 20.0f)
        {
            float HorizontalPanSensitivity = 0.2f;
            Vector3 parentPos = transform.parent.position;
            Vector3 cameraPos = transform.position;
            parentPos -= transform.right * 1.0f * HorizontalPanSensitivity;
            cameraPos -= transform.right * 1.0f * HorizontalPanSensitivity;
            transform.parent.position = parentPos;
            transform.position = cameraPos;
        }
        else if (mouseLocation.x > Screen.width - 20.0f)
        {
            float HorizontalPanSensitivity = 0.2f;
            Vector3 parentPos = transform.parent.position;
            Vector3 cameraPos = transform.position;
            parentPos -= transform.right * -1.0f * HorizontalPanSensitivity;
            cameraPos -= transform.right * -1.0f * HorizontalPanSensitivity;
            transform.parent.position = parentPos;
            transform.position = cameraPos;
        }
        else if (mouseLocation.y < 20.0f)
        {
            float VerticalPanSensitivity = 0.2f;
            Vector3 parentPos = transform.parent.position;
            Vector3 cameraPos = transform.position;
            parentPos -= transform.up * 1.0f * VerticalPanSensitivity;
            cameraPos -= transform.up * 1.0f * VerticalPanSensitivity;
            transform.parent.position = parentPos;
            transform.position = cameraPos;
        }
        else if (mouseLocation.y > Screen.height - 20.0f)
        {
            float VerticalPanSensitivity = 0.2f;
            Vector3 parentPos = transform.parent.position;
            Vector3 cameraPos = transform.position;
            parentPos -= transform.up * -1.0f * VerticalPanSensitivity;
            cameraPos -= transform.up * -1.0f * VerticalPanSensitivity;
            transform.parent.position = parentPos;
            transform.position = cameraPos;
        }
    }

    public void Zoom()
    {
        float newOrthographicSize = Mathf.Clamp(
            Camera.main.orthographicSize
            - Input.mouseScrollDelta.y * Time.unscaledDeltaTime * ZoomSensitivity,
            ZoomBounds.x,
            ZoomBounds.y
        );
        Camera.main.orthographicSize = newOrthographicSize;
    }

    public void Rotation(){
        Vector3 curDir = transform.localPosition.normalized * _startCameraDist;
        curDir.y = 9.15f;
        transform.localPosition = curDir;


        if (Input.GetKey(CWRotationKey))
        {
            transform.position -= transform.right * RotateSensitivity * Time.unscaledDeltaTime;
        }
        else if (Input.GetKey(CCWRotationKey))
        {
            transform.position += transform.right * RotateSensitivity * Time.unscaledDeltaTime;
        }

        // if (Input.GetKey(VerticalRotationUpKey) && _verticalRotationAngle <= 70)
        // {
        //     _verticalRotationAngle += 20 * Time.deltaTime;
        // }
        // else if (Input.GetKey(VerticalRotationDownKey) && _verticalRotationAngle >= 20)
        // {
        //     _verticalRotationAngle -= 20 * Time.deltaTime;
        // }

        transform.LookAt(transform.parent.position);

        float y = transform.localRotation.eulerAngles.y;
        float z = transform.localRotation.eulerAngles.z;
        Quaternion curRot = Quaternion.Euler(_verticalRotationAngle, y, z);

        transform.localRotation = curRot;

        //Debug.DrawLine(transform.position, transform.position + 10 * transform.right, Color.red, 10);
        //Debug.DrawLine(transform.parent.position, transform.position, Color.blue, 10); 
    }

}