using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlueGravity.Core
{
    public class PlayerInput : MonoBehaviour
    {
        public InputControl inputControl { get; private set; }
        private void Awake()
        {
            inputControl = new InputControl();
            inputControl.Enable();
        }
    }
}


