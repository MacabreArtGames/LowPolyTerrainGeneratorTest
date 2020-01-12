using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTestManager : MonoBehaviour
{
    private void Start()
    {
        TerrainSaveData data = TerrainGenerator.TerrainGenerate();

        TerrainGenerator.CreateTerrain(data);
    }
}
