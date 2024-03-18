using UnityEngine;

public class GameMain : MonoSingleton<GameMain>
{
    public int CurrentLevel => currentLevel;
    public int CurrentEnemyHP
    {
        get
        {
            int emenyIndex = Mathf.Clamp(CurrentLevel, 0, enemyConfigs.Length - 1);
            return enemyConfigs[emenyIndex].life;
        }
    }
    public EnemySpriteConfig CurrentEnemySpriteConfig
    {
        get
        {
            int emenyIndex = Mathf.Clamp(CurrentLevel, 0, enemyConfigs.Length - 1);
            return enemyConfigs[emenyIndex].spriteConfig;
        }
    }
    public string CurrentEnemyName
    {
        get
        {
            int emenyIndex = Mathf.Clamp(CurrentLevel, 0, enemyConfigs.Length - 1);
            return enemyConfigs[emenyIndex].name;
        }
    }
    public bool IsFinalLevel => CurrentLevel == enemyConfigs.Length - 1;

    public EnemyConfig[] enemyConfigs;

    int currentLevel;

    public void OnGameStart()
    {
        currentLevel = 0;
    }

    public void OnEnterNextLevel()
    {
        currentLevel += 1;
    }
}
