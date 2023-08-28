using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;       
    }
}
