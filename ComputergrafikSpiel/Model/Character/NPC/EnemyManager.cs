using System;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class EnemyManager
    {
        private Vector2 spawnPosition;

        public void EnemySpawner(Vector2 spawnPosition, WorldEnum.Type type)
        {
            this.spawnPosition = spawnPosition;

            switch (type)
            {
                case WorldEnum.Type.Water:
                    string[] waterTexture = { "Fungus", "WaterDrop", "Crab", "Lizard"};
                    this.SpawningEnemy(waterTexture, WorldEnum.Type.Water);
                    break;
                case WorldEnum.Type.Earth:
                    string[] earthTexture = { "EarthElemental", "EarthGolem", "Goblin", "Slime"};
                    this.SpawningEnemy(earthTexture, WorldEnum.Type.Earth);
                    break;
                case WorldEnum.Type.Fire:
                    string[] fireTexture = { "DemonEye", "FireClaw", "FireDemon", "FireLizard"};
                    this.SpawningEnemy(fireTexture, WorldEnum.Type.Fire);
                    break;
                case WorldEnum.Type.Air:
                    string[] airTexture = { "Ghost", "Imp", "Skull", "WitchDoctor"};
                    this.SpawningEnemy(airTexture, WorldEnum.Type.Air);
                    break;
                default:
                    break;
            }
        }

        private EnemyEnum.Variant WichVariantOfEnemy(string texture)
        {
            switch (texture)
            {
                case "Fungus":
                case "EarthGolem":
                case "FireClaw":
                case "Skull":
                    return EnemyEnum.Variant.Tank;
                case "WaterDrop":
                case "Slime":
                case "FireDemon":
                case "Ghost":
                    return EnemyEnum.Variant.Speed;
                case "Crab":
                case "EarthElemental":
                case "DemonEye":
                case "WitchDoctor":
                    return EnemyEnum.Variant.Range;
                case "Lizard":
                case "Goblin":
                case "FireLizard":
                case "Imp":
                    return EnemyEnum.Variant.Dash;
                default: return EnemyEnum.Variant.Error;
            }
        }

        private void SpawningEnemy(string[] texture, WorldEnum.Type type)
        {
            Random random = new Random();
            var randomTexture = texture[random.Next(0, texture.Length)];
            switch (this.WichVariantOfEnemy(randomTexture))
            {
                case EnemyEnum.Variant.Tank:
                    Scene.Scene.Current.SpawnObject(new TankEnemy(this.spawnPosition, randomTexture, type));
                    break;
                case EnemyEnum.Variant.Dash:
                    Scene.Scene.Current.SpawnObject(new DashEnemy(this.spawnPosition, randomTexture, type));
                    break;
                case EnemyEnum.Variant.Range:
                    Scene.Scene.Current.SpawnObject(new RangeEnemy(this.spawnPosition, randomTexture, type));
                    break;
                case EnemyEnum.Variant.Speed:
                    Scene.Scene.Current.SpawnObject(new SpeedEnemy(this.spawnPosition, randomTexture, type));
                    break;
            }
        }
    }
}
