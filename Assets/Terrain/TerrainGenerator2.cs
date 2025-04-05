using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGenerator2 : MonoBehaviour
{
    [Header("Ramp Settings")]
    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(100, 0, 1, 0);
    public float altitude = 50;
    public int resolution = 50;
    public float length = 50f;
    public float width = 20f;

    [ContextMenu("Generate Slope")]
    private void GenerateRampMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[resolution * 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(resolution - 1) * 6];

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            float z = t * length;
            float y = heightCurve.Evaluate(t) * altitude;
            float halfWidth = width / 2f;

            // Left and right vertex
            vertices[i * 2] = new Vector3(-halfWidth, y, z);  // Left
            vertices[i * 2 + 1] = new Vector3(halfWidth, y, z); // Right

            // Simple UVs
            uvs[i * 2] = new Vector2(0, t);
            uvs[i * 2 + 1] = new Vector2(1, t);

            // Create triangles
            if (i < resolution - 1)
            {
                int vi = i * 2;
                int ti = i * 6;

                triangles[ti] = vi;
                triangles[ti + 1] = vi + 2;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + 2;
                triangles[ti + 5] = vi + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        UpdateMeshCollider(mesh);
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
