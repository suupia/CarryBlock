// using VContainer;
// using VContainer.Unity;
// using Projects.BattleSystem.GameScene.Scripts;
// using Projects.Utility.Scripts;
// using Projects.BattleSystem.Enemy.Scripts;
// using Projects.BattleSystem.Player.Scripts;
//
// namespace Projects.BattleSystem.GameScene.Scripts
// {
//     public sealed class GameSceneLifetimeScope : LifetimeScope
//     {
//         protected override void Configure(IContainerBuilder builder)
//         {
//             // Serverのドメインスクリプト
//             builder.Register<ResourceAggregator>(Lifetime.Singleton);
//             builder.Register<GameContext>(Lifetime.Singleton);
//             builder.Register<WaveTimer>(Lifetime.Singleton);
//             // Localのドメインスクリプト
//             builder.Register<ReturnToMainBaseGauge>(Lifetime.Singleton);
//         }
//     }
// }