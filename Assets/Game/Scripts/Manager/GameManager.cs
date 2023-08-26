using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour

{
    private LevelModel _levelModel;

    [Inject]
    private void Construct(LevelModel levelModel)
    {
        _levelModel = levelModel;
    }

    private void Awake()
    {
      
        DontDestroyOnLoad(this);
    }

    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
        }
        else
        {
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CurrentLevel" , _levelModel.CurrentLevel);
        PlayerPrefs.SetInt("Score" , _levelModel.Score);
    }
}