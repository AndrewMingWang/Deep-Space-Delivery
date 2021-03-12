using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagesSpawner : MonoBehaviour
{
    [Header("State")]
    public static PackagesSpawner Instance;
    public bool DoneSpawning = false;
    public bool DoneLanding = false;
    public int NumLanded = 0;
    public List<Package> SpawnedPackages = new List<Package>();
    private int _spawnedPackagesIdx = 0;

    [Header("Spawning Parameters")]
    public Transform SpawnPoint;
    public GameObject PackagePrefab;
    public int NumPackages;
    public float PushForce;
    public float SecondsBetweenSpawn;

    [Header("Randomness")]
    public Vector3 MaxSpawnPosOffset;
    public Vector3 MaxTargetPosOffset;
    public Vector3 MaxTorque;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnPackages());
    }

    private void FixedUpdate()
    {
        if (!DoneLanding)
        {
            if (NumLanded == NumPackages)
            {
                DoneLanding = true;
                // After all packages land prevent them from
                // colliding with each other.
                Physics.IgnoreLayerCollision(16, 16);
            }
        }
    }

    private IEnumerator SpawnPackages()
    {
        DoneLanding = false;
        DoneSpawning = false;
        for (int i = 0; i < NumPackages; i++)
        {
            Vector3 spawnPosOffset = new Vector3(
                Random.Range(-MaxSpawnPosOffset.x, MaxSpawnPosOffset.x),
                Random.Range(-MaxSpawnPosOffset.y, MaxSpawnPosOffset.y),
                Random.Range(-MaxSpawnPosOffset.z, MaxSpawnPosOffset.z) 
            );

            Vector3 targetPosOffset = new Vector3(
                Random.Range(-MaxTargetPosOffset.x, MaxTargetPosOffset.x),
                Random.Range(-MaxTargetPosOffset.y, MaxTargetPosOffset.y),
                Random.Range(-MaxTargetPosOffset.z, MaxTargetPosOffset.z)
            );

            Vector3 torque = new Vector3(
                Random.Range(-MaxTorque.x, MaxTorque.x),
                Random.Range(-MaxTorque.y, MaxTorque.y),
                Random.Range(-MaxTorque.z, MaxTorque.z)
            );

            Vector3 spawnPos = SpawnPoint.transform.position + spawnPosOffset;
            Vector3 targetPos = transform.position + targetPosOffset;

            GameObject NewPackage = Instantiate(PackagePrefab, spawnPos, Quaternion.identity, transform);
            SpawnedPackages.Add(NewPackage.GetComponent<Package>());

            Vector3 dir = targetPos - spawnPos;

            NewPackage.GetComponent<Rigidbody>().AddForce(PushForce * dir.normalized, ForceMode.VelocityChange);
            NewPackage.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.VelocityChange);

            yield return new WaitForSecondsRealtime(SecondsBetweenSpawn);
        }
        DoneSpawning = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("pickup");

            UnitMovement unit = other.gameObject.GetComponent<UnitMovement>();
            if (!unit.PackageShown)
            {
                StartCoroutine(PickupPackage(unit));
                
            }
        }
    }

    private IEnumerator PickupPackage(UnitMovement unit)
    {
        yield return new WaitForSecondsRealtime(0.15f);
        unit.ShowPackage();
        if (_spawnedPackagesIdx <= SpawnedPackages.Count - 1)
        {
            SpawnedPackages[_spawnedPackagesIdx].gameObject.SetActive(false);
            _spawnedPackagesIdx += 1;
        }
    }

}
