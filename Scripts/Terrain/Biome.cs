using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain/Biome", order = 1)]
public class Biome : ScriptableObject
{
    public string ID;

    public float scale;

    public float persistance;
    public float lacunarity;
    public int octaves;

    public float heightMultipler;

    [System.Serializable]
    public class colorsetting
    {
        public string Name = "Null";
        public Color Color;
        [Range(0, 1)]
        public float StartHeight = 0;
    }

    public colorsetting[] ColorSettings;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertices">Mesh deki tum verticesler</param>
    /// <param name="multiplerHeight">Yuksekligi arttirmak icin carpan kullanildiysa o carpan,kullanilmadiysa bos</param>
    /// <returns></returns>
    public Color[] GetColours(Vector3[] vertices)
    {
        Color[] colours = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colours[i] = GetColor(vertices[i].y / heightMultipler / privateGameSettings.Instance.terrainSpacing);
        }

        return colours;
    }

    public Color GetColor(float height)
    {
        Color currentColor = Color.white;
        for (int i = 0; i < ColorSettings.Length; i++)
        {
            if (height >= ColorSettings[i].StartHeight)
            {
                Color randomColor = ColorSettings[i].Color;
                currentColor = randomColor;
            }
        }

        return currentColor;
    }

    public float[,] NoiseMapGenerate(int noiseSize, int seed, Vector3 offset)
    {
        float[,] noiseMap = new float[noiseSize, noiseSize];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.z;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = noiseSize / 2f;
        float halfHeight = noiseSize / 2f;


        for (int y = 0; y < noiseSize; y++)
        {
            for (int x = 0; x < noiseSize; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < noiseSize; y++)
        {
            for (int x = 0; x < noiseSize; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    public Material CreateMaterial()
    {
        //Material material = new Material(Shader.Find("Standard"));
        Material material = Resources.Load<Material>("Terrain/Materials/Terrain");

        return material;
    }
}
