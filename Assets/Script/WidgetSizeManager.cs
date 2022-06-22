using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSizeManager
{
    public float srcPointRadius;
    public float dstPointRadius;
    public float scoreWallRadius;
    public float bombRadius;
    public float candyRadius;

    static WidgetSizeManager instance;
    public static WidgetSizeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WidgetSizeManager();
            }
            return instance;
        }
    }

    private WidgetSizeManager()
    {
        int width = Screen.width;
        int height = Screen.height;

        srcPointRadius = width / 32;
        dstPointRadius = width / 32;
        scoreWallRadius = width / 16;
        bombRadius = width / 32;
        candyRadius = width / 32;
    }
}

