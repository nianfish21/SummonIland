using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class DragTransfer : MonoBehaviour
{
    //public Camera ca;
    private Ray ra;
    private RaycastHit hit;
    private bool is_element = false;
    private int flag = 0;
    private GameObject Element;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ra, out hit) && hit.collider.tag == "element")
            {
                is_element = true;
                Element = hit.collider.gameObject;
                if (flag == 0)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
        }

        if (flag == 1 && is_element)
        {
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(Element.transform.position);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                targetScreenPos.z);
            Element.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}