using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    // public AudioSource bgm;

    // private static BGMController instance;
    // private void Awake()
    // {
    //     if ()
    //     instance = this;
    //     DontDestroyOnLoad(this.gameObject);
    // }



    private static BGMController instance = null;
    public static BGMController Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);//使对象目标在加载新场景时不被自动销毁。
    }
}
