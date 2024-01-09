using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float Lifetime = 0.5f;
    public int Points = 0;
    public float PointDensity = 1f;
    public float ArcSize = 0.1f;
    public float PointOffset = 0.2f;

    public Vector3 StartPoint;
    public Vector3 EndPoint;
    private Vector3 Direction;

    private float Alpha = 0f;
    private int PointCount = 2;

    public Vector3 rotate(Vector3 v, float delta)
    {
        delta = delta * Mathf.Deg2Rad;
        return new Vector3(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta),
            v.z
        );
    }

    void Start()
    {
        StartPoint = transform.position;

        if (Points == 0)
            PointCount = Mathf.FloorToInt(Vector3.Distance(StartPoint, EndPoint) * PointDensity);
        else
            PointCount = Points;
        PointCount = Mathf.Max(PointCount, 2);
        lineRenderer.positionCount = PointCount;

        lineRenderer.SetPosition(0, StartPoint);
        lineRenderer.SetPosition(PointCount - 1, EndPoint);

        Direction = Vector3.Normalize(EndPoint - StartPoint);

        Vector3 point;
        for (int i = 0; i < PointCount; i++)
        {
            if (!(i == 0 || i == PointCount - 1))
            {
                point = Vector3.Lerp(StartPoint, EndPoint, Mathf.Round(i) / Mathf.Round(PointCount - 1));
                point += Direction * Random.Range(0 - PointOffset, PointOffset);
                point += rotate(Direction * Random.Range(0 - ArcSize, ArcSize), 90f); 

                lineRenderer.SetPosition(i, point);
            }
        }

        Destroy(gameObject, Lifetime);
    }

    void Update()
    {
        Alpha += Time.deltaTime / Lifetime;
        lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, 1f - (Alpha * Alpha));
        lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, 1f - (Alpha * Alpha));
    }
}
