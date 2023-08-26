using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel
{
    public int CurrentLevel;
    public int Score;

    public void LoadData()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
        Score = PlayerPrefs.GetInt("Score");
    }
}