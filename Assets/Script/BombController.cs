using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BombPosInfo
{
    public int bombId;
    // 位置
    public Vector2 pos;
};


public class BombController : MonoBehaviour
{
    public int bombId;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
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

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
