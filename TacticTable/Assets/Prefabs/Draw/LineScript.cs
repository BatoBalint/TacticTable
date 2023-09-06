using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;

    public void SetPosition(Vector2 pos)
    {
        if (!CanAppend(pos))
        {
            return;
        }

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);
    }

    private bool CanAppend(Vector2 pos)
    {
        if (_renderer.positionCount == 0)
        {
            return true;
        }

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > DrawManager.RESOLUTION;
    }

    // Function made by Balint
    public void Test()
    {
        _renderer.SetPositions(new Vector3[0]);
        _renderer.positionCount = 0;
    }
}
