using UnityEngine;
using System.Collections;

public class SpoonAction : MonoBehaviour
{
    public Transform potassiumPos;
    public Transform coneTopPos;
    public ParticleSystem smoke;
    public GameObject powderInSpoon;
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 150.0f;

    private bool isMoving = false;
    private bool hasPowder = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (powderInSpoon) powderInSpoon.SetActive(false);
    }

    // بيتنادى من MouseClickManager
    public void HandleClick()
    {
        if (!isMoving)
        {
            if (!hasPowder)
            {
                StartCoroutine(ScoopSequence());
            }
            else
            {
                StartCoroutine(PourSequence());
            }
        }
    }

    IEnumerator ScoopSequence()
    {
        isMoving = true;

        Quaternion scoopRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 30f);
        yield return MoveAndRotate(potassiumPos.position, scoopRot);
        yield return new WaitForSeconds(0.3f);

        if (powderInSpoon) powderInSpoon.SetActive(true);
        hasPowder = true;

        Vector3 liftPos = transform.position + Vector3.up * 1.5f;
        Quaternion uprightRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        yield return MoveAndRotate(liftPos, uprightRot);

        yield return MoveAndRotate(initialPosition, initialRotation);

        isMoving = false;
        Debug.Log("المعلقة غرفت ومستنية كليك التفريغ!");
    }

    IEnumerator PourSequence()
    {
        isMoving = true;

        Quaternion uprightRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        yield return MoveAndRotate(coneTopPos.position, uprightRot);
        yield return new WaitForSeconds(0.2f);

        Quaternion pourRot = Quaternion.Euler(0, transform.eulerAngles.y, 80f);
        yield return RotateTo(pourRot);

        if (powderInSpoon) powderInSpoon.SetActive(false);
        if (smoke != null) smoke.Play();
        hasPowder = false;

        yield return new WaitForSeconds(1.5f);

        yield return MoveAndRotate(initialPosition, initialRotation);
        isMoving = false;
    }

    IEnumerator MoveAndRotate(Vector3 targetPos, Quaternion targetRot)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f ||
               Quaternion.Angle(transform.rotation, targetRot) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        transform.rotation = targetRot;
    }

    IEnumerator RotateTo(Quaternion targetRot)
    {
        while (Quaternion.Angle(transform.rotation, targetRot) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRot;
    }
}