using UnityEngine;
using TheKiwiCoder;

public class FindPlayer : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.player = GameObject.FindGameObjectWithTag("Player");

        return blackboard.player ? State.Success : State.Failure;
    }
}