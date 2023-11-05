using System.Collections.Generic;
using UnityEngine;

public class SwitchMovement : Movement
{
    private GameObject disk;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    public int numberOfPoints = 100;
    public float standardDeviation = 1f;
    Vector3[] tomb = new Vector3[100]; // Creates an integer array of size 5
    Vector3[] tomb2 = new Vector3[100]; // Creates an integer array of size 5

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
        Debug.Log("usethis here calculate the arc");
        return Mathf.Pow(distance - 1.3f, 1.39f);
    }

    public override bool Animate(float time)
    {
        /*time = Mathf.Clamp01(time);

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
        */
        time = Mathf.Clamp01(time);
        float interval = 1f / numberOfPoints; // Adjust interval based on the range you want to cover

        Vector3 midpoint = (disk.transform.position + otherDisk.transform.position) * 0.5f; // Calculate midpoint of both x and y coordinates
        Vector3 pointOnLine = Vector3.Lerp(disk.transform.position, otherDisk.transform.position, time); // Point on the line between start and end

        // Calculate Gaussian offset based on the distance from the midpoint
        float distanceToMidpoint = Vector3.Distance(midpoint, pointOnLine);
        float gaussianValue = GaussianFunction(distanceToMidpoint, 0f, standardDeviation);

        // Calculate perpendicular vector to the line
        Vector3 lineDirection = (otherDisk.transform.position - disk.transform.position).normalized;
        Vector3 perpendicular = new Vector3(-lineDirection.y, lineDirection.x, 0f).normalized;

        // Calculate points for the Gaussian curves
        Vector3 curvePoint1 = pointOnLine + perpendicular * gaussianValue;
        Vector3 curvePoint2 = pointOnLine - perpendicular * gaussianValue;

        // Move the disks along the Gaussian curves
        otherDisk.transform.position = curvePoint1;
        disk.transform.position = curvePoint2;



        return false;
    }
    public float GaussianFunction(float x, float mean, float stdDev)
    {
        float exponent = -(Mathf.Pow(x - mean, 2) / (2 * Mathf.Pow(stdDev, 2)));
        return Mathf.Exp(exponent) / (stdDev * Mathf.Sqrt(2 * Mathf.PI));
    }

    public override Dictionary<DiskScript, Vector3> GetEndPositions()
    {
        Dictionary<DiskScript, Vector3> endPositions = new Dictionary<DiskScript, Vector3>();
        endPositions.Add(disk.GetComponent<DiskScript>(), endPos);
        endPositions.Add(otherDisk.GetComponent<DiskScript>(), otherEndPos);
        return endPositions;
    }
}
