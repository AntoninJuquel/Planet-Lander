using UnityEngine;

namespace DropSystem
{
    public class Drop : MonoBehaviour
    {
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void Setup(DropPreset preset)
        {
            _sr.sprite = preset.sprite;
            _sr.color = preset.color;
            gameObject.tag = preset.tag;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player") && !col.CompareTag("World")) return;
            Destroy(gameObject);
        }
    }
}