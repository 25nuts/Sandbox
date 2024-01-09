using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public LineRenderer CircleRenderer;

    public int Steps = 100;
    public float Radius = 1f;
    public float Thickness = 1f;
    public float Velocity = 1f;

    public void Awake()
    {
        CircleRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        DrawCircle(Steps, Radius, Thickness);
    }

    void Update()
    {
        DrawCircle(Steps, Radius, Thickness);
        Radius += 2f * Velocity * Time.deltaTime;
        Thickness -= 1f * Velocity * Time.deltaTime;
        Velocity -= Velocity * Time.deltaTime;
        Velocity = Mathf.Max(Velocity, 0f);
        if (Thickness <= 0.05f)
        {
            Destroy(this.gameObject);
        }
    }

    public void DrawCircle(int steps, float radius, float thickness)
    {
        CircleRenderer.positionCount = steps;
        CircleRenderer.startWidth = thickness;
        CircleRenderer.endWidth = thickness;

        for(int currentStep = 0; currentStep < steps; currentStep++)
        {
            float progress = (float)currentStep / (steps - 1);

            float currentRadian = progress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0f);

            CircleRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
