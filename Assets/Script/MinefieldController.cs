using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MinefieldController : MonoBehaviour
{
    public int minefieldId;
    public string minefieldName;
    private GameManager gameManager;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    PolygonCollider2D polygonCollider2D;
    List<Vector3> meshPosList;

    // Start is called before the first frame update
    void Start()
    {
        minefieldId = 1;
        minefieldName = "minefield_1";

        GameObject map = GameObject.Find("map");
        if (map)
        {
            gameManager = map.GetComponent<GameManager>();
        }

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        meshPosList = new List<Vector3>();
        

        Mesh mesh = new Mesh();

        int width = Screen.width;
        int height = Screen.height;

        meshPosList.Add(screenToWorldPoint(new Vector3(width / 2, height / 2, 0)));
        meshPosList.Add(screenToWorldPoint(new Vector3(width / 2, height, 0)));
        meshPosList.Add(screenToWorldPoint(new Vector3(width, height, 0)));

        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = meshPosList.ToArray();
        mesh.triangles = triangles;
        meshFilter.mesh = mesh;

        List<Vector2> v2List = new List<Vector2>();
        foreach(Vector3 v3 in meshPosList)
        {
            v2List.Add(new Vector2(v3.x, v3.y));
        }

        polygonCollider2D.SetPath(0, v2List.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Vector3 screenToWorldPoint(Vector3 screenPosition)
    {
        Camera camera = Camera.main;
        return camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, transform.position.z));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("xaf minefield OnTriggerEnter");

        SrcPoint srcPoint = collision.GetComponent<SrcPoint>();
        if(srcPoint)
        {
            gameManager.ReduceBloodHitMinefield(srcPoint);
        }

        
    }
}
