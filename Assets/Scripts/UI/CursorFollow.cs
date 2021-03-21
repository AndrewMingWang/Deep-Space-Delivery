using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{

    private const float OFFSET_X = 0.25f;
    private const float OFFSET_Y = -0.4f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPosition = Input.mousePosition;
        Vector2 adjustedPosition = new Vector2(cursorPosition.x + OFFSET_X, cursorPosition.y + OFFSET_Y);
        transform.position = adjustedPosition;
    }

}
