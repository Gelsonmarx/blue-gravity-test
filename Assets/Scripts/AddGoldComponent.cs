using System;
using BlueGravity.Core;
using BlueGravity.Sound;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = BlueGravity.Core.PlayerInput;

namespace BlueGravity.Tools
{
    public class AddGoldComponent : MonoBehaviour
    {
        [SerializeField] int m_amountOfGoldToAdd = 5;
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
                SoundManager.Instance.PlaySFXSound("Bonus", transform.position);
                GameManager.Instance.AddMoney(m_amountOfGoldToAdd);
            };
            _playerInput.inputControl.Ground.Interaction.performed += m_handler;
        }

        private void OnOutOfRange(PlayerInput _playerInput)
        {
            _playerInput.inputControl.Ground.Interaction.performed -= m_handler;
        }


    }
}

