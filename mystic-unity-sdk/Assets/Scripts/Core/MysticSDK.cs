using MetaMask.Models;
using MetaMask.Unity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core
{
    public class MysticSDK : MonoBehaviour
    {
        [SerializeField] private StringVariable walletAddress;
        [SerializeField] private StringVariable authenticationToken;
        [SerializeField] private StringVariable swapId;
        [SerializeField] private StringVariable chainId;
        private const string BaseUrl = "https://mystic-swap.herokuapp.com/marketplace-api/";
        private const string OrderNotCancelled = "order not cancelled";
        private const string OrderNotAccepted = "order not accepted";


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
                EndpointRequest(BaseUrl,
                    "get-balance",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId.Value),
                authenticationToken.Value);
            return result;
        }

        public async Task<string> GetNfts()
        {
            var result = await AsyncGetRequest(
                EndpointRequest(BaseUrl,
                    "get-nfts",
                    "address=" + walletAddress.Value,
                    "chainId=" + chainId.Value),
                authenticationToken.Value);
            return result;
        }


        public async Task<string> GetMetadata(Metadata request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "get-metadata"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> CreateSwap(CreateSwap request)
        {
            /*
             * create-swap request
             */
            var requestBody = ConvertToJson(request);
            var createSwapResponse = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "create-swap"), requestBody, authenticationToken.Value);
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

            swapId.SetValue(swapResponse.swapId);

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
                EndpointRequest(BaseUrl, "validate-swap"), requestBody, authenticationToken.Value);
            return result;
        }

        public async Task<string> AcceptSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var acceptSwapData = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "accept-swap"), requestBody, authenticationToken.Value);
            Debug.Log($"acceptSwapData: {acceptSwapData}");
            var resultTransaction = await MetaMaskSendTransaction(acceptSwapData);
            var resultVerifyAccepted = await VerifySwapAccepted(request.swapId);

            return $"{resultTransaction}\n{resultVerifyAccepted}";
        }

        public async Task<string> VerifySwapAccepted(string swapId)
        {
            var result = OrderNotAccepted;
            var retry = 0;
            while (result == OrderNotAccepted)
            {
                result = await AsyncGetRequest($"{BaseUrl}verify-accepted/{swapId}", authenticationToken.Value);
                Debug.Log($"VerifySwapAccepted retry: {++retry}");
            }
            return result;
        }

        public async Task<string> CancelSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var cancelSwapData = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "cancel-swap"), requestBody, authenticationToken.Value);

            /*
             * SendTransaction with MetaMask
             */
            var resultTransaction = await MetaMaskSendTransaction(cancelSwapData);
            var resultVerifyCancelled = await VerifySwapCancelled(request.swapId);

            return $"{resultTransaction}\n{resultVerifyCancelled}";
        }

        private async Task<string> VerifySwapCancelled(string _swapId)
        {
            var result = OrderNotCancelled;
            var retry = 0;
            while (result == OrderNotCancelled)
            {
                result = await AsyncGetRequest($"{BaseUrl}verify-cancelled/{_swapId}", authenticationToken.Value);
                Debug.Log($"VerifySwapCancelled retry: {++retry}");
            }
            return result;
        }

        public async Task<string> RetrieveAllSwaps(int page, int limit)
        {
            var result = await AsyncGetRequest(
                EndpointRequest(
                    BaseUrl,
                    "all-swaps",
                    $"page={page}",
                    $"limit={limit}",
                    $"chainId={chainId.Value}"),
                authenticationToken.Value);
            return result;
        }

        public async Task<string> RetrieveSwap(string swapId)
        {
            var result = await AsyncGetRequest(
                EndpointRequest(
                    BaseUrl,
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

        private async Task<string> MetaMaskSendTransaction(string data)
        {
            var metaMaskWallet = MetaMaskUnity.Instance.Wallet;
            var transactionDatas = TransactionData.DeserializedJson(data);
            var transactionParams = new MetaMaskTransaction()
            {
                From = GetAddress(),
                Data = transactionDatas[0].data,
                To = transactionDatas[0].to,
                Value = transactionDatas[0].value.hex,
            };


            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_sendTransaction",
                Parameters = new MetaMaskTransaction[] { transactionParams }
            };

            await metaMaskWallet.Request(request);
            return metaMaskWallet.DecryptedJson;
        }

        private string EndpointRequest(string _baseUrl, string endpoint, params string[] parameter)
        {
            var parameters = string.Join("&", parameter);
            return string.Format($"{_baseUrl}{endpoint}?{parameters}");
        }

        private string ConvertToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        private async Task<string> AsyncGetRequest(string baseUrl, string authToken)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl);
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

        private async Task<string> AsyncPostRequest(string baseUrl, string request, string authToken)
        {
            UnityWebRequest webRequest = UnityWebRequest.Post(baseUrl, request, "application/json");
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