using Zenject;

namespace Game.Scripts.Enemy
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemyStateManager>().AsSingle();

            Container.Bind<EnemyIdleState>().AsSingle();
            Container.Bind<EnemyStateAttack>().AsSingle();
            Container.Bind<EnemyStateDeath>().AsSingle();
            Container.Bind<EnemyStateFollow>().AsSingle();
            Container.Bind<EnemyStateMoveToTarget>().AsSingle();
        }
    }
}