using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multipleTouch : MonoBehaviour {
    public int playerCount = 1;
    public GameObject circle;
    public Camera camera;
    public List<touchLocation> touches = new List<touchLocation>();

    void Start() {
        for (int i = 0; i < playerCount; i++)
        {
            
        }
        // 生成三个点
        // 点的状态
    }

	// Update is called once per frame
	void Update () {

        int i = 0;
        while(i < Input.touchCount){
            Debug.Log("touch emter");

            Touch t = Input.GetTouch(i);
            if(t.phase == TouchPhase.Began){
                Debug.Log("touch began");
                touches.Add(new touchLocation(t.fingerId, createCircle(t)));
            }else if(t.phase == TouchPhase.Ended){
                Debug.Log("touch ended");
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                Destroy(thisTouch.circle);
                touches.RemoveAt(touches.IndexOf(thisTouch));
            }else if(t.phase == TouchPhase.Moved){
                Debug.Log("touch is moving");
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                thisTouch.circle.transform.position = getTouchPosition(t.position);
            }
            ++i;
        }
	}
    Vector2 getTouchPosition(Vector2 touchPosition){
        return camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
    GameObject createCircle(Touch t){
        GameObject c = Instantiate(circle, transform) as GameObject;
        c.name = "Touch" + t.fingerId;
        c.transform.position = getTouchPosition(t.position);

        return c;
    }
}