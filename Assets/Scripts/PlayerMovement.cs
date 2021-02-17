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
    public float WindSpeed = 100f;

    private float _distToGround;
    private float _radius;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
        Animator = GetComponent<Animator>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
        _radius = GetComponent<CapsuleCollider>().radius;
        
    }

    public Vector3 GetWind()
    {
        int windLayerMask = 1 << 11;
        int buildingLayerMask = 1 << 9;
        int layerMask = windLayerMask | buildingLayerMask;
        RaycastHit hit;

        Vector3 res = Vector3.zero;

        if (Physics.Raycast(GetComponent<Collider>().bounds.center, transform.forward, out hit, Mathf.Infinity, layerMask)) {
            Debug.Log("wind forward");
            res += -transform.forward;
        }
        if (Physics.Raycast(GetComponent<Collider>().bounds.center, transform.right, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("wind right");
            res += -transform.right;
        }
        if (Physics.Raycast(GetComponent<Collider>().bounds.center, -transform.right, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("wind left");
            res += transform.right;
        }
        if (Physics.Raycast(GetComponent<Collider>().bounds.center, -transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("wind back");
            res += transform.forward;
        }

        return res * Time.deltaTime * WindSpeed;
    }

    public bool IsGrounded()
    {
        Debug.DrawLine(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.center - Vector3.up * (_distToGround + 0.2f), Color.red, 300f);
        return Physics.Raycast(GetComponent<Collider>().bounds.center, -Vector3.up, _distToGround + 0.2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsStopped)
        {
            transform.position += speed * Time.deltaTime * direction + GetWind();
        }

        if (transform.position.y < -10.0f)
        {
            LosePlayer();
        }

        IsGroundedThisFrame = IsGrounded();
        
        if (WasGroundedLastFrame && !IsGroundedThisFrame)
        {
            //Debug.Log("Airborne");
            Animator.SetBool("isAirborne", true);
        } else if (!WasGroundedLastFrame && IsGroundedThisFrame)
        {
            //Debug.Log("Landed");
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
