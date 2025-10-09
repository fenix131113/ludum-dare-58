using Core.Data;
using InventorySystem;
using PlayerSystem;
using PlayerSystem.Data;
using PlayerSystem.View;
using ShopSystem;
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

            builder.RegisterInstance(layersData);
            builder.Register<GameVariables>(Lifetime.Scoped);

            #endregion

            #region Player

            _input = new InputSystem_Actions();
            _input.Player.Enable();
            builder.Register<Inventory>(Lifetime.Scoped); // Now we have only one inventory - for player. But Inventory class is extendable
            builder.Register<PlayerResources>(Lifetime.Scoped);
            builder.Register<ItemSelector>(Lifetime.Scoped)
                .As<ITickable>()
                .As<IInitializable>()
                .AsSelf();
            builder.RegisterInstance(_input);
            builder.RegisterInstance(playerConfig);
            builder.RegisterComponentInHierarchy<PlayerAim>();
            builder.RegisterComponentInHierarchy<PlayerInventoryView>();
            builder.RegisterComponentInHierarchy<UpgradesImplementer>();

            #endregion
        }
    }
}