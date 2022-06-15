using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GuideLine : MonoBehaviour
{
    public SrcPoint srcPoint;
    public DstPoint dstPoint;

    //线条粗细
    private float lineSize = 0.05f;
    private LineRenderer lr;
    //线条材质
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.name = "GuideLine";
        //动态添加LineRenderer组件
        //line.transform.SetParent(transform);
        
        //初始化
        InitLineRender();

        Debug.Log("InitLineRender InitLineRender");

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector1 = new Vector3(srcPoint.transform.position.x, srcPoint.transform.position.y, transform.position.z);
        Vector3 vector2 = new Vector3(dstPoint.transform.position.x, dstPoint.transform.position.y, transform.position.z);
        List<Vector3> LinePos = new List<Vector3> { vector1, vector2 };


        //lr.SetPosition(0, transform.position);
        lr.SetPosition(0, vector1);
        lr.SetPosition(1, vector2);
        //lr.SetPosition(3, transform.position);

        lr.SetPositions(LinePos.ToArray());
    }


    void InitLineRender()
    {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.transform.SetParent(transform);

        //");

        //lr.material = new Material(Shader.Find(Shader.Find("UI/Unlit/Transparent")));


        Shader sh = Shader.Find("UI/Unlit/Transparent");
        if (sh)
        {
            Debug.Log("xaf Shader.Find");
        }
        else
        {
            Debug.Log("xaf not Shader.Find");
        }

        lr.material = new Material(Shader.Find("UI/Unlit/Transparent"));
        //AssetDatabase.CreateAsset(mat, "Assets/mat.mat");
        //Material mat = AssetDatabase.LoadAssetAtPath("Assets/Res/m1.mat", typeof(Material)) as Material;
        //GameObject cube = GameObject.Find("Cube");
        //cube.GetComponent<Renderer>().material = mat;



        //lr.material = material;//设置材质
        lr.startColor = Color.red;
        lr.endColor = Color.red; ;
        lr.startWidth = lineSize;//设置线的宽度
        lr.endWidth = lineSize;
        lr.numCapVertices = 2;//设置端点圆滑度
        lr.numCornerVertices = 2;//设置拐角圆滑度，顶点越多越圆滑
        lr.positionCount = 2;//设置构成线条的点的数量


    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
