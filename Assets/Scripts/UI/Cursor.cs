using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{

    public float OFFSET_X;
    public float OFFSET_Y;
    public Sprite BaseCursor;
    public Sprite HoldCursor;

    Image cursorImage;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;
        cursorImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Cursor.visible = false;
        Vector2 cursorPosition = Input.mousePosition;
        Vector2 adjustedPosition = new Vector2(cursorPosition.x + OFFSET_X, cursorPosition.y + OFFSET_Y);
        transform.position = adjustedPosition;

        if (BuildManager.Instance.CurrBuilding != null)
        {
            cursorImage.sprite = HoldCursor;
        } else
        {
            cursorImage.sprite = BaseCursor;
        }
    }

}
