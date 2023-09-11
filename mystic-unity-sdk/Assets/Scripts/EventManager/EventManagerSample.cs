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

    public void JsonTest()
    {
        OwnedNFT ownedNft = JsonUtility.FromJson<OwnedNFT>(sdk.JsonString);
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

        Debugger.Instance.Log("test Json", sb.ToString());
    }

    // void OnWalletConnected(object sender, EventArgs e)
    // {
    //     Debugger.Instance.Log("Metamask Wallet", "Wallet is connected!");
    //     SetAddress();
    // }
}