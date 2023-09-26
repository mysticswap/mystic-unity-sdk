using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core;
using MetaMask;
using MetaMask.Models;
using MetaMask.Unity;
using UnityEngine;


public class EventManagerSample : MonoBehaviour
{
    // private MetaMaskUnity wallet = MetaMaskUnity.Instance.Wallet;
    private MetaMaskWallet wallet;
    private MysticSDK sdk;
    private string jsonResponse;
    [SerializeField] private StringVariable jsonSwapTest;
    [SerializeField] private StringVariable jsonSwapResponse;
    [SerializeField] private StringVariable signature;
    [SerializeField] private StringVariable swapId;
    public int Nonce = 0;

    private void Awake()
    {
        sdk = MysticSDKManager.Instance.sdk;
        MetaMaskUnity.Instance.Initialize();
        // wallet.WalletConnectedHandler += OnWalletConnected;
    }


    public void TestEvent()
    {
        Debug.Log("Test Event is Raised.");

        #region MysticSDK test

        // MysticSDK sdk = MysticSDKManager.Instance.sdk;
        // sdk.GetBalance();
        // sdk.GetNfts();
        // Debugger.Instance.Log("Testing debugger", "This is debugger's description");

        #endregion

        #region MetaMask Test

        wallet = MetaMaskUnity.Instance.Wallet;
        wallet.Connect();
        Debug.Log("MM Connected");

        #endregion
    }

    public void MetaMaskConnect()
    {
        wallet = MetaMaskUnity.Instance.Wallet;
        wallet.Connect();
    }

    public void WalletDisconnect()
    {
        wallet.Disconnect();
        wallet.Dispose();
    }

    public void GetAddress()
    {
        #region MetaMask

        Debugger.Instance.Log("Metamask Wallet", $"{wallet.ConnectedAddress}\n{wallet.SelectedAddress}");

        #endregion
    }

    public void SetAddress()
    {
        sdk.SetAddress(wallet.ConnectedAddress);
        Debugger.Instance.Log("Mystic SDK Wallet", $"Mystic Wallet address has been set to {sdk.GetAddress()}");
    }

    public async void FromJsonTest()
    {
        var result = await sdk.GetNfts();
        jsonResponse = result;
        OwnedNFT ownedNft = JsonUtility.FromJson<OwnedNFT>(jsonResponse);
        var nftList = ownedNft.ownedNfts;
        var sb = new StringBuilder();
        foreach (var item in nftList)
        {
            sb.AppendLine(item.title);
            sb.AppendLine(item.description);
            sb.AppendLine(item.rawMetadata.image);
            sb.AppendLine(item.balance.ToString());
            sb.AppendLine();
        }

        Debugger.Instance.Log("test From Json", sb.ToString());
    }


    public async void SwapTest()
    {
        var selectedOffer = new Offer()
        {
            itemtype = "ERC721",
            token = "0x037aca480459ae361a87b023f189532d80cb6769",
            identifier = "49",
            amount = "1",
        };

        var selectedConsideration = new Consideration()
        {
            itemtype = "NATIVE",
            token = "0x0000000000000000000000000000000000000000",
            identifier = "0",
            amount = "50000000000000000",
        };

        var swap = new CreateSwap()
        {
            chainId = 5,
            offerer = "0xCBD21691e26Da7FFA64cB1a6C47832fDAEE0Acce",
            creatorAddress = "0xCBD21691e26Da7FFA64cB1a6C47832fDAEE0Acce",
            contractAddress = "0x00000000000000ADc04C56Bf30aC9d3c0aAF14dC",
            offer = new List<Offer>() { selectedOffer },
            consideration = new List<Consideration>() { selectedConsideration },
        };

        var result = await sdk.CreateSwap(swap);
        Debugger.Instance.Log("Create Swap", $"{result}");
    }

