using Core;
using MetaMask;
using MetaMask.Models;
using MetaMask.Unity;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField] private StringVariable takerAddress;
    [SerializeField] private StringVariable cancelSwapData;
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

    public async void GetNftsCollection()
    {
        var result = await sdk.GetNfts();
        Debugger.Instance.Log("Get NFT", $"{result}");
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
            identifier = "66",
            amount = "1",
        };

        var selectedConsideration = new Consideration()
        {
            itemtype = "NATIVE",
            token = "0x0000000000000000000000000000000000000000",
            identifier = "0",
            amount = "21000000000000000",
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
            token = "0x74cb5611e89078b2e5cb638a873cf7bddc58659",
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
        var result = await sdk.VerifySwapAccepted(swapId.Value);
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


    public async void TestGetMetadata()
    {
        MetadataArray metadataArray = new MetadataArray()
        {
            type = "NFT",
            contractAddress = "0x74cb5611e89078b2e5cb638a873cf7bddc588659",
            tokenId = "633"
        };

        Metadata metadata = new Metadata()
        {
            chainId = 1,
            metadataArray = new List<MetadataArray>() { metadataArray }
        };

        var result = await sdk.GetMetadata(metadata);
        Debugger.Instance.Log("Metadata", result);
    }

    public async void TestCancelSwap()
    {
        SwapData swapData = new SwapData()
        {
            swapId = swapId.Value,
        };

        var result = await sdk.CancelSwap(swapData);
        Debugger.Instance.Log("Cancel Swap", result);
    }

    public async void TestAcceptSwap()
    {
        SwapData swapData = new SwapData()
        {
            swapId = swapId.Value,
            takerAddress = takerAddress.Value,
        };

        var result = await sdk.AcceptSwap(swapData);
        Debugger.Instance.Log("Accept Swap", result);
    }

    public async void TestSendRawTransaction()
    {
        var transactionParams = new MetaMaskTransaction
        {
            // To = "0x00000000000000ADc04C56Bf30aC9d3c0aAF14dC",
            From = MetaMaskUnity.Instance.Wallet.SelectedAddress,
            To = "0x00000000000000ADc04C56Bf30aC9d3c0aAF14dC",
            Data =
                "0xfb0f3ee100000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000592c5127dcc0000000000000000000000000005ee3813e9a4e9f3f92f93aff8f6c119d4a8a7430000000000000000000000000cbd21691e26da7ffa64cb1a6c47832fdaee0acce000000000000000000000000037aca480459ae361a87b023f189532d80cb6769000000000000000000000000000000000000000000000000000000000000003e0000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000200000000000000000000000000000000000000000000000000000000651a1f2600000000000000000000000000000000000000000000000000000000654238af000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000125fbd6d4560000007b02230091a7ed01230072f7006a004d60a8d4e71d599b8104250f0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000240000000000000000000000000000000000000000000000000000000000000026000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000041b24178724dceb64350d61c98e68f7be3ced656eaeeb759f7c3eb00acf4ebccac656e9e19bc0667876765cec89b1319983cfc4f14ef2e593fbab08a3e3a45eea81b00000000000000000000000000000000000000000000000000000000000000",

            // ChainId = "5"
        };

        Debug.Log($"Wallet: {MetaMaskUnity.Instance.Wallet.SelectedAddress}");

        // var transactionData = "0xfd9f1e10000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000020000000000000000000000000cbd21691e26da7ffa64cb1a6c47832fdaee0acce00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000160000000000000000000000000000000000000000000000000000000000000022000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000065136699ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000015c88509ea40000007b02230091a7ed01230072f7006a004d60a8d4e71d599b8104250f0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000002000000000000000000000000037aca480459ae361a87b023f189532d80cb67690000000000000000000000000000000000000000000000000000000000000042000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004a9b6384488000000000000000000000000000000000000000000000000000004a9b6384488000000000000000000000000000cbd21691e26da7ffa64cb1a6c47832fdaee0acce";

        var request = new MetaMaskEthereumRequest
        {
            Method = "eth_sendTransaction",
            Parameters = new MetaMaskTransaction[] { transactionParams }
            // Parameters = transactionData
        };
        await MetaMaskUnity.Instance.Wallet.Request(request);
    }

    // public async void TestVerifyCancelled()
    // {
    //     var result = await sdk.VerifySwapCancelled(swapId.Value);
    //     Debugger.Instance.Log("Test Verify Cancelled", result);
    // }

    // public async void TestGetJsonArray()
    // {
    //     // var input = @"[{""data"":""0xfd9f1e10000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000020000000000000000000000000cbd21691e26da7ffa64cb1a6c47832fdaee0acce0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000016000000000000000000000000000000000000000000000000000000000000002200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006512e56affffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000095a77cfea30000007b02230091a7ed01230072f7006a004d60a8d4e71d599b8104250f0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000002000000000000000000000000037aca480459ae361a87b023f189532d80cb6769000000000000000000000000000000000000000000000000000000000000003100000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000b1a2bc2ec5000000000000000000000000000000000000000000000000000000b1a2bc2ec50000000000000000000000000000cbd21691e26da7ffa64cb1a6c47832fdaee0acce"",""to"":""0x00000000000000ADc04C56Bf30aC9d3c0aAF14dC""}]";
    //     var input = cancelSwapData.Value;
    //     var output = TransactionData.DeserializedJson(input);
    //     Debug.Log($"data: {output[0].data}");
    //     Debug.Log($"to: {output[0].to}");
    //
    //     var result = await sdk.MetaMaskSendTransaction(input);
    //     if (result == null) Debug.Log($"rejected result: {(string)null}");
    //     Debug.Log($"result: {result}");
    //
    //     if (result != null)
    //     {
    //         Debug.Log("TESTTESTE");
    //     }
    // }
}