using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainSaveData
{
    public float[,] noiseMap;
    public string biomeID;

    public TerrainSaveData(float[,] noiseMap, string biomeID)
    {
        this.noiseMap = noiseMap;
        this.biomeID = biomeID;
    }
}
