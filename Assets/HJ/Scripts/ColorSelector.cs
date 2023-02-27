using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorSelector : MonoBehaviour
{

    public void SetRandomStyle()
    {
        int styleCount = transform.childCount;
        int rand = Random.Range(0, styleCount);

        InitStyle(styleCount);
        
        transform.GetChild(rand).gameObject.SetActive(true);
    }

    private void InitStyle(int styleCount)
    {
        for (int i = 0; i < styleCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
