using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderLoadSave : MonoBehaviour
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            _slider.value = PlayerPrefs.GetFloat(gameObject.name, _slider.value);
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSliderChanged);
        }

        private void OnSliderChanged(float value)
        {
            PlayerPrefs.SetFloat(gameObject.name, value);
        }
    }
}