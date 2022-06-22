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
    private WidgetSizeManager widgetSizeManager = WidgetSizeManager.Instance;
    private int minSafeSpace = 20;

    public List<GameLevel> GetGameLevels()
    {
        return null;
    }

    public GameLevel GetGameLevel(int currentLevel, List<SrcPointPosInfo> srcPointPosInfos)
    {
        GameLevelDescribet gameLevelDescribet = new GameLevelDescribet();
        gameLevelDescribet.playerCount = 3;
        gameLevelDescribet.bombCount = 2;
        gameLevelDescribet.candyCount = 1;

        GameLevel gameLevel = new GameLevel();
        gameLevel.levelIndex = currentLevel;

        int width = Screen.width;
        int height = Screen.height;

        // 生成目标点坐标
        List<DstPointPosInfo> dstPointPosInfos = new List<DstPointPosInfo>();
        for (int i = 0; i < gameLevelDescribet.playerCount; i++)
        {
            dstPointPosInfos.Add(generateRandomDstPoint(i, srcPointPosInfos, dstPointPosInfos));
        }
        gameLevel.dstPointPosInfos = dstPointPosInfos;

        // 生成炸弹坐标
        List<BombPosInfo> bombPosInfos = new List<BombPosInfo>();
        for (int i = 0; i < gameLevelDescribet.bombCount; i++)
        {
            bombPosInfos.Add(generateRandomBomb(i, srcPointPosInfos, dstPointPosInfos, bombPosInfos));
        }
        gameLevel.bombPosInfos = bombPosInfos;


        // 生成糖果坐标
        List<CandyPosInfo> candyPosInfos = new List<CandyPosInfo>();
        for (int i = 0; i < gameLevelDescribet.candyCount; i++)
        {
            candyPosInfos.Add(generateRandomCandy(i, srcPointPosInfos, dstPointPosInfos, bombPosInfos));
        }
        gameLevel.candyPosInfos = candyPosInfos;

        return gameLevel;
    }


    private DstPointPosInfo generateRandomDstPoint(int dstPointId, List<SrcPointPosInfo> srcPointPosInfos, List<DstPointPosInfo> dstPointPosInfos)
    {
        int safeSpace = 0;
        do
        {
            System.Random rd = RandomSystem.GetInstance().dstPointRandom;
            float srcPointRadius = widgetSizeManager.srcPointRadius;
            float dstPointRadius = widgetSizeManager.dstPointRadius;
            safeSpace = ((int)srcPointRadius) + minSafeSpace;

            int xPos = rd.Next(safeSpace, Screen.width - safeSpace);
            int yPos = rd.Next(safeSpace, Screen.height - safeSpace);
            Vector2 newDstPointPos = new Vector2(xPos, yPos);

            Debug.Log("xaf pos newDstPointPos:" + newDstPointPos.ToString() + "srcPointScreenRadius:" + srcPointRadius);

            foreach (SrcPointPosInfo _srcPointPos in srcPointPosInfos)
            {
                Vector2 srcPointPos = _srcPointPos.pos;
                //srcPointPos = worldToScreenPoint(srcPointPos);



                float distance = (srcPointPos - newDstPointPos).magnitude;

                //Debug.Log("xaf pos _srcPoint:" + srcPointPos.ToString() + "distance:" + distance);

                if (dstPointId == _srcPointPos.srcPointId)
                {
                    // 原点和目标点是同一组,距离要求远一点。
                    if (distance < ((Screen.width + Screen.height) / 4))
                    {
                        // 两个点直接距离小于1.5倍原点直径，重新生成位置。
                        goto reGenerate;
                    }
                }
                else
                {
                    // 原点和目标点不是同一组
                    if (distance < (srcPointRadius + dstPointRadius + minSafeSpace))
                    {
                        // 两个点直接距离小于1.5倍原点直径，重新生成位置。
                        goto reGenerate;
                    }
                }
            }

            foreach (DstPointPosInfo _dstPointPos in dstPointPosInfos)
            {
                Vector2 dstPointPos = _dstPointPos.pos;
                //dstPointPos = worldToScreenPoint(dstPointPos);

                float distance = (dstPointPos - newDstPointPos).magnitude;
                Debug.Log("xaf pos _dstPoint:" + dstPointPos.ToString() + "distance:" + distance);

                if (distance < (srcPointRadius * 3))
                {
                    // 两个点直接距离小于1.5倍原点直径，重新生成位置。
                    goto reGenerate;
                }
            }

            Vector2 scoreWallWorldPos = new Vector2(Screen.width / 2, Screen.height / 2);
            float circleWallScreenRadius = widgetSizeManager.scoreWallRadius;

            float distanceScoreWall = (scoreWallWorldPos - newDstPointPos).magnitude;
            if (distanceScoreWall < (srcPointRadius + circleWallScreenRadius) * 1.5)
            {
                goto reGenerate;
            }

            DstPointPosInfo dstPointPosInfo = new DstPointPosInfo();
            dstPointPosInfo.dstPointId = dstPointId;
            dstPointPosInfo.pos = newDstPointPos;

            return dstPointPosInfo;

        reGenerate:
            {
                Debug.Log("reGenerate dst point");
            };

        } while (true);

    }


    private BombPosInfo generateRandomBomb(int bombId, List<SrcPointPosInfo> srcPointPosInfos, List<DstPointPosInfo> dstPointPosInfos, List<BombPosInfo> bombPosInfos)
    {
        int safeSpace = 0;
        do
        {
            System.Random rd = RandomSystem.GetInstance().bombRandom;
            float bombRadius = widgetSizeManager.bombRadius;
            float srcPointRadius = widgetSizeManager.srcPointRadius;
            float dstPointRadius = widgetSizeManager.dstPointRadius;
            float scoreWallRadius = widgetSizeManager.scoreWallRadius;
            safeSpace = ((int)bombRadius) + 20;

            int xPos = rd.Next(safeSpace, Screen.width - safeSpace);
            int yPos = rd.Next(safeSpace, Screen.height - safeSpace);
            Vector2 newBombPos = new Vector2(xPos, yPos);

            Debug.Log("xaf pos newBombPos:" + newBombPos.ToString() + "bombRadius:" + bombRadius);

            foreach (SrcPointPosInfo _srcPointPosInfo in srcPointPosInfos)
            {
                Vector2 srcPointPos = _srcPointPosInfo.pos;
                float distance = (srcPointPos - newBombPos).magnitude;

                if (distance < ((bombRadius + srcPointRadius) * 1.5f))
                {
                    // 两个点距离太小，重新生成位置。
                    goto reGenerate;
                }
            }

            foreach (DstPointPosInfo _dstPointPosInfo in dstPointPosInfos)
            {
                Vector2 dstPointPos = _dstPointPosInfo.pos;

                float distance = (dstPointPos - newBombPos).magnitude;
                //Debug.Log("xaf pos _dstPoint:" + dstPointPos.ToString() + "distance:" + distance);

                if (distance < ((bombRadius + dstPointRadius) * 1.5f))
                {
                    // 两个点距离太小，重新生成位置。
                    goto reGenerate;
                }
            }

            Vector2 scoreWallWorldPos = new Vector2(Screen.width / 2, Screen.height / 2);
            float circleWallScreenRadius = widgetSizeManager.scoreWallRadius;

            float distanceScoreWall = (scoreWallWorldPos - newBombPos).magnitude;
            if (distanceScoreWall < (bombRadius + circleWallScreenRadius + minSafeSpace))
            {
                goto reGenerate;
            }


            foreach (BombPosInfo _bombPosInfo in bombPosInfos)
            {
                Vector2 bombPos = _bombPosInfo.pos;

                float distance = (bombPos - newBombPos).magnitude;
                if (distance < (Screen.width + Screen.height) / 8)
                {
                    // 两个点距离太小，重新生成位置。
                    goto reGenerate;
                }
            }

            BombPosInfo bombPosInfo = new BombPosInfo();
            bombPosInfo.bombId = bombId;
            bombPosInfo.pos = newBombPos;

            return bombPosInfo;

        reGenerate:
            {
                Debug.Log("reGenerate bomb");
            };

        } while (true);
    }

    private CandyPosInfo generateRandomCandy(int candyId, List<SrcPointPosInfo> srcPointPosInfos, List<DstPointPosInfo> dstPointPosInfos, List<BombPosInfo> bombPosInfos)
    {
        int safeSpace = 0;
        do
        {
            System.Random rd = RandomSystem.GetInstance().candyRandom;
            float candyRadius = widgetSizeManager.candyRadius;
            float srcPointRadius = widgetSizeManager.srcPointRadius;
            float dstPointRadius = widgetSizeManager.dstPointRadius;
            float scoreWallRadius = widgetSizeManager.scoreWallRadius;
            float bombRadius = widgetSizeManager.bombRadius;
            safeSpace = ((int)candyRadius) + 20;

            int xPos = rd.Next(safeSpace, Screen.width - safeSpace);
            int yPos = rd.Next(safeSpace, Screen.height - safeSpace);
            Vector2 newCandyPos = new Vector2(xPos, yPos);

            Debug.Log("xaf pos newCandyPos:" + newCandyPos.ToString() + "candyRadius:" + candyRadius);

            foreach (SrcPointPosInfo _srcPointPosInfo in srcPointPosInfos)
            {
                Vector2 srcPointPos = _srcPointPosInfo.pos;
                float distance = (srcPointPos - newCandyPos).magnitude;

                if (distance < ((candyRadius + srcPointRadius) * 1.5f))
                {
                    // 两个点距离太小，重新生成位置。
                    goto reGenerate;
                }
            }

            foreach (DstPointPosInfo _dstPointPosInfo in dstPointPosInfos)
            {
                Vector2 dstPointPos = _dstPointPosInfo.pos;

                float distance = (dstPointPos - newCandyPos).magnitude;
                //Debug.Log("xaf pos _dstPoint:" + dstPointPos.ToString() + "distance:" + distance);

                if (distance < ((candyRadius + dstPointRadius) * 1.5f))
                {
                    // 两个点距离太小，重新生成位置。
                    goto reGenerate;
                }
            }

            Vector2 scoreWallWorldPos = new Vector2(Screen.width / 2, Screen.height / 2);
            float circleWallScreenRadius = widgetSizeManager.scoreWallRadius;

            float distanceScoreWall = (scoreWallWorldPos - newCandyPos).magnitude;
            if (distanceScoreWall < (candyRadius + circleWallScreenRadius + minSafeSpace))
            {
                goto reGenerate;
            }


            foreach (BombPosInfo _bombPosInfo in bombPosInfos)
            {
                Vector2 bombPos = _bombPosInfo.pos;

                float distance = (bombPos - newCandyPos).magnitude;
                if (distance < (Screen.width + Screen.height) / 8)
                {
                    // 两个点距离太小，重新生成位置。
                    goto reGenerate;
                }
            }

            CandyPosInfo candyPosInfo = new CandyPosInfo();
            candyPosInfo.candyId = candyId;
            candyPosInfo.pos = newCandyPos;

            return candyPosInfo;

        reGenerate:
            {
                Debug.Log("reGenerate Candy");
            };

        } while (true);
    }
}