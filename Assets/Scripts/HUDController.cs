using System;
using System.Collections.Generic;
using System.Linq;
using BlueGravity.Core;
using BlueGravity.Inventory;
using BlueGravity.Sound;
using BlueGravity.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
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
        [SerializeField] Button m_inventoryCloseButton;
        [SerializeField] Color m_buttonSelectedColor;
        [SerializeField] GameObject[] m_slotItemPrefabs;
        [SerializeField] Transform m_slotContentInventoryTransformParent;
        List<ItemSlot> m_actualSlots = new List<ItemSlot>();

        [Header("Shop")]

        [SerializeField] GameObject[] m_slotShopPrefabs;
        [SerializeField] Transform m_slotContentShopTransformParent;
        [SerializeField] Button m_yesShopButton, m_noShopButton, m_quitShopButton;
        [SerializeField] GameObject m_welcomeScreen, m_buyScreen, m_shopScreen;
        List<ItemShopSlot> m_actualShopSlots = new List<ItemShopSlot>();

        private void Awake()
        {
            GameManager.Instance.OnWalletChange += UpdateGold;
        }

        private void Start()
        {
            GameManager.Instance.PlayerInventory.OnEquipmentChange += UpdateSetImagesAndSlots;
            GameManager.Instance.PlayerInventory.OnInventoryChange += UpdateShopSlots;
            var playerInput = GameManager.Instance.PlayerInventory.GetComponent<PlayerInput>();
            playerInput.inputControl.Ground.Inventory.performed += action => OpenCloseInventory();
            SetUpInventoryButtons();
            SetUpShopButtons();
        }


        void SetUpInventoryButtons()
        {
            m_inventoryCloseButton.onClick.AddListener(() =>
            {
                CloseInventory();
            });
            foreach (var _button in m_inventorySetButtons)
            {
                _button.onClick.AddListener(() =>
                 {
                     SoundManager.Instance.PlaySFXSound("Click", transform.position);
                     foreach (var _buttons in m_inventorySetButtons) _buttons.GetComponent<Image>().color = Color.white;
                     _button.GetComponent<Image>().color = m_buttonSelectedColor;
                     if (m_slotContentInventoryTransformParent.childCount > 0)
                     {
                         //Cleaning
                         for (int j = 0; j < m_slotContentInventoryTransformParent.childCount; j++)
                         {
                             var _slotObj = m_slotContentInventoryTransformParent.GetChild(j).gameObject.GetComponent<ItemSlot>();
                             m_actualSlots.Remove(_slotObj);
                             Destroy(_slotObj.gameObject);
                         }
                     }
                     var index = _button.transform.GetSiblingIndex();
                     var _itemTypeList = GameManager.Instance.PlayerInventory.ListOfEquipmentObjectsOfType((EquipmentType)index);
                     //Instancing Slots
                     if (_itemTypeList.Count > 0)
                     {
                         foreach (var _itemType in _itemTypeList)
                         {
                             var _slot = Instantiate(m_slotItemPrefabs[index], m_slotContentInventoryTransformParent);
                             _slot.GetComponent<ItemSlot>().InitSlot(_itemType);
                             m_actualSlots.Add(_slot.GetComponent<ItemSlot>());
                         }
                     }
                     float deltaX = m_slotContentInventoryTransformParent.GetComponent<RectTransform>().sizeDelta.x;
                     float deltaY = (m_actualSlots.Count) * m_slotItemPrefabs[0].GetComponent<RectTransform>().sizeDelta.y;
                     m_slotContentInventoryTransformParent.GetComponent<RectTransform>().sizeDelta = new Vector2(deltaX, deltaY);
                 });
            }
        }


        private void SetUpShopButtons()
        {
            m_yesShopButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySFXSound("Click", transform.position);
                m_welcomeScreen.SetActive(false);
                m_buyScreen.SetActive(true);
            });
            m_noShopButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySFXSound("Click", transform.position);
                m_welcomeScreen.SetActive(true);
                m_buyScreen.SetActive(false);
                m_shopScreen.SetActive(false);
                GameManager.Instance.SetPlayerCanMove(true);
            });
            m_quitShopButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySFXSound("Click", transform.position);
                m_welcomeScreen.SetActive(true);
                m_buyScreen.SetActive(false);
            });
        }

        public void OpenShop()
        {
            if (m_shopScreen.activeInHierarchy || m_inventoryObject.activeInHierarchy) return;
            GameManager.Instance.SetPlayerCanMove(false);
            m_shopScreen.SetActive(true);
            m_welcomeScreen.SetActive(true);
            m_buyScreen.SetActive(false);
            SoundManager.Instance.PlaySFXSound("Click", transform.position);
            if (m_slotContentShopTransformParent.childCount > 0)
                for (int i = 0; i < m_slotContentShopTransformParent.childCount; i++)
                {
                    var _shopSlot = m_slotContentShopTransformParent.GetChild(i).GetComponent<ItemShopSlot>();
                    m_actualShopSlots.Remove(_shopSlot);
                    Destroy(_shopSlot.gameObject);
                }

            foreach (var item in GameManager.Instance.NPCShop.equipmentObjectsShopList)
            {
                var index = (int)item.Type;
                var _slot = Instantiate(m_slotShopPrefabs[index], m_slotContentShopTransformParent);
                _slot.GetComponent<ItemShopSlot>().InitShopSlot(item);
                m_actualShopSlots.Add(_slot.GetComponent<ItemShopSlot>());
            }
            float deltaX = m_slotContentShopTransformParent.GetComponent<RectTransform>().sizeDelta.x;
            float deltaY = (m_actualShopSlots.Count + 6) * m_slotShopPrefabs[0].GetComponent<RectTransform>().sizeDelta.y;
            m_slotContentShopTransformParent.GetComponent<RectTransform>().sizeDelta = new Vector2(deltaX, deltaY);
        }

        private void OpenCloseInventory()
        {

            var _inventoryActive = m_inventoryObject.activeInHierarchy;
            if (_inventoryActive)
            {
                CloseInventory();
            }
            else
            {
                if (m_shopScreen.activeInHierarchy || m_inventoryObject.activeInHierarchy) return;
                OpenInventory();
            }
        }

        private void OpenInventory()
        {
            SoundManager.Instance.PlaySFXSound("Click", transform.position);
            m_hudObject.SetActive(false);
            m_inventoryObject.SetActive(true);
            GameManager.Instance.SetPlayerCanMove(false);
        }

        private void CloseInventory()
        {
            SoundManager.Instance.PlaySFXSound("Click", transform.position);
            m_hudObject.SetActive(true);
            m_inventoryObject.SetActive(false);
            GameManager.Instance.SetPlayerCanMove(true);
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

        private void UpdateShopSlots(List<EquipmentObject> _inventoryItems)
        {
            if (m_actualShopSlots.Count <= 0) return;
            foreach (var _slot in m_actualShopSlots)
            {
                _slot.UpdateBuyButton(_inventoryItems);
            }
        }

    }
}


