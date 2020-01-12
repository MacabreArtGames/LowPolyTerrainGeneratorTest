using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainGenerator
{
    public static TerrainSaveData TerrainGenerate()
    {
        Biome biome = GetRandomBiome();

        int seed = Random.Range(-999999999, 999999999);

        TerrainSaveData data = new TerrainSaveData(biome.NoiseMapGenerate(privateGameSettings.Instance.terrainSize, seed, Vector3.zero), biome.ID);

        return data;
    }

    public static Biome GetRandomBiome()
    {
        Biome[] biomes = Resources.LoadAll<Biome>("Terrain/Biomes");

        return biomes[Random.Range(0, biomes.Length)];
    }

    public static Biome GetBiome(string ID)
    {
        Biome[] biomes = Resources.LoadAll<Biome>("Terrain/Biomes");


        for (int i = 0; i < biomes.Length; i++)
        {
            if(biomes[i].ID == ID)
            {
                return biomes[i];
            }
        }

        Debug.Log("Biome Bulunamadi!!!!!!");
        return null;
    }

    public static GameObject CreateTerrain(TerrainSaveData data)
    {
        GameObject terrain = new GameObject("Terrain");
        Biome biome = GetBiome(data.biomeID);

        Mesh mesh = GenerateMesh(data.noiseMap, biome);
        terrain.AddComponent<MeshFilter>().mesh = mesh;
        terrain.AddComponent<MeshRenderer>().material = biome.CreateMaterial();

        return terrain;
    }

    private static Mesh GenerateMesh(float[,] noiseMap, Biome biome)
    {
        TerrainData terrainData = new TerrainData(biome, noiseMap);

        terrainData.Generate();

        Mesh mesh = new Mesh();
        mesh.name = "Terrain";

        mesh.vertices = terrainData.vertices.ToArray();
        mesh.triangles = terrainData.triangles.ToArray();
        mesh.colors = biome.GetColours(terrainData.vertices.ToArray());

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        

        return mesh;

    }
}
