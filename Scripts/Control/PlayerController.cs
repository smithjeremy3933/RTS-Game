using RTS.Combat;
using RTS.Core;
using RTS.Movement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] public List<GameObject> PlayerUnits = new List<GameObject>();
        public GameObject hoveredObject;
        public List<GameObject> SelectedUnitList = new List<GameObject>();

        Vector3 p1;
        bool dragSelect;
        SelectionDict selectedTable;
        HealthDisplay healthDisplay;
        ExperienceDisplay experienceDisplay;
        float maxRayDist = 1000f;
        int screenHeight = 1080;
        int dragControl = 40;

        private void Awake()
        {
            dragSelect = false;
            selectedTable = GetComponent<SelectionDict>();
            healthDisplay = FindObjectOfType<HealthDisplay>();
            experienceDisplay = FindObjectOfType<ExperienceDisplay>();
        }

        private void Update()
        {
            CheckCursor();
            if (InteractWithCombat()) return;
            InteractWithMovement();
            SelectionDrag();
        }

        private void CheckCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            LayerMask mask = LayerMask.GetMask("Unit");
            bool hasHit = Physics.Raycast(ray, out hitInfo, maxRayDist, mask);

            if (hasHit)
            {
                GameObject hitObject = hitInfo.transform.root.gameObject;

                HoveredObject(hitObject);
            }
            else
            {
                ClearSelection();
            }
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButton(1))
                {
                    foreach (GameObject unit in SelectedUnitList)
                    {
                        IUnit currentUnit = unit.GetComponent<IUnit>();
                        if (currentUnit.IsSelected())
                        {
                            unit.GetComponent<Fighter>().Attack(target.gameObject);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private void InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            foreach (GameObject unit in SelectedUnitList)
            {
                Mover selectedUnit = unit.GetComponent<Mover>();
                if (selectedUnit != null)
                {
                    if (selectedUnit.IsSelected())
                    {
                        if (hasHit)
                        {
                            if (Input.GetMouseButtonDown(1))
                            {
                                selectedUnit.StartMoveAction(hit.point);
                            }
                        }
                    }
                }             
                else
                {
                    Debug.Log("Not selected");
                }
            }
        }

        void HoveredObject(GameObject obj)
        {
            if (hoveredObject != null)
            {
                if (obj == hoveredObject)
                    return;

                ClearSelection();
            }

            hoveredObject = obj;
        }

        void ClearSelection()
        {
            if (hoveredObject == null)
                return;

            hoveredObject = null;
        }

        private void SelectionDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                p1 = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                if ((p1 - Input.mousePosition).magnitude > dragControl)
                {
                    dragSelect = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                SelectedUnitList.Clear();

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
            if (hoveredObject != null && PlayerUnits.Contains(hoveredObject))
            {
                if (Input.GetKey(KeyCode.LeftShift)) // Inclusive Select
                {
                    selectedTable.AddSelected(hoveredObject);
                }
                else // Exclusive Select
                {
                    ExclusiveSelect();
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
                    SelectedUnitList.Clear();
                    ClearDisplay();
                }
            }
        }

        private void EndDragSelect()
        {
            Rect rect = Utils.GetScreenRect(p1, Input.mousePosition);
            var yMin = screenHeight - rect.yMax;
            var yMax = screenHeight - rect.yMin;
            ClearDisplay();
            selectedTable.DeselectAll();

            foreach (GameObject unit in PlayerUnits)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (screenPos.x > rect.xMin && screenPos.x < rect.xMax && (screenPos.y) > (yMin - 20) && (screenPos.y) < yMax)
                {
                    SelectedUnitList.Add(unit);
                    selectedTable.AddSelected(unit);
                }
            }
            Debug.Log(SelectedUnitList.Count);
            SetDisplay();
        }

        private void ExclusiveSelect()
        {
            selectedTable.DeselectAll();
            SelectedUnitList.Clear();
            ClearDisplay();
            selectedTable.AddSelected(hoveredObject);
            SelectedUnitList.Add(hoveredObject);
            SetDisplay();
        }

        private void SetDisplay()
        {
            healthDisplay.SetInitHealthText();
            experienceDisplay.SetInitExpText();
        }

        private void ClearDisplay()
        {
            healthDisplay.ClearHealthText();
            experienceDisplay.ClearExpText();
        }

        public bool IsSelectListEmpty()
        {
            if (SelectedUnitList.Count > 0)
            {
                return false;
            }
            return true;
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