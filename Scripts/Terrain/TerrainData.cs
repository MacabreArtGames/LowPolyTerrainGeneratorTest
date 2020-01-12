using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
    private float[,] noiseMap;

    public List<Vector3> vertices;
    public List<int> triangles;

    public Biome biome;

    public TerrainData(Biome biome, float[,] noiseMap)
    {
        this.biome = biome;
        this.noiseMap = noiseMap;

        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void Generate()
    {
        for (int z = 0, i = 0; z < privateGameSettings.Instance.terrainSize; z++)
        {
            for (int x = 0; x < privateGameSettings.Instance.terrainSize; x++, i++)
            {
                AddVertex(x, z);

                if(x != privateGameSettings.Instance.terrainSize - 1 && z!= privateGameSettings.Instance.terrainSize - 1)
                {
                    AddTriangles(i);
                }
            }
        }
    }

    private void AddTriangles(int index)
    {
        triangles.Add(index);
        triangles.Add(index + privateGameSettings.Instance.terrainSize);
        triangles.Add(index + privateGameSettings.Instance.terrainSize + 1);

        triangles.Add(index + privateGameSettings.Instance.terrainSize + 1);
        triangles.Add(index + 1);
        triangles.Add(index);
    }

    private void AddVertex(int x,int z)
    {
        Vector3 position = new Vector3(x / (float)privateGameSettings.Instance.terrainQuality * privateGameSettings.Instance.terrainSpacing, noiseMap[x, z] * biome.heightMultipler * privateGameSettings.Instance.terrainSpacing, z / (float)privateGameSettings.Instance.terrainQuality * privateGameSettings.Instance.terrainSpacing) + GetRandomPosition();
        vertices.Add(position);
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-(1 / (float)privateGameSettings.Instance.terrainQuality * privateGameSettings.Instance.terrainSpacing) / 3.5f, (1 / (float)privateGameSettings.Instance.terrainQuality * privateGameSettings.Instance.terrainSpacing) / 3.5f), 0, Random.Range(-(1 / (float)privateGameSettings.Instance.terrainQuality * privateGameSettings.Instance.terrainSpacing) / 3.5f, (1 / (float)privateGameSettings.Instance.terrainQuality * privateGameSettings.Instance.terrainSpacing) / 3.5f));
    }

}
