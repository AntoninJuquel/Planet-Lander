using System.Collections;
using Managers.Event;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;
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
        EventHandler.Instance.AddListener<PlayerSpawnEvent>(PlayerSpawnHandler);
    }

    private void OnDisable()
    {
        EventHandler.Instance.RemoveListener<PlayerSpawnEvent>(PlayerSpawnHandler);
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
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + offset, ref _camVelocity, smoothTime);
            yield return _fixedUpdate;
        }
    }
}