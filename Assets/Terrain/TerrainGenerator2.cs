using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGenerator2 : MonoBehaviour
{
    [Header("Ramp Settings")]
    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(0, 100, 1, 0);
    public float altitude = 50;
    public int resolution = 50;
    public float length = 50f;
    public float edgeWidth = 3f;
    public float edgeHeight = 10f;
    public AnimationCurve widthCurve = AnimationCurve.Constant(0, 1, 1);
    public float width = 20f;
    public AnimationCurve curve = AnimationCurve.Constant(0, 1, 0);
    public float curveFactor = 1f;

    [Header("Environment Settings")]
    public MeshFilter bottomObject;
    public float terrainWidth = 200;
    public GameObject treePrefab1;
    public GameObject treePrefab2;
    public float totalTrees = 300;

    [Header("Fence Settings")]
    public Transform FenceContainer;
    public GameObject FencePrefab;
    [Range(0f, 1f)]
    public float Density = 0.5f;
    public float FenceGap = 2f;
    [Range(0f, 1f)]
    public float FenceMargin = 0.1f;

    [Header("Penguin Settings")]
    [Range(2, 30)]
    public int PenguinClusters = 5;
    public Transform PenguinContainer;
    public GameObject PenguinPrefab;

    [Header("Snowmen Settings")]
    public int SnowMen = 10;
    public Transform SnowMenContainer;
    public GameObject SnowMenPrefab;


    private void GeneratePenguins()
    {
        ClearObjects(PenguinContainer, "Penguin");
        for (int i = 0; i< PenguinClusters; i++) 
        {
            var count = Random.Range(2, 5);
            var t = Random.Range(0.1f, 0.95f);
            var x = Random.Range(-0.8f, 0.8f);

            for (int j = 0; j < count; j++)
            {
                var dt = Random.Range(-0.01f, 0.01f);
                var dx = Random.Range(-0.1f, 0.1f);
                var pos = GetPos(t + dt, x + dx);
                var obj = Instantiate(PenguinPrefab, pos, Quaternion.identity, PenguinContainer);
            }
        }
    }

    private void GenerateSnowMen()
    {
        ClearSnowMen();
        for (int i = 0; i < PenguinClusters; i++)
        {
            var t = Random.Range(0.1f, 0.95f);
            var x = Random.Range(-0.8f, 0.8f);
            Instantiate(SnowMenPrefab, GetPos(t, x), Quaternion.identity, SnowMenContainer);
        }
    }

    private void ClearSnowMen()
    {
        var objs = SnowMenContainer.GetComponentsInChildren<Transform>().Where(x => x.CompareTag("SnowMan")).ToList();
        foreach (var obj in objs)
        {
            DestroyImmediate(obj.gameObject);
        }
    }

    private void ClearObjects(Transform parent, string tag)
    {
        var objs = parent.GetComponentsInChildren<Transform>().Where(x => x.CompareTag(tag)).ToList();
        foreach (var obj in objs)
        {
            DestroyImmediate(obj.gameObject);
        }
    }

    private void GenerateFence()
    {
        ClearFences();

        float halfWidth = 1 - FenceMargin;
        var tmp_resolution = length / FenceGap;
        for (int i = 0; i < tmp_resolution; i++)
        {
            float t = i / (float)(tmp_resolution - 1);
            
            if (Density >= Random.Range(0f, 1f))
            {
                var fenceL = Instantiate(FencePrefab, GetPos(t, halfWidth), Quaternion.identity, FenceContainer);
                GetRotation(fenceL.transform, t);
            }
            if (Density >= Random.Range(0f, 1f))
            {
                var fenceR = Instantiate(FencePrefab, GetPos(t, -halfWidth), Quaternion.identity, FenceContainer);
                GetRotation(fenceR.transform, t);
            }
        }
    }

    private void GetRotation(Transform transform, float t)
    {
        float delta = 0.001f;
        float dy = heightCurve.Evaluate(t + delta) - heightCurve.Evaluate(t - delta);
        float dx = 2 * delta;
        var angle = Vector3.Angle(Vector3.forward, new Vector3(0, dy, dx).normalized);
        transform.localRotation = Quaternion.Euler(0, 90, -angle*0.3f);
    }

    private void ClearFences()
    {
        var fences = FenceContainer.GetComponentsInChildren<BoxCollider>();
        foreach(var fence in fences)
        {
            DestroyImmediate(fence.gameObject);
        }
    }

    [ContextMenu("Clear Objects")]
    private void ClearAllObjects()
    {
        ClearFences();
        ClearSnowMen();
        ClearObjects(PenguinContainer, "Penguin");
    }

    [ContextMenu("Generate ALL")]
    private void GenerateAll()
    {
        GenerateSnowMen();
        GenerateRampMesh();
        GenerateFence();
        GeneratePenguins();
        GenerateBottomTerrain();
    }

    [ContextMenu("Generate Slope")]
    private void GenerateRampMesh()
    {
        Mesh mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        const int verticesPerSegment = 4;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);

            // Add vertices
            var left = i * verticesPerSegment;
            var right = i * verticesPerSegment + 1;
            var botLeft = i * verticesPerSegment + 2;
            var botRight = i * verticesPerSegment + 3;
            vertices.Add(GetPos(t, -1));  // Left
            vertices.Add(GetPos(t, 1)); // Right
            vertices.Add(GetPos(t, -1) + new Vector3(-edgeWidth, -edgeHeight, 0));  // Left bot
            vertices.Add(GetPos(t, 1) + new Vector3(edgeWidth, -edgeHeight, 0)); // Right right

            // Create triangles
            if (i > 0)
            {
                AddQuad(triangles, left, right, verticesPerSegment);
                AddQuad(triangles, botLeft, left, verticesPerSegment);
                AddQuad(triangles, right, botRight, verticesPerSegment);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        UpdateMeshCollider(mesh);

        GenerateBottomMesh();
    }

    private void GenerateBottomMesh()
    {
        Mesh mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        const int verticesPerSegment = 2;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);

            // Add vertices
            var left = i * verticesPerSegment;
            var right = i * verticesPerSegment + 1;
            vertices.Add(GetBottomPos(t, -1));  // Left
            vertices.Add(GetBottomPos(t, 1)); // Right

            // Create triangles
            if (i > 0)
            {
                AddQuad(triangles, left, right, verticesPerSegment);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        bottomObject.mesh = mesh;
    }

    [ContextMenu("Generate bot terrain")]
    public void GenerateBottomTerrain()
    {
        ClearObjects(bottomObject.transform, "Tree");
        for (int i = 0; i < totalTrees; i++)
        {
            var prefab = i % 2 == 0 ? treePrefab1 : treePrefab2;
            var t = Random.Range(0f, 1f);
            var x = Random.Range(-1f, 1f);
            Instantiate(prefab, GetBottomPos(t, x), Quaternion.identity, bottomObject.transform);
        }
    }

    private Vector3 GetPos(float t, float dx)
    {
        float xOffset = curve.Evaluate(t) * curveFactor;
        float x = dx * GetWidth(t) / 2f;
        float y = heightCurve.Evaluate(t) * altitude;
        float z = t * length;
        return new Vector3(x + xOffset, y, z);
    }

    private Vector3 GetBottomPos(float t, float dx)
    {
        float x = dx * terrainWidth / 2f;
        float y = heightCurve.Evaluate(t) * altitude - edgeHeight;
        float z = t * length;
        return new Vector3(x, y, z);
    }

    private float GetWidth(float t)
    {
        return widthCurve.Evaluate(t) * width;
    }

    private void AddQuad(List<int> triangles, int v1, int v2, int verticesPerSegment)
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
