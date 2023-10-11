using Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapsPanel : MonoBehaviour
{
    public string CreatorAddress { get; private set; }
    public string TakerAddress { get; private set; }
    public string SwapId { get; private set; }
    public List<NFT> NFTsOffer { get; private set; }
    public List<NFT> NFTsConsideration { get; private set; }
    public string SwapStatus { get; private set; }
    private const string statusValidated = "validated";
    private const string textButtonAccept = "Accept";
    private const string textButtonCancel = "Cancel";
    [SerializeField] private NFTsItem nFTsItem;
    [SerializeField] private GameObject offerPanel;
    [SerializeField] private GameObject considerationPanel;
    [SerializeField] private Button swapInteractionButton;
    private bool isSwapAcceptable;
    private SwapData swapData;


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

        isSwapAcceptable = IsSwapAcceptable(SwapStatus, CreatorAddress);
        settingUpButton(isSwapAcceptable);
        swapData = new SwapData()
        {
            swapId = SwapId,
            takerAddress = TakerAddress,
        };
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

    private bool IsSwapAcceptable(string status, string address)
    {
        var connectedAddress = MysticSDKManager.Instance.sdk.GetAddress();

        bool _isSwapAcceptable = (status == statusValidated && address != connectedAddress) ? true : false;
        return _isSwapAcceptable;
    }

    private void settingUpButton(bool isAcceptButton)
    {
        string textButton;
        if (isAcceptButton)
        {
            textButton = textButtonAccept;
        }
        else
        {
            textButton = textButtonCancel;
        }
        swapInteractionButton.GetComponentInChildren<Text>().text = textButton;
    }

    public async void OnClickSwapInteraction()
    {
        var sdk = MysticSDKManager.Instance.sdk;
        if (isSwapAcceptable)
        {
            await sdk.AcceptSwap(swapData);
        }
        else
        {
            await sdk.CancelSwap(swapData);
        }
    }



    //public void OnClickAcceptOrCancel()
    //{



    //}
}
