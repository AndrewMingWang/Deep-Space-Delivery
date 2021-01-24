using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStates : MonoBehaviour
{
    public enum State{Planning, Placed, Locked};
    public State currState;

    private void Start() {
        currState = State.Planning;
    }
}
