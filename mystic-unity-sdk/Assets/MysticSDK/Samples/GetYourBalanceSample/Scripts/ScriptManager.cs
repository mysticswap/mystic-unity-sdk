using System;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Samples.GetYourBalanceSample.Scripts
{
    public class ScriptManager : MonoBehaviour
    {
        private MysticSDK _sdk;
        [SerializeField] private TextMeshProUGUI textDisplayBalance;

        private void Awake()
        {
            _sdk = MysticSDKManager.Instance.sdk;
        }

        public async void ShowBalance()
        {
            var result = await _sdk.GetBalance();
            textDisplayBalance.text = result;
        }

        public async void ShowBalanceEth()
        {
            var result = await _sdk.GetBalanceEth();
            textDisplayBalance.text = result;
        }

        public async void ShowBalanceWeth()
        {
            var result = await _sdk.GetBalanceWeth();
            textDisplayBalance.text = result;
        }
    }
}
