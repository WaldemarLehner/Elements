using System;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class EnemyManager
    {
        private Vector2 spawnPosition;

        public void EnemySpawner(Vector2 spawnPosition, WorldEnum.Type type, int random)
        {
            this.spawnPosition = spawnPosition;

            switch (type)
            {
                case WorldEnum.Type.Water:
                    string[] waterTexture = { "Fungus", "WaterDrop", "Crab", "Lizard" };
                    this.SpawningEnemy(waterTexture, type, random);
                    break;
                case WorldEnum.Type.Earth:
                    string[] earthTexture = { "EarthElemental", "EarthGolem", "Goblin", "Slime" };
                    this.SpawningEnemy(earthTexture, type, random);
                    break;
                case WorldEnum.Type.Fire:
                    string[] fireTexture = { "DemonEye", "FireClaw", "FireDemon", "FireLizard" };
                    this.SpawningEnemy(fireTexture, type, random);
                    break;
                case WorldEnum.Type.Air:
                    string[] airTexture = { "Ghost", "Imp", "Skull", "WitchDoctor" };
                    this.SpawningEnemy(airTexture, type, random);
                    break;
                default:
                    break;
            }
        }

        public void BossSpawner(Vector2 spawnPosition, WorldEnum.Type type)
        {
            switch (type)
            {
                case WorldEnum.Type.Water:
                    string waterTexture = "WaterTree";
                    Scene.Scene.Current.SpawnObject(new EnemyBoss(spawnPosition, waterTexture, type));
                    break;
                case WorldEnum.Type.Earth:
                    string earthTexture = "StoneGolem";
                    Scene.Scene.Current.SpawnObject(new EnemyBoss(spawnPosition, earthTexture, type));
                    break;
                case WorldEnum.Type.Fire:
                    string fireTexture = "FireBoss";
                    Scene.Scene.Current.SpawnObject(new EnemyBoss(spawnPosition, fireTexture, type));
                    break;
                case WorldEnum.Type.Air:
                    string airTexture = "AirBoss";
                    Scene.Scene.Current.SpawnObject(new EnemyBoss(spawnPosition, airTexture, type));
                    break;
                default: break;
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

        private void SpawningEnemy(string[] texture, WorldEnum.Type type, int random)
        {
            var randomTexture = texture[random];
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
