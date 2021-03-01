using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [Header("Sensitivity")]
    public float RotateSensitivity = 0.05f;
    public float ZoomSensitivity = 0.05f;

    private float _verticalRotationAngle;

    // For Panning
    private Vector3 screenOrigin;
    private Vector3 worldOrigin;
    private bool _originSet = false;

    // For Rotation
    [Header("Rotation Keys")]
    public KeyCode CWRotationKey = KeyCode.Q;
    public KeyCode CCWRotationKey = KeyCode.E;
    // public KeyCode VerticalRotationUpKey = KeyCode.W;
    // public KeyCode VerticalRotationDownKey = KeyCode.S;
    float _startCameraDist;

    void Start()
    {
        _startCameraDist = transform.localPosition.magnitude;
        _verticalRotationAngle = 45;
    }


    void Update() {
        Panning();
        Zoom();
        Rotation();
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

                transform.parent.position = worldOrigin - planeDelta;
            } else
            {
                _originSet = false;
            }
        } else
        {
            _originSet = false;
        }
    }

    public void Zoom()
    {
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * ZoomSensitivity;
    }

    public void Rotation(){
        Vector3 curDir = transform.localPosition.normalized * _startCameraDist;
        curDir.y = 9.15f;
        transform.localPosition = curDir;


        if (Input.GetKey(CWRotationKey))
        {
            transform.position -= transform.right * RotateSensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(CCWRotationKey))
        {
            transform.position += transform.right * RotateSensitivity * Time.deltaTime;
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