using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject UserBuildings;
    public GameObject HitchhikerManager;
    private List<Vector3> _allChildrenTransformsPositions;

    public Button PlayButton;
    public Button ResetButton;
    public Sprite playButtonPlay;
    public Sprite playButtonPause;
    public Sprite resetButtonReset;
    public Sprite resetButtonRewind;

    public GameObject Spawn;
    private SpawnPlayers _spawnscript;

    public enum State{Plan,Play,Paused}
    public State CurrState;

    public GameObject BuildingPanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrState = State.Plan;
        _allChildrenTransformsPositions = new List<Vector3>();
        _spawnscript = Spawn.GetComponent<SpawnPlayers>();
        // _resetButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayButtonPressed() {
        switch(CurrState){
        case State.Plan:
            CurrState = State.Play;
            // _resetButton.interactable = true;

            // SAVING USERBUILDING TRANSFORMS
            _allChildrenTransformsPositions.Clear();
            foreach (Transform child in UserBuildings.transform){
                _allChildrenTransformsPositions.Add(child.localPosition);
            }
             
            PlayButton.GetComponent<Image>().sprite = playButtonPause;
            ResetButton.GetComponent<Image>().sprite = resetButtonRewind;
            BuildingPanel.SetActive(false);
            
            StartCoroutine(_spawnscript.AddPlayers());

            break;
        case State.Play:
            CurrState = State.Paused;
                PlayButton.GetComponent<Image>().sprite = playButtonPlay;
                Time.timeScale = 0f;
            break;
        case State.Paused:
            CurrState = State.Play;
                PlayButton.GetComponent<Image>().sprite = playButtonPause;
                Time.timeScale = 1.0f;
            break;
        }
    }

    public void ResetButtonPressed(){
        Time.timeScale = 1.0f;
        switch (CurrState){
        case State.Plan:
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            break;
        default:
            CurrState = State.Plan;
            int i = 0;
            foreach (Transform child in UserBuildings.transform){
                child.localPosition = _allChildrenTransformsPositions[i];
                i++;
            }
            foreach (Transform child in HitchhikerManager.transform){
                Destroy(child.gameObject);
            }
            StopAllCoroutines();
            PlayButton.GetComponent<Image>().sprite = playButtonPlay;
            ResetButton.GetComponent<Image>().sprite = resetButtonReset;
            BuildingPanel.SetActive(true);
            GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().ResetPlayerResults();
            break;
        }
    }


    public void ResetButtonPressedTUTORIAL(){
        Time.timeScale = 1.0f;
        switch (CurrState){
        case State.Plan:
            break;
        default:
            CurrState = State.Plan;
            int i = 0;
            foreach (Transform child in UserBuildings.transform){
                child.localPosition = _allChildrenTransformsPositions[i];
                i++;
            }
            foreach (Transform child in HitchhikerManager.transform){
                Destroy(child.gameObject);
            }
            StopAllCoroutines();
            PlayButton.GetComponent<Image>().sprite = playButtonPlay;
            break;
        }
    }
}
