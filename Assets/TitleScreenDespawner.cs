using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenDespawner : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -10)
        {
            gameObject.SetActive(false);
        }
    }
}
