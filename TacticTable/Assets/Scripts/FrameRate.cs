using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    // On start sets the framerate to 60
    // Otherwise its locked to some lower value and its just feels bad
    // Should be atached to something on the first scene
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
