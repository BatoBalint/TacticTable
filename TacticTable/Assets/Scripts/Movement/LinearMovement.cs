using Newtonsoft.Json;
using System.Collections.Generic;
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

    public override Dictionary<DiskScript, Vector3> GetEndPositions()
    {
        Dictionary<DiskScript, Vector3> endPositions = new Dictionary<DiskScript, Vector3>();
        endPositions.Add(disk.GetComponent<DiskScript>(), endPos);
        return endPositions;
    }

    public override string ToJSON()
    {
        Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
        dic.Add("movementType", "linear");
        dic.Add("diskId", disk.GetComponent<DiskScript>().Id);
        dic.Add("startPos", new float[] { startPos.x, startPos.y });
        dic.Add("endPos", new float[] { endPos.x, endPos.y });

        return JsonConvert.SerializeObject(dic);
    }
}
