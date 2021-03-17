using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Mechanics")]
    public float UnitSpeed = 2.0f;

    [Header("Wind")]
    public float WindSpeed;
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
    public GameObject Package;
    public bool PackageShown = false;
    private bool _groundedLastFrame = false;
    private bool _groundedThisFrame = true;
    private bool _isAirborne = false;
    private float _distToGround;
    private AudioSource AudioSource;

    public void ShowPackage()
    {
        PackageShown = true;
        Package.SetActive(true);
    }

    public void HidePackage()
    {
        PackageShown = false;
        Package.SetActive(false);
    }

    private void Awake()
    {
        SceneParent = GameObject.FindGameObjectWithTag("SceneParent");
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider>();
        AudioSource = GetComponent<AudioSource>();
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
        int envBuildingLayerMask = 1 << 13; // Environment Buildings
        int layerMask = windLayerMask | buildingLayerMask | envBuildingLayerMask;
        RaycastHit hit;

        Vector3 res = Vector3.zero;

        // Check if wind plane present in 4 cardinal directions
        if (Physics.Raycast(Collider.bounds.center, SceneParent.transform.forward, out hit, Mathf.Infinity, layerMask)) {
            if (hit.transform.CompareTag("Wind"))
            {
                if ((transform.forward.z < 0.71f && transform.forward.z > 0.70f) || (transform.forward.z > -0.71f && transform.forward.z < -0.70f)){
                    res += -SceneParent.transform.forward*0.77f;
                    
                } else {
                    res += -SceneParent.transform.forward;
                }
            }
        }
        if (Physics.Raycast(Collider.bounds.center, SceneParent.transform.right, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Wind"))
            {
                if ((transform.forward.z < 0.71f && transform.forward.z > 0.70f) || (transform.forward.z > -0.71f && transform.forward.z < -0.70f)){
                    res += -SceneParent.transform.right*0.77f;
                    
                } else {
                    res += -SceneParent.transform.right;
                }
                
            }
        }
        if (Physics.Raycast(Collider.bounds.center, -SceneParent.transform.right, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Wind"))
            {
                if ((transform.forward.z < 0.71f && transform.forward.z > 0.70f) || (transform.forward.z > -0.71f && transform.forward.z < -0.70f)){
                    res += SceneParent.transform.right*0.77f;
                    
                } else {
                    res += SceneParent.transform.right;
                }
                
            }
        }
        if (Physics.Raycast(Collider.bounds.center, -SceneParent.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Wind"))
            {
                if ((transform.forward.z < 0.71f && transform.forward.z > 0.70f) || (transform.forward.z > -0.71f && transform.forward.z < -0.70f)){
                    res += SceneParent.transform.forward*0.77f;
                    
                } else {
                    res += SceneParent.transform.forward;
                }
            }
        }
        Debug.Log(res);
        // Debug.Log(transform.forward.z);
        // forward
        

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
            _isAirborne = true;
        } else if (!_groundedLastFrame && _groundedThisFrame)
        {
            Animator.SetBool("isAirborne", false);
            _isAirborne = false;
        }
        _groundedLastFrame = _groundedThisFrame;

        // Walking SFX
        if (_isAirborne)
        {
            if (AudioSource.isPlaying)
            {
                GetComponent<AudioSource>().Pause();
            }
        } else
        {
            if (!AudioSource.isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }

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
        if (transform.position.y < -30.0f)
        {
            LosePlayer();
        }
    }

    public void LosePlayer()
    {
        GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().packagesLost += 1;
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
