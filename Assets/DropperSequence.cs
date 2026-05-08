using UnityEngine;
using System.Collections;

public class DropperSequence : MonoBehaviour
{
    public Transform liquidPos;
    public Transform conePos;
    public Transform coverDownPos;
    public GameObject hydrogenPeroxideCover;
    public float speed = 5.0f;
    public GameObject liquidInDropper;
    public GameObject liquidInCone;

    private bool isMoving = false;
    private bool hasLiquid = false;
    private bool isCoverRemoved = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (liquidInDropper) liquidInDropper.SetActive(false);
        if (liquidInCone) liquidInCone.SetActive(false);
    }

    // بيتنادى من MouseClickManager
    public void HandleClick()
    {
        if (!isMoving)
        {
            if (!isCoverRemoved)
            {
                StartCoroutine(RemoveCoverSequence());
            }
            else if (!hasLiquid)
            {
                StartCoroutine(FillSequence());
            }
            else
            {
                StartCoroutine(PourSequence());
            }
        }
    }

    IEnumerator RemoveCoverSequence()
    {
        isMoving = true;
        Debug.Log("بنشيل الغطا الأول...");

        Vector3 targetPos = coverDownPos.position;
        float coverSpeed = 3.0f;

        while (Vector3.Distance(hydrogenPeroxideCover.transform.position, targetPos) > 0.01f)
        {
            hydrogenPeroxideCover.transform.position = Vector3.MoveTowards(
                hydrogenPeroxideCover.transform.position, targetPos, coverSpeed * Time.deltaTime);
            yield return null;
        }

        isCoverRemoved = true;
        isMoving = false;
        Debug.Log("الغطا اتفك، القطارة جاهزة تسحب!");
    }

    IEnumerator FillSequence()
    {
        isMoving = true;
        yield return MoveTo(liquidPos.position);
        yield return new WaitForSeconds(0.5f);

        if (liquidInDropper) liquidInDropper.SetActive(true);
        hasLiquid = true;

        yield return new WaitForSeconds(0.5f);
        yield return MoveTo(initialPosition);
        isMoving = false;
    }

    IEnumerator PourSequence()
    {
        isMoving = true;
        yield return MoveTo(conePos.position);
        yield return new WaitForSeconds(0.5f);

        if (liquidInDropper) liquidInDropper.SetActive(false);
        if (liquidInCone) liquidInCone.SetActive(true);
        hasLiquid = false;

        yield return new WaitForSeconds(0.5f);
        yield return MoveTo(initialPosition);
        transform.rotation = initialRotation;
        isMoving = false;
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}