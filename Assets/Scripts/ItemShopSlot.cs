using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlueGravity.Inventory;
using BlueGravity.Core;
using System.Linq;

namespace BlueGravity.UI
{
    public class ItemShopSlot : MonoBehaviour
    {
        [SerializeField] private Image m_slotimage;
        [SerializeField] private TextMeshProUGUI m_slotItemNameText, m_slotItemPriceText, m_alreadyBoughtText;
        [SerializeField] Button m_buyButton;

        EquipmentObject m_equipmentObject = null;
        public void InitShopSlot(EquipmentObject _equipmentObject)
        {
            m_equipmentObject = _equipmentObject;
            m_slotimage.sprite = _equipmentObject.EquipmentSprite[0];
            m_slotItemNameText.text = _equipmentObject.EquipmentName;
            m_slotItemPriceText.text = "X "+ _equipmentObject.BuyPrice;

            m_buyButton.onClick.AddListener(() =>
            {
                bool _hasMoneyEnough = GameManager.Instance.IsEnoughToBuy(_equipmentObject.BuyPrice);
                if(_hasMoneyEnough) GameManager.Instance.PlayerInventory.AddItemToInventory(_equipmentObject);

            });
            UpdateBuyButton(GameManager.Instance.PlayerInventory.InventoryItems);
        }

        public void UpdateBuyButton(List<EquipmentObject> _inventoryItems)
        {
            bool _IsBought = _inventoryItems.Contains(m_equipmentObject);
            m_buyButton.gameObject.SetActive(!_IsBought);
            m_alreadyBoughtText.gameObject.SetActive(_IsBought);
        }
    }

}

