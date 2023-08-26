using Zenject;

namespace Game.Scripts.Signals
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<EnemyTouchedPlayerSignal>();
            Container.DeclareSignal<BridgeTriggeredSignal>();
            Container.DeclareSignal<LevelEndSignal>();
            Container.DeclareSignal<LeftHealthSignal>();
            Container.DeclareSignal<KilledEnemySignal>();
            Container.DeclareSignal<LevelFailedSignal>();
        }
    }
}