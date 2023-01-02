using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFX : MonoBehaviour
{
    public Color CircleColor;
    public LineRenderer lr;

    public float Radius;
    public float DestroyDelay;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, this.DestroyDelay);
        DrawCircle(this.gameObject, Radius, 0.15f, CircleColor);
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //this makes it brighter idk
        lr.generateLightingData = true;
        lr.material.color = CircleColor;
    }

    public void DrawCircle(GameObject selected, float radius,
        float lineWidth, Color color) {
        var segments = 360;
        var line = selected.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++) {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
        
        //green line
        line.material.color = color;
    }
}
