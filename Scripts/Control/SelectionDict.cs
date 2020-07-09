using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Control
{
    public class SelectionDict : MonoBehaviour
    {
        public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

        public void AddSelected(GameObject go)
        {
            int id = go.GetInstanceID();

            if (!(selectedTable.ContainsKey(id)))
            {
                selectedTable.Add(id, go);
                go.AddComponent<SelectionComponent>();
                Debug.Log("Added " + id + " to selected dict");
            }
        }

        public void Deselect(int id)
        {
            Destroy(selectedTable[id].GetComponent<SelectionComponent>());
            selectedTable.Remove(id);
        }

        public void DeselectAll()
        {
            foreach (KeyValuePair<int, GameObject> pair in selectedTable)
            {
                if (pair.Value != null)
                {
                    Destroy(selectedTable[pair.Key].GetComponent<SelectionComponent>());
                }
            }
            selectedTable.Clear();
        }

        public bool Contains(int id)
        {
            return selectedTable.ContainsKey(id);
        }
    }
}