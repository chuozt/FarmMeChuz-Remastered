using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    [SerializeField] private Item craftingItem;
    public Item CraftingItem => craftingItem;

    void Awake() => transform.GetChild(0).GetComponent<Image>().sprite = craftingItem.ItemSprite;
}
