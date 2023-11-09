using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlueGravity.Inventory
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Create New Equipment")]
    public class EquipmentObject : ScriptableObject
    {
        public string EquipmentName;
        public Sprite EquipmentSprite;
        public EquipmentType Type;
    }

}
