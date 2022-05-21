using UnityEngine;
using TheKiwiCoder;

public class MoveToPlayer : ActionNode
{
    public float speed = 5;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!blackboard.player) return State.Failure;

        var playerPosition = blackboard.player.transform.position;

        if (Vector2.Distance(context.transform.position, playerPosition) <= blackboard.range)
        {
            return State.Success;
        }

        var dir = (playerPosition - context.transform.position).normalized;

        context.rb.velocity = dir * speed;
        context.transform.up = dir;
        return State.Running;
    }
}