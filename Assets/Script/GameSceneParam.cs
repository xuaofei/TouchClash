using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePlayerType
{
    LOCAL,
    ONLINE,
}

public class GameSceneParam
{
    public GamePlayerType gamePlayerType = GamePlayerType.LOCAL;
    public int playerCount = 3;

    static GameSceneParam instance;
    public static GameSceneParam Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameSceneParam();
            }
            return instance;
        }
    }

    private GameSceneParam()
    {

    }
}
