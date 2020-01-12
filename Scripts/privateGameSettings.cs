using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings/privateGameSettings", fileName = "privateGameSetting")]
public class privateGameSettings : ScriptableObject
{
    public static privateGameSettings Instance;

    public int terrainSize = 50;
    public float terrainQuality = 1;
    public float terrainSpacing = 1;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        Instance = Resources.LoadAll<privateGameSettings>("GameSettings")[0];
    }
}
