using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MinefieldArea
{
    Area0,
    Area1,
    Area2,
    Area3,
    Area4,
    Area5,
    Area6,
    Area7,
}

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MinefieldController : MonoBehaviour
{
    public int minefieldId;
    public string minefieldName;
    public MinefieldArea minefieldArea;
    // 雷区作用对象
    public List<int> targets;

    private GameManager gameManager;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private PolygonCollider2D polygonCollider2D;
    private List<Vector3> meshPosList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        minefieldId = (int)minefieldArea;
        minefieldName = "minefield_" + minefieldId;

        GameObject map = GameObject.Find("foreground");
        if (map)
        {
            gameManager = map.GetComponent<GameManager>();
        }

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();

        int width = Screen.width;
        int height = Screen.height;

        meshPosList.Add(screenToWorldPoint(new Vector3(width / 2, height / 2, 0)));
        meshPosList.Add(screenToWorldPoint(new Vector3(width / 2, height, 0)));
        meshPosList.Add(screenToWorldPoint(new Vector3(width, height, 0)));

        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        Mesh mesh = new Mesh();
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

    Vector3 screenToWorldPoint(Vector3 screenPosition)
    {
        Camera camera = Camera.main;
        return camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, transform.position.z));
    }
}
