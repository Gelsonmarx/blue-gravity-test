using BlueGravity.Core;
using BlueGravity.Tools;
using TMPro;
using UnityEngine;

namespace BlueGravity.UI
{
    public class HUDController : Singleton<HUDController>
    {
        [Header("HUD")]
        [SerializeField] TextMeshProUGUI m_goldValueTxt;


        private void Awake()
        {
            GameManager.Instance.OnWalletChange += UpdateGold;
        }

        private void UpdateGold(int _amount)
        {
            m_goldValueTxt.text = "X " + _amount;
        }

    }
}


