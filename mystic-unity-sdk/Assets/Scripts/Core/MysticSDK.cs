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
        private const string Uri = "https://mystic-swap.herokuapp.com/marketplace-api/";
        private const string UriVerifySwap = "https://mystic-swap.herokuapp.com/verify-accepted/";
        private const string UriVerifyCanceled = "https://mystic-swap.herokuapp.com/verify-cancelled/";

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
                    Uri,
                    "get-balance",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId));
        }

        public void GetNfts()
        {
            CallRequest(
                EndpointRequest(
                    Uri,
                    "get-nfts",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId));
        }

        public void CreateSwap(CreateSwap request)
        {
            var requestBody = ConvertToJson(request);
            Debug.Log($"{requestBody}");
            StartCoroutine(
                PostRequest(
                    EndpointRequest(Uri, "create-swap"), requestBody, authenticationToken.Value)
            );
        }

        public void ValidateSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            StartCoroutine(
                PostRequest(
                    EndpointRequest(Uri, "validate-swap"), requestBody, authenticationToken.Value)
            );
        }

        public void AcceptSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            StartCoroutine(
                PostRequest(
                    EndpointRequest(Uri, "accept-swap"), requestBody, authenticationToken.Value)
            );
        }

        public void VerifySwap(string swapId)
        {
            CallRequest(
                $"{UriVerifySwap}{swapId}");
        }

        public void CancelSwap(SwapData swapData)
        {
            var requestBody = ConvertToJson(swapData);
            StartCoroutine(
                PostRequest(
                    EndpointRequest(
                        Uri,
                        "cancel-swap"), requestBody, authenticationToken.Value)
            );
        }

        public void VerifyCancelled(string swapId)
        {
            CallRequest(
                $"{UriVerifyCanceled}{swapId}");
        }

        public void RetrieveAllSwaps(int page, int limit)
        {
            CallRequest(
                EndpointRequest(
                    Uri,
                    "all-swaps",
                    $"page={page}",
                    $"limit={limit}",
                    $"chainId={chainId}")
            );
        }

        public void RetrieveSwap(string swapid)
        {
            CallRequest(
                EndpointRequest(
                    Uri,
                    "findSwap",
                    $"swapId={swapid}")
            );
        }

        private void CallRequest(string endpointRequest)
        {
            StartCoroutine(GetRequest(endpointRequest, authenticationToken.Value));
        }

        private string EndpointRequest(string _uri, string endpoint, params string[] parameter)
        {
            var parameters = string.Join("&", parameter);
            return string.Format($"{_uri}{endpoint}?{parameters}");
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