using UnityEngine;
using System.Collections;

public class TutorialCameraMovement : MonoBehaviour
{
    public float VerticalPanSensitivity = 0.08f;
    public float HorizontalPanSensitivity = 0.05f;
    public float RotateSensitivity = 0.05f;
    public GameObject SpecialStateManager;
    public GameObject CameraParent;
    public GameObject Spawn;
    public GameObject HitchikerManager;

    Vector3 cameraPos;
    Vector3 parentPos;

    Vector3 startingPos;
    Vector3 startingRot;

    float dist;

    // private float _rotateDirection;

    void Start()
    {
        CameraParent = transform.parent.gameObject;
        dist = transform.localPosition.magnitude;
        cameraPos = transform.position;
        parentPos = transform.parent.position;

        startingPos = transform.position;
        // _rotateDirection = 1f;
    }


    void Update() {
        // Panning();
        // Zoom();
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

        // Vector3 curDir = transform.localPosition.normalized * dist;
        // curDir.y = 9.15f;
        // transform.localPosition = curDir;

       

        // if (( Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && (TutorialBuildManager.BuildingSelected == false)) {
                
        //     if (Input.GetKey(KeyCode.Q)){
        //         _rotateDirection = 1f;
        //     } else {
        //         _rotateDirection = -1f;
        //     }

        //     cameraPos -= transform.right * _rotateDirection * RotateSensitivity * Time.deltaTime;
        //     RotateSensitivity *= 1.008f;

        // }

        if (SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.zeroStart ||
            SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.zeroA ||
            SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.zeroB ||
            SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.zeroSB ||
            SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.oneS ||
            SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.oneU)
        {
            transform.parent = Spawn.transform;
        } else if (SpecialStateManager.GetComponent<TutorialStateMachine>().currState == TutorialStateMachine.State.zeroS)
        {
            foreach(Transform child in HitchikerManager.transform){
                transform.parent = child.transform;
            }
        } else {
            transform.position = startingPos;
            transform.parent = CameraParent.transform;  
        }
         

        float y = transform.localRotation.eulerAngles.y;
        float z = transform.localRotation.eulerAngles.z;
        Quaternion curRot = Quaternion.Euler(45, y, z);

        transform.localRotation = curRot;

        //Debug.DrawLine(transform.position, transform.position + 10 * transform.right, Color.red, 10);
        //Debug.DrawLine(transform.parent.position, transform.position, Color.blue, 10); 
    }

}