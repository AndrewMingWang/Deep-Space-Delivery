using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    [Header("Info")]
    public List<int> Seen = new List<int>();

    private void Start()
    {
        AudioManager.EnrollSFXSource(GetComponent<AudioSource>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (!Seen.Contains(other.gameObject.GetInstanceID()))
            {
                Seen.Add(other.gameObject.GetInstanceID());

                // Set unit movement direction
                other.gameObject.GetComponent<Dog>().TargetDirection = transform.parent.forward;

                // Stop and Animate unit
                other.gameObject.GetComponent<Dog>().StopPlayer();
                other.gameObject.GetComponent<Dog>().Animator.SetTrigger("stop");
                StartCoroutine(RecenterDog(other.transform, transform.position, 0.1f));

                // SFX
                GetComponent<AudioSource>().pitch = Random.Range(0.99f, 1.01f);
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private IEnumerator RecenterDog(Transform dogTransform, Vector3 targetPos, float length)
    {
        // print(GameStateManager.Instance._fast);
        Vector3 startPos = dogTransform.position;
        int numSteps = 100;
        for (int i = 1; i < numSteps+1; i++)
        {
            if(GameStateManager.Instance._fast){
                i++;
            }
            dogTransform.position = Vector3.Lerp(startPos, targetPos, (float) i / numSteps);
            yield return new WaitForSeconds((float) length / numSteps);
        }
    }
     
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Seen.Remove(other.gameObject.GetInstanceID());
        }
    }
}
