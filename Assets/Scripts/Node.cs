using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public static float dt = 0.02f;
    public static float criticalStickForce = 1;
    public static float dampConst = 1f;
    public bool constrain;
    public GameObject obj;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 positionTmp;
    public Vector2 velocityTmp;
    public List<Bond> bonds = new List<Bond>();
    public bool stick = false;
    public Vector2 Force(Vector2 tempPos, bool breakOrSlip = false)
    {
        Vector2 force = Vector2.zero;
        foreach (var item in bonds)
        {
            if (item == null) continue;
            Node otherNode = item.GetConnnectedNode(this);
            Vector2 vec = otherNode.position - tempPos;
            float dist = vec.magnitude;
            Vector2 direction = vec / dist;
            Vector2 forceFromBond = Bond.K * (dist - Bond.L) * direction;
            if (breakOrSlip)
            {
                float bondStrain = forceFromBond.magnitude;
                item.strain += bondStrain;
            }
            force += forceFromBond;
        }
        if (breakOrSlip && force.sqrMagnitude < criticalStickForce * criticalStickForce)
        {
            stick = true;
        }
        else stick = false;
        return force - dampConst * velocity;
    }
    public void UpdateTmp()
    {
        Vector2 force = Force(position, true);
        if (constrain || stick)
        {
            velocityTmp = Vector2.zero;
            positionTmp = position;
            return;
        }
        Vector2 vel = velocity;
        Vector2 k1v = force * dt;
        Vector2 k1x = vel * dt;

        Vector2 k2v = Force(position + k1x / 2f) * dt;
        Vector2 k2x = (vel + k1v / 2f) * dt;

        Vector2 k3v = Force(position + k2x / 2f) * dt;
        Vector2 k3x = (vel + k2v / 2f) * dt;

        Vector2 k4v = Force(position + k3x) * dt;
        Vector2 k4x = (vel + k3v) * dt;

        velocityTmp = vel + (k1v + 2f * k2v + 2f * k3v + k4v) / 6f;
        positionTmp = position + (k1x + 2f * k2x + 2f * k3x + k4x) / 6f;

        // velocityTmp = vel + dt * Force(position);
        // positionTmp = position + vel * dt;
    }
    public void Update()
    {
        velocity = velocityTmp;
        position = positionTmp;
        obj.transform.position = position;
    }
}