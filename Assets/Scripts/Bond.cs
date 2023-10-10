using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond
{
    // Natural Length
    public float L = 1;
    // Hook constant
    public float criticalBreakForce = 10f;
    public float K = 1;
    public List<Node> nodes = new List<Node>();
    public BondLineRenderer renderer;
    public float strain = 0;
    public bool destroyed = false;

    public Node GetConnnectedNode(Node node)
    {
        if (node == nodes[0]) return nodes[1];
        else if (node == nodes[1]) return nodes[0];
        return null;
    }
    public Bond(Node node0, Node node1, float initL, float initK, float cbf)
    {
        L = initL;
        K = initK;
        criticalBreakForce = cbf;
        nodes.Add(node0);
        nodes.Add(node1);
        node0.bonds.Add(this);
        node1.bonds.Add(this);
    }
    public void Update()
    {
        if (destroyed) return;
        if (strain > criticalBreakForce || renderer.debugBool)
        {
            destroyed = true;
            UnityEngine.Object.Destroy(renderer.gameObject);
            foreach (var item in nodes)
            {
                item.bonds.Remove(this);
            }
        }

        renderer.text.text = strain.ToString("0.00");
        strain = 0;
    }
}