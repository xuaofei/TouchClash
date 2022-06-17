using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSizeManager
{
    public float srcPointRadius;
    public float dstPointRadius;
    public float scoreWallRadius;
    public float bombRadius;

    private static readonly WidgetSizeManager instance = new WidgetSizeManager();
    private WidgetSizeManager()
    {
        int width = Screen.width;
        int height = Screen.height;

        srcPointRadius = width / 16;
        dstPointRadius = width / 16;
        scoreWallRadius = width / 8;
        bombRadius = width / 16;
    }


    public static WidgetSizeManager GetInstance()
    {
        return instance;
    }
}

