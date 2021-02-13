using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float Sensitivity = 0.05f;
    public float RotationDuration = 0.2f;
    private float _rotationElapsed;

    Vector3 cameraPos;

    public GameObject SceneObjects;
    
    private bool _inRotation;
    
    private Quaternion _rotateFrom;
    private GameObject _rotateToObj;
    private Quaternion _rotateTo;
 

    void Start()
    {
        cameraPos = transform.position;
        _inRotation = false;
        _rotationElapsed = 0f;
    }


    void Update() {
        Panning();
        Zoom();
        Rotation();
    }


    public void Panning()
    {
        cameraPos -= transform.right * (Input.GetAxis("Horizontal") * -1) * Sensitivity;
        cameraPos -= transform.up * (Input.GetAxis("Vertical") * -1) * Sensitivity;
        transform.position = cameraPos;
    }

    public void Zoom()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera.main.orthographicSize = 3;
            Sensitivity = 0.02f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera.main.orthographicSize = 5;
            Sensitivity = 0.05f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Camera.main.orthographicSize = 8;
            Sensitivity = 0.05f;
        }
    }

    public void Rotation()
    {
        if (_inRotation == false)
        {
            if (( Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && (BuildManager.BuildingSelected == false)) {
                _rotateFrom = SceneObjects.transform.rotation;
                _rotateToObj = new GameObject();
                _rotateToObj.transform.position = new Vector3(SceneObjects.transform.position.x, SceneObjects.transform.position.y, SceneObjects.transform.position.z);
                _rotateToObj.transform.rotation = new Quaternion(_rotateFrom.x, _rotateFrom.y, _rotateFrom.z, _rotateFrom.w);
                if (Input.GetKeyDown(KeyCode.Q)){
                    _rotateToObj.transform.Rotate(0,-22.5f, 0);
                } else {
                    _rotateToObj.transform.Rotate(0,22.5f, 0);
                }
                _rotateTo = _rotateToObj.transform.rotation;

                _inRotation = true;

                SceneObjects.transform.rotation = Quaternion.Slerp(_rotateFrom, _rotateTo, _rotationElapsed / RotationDuration );
                _rotationElapsed += Time.deltaTime;
            }   
        } 
        else { //inRotation == True
            SceneObjects.transform.rotation = Quaternion.Slerp(_rotateFrom, _rotateTo, _rotationElapsed / RotationDuration );
            _rotationElapsed += Time.deltaTime;

            if (_rotationElapsed >= RotationDuration){
                _inRotation = false;
                Destroy(_rotateToObj);
                _rotationElapsed = 0.0f;
            }
            
            
        }

    }

}