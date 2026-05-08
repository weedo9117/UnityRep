using UnityEngine;

public class MouseClickManager : MonoBehaviour
{
    void Update()
    {
        // كليك شمال = مسك او اكشن
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);

            foreach (RaycastHit hit in hits)
            {
                Debug.Log("ضرب: " + hit.collider.gameObject.name);

                var labtool = hit.collider.GetComponentInParent<LabTool>()
                              ?? hit.collider.GetComponent<LabTool>();
                if (labtool != null)
                {
                    labtool.HandleClick();
                    return;
                }

                var dropper = hit.collider.GetComponentInParent<DropperSequence>()
                              ?? hit.collider.GetComponent<DropperSequence>();
                if (dropper != null)
                {
                    dropper.HandleClick();
                    return;
                }

                var spoon = hit.collider.GetComponentInParent<SpoonAction>()
                            ?? hit.collider.GetComponent<SpoonAction>();
                if (spoon != null)
                {
                    spoon.HandleClick();
                    return;
                }
            }
        }

        // كليك يمين = حط الاداة
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);

            foreach (RaycastHit hit in hits)
            {
                var labtool = hit.collider.GetComponentInParent<LabTool>()
                              ?? hit.collider.GetComponent<LabTool>();
                if (labtool != null)
                {
                    labtool.HandleRightClick();
                    return;
                }
            }
        }
    }
}