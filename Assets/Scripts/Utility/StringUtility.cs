using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StringUtility : MonoBehaviour
{

    public static StringUtility Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void TypeTextEffect(TMP_Text displayText, string contentText, float speedMultiplier)
    {
        Instance.StartCoroutine(Instance.TypeText(displayText, contentText, speedMultiplier));
    }

    private IEnumerator TypeText(TMP_Text displayText, string contentText, float speedMultiplier)
    {
        displayText.text = "";
        foreach (char letter in contentText)
        {
            displayText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f / speedMultiplier);
        }
    }

}
