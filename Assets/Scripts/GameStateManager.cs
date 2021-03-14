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
                foreach (Transform child in UserBuildings.transform)
                {
                    _allChildrenTransformsPositions.Add(child.localPosition);
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
                //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                foreach (Transform child in UserBuildings.transform)
                {
                    MoneyManager.Instance.RefundItem(child.gameObject.name.Split('(')[0].ToLower());

                    Destroy(child.gameObject);
                }
                PackagesSpawner.Instance.ResetAllPackages();
                ResultsText.text = "";
                break;
            default:
                StopCoroutine(_spawner);
                CurrState = State.Plan;
                int i = 0;
                foreach (Transform child in UserBuildings.transform){
                    child.localPosition = _allChildrenTransformsPositions[i];
                    // TODO: Remove this hack
                    child.gameObject.SetActive(true);
                    if (child.GetComponent<Holding>() != null)
                    {
                        child.GetComponent<Holding>().stopped = false;
                        child.GetComponent<Holding>().ThresholdText.text = "0/" + child.GetComponent<Holding>().ThresholdNumHeldPlayers.ToString();
                    }
                    i++;
                }
                foreach (Transform child in HitchhikerManager.transform){
                    // Destroy(child.gameObject);
                    child.gameObject.SetActive(false);
                }
                if (EnemyManager!= null){
                    foreach (Transform child in EnemyManager.transform){
                        child.GetComponent<EnemyAI>().resetState();
                    }
                } else {
                    Debug.Log("No EnemyManager GameObject attached to GameStateManager");
                }
                PlayButtonIcon.sprite = playButtonPlay;
                ResetButtonIcon.sprite = resetButtonReset;
                GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().ResetPlayerResults();
                PackagesSpawner.Instance.ResetAllPackages();
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
        }
        
        // ResultsPanel.GetComponent<Animator>().SetBool("closed", false);
        
    }

    public void SFXButtonPress()
    {
        AudioManager.PlaySFX(AudioManager.UI_BUTTON_PRESS);
    }
}
