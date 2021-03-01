using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialStateMachine : MonoBehaviour
{

    public enum State{minusOneS, zeroStart, zeroA, zeroB, zeroS, zeroSB, oneS, oneU, twoS, twoU, threeS, threeU, fourS, fourU, fourUA, fourUAA, fourUAB, fourUB, fiveS, fiveU, sixS, sixU, sixB, sevenS, sevenSB, sevenU, eightS}
    public State currState;

    public GameObject BuildingPanel;
    public GameObject PlayButton;
    public GameObject ResetButton;
    public GameObject Menu;
    public GameObject Controls;
    public GameObject EnergyText;
    public GameObject EnergyBar;
    public GameObject TutorialNarration;
    public GameObject LevelTitle;
    public GameObject MainCamera;
    private Animator _cameraAnim;
  

    public GameObject GameManager;
    public GameObject BuildManager;
    public GameObject HitchikerManager;
    public GameObject Goal;
    public GameObject TheTile;

    // Start is called before the first frame update
    void Start()
    {
        currState = State.zeroStart;
        _cameraAnim = MainCamera.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState){
        case State.minusOneS:
            BuildingPanel.SetActive(false);
            PlayButton.SetActive(false);
            ResetButton.SetActive(false);
            EnergyBar.SetActive(false);
            EnergyText.SetActive(false);
            Menu.SetActive(false);
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "I hear something, but it's so far away. I can zoom in by scrolling the mouse wheel.";
            if (Input.mouseScrollDelta.y != 0){
                currState = State.zeroStart;
            }
            break;
        case State.zeroStart:
            BuildingPanel.SetActive(false);
            PlayButton.SetActive(false);
            ResetButton.SetActive(false);
            EnergyBar.SetActive(false);
            EnergyText.SetActive(false);
            Menu.SetActive(false);
            TutorialNarration.SetActive(false);
            _cameraAnim.Play("CameraZoomIn");
            currState = State.zeroA;
            break;
        case State.zeroA:
            if (_cameraAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                currState = State.zeroB;
            }
            break;
        case State.zeroB:
            GameManager.GetComponent<TutorialGameStateManager>().PlayButtonPressed();
            currState = State.zeroS;
            break;
        case State.zeroS:
            foreach (Transform child in HitchikerManager.transform){
                if(child.transform.position.y < -20){
                    _cameraAnim.Play("CameraZoomOut");
                    currState = State.zeroSB;
                }
            }
            break;
        case State.zeroSB:
            if (_cameraAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                currState = State.oneS;
                MainCamera.GetComponent<Animator>().SetTrigger("cameraAnimDone");
                MainCamera.GetComponent<Animator>().enabled = false;
            }
            break;
        case State.oneS:
            Goal.GetComponent<TutorialGoalTrigger>().ResetPlayerResults();
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Oh no! There must be something I can do! Let's try rewinding time.";
            TutorialNarration.SetActive(true);
            ResetButton.SetActive(true);
            currState = State.oneU;
            break;
        case State.oneU:
            if (GameManager.GetComponent<TutorialGameStateManager>().CurrState == TutorialGameStateManager.State.Plan){
                currState = State.twoS;
            }
            break;
        case State.twoS:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "And if I press this...?";
            PlayButton.SetActive(true);
            ResetButton.SetActive(false);
            if (GameManager.GetComponent<TutorialGameStateManager>().CurrState == TutorialGameStateManager.State.Play){
                currState = State.twoU;
            }
            break;
        case State.twoU:
            PlayButton.SetActive(false);
            TutorialNarration.SetActive(false);
            if(Goal.GetComponent<TutorialGoalTrigger>().IsLevelFailed()){
                currState = State.threeS;
            }
            break;
        case State.threeS:
            Goal.GetComponent<TutorialGoalTrigger>().ResetPlayerResults();
            TutorialNarration.SetActive(true);
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Hmm, I have to put something in the way. I have an idea!";
            ResetButton.SetActive(true);
            if (GameManager.GetComponent<TutorialGameStateManager>().CurrState == TutorialGameStateManager.State.Plan){
                currState = State.fourS;
            }
            break;
        case State.fourS:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Let's try putting down a sign.";
            ResetButton.SetActive(false);
            BuildingPanel.SetActive(true);
            EnergyBar.SetActive(true);
            EnergyText.SetActive(true);
            if (BuildManager.GetComponent<TutorialBuildManager>().CurrBuilding != null){
                currState = State.fourU;
            }
            break;    
        case State.fourU:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "That took some energy... I just need to click it to place it down.";
            if (TheTile.GetComponent<Tile>().OccupyingBuilding != null){
                currState = State.fourUB;
            }
            break;
        case State.fourUA:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "If I place it in the wrong spot, I think I can pick it up and remove it.";
            if (BuildManager.GetComponent<TutorialBuildManager>().CurrBuilding != null){
                currState = State.fourUAA;
            }
            break;
        case State.fourUAA:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "I'll try to remove it by right clicking.";
            if (BuildManager.GetComponent<TutorialBuildManager>().CurrBuilding == null){
                currState = State.fourUAB;
            }
            break;
        case State.fourUAB:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Good to know, I'll put one back down.";
            if (TheTile.GetComponent<Tile>().OccupyingBuilding != null){
                currState = State.fourUB;
            }
            break;
        case State.fourUB:
            BuildingPanel.SetActive(false);
            PlayButton.SetActive(true);
            EnergyBar.SetActive(false);
            EnergyText.SetActive(false);
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Maybe this will work?";
            if (GameManager.GetComponent<TutorialGameStateManager>().CurrState == TutorialGameStateManager.State.Play){
                currState = State.fiveS;
            }
            break;
        case State.fiveS:
            PlayButton.SetActive(false);
            if(Goal.GetComponent<TutorialGoalTrigger>().IsLevelFailed()){
                currState = State.fiveU;
            }
            break;
        case State.fiveU:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Wait, I forgot to point the sign in the right direction!";
            ResetButton.SetActive(true);
            if (GameManager.GetComponent<TutorialGameStateManager>().CurrState == TutorialGameStateManager.State.Plan){
                currState = State.sixS;
            }
            break;
        case State.sixS:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Just have to pick up the sign by clicking on it.";
            ResetButton.SetActive(false);
            if (BuildManager.GetComponent<TutorialBuildManager>().CurrBuilding != null){
                currState = State.sixU;
            }
            break;
        case State.sixU:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "How do you rotate again... A or D? I should turn the sign to face the exit.";
            if (BuildManager.GetComponent<TutorialBuildManager>().CurrBuilding == null){
                if (TheTile.GetComponent<Tile>().OccupyingBuilding.transform.rotation.eulerAngles.y > 89 &&
                    TheTile.GetComponent<Tile>().OccupyingBuilding.transform.rotation.eulerAngles.y < 91){
                    currState = State.sevenS;
                }
                else {
                    currState = State.sixB;
                }
            }
            break;
        case State.sixB:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "It's not quite facing the right direction, I'll pick it up and rotate it again.";
            if (BuildManager.GetComponent<TutorialBuildManager>().CurrBuilding != null){
                currState = State.sixU;
            }
            break;
        case State.sevenS:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "This should be it!";
            PlayButton.SetActive(true);
            if (GameManager.GetComponent<TutorialGameStateManager>().CurrState == TutorialGameStateManager.State.Play){
                currState = State.sevenSB;
            }
            break;
        case State.sevenSB:
            PlayButton.SetActive(false);
            if(Goal.GetComponent<TutorialGoalTrigger>().IsLevelPassed()){
                currState = State.sevenU;
            }
            break;
        case State.sevenU:
            TutorialNarration.GetComponent<TextMeshProUGUI>().text = "Nice! My powers are coming back to me.";
            Menu.SetActive(true);
            if (Controls.activeInHierarchy){
                currState = State.eightS;
            }
            break;
        case State.eightS:
            TutorialNarration.SetActive(false);
            LevelTitle.SetActive(false);
            Controls.SetActive(true);
            break;
        }
    }
}
