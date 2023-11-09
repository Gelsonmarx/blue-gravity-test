using System;
using BlueGravity.Inventory;
using BlueGravity.Tools;
using UnityEngine;

namespace BlueGravity.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] int initialGold = 0;
        public int Gold { get; private set; } = 0;
        public event Action<int> OnWalletChange;

        public PlayerInventory PlayerInventory {get; private set;}

        private void Awake() {
            PlayerInventory = FindObjectOfType<PlayerInventory>();
        }

        private void Start()
        {
            AddMoney(initialGold);
        }

        public void AddMoney(int _amount)
        {
            Gold += _amount;
            if (OnWalletChange != null) OnWalletChange.Invoke(Gold);
        }

        public void SubtractMoney(int _amount)
        {
            Gold -= _amount;
            if (OnWalletChange != null) OnWalletChange.Invoke(Gold);
        }
    }
}

