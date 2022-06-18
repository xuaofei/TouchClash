using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

// 积分墙
public class ScoreWallController : MonoBehaviour
{
    public Text score;
    public Text blood;
    public Text countdown;

    private int initBlood = 3;
    private int maxBlood = 3;
    private int currentBlood;
    private float totalDuration = 30.0f;
    private float costDuration = 0.0f;
    private int totalScore = 0;
    private UnityTimer.Timer countDownTimer;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject map = GameObject.Find("map");
        if (map)
        {
            gameManager = map.GetComponent<GameManager>();
        }

        currentBlood = initBlood;
        blood.text = currentBlood.ToString();

        score.text = "0";
        countdown.text = totalDuration.ToString("f0");

        countDownTimer = UnityTimer.Timer.Register(1f, ()=> {
            float remainTime = totalDuration - (++costDuration);
            if (remainTime >= 0.0f)
            {
                countdown.text = remainTime.ToString("f0");
            }
            else
            {
                countdown.text = "0";
                CountDownOver();
                countDownTimer = null;
            }
        }, isLooped: true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int num) {
        totalScore += num;

        score.text = totalScore.ToString();

        //Debug.Log("xaf currentThread:" + Thread.CurrentThread.ManagedThreadId.ToString());
        
    }

    public void EduceScore(int num)
    {
        totalScore -= num;

        score.text = totalScore.ToString();

        //Debug.Log("xaf currentThread:" + Thread.CurrentThread.ManagedThreadId.ToString());

    }

    public void AddBlood(int num)
    {
        currentBlood += num;
        if (currentBlood > maxBlood)
        {
            currentBlood = maxBlood;
        }

        blood.text = currentBlood.ToString();
    }

    public void EduceBlood(int num)
    {
        currentBlood -= num;
        if (currentBlood <= 0)
        {
            currentBlood = 0;
            gameManager.CountDownGameOver();
        }

        blood.text = currentBlood.ToString();
    }

    public Vector2 getScoreWallWorldPos()
    {
        Vector2 size = new Vector2(transform.position.x, transform.position.y);
        return size;
    }

    public float getScoreWallScreenRadius()
    {
        float pointScreenHeight = 0.0f;
        // 物体大小，摄像机大小，非屏幕大小。
        Vector2 pointSize = GetComponent<SpriteRenderer>().bounds.size;
        float cameraSize = Camera.main.orthographicSize;
        // 物体高度，屏幕大小。
        pointScreenHeight = Screen.height / (cameraSize * 2) * pointSize.y;

        return pointScreenHeight / 2;
    }

    public void resetCountdown()
    {
        costDuration = 0.0f;
    }


    // 倒计时结束，游戏结束。
    public void CountDownOver()
    {
        gameManager.CountDownGameOver();
    }
}
