using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 0.05f;
    Vector3 cameraPos;

    void Start()
    {
        cameraPos = transform.position;
    }


    void Update() {
        Panning();
        Zoom();
    }


    public void Panning()
    {
        cameraPos -= transform.right * (Input.GetAxis("Horizontal") * -1) * sensitivity;
        cameraPos -= transform.up * (Input.GetAxis("Vertical") * -1) * sensitivity;
        transform.position = cameraPos;
    }

    public void Zoom()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera.main.orthographicSize = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera.main.orthographicSize = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Camera.main.orthographicSize = 7;
        }
    }
}