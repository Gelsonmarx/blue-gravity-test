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
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image m_slotimage;
        [SerializeField] private TextMeshProUGUI m_slotItemNameText;
        [SerializeField] Button m_equipButton;

        EquipmentObject m_equipmentObject = null;
        public void InitSlot(EquipmentObject _equipmentObject)
        {
            m_equipmentObject = _equipmentObject;
            m_slotimage.sprite = _equipmentObject.EquipmentSprite[0];
            m_slotItemNameText.text = _equipmentObject.EquipmentName;
            m_equipButton.onClick.AddListener(() =>
            {
                GameManager.Instance.PlayerInventory.Equip(_equipmentObject);
            });
            bool _isEquipped = GameManager.Instance.PlayerInventory.IsEquipped(_equipmentObject);
            m_equipButton.gameObject.SetActive(_isEquipped);

            GameManager.Instance.PlayerInventory.OnEquipmentChange += _actualSet =>
            {
                bool _isEquipped = _actualSet.Contains(_equipmentObject);
                m_equipButton.gameObject.SetActive(_isEquipped);

            };

        }

        private void OnDestroy()
        {
            GameManager.Instance.PlayerInventory.OnEquipmentChange -= _actualSet =>
            {
                bool _isEquipped = _actualSet.Contains(m_equipmentObject);
                m_equipButton.gameObject.SetActive(_isEquipped);

            };
        }

    }

}
