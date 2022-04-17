using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Gamepad _input;

    private void Awake()
    {
        _input = Gamepad.current;
    }
}
