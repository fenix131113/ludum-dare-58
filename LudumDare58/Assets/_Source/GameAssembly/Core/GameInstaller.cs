using Core.Data;
using InventorySystem;
using PlayerSystem;
using PlayerSystem.Data;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameInstaller : LifetimeScope
    {
        [SerializeField] private PlayerConfigSO playerConfig;
        [SerializeField] private LayersDataSO layersData;

        private InputSystem_Actions _input;

        private void Start() => ObjectInjector.Initialize(Container);

        protected override void Configure(IContainerBuilder builder)
        {
            #region Core

            LayersDataSO.SetupLayersInstance(layersData);
            builder.Register<GameVariables>(Lifetime.Singleton);

            #endregion
            
            #region Player

            _input = new InputSystem_Actions();
            _input.Player.Enable();
            builder.RegisterInstance(new Inventory()); // Now we have only one inventory - for player. But Inventory class is extendable
            builder.RegisterInstance(_input);
            builder.RegisterInstance(playerConfig);
            builder.RegisterComponentInHierarchy<PlayerAim>();
            
            #endregion
        }
    }
}