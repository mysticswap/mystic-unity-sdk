using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core
{

    public class MysticSDK : MonoBehaviour
    {
        [SerializeField] private StringVariable walletAddress;
        [SerializeField] private StringVariable authenticationToken;
        [SerializeField] private string chainId = "5";
        private const string uri = "https://mystic-swap.herokuapp.com/marketplace-api/";

        public string JsonString { get; private set; }
        public string JsonPost { get; private set; }
        public string JsonResponse { get; private set; }



        public void SetAddress(string address)
        {
            walletAddress.SetValue(address);
        }

        public string GetAddress()
        {
            return walletAddress.Value;
        }

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

        public void CreateSwap(object request)
        {
            var requestBody = ConvertToJson(request);
            Debug.Log($"{requestBody}");
            var _uri = "create-swap";
            StartCoroutine(
                PostRequest(
                    $"{uri}{_uri}", requestBody, authenticationToken.Value)
            );
        }

        private void CallRequest(string endpointRequest)
        {
            StartCoroutine(GetRequest(endpointRequest, authenticationToken.Value));
        }

        private static string EndpointRequest(string endpoint, params string[] parameter)
        {
            var parameters = string.Empty;
            var index = 0;
            foreach (var param in parameter)
            {
                parameters = index > 0 ? parameters += "&" + param : parameters += param;
                index++;
            }

            return string.Format($"{uri}{endpoint}?{parameters}");
        }

        private IEnumerator GetRequest(string uri, string authToken)
        {
            Debug.Log($"Get Request: {uri}");
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            AttachHeader(webRequest, "Authorization", authToken);
            yield return webRequest.SendWebRequest();

            Debug.Log($"Response code: {webRequest.responseCode}");
            Debug.Log($"Result status: {webRequest.result}");
            Debug.Log($"Response text: {webRequest.downloadHandler.text}");
            JsonString = webRequest.downloadHandler.text;
            Debugger.Instance.Log(uri, webRequest.downloadHandler.text);
        }

        private IEnumerator PostRequest(string uri, string request, string authToken)
        {
            Debug.Log($"Post Request: {uri}");
            using UnityWebRequest webRequest = UnityWebRequest.Post(uri, request, "application/json");
            AttachHeader(webRequest, "Authorization", authToken);
            yield return webRequest.SendWebRequest();
            
            Debug.Log($"Response code: {webRequest.responseCode}");
            Debug.Log($"Result status: {webRequest.result}");
            Debug.Log($"Result error: {webRequest.error}");

            JsonResponse = webRequest.downloadHandler.text;
        }

        private void AttachHeader(UnityWebRequest webRequest, string key, string value)
        {
            webRequest.SetRequestHeader(key, value);
        }

        private string ConvertToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}