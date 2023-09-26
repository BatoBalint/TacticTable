using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public string movementName = "undefined";

    // returns true if the Animation is finished
    // more freedom in future Movements
    public abstract bool Animate(float time);

    public abstract Dictionary<DiskScript, Vector3> GetEndPositions();

    public abstract string ToJSON();
}
