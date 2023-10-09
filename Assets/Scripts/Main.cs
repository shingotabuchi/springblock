using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Main : MonoBehaviour
{
    public float setL = Bond.L;
    public float setK = Bond.K;
    public float setFcb = Bond.criticalBreakForce;
    public float setFcs = Node.criticalStickForce;
    private void OnValidate()
    {
        Bond.L = setL;
        Bond.K = setK;
        Bond.criticalBreakForce = setFcb;
        Node.criticalStickForce = setFcs;
    }
    public GameObject nodePrefab;
    public GameObject bondRendererPrefab;
    public int width;
    public int height;
    public float initialDistance;
    [SerializeField] List<Node> nodes = new List<Node>();
    [SerializeField] List<Bond> bonds = new List<Bond>();
    private void Start()
    {
        Vector3 startPos = width / 2 * new Vector3(-1, -1, 0);
        OnValidate();
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

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Node newNode = new Node();
                newNode.obj = Instantiate(nodePrefab);
                Vector3 position = startPos + new Vector3(i, j, 0) * initialDistance;
                newNode.position = position;
                newNode.obj.transform.position = position;

                if (i == 0 || i == width - 1 || j == height - 1) newNode.constrain = true;

                // if (i > 0 && !(j == 0 && i == width / 2)) SpawnNewBond(newNode, nodes[nodes.Count - 1]);
                if (i > 0 && !(j == 0 && i == width / 2)) SpawnNewBond(newNode, nodes[nodes.Count - 1]);
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
        newBond.renderer = newBondRenderer;
        bonds.Add(newBond);
    }

    private void FixedUpdate()
    {
        foreach (var item in nodes)
        {
            item.UpdateTmp();
        }
        foreach (var item in nodes)
        {
            item.Update();
        }
        // for (int i = 0; i < bondsToDestroy.Count; i++)
        // {
        //     Bond bond = bondsToDestroy[i];
        //     Destroy(bond.obj);
        //     foreach (var item in bond.nodes)
        //     {
        //         item.bonds.Remove(bond);
        //     }
        //     bond = null;
        // }
        foreach (var item in bonds)
        {
            item.Update();
        }
        // bondsToDestroy.Clear();
    }
}