using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SrcPointActiveState
{
    UNACTIVE,       //未激活状态
    ACTIVE          //已经激活
}

public enum SrcPointMovementState
{
    IDLE,           //手指松开，空闲状态
    RUN             //手指按住，移动状态
}

public struct SrcPointPosInfo
{
    public int srcPointId;
    // 位置
    public Vector2 pos;
    //// 半径
    //public float radius;
};

public class SrcPoint : MonoBehaviour
{
    // 大小
    public SrcPointActiveState activeState;
    public SrcPointMovementState movementState;
    public Animation animation;
    public int fingerId;                    //触摸ID
    public int srcPointId;                  //dstPointId相同
    public Color color;
    public float immuneDuration = 2.0f;

    List<string> immuneTargetList = new List<string>();

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        activeState = SrcPointActiveState.UNACTIVE;
        movementState = SrcPointMovementState.IDLE;
        fingerId = -1;

        GameObject map = GameObject.Find("map");
        if (map)
        {
            gameManager = map.GetComponent<GameManager>();
        }

        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (sp)
        {
            sp.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeMovementState(SrcPointMovementState state)
    {
        movementState = state;
        //Debug.Log("changeMovementState");
    }

    public bool canMove()
    {
        return movementState == SrcPointMovementState.RUN;
    }

    public void setFingerID(int finger)
    {
        fingerId = finger;
    }

    /// <summary>
    /// 获取原点的屏幕半径
    /// </summary>
    /// <returns>屏幕半径</returns>
    public float getPointScreenRadius()
    {
        float pointScreenHeight = 0.0f;
        // 物体大小，摄像机大小，非屏幕大小。
        Vector2 pointSize = GetComponent<SpriteRenderer>().bounds.size;
        float cameraSize = Camera.main.orthographicSize;
        // 物体高度，屏幕大小。
        pointScreenHeight = Screen.height / (cameraSize * 2) * pointSize.y;

        return pointScreenHeight / 2;
    }

    // 无敌状态
    public void Invincible()
    {
        //transform.TransformPoint
        //Invincible
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float ddd = getPointScreenRadius();
        Debug.Log("xaf srcpoint OnTriggerEnter2D");

        DstPoint dstPoint = collision.GetComponent<DstPoint>();
        if (dstPoint)
        {
            gameManager.DstPointTouchEnter(this, dstPoint);
      
        }

        MinefieldController minefieldController = collision.GetComponent<MinefieldController>();
        if (minefieldController)
        {
            if (immuneTargetList.Contains(minefieldController.minefieldName))
            {
                // 还在免疫时间内
                Debug.Log("xaf already in immuneTarge, not add:" + minefieldController.minefieldName);
                return;
            }

            // 加入免疫列表
            immuneTargetList.Add(minefieldController.minefieldName);
            Handheld.Vibrate();
            Debug.Log("xaf immuneTarge add:" + minefieldController.minefieldName);

            gameManager.ReduceBloodHitMinefield(this);

            // 开启免疫倒计时
            UnityTimer.Timer timer = UnityTimer.Timer.Register(immuneDuration, ()=> {
                immuneTargetList.Remove(minefieldController.minefieldName);

                Debug.Log("xaf immuneTarge Remove:" + minefieldController.minefieldName);
            });
        }


        BombController bombController = collision.GetComponent<BombController>();
        if (bombController)
        {
            gameManager.ReduceBloodHitBomb(this, bombController);
            Handheld.Vibrate();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("xaf srcpoint OnTriggerEnter2D");

        DstPoint dstPoint = collision.GetComponent<DstPoint>();
        if (dstPoint)
        {
            gameManager.DstPointTouchExit(this, dstPoint);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("xaf srcpoint OnCollisionEnter2D");


        //collision.collider.bounds;

        //transform.localScale
    }

    public SrcPointPosInfo GetPosInfo()
    {
        SrcPointPosInfo srcPointPosInfo;
        srcPointPosInfo.srcPointId = srcPointId;
        Camera camera = Camera.main;
        srcPointPosInfo.pos = camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        return srcPointPosInfo;
    }
}
