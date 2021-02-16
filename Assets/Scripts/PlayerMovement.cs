using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 2.0f;
    public Vector3 direction;
    public Vector3 rotation;
    public Animator Animator;
    public bool WasGroundedLastFrame = false;
    public bool IsGroundedThisFrame = true;
    public bool IsStopped = false;

    private float _distToGround;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
        Animator = GetComponent<Animator>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    public bool IsGrounded()
    {
        // Debug.DrawLine(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.center - Vector3.up * (_distToGround + 0.05f), Color.red, 1.0f);
        return Physics.Raycast(GetComponent<Collider>().bounds.center, -Vector3.up, _distToGround + 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStopped)
        {
            transform.position += speed * Time.deltaTime * direction;
        }

        if (transform.position.y < -10.0f)
        {
            LosePlayer();
        }

        IsGroundedThisFrame = IsGrounded();
        
        if (WasGroundedLastFrame && !IsGroundedThisFrame)
        {
            Animator.SetBool("isAirborne", true);
            Animator.ResetTrigger("jump");
        } else if (!WasGroundedLastFrame && IsGroundedThisFrame && GetComponent<Rigidbody>().velocity.y < 0)
        {
            Animator.SetBool("isAirborne", false);
        }

        WasGroundedLastFrame = IsGroundedThisFrame;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 360f * Time.deltaTime);
    }

    public void LosePlayer()
    {
        GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().PlayersFailed += 1;
        gameObject.SetActive(false);
    }

    public void StopPlayer()
    {
        IsStopped = true;
    }

    public void UnstopPlayer()
    {
        IsStopped = false;
    }

    

}
