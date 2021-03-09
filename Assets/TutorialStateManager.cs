using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialStateManager : MonoBehaviour
{
  
    public enum State{one_text, one_wait, two_text, two_wait, three_text, three_wait, four_text, four_wait, five_text, five_wait, 
                        six_text, six_wait, seven_text, seven_wait, eight_text, eight_wait, nine_text, nine_wait, ten_text, ten_wait,
                        eleven_text, eleven_wait, twelve_text, twelve_wait, thirteen_text, thirteen_wait, fourteen_text, fourteen_wait, fifteen_text, fifteen_wait}

    public State currState;

    [Header("UI_Elements")]
    public TextMeshProUGUI tutorialNarration;
    public GameObject NarrationPanel;
    public Button PlayButton;
    public Button ResetButton;
    public GameObject BuildingPanel;
    public GameObject MenuPanel;
    public GameObject Controls;

    [Header("Tiles")]
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile2_pcube1;

    [Header("Abstract")]
    public GameObject buildManager;

    private bool textFlag = true;

    private void Start() {
        
    }

    private void Update() {
        switch(currState){
        case State.one_text:
            if (textFlag){
                StringUtility.TypeTextEffect(tutorialNarration, 
                "Hey!\\p Laura here,\\p\\p\\n" +
                "let's walk through how \\n" +
                "loss prevention works.\\p\\p\\n" +
                "Press the play button to\\n" +
                "send in our associates!",
                2.0f);
                PlayButton.interactable = false;
                ResetButton.interactable = false;
                textFlag = false;
            }
            // PlayButton.GetComponent<Animator>().SetTrigger("Highlighted");
            if (!StringUtility.Instance.IsTyping){
                PlayButton.interactable = true;
                currState = State.one_wait;
                textFlag = true;
            }
            break;
        case State.one_wait:
            if (GameStateManager.Instance.CurrState == GameStateManager.State.Play){
                PlayButton.interactable = false;
                ResetButton.interactable = false;
                // PlayButton.GetComponent<Animator>().SetTrigger("Normal");
            }
            if (GoalTrigger.Instance.IsLevelDone()){
                currState = State.two_text;
                textFlag = true;
            }
            break;
        case State.two_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "Oops!\\p\\p Don't worry we'll pick them up later. Did I mention Deep Space Delivery is number 1 in human employee safety? Press the retry button to try again.",
                2.0f);
                ResetButton.interactable = true;
                textFlag = false;
            }
            // ResetButton.GetComponent<Animator>().SetTrigger("Highlighted");
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.two_wait;
            }
            break;
        case State.two_wait:
            if (GameStateManager.Instance.CurrState == GameStateManager.State.Plan){
                currState = State.three_text;
                // ResetButton.GetComponent<Animator>().SetTrigger("Normal");
            }
            break;
        case State.three_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "While our associates are diligent, they're not always the brightest. It's our job to point them in the right direction. Let's try building a sign.",
                2.0f);
                BuildingPanel.SetActive(true);
                textFlag = false;
                ResetButton.interactable = false;
            }
            if (!StringUtility.Instance.IsTyping){
                BuildManager.Instance.allowBuildingNewBuildings = true;
                textFlag = true;
                currState = State.three_wait;
            }
            break;
        case State.three_wait:
            if (BuildManager.Instance.CurrBuilding != null){
                currState = State.four_text;
                BuildManager.Instance.allowBuildingNewBuildings = false;
                BuildingPanel.SetActive(false);
            }
            break;
        case State.four_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "Just click the tile to place it down.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.four_wait;
            }
            break;
        case State.four_wait:
            if (Tile1.GetComponent<Tile>().OccupyingBuilding != null){
                currState = State.five_text;
            }
            break;
        case State.five_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "Right now, it won’t point the dogs in a new direction, but we can rotate it. Click the sign to pick it up again.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                BuildManager.Instance.allowPickingUpBuildings = true;
                textFlag = true;
                currState = State.five_wait;
            }
            break;
        case State.five_wait:
            if (BuildManager.Instance.CurrBuilding != null){
                BuildManager.Instance.allowRotatingBuildings = true;
                currState = State.six_text;
            }
            break;
        case State.six_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "You can rotate the sign with A or D, turn it to point to the right.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.six_wait;
            }
            break;
        case State.six_wait:
            if (Tile1.GetComponent<Tile>().OccupyingBuilding != null){
                if (Tile1.GetComponent<Tile>().OccupyingBuilding.transform.eulerAngles.y > 89 && Tile1.GetComponent<Tile>().OccupyingBuilding.transform.eulerAngles.y < 91){
                    currState = State.eight_text;
                } else {
                    currState = State.seven_text;
                }
            }
            break;
        case State.seven_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "That’s not quite the right direction, let’s pick it up again, and point it to the right",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.seven_wait;
            }
            break;
        case State.seven_wait:
            if (BuildManager.Instance.CurrBuilding != null){
                currState = State.six_wait;
                tutorialNarration.text = "";
            }
            break;
        case State.eight_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "Great, now put down one more sign to redirect the dogs to the exit.",
                2.0f);
                textFlag = false;
                BuildingPanel.SetActive(true);
            }
            TileManager.Instance.AddUnoccupiedTile(Tile2.GetComponent<Tile>());
            if (!StringUtility.Instance.IsTyping){
                BuildManager.Instance.allowBuildingNewBuildings = true;
                BuildManager.Instance.allowRotatingBuildings = false;
                textFlag = true;
                currState = State.eight_wait;
            }
            break;
        case State.eight_wait:
            if (Tile2.GetComponent<Tile>().OccupyingBuilding != null){
                currState = State.nine_text;
                PlayButton.interactable = true;
                // PlayButton.GetComponent<Animator>().SetTrigger("Normal");
            }
            break;
        case State.nine_text:
            // PlayButton.GetComponent<Animator>().SetTrigger("Highlighted");
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "This should work! Let’s test it.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.nine_wait;
            }
            break;
        case State.nine_wait:
            if (GoalTrigger.Instance.IsLevelDone()){
                currState = State.ten_text;
                ResetButton.interactable = true;
            }
            break;
        case State.ten_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "That was great, but we can actually solve it more efficiently. Every penny counts. Restart the level.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.ten_wait;
            }
            break;
        case State.ten_wait:
            if (GameStateManager.Instance.CurrState == GameStateManager.State.Plan){
                currState = State.eleven_text;
                ResetButton.interactable = false;
                PlayButton.interactable = false;
            }
            break;
        case State.eleven_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "If you pick up and then right click you can remove objects you’ve put down. Remove all of the signs.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                BuildManager.Instance.allowPickingUpBuildings = true;
                BuildManager.Instance.allowDeletingBuildings = true;
                textFlag = true;
                currState = State.eleven_wait;
            }
            break;
        case State.eleven_wait:
            int childcount = 0;
            foreach (Transform building in buildManager.transform){
                childcount++;
            }
            if (childcount == 0){
                currState = State.twelve_text;
            }
            break;
        case State.twelve_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "Put a sign down on the center tile, but have it face the goal.",
                2.0f);
                textFlag = false;
                BuildingPanel.SetActive(true);
            }
            if (!StringUtility.Instance.IsTyping){
                BuildManager.Instance.allowBuildingNewBuildings = true;
                BuildManager.Instance.allowRotatingBuildings = true;
                textFlag = true;
                currState = State.twelve_wait;
            }
            break;
        case State.twelve_wait:
            if (Tile1.GetComponent<Tile>().OccupyingBuilding != null){
                if (Tile1.GetComponent<Tile>().OccupyingBuilding.transform.eulerAngles.y > 44 && Tile1.GetComponent<Tile>().OccupyingBuilding.transform.eulerAngles.y < 46){
                    currState = State.fourteen_text;
                } else {
                    currState = State.thirteen_text;
                }
            }
            break;
        case State.thirteen_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "That’s not quite the right direction, let’s pick it up again, and make it face the goal",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.thirteen_wait;
            }
            break;
        case State.thirteen_wait:
             if (BuildManager.Instance.CurrBuilding != null){
                currState = State.twelve_wait;
                tutorialNarration.text = "";
            }
            break;
        case State.fourteen_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "That’s how it’s done. Now press play and see the fruits of our labour.",
                2.0f);
                textFlag = false;
            }
            if (!StringUtility.Instance.IsTyping){
                currState = State.fourteen_wait;
                textFlag = true;
                PlayButton.interactable = true;
            }
            break;
        case State.fourteen_wait:
            if (GoalTrigger.Instance.IsLevelDone()){
                currState = State.fifteen_text;
            }
            break;
        case State.fifteen_text:
            if (textFlag){
                tutorialNarration.text = "";
                StringUtility.TypeTextEffect(tutorialNarration, 
                "You can always find the controls to the interface up here, make sure you read them.",
                2.0f);
                textFlag = false;
                MenuPanel.SetActive(true);
            }
            if (!StringUtility.Instance.IsTyping){
                textFlag = true;
                currState = State.fifteen_wait;
            }
            break;
        case State.fifteen_wait:
            if (Controls.activeInHierarchy){
                tutorialNarration.text = "";
                NarrationPanel.SetActive(false);
            }
            break;
        }
    }

}
