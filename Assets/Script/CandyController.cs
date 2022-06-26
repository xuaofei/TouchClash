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
    public SrcPoint test;

    private UnityTimer.Timer timer;
    private Tween lastTween;
    private Vector3 lastSrcPointPos;

    private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        timer = UnityTimer.Timer.Register(0.5f, () =>
        {
            //randMove();
        }, isLooped: true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate((test.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime);
        //transform.R
    }

    private void OnDestroy()
    {
        timer.Pause();
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

    public void randMove()
    {
        PathType pathType = PathType.CatmullRom;
        List<Vector3> path = new List<Vector3>();
        path.Add(transform.position);//起始点

        if (lastTween != null)
        {
            path.Add(lastTween.PathGetPoint(0.15f));

            pathType = PathType.CubicBezier;
            lastTween.Pause();
            lastTween = null;
        }

        path.Add(test.transform.position);//终点

        lastTween = transform.DOPath(path.ToArray(), 10f, pathType);


        Debug.Log("xaflog xxxxxxxxxxxxxxxxxxxxxxxxxxx");
        foreach (Vector3 v3 in path.ToArray())
        {

            
            Debug.Log("xaflog DOPATH:" + v3.ToString());
        }

        lastTween.onComplete = () =>
        {

        };

        //lastTween.debugTargetId = "";
        

        lastSrcPointPos = test.transform.position;
    }
}
