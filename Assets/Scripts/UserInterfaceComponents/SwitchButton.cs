using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterfaceComponents
{
    [System.Serializable]
    public struct State
    {
        public string text;
        public Sprite image;
    }

    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private State[] states;
        private int _index;

        public void Next()
        {
            _index = (_index + 1) % states.Length;
            text.text = states[_index].text;
            image.sprite = states[_index].image;
        }
    }
}