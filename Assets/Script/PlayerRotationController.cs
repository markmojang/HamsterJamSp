using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    public float distanceFromCenter = 5f;
    public float rotationSpeed = 5f;
    public GameObject arrow;

     void Start()
    {
        // ซ่อนเคอร์เซอร์เมาส์
        Cursor.visible = false;
        // ล็อคเคอร์เซอร์ให้อยู่ในหน้าจอเกม
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        float currentAngle = transform.rotation.eulerAngles.z;
        
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));

        Vector3 arrowPosition = transform.position + (Vector3)direction.normalized * distanceFromCenter;
        arrow.transform.position = arrowPosition;
    }
}
