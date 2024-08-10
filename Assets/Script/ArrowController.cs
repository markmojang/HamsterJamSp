using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // ตัวแปร public สำหรับกำหนดระยะห่างจากจุดศูนย์กลางของหน้าจอ
    public float distanceFromCenter = 5f;
    

 void Start()
    {
        // ซ่อนเคอร์เซอร์เมาส์
        Cursor.visible = false;

        // ล็อคเคอร์เซอร์ให้อยู่ในหน้าจอเกม
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        // รับตำแหน่งของเคอร์เซอร์ในพื้นที่หน้าจอ
        Vector3 mousePosition = Input.mousePosition;

        // แปลงตำแหน่งเคอร์เซอร์จากพื้นที่หน้าจอเป็นพื้นที่โลก (world space)
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        // รับตำแหน่งศูนย์กลางของหน้าจอในพื้นที่โลก
        Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));

        // คำนวณทิศทางจากศูนย์กลางหน้าจอไปยังเคอร์เซอร์
        Vector2 direction = new Vector2(
            mousePosition.x - screenCenter.x,
            mousePosition.y - screenCenter.y
        );

        // คำนวณมุมที่ลูกศรควรหมุนไป
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // ตั้งค่ามุมหมุนของลูกศร
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // คำนวณตำแหน่งใหม่ของลูกศรโดยอยู่ห่างจากจุดศูนย์กลางของหน้าจอ
        Vector3 arrowPosition = screenCenter + (Vector3)direction.normalized * distanceFromCenter;

        // ตั้งค่าตำแหน่งของลูกศร
        transform.position = arrowPosition;
    }
}
