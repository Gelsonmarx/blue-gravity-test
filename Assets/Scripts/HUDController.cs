using System;
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

        private void Awake()
        {
            GameManager.Instance.OnWalletChange += UpdateGold;
        }

        private void Start()
        {
            GameManager.Instance.PlayerInventory.OnEquipmentChange += UpdateSetImages;
            var playerInput = GameManager.Instance.PlayerInventory.GetComponent<PlayerInput>();
            playerInput.inputControl.Ground.Inventory.performed += action => OpenCloseInventory();
            SetUpInventoryButtons();
        }


        void SetUpInventoryButtons()
        {
            for (int i = 0; i < m_inventorySetButtons.Length; i++)
            {
                m_inventorySetButtons[i].onClick.AddListener(() =>
                {
                    m_inventorySetButtons[i].image.color = m_buttonSelectedColor;
                    if (m_slotContentTransformParent.childCount > 0)
                    {
                        //Cleaning
                        for (int i = 0; i < m_slotContentTransformParent.childCount; i++)
                        {
                            Destroy(m_slotContentTransformParent.GetChild(i).gameObject);
                        }
                        //Getting Item list Type
                        var _itemTypeList = GameManager.Instance.PlayerInventory.InventoryItems.Where(_item => _item.Type == (EquipmentType)i).ToList();
                        //Instancing Slots
                        if (_itemTypeList.Count > 0)
                        {
                            foreach (var _itemType in _itemTypeList)
                            {
                                var _slot = Instantiate(m_slotItemPrefabs[i], m_slotContentTransformParent);
                                _slot.GetComponent<ItemSlot>().InitSlot(_itemType);
                            }
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

        private void UpdateSetImages(EquipmentObject[] _actualSet)
        {
            foreach (var _equipmentObject in _actualSet)
            {
                int index = (int)_equipmentObject.Type;
                m_actualSetImages[index].sprite = _equipmentObject.EquipmentSprite[0];
            }
        }

    }
}


