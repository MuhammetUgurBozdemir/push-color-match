using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using UnityEngine;

public readonly struct EnemyTouchedPlayerSignal
{
    public readonly EnemyFacade EnemyFacade;

    public EnemyTouchedPlayerSignal(EnemyFacade enemyFacade)
    {
        EnemyFacade = enemyFacade;
    }
}

public readonly struct BridgeTriggeredSignal
{
    public readonly Color32 Color;
    public readonly Color32 Color2;
    public readonly int Index;

    public BridgeTriggeredSignal(Color32 color, Color32 color2,int index)
    {
        Color = color;
        Color2 = color2;
        Index = index;
    }
}

public readonly struct LevelEndSignal
{
}

public readonly struct LevelFailedSignal
{
}
public readonly struct KilledEnemySignal
{
    public readonly EnemyFacade EnemyFacade;

    public KilledEnemySignal(EnemyFacade enemyFacade)
    {
        EnemyFacade = enemyFacade;
    }
}
public readonly struct LeftHealthSignal
{
    public readonly int LeftHealth;

    public LeftHealthSignal(int leftHealth)
    {
        LeftHealth = leftHealth;
    }
}