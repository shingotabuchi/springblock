using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CircleCrackMainV2 : MonoBehaviour
{
    public float setL;
    public float setK;
    public float setFcb;
    public float setFcs;
    private void OnValidate()
    {
        // Bond.L = setL;
        // Bond.K = setK;
        // Bond.criticalBreakForce = setFcb;
        Node.criticalStickForce = setFcs;
        foreach (var item in bonds)
        {
            item.criticalBreakForce = setFcb;
        }
    }
    public GameObject nodePrefab;
    public GameObject bondRendererPrefab;
    public int radius;
    public float edgeNodeInterval;
    public float ringInterval;
    public float initialDistance;
    [SerializeField] List<Node> nodes = new List<Node>();
    [SerializeField] List<Bond> bonds = new List<Bond>();
    private void Start()
    {
        OnValidate();
        int edgeNodeCount = (int)((2f * Mathf.PI * radius) / edgeNodeInterval);
        int ringCount = (int)((float)radius / ringInterval);
        int nodeCount = ringCount * edgeNodeCount;
        for (int r = 1; r <= ringCount; r++)
        {
            for (int i = 0; i < edgeNodeCount; i++)
            {
                Node newNode = new Node();
                newNode.obj = Instantiate(nodePrefab);
                float theta = (2.0f * Mathf.PI * i) / edgeNodeCount;
                Vector3 position = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * r * ringInterval;
                newNode.position = position;
                newNode.obj.transform.position = position;

                if (r > 1) SpawnNewBond(newNode, nodes[nodes.Count - edgeNodeCount]);
                if (i != 0) SpawnNewBond(newNode, nodes[nodes.Count - 1]);
                if (i == edgeNodeCount - 1) SpawnNewBond(newNode, nodes[nodes.Count - edgeNodeCount + 1]);
                if (r == ringCount) newNode.constrain = true;
                nodes.Add(newNode);
            }
        }
    }

    void SpawnNewBond(Node node0, Node node1)
    {
        Bond newBond = new Bond(node0, node1, setL, setK, setFcb);
        BondLineRenderer newBondRenderer = Instantiate(bondRendererPrefab).GetComponent<BondLineRenderer>();
        newBondRenderer.endPoint0 = node0.obj.transform;
        newBondRenderer.endPoint1 = node1.obj.transform;
        newBond.renderer = newBondRenderer;
        bonds.Add(newBond);
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