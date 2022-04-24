using MessagingSystem;
using ReferenceSharing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = ScreenNavigation.Screen;

namespace UI
{
    [System.Serializable]
    internal struct TextRef<T>
    {
        public TextMeshProUGUI text;
        public Reference<T> variableRef;
        public string sub;
    }

    public class GameOver : Screen
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button next;

        [SerializeField] private TextRef<float>[] textFloatRefs;
        [SerializeField] private TextRef<int>[] textIntRefs;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<GameOverEvent>(GameOverHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<GameOverEvent>(GameOverHandler);
        }

        private void GameOverHandler(GameOverEvent e)
        {
            title.text = (e.Win ? "VICTOIRE" : "DEFAITE");
            next.interactable = e.Win;

            foreach (var textFloatRef in textFloatRefs)
            {
                var text = textFloatRef.text;
                text.text = textFloatRef.variableRef.Value.ToString("0.00") + textFloatRef.sub;
            }

            foreach (var textIntRef in textIntRefs)
            {
                var text = textIntRef.text;
                text.text = textIntRef.variableRef.Value.ToString() + textIntRef.sub;
            }
        }
    }
}