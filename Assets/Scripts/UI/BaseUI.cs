using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void GoToScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    protected void LoadLevel(int levelNumber)
    {
        //TODO: Change this from hard coded
        AudioManager.PlayMusic(AudioManager.MUSIC_WORLD1);
        GoToScene("Level" + levelNumber);
    }

}
