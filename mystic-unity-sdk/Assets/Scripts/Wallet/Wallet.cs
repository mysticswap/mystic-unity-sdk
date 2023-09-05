using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Wallet
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField]
        private StringVariable walletAddress;
        [SerializeField]
        private StringVariable authenticationToken;
        [SerializeField]
        private string chainId = "5";
        private const string uri = "https://mystic-swap.herokuapp.com/marketplace-api/";

        public void GetBalance()
        {
            CallRequest(
                EndpointRequest(
                    "get-balance",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId));
        }

        public void GetNfts()
        {
            CallRequest(
                EndpointRequest(
                    "get-nfts",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId));
        }

        private void CallRequest(string endpointRequest)
        {
            StartCoroutine(GetRequest(endpointRequest, authenticationToken.Value));
        }

        private static string EndpointRequest(string endpoint, params string[] parameter)
        {
            string parameters = null;
            var index = 0;
            foreach (var param in parameter)
            {
                parameters = index > 0 ? parameters += "&" + param : parameters += param;
                index++;
            }
            return string.Format($"{uri}{endpoint}?{parameters}");
        }

        private IEnumerator GetRequest(string uri, string authenticationToken)
        {
            Debug.Log($"Get Request: {uri}");
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                AttachHeader(webRequest, "Authorization", authenticationToken);
                yield return webRequest.SendWebRequest();

                Debug.Log($"Response code: {webRequest.responseCode}");
                Debug.Log($"Result status: {webRequest.result}");
                Debug.Log($"Response text: {webRequest.downloadHandler.text}");
            }
        }

        private void AttachHeader(UnityWebRequest webRequest, string key, string value)
        {
            webRequest.SetRequestHeader(key, value);
        }

    }
}
