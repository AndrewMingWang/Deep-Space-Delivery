using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dog : MonoBehaviour
{
    [Header("Mechanics")]
    public float UnitSpeed = 2.0f;

    [Header("Wind")]
    public float WindSpeed;
    public GameObject SceneParent;

    [Header("Info")]
    public Vector3 TargetDirection;
    public bool IsStopped = false;
    public float GroundingEpsilon;
    public int NumPackages = 1;
    public Collider Collider;
    public List<GameObject> Packages;
    public int PackagesShown = 0;

    [Header("Animation")]
    public Animator Animator;
    private bool _groundedLastFrame = false;
    private bool _groundedThisFrame = true;
    private bool _isAirborne = false;
    private float _distToGround;
    public TMP_Text PackagesText;
    public GameObject PackagesTextPanel;

    private AudioSource AudioSource;
    [Header("Audio")]
    public AudioSource BarkSource;
    public AudioClip Bark1;
    public AudioClip Bark2;

    public void SetNumPackages(int n)
    {
        NumPackages = n;
        ShowNPackagesAndText(n);
    }

    public void ShowNPackagesAndText(int n = 1)
    {
        // Activate pacakge gameobjects
        for (int i = 0; i < Packages.Count; i++)
        {
            if (i < n)
            {
                Packages[i].SetActive(true); 
            } else
            {
                Packages[i].SetActive(false);
            }
        }

        // Set text
        PackagesShown = n;
        PackagesText.text = n.ToString();
        if (n <= 1)
        {
            PackagesTextPanel.SetActive(false);
        } else
        {
            PackagesTextPanel.SetActive(true);
        }
    }

    public void ShowAllPackages()
    {
        foreach (GameObject p in Packages)
        {
            p.SetActive(true);
        }
        PackagesShown = Packages.Count;
    }

    public void HideAllPackages()
    {
        foreach (GameObject p in Packages)
        {
            p.SetActive(false);
        }
        PackagesShown = 0;
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
        AudioManager.EnrollSFXSource(AudioSource);
        AudioManager.EnrollSFXSource(BarkSource);
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
                    res += -SceneParent.transform.forward*1.01f;
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
                    res += -SceneParent.transform.right*1.01f;
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
                    res += SceneParent.transform.right*1.01f;
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
                    res += SceneParent.transform.forward*1.01f;
                }
            }
        }
        // Debug.Log(res);
        // Debug.Log(transform.forward.z);
        // forward
        
        if (!IsGrounded()){
            res = res*1.1f;
        }
        return res;
    }

    public void EnemyCheck()
    {
        int windLayerMask = 1 << 18; // Wind layer
        int buildingLayerMask = 1 << 9; // Building layer
        int envBuildingLayerMask = 1 << 13; // Environment Buildings
        int layerMask = windLayerMask | buildingLayerMask | envBuildingLayerMask;
        RaycastHit hit;

        Vector3 res = Vector3.zero;

        // Check if wind plane present in 4 cardinal directions
        if (Physics.Raycast(Collider.bounds.center, SceneParent.transform.forward, out hit, Mathf.Infinity, layerMask)) {
            if (hit.transform.CompareTag("enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyAI>().LineOfSightCheck(Collider.bounds.center.y);
            }
        }
        if (Physics.Raycast(Collider.bounds.center, SceneParent.transform.right, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyAI>().LineOfSightCheck(Collider.bounds.center.y);
            }
        }
        if (Physics.Raycast(Collider.bounds.center, -SceneParent.transform.right, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyAI>().LineOfSightCheck(Collider.bounds.center.y); 
            }
        }
        if (Physics.Raycast(Collider.bounds.center, -SceneParent.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("enemy"))
            {
                hit.transform.gameObject.GetComponent<EnemyAI>().LineOfSightCheck(Collider.bounds.center.y);
            }
        }
    }

    public bool IsGrounded()
    {
        // Debug.DrawLine(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.center - Vector3.up * (_distToGround + 0.2f), Color.red, 300f);
        return Physics.Raycast(Collider.bounds.center, -Vector3.up, _distToGround + GroundingEpsilon);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float barkRV = Random.Range(0.0f, 500.0f);
        if (barkRV > 499)
        {
            if (Random.Range(0.0f, 1.0f) >= 0.5)
            {
                BarkSource.PlayOneShot(Bark1);
            } else
            {
                BarkSource.PlayOneShot(Bark2);
            }
        }

        EnemyCheck();

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
                AudioSource.Pause();
            }
        } else
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.Play();
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
        GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().packagesLost += NumPackages;
        DeleteDog();
    }

    public void DeleteDog()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 5.0f);
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
