using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond
{
    // Natural Length
    public static float L = 1;
    // Hook constant
    public static float criticalBreakForce = 10f;
    public static float K = 1;
    public List<Node> nodes = new List<Node>();
    public BondLineRenderer renderer;
    public float strain = 0;

    public Node GetConnnectedNode(Node node)
    {
        if (node == nodes[0]) return nodes[1];
        else if (node == nodes[1]) return nodes[0];
        return null;
    }
    public Bond(Node node0, Node node1)
    {
        nodes.Add(node0);
        nodes.Add(node1);
        node0.bonds.Add(this);
        node1.bonds.Add(this);
    }
    public void Update()
    {
        if (strain > criticalBreakForce || renderer.debugBool)
        {
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