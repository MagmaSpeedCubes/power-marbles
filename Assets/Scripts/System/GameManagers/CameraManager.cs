using UnityEngine;

using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private GameObject camera;



    private float currentScale = 1f;
    private bool isPanning = false;
    private Vector2 lastMousePosition;

    void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        HandleDesktopInput();
        #endif
        #if UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
        #endif
    }

        
    void HandleTouchInput()
    {

        if (Touchscreen.current == null)
            return;

        var touches = Touchscreen.current.touches;

        // 1 finger → pan
        if (touches.Count > 0 && touches[0].isInProgress && touches.Count == 1)
        {
            Vector2 delta = touches[0].delta.ReadValue();
            Pan(delta);
        }

        // 2 fingers → pinch zoom
        if (touches.Count >= 2 &&
            touches[0].isInProgress &&
            touches[1].isInProgress)
        {
            Vector2 prevPos1 = touches[0].position.ReadValue() - touches[0].delta.ReadValue();
            Vector2 prevPos2 = touches[1].position.ReadValue() - touches[1].delta.ReadValue();

            float prevDistance = Vector2.Distance(prevPos1, prevPos2);
            float currentDistance = Vector2.Distance(
                touches[0].position.ReadValue(),
                touches[1].position.ReadValue());

            float pinchDelta = (currentDistance - prevDistance) * 0.001f;
            ApplyZoom(pinchDelta);
        }

    }

    void HandleDesktopInput()
    {

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
            ApplyZoom(scroll > 0 ? zoomSpeed : -zoomSpeed);

        if (Keyboard.current.equalsKey.wasPressedThisFrame ||
            Keyboard.current.numpadPlusKey.wasPressedThisFrame)
            ApplyZoom(zoomSpeed);

        if (Keyboard.current.minusKey.wasPressedThisFrame ||
            Keyboard.current.numpadMinusKey.wasPressedThisFrame)
            ApplyZoom(-zoomSpeed);

        bool middleMouse = Mouse.current.middleButton.isPressed;
        bool shiftLeftMouse = Keyboard.current.leftShiftKey.isPressed &&
                            Mouse.current.leftButton.isPressed;

        if ((middleMouse || shiftLeftMouse) && !isPanning)
        {
            isPanning = true;
            lastMousePosition = Mouse.current.position.ReadValue();
        }
        else if (!(middleMouse || shiftLeftMouse))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector2 current = Mouse.current.position.ReadValue();
            Pan(current - lastMousePosition);
            lastMousePosition = current;
        }

    }



    void Pan(Vector2 delta)
    {
        float panMultiplier =
            4f * camera.GetComponent<Camera>().orthographicSize
            / Constants.SCREEN_SIZE.y;

        camera.transform.position -= (Vector3)delta * panMultiplier;
    }

    void ApplyZoom(float delta)
    {
        currentScale = Mathf.Clamp(currentScale + delta, minScale, maxScale);
        camera.GetComponent<Camera>().orthographicSize =
            currentScale * Constants.CAMERA_DEFAULT_SIZE;
    }
}