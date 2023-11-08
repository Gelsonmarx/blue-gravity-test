using System;
using BlueGravity.Core;
using UnityEngine;


namespace BlueGravity.Tools
{
    public class OnTriggerEnter2DActionComponent : MonoBehaviour
    {
        public event Action<PlayerInput> OnColisionEnter;
        public event Action<PlayerInput> OnColisionExit;


        private void OnTriggerEnter2D(Collider2D other)
        {
            var _playerInput = other.gameObject.GetComponent<PlayerInput>();
            if (_playerInput != null)
            {
                OnColisionEnter?.Invoke(_playerInput);
            }


        }
        private void OnTriggerExit2D(Collider2D other)
        {
            var _playerInput = other.gameObject.GetComponent<PlayerInput>();
            if (_playerInput != null)
            {
                OnColisionExit?.Invoke(_playerInput);
            }
        }
    }
}

