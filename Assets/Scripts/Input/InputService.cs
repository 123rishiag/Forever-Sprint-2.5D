namespace ServiceLocator.Controls
{
    public class InputService
    {
        // Private Variables
        private InputControls inputControls;

        public InputService()
        {
            inputControls = new InputControls();
        }
        public void Init()
        {
            inputControls.Enable();
        }
        public void Destroy()
        {
            inputControls.Disable();
        }
        public void Update()
        {
            WasJumpPressed = inputControls.Player.Jump.WasPressedThisFrame();
            IsSlidePressed = inputControls.Player.Slide.IsPressed();
            IsDashPressed = inputControls.Player.Dash.IsPressed();
            IsEscapePressed = inputControls.Game.Escape.IsPressed();
        }

        public bool WasJumpPressed { get; private set; }

        public bool IsSlidePressed { get; private set; }

        public bool IsDashPressed { get; private set; }

        public bool IsEscapePressed { get; private set; }
    }
}
