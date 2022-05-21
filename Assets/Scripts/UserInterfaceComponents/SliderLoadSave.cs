using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SliderLoadSave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI percent;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnSliderChanged);
        _slider.value = PlayerPrefs.GetFloat(gameObject.name, _slider.value);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        PlayerPrefs.SetFloat(gameObject.name, value);
        percent.text = $"{value * 100f: 00}%";
    }
}