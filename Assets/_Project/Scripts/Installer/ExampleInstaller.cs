using Zenject;
using HOTS.Manager.Battle;

namespace HOTS.Installer
{
    /// <summary>
    /// Class provides a method to install
    /// all bindings in the example scene.
    /// </summary>
    public class ExampleInstaller : MonoInstaller
    {
        /// <summary>
        /// Installs the bindings for the example scene.
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind<BattleEventSystem>().AsSingle().NonLazy();
        }
    }

}
