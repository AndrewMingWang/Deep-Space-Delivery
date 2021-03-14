using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum State{waiting, chargingAnimationStart, charging, chargingAnimationEnd, returning, collisionAnimation, stunned}

    public State currState = State.waiting;
    int playerLayerMask = 1 << 10; // Player layer
    int buildingLayerMask = 1 << 9; // Building layer
    int envBuildingLayerMask = 1 << 13; // Environment Buildings
    int layerMask;
    RaycastHit hit;

    [HideInInspector]
    public Collider Collider;
    [HideInInspector]
    public Animator Animator;

    
    private Vector3 target_pos;
    private Vector3 starting_pos;
    private int lerpFrameTotal; // Number of frames to completely interpolate between the 2 positions
    private int elapsedFrames = 0;
    private float lerpRatio;
    private Vector3 lerpPosition;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = playerLayerMask | buildingLayerMask | envBuildingLayerMask;
        Collider = GetComponent<Collider>();
        Animator = GetComponent<Animator>();
        starting_pos = transform.position;
        lerpRatio = 0;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currState){
            case State.waiting:
                if (Physics.Raycast(Collider.bounds.center, transform.forward, out hit, Mathf.Infinity, layerMask)){
                    if (hit.transform.CompareTag("player"))
                    {
                        // currState = State.chargingAnimationStart;
                        // Debug.Log("HIT PLAYER");
                        currState = State.chargingAnimationStart;
                        target_pos = hit.transform.position;
                        lerpFrameTotal = (int)(12*hit.distance);
                        Animator.SetBool("ChargeUp", true);
                    }
                }
                break;
            case State.chargingAnimationStart:
                if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("ChargeUpFinished")){
                    Animator.SetBool("ChargeUp", false);
                    currState = State.charging;
                }
                break;
            case State.charging:
                lerpRatio = (float)elapsedFrames / lerpFrameTotal;
                lerpPosition = Vector3.Lerp(starting_pos, target_pos, lerpRatio);
                transform.position = lerpPosition;
                if (elapsedFrames != lerpFrameTotal){
                    elapsedFrames++;
                } else {
                    elapsedFrames = 0;
                    // currState = State.chargingAnimationEnd;
                    currState = State.chargingAnimationEnd;
                    lerpFrameTotal = (int)(25*hit.distance);
                    Animator.SetBool("CoolDown", true);   
                }
                break;
            case State.chargingAnimationEnd:
                if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("CoolDownFinished")){
                    Animator.SetBool("CoolDown", false);
                    currState = State.returning;
                }
                break;
            case State.returning:
                lerpRatio = (float)elapsedFrames / lerpFrameTotal;
                lerpPosition = Vector3.Lerp(target_pos, starting_pos, lerpRatio);
                transform.position = lerpPosition;
                if (elapsedFrames != lerpFrameTotal){
                    elapsedFrames++;
                } else {
                    elapsedFrames = 0;
                    // currState = State.chargingAnimationEnd;
                    currState = State.waiting;
                }
                break;
            case State.collisionAnimation:
                break;
            case State.stunned:
                break;
        }
    }

    public void resetState(){
        Animator.SetTrigger("LevelReset");
        Animator.SetBool("ChargeUp", false);
        Animator.SetBool("CoolDown", false);
        Animator.SetBool("EnemyCollision", false);
        transform.position = starting_pos;
        lerpPosition = starting_pos;
        elapsedFrames = 0;
        currState = State.waiting;
    }
}
