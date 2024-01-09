using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed;
    public float Velocity;

    public Vector2 Target;
    public Vector2 Target2 = new Vector2(-999f, -999f);
    public bool Lerp;

    [HideInInspector]
    Camera Cam;

    private void Awake()
    {
        Cam = GetComponent<Camera>();
    }

    public void LateUpdate()
    {
        if (Target2 != new Vector2(-999f, -999f))
        {
            Vector2 midPoint = Vector2.Lerp(Target, Target2, 0.5f);
            transform.position = new Vector3(midPoint.x, midPoint.y, transform.position.z);

            float yDistance = Mathf.Abs(Target.y - Target2.y);
            float distance = Vector2.Distance(Target, Target2);
            
            Cam.orthographicSize = (distance * ((yDistance / distance * 0.25f) + 0.25f)) + 5f;
        }
        else
        {
            if (Lerp)
            {
                Velocity = Speed * Vector3.Distance(transform.position, new Vector3(Target.x, Target.y, transform.position.z));
                Velocity = Mathf.Max(Velocity, 1f);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Target.x, Target.y, transform.position.z), Velocity * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(Target.x, Target.y, transform.position.z);
            }
        }
    }
}
