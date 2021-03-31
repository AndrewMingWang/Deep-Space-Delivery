using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class Ruler : MonoBehaviour
{
    [System.Serializable]
    public struct HeightIndicator
    {
        [Range(1, 5)]
        public int height;
        public int pos;
    }

    [Header("XAxis")]
    public Vector2Int XAxisBounds;
    public Transform XAxis;
    public Transform XTicksParent;
    public Transform XHeightsParent;

    [Header("Max 5 indicators")]
    public HeightIndicator[] XAxisHeightIndicators;

    [Header("ZAxis")]
    public Vector2Int ZAxisBounds;
    public Transform ZAxis;
    public Transform ZTicksParent;
    public Transform ZHeightsParent;

    [Header("Max 5 indicators")]
    public HeightIndicator[] ZAxisHeightIndicators;



    // Update is called once per frame
    void Update()
    {
        // ------------ XAXIS --------------
        int NumXTicks = XAxisBounds.y - XAxisBounds.x + 1;

        // Position and scale axis
        Vector3 XScale = XAxis.localScale;
        XScale.x = XAxisBounds.y - XAxisBounds.x;
        Vector3 XCenter = XAxis.localPosition;
        XCenter.x = ((float) XAxisBounds.y + XAxisBounds.x) / 2;

        XAxis.localScale = XScale;
        XAxis.localPosition = XCenter;

        // Position and set numbers on ticks
        XTicksParent.localScale = new Vector3(
            1f / XAxis.localScale.x,
            1f / XAxis.localScale.y,
            1f / XAxis.localScale.z
        );
        XHeightsParent.localScale = new Vector3(
            1f / XAxis.localScale.x,
            1f / XAxis.localScale.y,
            1f / XAxis.localScale.z
        );

        // Hide all ticks
        foreach (Transform tick in XTicksParent)
        {
            tick.gameObject.SetActive(false);
        }

        // Unhide and position relevant ticks
        Vector3 start = XAxisBounds.x * Vector3.right - XAxis.localPosition;
        for (int i = 0; i < NumXTicks; i++)
        {
            Transform tick = XTicksParent.GetChild(i);

            tick.gameObject.SetActive(true);
            tick.localPosition = start + i * Vector3.right;

            int value = i + XAxisBounds.x;
            if (value == 0)
            {
                tick.gameObject.SetActive(false);
            } else
            {
                tick.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = (value).ToString();
            }
        }

        // Height Indicators
        foreach (Transform hi in XHeightsParent)
        {
            hi.gameObject.SetActive(false);
        }

        // Position Height indicators
        for (int j = 0; j < XAxisHeightIndicators.Length; j++)
        {
            HeightIndicator hi = XAxisHeightIndicators[j];
            Transform hiTransform = XHeightsParent.GetChild(j);
            hiTransform.gameObject.SetActive(true);
            Transform ticksParent = hiTransform.GetChild(0);

            // Set HI height and position
            Vector3 hiScale = hiTransform.localScale;
            hiScale.y = hi.height;
            Vector3 hiPos = new Vector3(hi.pos, (float)hi.height / 2f, 0);
            hiTransform.localScale = hiScale;
            hiTransform.localPosition = hiPos - XAxis.localPosition;

            ticksParent.localScale = new Vector3(
                1f / hiTransform.localScale.x,
                1f / hiTransform.localScale.y,
                1f / hiTransform.localScale.z
            );

            foreach (Transform tick in ticksParent)
            {
                tick.gameObject.SetActive(false);
            }

            // Set Tick positions
            int NumYTicks = hi.height;
            for (int k = 0; k < NumYTicks; k++)
            {
                Transform tick = ticksParent.GetChild(k);
                tick.gameObject.SetActive(true);
                tick.localPosition = new Vector3(0, k + 1, 0);
                tick.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = (k + 1).ToString();
            }
        }

        // ------------ ZAXIS --------------
        int NumZTicks = ZAxisBounds.y - ZAxisBounds.x + 1;

        // Position and scale axis
        Vector3 ZScale = ZAxis.localScale;
        ZScale.z = ZAxisBounds.y - ZAxisBounds.x;
        Vector3 ZCenter = ZAxis.localPosition;
        ZCenter.z = ((float)ZAxisBounds.y + ZAxisBounds.x) / 2;

        ZAxis.localScale = ZScale;
        ZAxis.localPosition = ZCenter;

        // Position and set numbers on ticks
        ZTicksParent.localScale = new Vector3(
            1f / ZAxis.localScale.x,
            1f / ZAxis.localScale.y,
            1f / ZAxis.localScale.z
        );

        // Hide all ticks
        foreach (Transform tick in ZTicksParent)
        {
            tick.gameObject.SetActive(false);
        }

        // Unhide and position relevant ticks
        Vector3 ZStart = ZAxisBounds.x * Vector3.forward - ZAxis.localPosition;
        for (int i = 0; i < NumZTicks; i++)
        {
            Transform tick = ZTicksParent.GetChild(i);

            tick.gameObject.SetActive(true);
            tick.localPosition = ZStart + i * Vector3.forward;

            int value = i + ZAxisBounds.x;
            if (value == 0)
            {
                tick.gameObject.SetActive(false);
            }
            else
            {
                tick.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = (value).ToString();
            }
        }

        // Height Indicators
        foreach (Transform hi in ZHeightsParent)
        {
            hi.gameObject.SetActive(false);
        }

        // Position Height indicators
        for (int j = 0; j < ZAxisHeightIndicators.Length; j++)
        {
            HeightIndicator hi = ZAxisHeightIndicators[j];
            Transform hiTransform = ZHeightsParent.GetChild(j);
            hiTransform.gameObject.SetActive(true);
            Transform ticksParent = hiTransform.GetChild(0);

            // Set HI height and position
            Vector3 hiScale = hiTransform.localScale;
            hiScale.y = hi.height;
            Vector3 hiPos = new Vector3(0, (float)hi.height / 2f, hi.pos);
            hiTransform.localScale = hiScale;
            hiTransform.localPosition = hiPos - ZAxis.localPosition;

            ticksParent.localScale = new Vector3(
                1f / hiTransform.localScale.x,
                1f / hiTransform.localScale.y,
                1f / hiTransform.localScale.z
            );

            foreach (Transform tick in ticksParent)
            {
                tick.gameObject.SetActive(false);
            }

            // Set Tick positions
            int NumYTicks = hi.height;
            for (int k = 0; k < NumYTicks; k++)
            {
                Transform tick = ticksParent.GetChild(k);
                tick.gameObject.SetActive(true);
                tick.localPosition = new Vector3(0, k + 1, 0);
                tick.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = (k + 1).ToString();
            }
        }
    }
}
