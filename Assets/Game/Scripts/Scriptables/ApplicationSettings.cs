using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = nameof(ApplicationSettings), menuName = "ApplicationSettings")]
public class ApplicationSettings : ScriptableObject
{
   public EnemyFacade enemyFacade;
   public PlatformView platformView;
   public PlayerMovement playerMovement;
   public GameView gameView;
}
