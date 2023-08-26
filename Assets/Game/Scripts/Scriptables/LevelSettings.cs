using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public Vector3 playerSpawnPos;
    public List<LevelsData> levels;

    public LevelsData GetCurrentLevel(int index)
    {
        var levelIndex = index < levels.Count ? levels[index].levelIndex : levels[Random.Range(0, levels.Count - 1)].levelIndex;
        return levels[levelIndex];
    }
}

[Serializable]
public class LevelsData
{
    public int levelIndex;
    public List<PlatformData> platformData;
}

[Serializable]
public class PlatformData
{
    public int platformIndex;
    [HideInInspector] public int bridgePartAmount;
    public int firstGroupAmount;
    public Color32 firstGroupColor;
    public int secondGroupAmount;
    public Color32 secondGroupColor;
}