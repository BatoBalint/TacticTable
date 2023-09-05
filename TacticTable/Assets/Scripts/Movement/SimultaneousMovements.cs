using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
