using UnityEngine;

public class SwitchMovement : Movement
{
    [SerializeField]
    private GameObject otherDisk;
    [SerializeField]
    private Vector3 otherStartPos;
    [SerializeField]
    private Vector3 otherEndPos;

    public SwitchMovement(GameObject disk, GameObject otherDisk, Vector3 startPos, Vector3 endPos, Vector3 otherStartPos, Vector3 otherEndPos)
        : base(disk, startPos, endPos)
    {
        this.otherDisk = otherDisk;
        this.otherStartPos = otherStartPos;
        this.otherEndPos = otherEndPos;
    }

    public override void Animate(float time)
    {
        time = Mathf.Clamp01(time);

        Vector3 newPosition = Vector3.Lerp(startPos, endPos, time);
        //Debug.Log("NEWPos: " + newPosition);
        Vector3 newOtherPosition = Vector3.Lerp(otherStartPos, otherEndPos, time);

        
        float distance = Vector3.Distance(newPosition, newOtherPosition);
        //Debug.Log(distance);

        


        if (distance < 1.5f)
        {
            return;
            /*float x = Mathf.Cos(time * 10);
            float y = Mathf.Sin(time * 10);


            disk.transform.position = new Vector3(x, y, 0);
            otherDisk.transform.position = new Vector3(x, y, 0);
*/
        }
        else
        {
            disk.transform.position = newPosition;
            otherDisk.transform.position = newOtherPosition;
        }
    }
}
