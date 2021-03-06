using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityTimer;

public struct CandyPosInfo
{
    public int candyId;
    // 位置
    public Vector2 pos;
};

public class CandyController : MonoBehaviour
{
    public int candyId;
    //public SrcPoint test;
    UnityTimer.Timer timer;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        timer = UnityTimer.Timer.Register(1f, ()=> {
            gameObject.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //transform.Translate((test.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime, Space.World);
        //Vector3 v = (test.transform.position - transform.position).normalized;
        //transform.right = v;
    }

    private void OnDestroy()
    {
        timer.Cancel();
        timer = null;
        Destroy(gameObject);
    }

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
}
