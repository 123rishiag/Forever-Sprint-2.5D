using UnityEngine;

public class InputService : MonoBehaviour
{
    // Private Variables
    private InputControls inputControls;

    private void Awake()
    {
        inputControls = new InputControls();
    }
    private void OnEnable()
    {
        inputControls.Enable();
    }
    private void Update()
    {
        IsMoving = inputControls.Player.Move.IsPressed();
        IsJumping = inputControls.Player.Jump.IsPressed();
        WasJumpPressed = inputControls.Player.Jump.WasPressedThisFrame();
    }
    private void OnDisable()
    {
        inputControls.Disable();
    }

    public bool IsMoving { get; private set; }

    public bool IsJumping { get; private set; }

    public bool WasJumpPressed { get; private set; }
}
