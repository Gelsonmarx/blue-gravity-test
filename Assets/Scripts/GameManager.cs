using System;
using BlueGravity.Inventory;
using BlueGravity.Shop;
using BlueGravity.Tools;
using UnityEngine;

namespace BlueGravity.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] int initialGold = 0;
        public int Gold { get; private set; } = 0;
        public event Action<int> OnWalletChange;

        public PlayerInventory PlayerInventory { get; private set; }
        public NPCShop NPCShop { get; private set; }

        private void Awake()
        {
            PlayerInventory = FindObjectOfType<PlayerInventory>();
            NPCShop = FindObjectOfType<NPCShop>();
        }

        private void Start()
        {
            AddMoney(initialGold);
        }

        public bool IsEnoughToBuy(int _amount)
        {

            bool _enough = _amount <= Gold;
            if (_enough) SubtractMoney(_amount);
            return _enough;

        }

        public void AddMoney(int _amount)
        {
            Gold += _amount;
            if (OnWalletChange != null) OnWalletChange.Invoke(Gold);
        }

        void SubtractMoney(int _amount)
        {
            Gold -= _amount;
            if (OnWalletChange != null) OnWalletChange.Invoke(Gold);
        }

        public void SetPlayerCanMove(bool value)
        {
            PlayerInventory.GetComponent<PlayerMovement>().SetCanMove(value);
        }
    }
}

