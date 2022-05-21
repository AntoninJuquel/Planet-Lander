using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CircleAround : ActionNode
{
    [SerializeField] private float speed;
    private DistanceJoint2D _joint;

    protected override void OnStart()
    {
        if (!_joint)
            _joint = context.gameObject.AddComponent<DistanceJoint2D>();

        _joint.enabled = true;
        _joint.autoConfigureDistance = false;
        _joint.distance = blackboard.range - .1f;
        _joint.enableCollision = true;
    }

    protected override void OnStop()
    {
        _joint.enabled = false;
    }

    protected override State OnUpdate()
    {
        if (!blackboard.player) return State.Failure;

        if (Vector2.Distance(blackboard.player.transform.position, context.transform.position) > blackboard.range) return State.Failure;

        _joint.connectedAnchor = blackboard.player.transform.position;
        context.rb.velocity = context.transform.right * speed;
        context.transform.up = (blackboard.player.transform.position - context.transform.position).normalized;

        return State.Running;
    }
}