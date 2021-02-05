using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject UserBuildings;
    public GameObject HitchhikerManager;
    private List<Vector3> _allChildrenTransformsPositions;

    public GameObject PlayButtonObj;
    public GameObject ResetButtonObj;
    private Button _playButton;
    private Button _resetButton;

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
        _playButton = PlayButtonObj.GetComponent<Button>();
        _resetButton = ResetButtonObj.GetComponent<Button>();
        _playButton.onClick.AddListener(PlayButtonPressed);
        _resetButton.onClick.AddListener(ResetButtonPressed);
        _allChildrenTransformsPositions = new List<Vector3>();
        _spawnscript = Spawn.GetComponent<SpawnPlayers>();
        // _resetButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayButtonPressed(){
        switch(CurrState){
        case State.Plan:
            CurrState = State.Play;
            // _resetButton.interactable = true;

            // SAVING USERBUILDING TRANSFORMS
            _allChildrenTransformsPositions.Clear();
            foreach (Transform child in UserBuildings.transform){
                _allChildrenTransformsPositions.Add(child.position);
            }
             
            PlayButtonObj.GetComponentInChildren<Text>().text = "Pause";
            ResetButtonObj.GetComponentInChildren<Text>().text = "Restart";
            BuildingPanel.SetActive(false);
            
            StartCoroutine(_spawnscript.AddPlayers());

            break;
        case State.Play:
            CurrState = State.Paused;
            PlayButtonObj.GetComponentInChildren<Text>().text = "Play";
            Time.timeScale = 0f;
            break;
        case State.Paused:
            CurrState = State.Play;
            PlayButtonObj.GetComponentInChildren<Text>().text = "Pause";
            Time.timeScale = 1.0f;
            break;
        }
    }

    void ResetButtonPressed(){
        switch (CurrState){
        case State.Plan:
            GameObject.FindGameObjectWithTag("moneyManager").GetComponent<MoneyManager>().ResetMoney();
            foreach (Transform child in UserBuildings.transform){
                Destroy(child.gameObject);
            }
            break;
        default:
            Time.timeScale = 1.0f;
            CurrState = State.Plan;
            int i = 0;
            foreach (Transform child in UserBuildings.transform){
                child.position = _allChildrenTransformsPositions[i];
                i++;
            }
            foreach (Transform child in HitchhikerManager.transform){
                Destroy(child.gameObject);
            }
            StopAllCoroutines();
            PlayButtonObj.GetComponentInChildren<Text>().text = "Play";
            ResetButtonObj.GetComponentInChildren<Text>().text = "Reset";
            BuildingPanel.SetActive(true);

            break;
        }
    }
}