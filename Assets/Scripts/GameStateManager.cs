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
    public Sprite slowSpeedIcon;
    public Sprite fastSpeedIcon;
    public GameObject FastForwardButtonIcon;

    public GameObject Spawn;
    public GameObject ResultsPanel;
    public GameObject ActionsPanel;
    public GameObject ControlsPanel;
    public GameObject MenuPanel;
    private SpawnPlayers _spawnscript;
    public TMP_Text ResultsText;

    public enum State{Intro,Preplan,Plan,Play,Paused}
    public State CurrState;

    public GameObject BuildingPanel;
    public bool EnablePanelsOnReset = true;
    private Coroutine _spawner;
    [HideInInspector]
    public bool _fast;
    public GameObject menuButtonsParentPanelForeground;
    public GameObject continueText;
    private Vector3 _startCameraPos;
    private Quaternion _startCameraRot;
    private Quaternion _startCameraParentRot;
    private Quaternion _endCameraParentRot;
    private int _elapsedSlerp;

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
        CurrState = State.Intro;
        _allChildrenTransformsPositions = new List<Vector3>();
        _spawnscript = Spawn.GetComponent<SpawnPlayers>();
        // _resetButton.interactable = false;
        ControlsPanel = GameObject.Find("Controls");
        _fast = false;
        FastForwardButtonIcon.GetComponent<Image>().sprite = slowSpeedIcon;
        _startCameraPos = CameraMovement.Instance.gameObject.transform.localPosition;
        _startCameraRot = CameraMovement.Instance.gameObject.transform.localRotation;
        _startCameraParentRot = CameraMovement.Instance.gameObject.transform.parent.rotation;
        _elapsedSlerp = 0;
    }

    private void Update() {
        switch(CurrState){
            case State.Intro:
                if (continueText.activeInHierarchy){
                    float newY = CameraMovement.Instance.gameObject.transform.parent.rotation.eulerAngles.y + 0.05f;
                    CameraMovement.Instance.gameObject.transform.parent.rotation = Quaternion.Euler(CameraMovement.Instance.gameObject.transform.parent.rotation.eulerAngles.x, newY, CameraMovement.Instance.gameObject.transform.parent.rotation.eulerAngles.z);
                }
                if (Input.anyKeyDown && continueText.activeInHierarchy)
                {
                    continueText.SetActive(false);
                    CurrState = State.Preplan;
                    _endCameraParentRot = CameraMovement.Instance.gameObject.transform.parent.rotation;
                }
                break;
            case State.Preplan:
                if (_elapsedSlerp >= 60.0f){
                    CameraMovement.Instance.gameObject.transform.localPosition = _startCameraPos;
                    CameraMovement.Instance.gameObject.transform.localRotation = _startCameraRot;
                    CurrState = State.Plan;
                    _elapsedSlerp = 0;
                    LevelEntryAnimationPlus.Instance.TriggerBringUpUI();
                } else {
                    _elapsedSlerp++;
                    CameraMovement.Instance.gameObject.transform.parent.rotation = Quaternion.Slerp(_endCameraParentRot, _startCameraParentRot, _elapsedSlerp/45.0f);
                }
                // CameraMovement.Instance.gameObject.transform.parent.rotation = _startCameraParentRot;
                break;
        }
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
                if (_fast){
                    Time.timeScale = 2.0f;
                } else {
                    Time.timeScale = 1.0f;
                }
                break;
            }
        ResultsPanel.GetComponent<Animator>().SetBool("closed", false);
    }

    public void ResetButtonPressed()
    {
        if (BuildManager.BuildingSelected)
        {
            BuildManager.Instance.CancelBuilding();
            return;
        }
        if (_fast){
            Time.timeScale = 2.0f;
        } else {
            Time.timeScale = 1.0f;
        }
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
                    building.HardReset();

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
                    child.gameObject.GetComponent<Dog>().DeleteDog();
                }
                if (EnemyManager!= null){
                    foreach (Transform child in EnemyManager.transform){
                        if (child.name.Contains("Alien"))
                        {
                            child.GetChild(0).GetComponentInChildren<EnemyAI>().resetState();
                        }
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

    public void SpeedChange(){
        // if (CurrState == State.Play){
            if (!_fast && CurrState != State.Paused){
                _fast = true;
                Time.timeScale = 2.0f;
                FastForwardButtonIcon.GetComponent<Image>().sprite = fastSpeedIcon;   
            } else if (CurrState != State.Paused) {
                _fast = false;
                Time.timeScale = 1.0f;
                FastForwardButtonIcon.GetComponent<Image>().sprite = slowSpeedIcon;
            }
        // }
        
    }

// ONLY USE THIS IF YOU KNOW WHAT YOU ARE DOING
    public void RulerHighlightToggle(){
        menuButtonsParentPanelForeground.GetComponent<Animator>().SetBool("highlighted", false);
    }

    public void EnableContinueText(){
        continueText.SetActive(true);
    }
}
