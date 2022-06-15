using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

// 积分墙
public class ScoreWallController : MonoBehaviour
{
    public Text score;
    public Text countdown;
    public SpriteRenderer circleSprite;

    private float totalDuration = 60.0f;
    private float costDuration = 0.0f;
    private int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "0";
        countdown.text = totalDuration.ToString("f0");
    }

    // Update is called once per frame
    void Update()
    {
        costDuration += Time.deltaTime;
        float remainTime = totalDuration - costDuration;
        if (remainTime >= 0.0f)
        {
            countdown.text = remainTime.ToString("f0");
        }
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
        //totalScore += num;

        //score.text = totalScore.ToString();

        //Debug.Log("xaf currentThread:" + Thread.CurrentThread.ManagedThreadId.ToString());

    }

    public void EduceBlood(int num)
    {
        //totalScore -= num;

        //score.text = totalScore.ToString();

        //Debug.Log("xaf currentThread:" + Thread.CurrentThread.ManagedThreadId.ToString());

    }



    public Vector2 getScoreWallWorldPos()
    {
        Vector2 size = new Vector2(circleSprite.transform.position.x, circleSprite.transform.position.y);
        return size;
    }

    public float getScoreWallScreenRadius()
    {
        float pointScreenHeight = 0.0f;
        // 物体大小，摄像机大小，非屏幕大小。
        Vector2 pointSize = circleSprite.GetComponent<SpriteRenderer>().bounds.size;
        float cameraSize = Camera.main.orthographicSize;
        // 物体高度，屏幕大小。
        pointScreenHeight = Screen.height / (cameraSize * 2) * pointSize.y;

        return pointScreenHeight / 2;
    }

    public void resetCountdown()
    {
        costDuration = 0.0f;
    }
}
