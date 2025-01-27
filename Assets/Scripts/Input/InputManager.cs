using UnityEngine;

public class InputManager : MonoBehaviour
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
        WasJumpPressed = inputControls.Player.Jump.WasPressedThisFrame();
        IsSlidePressed = inputControls.Player.Slide.IsPressed();
        IsDashPressed = inputControls.Player.Dash.IsPressed();
    }
    private void OnDisable()
    {
        inputControls.Disable();
    }

    public bool WasJumpPressed { get; private set; }

    public bool IsSlidePressed { get; private set; }

    public bool IsDashPressed { get; private set; }
}
