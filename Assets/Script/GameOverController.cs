using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityTimer.Timer timer = UnityTimer.Timer.Register(3,()=> {
            SceneManager.LoadScene(0);
        });
    }
}
