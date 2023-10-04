using Core;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private MysticSDK sdk;

    private void Awake()
    {
        sdk = MysticSDKManager.Instance.sdk;
    }


    public async void GetNFTsCollection()
    {
        // Get NFT data from Mystic SDK and convert it into OwnedNFT List.
        var listNFTs = await sdk.GetOwnedNFTs();

        // Accessing all the NFTs collections
        foreach (var item in listNFTs)
        {
            Debug.Log(
                $"itemType: {item.tokenType}\n" +
                $"token: {item.contract.address}\n" +
                $"identifier: {item.tokenId}\n" +
                $"imageUrl: {item.rawMetadata.image}"
                );
        }

    }
}

