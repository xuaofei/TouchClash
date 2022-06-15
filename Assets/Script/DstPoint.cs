//#define TEST_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DstPointMovementState
//{
//    IDLE,           //手指松开，空闲状态
//    RUN             //手指按住，移动状态
//}

public enum DstPointTouchState
{
    ENTER,           //原点和目标点相碰
    EXIT             //原点和目标点未相碰
}

public class DstPoint : MonoBehaviour
{
    // 委托声明
    //public delegate void DstPointTouchEnterDelegate(int srcPointId, int dstPointId);
    //public delegate void DstPointTouchExitDelegate(int srcPointId, int dstPointId);

    private GameManager gameManager;
    private ScoreWallController scoreWallController;
    public Animation animation;
    public int dstPointId; //srcPointId相同
    public Color color;
    DstPointTouchState touchState;

    //public DstPointTouchEnterDelegate dstPointTouchEnterDelegate;
    //public DstPointTouchExitDelegate dstPointTouchExitDelegate;

#if TEST_ENABLE
    int i = 0;
    int j = 0;
#endif

    // Start is called before the first frame update
    void Start()
    {
        touchState = DstPointTouchState.EXIT;

#if TEST_ENABLE
        j = new System.Random().Next(60, 150);
#endif
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (sp)
        {
            sp.color = color;
        }

        GameObject scoreWall = GameObject.Find("ScoreWall");
        if (scoreWall)
        {
            scoreWallController = scoreWall.GetComponent<ScoreWallController>();
        }


        GameObject map = GameObject.Find("map");
        if (map)
        {
            gameManager = map.GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
#if TEST_ENABLE
        if (i++ == j)
        {
            if (scoreWallController)
            {
                scoreWallController.AddScore(1);
            }

            if (gameManager)
            {
                gameManager.removeDstPoint(this);
                gameManager.generateRandDstPoint(dstPointId);
            }

            Destroy(this.gameObject);
            Destroy(this);
        }
#endif
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        SrcPoint srcPoint = collision.GetComponent<SrcPoint>();
        //if (srcPoint)
        //{
        //    if (srcPoint.srcPointId == dstPointId)
        //    {
        //        if (scoreWallController)
        //        {
        //            scoreWallController.AddScore(1);
        //        }

        //        if (gameManager)
        //        {
        //            gameManager.removeDstPoint(this);
        //            gameManager.generateRandDstPoint(dstPointId);
        //        }

        //        Destroy(this.gameObject);
        //        Destroy(this);
        //    }
        //}

        if (srcPoint)
        {
            if (gameManager)
            {
                gameManager.dstPointTouchEnter(srcPoint, this);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SrcPoint srcPoint = collision.GetComponent<SrcPoint>();
        if (srcPoint)
        {
            if (gameManager)
            {
                gameManager.dstPointTouchExit(srcPoint, this);
            }
        }
    }

    public void changeTouchState(DstPointTouchState state)
    {
        touchState = state;
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
