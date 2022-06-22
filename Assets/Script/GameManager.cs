using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerCount = 3;
    public ScoreWallController scoreWallPrefab;
    public SrcPoint srcPointPrefab;
    public DstPoint dstPointPrefab;
    public BombController bombPrefab;
    public CandyController candyPrefab;

    public AudioSource triggerDstPointMusic;
    public AudioSource triggerBobmMusic;
    public AudioSource levelSuccessMusic;
    public AudioSource candyMusic;

    private ScoreWallController scoreWall;
    public bool playersReady = false;
    private List<SrcPoint> srcPointList = new List<SrcPoint>();
    private List<DstPoint> dstPointList = new List<DstPoint>();
    private List<BombController> bombList = new List<BombController>();
    private List<CandyController> candyList = new List<CandyController>();
    private List<DstPoint> touchEnterDstPoint = new List<DstPoint>();
    private List<Vector2> srcPointPos = new List<Vector2>();
    private List<GuideLine> guideLineList = new List<GuideLine>();
    private int currentGameLevel = 0;

    GameLevelManager gameLevelManager;

    List<Color> colors = new List<Color> { Color.blue, Color.yellow, Color.green, Color.red };

    // Start is called before the first frame update
    void Start()
    {
        gameLevelManager = new GameLevelManager();

        Camera.main.orthographicSize = Screen.height / 100f / 2f;


        GenerateScoreWall();
        GenerateSrcPointPos(playerCount);

        // 生成原始点
        GenerateSrcPoint();

        StartLevel(currentGameLevel);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            if (t.phase == TouchPhase.Began)
            {
                Vector2 touchPos = ScreenToWorldPoint(t.position);

                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider)
                {
                    Debug.Log("touch began:" + hit.transform.name);

                    SrcPoint srcPoint = hit.collider.GetComponent<SrcPoint>();
                    if (srcPoint)
                    {
                        srcPoint.changeMovementState(SrcPointMovementState.RUN);
                        srcPoint.setFingerID(t.fingerId);
                    }
                }

            }
            else if (t.phase == TouchPhase.Ended)
            {

                SrcPoint srcPoint = srcPointList.Find(SrcPoint => SrcPoint.fingerId == t.fingerId);
                if (srcPoint)
                {
                    Debug.Log("touch ended:" + srcPoint.name);

                    srcPoint.changeMovementState(SrcPointMovementState.IDLE);
                    srcPoint.setFingerID(-1);
                }
            }
            else if (t.phase == TouchPhase.Moved)
            {

                SrcPoint srcPoint = srcPointList.Find(SrcPoint => SrcPoint.fingerId == t.fingerId);
                if (srcPoint && srcPoint.canMove())
                {
                    Debug.Log("touch moving:" + srcPoint.name);
                    srcPoint.transform.position = ScreenToWorldPoint(t.position);
                }

                //Debug.Log("touch pos:" + t.position.ToString());
            }
        }

        playersReady = checkPlayerReady();

        if (!playersReady)
        {
            // 暂停游戏，并且警告提示用户。
            //Time.timeScale = 0;
        }
    }

    Vector2 ScreenToWorldPoint(Vector2 screenPosition)
    {
        Camera camera = Camera.main;
        return camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, transform.position.z));
    }

    Vector2 WorldToScreenPoint(Vector2 worldPosition)
    {
        Camera camera = Camera.main;
        return camera.WorldToScreenPoint(new Vector3(worldPosition.x, worldPosition.y, transform.position.z));
    }

    private bool checkPlayerReady()
    {
        foreach (SrcPoint srcPoint in srcPointList)
        {
            if (!srcPoint.canMove())
            {
                return false;
            }
        }
        return true;
    }

    private void GenerateSrcPointPos(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (i == 0)
            {
                srcPointPos.Add(new Vector2(Screen.width / 4, Screen.height / 2));
            }
            else if (i == 1)
            {
                srcPointPos.Add(new Vector2(Screen.width / 2, Screen.height / 4 * 3));
            }
            else if (i == 2)
            {
                srcPointPos.Add(new Vector2(Screen.width / 4 * 3, Screen.height / 2));
            }
            else if (i == 3)
            {
                srcPointPos.Add(new Vector2(Screen.width / 4, Screen.height / 4 * 3));
            }
        }
    }

    public void RemoveDstPoint(DstPoint dstPoint)
    {
        dstPointList.RemoveAt(dstPointList.IndexOf(dstPoint));
    }

    public void GenerateRandDstPoint(int dstPointId)
    {
        int safeSpace = 0;
        do
        {
            System.Random rd = new System.Random();
            float srcPointScreenRadius = getSrcPointScreenRadius();
            safeSpace = ((int)srcPointScreenRadius) + 20;

            int xPos = rd.Next(safeSpace, Screen.width - safeSpace);
            int yPos = rd.Next(safeSpace, Screen.height - safeSpace);
            Vector2 newDstPointPos = new Vector2(xPos, yPos);

            Debug.Log("xaf pos newDstPointPos:" + newDstPointPos.ToString() + "srcPointScreenRadius:" + srcPointScreenRadius);

            foreach (SrcPoint _srcPoint in srcPointList)
            {
                Vector2 srcPointPos = new Vector2(_srcPoint.transform.position.x, _srcPoint.transform.position.y);
                srcPointPos = WorldToScreenPoint(srcPointPos);

                float distance = (srcPointPos - newDstPointPos).magnitude;

                Debug.Log("xaf pos _srcPoint:" + srcPointPos.ToString() + "distance:" + distance);

                if (dstPointId == _srcPoint.srcPointId)
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
                    if (distance < (srcPointScreenRadius * 3))
                    {
                        // 两个点直接距离小于1.5倍原点直径，重新生成位置。
                        goto reGenerate;
                    }
                }
            }

            foreach (DstPoint _dstPoint in dstPointList)
            {
                Vector2 dstPointPos = new Vector2(_dstPoint.transform.position.x, _dstPoint.transform.position.y);
                dstPointPos = WorldToScreenPoint(dstPointPos);

                float distance = (dstPointPos - newDstPointPos).magnitude;
                Debug.Log("xaf pos _dstPoint:" + dstPointPos.ToString() + "distance:" + distance);

                if (distance < (srcPointScreenRadius * 3))
                {
                    // 两个点直接距离小于1.5倍原点直径，重新生成位置。
                    goto reGenerate;
                }
            }

            Vector2 scoreWallWorldPos = scoreWall.getScoreWallWorldPos();
            float circleWallScreenRadius = scoreWall.getScoreWallScreenRadius();
            scoreWallWorldPos = WorldToScreenPoint(scoreWallWorldPos);

            float distanceScoreWall = (scoreWallWorldPos - newDstPointPos).magnitude;
            if (distanceScoreWall < (srcPointScreenRadius + circleWallScreenRadius) * 1.5)
            {
                goto reGenerate;
            }

            DstPoint dstPoint = Instantiate(dstPointPrefab, transform) as DstPoint;
            dstPoint.name = "dstPoint" + dstPointId.ToString();
            dstPoint.transform.position = ScreenToWorldPoint(newDstPointPos);
            dstPoint.dstPointId = dstPointId;
            dstPoint.color = colors[dstPointId];

            dstPointList.Add(dstPoint);
            Debug.Log("dstPointList count: " + dstPointList.Count);
            return;

        reGenerate:
            {
                Debug.Log("xaf reGenerate dst point");
            };

        } while (true);

    }

    /// <summary>
    /// 获取原点的屏幕高度，原点和目标点一样大小
    /// </summary>
    /// <returns>屏幕高度</returns>
    private float getSrcPointScreenRadius()
    {
        float srcPointScreenRadius = 0.0f;
        foreach (SrcPoint srcPoint in srcPointList)
        {
            //// 物体大小，摄像机大小，非屏幕大小。
            //Vector2 pointSize = srcPoint.GetComponent<SpriteRenderer>().bounds.size;
            //float cameraSize = Camera.main.orthographicSize;
            //// 物体高度，屏幕大小。
            //pointScreenHeight = Screen.height / (cameraSize * 2) * pointSize.y;

            srcPointScreenRadius = srcPoint.getPointScreenRadius();
            break;
        }

        return srcPointScreenRadius;
    }


    public void DstPointTouchEnter(SrcPoint srcPoint, DstPoint dstPoint)
    {
        if (srcPoint.srcPointId == dstPoint.dstPointId)
        {
            touchEnterDstPoint.Add(dstPoint);
            dstPoint.changeTouchState(DstPointTouchState.ENTER);

            if (touchEnterDstPoint.Count == playerCount)
            {
                LevelSuccess(currentGameLevel);
                StopLevel(currentGameLevel);
                StartLevel(++currentGameLevel);

                levelSuccessMusic.Play();
            }
            else
            {
                triggerDstPointMusic.Play();
            }
        }
    }

    public void DstPointTouchExit(SrcPoint srcPoint, DstPoint dstPoint)
    {
        if (srcPoint.srcPointId == dstPoint.dstPointId)
        {
            touchEnterDstPoint.Remove(dstPoint);
            dstPoint.changeTouchState(DstPointTouchState.EXIT);
        }
    }

    private void GenerateScoreWall()
    {
        scoreWall = Instantiate(scoreWallPrefab, transform) as ScoreWallController;
        scoreWall.name = "scoreWall";
        scoreWall.transform.position = new Vector2(0f, 0f);
        float screenRadius = scoreWall.getScoreWallScreenRadius();
        float scale = WidgetSizeManager.Instance.scoreWallRadius / screenRadius;
        scoreWall.transform.localScale = new Vector3(scale, scale, 1f);
    }
    private void GenerateSrcPoint()
    {
        // 生成目标点
        for (int i = 0; i < playerCount; i++)
        {
            SrcPoint srcPoint = Instantiate(srcPointPrefab, transform) as SrcPoint;
            srcPoint.name = "srcPoint_" + i.ToString();
            Vector2 srtPointPos = ScreenToWorldPoint(srcPointPos[i]);
            srcPoint.transform.position = srtPointPos;
            srcPoint.srcPointId = i;
            srcPoint.color = colors[i];
            float screenRadius = srcPoint.getPointScreenRadius();
            float scale = WidgetSizeManager.Instance.srcPointRadius / screenRadius; 
            srcPoint.transform.localScale = new Vector3(scale, scale, 1f);

            srcPointList.Add(srcPoint);
        }
    }

    private void GenerateDstPoint(GameLevel gameLevel)
    {
        foreach (DstPointPosInfo dstPointPosInfo in gameLevel.dstPointPosInfos)
        {
            DstPoint dstPoint = Instantiate(dstPointPrefab, transform) as DstPoint;
            dstPoint.name = "dstPoint" + dstPointPosInfo.dstPointId.ToString();
            dstPoint.transform.position = ScreenToWorldPoint(dstPointPosInfo.pos);
            dstPoint.dstPointId = dstPointPosInfo.dstPointId;
            dstPoint.color = colors[dstPointPosInfo.dstPointId];
            float screenRadius = dstPoint.getPointScreenRadius();
            float scale = WidgetSizeManager.Instance.dstPointRadius / screenRadius;
            dstPoint.transform.localScale = new Vector3(scale, scale, 1f);

            dstPointList.Add(dstPoint);
            Debug.Log("dstPointList count: " + dstPointList.Count);
        }
    }

    private void TestGuideLine(SrcPoint srcPoint, DstPoint dstPoint)
    {
        GameObject go = new GameObject();
        go.name = "GuideLine";
        go.AddComponent<Rigidbody2D>();
        go.transform.SetParent(transform);
        GuideLine guideLine = go.AddComponent<GuideLine>();
        guideLine.srcPoint = srcPoint;
        guideLine.dstPoint = dstPoint;

        guideLineList.Add(guideLine);

        Debug.Log("testGuideLine");
    }

    private void GenerateBomb(GameLevel gameLevel)
    {
        foreach (BombPosInfo bombPosInfo in gameLevel.bombPosInfos)
        {
            BombController bomb = Instantiate(bombPrefab, transform) as BombController;
            bomb.name = "bomb" + bombPosInfo.bombId.ToString();
            bomb.transform.position = ScreenToWorldPoint(bombPosInfo.pos);
            bomb.bombId = bombPosInfo.bombId;
            bomb.color = colors[bombPosInfo.bombId];

            float screenRadius = bomb.getPointScreenRadius();
            float scale = WidgetSizeManager.Instance.bombRadius / screenRadius;
            bomb.transform.localScale = new Vector3(scale, scale, 1f);

            bombList.Add(bomb);
            Debug.Log("bombList count: " + bombList.Count);
        }
    }

    private void GenerateCandy(GameLevel gameLevel)
    {
        foreach (CandyPosInfo candyPosInfo in gameLevel.candyPosInfos)
        {
            CandyController candy = Instantiate(candyPrefab, transform) as CandyController;
            candy.name = "candy" + candyPosInfo.candyId.ToString();
            candy.transform.position = ScreenToWorldPoint(candyPosInfo.pos);
            candy.candyId = candyPosInfo.candyId;

            float screenRadius = candy.getPointScreenRadius();
            float scale = WidgetSizeManager.Instance.bombRadius / screenRadius;
            candy.transform.localScale = new Vector3(scale, scale, 1f);

            candyList.Add(candy);
            Debug.Log("candyList count: " + candyList.Count);
        }
    }

    private void GenerateGuideLine(GameLevel gameLevel) 
    {
        for (int i = 0; i < playerCount; i++)
        {
            DstPoint dstPoint = dstPointList[i];
            SrcPoint srcPoint = srcPointList[i];

            TestGuideLine(srcPoint, dstPoint);
        }
    }

    public void AddBloodHitCandy(SrcPoint srcPoint, CandyController candyController)
    {
        scoreWall.AddBlood(1);
        Destroy(candyController);
        candyMusic.Play();
    }

    public void ReduceBloodHitMinefield(SrcPoint srcPoint)
    {
        scoreWall.EduceBlood(1);
        //srcPoint.Invincible();
    }

    public void ReduceBloodHitBomb(SrcPoint srcPoint, BombController bombController)
    {
        scoreWall.EduceBlood(1);
        Destroy(bombController);
        triggerBobmMusic.Play();
        //srcPoint.Invincible();
    }

    private void StartLevel(int levelIndex)
    {
        List<SrcPointPosInfo> srcPointPosInfos = new List<SrcPointPosInfo>();
        foreach (SrcPoint srcPoint in srcPointList)
        {
            srcPointPosInfos.Add(srcPoint.GetPosInfo());
        }

        GameLevel gameLevel = gameLevelManager.GetGameLevel(levelIndex, srcPointPosInfos);

        GenerateDstPoint(gameLevel);

        GenerateGuideLine(gameLevel);

        GenerateBomb(gameLevel);

        GenerateCandy(gameLevel);
    }

    private void StopLevel(int levelIndex)
    {
        // 进入下一关，重新开始计时，DstPoint重新生成位置，计算积分。
        foreach (GuideLine guideLine in guideLineList)
        {
            Destroy(guideLine);
        }

        foreach (DstPoint dstPoint in dstPointList)
        {
            Destroy(dstPoint);
        }

        foreach (CandyController candyController in candyList)
        {
            Destroy(candyController);
        }

        foreach (BombController bombController in bombList)
        {
            Destroy(bombController);
        }


        guideLineList.Clear();
        dstPointList.Clear();
        candyList.Clear();
        bombList.Clear();
        touchEnterDstPoint.Clear();
    }

    private void LevelSuccess(int levelIndex)
    {
        scoreWall.resetCountdown();
        scoreWall.AddScore(3);
    }

    private void LevelFailed(int levelIndex)
    {

    }

    public void CountDownGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}