using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CircleCrackMain : MonoBehaviour
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
    public float initialDistance;
    [SerializeField] List<Node> nodes = new List<Node>();
    [SerializeField] List<Bond> bonds = new List<Bond>();
    private void Start()
    {
        OnValidate();

        for (int r = 0; r < radius; r++)
        {
            if (r == 0)
            {
                Node newNode = new Node();
                newNode.obj = Instantiate(nodePrefab);
                Vector3 position = Vector3.zero;
                newNode.position = position;
                newNode.obj.transform.position = position;
                nodes.Add(newNode);
            }
            for (int i = 0; i < r * 6; i++)
            {
                Node newNode = new Node();
                newNode.obj = Instantiate(nodePrefab);
                float theta = i * Mathf.PI / (3f * r);
                Vector3 position = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * r * initialDistance;
                newNode.position = position;
                newNode.obj.transform.position = position;
                if (i != 0) SpawnNewBond(newNode, nodes[nodes.Count - 1]);
                if (i == r * 6 - 1) SpawnNewBond(newNode, nodes[nodes.Count - r * 6 + 1]);
                if (r == 1) SpawnNewBond(newNode, nodes[0]);

                if (r > 1)
                {
                    int lastFirst = nodes.Count - i - (r - 1) * 6;
                    int newIndex = i - ((i - 1) / r + 1);
                    if (i == 0) newIndex = 0;
                    SpawnNewBond(newNode, nodes[lastFirst + newIndex]);
                    if (i % r != 0)
                    {
                        if (i == r * 6 - 1) SpawnNewBond(newNode, nodes[(lastFirst + newIndex + 1) - (r - 1) * 6]);
                        else SpawnNewBond(newNode, nodes[(lastFirst + newIndex + 1)]);
                    }
                }
                if (r == radius - 1) newNode.constrain = true;
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