using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetSizeManager
{
    public float srcPointRadius;
    public float dstPointRadius;
    public float scoreWallRadius;
    public float bombRadius;

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


    //private static readonly WidgetSizeManager instance = new WidgetSizeManager();
    private WidgetSizeManager()
    {
        int width = Screen.width;
        int height = Screen.height;

        srcPointRadius = width / 32;
        dstPointRadius = width / 32;
        scoreWallRadius = width / 8;
        bombRadius = width / 32;
    }


    //public static WidgetSizeManager GetInstance()
    //{
    //    return instance;
    //}
}

