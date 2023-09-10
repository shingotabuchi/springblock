using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond
{
    // Natural Length
    public static float L = 1;
    // Hook constant
    public static float K = 1;
    List<Node> nodes = new List<Node>();

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
}