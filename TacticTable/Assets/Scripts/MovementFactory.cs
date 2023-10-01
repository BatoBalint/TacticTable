using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementFactory
{
    public static Movement LoadMovement(string movementAsJson)
    {
        Movement movement = null;

        Dictionary<string, string> movementDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(movementAsJson);
        string mode = movementDic["movementType"] ?? "";
        switch (mode)
        {
            case "linear":
                movement = CreateLinearMovement(movementDic);
                break;
            case "switch":
                movement = CreateSwitchMovement(movementDic);
                break;
            default:
                break;
        }
        return movement;
    }

    private static LinearMovement CreateLinearMovement(Dictionary<string, string> dic) 
    {
        LinearMovement movement;

        // Get id or terminate if can't
        int diskId = -1;
        if (!int.TryParse(dic["diskId"], out diskId)) return null;

        // Get disk or terminate if can't
        DiskScript[] disks = DiskScript.DiskScripts.Where(d => d.Id == diskId).ToArray();
        if (disks.Length == 0) return null;

        movement = new LinearMovement(
            disks[0].gameObject,
            ConvertToVector3(dic, "startPos"),
            ConvertToVector3(dic, "endPos")
            );

        return movement;
    }

    private static SwitchMovement CreateSwitchMovement(Dictionary<string, string> dic) 
    {
        SwitchMovement movement;

        // Get ids or terminate if can't
        string[] idStrings = dic["diskIds"].Split(",");
        if (idStrings.Length < 2) return null;
        int[] ids = { -1, -1 };
        int.TryParse(idStrings[0], out ids[0]);
        int.TryParse(idStrings[1], out ids[1]);

        // Get disks or terminate if can't
        DiskScript disk1 = DiskScript.DiskScripts.Where(d => d.Id == ids[0]).FirstOrDefault();
        if (disk1 == null) return null;
        DiskScript disk2 = DiskScript.DiskScripts.Where(d => d.Id == ids[1]).FirstOrDefault();
        if (disk2 == null) return null;

        movement = new SwitchMovement(
            disk1.gameObject,
            disk2.gameObject,
            ConvertToVector3(dic, "diskPos"),
            ConvertToVector3(dic, "otherDiskPos")
            );

        return movement;
    }

    private static Vector3 ConvertToVector3(Dictionary<string, string> dic, string key)
    {
        float x = 0f;
        float y = 0f;

        string[] coords = dic[key].Split(",");
        if (coords.Length > 0) float.TryParse(coords[0], out x);
        if (coords.Length > 1) float.TryParse(coords[1], out y);

        return new Vector3(x, y, 0);
    }
}
