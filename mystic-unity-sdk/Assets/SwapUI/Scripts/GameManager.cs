using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private MysticSDK sdk;

    public GameObject NFTCollectionPanel;
    public OwnedNFTButton ownedNFTButton;

    private List<NFT> listNFTs;

    private void Awake()
    {
        sdk = MysticSDKManager.Instance.sdk;
    }


    public async void GetNFTsCollection()
    {
        // Get NFT data from Mystic SDK and convert it into OwnedNFT List.
        listNFTs = await sdk.GetOwnedNFTs();

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

    public async void LoadNFTsCollection()
    {

        while (listNFTs == null)
        {
            await Task.Yield();
        }

        foreach (var item in listNFTs)
        {
            var newOwnedNFTButton = Instantiate(ownedNFTButton, transform.position, transform.rotation);
            newOwnedNFTButton.Init(item.title, item.tokenType, item.contract.address, item.tokenId, item.balance);

            newOwnedNFTButton.transform.SetParent(NFTCollectionPanel.transform);

        }

        Debug.Log("LoadNFTsCollection excecuted");
    }
}

