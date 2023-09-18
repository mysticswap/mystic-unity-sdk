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



        public void SetAddress(string address)
        {
            walletAddress.SetValue(address);
        }

        public string GetAddress()
        {
            return walletAddress.Value;
        }

        public async Task<string> GetBalance()
        {
            var result = await AsyncGetRequest(
                EndpointRequest(Uri,
                    "get-balance",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId),
                authenticationToken.Value);
            return result;
        }

        public async Task<string> GetNfts()
        {
            var result = await AsyncGetRequest(
                EndpointRequest(Uri,
                    "get-nfts",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId),
                authenticationToken.Value);
            return result;
        }

        public async Task<string> CreateSwap(CreateSwap request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(Uri, "create-swap"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> ValidateSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(Uri, "validate-swap"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> AcceptSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(Uri, "accept-swap"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> VerifySwap(string swapId)
        {
            var result = await AsyncGetRequest($"{UriVerifySwap}{swapId}", authenticationToken.Value);
            return result;
        }

        public async Task<string> CancelSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(Uri, "cancel-swap"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> VerifyCancelled(string swapId)
        {
            var result = await AsyncGetRequest($"{UriVerifyCanceled}{swapId}", authenticationToken.Value);
            return result;
        }

        public async Task<string> RetrieveAllSwaps(int page, int limit)
        {
            var result = await AsyncGetRequest(
                EndpointRequest(
                    Uri,
                    "all-swaps",
                    $"page={page}",
                    $"limit={limit}",
                    $"chainId={chainId}"),
                authenticationToken.Value);
            return result;
        }

        public async Task<string> RetrieveSwap(string swapId)
        {
            var result = await AsyncGetRequest(
                EndpointRequest(
                    Uri,
                    "findSwap",
                    $"swapId={swapId}"),
                authenticationToken.Value);
            return result;
        }

        private string EndpointRequest(string _uri, string endpoint, params string[] parameter)
        {
            var parameters = string.Join("&", parameter);
            return string.Format($"{_uri}{endpoint}?{parameters}");
        }

        private string ConvertToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        private async Task<string> AsyncGetRequest(string uri, string authToken)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            webRequest.SetRequestHeader("Authorization", authToken);
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debugger.Instance.Log("Web Request GET", $"Failed to request: {webRequest.error}");
                return webRequest.error;
            }
            else
            {
                return webRequest.downloadHandler.text;
            }
        }

        private async Task<string> AsyncPostRequest(string uri, string request, string authToken)
        {
            UnityWebRequest webRequest = UnityWebRequest.Post(uri, request, "application/json");
            webRequest.SetRequestHeader("Authorization", authToken);
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debugger.Instance.Log("Web Request POST", $"Failed to request: {webRequest.error}");
                return webRequest.error;
            }
            else
            {
                return webRequest.downloadHandler.text;
            }
        }
    }
}