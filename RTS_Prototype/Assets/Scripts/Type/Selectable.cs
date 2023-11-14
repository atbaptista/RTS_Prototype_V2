using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected = false;
    public Camera cam;
    public Selection Selector;
    [HideInInspector] public float health;
    [HideInInspector] public enum unitTypes { Robot, Dinosaur, Dead };
    [HideInInspector] public unitTypes unitType;

    public void DeselectUnit()
    {
        Selector.RemoveUnit(this);
    }

    public bool IsUnitSelected(Vector3 start, Vector3 end)
    {
        Vector3 unitPos = cam.WorldToScreenPoint(transform.position);
        float topY;
        float bottomY;
        float rightX;
        float leftX;

        // start is on left of end
        if (start.x <= end.x)
        {
            rightX = end.x;
            leftX = start.x;
        }
        // start is on right of end
        else
        {
            rightX = start.x;
            leftX = end.x;
        }

        // start is below end
        if (start.y <= end.y)
        {
            bottomY = start.y;
            topY = end.y;
        }
        // start is above end
        else
        {
            bottomY = end.y;
            topY = start.y;
        }

        return unitPos.x <= rightX && unitPos.x >= leftX && unitPos.y <= topY && unitPos.y >= bottomY;
    }

    public void DrawCircle(GameObject selected, float radius,
        float lineWidth, Color color)
    {
        var segments = 360;
        var line = selected.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);

        //green line
        line.material.color = color;
    }
}
