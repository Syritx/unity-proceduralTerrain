using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    // main terrain
    [Range(2, 256)]   public int resolution = 10;
    [Range(0,400)]    public float frequency = 1;
    [Range(.5f, 100)] public float tileScale = 1;

    public float xOffset, yOffset, seed;
    public float amplitude = 2;

    // terrain style
    [Range(1, 55)]   public int octaves = 1;
    [Range(0, 1.75f)] public float lacunarity;
    [Range(0, 100.5f)]  public float persistance;

    MeshFilter meshFilter;
    TerrainTile tile;
    GameObject obj;

    ImprovedNoise noise = new ImprovedNoise();

    private void OnValidate() {
        Init();
        GenerateMesh();
    }

    void Init() {

        if (meshFilter == null) {
            obj = new GameObject("terrain");
            obj.transform.parent = transform;

            obj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilter = obj.AddComponent<MeshFilter>();
        }

        obj.transform.localScale = new Vector3(resolution*tileScale, 1, resolution*tileScale);

        tile = new TerrainTile(resolution, octaves, amplitude,
                               frequency, persistance, lacunarity, xOffset, yOffset, seed, noise);
        meshFilter.sharedMesh = tile.mesh;
    }

    void GenerateMesh()
    {
        tile.GenerateTerrain();
    }
}
