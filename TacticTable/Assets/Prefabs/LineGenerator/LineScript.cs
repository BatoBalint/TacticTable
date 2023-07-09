using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    LineRenderer lineRenderer;
    public int pointCount = 0;  // a vonal pontjainak darabja

    public void Awake()
    {
        // Megkeresi a gameObject-en a LineRenderer komponenst es elmenti kenyelem kedveert
        lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Ez a fuggvennyel lehet uj pontot felvenni a lineRendererbe
    /// </summary>
    /// <param name="point"> A pozicio ahova tesszuk a pontot </param>
    /// <param name="minDstanceBetweenPoints"> Minimum tavolsag az elozo ponthoz kepest, 0.1 egesz pontos vonalat rajzol </param>
    public void AddPoint(Vector3 point, float minDstanceBetweenPoints)
    {
        // ha meg egy darab pont sincs akkor letrehoz 2 pontot csak hogy
        // azonnal latszodjon valami, igy tudsz rajzolni kis pontokat
        if (pointCount == 0)
        {
            lineRenderer.positionCount += 2;
            lineRenderer.SetPosition(pointCount, point - new Vector3(lineRenderer.startWidth / 2, 0, 0));
            lineRenderer.SetPosition(pointCount + 1, point + new Vector3(lineRenderer.startWidth / 2, 0, 0));
            pointCount += 2;
            return;
        }

        // ha nem huztuk eleg messze az egeret akkor ne rakjon uj pontot
        // ez optimalizacio miatt kell mert 100 fps-nel 100 pontot rakna masodpercenkent es az folosleges is meg eroforras igenyes
        if (Vector2.Distance(point, lineRenderer.GetPosition(pointCount - 1)) < minDstanceBetweenPoints)
        {
            return;
        }

        // elosszor megmondjuk a lineRenderer-nek hogy bovitjuk a pontListat
        lineRenderer.positionCount++;
        // a lista utolso elemere beallitjuk a kapott poziciot
        lineRenderer.SetPosition(pointCount, point);

        pointCount++;
    }
}
