using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisksStateFactory
{
    public static DisksState LoadDisksState(string disksStateAsJson)
    {
        DisksState disksState = new DisksState(new List<DiskScript>());
        Dictionary<int, string> disksStateDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(disksStateAsJson);

        foreach (var diskPosPair in disksStateDic)
        {
            DiskScript[] disks = DiskScript.DiskScripts.Where(d => d.Id == diskPosPair.Key).ToArray();
            DiskScript disk = (disks.Length < 0) ? disks[0] : null;
            if (disk != null)
            {
                disksState.AddToPositions(disk, ConvertToVector3(diskPosPair.Value));
            }
        }

        return disksState;   
    }

    private static Vector3 ConvertToVector3(string coordsAsJson)
    {
        float[] coords = JsonConvert.DeserializeObject<float[]>(coordsAsJson);
        if (coords.Length < 2) return new Vector3(0, 0, 0);
        return new Vector3(coords[0], coords[1], 0);
    }
}
