using Core;
using System.Collections.Generic;
using UnityEngine;

public class SwapsPanel : MonoBehaviour
{
    public string CreatorAddress { get; private set; }
    public string TakerAddress { get; private set; }
    public string SwapId { get; private set; }
    public List<NFT> NFTsOffer { get; private set; }
    public List<NFT> NFTsConsideration { get; private set; }
    public string SwapStatus { get; private set; }
    [SerializeField] private NFTsItem nFTsItem;
    [SerializeField] private GameObject offerPanel;
    [SerializeField] private GameObject considerationPanel;


    public void Init(string creatorAddress, string takerAddress, string swapId, List<NFT> nFTsOffer, List<NFT> nFTsConsideration, string swapStatus)
    {
        CreatorAddress = creatorAddress;
        TakerAddress = takerAddress;
        SwapId = swapId;
        NFTsOffer = nFTsOffer;
        NFTsConsideration = nFTsConsideration;
        SwapStatus = swapStatus;
        GenerateNFTsItem(NFTsOffer, nFTsItem, offerPanel);
        GenerateNFTsItem(NFTsConsideration, nFTsItem, considerationPanel);
    }

    public void GenerateNFTsItem(List<NFT> listNft, NFTsItem _nFTsItem, GameObject _parentPanel)
    {
        foreach (var nft in listNft)
        {
            var newNFTsItem = Instantiate(_nFTsItem, transform.position, transform.rotation);
            newNFTsItem.Init(nft.title, nft.rawMetadata.image);

            newNFTsItem.transform.SetParent(_parentPanel.transform);

        }

    }
}
