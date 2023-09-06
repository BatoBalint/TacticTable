using System.Collections.Generic;
using UnityEngine;

public class TestMovement : Movement
{
    public TestMovement()
    {
    }

    public override bool Animate(float time)
    {
        float percentage = Mathf.Clamp01(time); //percentage is between 0 and 1
        Debug.Log("Animation progress: " + Mathf.RoundToInt(percentage * 100) + "%");

        return false;
    }

    public override Dictionary<DiskScript, Vector3> GetEndPositions()
    {
        return new Dictionary<DiskScript, Vector3>();
    }
}
