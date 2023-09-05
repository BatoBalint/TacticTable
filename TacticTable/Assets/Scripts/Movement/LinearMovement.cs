using UnityEngine;

public class LinearMovement : Movement
{
    private GameObject disk;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    public LinearMovement(GameObject disk, Vector3 startPos, Vector3 endPos)
    {
        this.disk = disk;
        this.startPos = startPos;
        this.endPos = endPos;

        movementName = "Mozgás";
    }


    public override bool Animate(float time)
    {
        // Ensure the time is between 0 and 1
        time = Mathf.Clamp01(time);

        Vector3 newPosition = Vector3.Lerp(startPos, endPos, time);

        disk.transform.position = newPosition;

        return false;
    }
}
