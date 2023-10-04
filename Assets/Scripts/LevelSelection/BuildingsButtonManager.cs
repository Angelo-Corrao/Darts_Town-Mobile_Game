using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsButtonManager : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}
