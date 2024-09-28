using UnityEngine;

public class CamControl : MonoBehaviour
{
    //UseKeyBoardControlCam
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float fastMove;
    [SerializeField]
    float smooth;
    [SerializeField]
    float rotateSpeed;
    Vector3 camRootPos;
    Quaternion camRootRot;
    [SerializeField]
    AnimationCurve moveCurve;

    [SerializeField]
    float zoom;   
    [SerializeField]
    Transform cam;
    Vector3 camPos;

    //UseMouseControlCam
    Vector3 mouseDragStartPos;
    Vector3 mouseDragCurrentPos;
    Vector3 mouseDragStartRot;
    Vector3 mouseDragCurrentRot;

    //select target
    public Transform target;
    public static CamControl instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        camRootPos = transform.position;
        camRootRot = transform.rotation;
        camPos = cam.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
        else
        {
            InputHandle();
            CamLimit();
            HandleMouse();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) target = null;        
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                mouseDragStartPos = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                mouseDragCurrentPos = ray.GetPoint(entry);
                camRootPos = transform.position + mouseDragStartPos - mouseDragCurrentPos;
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            mouseDragStartRot = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            mouseDragCurrentRot = Input.mousePosition;

            Vector3 distance = mouseDragCurrentRot - mouseDragStartRot;
            mouseDragStartRot = mouseDragCurrentRot;
            camRootRot *= Quaternion.Euler(Vector3.up * (distance.x / 5f));
        }
    }

    void InputHandle()
    {
        camRootPos.x += Input.GetKey(KeyCode.LeftShift) ? Input.GetAxis("Horizontal") * fastMove : Input.GetAxis("Horizontal") * moveSpeed;
        camRootPos.z += Input.GetKeyDown(KeyCode.LeftShift) ? Input.GetAxis("Vertical") * fastMove : Input.GetAxis("Vertical") * moveSpeed;
        camRootRot *= Input.GetKey(KeyCode.Q) ? Quaternion.Euler(Vector3.up * rotateSpeed) : Quaternion.Euler(Vector3.up * 0);
        camRootRot *= Input.GetKey(KeyCode.E) ? Quaternion.Euler(Vector3.up * -rotateSpeed) : Quaternion.Euler(Vector3.up * 0);
        camPos.y += Input.GetAxis("Mouse ScrollWheel") * zoom * -1.0f * moveSpeed;
        camPos.z -= Input.GetAxis("Mouse ScrollWheel") * zoom * -1.0f * moveSpeed;
        transform.position = Vector3.Lerp(transform.position, camRootPos, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Lerp(transform.rotation, camRootRot, Time.deltaTime * smooth);
        cam.localPosition = Vector3.Lerp(cam.localPosition, camPos, Time.deltaTime * smooth);
    }

    void CamLimit()
    {
        camRootPos.x = camRootPos.x < 33.0f ? 33.0f : camRootPos.x;
        camRootPos.z = camRootPos.z < 10.0f ? 10.0f : camRootPos.z;

        camPos.y = camPos.y < 10.0f ? 10.0f : camPos.y;
        camPos.z = camPos.z > -10.0f ? -10.0f : camPos.z;
        camPos.y = camPos.y > 100.0f ? 100.0f : camPos.y;
        camPos.z = camPos.z < -100.0f ? -100.0f : camPos.z;
    }
}
