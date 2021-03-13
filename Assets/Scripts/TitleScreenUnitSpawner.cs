using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUnitSpawner : MonoBehaviour
{
    [Header("Spawned Objects")]
    public List<GameObject> ToBeSpawned = new List<GameObject>();

    [Header("Bounds")]
    public Vector2 XBounds;
    public Vector2 ZBounds;
    public Vector2 SizeBounds;
    public Vector3 TorqueBounds;

    public Vector2 TimeBetweenSpawnBounds;

    public bool KeepSpawning;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(KeepSpawning)
        {
            float xPos = Random.Range(XBounds.x, XBounds.y);
            float zPos = Random.Range(ZBounds.x, ZBounds.y);
            float yPos = transform.position.y;

            Vector3 pos = new Vector3(xPos, yPos, zPos);

            Vector3 torque = new Vector3(
                Random.Range(-TorqueBounds.x, TorqueBounds.x),
                Random.Range(-TorqueBounds.y, TorqueBounds.y),
                Random.Range(-TorqueBounds.z, TorqueBounds.z)
                );

            int objIdx = Random.Range(0, ToBeSpawned.Count);

            // Spawn
            GameObject newSpawn = Instantiate(ToBeSpawned[objIdx],
                pos,
                Quaternion.identity,
                transform);

            // Apply rotation
            Rigidbody newRB = newSpawn.GetComponent<Rigidbody>();
            newRB.AddTorque(torque, ForceMode.VelocityChange);

            // Apply size
            float scale = Random.Range(SizeBounds.x, SizeBounds.y);
            newSpawn.transform.localScale *= scale;


            // Wait until next spawn
            float waitTime = Random.Range(TimeBetweenSpawnBounds.x, TimeBetweenSpawnBounds.y);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
}
