using System.Collections.Generic;
using UnityEngine;

public class SwitchMovement : Movement
{
    private GameObject disk;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    private GameObject otherDisk;
    private Vector3 otherStartPos;
    private Vector3 otherEndPos;
    [SerializeField]
    private float arc = 1f;

    public SwitchMovement(GameObject disk, GameObject otherDisk, Vector3 diskPos, Vector3 otherDiskPos)
    {
        this.disk = disk;
        this.startPos = diskPos;
        this.endPos = otherDiskPos;

        this.otherDisk = otherDisk;
        this.otherStartPos = otherDiskPos;
        this.otherEndPos = diskPos;

        movementName = "Helycsere";

        arc = CalculateArc();
    }

    private float CalculateArc()
    {
        float distance = Vector3.Distance(startPos, otherStartPos);
        return Mathf.Pow(distance - 1.3f, 1.39f);
    }

    public override bool Animate(float time)
    {
        time = Mathf.Clamp01(time);

        var centerPivot = (startPos + endPos) * 0.5f;
        var centerPivotfel = (otherStartPos + otherEndPos) * 0.5f;

        centerPivot -= new Vector3(0, -1 * arc);
        centerPivotfel -= new Vector3(0, 1 * arc);

        var startRelativeCenter = startPos - centerPivot;
        var endRelativeCenter = endPos - centerPivot;
        var startRelativeCenterfel = otherStartPos - centerPivotfel;
        var endRelativeCenterfel = otherEndPos - centerPivotfel;

        otherDisk.transform.position = Vector3.Slerp(startRelativeCenterfel, endRelativeCenterfel, time) + centerPivotfel;
        disk.transform.position = Vector3.Slerp(startRelativeCenter, endRelativeCenter, time) + centerPivot;

        return false;
    }

    public override Dictionary<DiskScript, Vector3> GetEndPositions()
    {
        Dictionary<DiskScript, Vector3> endPositions = new Dictionary<DiskScript, Vector3>();
        endPositions.Add(disk.GetComponent<DiskScript>(), endPos);
        endPositions.Add(otherDisk.GetComponent<DiskScript>(), otherEndPos);
        return endPositions;
    }
}
