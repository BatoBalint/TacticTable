using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurves 
{
    public Vector3[] Points;

    public BezierCurves()
    {
        Points = new Vector3[4];
    }
    public BezierCurves(Vector3[] points)
    {
        Points = points;
    }
    public Vector3 StartPosition
    {
        get
        {
            return Points[0];
        }
    }

    public Vector3 EndPosition { get { return Points[3]; } }

    public Vector3 GetSegment(float Time)
    {
        Time = Mathf.Clamp01(Time);
        float time = 1 -Time;
        return (time * time * time * Points[0])
            + (3 * time * time * time * Points[1])
            + (3 * time * time * time * Points[2])
            + (Time * Time * Time * Points[3]);
    }
    public Vector3[] GetSegments(int Subdivisions)
    {
        Vector3[] segments = new Vector3[Subdivisions];

        float time;
        for (int i = 0; i < Subdivisions; i++)
        {
            time = (float)i / Subdivisions;
            segments[i] = GetSegment(time);
        }
        return segments;
    }
}
