using System.Collections.Generic;
using UnityEngine;

public class DrawConeMesh : MonoBehaviour
{
    [SerializeField] private int m_verticesNum = 4;
    [SerializeField] private float m_radius = 0f;
    [SerializeField] private float m_length = 0f;
    [SerializeField] private Mesh m_mesh = null;
    [SerializeField] private Transform m_followTarget = null;
    [SerializeField, HideInInspector] private Vector3[] m_vertices;
    [SerializeField, HideInInspector] private int[] m_triangles;
    private Transform m_tf = null;
    private MeshRenderer m_meshRenderer = null;
    private Color m_meshColor = Color.white;
    public float SetRadius
    {
        set { m_radius = value; }
    }
    public float SetLength
    {
        set { m_length = value; }
    }
    public Color ChangeMeshColor
    {
        set { m_meshColor = value; }
    }
    public Transform SetTransform
    {
        set { m_followTarget = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (m_mesh == null)
        {
            m_mesh = GetComponent<MeshFilter>().mesh;
        }
        m_tf = GetComponent<Transform>();
        m_meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        m_vertices = GetVerticesPoint().ToArray();
        m_triangles = GetTriangles();
        m_meshRenderer.material.color = m_meshColor;


        m_mesh.Clear();
        m_mesh.vertices = m_vertices;
        m_mesh.triangles = m_triangles;
    }
    private void LateUpdate()
    {
        //m_tf.position = m_followTarget.position;
        //m_tf.rotation = m_followTarget.rotation * Quaternion.AngleAxis(90f, Vector3.up);
    }
    [ContextMenu("CreatMesh")]
    private void CreatMesh()
    {
        m_mesh = new Mesh();

        //transform.position = m_followTarget.position;
        //transform.rotation = m_followTarget.rotation;
        //m_mesh.vertices = new Vector3[]
        //{
        //    new Vector3(-5,0,0),
        //    new Vector3(5,0,0),
        //    new Vector3(0,5,0)
        //};
        //m_mesh.triangles = new int[]
        //{
        //    0,1,2
        //};
        m_vertices = GetVerticesPoint().ToArray();
        m_triangles = GetTriangles();


        m_mesh.Clear();

        m_mesh.vertices = m_vertices;
        m_mesh.triangles = m_triangles;

        GetComponent<MeshFilter>().mesh = m_mesh;
    }
    private void OnValidate()
    {
        CreatMesh();
    }
    private List<Vector3> GetVerticesPoint()
    {
        List<Vector3> points = new List<Vector3>();
        float step = 2.0f * Mathf.PI / (float)m_verticesNum;

        float x, y;
        //円錐の頂点（とがってる部分）の頂点
        points.Add(new Vector3(0f, 0f, 0f));
        //底面（円）の頂点
        for (int i = 0; i < m_verticesNum; i++)
        {
            x = m_radius * Mathf.Cos(i * step);
            y = m_radius * Mathf.Sin(i * step);
            points.Add(new Vector3(x, y, m_length));
        }
        return points;
    }
    private int[] GetTriangles()
    {
        List<int> triangles = new List<int>();
        //頂点から底面を結ぶ三角形
        for (int i = 0; i < m_vertices.Length - 2; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
        }
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(m_vertices.Length - 1);
        //底面を結ぶ三角形
        for (int i = 0; i < m_vertices.Length - 2; i++)
        {
            triangles.Add(1);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }
        return triangles.ToArray();
    }

}
