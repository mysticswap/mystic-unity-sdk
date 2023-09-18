using System;
using System.Collections.Generic;
using System.Text;
using Core;
using MetaMask;
using MetaMask.Unity;
using UnityEngine;

public class EventManagerSample : MonoBehaviour
{
    // private MetaMaskUnity wallet = MetaMaskUnity.Instance.Wallet;
    private MetaMaskWallet wallet;
    private MysticSDK sdk;
    private string jsonResponse;

    private void Awake()
    {
        sdk = MysticSDKManager.Instance.sdk;
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
            token = "0x229dd7144fec1008dddf5fcf779ec63c3d576aa7",
            identifier = "47",
            amount = "1",
        };

        var selectedConsideration = new Consideration()
        {
            itemtype = "NATIVE",
            token = "0x0000000000000000000000000000000000000000",
            identifier = "0",
            amount = "10000000000000000",
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
        jsonResponse = result;
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
            $"swapId: {swapData.swapId}\n" +
            $"takerAddress: {swapData.takerAddress}");
    }

    public async void GetBalanceTest()
    {
        var balance = await sdk.GetBalance();
        Debugger.Instance.Log("test balance async", $"balance: {balance}");
    }

    // void OnWalletConnected(object sender, EventArgs e)
    // {
    //     Debugger.Instance.Log("Metamask Wallet", "Wallet is connected!");
    //     SetAddress();
    // }
}