    public void ToJsonTest()
    {
        var fees = new List<Fees>();
        var offers = new List<Offer>();
        var considerations = new List<Consideration>();

        var selectedFee = new Fees()
        {
            recipient = "0x123456789abcdef145216789fcfcf123456745d",
            basisPoints = 500,
        };

        var selectedOffer = new Offer()
        {
            itemtype = "ERC721",
            token = "0x74cb5611e89078b2e5cb638a873cf7bddc588659",
            identifier = "633",
            amount = "1",
        };

        var selectedConsideration1 = new Consideration()
        {
            itemtype = "ERC721",
            token = "0x74cb5611e89078b2e5cb638a873cf7bddc588659",
            identifier = "34",
            amount = "1",
        };
        var selectedConsideration2 = new Consideration()
        {
            itemtype = "NATIVE",
            token = "0x123456789abcdef123456789abcdef123456789a",
            identifier = "ETH",
            amount = "500000000000000000",
        };

        fees.Add(selectedFee);
        offers.Add(selectedOffer);
        considerations.Add(selectedConsideration1);
        considerations.Add(selectedConsideration2);
        var swap1 = new CreateSwap()
        {
            chainId = 1,
            offerer = "0x123456789abcdef145216789abcdef123456787f",
            contractAddress = "0x00000000000000ADc04C56Bf30aC9d3c0aAF14dC",
            fees = fees,
            offer = offers,
            consideration = considerations,
        };

        var swap1Json = JsonUtility.ToJson(swap1);
        Debugger.Instance.Log("test To Json", swap1Json);
    }

    public void FetchSwapData()
    {
        var swapData = JsonUtility.FromJson<SwapData>(jsonResponse);
        Debugger.Instance.Log("Swap Data",
            $"signature: {swapData.signature}\n" +
            $"swapId: {swapData.swapId}\n"
        );
    }

    public async void GetBalanceTest()
    {
        var balance = await sdk.GetBalance();
        Debugger.Instance.Log("test balance async", $"balance: {balance}");
    }

    public async void SignSwapTest()
    {
        var msgParams = jsonSwapTest.Value;
        var from = sdk.GetAddress();

        var paramsArray = new string[] { from, msgParams };
        var request = new MetaMaskEthereumRequest()
        {
            Method = "eth_signTypedData_v4",
            Parameters = paramsArray
        };
        var result = await MetaMaskUnity.Instance.Wallet.Request(request);
        MetaMaskResponse mmResult = new MetaMaskResponse();

        var jsonResult = JsonUtility.ToJson(result);
        Debug.Log($"jsonResult: {jsonResult}");

        // var signature = _signer.SignTypedDataV4(request);

        // var signData = JsonUtility.FromJson<SignData>(jsonSwapTest.Value);
        // Debug.Log(signData.signTypedMessage);
        // var request = new MetaMaskEthereumRequest()
        // {
        //
        // };
    }

    public void MetaMaskDisconnect()
    {
        MetaMaskUnity.Instance.Wallet.Disconnect();
    }

    public async void SendTransaction()
    {
        var transactionParams = new MetaMaskTransaction
        {
            To = "0x037acA480459Ae361a87b023f189532d80cB6769",
            From = MetaMaskUnity.Instance.Wallet.SelectedAddress,
            Value = "0",
            Data =
                "0xa22cb4650000000000000000000000001e0049783f008a0085193e00003d00cd54003c710000000000000000000000000000000000000000000000000000000000000001",
            ChainId = "5"
        };

        var request = new MetaMaskEthereumRequest
        {
            Method = "eth_sendTransaction",
            Parameters = new MetaMaskTransaction[] { transactionParams }
        };
        await MetaMaskUnity.Instance.Wallet.Request(request);
    }


    public async void ValidateSwapTest()
    {
        var swapData = new SwapData()
        {
            signature = signature.Value,
            swapId = swapId.Value,
        };

        var result = await sdk.ValidateSwap(swapData);
        Debugger.Instance.Log("ValidateSwapTest", result);
    }

    public async void VerifyAcceptedSwapTest()
    {
        var result = await sdk.VerifySwap(swapId.Value);
        Debugger.Instance.Log("VerifyAcceptedSwapTest", result);
    }

    public async void TestRetrieveSwap()
    {
        var result = await sdk.RetrieveSwap("6512e56b1f61ba180f108927");
        Debugger.Instance.Log("Retrieve Swap", result);
    }

    public async void TestRetrieveAllSwap()
    {
        var result = await sdk.RetrieveAllSwaps(1, 20);
        Debugger.Instance.Log("Retrieve All Swap", result);
    }


}