using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{

    public GameObject UserBuildings;
    private List<Vector3> _allChildrenTransformsPositions;

    public GameObject PlayButtonObj;
    public GameObject ResetButtonObj;
    private Button _playButton;
    private Button _resetButton;

   

    public enum State{Plan,Play,Paused}
    private State _currState;

    // Start is called before the first frame update
    void Start()
    {
        _currState = State.Plan;
        _playButton = PlayButtonObj.GetComponent<Button>();
        _resetButton = ResetButtonObj.GetComponent<Button>();
        _playButton.onClick.AddListener(PlayButtonPressed);
        _resetButton.onClick.AddListener(ResetButtonPressed);
        _allChildrenTransformsPositions = new List<Vector3>();
        // _resetButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayButtonPressed(){
        switch(_currState){
        case State.Plan:
            _currState = State.Play;
            // _resetButton.interactable = true;

            // SAVING USERBUILDING TRANSFORMS
            _allChildrenTransformsPositions.Clear();
            foreach (Transform child in UserBuildings.transform){
                _allChildrenTransformsPositions.Add(child.position);
            }
             
            PlayButtonObj.GetComponentInChildren<Text>().text = "Pause";
            break;
        case State.Play:
            _currState = State.Paused;
            PlayButtonObj.GetComponentInChildren<Text>().text = "Play";
            break;
        case State.Paused:
            _currState = State.Play;
            PlayButtonObj.GetComponentInChildren<Text>().text = "Pause";
            break;
        }
    }

    void ResetButtonPressed(){
        switch(_currState){
        case State.Plan:
            foreach (Transform child in UserBuildings.transform){
                Destroy(child.gameObject);
            }
            break;
        default:
            _currState = State.Plan;
            int i = 0;
            foreach (Transform child in UserBuildings.transform){
                child.position = _allChildrenTransformsPositions[i];
                i++;
            }
            PlayButtonObj.GetComponentInChildren<Text>().text = "Play";
            break;
        }
    }
}