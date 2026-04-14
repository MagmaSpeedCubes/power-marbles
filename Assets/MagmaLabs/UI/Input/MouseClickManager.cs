using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
namespace MagmaLabs.UI{
    public class MouseClickManager : MonoBehaviour
    {
        public static MouseClickManager instance;
        public UnityEvent<Vector2, Vector3> OnLeftClick;


        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                Debug.LogWarning("Multiple instances of MouseClickManager detected. Destroying duplicate.");
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }


        void Update()
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Mouse Left Click");
                Vector2 screenPos = Mouse.current.position.ReadValue();
                Vector3 worldPos = ScreenToWorld(screenPos);
                OnLeftClick.Invoke(screenPos, worldPos);
            }
        }

        Vector3 ScreenToWorld(Vector2 screenPos)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                cam = FindObjectOfType<Camera>();
                if (cam == null) return Vector3.zero;
            }
            float z = -cam.transform.position.z; // distance to z=0 plane (works for 2D)
            Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
            world.z = 0f; // keep on 2D plane
            return world;
        }

    }

}
