using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseUI : MonoBehaviour
{
    protected const int MIN_WORLD = 1;
    protected const int MAX_WORLD = 3;
    protected const int LEVELS_PER_WORLD = 7;

    public virtual void Awake()
    {
        GameObject cursorPrefab = (GameObject)Resources.Load("Cursor");
        GameObject newCursor = Instantiate(cursorPrefab, Vector2.zero, Quaternion.identity);
        newCursor.transform.SetParent(GameObject.FindGameObjectWithTag("cursorCanvas").transform, false);
    }

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
        int worldNum = Mathf.CeilToInt((float) levelNumber / LEVELS_PER_WORLD);
        AudioManager.Instance.PlayWorldMusic(worldNum);

        GoToScene("Level" + levelNumber);
    }

    protected void LoadLevelIntro(int levelNumber)
    {
        int worldNum = Mathf.CeilToInt((float)levelNumber / LEVELS_PER_WORLD);
        AudioManager.Instance.PlayWorldMusic(worldNum);

        GoToScene("Level" + levelNumber + "intro");
    }

}
