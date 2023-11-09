using System;
using System.Collections.Generic;
using System.Linq;
using BlueGravity.Core;
using BlueGravity.Inventory;
using BlueGravity.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueGravity.UI
{
    public class HUDController : Singleton<HUDController>
    {
        [Header("HUD")]
        [SerializeField] TextMeshProUGUI m_goldValueTxt;
        [SerializeField] GameObject m_hudObject;

        [Header("Inventory")]
        [SerializeField] GameObject m_inventoryObject;

        [SerializeField] Image[] m_actualSetImages;
        [SerializeField] Button[] m_inventorySetButtons;
        [SerializeField] Color m_buttonSelectedColor;
        [SerializeField] GameObject[] m_slotItemPrefabs;
        [SerializeField] Transform m_slotContentTransformParent;

        List<ItemSlot> m_actualSlots = new List<ItemSlot>();

        private void Awake()
        {
            GameManager.Instance.OnWalletChange += UpdateGold;
        }

        private void Start()
        {
            GameManager.Instance.PlayerInventory.OnEquipmentChange += UpdateSetImagesAndSlots;
            var playerInput = GameManager.Instance.PlayerInventory.GetComponent<PlayerInput>();
            playerInput.inputControl.Ground.Inventory.performed += action => OpenCloseInventory();
            SetUpInventoryButtons();
        }


        void SetUpInventoryButtons()
        {
            foreach (var _button in m_inventorySetButtons)
            {
                _button.onClick.AddListener(() =>
                 {

                     //  m_inventorySetButtons[i].GetComponent<Image>().color = m_buttonSelectedColor;
                     if (m_slotContentTransformParent.childCount > 0)
                     {
                         //Cleaning
                         for (int j = 0; j < m_slotContentTransformParent.childCount; j++)
                         {
                             var _slotObj = m_slotContentTransformParent.GetChild(j).gameObject.GetComponent<ItemSlot>();
                             m_actualSlots.Remove(_slotObj);
                             Destroy(_slotObj.gameObject);
                         }
                     }
                     var index = _button.transform.GetSiblingIndex();
                     var _itemTypeList = GameManager.Instance.PlayerInventory.ListOfEquipmentObjectsOfType((EquipmentType)index);
                     Debug.Log("List Items: " + _itemTypeList.Count + " i: " + index);
                     //Instancing Slots
                     if (_itemTypeList.Count > 0)
                     {
                         foreach (var _itemType in _itemTypeList)
                         {
                             var _slot = Instantiate(m_slotItemPrefabs[index], m_slotContentTransformParent);
                             _slot.GetComponent<ItemSlot>().InitSlot(_itemType);
                             m_actualSlots.Add(_slot.GetComponent<ItemSlot>());
                         }
                     }


                 });
            }
        }

        private void OpenCloseInventory()
        {
            var _inventoryActive = m_inventoryObject.activeInHierarchy;
            if (_inventoryActive)
            {
                m_hudObject.SetActive(true);
                m_inventoryObject.SetActive(false);
            }
            else
            {
                m_hudObject.SetActive(false);
                m_inventoryObject.SetActive(true);
            }
        }

        private void UpdateGold(int _amount)
        {
            m_goldValueTxt.text = "X " + _amount;
        }

        private void UpdateSetImagesAndSlots(EquipmentObject[] _actualSet)
        {
            foreach (var _equipmentObject in _actualSet)
            {
                int index = (int)_equipmentObject.Type;
                m_actualSetImages[index].sprite = _equipmentObject.EquipmentSprite[0];
            }

            if (m_actualSlots.Count <= 0) return;
            foreach (var _slot in m_actualSlots)
            {
                _slot.UpdateEquipButton(_actualSet);
            }
        }

    }
}


