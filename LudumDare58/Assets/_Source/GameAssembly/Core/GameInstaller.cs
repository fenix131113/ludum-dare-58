using PlayerSystem;
using PlayerSystem.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameInstaller : LifetimeScope
    {
        [SerializeField] private PlayerConfigSO playerConfig;

        private InputSystem_Actions _input;

        protected override void Configure(IContainerBuilder builder)
        {
            #region Player

            _input = new InputSystem_Actions();
            _input.Player.Enable();
            builder.RegisterInstance(_input);
            builder.RegisterInstance(playerConfig);

            #endregion
        }
    }
}