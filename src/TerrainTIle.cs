using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile {

    [HideInInspector]
    public Mesh mesh = new Mesh();
    int resolution = 0;

    float ampl, freq, lac, per, xOff, yOff, seed;
    int octaves;

    ImprovedNoise imprNoise;

    public TerrainTile(int resolution, int octaves, float amplitude, float frequency, float persistance, float lacunarity,
                       float xOff, float yOff, float seed, ImprovedNoise noise) {
        this.resolution = resolution;
        ampl = amplitude;
        freq = frequency;

        this.octaves = octaves;
        lac = lacunarity;
        per = persistance;

        this.xOff = xOff;
        this.yOff = yOff;
        this.seed = seed;

        this.imprNoise = noise;
    }

    public void GenerateTerrain()
    {
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(resolution,2)];
        int[] indices = new int[(int)Mathf.Pow(resolution - 1, 2) * 6];

        int idTriangle = 0;

        for (int z = 0; z < resolution; z++) {
            for (int x = 0; x < resolution; x++) {

                int index = x + z * resolution;

                float heightMap = 0;
                float frequency = freq;
                float amplitude = ampl;

                for (int o = 0; o < octaves; o++)
                {
                    heightMap += (float)imprNoise.noise((float)(x+xOff)/resolution*frequency, (float)(z + yOff)/resolution*frequency, seed) * amplitude;
                    frequency *= lac;
                    amplitude *= per;
                }

                //MonoBehaviour.print(heightMap);

                Vector2 percent = new Vector2(x, z) / (int)Mathf.Sqrt((indices.Length/6));
                vertices[index] = Vector3.up + (percent.x - .5f) * 2 * new Vector3(1, 0, 0) + (percent.y - .5f) * 2 * Vector3.Cross(Vector3.up, new Vector3(1,0,0)) + new Vector3(0, heightMap*10,0);

                if (x != resolution-1 && z != resolution-1)
                {
                    indices[idTriangle] = index;
                    indices[idTriangle+1] = index+resolution+1;
                    indices[idTriangle+2] = index+resolution;

                    indices[idTriangle+3] = index;
                    indices[idTriangle+4] = index + 1;
                    indices[idTriangle+5] = index + resolution + 1;

                    idTriangle += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
    }
}
