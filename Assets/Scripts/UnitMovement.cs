using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Mechanics")]
    public float UnitSpeed = 2.0f;

    [Header("Wind")]
    public float WindSpeed = 100f;
    [HideInInspector]
    public GameObject SceneParent;

    [Header("Info")]
    public Vector3 TargetDirection;
    public bool IsStopped = false;
    public float GroundingEpsilon;
    [HideInInspector]
    public Collider Collider;

    [Header("Animation")]
    [HideInInspector]
    public Animator Animator;
    private bool _groundedLastFrame = false;
    private bool _groundedThisFrame = true;
    private float _distToGround;

    private void Awake()
    {
        SceneParent = GameObject.FindGameObjectWithTag("SceneParent");
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
    }

    void Start()
    {
        TargetDirection = transform.forward;
        _distToGround = Collider.bounds.extents.y;
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    public Vector3 WindDirection()
    {
        int windLayerMask = 1 << 11; // Wind layer
        int buildingLayerMask = 1 << 9; // Building layer
        int layerMask = windLayerMask | buildingLayerMask;
        RaycastHit hit;

        Vector3 res = Vector3.zero;

        // Check if wind plane present in 4 cardinal directions
        if (Physics.Raycast(Collider.bounds.center, SceneParent.transform.forward, out hit, Mathf.Infinity, layerMask)) {
            if (hit.transform.CompareTag("Wind"))
            {
                res += -SceneParent.transform.forward;
            }
        }
        if (Physics.Raycast(Collider.bounds.center, SceneParent.transform.right, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Wind"))
            {
                res += -SceneParent.transform.right;
            }
        }
        if (Physics.Raycast(Collider.bounds.center, -SceneParent.transform.right, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Wind"))
            {
                res += SceneParent.transform.right;
            }
        }
        if (Physics.Raycast(Collider.bounds.center, -SceneParent.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Wind"))
            {
                res += SceneParent.transform.forward;
            }
        }

        return res;
    }

    public bool IsGrounded()
    {
        // Debug.DrawLine(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.center - Vector3.up * (_distToGround + 0.2f), Color.red, 300f);
        return Physics.Raycast(Collider.bounds.center, -Vector3.up, _distToGround + GroundingEpsilon);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check for grounding
        _groundedThisFrame = IsGrounded();
        if (_groundedLastFrame && !_groundedThisFrame)
        {
            Animator.SetBool("isAirborne", true);
            GetComponent<AudioSource>().Pause();
        } else if (!_groundedLastFrame && _groundedThisFrame)
        {
            Animator.SetBool("isAirborne", false);
            GetComponent<AudioSource>().Play();
        }
        _groundedLastFrame = _groundedThisFrame;

        // Move unit
        if (!IsStopped)
        {
            // Apply self movement
            transform.localPosition += UnitSpeed * Time.deltaTime * TargetDirection;
            // Apply wind movement
            transform.position += WindSpeed * Time.deltaTime * WindDirection();
        }

        // Rotate unit
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(TargetDirection), 360f * Time.deltaTime);

        // Check for unit out of bounds
        if (transform.position.y < -10.0f)
        {
            LosePlayer();
        }
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
