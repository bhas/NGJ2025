using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGenerator2 : MonoBehaviour
{
    [Header("Ramp Settings")]
    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(100, 0, 1, 0);
    public float altitude = 50;
    public int resolution = 50;
    public float length = 50f;
    public float width = 20f;
    public float edgeWidth = 3f;
    public float edgeHeight = 10f;

    private const int verticesPerSegment = 4;

    [ContextMenu("Generate Slope")]
    private void GenerateRampMesh()
    {
        Mesh mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        float halfWidth = width / 2f;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            float y = heightCurve.Evaluate(t) * altitude;
            float z = t * length;

            // Add vertices
            var left = i * verticesPerSegment;
            var right = i * verticesPerSegment + 1;
            var botLeft = i * verticesPerSegment + 2;
            var botRight = i * verticesPerSegment + 3;
            vertices.Add(new Vector3(-halfWidth, y, z));  // Left
            vertices.Add(new Vector3(halfWidth, y, z)); // Right
            vertices.Add(new Vector3(-halfWidth - edgeWidth, y - edgeHeight, z));  // Left bot
            vertices.Add(new Vector3(halfWidth + edgeWidth, y - edgeHeight, z)); // Right right

            // Create triangles
            if (i > 0)
            {
                AddQuad(triangles, left, right);
                AddQuad(triangles, botLeft, left);
                AddQuad(triangles, right, botRight);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        UpdateMeshCollider(mesh);
    }

    private void AddQuad(List<int> triangles, int v1, int v2)
    {
        triangles.Add(v1);
        triangles.Add(v2);
        triangles.Add(v1 - verticesPerSegment);

        triangles.Add(v2);
        triangles.Add(v2 - verticesPerSegment);
        triangles.Add(v1 - verticesPerSegment);
    }

    private void UpdateMeshCollider(Mesh mesh)
    {
        // Update MeshCollider if present
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null; // Clear first to force refresh
            meshCollider.sharedMesh = mesh;
        }
    }
}
