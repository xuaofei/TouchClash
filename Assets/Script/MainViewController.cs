using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainViewController : MonoBehaviour
{
    public void ThreePlayerGame()
    {
        GameSceneParam.Instance.gamePlayerType = GamePlayerType.LOCAL;
        GameSceneParam.Instance.playerCount = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Debug.Log("ThreePlayerGame Clicked.");
    }

    public void FourPlayerGame()
    {
        GameSceneParam.Instance.gamePlayerType = GamePlayerType.LOCAL;
        GameSceneParam.Instance.playerCount = 4;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("FourPlayerGame Clicked.");
    }

    public void FivePlayerGame()
    {
        GameSceneParam.Instance.gamePlayerType = GamePlayerType.LOCAL;
        GameSceneParam.Instance.playerCount = 5;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("FivePlayerGame Clicked.");
    }

    public void OnlinePlayerGame()
    {
        GameSceneParam.Instance.gamePlayerType = GamePlayerType.ONLINE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("OnlinePlayerGame Clicked.");
    }

    public void Ranking()
    {
        SceneManager.LoadScene("RankingScene");
        Debug.Log("Ranking Clicked.");
    }

    public void Setting()
    {
        SceneManager.LoadScene("SettingScene");
        Debug.Log("Setting Clicked.");
    }
}
