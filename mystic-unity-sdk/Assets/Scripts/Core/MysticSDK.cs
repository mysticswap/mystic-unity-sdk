using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MetaMask.Models;
using MetaMask.Unity;
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


        public async Task<string> GetMetadata(Metadata request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(Uri, "get-metadata"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> CreateSwap(CreateSwap request)
        {
            /*
             * create-swap request
             */
            var requestBody = ConvertToJson(request);
            var createSwapResponse = await AsyncPostRequest(
                EndpointRequest(Uri, "create-swap"), requestBody, authenticationToken.Value);
            Debug.Log($"createSwapResponse: {createSwapResponse}");

            /*
             * Converting to EIP712 TypedData
             */
            SwapResponse swapResponse = JsonUtility.FromJson<SwapResponse>(createSwapResponse);
            SignatureData signatureData = new SignatureData()
            {
                domain = swapResponse.signTypedMessage.domainData,
                types = swapResponse.signTypedMessage.types,
                message = swapResponse.signTypedMessage.value,
                primaryType = "OrderComponents",
                swapId = swapResponse.swapId,
            };
            var EIP712Domain = new List<TypesComponents>()
            {
                new() { name = "name", type = "string" },
                new() { name = "version", type = "string" },
                new() { name = "chainId", type = "uint256" },
                new() { name = "verifyingContract", type = "address" },
            };
            signatureData.types.EIP712Domain = EIP712Domain;

            /*
             * Sign Request to Metamask
             */
            var jsonSignatureData = JsonUtility.ToJson(signatureData);
            Debug.Log($"jsonSignatureData: {jsonSignatureData}");
            var signature = await MetaMaskSignature(jsonSignatureData);

            /*
             * Submitting signature and swapId to SwapData
             */
            SwapData swapData = new SwapData()
            {
                signature = signature,
                swapId = swapResponse.swapId,
            };

            Debug.Log($"swapData - signature: {swapData.signature}, swapId: {swapData.swapId}");

            /*
             * Proceed the Swap creation to validateSwap
             */
            var result = await ValidateSwap(swapData);
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
                    "find-swap",
                    $"swapId={swapId}"),
                authenticationToken.Value);
            return result;
        }

        private string SearchSignature(string decryptedJson)
        {
            var pattern = @"\b0x([a-zA-Z0-9]{130})";
            Regex regex = new Regex(pattern);
            var matchedSignature = regex.Match(decryptedJson);
            return matchedSignature.Value;
        }

        public async Task<string> MetaMaskSignature(string jsonData)
        {
            var metaMaskWallet = MetaMaskUnity.Instance.Wallet;
            var from = GetAddress();
            var paramsArray = new string[] { from, jsonData };

            var request = new MetaMaskEthereumRequest()
            {
                Method = "eth_signTypedData_v4",
                Parameters = paramsArray
            };

            await metaMaskWallet.Request(request);
            var searchSignature = SearchSignature(metaMaskWallet.DecryptedJson);

            return searchSignature;
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