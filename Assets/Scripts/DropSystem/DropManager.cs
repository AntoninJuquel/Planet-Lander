using MessagingSystem;
using UnityEngine;

namespace DropSystem
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField] private DropPreset[] dropPresets;
        [SerializeField] private Drop dropPrefab;

        private void OnEnable()
        {
            EventManager.Instance.AddListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<EntityKilledEvent>(EntityKilledHandler);
        }

        private void Drop(Vector3 position)
        {
            foreach (var dropPreset in dropPresets)
            {
                var chance = Random.value;
                if (chance > dropPreset.chance) continue;
                var drop = Instantiate(dropPrefab, position, Quaternion.identity);
                drop.Setup(dropPreset);
                break;
            }
        }

        private void EntityKilledHandler(EntityKilledEvent e)
        {
            if (!e.Transform.CompareTag("Enemy")) return;

            Drop(e.Transform.position);
        }
    }
}