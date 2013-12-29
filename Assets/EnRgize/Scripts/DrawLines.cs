using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLines : MonoBehaviour
{
    public List<Vector2> points;
    public Material lineMaterial;
    public float thickness = 0.05f;

    void Start() {

    }

    void Update() {

    }

    void OnPostRender() {
        GL.PushMatrix();
        lineMaterial.SetPass(0);
        GL.Color(Color.white);

        GL.LoadOrtho();

        /*
        GL.Begin(GL.LINES);
        for (int i = 0; i < points.Count - 1; i++) {
            GL.Vertex(new Vector3(points[i].x, points[i].y, 0));
            GL.Vertex(new Vector3(points[i+1].x, points[i+1].y, 0));
        }
        GL.End();
        */


        GL.Begin(GL.QUADS);
        for (int i = 0; i < points.Count - 1; i++) {
            Vector3 point1 = new Vector3(points[i].x, points[i].y, 0);
            Vector3 point2 = new Vector3(points[i+1].x, points[i+1].y, 0);
            Vector3 difference = point2 - point1;
            Vector3 normal = new Vector3(difference.y, -difference.x, 0).normalized;
            Vector3 perpendicular = normal * thickness;
            
            GL.Vertex(point1 + perpendicular);
            GL.Vertex(point1 - perpendicular);
            GL.Vertex(point2 - perpendicular);
            GL.Vertex(point2 + perpendicular);
        }
        GL.End();

        /*
        GL.Begin(GL.QUADS);
        GL.Color(Color.red);
        // 0.1, 0.1 to 0.5, 0.5
        GL.Vertex3(0.2f, 0.0f, 0);
        GL.Vertex3(0.0f, 0.2f, 0);
        GL.Vertex3(0.4f, 0.6f, 0);
        GL.Vertex3(0.6f, 0.4f, 0);
        GL.End();
        */



        /*
        GL.Begin(GL.QUADS);
        GL.Color(Color.red);
        GL.Vertex3(0, 0.5F, 0);
        GL.Vertex3(0.5F, 1, 0);
        GL.Vertex3(1, 0.5F, 0);
        GL.Vertex3(0.5F, 0, 0);
        GL.Color(Color.cyan);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0.25F, 0);
        GL.Vertex3(0.25F, 0.25F, 0);
        GL.Vertex3(0.25F, 0, 0);
        GL.End();
        */

        GL.PopMatrix();
    }
}
