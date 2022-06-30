using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BombPosInfo
{
    public int bombId;
    // 位置
    public Vector2 pos;
};


public class BombController : EnemyBase
{
    public int bombId;
    public Color color;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (sp)
        {
            sp.color = color;
        }

        //speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (attackTarget)
        {
            transform.Translate((attackTarget.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime, Space.World);
            Vector3 v = (attackTarget.transform.position - transform.position).normalized;
            transform.right = v;
        }
    }

    private void OnDestroy()
    {
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
