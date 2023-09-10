using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public static float dt = 0.02f;
    public GameObject obj;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 positionTmp;
    public Vector2 velocityTmp;
    public List<Bond> bonds = new List<Bond>();
    public Vector2 Force(Vector2 tempPos)
    {
        Vector2 force = Vector2.zero;
        foreach (var item in bonds)
        {
            if (item == null) continue;
            Node otherNode = item.GetConnnectedNode(this);
            Vector2 vec = otherNode.position - tempPos;
            float dist = vec.magnitude;
            Vector2 direction = vec / dist;
            force += Bond.K * (dist - Bond.L) * direction;
        }
        return force;
    }
    public void UpdateTmp()
    {
        Vector2 vel = velocity;
        Vector2 k1v = Force(position) * dt;
        Vector2 k1x = vel * dt;

        Vector2 k2v = Force(position + k1x / 2f) * dt;
        Vector2 k2x = (vel + k1v / 2f) * dt;

        Vector2 k3v = Force(position + k2x / 2f) * dt;
        Vector2 k3x = (vel + k2v / 2f) * dt;

        Vector2 k4v = Force(position + k3x) * dt;
        Vector2 k4x = (vel + k3v) * dt;

        velocityTmp = vel + (k1v + 2f * k2v + 2f * k3v + k4v) / 6f;
        positionTmp = position + (k1x + 2f * k2x + 2f * k3x + k4x) / 6f;
    }
    public void Update()
    {
        velocity = velocityTmp;
        position = positionTmp;
        obj.transform.position = position;
    }
}