using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEditEventSystem : MonoBehaviour
{
    public static AnimEditEventSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public event Action<AnimationMode> onAnimMenuButtonClick;
    public void AnimMenuButtonClick(AnimationMode mode)
    {
        if (onAnimMenuButtonClick != null)
        {
            onAnimMenuButtonClick(mode);
        }
    }

    public event Action<DiskScript> onDiskPositionChange;
    public void DiskPositionChange(DiskScript diskScript)
    {
        if (onDiskPositionChange != null)
        {
            onDiskPositionChange(diskScript);
        }
    }
}
