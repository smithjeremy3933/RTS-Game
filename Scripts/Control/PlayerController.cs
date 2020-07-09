using RTS.Movement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS.Control
{
    public class PlayerController : MonoBehaviour
    {
        List<GameObject> selectedUnitList;
        [SerializeField] List<GameObject> playerUnits = new List<GameObject>();
        Vector3 p1;
        bool dragSelect;
        SelectionDict selectedTable;

        private void Awake()
        {
            selectedUnitList = new List<GameObject>();
            dragSelect = false;
            selectedTable = GetComponent<SelectionDict>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                p1 = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                if ((p1 - Input.mousePosition).magnitude > 40)
                {
                    dragSelect = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                selectedUnitList.Clear();

                if (!dragSelect)
                {
                    EndPointSelect();
                }
                else
                {
                    EndDragSelect();
                }
                dragSelect = false;
            }
        }

        private void EndPointSelect()
        {
            Ray ray = Camera.main.ScreenPointToRay(p1);
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Unit");
            bool hasHit = Physics.Raycast(ray, out hit, 1000f, mask);
            if (hasHit)
            {
                if (Input.GetKey(KeyCode.LeftShift)) // Inclusive Select
                {
                    selectedTable.AddSelected(hit.transform.gameObject);
                }
                else // Exclusive Select
                {
                    selectedTable.DeselectAll();
                    selectedTable.AddSelected(hit.transform.gameObject);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Do Nothing
                }
                else
                {
                    selectedTable.DeselectAll();
                }
            }
        }

        private void EndDragSelect()
        {
            Rect rect = Utils.GetScreenRect(p1, Input.mousePosition);
            var yMin = 1080 - rect.yMax;
            var yMax = 1080 - rect.yMin;
            selectedTable.DeselectAll();

            foreach (GameObject unit in playerUnits)
            {
                //GameObject go = unit.transform.parent.gameObject;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (screenPos.x > rect.xMin && screenPos.x < rect.xMax && (screenPos.y) > (yMin - 20) && (screenPos.y) < yMax)
                {
                    selectedUnitList.Add(unit);
                    selectedTable.AddSelected(unit);
                }
            }
            //if (!Input.GetKey(KeyCode.LeftShift))
            //{
            //    selectedTable.DeselectAll();
            //}

            Debug.Log(selectedUnitList.Count);
        }

        private void OnGUI()
        {
            if (dragSelect)
            {
                Rect rect = Utils.GetScreenRect(p1, Input.mousePosition);
                Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
                Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
            }
        }
    }
}