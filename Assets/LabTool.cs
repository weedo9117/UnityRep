using UnityEngine;

public class LabTool : MonoBehaviour
{
    public GameObject contentInside;
    public ParticleSystem actionEffect;
    public Transform rightClickTarget;

    private bool isHeld = false;
    private bool isRightClickHeld = false;
    private bool hasContent = false;
    private float distanceFromCamera;
    private float rightClickDistance;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (contentInside) contentInside.SetActive(false);
    }

    void Update()
    {
        // Left-click drag
        if (isHeld)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = distanceFromCamera;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * 20f);
        }

        // ✅ Right-click DOWN: نشوف لو الماوس فوق الأوبجيكت ده بالظبط
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isRightClickHeld = true;
                if (rb) rb.isKinematic = true;
                rightClickDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
                Debug.Log("بدأ السحب بالكليك الأيمن: " + gameObject.name);
            }
        }

        // ✅ Right-click HOLD: الأوبجيكت يمشي مع الماوس
        if (isRightClickHeld)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = rightClickDistance;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * 20f);
        }

        // ✅ Right-click UP: يتحط على الـ target
        if (Input.GetMouseButtonUp(1) && isRightClickHeld)
        {
            isRightClickHeld = false;
            SnapToTarget();
        }
    }

    public void HandleClick()
    {
        if (!isHeld)
            PickUp();
        else
            ExecuteAction();
    }

    // ✅ مش محتاجينها خالص بس خليناها علشان MouseClickManager ميبوظش
    public void HandleRightClick() { }

    void PickUp()
    {
        isHeld = true;
        if (rb) rb.isKinematic = true;
        distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        Debug.Log("اتمسكت: " + gameObject.name);
    }

    void SnapToTarget()
    {
        if (rightClickTarget != null)
        {
            transform.position = rightClickTarget.position;
            transform.rotation = rightClickTarget.rotation;
            Debug.Log("اتحط على الهدف: " + rightClickTarget.name);
        }
        else
        {
            Debug.LogWarning("مفيش rightClickTarget محدد!");
        }

        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    void ExecuteAction()
    {
        if (!hasContent)
        {
            hasContent = true;
            if (contentInside) contentInside.SetActive(true);
            Debug.Log("تم سحب المادة");
        }
        else
        {
            hasContent = false;
            if (contentInside) contentInside.SetActive(false);
            if (actionEffect) actionEffect.Play();
            Debug.Log("تم صب المادة");
        }
    }
}