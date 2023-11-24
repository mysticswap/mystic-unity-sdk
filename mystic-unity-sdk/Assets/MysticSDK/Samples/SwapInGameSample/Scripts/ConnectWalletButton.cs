using Core;
using MetaMask;
using MetaMask.Unity;
using System.Threading.Tasks;
using UnityEngine;

public class ConnectWalletButton : MonoBehaviour
{
    private MysticSDK sdk;
    private MetaMaskWallet wallet;
    [SerializeField] private GameEvent OnWalletOfferConnected;
    

    public async void ConnectWallet()
    {
        sdk = MysticSDKManager.Instance.sdk;
        wallet = MetaMaskUnity.Instance.Wallet;
        wallet.Connect();

        while (wallet.ConnectedAddress == string.Empty)
        {
            await Task.Yield();
        }

        MysticSDKManager.Instance.ConnectedAddress = wallet.ConnectedAddress;
        OnWalletOfferConnected.Raise();
    }

}
