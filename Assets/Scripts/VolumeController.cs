using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    private Coroutine _flash;
    private VolumeProfile _profile;

    private ChromaticAberration _chromaticAberration;
    [SerializeField] private float chromaticMult;
    private float _chromaticMin, _bloomMin, _lensDistortionMin;

    private Bloom _bloom;
    [SerializeField] private float bloomMult;

    private LensDistortion _lensDistortion;
    [SerializeField] private float lensDistortionMult;

    private void Awake()
    {
        _profile = GetComponent<Volume>().profile;

        _profile.TryGet(out _chromaticAberration);
        _chromaticMin = _chromaticAberration.intensity.value;

        _profile.TryGet(out _bloom);
        _bloomMin = _bloom.intensity.value;

        _profile.TryGet(out _lensDistortion);
        _lensDistortionMin = _lensDistortion.intensity.value;
    }

    private IEnumerator Flash()
    {
        var timer = 0f;

        while (timer < curve.keys[^1].time)
        {
            timer += Time.deltaTime;

            _chromaticAberration.intensity.Override(_chromaticMin + curve.Evaluate(timer) * chromaticMult);
            _bloom.intensity.Override(_bloomMin + curve.Evaluate(timer) * bloomMult);
            _lensDistortion.intensity.Override(_lensDistortionMin + curve.Evaluate(timer) * lensDistortionMult);

            yield return null;
        }

        _chromaticAberration.intensity.Override(_chromaticMin);
        _bloom.intensity.Override(_bloomMin);
        _lensDistortion.intensity.Override(_lensDistortionMin);
    }

    [ContextMenu("DoFlash")]
    public void DoFlash()
    {
        if (_flash != null) StopCoroutine(_flash);
        StartCoroutine(Flash());
    }

    [ContextMenu("DoMultipleFlash")]
    public void DoMultipleFlash()
    {
        StartCoroutine(MultFlashes());
    }

    public int number;

    IEnumerator MultFlashes()
    {
        for (int i = 0; i < number; i++)
        {
            DoFlash();
            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }
    }
}