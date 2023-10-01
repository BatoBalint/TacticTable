using UnityEngine;

public class GaussianCurves : MonoBehaviour
{
    public int numberOfPoints = 100;
    public float standardDeviation = 1f;

    public GameObject startPosition;
    public GameObject endPosition;

    LineRenderer lineRenderer;

    void Update()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints * 2; // Double the points to accommodate both curves

        DrawGaussianCurves(startPosition.transform.position, endPosition.transform.position);
    }

    void DrawGaussianCurves(Vector3 start, Vector3 end)
    {
        float interval = 1f / numberOfPoints; // Adjust interval based on the range you want to cover

        float midpointX = (start.x + end.x) * 0.5f; // Calculate midpoint of x-coordinates

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i * interval;
            float x = Mathf.Lerp(start.x, end.x, t); // Interpolate x between start and end

            // Right-side up curve with peak between half the two points
            float y1 = Mathf.Lerp(start.y, end.y, t);
            y1 += GaussianFunction(x, midpointX, standardDeviation); // Add Gaussian function to y

            // Upside down curve (subtract Gaussian function from y)
            float y2 = Mathf.Lerp(start.y, end.y, t);
            y2 -= GaussianFunction(x, midpointX, standardDeviation); // Subtract Gaussian function from y

            // Set positions for both curves
            lineRenderer.SetPosition(i, new Vector3(x, y1, 0f)); // Right-side up curve
            lineRenderer.SetPosition(i + numberOfPoints, new Vector3(x, y2, 0f)); // Upside down curve
        }
    }

    public float GaussianFunction(float x, float mean, float stdDev)
    {
        float exponent = -(Mathf.Pow(x - mean, 2) / (2 * Mathf.Pow(stdDev, 2)));
        return Mathf.Exp(exponent) / (stdDev * Mathf.Sqrt(2 * Mathf.PI));
    }
}
