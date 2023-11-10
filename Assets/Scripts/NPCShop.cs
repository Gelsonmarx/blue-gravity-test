using System;
using System.Collections.Generic;
using BlueGravity.Inventory;
using BlueGravity.Tools;
using BlueGravity.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = BlueGravity.Core.PlayerInput;

namespace BlueGravity.Shop
{
    public class NPCShop : MonoBehaviour
    {
        public List<EquipmentObject> equipmentObjectsShopList;
        OnTriggerEnter2DActionComponent m_onTriggerEnter2DActionComponent;
        Action<InputAction.CallbackContext> m_handler = null;

        private void Start()
        {
            m_onTriggerEnter2DActionComponent = GetComponent<OnTriggerEnter2DActionComponent>();
            m_onTriggerEnter2DActionComponent.OnColisionEnter += OnPlayerOnRange;
            m_onTriggerEnter2DActionComponent.OnColisionExit += OnOutOfRange;
        }

        private void OnPlayerOnRange(PlayerInput _playerInput)
        {

            m_handler = (InputAction.CallbackContext ctx) =>
            {
                HUDController.Instance.OpenShop();
            };
            _playerInput.inputControl.Ground.Interaction.performed += m_handler;
        }

        private void OnOutOfRange(PlayerInput _playerInput)
        {
            _playerInput.inputControl.Ground.Interaction.performed -= m_handler;
        }
    }

}
