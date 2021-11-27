using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public float treeGap;

    public List<GameObject> m_DummyList;

    public Transform m_LeftStart;
    public Transform m_LeftEnd;

    public Transform m_RightStart;
    public Transform m_RightEnd;

    private List<GameObject> m_TreeList;

    private Queue<GameObject> m_Pool;

    public Transform poolObject;
    public Transform treeGroup;

    void Start()
    {
        m_Pool = new Queue<GameObject>();

        m_TreeList = new List<GameObject>();

        var posX = Camera.main.nearClipPlane + Camera.main.transform.position.x;
        var camFar = Camera.main.farClipPlane + Camera.main.transform.position.x;


        while(posX < m_LeftEnd.transform.position.x)
        {
            CreateTwoTree(posX);

            posX += treeGap;

            Debug.Log(posX);

        }
    }
    // void Update()
    // {
    //     CheckTreeList();
    // }

    void CheckTreeList()
    {
        var camNear = Camera.main.nearClipPlane + Camera.main.transform.position.x;
        var camFar = Camera.main.farClipPlane + Camera.main.transform.position.x;
        var last = m_TreeList.OrderBy(x=>x.transform.position.x);
        var posX = last.Last().transform.position.x + treeGap;

        if (posX <= camFar && camFar <= m_LeftEnd.position.x)
        {
            CreateTwoTree(posX);
        }

        foreach (var tree in m_TreeList)
        {
            if(tree.transform.position.x < camNear || tree.transform.position.x > camFar)
            {
                DisableTree(tree);
            }
        }
    }

    void CreateTwoTree(float _x)
    {
        {
            var lx = _x;
            var ly = Random.Range(m_LeftStart.position.y, m_LeftEnd.position.y);
            var lz = Random.Range(m_LeftStart.position.z, m_LeftEnd.position.z);

            CreateTree(new Vector3(lx, ly, lz));
        }

        {
            var rx = _x;
            var ry = Random.Range(m_RightStart.position.y, m_RightEnd.position.y);
            var rz = Random.Range(m_RightStart.position.z, m_RightEnd.position.z);

            CreateTree(new Vector3(rx, ry, rz));
        }
    }

    void CreateTree(Vector3 _pos)
    {
        var tree = m_Pool.Count > 0 ? m_Pool.Dequeue() : Instantiate(m_DummyList[Random.Range(0, m_DummyList.Count-1)]);

        tree.SetActive(true);

        m_TreeList.Add(tree);

        tree.transform.SetParent(treeGroup);

        tree.transform.position = _pos;
    }

    void DisableTree(GameObject _tree)
    {
        _tree.SetActive(false);
        m_Pool.Enqueue(_tree);

        m_TreeList.Remove(_tree);
        _tree.transform.SetParent(poolObject);
    }
}
