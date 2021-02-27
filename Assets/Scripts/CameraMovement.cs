using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float RotateSensitivity = 0.05f;
    public float ZoomSensitivity = 0.05f;

    // For Panning
    private Vector3 screenOrigin;
    private Vector3 worldOrigin;

    // For Rotation
    private float _rotateDirection;
    float dist;
    Vector3 cameraPos;
    Vector3 parentPos;

    void Start()
    {
        dist = transform.localPosition.magnitude;
        cameraPos = transform.position;
        parentPos = transform.parent.position;
        _rotateDirection = 1f;
    }


    void Update() {
        Panning();
        Zoom();
        Rotation();
    }


    public void Panning()
    {
        if (Input.GetMouseButtonDown(0))
        {
            screenOrigin = Input.mousePosition;
            worldOrigin = transform.parent.position;
        } else if (Input.GetMouseButton(0))
        {
            Vector3 worldDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(screenOrigin);
            Vector3 planeDelta = Vector3.ProjectOnPlane(worldDelta, Vector3.up);

            transform.parent.position = worldOrigin - planeDelta;
        }
    }

    public void Zoom()
    {
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * ZoomSensitivity;
    }

    public void Rotation(){
        
        Vector3 curDir = transform.localPosition.normalized * dist;
        curDir.y = 9.15f;
        transform.localPosition = curDir;

        if (( Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && (BuildManager.BuildingSelected == false)) {
            if (Input.GetKey(KeyCode.Q)){
                _rotateDirection = 1f;
            } else {
                _rotateDirection = -1f;
            }

            cameraPos -= transform.right * _rotateDirection * RotateSensitivity * Time.deltaTime;
            RotateSensitivity *= 1.008f;

        }

        transform.LookAt(transform.parent.position);

        float y = transform.localRotation.eulerAngles.y;
        float z = transform.localRotation.eulerAngles.z;
        Quaternion curRot = Quaternion.Euler(45, y, z);

        transform.localRotation = curRot;

        //Debug.DrawLine(transform.position, transform.position + 10 * transform.right, Color.red, 10);
        //Debug.DrawLine(transform.parent.position, transform.position, Color.blue, 10); 
    }

}