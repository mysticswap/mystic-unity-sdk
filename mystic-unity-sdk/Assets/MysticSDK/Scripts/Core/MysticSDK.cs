using MetaMask.Models;
using MetaMask.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core
{
    /// <summary>
    /// Mystic Unity SDK to integrate Swaps & Marketplace API from https://docs.withmystic.xyz/swaps-and-marketplace-api
    /// </summary>
    public class MysticSDK
    {
        private const string BaseUrl = "https://mystic-swap.herokuapp.com/marketplace-api/";
        private const string OrderNotCancelled = "order not cancelled";
        private const string OrderNotAccepted = "order not accepted";
        public readonly string ContractAddress = "0x00000000000000ADc04C56Bf30aC9d3c0aAF14dC";


        public MysticSDKSession session;

        public MysticSDK(string walletAddress, string authenticationToken, int chainId)
        {
            session = new MysticSDKSession(walletAddress, authenticationToken, chainId);
        }

        /// <summary>
        /// Get the wallet address from the SDK session.
        /// </summary>
        /// <returns> The string of address.</returns>
        public string GetAddress()
        {
            return session.WalletAddress;
        }

        /// <summary>
        /// Set the address of the SDK session.
        /// </summary>
        /// <param name="address">String of address that will replace to.</param>
        public void SetAddress(string address)
        {
            session.WalletAddress = address;
        }

        /// <summary>
        /// Get the Eth and Weth Balance from the SDK session address.
        /// </summary>
        /// <returns>Return a string json contains Eth and Weth in String.</returns>
        public async Task<string> GetBalance()
        {
            var result = await AsyncGetRequest(
                EndpointRequest(BaseUrl,
                    "get-balance",
                    "address=" + session.WalletAddress,
                    "chainId=" + session.ChainId),
                session.AuthenticationToken);
            return result;
        }

        /// <summary>
        /// Get Eth balance from the SDK session address.
        /// </summary>
        /// <returns>Eth number in string.</returns>
        public async Task<string> GetBalanceEth()
        {
            var result = await AsyncGetRequest(
                EndpointRequest(BaseUrl,
                    "get-balance",
                    "address=" + session.WalletAddress,
                    "chainId=" + session.ChainId),
                session.AuthenticationToken);
            BalanceData balanceData = JsonUtility.FromJson<BalanceData>(result);
            return balanceData.ETH;
        }

        /// <summary>
        /// Get Weth balance from the SDK session address.
        /// </summary>
        /// <returns>Weth number in string.</returns>
        public async Task<string> GetBalanceWeth()
        {
            var result = await AsyncGetRequest(
                EndpointRequest(BaseUrl,
                    "get-balance",
                    "address=" + session.WalletAddress,
                    "chainId=" + session.ChainId),
                session.AuthenticationToken);
            BalanceData balanceData = JsonUtility.FromJson<BalanceData>(result);
            return balanceData.WETH;
        }

        /// <summary>
        /// Get NFTs collection from particular address.
        /// </summary>
        /// <param name="_address">By default it will be the address on SDK session.</param>
        /// <returns>String of json contains NFTs collection.</returns>
        public async Task<string> GetNfts(string _address = null)
        {
            _address = _address ?? session.WalletAddress;
            var result = await AsyncGetRequest(
                EndpointRequest(BaseUrl,
                    "get-nfts",
                    "address=" + _address,
                    "chainId=" + session.ChainId),
                session.AuthenticationToken);
            return result;
        }

        /// <summary>
        /// Get NFTs in form of OwnedNFT types (see Types.cs).
        /// </summary>
        /// <param name="_address">By default it will be the address on SDK session.</param>
        /// <returns>List of NFTs items.</returns>
        public async Task<List<NFT>> GetOwnedNFTs(string _address = null)
        {
            _address = _address ?? session.WalletAddress;
            var data = await GetNfts(_address);
            OwnedNFT ownedNft = JsonUtility.FromJson<OwnedNFT>(data);
            return ownedNft.ownedNfts;
        }


        /// <summary>
        /// Retrieve detailed Metadata on a single NFTItem.
        /// </summary>
        /// <param name="request">Metadata type of the NFTItem.</param>
        /// <returns>Detailed Metadata in json string.</returns>
        public async Task<string> GetMetadata(Metadata request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "get-metadata"), requestBody, session.AuthenticationToken);
            return result;
        }

        /// <summary>
        /// To create a new swap or offer on a listed NFT.
        /// </summary>
        /// <param name="request">CreateSwap type to be requested (please see GameManager.cs CreateSwap() to see the implementation.</param>
        /// <returns>A result report of request.</returns>
        public async Task<string> CreateSwap(CreateSwap request)
        {
            /*
             * create-swap request
             */
            var requestBody = ConvertToJson(request);
            Debug.Log($"requestSwap: {requestBody}");
            var createSwapResponse = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "create-swap"), requestBody, session.AuthenticationToken);
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
            // swapResponse.approvalsNeeded
            var EIP712Domain = new List<TypesComponents>()
            {
                new() { name = "name", type = "string" },
                new() { name = "version", type = "string" },
                new() { name = "chainId", type = "uint256" },
                new() { name = "verifyingContract", type = "address" },
            };
            signatureData.types.EIP712Domain = EIP712Domain;

            /*
             * Approvals
             */
            bool isApprovalNeeded = IsApprovalsNeeded(swapResponse);
            if (isApprovalNeeded)
            {
                var listApprovals = swapResponse.approvalsNeeded;
                foreach (var approval in listApprovals)
                {
                    var approvalResult = await MetaMaskSendTransactionApprovals(approval.data, approval.to);

                    Debug.Log($"Approvals done: {approvalResult}");
                }
            }


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

        private bool IsApprovalsNeeded(SwapResponse swapResponse)
        {
            var listApprovals = swapResponse.approvalsNeeded;
            bool isApprovalsNeeded = listApprovals.Any();

            return isApprovalsNeeded;
        }

        private async Task<string> MetaMaskSendTransactionApprovals(string data, string to)
        {
            var metaMaskWallet = MetaMaskUnity.Instance.Wallet;
            var transactionParams = new MetaMaskTransaction()
            {
                From = MysticSDKManager.Instance.sdk.session.WalletAddress,
                Data = data,
                To = to,
            };

            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_sendTransaction",
                Parameters = new MetaMaskTransaction[] { transactionParams }
            };

            await metaMaskWallet.Request(request);
            return metaMaskWallet.DecryptedJson;
        }

        public async Task<string> ValidateSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var result = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "validate-swap"), requestBody, session.AuthenticationToken);
            return result;
        }

        /// <summary>
        /// Accept a listed swap / offer.
        /// </summary>
        /// <param name="request">SwapData type that contains swapId and takerAddress.</param>
        /// <returns>A result of request.</returns>
        public async Task<string> AcceptSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var acceptSwapData = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "accept-swap"), requestBody, session.AuthenticationToken);
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
                result = await AsyncGetRequest($"{BaseUrl}verify-accepted/{swapId}", session.AuthenticationToken);
                Debug.Log($"VerifySwapAccepted retry: {++retry}");
            }

            return result;
        }

        /// <summary>
        /// Cancel a listed swap / offer.
        /// </summary>
        /// <param name="request">SwapData type that contains swapId and takerAddress.</param>
        /// <returns>A result of request.</returns>
        public async Task<string> CancelSwap(SwapData request)
        {
            var requestBody = ConvertToJson(request);
            var cancelSwapData = await AsyncPostRequest(
                EndpointRequest(BaseUrl, "cancel-swap"), requestBody, session.AuthenticationToken);

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
                result = await AsyncGetRequest($"{BaseUrl}verify-cancelled/{_swapId}", session.AuthenticationToken);
                Debug.Log($"VerifySwapCancelled retry: {++retry}");
            }

            return result;
        }

        /// <summary>
        /// Retrieve all existing swaps.
        /// </summary>
        /// <param name="page">Number of pages we want to retrieve. Default is 1.</param>
        /// <param name="limit">Number of swaps we want to retrieve. Default is 20, limit is 100</param>
        /// <returns>A list of swaps according to the made request. </returns>
        public async Task<string> RetrieveAllSwaps(int page = 1, int limit = 20)
        {
            var result = await AsyncGetRequest(
                EndpointRequest(
                    BaseUrl,
                    "all-swaps",
                    $"page={page}",
                    $"limit={limit}",
                    $"chainId={session.ChainId}"),
                session.AuthenticationToken);
            return result;
        }

        /// <summary>
        /// Retrieve all existing swaps but only for the particular address.
        /// </summary>
        /// <param name="page">Number of pages we want to retrieve. Default is 1.</param>
        /// <param name="limit">Number of swaps we want to retrieve. Default is 20, limit is 100</param>
        /// <param name="creatorAddress">By default the address from SDK session.</param>
        /// <param name="takerAddress">By default the address from SDK session.</param>
        /// <returns>A list of swaps according to the made request.</returns>
        public async Task<string> RetrieveMySwaps(int page = 1, int limit = 20, string creatorAddress = null,
            string takerAddress = null)
        {
            creatorAddress = creatorAddress ?? session.WalletAddress;
            takerAddress = takerAddress ?? session.WalletAddress;
            var result = await AsyncGetRequest(
                EndpointRequest(
                    BaseUrl,
                    "all-swaps",
                    $"page={page}",
                    $"limit={limit}",
                    $"creatorAddress={creatorAddress}",
                    $"takerAddress={takerAddress}",
                    $"chainId={session.ChainId}"),
                session.AuthenticationToken);
            return result;
        }

        /// <summary>
        /// Retrieve a single swap object.
        /// </summary>
        /// <param name="swapId">String of swapID.</param>
        /// <returns>The respective swap object.</returns>
        public async Task<string> RetrieveSwap(string swapId)
        {
            var result = await AsyncGetRequest(
                EndpointRequest(
                    BaseUrl,
                    "find-swap",
                    $"swapId={swapId}"),
                session.AuthenticationToken);
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
            var from = session.WalletAddress;
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
                From = session.WalletAddress,
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