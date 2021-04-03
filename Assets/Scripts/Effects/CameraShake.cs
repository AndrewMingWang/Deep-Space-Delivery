using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public void StartCameraShake(float magnitude, float duration)
    {
        StartCoroutine(DoCameraShake(magnitude, duration));
    }
     
    private IEnumerator DoCameraShake(float magnitude, float duration)
    {
        Vector3 startPosition = transform.localPosition;
        float secondsPassed = 0.0f;
        while (secondsPassed < duration)
        {
            float xShake = Random.Range(-0.1f, 0.1f) * magnitude;
            float yShake = Random.Range(-0.1f, 0.1f) * magnitude;
            transform.localPosition = new Vector3(xShake, yShake, transform.localPosition.z);
            secondsPassed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPosition;
    }

}
