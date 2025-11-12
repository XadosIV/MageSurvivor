using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public Transform cam;
    public GameObject fireballPrefab;

    CharacterController cc;
    float pitch = 0f;
    float Velocite = 0f;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (cam == null) cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0f, mx, 0f);
        pitch -= my;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * h + transform.forward * v).normalized * moveSpeed;

        if (cc.isGrounded)
        {
            if (Velocite < 0f) Velocite = -2f;
            if (Input.GetButtonDown("Jump")) Velocite = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Velocite += gravity * Time.deltaTime;
        Vector3 vel = new Vector3(move.x, 0f, move.z);
        cc.Move((vel + Vector3.up * Velocite) * Time.deltaTime);

            
            if (Input.GetButtonDown("Fire1"))
            {
                Instantiate(fireballPrefab, cam.position + cam.forward * 0.5f, cam.rotation);
            }
    }
}
