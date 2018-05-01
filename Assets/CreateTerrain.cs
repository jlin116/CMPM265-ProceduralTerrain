using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCreator))]
public class CreateTerrain : MonoBehaviour
{
    [Range(0, 100)]
    public int width;

    [Range(0, 100)]
    public int height;

    [Range(1, 10)]
    public float scale;

    [Range(0, 250)]
    public float perlinXValue;

    [Range(0, 250)]
    public float perlinYValue;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCreator meshCreator;

    Vector3 cubeSize = Vector3.one;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCreator = GetComponent<MeshCreator>();
    }

    private void Update()
    {
        // Clear the mesh data
        meshCreator.Clear();

        // Loop to obtain grids of terrain
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < width; ++y)
            {
                CreateTerrainSqure(x, y);
            }
        }
        meshFilter.mesh = meshCreator.CreateMesh();
    }

    protected void CreateTerrainSqure(int x, int y)
    {
        Vector3 scaledSize = cubeSize * 0.5f;
        Vector3 center = new Vector3(x * cubeSize.x, 0, y * cubeSize.z);

        // top of the cube
        // t0 is top left point
        Vector3 t0 = new Vector3(center.x + scaledSize.x, center.y + scaledSize.y, center.z - scaledSize.z);
        Vector3 t1 = new Vector3(center.x - scaledSize.x, center.y + scaledSize.y, center.z - scaledSize.z);
        Vector3 t2 = new Vector3(center.x - scaledSize.x, center.y + scaledSize.y, center.z + scaledSize.z);
        Vector3 t3 = new Vector3(center.x + scaledSize.x, center.y + scaledSize.y, center.z + scaledSize.z);

        // Apply perlin noise to every vertice by mapping the coordinates to perlin range
        t0.y = Perlin.Noise(t0.x / width * 5 + perlinXValue, t0.z / width * 5 + perlinYValue) * scale;
        t1.y = Perlin.Noise(t1.x / width * 5 + perlinXValue, t1.z / width * 5 + perlinYValue) * scale;
        t2.y = Perlin.Noise(t2.x / width * 5 + perlinXValue, t2.z / width * 5 + perlinYValue) * scale;
        t3.y = Perlin.Noise(t3.x / width * 5 + perlinXValue, t3.z / width * 5 + perlinYValue) * scale;

        // Top square
        meshCreator.BuildTriangle(t0, t1, t2);
        meshCreator.BuildTriangle(t0, t2, t3);
    }
}
