using UnityEngine;

public class DragWin : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D hit = Physics2D.OverlapPoint(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit != null && hit.transform == transform)
            {
                dragging = true;
                offset = transform.position - mousePos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            transform.position = mousePos + offset;
        }
    }
}