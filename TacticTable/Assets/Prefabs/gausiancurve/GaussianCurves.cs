using UnityEngine;
using System.Collections;

public class GaussianCurves : MonoBehaviour
{
    public int numberOfPoints = 100;
    public float standardDeviation = 1f;
    public float test = 0f;
    public GameObject startPosition;
    public GameObject endPosition;

    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;
    Vector3[] tomb = new Vector3[100]; // Creates an integer array of size 5
    Vector3[] tomb2 = new Vector3[100]; // Creates an integer array of size 5


    void Update()
    {
        // Check for mouse button click (left mouse button is 0)
        if (Input.GetMouseButtonDown(0))
        {
            Onclick();
        }
    }
    void Onclick()
    {
        // lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints;

        //lineRenderer2 = GetComponent<LineRenderer>();
        lineRenderer2.positionCount = numberOfPoints;

        DrawGaussianCurvesalmost(startPosition.transform.position, endPosition.transform.position);
        StartCoroutine(MoveThroughWaypoints(tomb,startPosition));
        StartCoroutine(MoveThroughWaypoints(tomb2,endPosition));
    }
    void DrawGaussianCurvesalmost(Vector3 start, Vector3 end)
    {
        float interval = 1f / numberOfPoints; // Adjust interval based on the range you want to cover

        Vector3 midpoint = (start + end) * 0.5f; // Calculate midpoint of both x and y coordinates

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i * interval;
            Vector3 pointOnLine = Vector3.Lerp(start, end, t); // Point on the line between start and end

            // Calculate Gaussian offset based on the distance from the midpoint
            float distanceToMidpoint = Vector3.Distance(midpoint, pointOnLine);
            float gaussianValue = GaussianFunction(distanceToMidpoint, 0f, standardDeviation);

            // Calculate perpendicular vector to the line
            // Vector3 perpendicular = new Vector3(midpoint.y - pointOnLine.y, pointOnLine.x - midpoint.x, 0f).normalized;
            Vector3 lineDirection = (end - start).normalized;
            Vector3 perpendicular = new Vector3(-lineDirection.y, lineDirection.x, 0f).normalized;


            // Calculate points for the Gaussian curves
            Vector3 curvePoint1 = pointOnLine + perpendicular * gaussianValue;
            Vector3 curvePoint2 = pointOnLine - perpendicular * gaussianValue;

            // Set positions for both curves
            lineRenderer.SetPosition(i, curvePoint1); // Right-side up curve
            lineRenderer2.SetPosition(numberOfPoints - i - 1, curvePoint2); // Upside down curve

            tomb[i] = curvePoint1;
            tomb2[numberOfPoints - i - 1] = curvePoint2;
        }
        

    }
    float speed = 40f;
    IEnumerator MoveThroughWaypoints(Vector3 [] tomb, GameObject obj)
    {
        for (int i = 0; i < tomb.Length - 1; i++)
        {
            float t = 0f;
            Vector3 startPos = tomb[i];
            Vector3 endPos = tomb[i + 1];

            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                obj.transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }
    }

    public float GaussianFunction(float x, float mean, float stdDev)
    {
        float exponent = -(Mathf.Pow(x - mean, 2) / (2 * Mathf.Pow(stdDev, 2)));
        return Mathf.Exp(exponent) / (stdDev * Mathf.Sqrt(2 * Mathf.PI));
    }
}
