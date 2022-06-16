using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType
{
    LevelFixed,
    LevelRandom,
}

public class GameLevelManager
{
    public int playerCount;
    public GameType gameType = GameType.LevelFixed;
    public List<int> randoms;

    public List<GameLevel> GetGameLevels()
    {
        return null;
    }

    public GameLevel GetNextGameLevel(int currentLevel)
    {
        GameLevelDescribet gameLevelDescribet = new GameLevelDescribet();
        GameLevel gameLevel = new GameLevel(gameLevelDescribet);
        gameLevel.levelIndex = currentLevel;

        int width = Screen.width;
        int height = Screen.height;

        switch (currentLevel)
        {
            case 0:
                {
                    gameLevel.levelName = "第一关";
                    Vector2 v1 = new Vector2(width / 2, height / 8);
                    Vector2 v2 = new Vector2(width / 8 * 7, height / 2);
                    Vector2 v3 = new Vector2(width / 8, height / 2);
                    gameLevel.dstPointPos.Add(v1);
                    gameLevel.dstPointPos.Add(v2);
                    gameLevel.dstPointPos.Add(v3);

                    gameLevel.minefieldConfig.minefieldCount = 1;
                    gameLevel.minefieldConfig.minefieldTargets.Add(new List<int> { 0, 1,2 });
                    break;
                }
            default:
                {
                    break;
                }
        }
        return gameLevel;
    }
}
