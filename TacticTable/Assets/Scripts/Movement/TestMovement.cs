using UnityEngine;

public class TestMovement : Movement
{
    public TestMovement(GameObject disk, Vector3 startPos, Vector3 endPos) : base(disk, startPos, endPos)
    {
    }

    public override void Animate(float time)
    {
        float percentage = Mathf.Clamp01(time); //percentage is between 0 and 1
        Debug.Log("Animation progress: " + Mathf.RoundToInt(percentage * 100) + "%");
        
    }
}
