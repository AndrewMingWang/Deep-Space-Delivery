using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;

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
    private bool _originSet = false;

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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

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
        if (BuildManager.BuildingSelected == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenOrigin = Input.mousePosition;
                worldOrigin = transform.parent.position;
                _originSet = true;
            }
            else if (Input.GetMouseButton(0) && _originSet)
            {
                Vector3 worldDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(screenOrigin);
                Vector3 planeDelta = Vector3.ProjectOnPlane(worldDelta, Vector3.up);
                Vector3 newPos = worldOrigin - planeDelta;

                // Bound new position
                newPos.x = Mathf.Clamp(newPos.x, XBounds.x, XBounds.y);
                newPos.z = Mathf.Clamp(newPos.z, ZBounds.x, ZBounds.y);

                transform.parent.position = newPos;
            }
            else
            {
                _originSet = false;
            }
        }
        else
        {
            _originSet = false;
        }
    }

    public void SetPanning(bool panningEnabled)
    {
        allowPan = panningEnabled;
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