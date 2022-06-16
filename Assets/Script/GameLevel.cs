using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GetSrcPoints();


public struct MinefieldConfig
{
    // 雷区数量
    public int minefieldCount;
    // 雷区目标
    public List<List<int>> minefieldTargets;
    // 雷区位置
    public List<MinefieldArea> minefieldAreas;
};

public class GameLevel
{
    public string levelName;
    public int levelIndex;

    public List<Vector2> dstPointPos = new List<Vector2>();
    public MinefieldConfig minefieldConfig = new MinefieldConfig();

    public List<Candy> candies;
    public List<Bullet> bullets;
    public List<LaseRay> laseRays;

    public string bgMuisc;

    public GetSrcPoints getSrcPointsFun;

    public GameLevel(GameLevelDescribet gameLevelDescribet)
    {
        int width = Screen.width;
        int height = Screen.height;

        if (0 == gameLevelDescribet.levelIndex)
        {
            levelName = "第一关";
            Vector2 v1 = new Vector2(width / 2, height / 8);
            Vector2 v2 = new Vector2(width / 8 * 7, height / 2);
            Vector2 v3 = new Vector2(width / 8, height / 2);
            dstPointPos.Add(v1);
            dstPointPos.Add(v2);
            dstPointPos.Add(v3);


        }
        else
        {
            for(int i = 0;i < gameLevelDescribet.playerCount; i++)
            {
                Rect r;
            }
        }

        minefieldConfig.minefieldCount = gameLevelDescribet.minefieldCount;
        for (int i = 0; i < gameLevelDescribet.minefieldCount; i++)
        {
            System.Random random = new System.Random();
            List<int> targetIds = new List<int>();
            int targetCount = random.Next(1, 3);
            for (int j = 0; j < targetCount; j++)
            {
                targetIds.Add(random.Next(1, gameLevelDescribet.playerCount));
            }
            minefieldConfig.minefieldTargets.Add(targetIds);

            int minefieldArea = random.Next((int)MinefieldArea.Area0, (int)MinefieldArea.Area7);
            minefieldConfig.minefieldAreas.Add((MinefieldArea)minefieldArea);
        }
    }
}
