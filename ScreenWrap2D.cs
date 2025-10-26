using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScreenWrap2D : MonoBehaviour
{
    private Camera mainCam;
    private Renderer rend;

    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        mainCam = Camera.main;
        rend = GetComponent<Renderer>();

        // Get the visible area in world units
        halfHeight = mainCam.orthographicSize;
        halfWidth = halfHeight * mainCam.aspect;
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // Wrap horizontally
        if (pos.x > halfWidth)
            pos.x = -halfWidth;
        else if (pos.x < -halfWidth)
            pos.x = halfWidth;

        // Wrap vertically
        if (pos.y > halfHeight)
            pos.y = -halfHeight;
        else if (pos.y < -halfHeight)
            pos.y = halfHeight;

        transform.position = pos;
    }

#if UNITY_EDITOR
    // Optional gizmo to show screen bounds
    private void OnDrawGizmosSelected()
    {
        if (!Camera.main) return;
        float h = Camera.main.orthographicSize;
        float w = h * Camera.main.aspect;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(w * 2f, h * 2f, 0f));
    }
#endif
}
