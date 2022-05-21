using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Shoot : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!blackboard.player) return State.Failure;
        
        if (Vector2.Distance(blackboard.player.transform.position, context.transform.position) > blackboard.range)
            return State.Failure;

        Debug.Log("Shoot");

        return State.Running;
    }
}