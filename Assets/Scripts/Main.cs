using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Main : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject bondRendererPrefab;
    public int width;
    public float initialDistance;
    List<Node> nodes = new List<Node>();
    private void Start()
    {
        Vector3 startPos = width / 2 * new Vector3(-1, -1, 0);

        // int j = 0;
        // for (int i = 0; i < width; i++)
        // {
        //     Node newNode = new Node();
        //     newNode.obj = Instantiate(nodePrefab);
        //     Vector3 position = startPos + new Vector3(i, j, 0) * initialDistance;
        //     newNode.position = position;
        //     newNode.obj.transform.position = position;

        //     if (i > 0) SpawnNewBond(newNode, nodes[nodes.Count - 1]);
        //     if (j > 0) SpawnNewBond(newNode, nodes[nodes.Count - width]);
        //     nodes.Add(newNode);
        // }

        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Node newNode = new Node();
                newNode.obj = Instantiate(nodePrefab);
                Vector3 position = startPos + new Vector3(i, j, 0) * initialDistance;
                newNode.position = position;
                newNode.obj.transform.position = position;

                if (i > 0) SpawnNewBond(newNode, nodes[nodes.Count - 1]);
                if (j > 0) SpawnNewBond(newNode, nodes[nodes.Count - width]);
                nodes.Add(newNode);
            }
        }
    }

    void SpawnNewBond(Node node0, Node node1)
    {
        Bond newBond = new Bond(node0, node1);
        BondLineRenderer newBondRenderer = Instantiate(bondRendererPrefab).GetComponent<BondLineRenderer>();
        newBondRenderer.endPoint0 = node0.obj.transform;
        newBondRenderer.endPoint1 = node1.obj.transform;
    }

    private void Update()
    {
        foreach (var item in nodes)
        {
            item.UpdateTmp();
        }
        foreach (var item in nodes)
        {
            item.Update();
        }
    }
}