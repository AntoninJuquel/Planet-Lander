using UnityEngine;

namespace Game
{
    public class AspectRatioManager : MonoBehaviour
    {
        [Tooltip("Aspect Ratio to use for game.  If Vector2.zero, the default aspect ratio will be used.")] [SerializeField]
        private Vector2Int aspectRatio = Vector2Int.zero;

        [Tooltip("Whether or not full screen will be used")] [SerializeField]
        private bool fullScreen = false;

        private void Awake()
        {
            if (aspectRatio != Vector2.zero)
            {
                var x = Screen.height * (aspectRatio.x / aspectRatio.y);
                var y = Screen.height;
                Screen.SetResolution(x, y, fullScreen);
            }
        }
    }
}