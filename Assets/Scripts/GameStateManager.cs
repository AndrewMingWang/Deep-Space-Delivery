using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject UserBuildings;
    public GameObject HitchhikerManager;
    public GameObject EnemyManager;
    private List<Vector3> _allChildrenTransformsPositions;

    public Image PlayButtonIcon;
    public Image ResetButtonIcon;
    public Sprite playButtonPlay;
    public Sprite playButtonPause;
    public Sprite resetButtonReset;
    public Sprite resetButtonRewind;

    public GameObject Spawn;
    public GameObject ResultsPanel;
    public GameObject ActionsPanel;
    public GameObject ControlsPanel;
    public GameObject MenuPanel;
    private SpawnPlayers _spawnscript;
    public TMP_Text ResultsText;

    public enum State{Plan,Play,Paused}
    public State CurrState;

    public GameObject BuildingPanel;
    public bool EnablePanelsOnReset = true;
    private Coroutine _spawner;

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
        ControlsPanel = GameObject.Find("Controls");
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
                foreach (Building building in BuildManager.Instance.ExistingBuildings)
                {
                    _allChildrenTransformsPositions.Add(building.transform.localPosition);
                }
             
                PlayButtonIcon.sprite = playButtonPause;
                ResetButtonIcon.sprite = resetButtonRewind;
                BuildManager.Instance.CancelBuilding();
                BuildingPanel.SetActive(false);

                _spawner = StartCoroutine(_spawnscript.AddPlayers());

                break;
            case State.Play:
                CurrState = State.Paused;
                AudioManager.PauseAllLoopingSFX();
                PlayButtonIcon.sprite = playButtonPlay;
                Time.timeScale = 0f;
                break;
            case State.Paused:
                CurrState = State.Play;
                AudioManager.UnpauseAllLoopingSFX();
                PlayButtonIcon.sprite = playButtonPause;
                Time.timeScale = 1.0f;
                break;
            }
        ResultsPanel.GetComponent<Animator>().SetBool("closed", false);
    }

    public void ResetButtonPressed()
    {
        Time.timeScale = 1.0f;
        switch (CurrState){
            case State.Plan:
                List<GameObject> cleanup = new List<GameObject>();
                foreach (Building building in BuildManager.Instance.ExistingBuildings)
                {
                    Transform child = building.transform;
                    string childName = child.gameObject.name.Split('(')[0].ToLower();
                    if (childName.Contains("wall"))
                    {
                        childName = "wall";
                    }
                    MoneyManager.Instance.RefundItem(childName);

                    cleanup.Add(building.gameObject);
                }

                // Cleanup memory
                BuildManager.Instance.ExistingBuildings.Clear();
                foreach (GameObject g in cleanup)
                {
                    Destroy(g);
                }

                PackagesSpawner.Instance.ResetAllPackages();
                ResultsText.text = "";
                break;
            default:
                StopCoroutine(_spawner);
                CurrState = State.Plan;
                int i = 0;
                foreach (Building building in BuildManager.Instance.ExistingBuildings)
                {
                    building.transform.localPosition = _allChildrenTransformsPositions[i];
                    building.gameObject.SetActive(true);
                    building.Reset();
                    i++;
                }


                foreach (Transform child in HitchhikerManager.transform){
                    // Destroy(child.gameObject);
                    child.gameObject.SetActive(false);
                }
                if (EnemyManager!= null){
                    foreach (Transform child in EnemyManager.transform){
                        child.GetComponentInChildren<EnemyAI>().resetState();
                    }
                } else {
                    Debug.Log("No EnemyManager GameObject attached to GameStateManager");
                }
                PlayButtonIcon.sprite = playButtonPlay;
                ResetButtonIcon.sprite = resetButtonReset;
                GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().ResetPlayerResults();
                PackagesSpawner.Instance.ResetAllPackages();
                ResultsPanelTypeEffect.Instance.Reset();
                ResultsText.text = "";
                break;
        }
        ResultsPanel.GetComponent<Animator>().SetBool("open", false);
        ResultsPanel.GetComponent<Animator>().SetBool("closed", true);        
        // ResultsPanel.GetComponent<CanvasGroup>().interactable = false;
        // ResultsPanel.GetComponent<CanvasGroup>().alpha = 0;
        if (EnablePanelsOnReset){
            ActionsPanel.SetActive(true);
            MenuPanel.SetActive(true);
            BuildingPanel.SetActive(true);
            ControlsPanel.SetActive(true);
            ControlsPanel.GetComponent<Animator>().SetBool("show", false);
        }
        
        // ResultsPanel.GetComponent<Animator>().SetBool("closed", false);
        
    }

    public void SFXButtonPress()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }
}
