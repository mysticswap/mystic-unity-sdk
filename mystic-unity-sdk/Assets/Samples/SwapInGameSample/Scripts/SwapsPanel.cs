using Core;
using System.Collections.Generic;
using TMPro;
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
    private const string statusCancelled = "cancelled";
    private const string statusAccepted = "accepted";
    private const string textButtonAccept = "Accept";
    private const string textButtonCancel = "Cancel";
    private const string textButtonAccepted = "Accepted";
    private const string textButtonCancelled = "Cancelled";
    [SerializeField] private TMP_Text addressOfferingText;
    [SerializeField] private TMP_Text addressForText;
    [SerializeField] private NFTsItem nFTsItem;
    [SerializeField] private GameObject offerPanel;
    [SerializeField] private GameObject considerationPanel;
    [SerializeField] private Button swapInteractionButton;
    [SerializeField] private TMP_Text swapInteractionText;
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

        SettingUpButton(SwapStatus, CreatorAddress);
        SettingUpTexts();

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

    private void SettingUpButton(string status, string address)
    {
        var connectedAddress = MysticSDKManager.Instance.sdk.GetAddress();
        bool isButton = (status == statusValidated);
        if (isButton)
        {
            var textButton = (address == connectedAddress) ? textButtonCancel : textButtonAccept;
            swapInteractionButton.gameObject.SetActive(true);
            swapInteractionText.gameObject.SetActive(false);
            swapInteractionButton.GetComponentInChildren<Text>().text = textButton;
        }
        else
        {
            var textButton = (status == statusCancelled) ? textButtonCancelled : textButtonAccepted;
            swapInteractionButton.gameObject.SetActive(false);
            swapInteractionText.gameObject.SetActive(true);
            swapInteractionText.text = textButton;
        }

    }

    private void SettingUpTexts()
    {
        addressOfferingText.text = $"{CreatorAddress[..6]}...{CreatorAddress[38..]}";
        addressForText.text = (TakerAddress != string.Empty) ? $"{TakerAddress[..6]}...{TakerAddress[38..]}" : string.Empty;
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

}
