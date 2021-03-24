using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatinGraphicController : MonoBehaviour
{
    public static FloatinGraphicController Instance;
    public int curSceneNumber;
    public List<GameObject> SceneObjects;

    public Light MainLight;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        curSceneNumber = 0;
        ShowScene(curSceneNumber);
    }

    public void ShowScene(int sceneNumber)
    {
        SceneObjects[curSceneNumber].SetActive(false);

        switch (sceneNumber)
        {
            case 0:
                MainLight.intensity = 1.5f;
                break;
            case 1:
                MainLight.intensity = 0.5f;
                break;
            case 2:
                MainLight.intensity = 1.5f;
                break;
            case 3:
                MainLight.intensity = 1.5f;
                break;
            case 4:
                MainLight.intensity = 1.5f;
                break;
            default:
                Debug.Log("This scene doesn't exist.");
                break;
        }

        if (sceneNumber < 0 || sceneNumber >= SceneObjects.Count)
        {
            Debug.Log("Scene doesn't exist");
            return;
        }
        SceneObjects[sceneNumber].SetActive(true);
        curSceneNumber = sceneNumber;
    }
}
