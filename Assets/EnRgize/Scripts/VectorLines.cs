using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class VectorLines : MonoBehaviour
{
    // Lists of properties for each line
    public Color lineColor;
    public int lineWidth;
    public bool drawLines = true;
    
    // Material and camera
    private Material lineMaterial;
    private Camera cam;
    
    // List of lines (each a list of vertices) and getter/setter
    private List<List<Vector2>> linePoints;
    
    void Awake() {
        lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
            "SubShader { Pass {" +
            "   BindChannels { Bind \"Color\",color }" +
            "   Blend SrcAlpha OneMinusSrcAlpha" +
            "   ZWrite Off Cull Off Fog { Mode Off }" +
            "} } }");
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        cam = camera;
    }

    void Start() {
        List<List<Vector2>> newPointsList = new List<List<Vector2>>();
        List<Vector2> newPoints = new List<Vector2>();

        newPoints.Add(new Vector2(0, 0));
        newPoints.Add(new Vector2(0.5f, 1));
        newPoints.Add(new Vector2(1, 0));
        newPoints.Add(new Vector2(0, 0));
        newPoints.Add(new Vector2(10, 30));
        newPointsList.Add(newPoints);

        InitializeLines(newPointsList);
    }
    
    public void InitializeLines(List<List<Vector2>> newPointsList) {
        // Creates new list of points
        linePoints = new List<List<Vector2>>();
        for (int i = 0; i < newPointsList.Count; ++i) {
            List<Vector2> newList = new List<Vector2>();
            for (int j = 0; j < newPointsList[i].Count; ++j) {
                newList.Add(newPointsList[i][j]);
            }
            linePoints.Add(newList);
        }
    }
    
    public void UpdateLines(List<List<Vector2>> updatedPoints) {
        // Sets all points of list to update list of points
        for (int i = 0; i < linePoints.Count; ++i)
            for (int j = 0; j < linePoints[i].Count; ++j)
                linePoints[i][j] = updatedPoints[i][j];
    }
    
    void OnPostRender() {
        // Cycles through each separate line
        for (int i = 0; i < linePoints.Count; ++i) {
            if (!drawLines || linePoints == null || linePoints[i].Count < 2)
                return;
            
            float nearClip = cam.nearClipPlane + 0.00001f;
            int end = linePoints[i].Count - 1;
            float thisWidth = 1f / Screen.width * lineWidth * 0.5f;
            
            lineMaterial.SetPass(0);
            GL.Color(lineColor);
            
            if (lineWidth == 1) {
                GL.Begin(GL.LINES);
                for (int j = 0; j < end; ++j) {
                    GL.Vertex(new Vector3(linePoints[i][j].x, linePoints[i][j].y, 0));
                    GL.Vertex(new Vector3(linePoints[i][j+1].x, linePoints[i][j+1].y, 0));
                    //GL.Vertex(cam.ViewportToWorldPoint(new Vector3(linePoints[i][j].x, linePoints[i][j].y, nearClip)));
                    //GL.Vertex(cam.ViewportToWorldPoint(new Vector3(linePoints[i][j + 1].x, linePoints[i][j + 1].y, nearClip)));
                }
            } else {
                GL.Begin(GL.QUADS);
                for (int j = 0; j < end; ++j) {
                    Vector3 perpendicular = (new Vector3(linePoints[i][j + 1].y, linePoints[i][j].x, nearClip) -
                        new Vector3(linePoints[i][j].y, linePoints[i][j + 1].x, nearClip)).normalized * thisWidth;
                    Vector3 v1 = new Vector3(linePoints[i][j].x, linePoints[i][j].y, nearClip);
                    Vector3 v2 = new Vector3(linePoints[i][j + 1].x, linePoints[i][j + 1].y, nearClip);
                    GL.Vertex(cam.ViewportToWorldPoint(v1 - perpendicular));
                    GL.Vertex(cam.ViewportToWorldPoint(v1 + perpendicular));
                    GL.Vertex(cam.ViewportToWorldPoint(v2 + perpendicular));
                    GL.Vertex(cam.ViewportToWorldPoint(v2 - perpendicular));
                    print("in GL.QUADS");
                }
            }
            GL.End();
        }
    }
    
    void OnApplicationQuit() {
        DestroyImmediate(lineMaterial);
    }
}