using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;

public class SimultaneousMovements : Movement
{
    [SerializeField]
    private List<Movement> movements = new List<Movement>();

    public SimultaneousMovements()
    {
    }
    
    public void AddMovement(Movement m)
    {
        movements.Add(m);
    }
    public List<Movement> GetMovements()
    {
        List<Movement> returnList = new List<Movement>();
        foreach(Movement m in movements)
        {
            returnList.Add(m);
        }
        return returnList;
    }

    public override bool Animate(float time)
    {
        foreach(Movement m in movements)
        {
            m.Animate(time);
        }

        return false;
    }

    public override Dictionary<DiskScript, Vector3> GetEndPositions()
    {
        Dictionary<DiskScript, Vector3> endPositions = new Dictionary<DiskScript, Vector3>();
        foreach (var move in movements)
        {
            endPositions.AddRange(move.GetEndPositions());
        }
        return endPositions;
    }

    public override string ToJSON()
    {
        Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
        dic.Add("movementType", "simultaneous");

        List<string> movementsAsJson = new List<string>();
        foreach (var move in movements)
        {
            movementsAsJson.Add(move.ToJSON());
        }
        dic.Add("movements", JsonConvert.SerializeObject(movementsAsJson));

        return JsonConvert.SerializeObject(dic);
    }
}
