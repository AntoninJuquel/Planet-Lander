using System.Collections;
using MessagingSystem;
using ReferenceSharing;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Reference<Vector3> offsetRef;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime, range;
    private Transform _target;
    private Vector3 _camVelocity;
    private Coroutine _followTargetRoutine;
    private WaitForFixedUpdate _fixedUpdate;

    private void Awake()
    {
        _fixedUpdate = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
    }

    private void PlayerSpawnHandler(PlayerSpawnEvent e)
    {
        _target = e.Player;
        if (_followTargetRoutine != null) StopCoroutine(_followTargetRoutine);
        _followTargetRoutine = StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (_target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + offset + (offsetRef.Value * range), ref _camVelocity, smoothTime);
            yield return _fixedUpdate;
        }
    }
}