using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float dragSensitivity = 0.01f;
    [SerializeField] private float zoomSensitivity = 0.1f;
    [SerializeField] private float minOrthographicSize = 1f;
    [SerializeField] private float maxOrthographicSize = 20f;
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;

    private Vector2 dragDelta = Vector2.zero;
    private bool isDragging = false;
    private float lastPinchDistance = 0f;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Apply camera movement if dragging
        if (isDragging && dragDelta.magnitude > 0.01f)
        {
            MoveCameraByDrag(dragDelta);
        }

        // Handle two-finger pinch zoom
        if (Input.touchCount == 2)
        {
            HandlePinchZoom();
        }
        else
        {
            lastPinchDistance = 0f; // reset when fingers lift
        }
    }

    private void HandlePinchZoom()
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        // Calculate distance between fingers
        float currentDistance = Vector2.Distance(touch1.position, touch2.position);

        // On first frame of two-finger touch, initialize lastPinchDistance
        if (lastPinchDistance == 0f)
        {
            lastPinchDistance = currentDistance;
            return;
        }

        // Calculate the change in distance
        float pinchDelta = currentDistance - lastPinchDistance;
        lastPinchDistance = currentDistance;

        // Adjust camera zoom based on pinch direction
        if (mainCamera.orthographic)
        {
            // For 2D (orthographic camera)
            float newSize = mainCamera.orthographicSize - (pinchDelta * zoomSensitivity);
            mainCamera.orthographicSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);
        }
        else
        {
            // For 3D (perspective camera) — adjust field of view
            float newFOV = mainCamera.fieldOfView - (pinchDelta * zoomSensitivity);
            mainCamera.fieldOfView = Mathf.Clamp(newFOV, 1f, 179f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            isDragging = true;
            dragDelta = Vector2.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        if (isDragging && Input.touchCount <= 1) // disable drag if two fingers
        {
            dragDelta = eventData.delta;

            float xMove = invertX ? -dragDelta.x : dragDelta.x;
            float yMove = invertY ? -dragDelta.y : dragDelta.y;

            Vector3 move = new Vector3(xMove, yMove, 0f) * dragSensitivity;
            mainCamera.transform.Translate(move);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        isDragging = false;
        dragDelta = Vector2.zero;
    }

    private void MoveCameraByDrag(Vector2 delta)
    {
        float xMove = invertX ? -delta.x : delta.x;
        float yMove = invertY ? -delta.y : delta.y;
        Vector3 move = new Vector3(xMove, yMove, 0f) * dragSensitivity;
        mainCamera.transform.Translate(move);
    }
}