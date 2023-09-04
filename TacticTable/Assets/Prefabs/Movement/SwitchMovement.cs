using System.Collections.Generic;
using UnityEngine;

public class SwitchMovement : Movement
{

    private GameObject otherDisk;
    private Vector3 otherStartPos;
    private Vector3 otherEndPos;
    [SerializeField]
    private float arc = 0.1f;
  
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
       
        var centerPivot = (startPos + endPos) * 0.5f;
        var centerPivotfel = (otherStartPos + otherEndPos) * 0.5f;

        centerPivot -= new Vector3(0, -1 *arc);
        centerPivotfel -= new Vector3(0, 1*arc);

        var startRelativeCenter = startPos - centerPivot;
        var endRelativeCenter = endPos - centerPivot;
        var startRelativeCenterfel = otherStartPos - centerPivotfel;
        var endRelativeCenterfel = otherEndPos- centerPivotfel;


        otherDisk.transform.position= Vector3.Slerp(startRelativeCenterfel, endRelativeCenterfel, time) + centerPivotfel;
        disk.transform.position=Vector3.Slerp(startRelativeCenter, endRelativeCenter, time) + centerPivot;
        

    }
      


}
