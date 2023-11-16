using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;
    public Text countText2;
    public Image bg;

    [HideInInspector]
    public Item item;
    [HideInInspector]
    public int count = 1;
    [HideInInspector]
    public Transform parentAfterDrag;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.ItemSprite;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText2.text = countText.text;
        bool textAcive = count > 1;
        countText.gameObject.SetActive(textAcive);
        countText2.gameObject.SetActive(textAcive);
        bg.gameObject.SetActive(textAcive);

        if(count <= 0)
            Destroy(this.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}