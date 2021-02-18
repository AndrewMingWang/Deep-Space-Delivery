using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float VerticalPanSensitivity = 0.08f;
    public float HorizontalPanSensitivity = 0.05f;
    public float RotateSensitivity = 0.05f;

    Vector3 cameraPos;
    Vector3 parentPos;

 
    private float _rotateDirection;

    void Start()
    {
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
        parentPos -= transform.right * (Input.GetAxis("Horizontal") * -1) * HorizontalPanSensitivity;
        cameraPos -= transform.right * (Input.GetAxis("Horizontal") * -1) * HorizontalPanSensitivity;
        parentPos -= Vector3.ProjectOnPlane(transform.up * (Input.GetAxis("Vertical") * -1),Vector3.up) * VerticalPanSensitivity;
        cameraPos -= Vector3.ProjectOnPlane(transform.up * (Input.GetAxis("Vertical") * -1),Vector3.up) * VerticalPanSensitivity;
        transform.parent.position = parentPos;
        transform.position = cameraPos;
        // transform.LookAt(transform.parent.position);
    }

    public void Zoom()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera.main.orthographicSize = 3;
            HorizontalPanSensitivity = 0.03f;
            VerticalPanSensitivity = 0.045f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera.main.orthographicSize = 5;
            HorizontalPanSensitivity = 0.05f;
            VerticalPanSensitivity = 0.08f;

        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Camera.main.orthographicSize = 8;
            HorizontalPanSensitivity = 0.05f;
            VerticalPanSensitivity = 0.08f;
        }
    }

    // public void Rotation()
    // {

        // if (_inRotation == false)
        // {
        //     if (( Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && (BuildManager.BuildingSelected == false)) {
        //         _rotateFrom = SceneObjects.transform.rotation;
        //         _rotateToObj = new GameObject();
        //         _rotateToObj.transform.position = new Vector3(SceneObjects.transform.position.x, SceneObjects.transform.position.y, SceneObjects.transform.position.z);
        //         _rotateToObj.transform.rotation = new Quaternion(_rotateFrom.x, _rotateFrom.y, _rotateFrom.z, _rotateFrom.w);
        //         if (Input.GetKeyDown(KeyCode.Q)){
        //             _rotateToObj.transform.Rotate(0,-22.5f, 0);
        //         } else {
        //             _rotateToObj.transform.Rotate(0,22.5f, 0);
        //         }
        //         _rotateTo = _rotateToObj.transform.rotation;

        //         _inRotation = true;

        //         SceneObjects.transform.rotation = Quaternion.Slerp(_rotateFrom, _rotateTo, _rotationElapsed / RotationDuration );
        //         _rotationElapsed += Time.deltaTime;
        //     }   
        // } 
        // else { //inRotation == True
        //     SceneObjects.transform.rotation = Quaternion.Slerp(_rotateFrom, _rotateTo, _rotationElapsed / RotationDuration );
        //     _rotationElapsed += Time.deltaTime;

        //     if (_rotationElapsed >= RotationDuration){
        //         _inRotation = false;
        //         Destroy(_rotateToObj);
        //         _rotationElapsed = 0.0f;
        //     }
            
            
        // }

    // }

    public void Rotation(){
        
        if (( Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && (BuildManager.BuildingSelected == false)) {
                
            if (Input.GetKey(KeyCode.Q)){
                _rotateDirection = 1f;
            } else {
                _rotateDirection = -1f;
            }

            cameraPos -= transform.right * _rotateDirection * RotateSensitivity;

        }


        transform.LookAt(transform.parent.position);
    }

}