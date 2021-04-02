using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum State{waiting, chargingAnimationStart, charging, chargingAnimationEnd, returning, collisionAnimation, stunned, reset_wait}

    public Transform TopLevelParent;
    public Transform Indicator;
    public State currState = State.waiting;
    int playerLayerMask = 1 << 10; // Player layer
    int buildingLayerMask = 1 << 9; // Building layer
    int envBuildingLayerMask = 1 << 13; // Environment Buildings

    int tileLayerMask = 1 << 8;
    int layerMask;
    RaycastHit hit;
    RaycastHit hit2;

    [HideInInspector]
    public Collider Collider;
    [HideInInspector]
    public Animator Animator;

    private Vector3 target_pos;
    private Vector3 starting_parent_pos;
    private Quaternion starting_parent_rotation;
      private Vector3 starting_local_pos;
    private Quaternion starting_local_rotation;
    private int lerpFrameTotal; // Number of frames to completely interpolate between the 2 positions
    private int elapsedFrames = 0;
    private float lerpRatio;
    private Vector3 lerpPosition;
    private Vector3 lastTile;
    private Vector3 currTile;

    // Start is called before the first frame update
    void Start()
    {
        Indicator.parent = TopLevelParent.parent;
        currState = State.waiting;
        layerMask = playerLayerMask | buildingLayerMask | envBuildingLayerMask;
        Collider = GetComponent<Collider>();
        Animator = transform.parent.parent.GetComponent<Animator>();
        starting_parent_pos = TopLevelParent.position;
        starting_parent_rotation = transform.parent.rotation;
        starting_local_pos = transform.localPosition;
        starting_local_rotation = transform.localRotation;
        lerpRatio = 0;
        Physics.Raycast(Collider.bounds.center, -transform.up, out hit, Mathf.Infinity, tileLayerMask);
        lastTile = hit.collider.gameObject.transform.position;
        currTile = hit.collider.gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currState){
            case State.waiting:
                Indicator.gameObject.SetActive(false);
                
                // Debug.Log("xnoise");
                TopLevelParent.position = starting_parent_pos + EnemyManager.Instance.SceneObjects.transform.position;
                transform.parent.rotation = starting_parent_rotation;
                transform.localPosition = starting_local_pos; 
                transform.localRotation = starting_local_rotation;
                lerpPosition = starting_parent_pos;

                break;
            case State.chargingAnimationStart:
                if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("ChargeUpFinished")){
                    Animator.SetBool("ChargeUp", false);
                    currState = State.charging;
                }
                break;
            case State.charging:
                lerpRatio = (float)elapsedFrames / lerpFrameTotal;
                lerpPosition = Vector3.Lerp(starting_parent_pos, target_pos, lerpRatio);
                TopLevelParent.position = lerpPosition;
                if (elapsedFrames != lerpFrameTotal){
                    elapsedFrames++;
                } else {
                    elapsedFrames = 0;
                    // currState = State.chargingAnimationEnd;
                    currState = State.chargingAnimationEnd;
                    lerpFrameTotal = (int)(27*hit.distance);
                    Animator.SetBool("CoolDown", true);   
                }
                break;
            case State.chargingAnimationEnd:
                if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("CoolDownFinished")){
                    Animator.SetBool("CoolDown", false);
                    currState = State.returning;
                }
                Indicator.gameObject.SetActive(false);
                break;
            case State.returning:
                lerpRatio = (float)elapsedFrames / lerpFrameTotal;
                lerpPosition = Vector3.Lerp(target_pos, starting_parent_pos, lerpRatio);
                TopLevelParent.position = lerpPosition;
                if (elapsedFrames != lerpFrameTotal){
                    elapsedFrames++;
                } else {
                    elapsedFrames = 0;
                    // currState = State.chargingAnimationEnd;
                    currState = State.waiting;
                }
                break;
            case State.collisionAnimation:
                lerpRatio = (float)elapsedFrames / lerpFrameTotal;
                lerpPosition = Vector3.Lerp(TopLevelParent.position, lastTile, lerpRatio);
                TopLevelParent.position = lerpPosition;
                if (elapsedFrames != lerpFrameTotal){
                    elapsedFrames++;
                } else {
                    elapsedFrames = 0;
                    currState = State.stunned;
                }
                break;
            case State.stunned:
                // Debug.Log("Stunned");
                break;
            case State.reset_wait:
                if (Animator.GetBool("PreWaitingState")){
                    currState = State.waiting;
                    Animator.SetBool("PreWaitingState", false);
                }
                break;
        }

        if (Physics.Raycast(Collider.bounds.center, -transform.up, out hit2, Mathf.Infinity, tileLayerMask) && currState != State.collisionAnimation){
            if (hit2.collider.gameObject.transform.position != currTile){
                lastTile = currTile;
                currTile = hit2.collider.gameObject.transform.position;
            }
            if (currState == State.waiting){
                currTile = hit2.collider.gameObject.transform.position;
                lastTile = currTile;
            }
        }
    }

    public void LineOfSightCheck(float dog_y, float dog_x, float dog_z){
        Debug.DrawRay(TopLevelParent.position + new Vector3(Mathf.Floor(TopLevelParent.position.x)+(dog_x - Mathf.Floor(dog_x)), dog_y - TopLevelParent.position.y, Mathf.Floor(TopLevelParent.position.z)+(dog_z - Mathf.Floor(dog_z))), transform.forward*10.0f, Color.blue, 0.5f);
        if (currState == State.waiting && Physics.Raycast(TopLevelParent.position + new Vector3(Mathf.Floor(TopLevelParent.position.x)+(dog_x - Mathf.Floor(dog_x)) - TopLevelParent.position.x, dog_y - TopLevelParent.position.y, Mathf.Floor(TopLevelParent.position.z)+(dog_z - Mathf.Floor(dog_z)) - TopLevelParent.position.z), transform.forward, out hit, Mathf.Infinity, layerMask)){
            if (hit.transform.CompareTag("player"))
            {
                currState = State.chargingAnimationStart;
                target_pos = hit.transform.position;


                float target_pos_x = Mathf.Floor(target_pos.x) + 0.5f;
                float target_pos_z = Mathf.Floor(target_pos.z) + 0.5f;
                float target_pos_y = Mathf.Floor(Collider.bounds.center.y) + 0.1f;
                target_pos.x = target_pos_x;
                target_pos.z = target_pos_z;
                target_pos.y = target_pos_y;

                lerpFrameTotal = (int)(8*hit.distance);
                Animator.SetBool("ChargeUp", true);
                Indicator.position = target_pos + transform.up*0.03f;
                Indicator.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "player"){
            // Debug.Log("yea");
            other.gameObject.GetComponent<Dog>().LosePlayer();
        }
        if (other.gameObject.tag == "enemy"){
            elapsedFrames = 0;
            lerpFrameTotal = 15;
            // Debug.Log(other.gameObject.name);
            currState = State.collisionAnimation;
            Animator.SetTrigger("EnemyCollision");
            Indicator.gameObject.SetActive(false);
        }
    }

    public void resetState(){
        Animator.SetTrigger("LevelReset");
        Animator.SetBool("ChargeUp", false);
        Animator.SetBool("CoolDown", false);
        Indicator.gameObject.SetActive(false); 
        elapsedFrames = 0;
        
        currState = State.waiting;
    }
}
