using UnityEngine;

public class GaussianCurves : MonoBehaviour
{
    public int numberOfPoints = 100;
    public float standardDeviation = 1f;
    public float test = 0f;
    public GameObject startPosition;
    public GameObject endPosition;

    LineRenderer lineRenderer;

    void Update()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints * 2; // Double the points to accommodate both curves
        
         DrawGaussianCurvesalmost(startPosition.transform.position, endPosition.transform.position);
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
            lineRenderer.SetPosition(i + numberOfPoints, curvePoint2); // Upside down curve
        }
    }
   
    public float GaussianFunction(float x, float mean, float stdDev)
    {
        float exponent = -(Mathf.Pow(x - mean, 2) / (2 * Mathf.Pow(stdDev, 2)));
        return Mathf.Exp(exponent) / (stdDev * Mathf.Sqrt(2 * Mathf.PI));
    }
}
