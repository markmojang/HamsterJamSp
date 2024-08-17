using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool skip_check = false;
    public void OnDrop(PointerEventData eventData){
        if (transform.childCount > 0 && !skip_check)
        {
            Debug.Log("Slot is already occupied!");
            return; // Prevent the drop if the slot is occupied
        }
        GameObject dropped = eventData.pointerDrag;
        Dragable_Object draggableitem = dropped.GetComponent<Dragable_Object>();
        draggableitem.parentAfterDrag = transform;
    }
}
