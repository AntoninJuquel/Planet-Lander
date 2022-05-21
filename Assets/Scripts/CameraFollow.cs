using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;
    private Transform _target;
    private Vector3 _velocity;
    private Coroutine _follow;

    public void SetTarget(GameObject target) => SetTarget(target.transform);

    public void SetTarget(Transform target)
    {
        _target = target;
        if (_follow != null) StopCoroutine(_follow);
        _follow = StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (_target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + offset, ref _velocity, smoothTime);
            yield return new WaitForFixedUpdate();
        }
    }
}