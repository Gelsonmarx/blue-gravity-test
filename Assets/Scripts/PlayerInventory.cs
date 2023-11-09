using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueGravity.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private EquipmentObject[] m_initialSet = new EquipmentObject[10];
        [SerializeField] private EquipmentObject[] otherItems = new EquipmentObject[10];
        [Header("Renderer Positions")]
        [SerializeField] private SpriteRenderer hoodRenderer;
        [SerializeField]
        private SpriteRenderer maskRenderer, shoulderRendererRight, shoulderRendererLeft, elbowRendererRight,
        elbowRendererLeft, wristRendererRight, wristRendererLeft, torsoRenderer, pelvisRenderer, legRendererRight, legRendererLeft,
        bootsRendererRight, bootsRendererLeft, weaponRendererRight, weaponRendererLeft;
        public EquipmentObject[] ActualSet { get; private set; } = new EquipmentObject[10];
        public List<EquipmentObject> InventoryItems { get; private set; } = new List<EquipmentObject>();
        public event Action<EquipmentObject[]> OnEquipmentChange;
        public event Action<List<EquipmentObject>> OnInventoryChange;

        private void Awake()
        {
            OnEquipmentChange += EquipOnSet;

            foreach (var _item in m_initialSet)
            {
                AddItemToInventory(_item);
                Equip(_item);
            }
            foreach (var _item in otherItems)
            {
                AddItemToInventory(_item);
            }

        }

        public bool IsEquipped(EquipmentObject _equipmentObject) => ActualSet.Contains(_equipmentObject);

        public void Equip(EquipmentObject _equipmentObject)
        {
            int _index = (int)_equipmentObject.Type;
            ActualSet[_index] = _equipmentObject;
            if (OnEquipmentChange != null) OnEquipmentChange.Invoke(ActualSet);
        }

        public void AddItemToInventory(EquipmentObject _equipmentObject)
        {
            InventoryItems.Add(_equipmentObject);
            if (OnInventoryChange != null) OnInventoryChange.Invoke(InventoryItems);
        }

        public void RemoveFromInventory(EquipmentObject _equipmentObject)
        {
            if (InventoryItems.Contains(_equipmentObject)) InventoryItems.Remove(_equipmentObject);
            if (OnInventoryChange != null) OnInventoryChange.Invoke(InventoryItems);
        }

        public List<EquipmentObject> ListOfEquipmentObjectsOfType(EquipmentType _equipmentType)
        {
            List<EquipmentObject> _equipmentObjects = new List<EquipmentObject>();
            foreach (var _item in InventoryItems) if (_item.Type == _equipmentType) _equipmentObjects.Add(_item);
            return _equipmentObjects;
        }

        void EquipOnSet(EquipmentObject[] _actualSet)
        {
            foreach (var _equipmentObject in _actualSet)
            {
                if (_equipmentObject != null)
                    switch (_equipmentObject.Type)
                    {
                        case EquipmentType.Hood:
                            hoodRenderer.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Mask:
                            maskRenderer.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Shoulder:
                            shoulderRendererRight.sprite = _equipmentObject.EquipmentSprite[1];
                            shoulderRendererLeft.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Elbow:
                            elbowRendererRight.sprite = _equipmentObject.EquipmentSprite[1];
                            elbowRendererLeft.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Wrist:
                            wristRendererRight.sprite = _equipmentObject.EquipmentSprite[1];
                            wristRendererLeft.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Torso:
                            torsoRenderer.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Pelvis:
                            pelvisRenderer.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Leg:
                            legRendererRight.sprite = _equipmentObject.EquipmentSprite[1];
                            legRendererLeft.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Boots:
                            bootsRendererRight.sprite = _equipmentObject.EquipmentSprite[1];
                            bootsRendererLeft.sprite = _equipmentObject.EquipmentSprite[0];
                            break;
                        case EquipmentType.Weapon:
                            weaponRendererRight.sprite = _equipmentObject.EquipmentSprite[0];
                            weaponRendererLeft.sprite = _equipmentObject.EquipmentSprite[1];
                            break;
                    }
            }

        }

    }

    public enum EquipmentType { Hood, Mask, Shoulder, Elbow, Wrist, Torso, Pelvis, Leg, Boots, Weapon }
}



