using UnityEngine;

public class LinearMovement : Movement
{
    public LinearMovement(GameObject disk, Vector3 startPos, Vector3 endPos) : base(disk, startPos, endPos)
    {
    }


    public override void Animate(float time)
    {
        // Ensure the time is between 0 and 1
        time = Mathf.Clamp01(time);

        Vector3 newPosition = Vector3.Lerp(startPos, endPos, time);

        disk.transform.position = newPosition;
    }
}
