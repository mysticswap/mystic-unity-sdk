using Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private MysticSDK sdk;

    public GameObject OfferCollectionsPanel;
    public GameObject RequestCollectionsPanel;
    [SerializeField] private TextMeshProUGUI OfferAddress;
    [SerializeField] private TextMeshProUGUI RequestAddress;
    [SerializeField] private TMP_InputField RequesterAddress;

    [SerializeField] private Button GetNFTsOfferButton;
    [SerializeField] private Button GetNFTsRequestButton;

    public OwnedNFTButton ownedNFTButton;

    private List<NFT> listNFTs;

    private void Awake()
    {
        sdk = MysticSDKManager.Instance.sdk;
    }

    private void Start()
    {
        OfferAddress.text = ShortenAddress(sdk.GetAddress());
        sdk.session.OfferAddress = sdk.GetAddress();

        GetNFTsOfferButton.onClick.AddListener(delegate { GetLoadNFTsCollection(sdk.session.OfferAddress, OfferCollectionsPanel); });
        GetNFTsRequestButton.onClick.AddListener(delegate { GetLoadNFTsCollection(sdk.session.RequestAddress, RequestCollectionsPanel); });
    }

    public void SetRequestAddress()
    {
        sdk.session.RequestAddress = RequesterAddress.text;
    }

    public async void GetLoadNFTsCollection(string _ownerAddress, GameObject _parentPanel)
    {
        Debug.Log("GetLoadNFTsCollection");
        Debug.Log($"address: {_ownerAddress}");
        List<NFT> list = new List<NFT>();

        list = await sdk.GetOwnedNFTs(_ownerAddress);
        Debug.Log($"list count: {list.Count}");

        LoadNFTsCollection(_ownerAddress, list, _parentPanel);

    }


    public async void GetNFTsCollection()
    {
        var ownerAddress = sdk.GetAddress();
        // Get NFT data from Mystic SDK and convert it into OwnedNFT List.
        listNFTs = await sdk.GetOwnedNFTs(ownerAddress);

        Debug.Log($"list count: {listNFTs.Count}");

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

        LoadNFTsCollection(ownerAddress, listNFTs, OfferCollectionsPanel);
    }

    private async void LoadNFTsCollection(string _ownerAddress, List<NFT> _list, GameObject parentPanel)
    {

        while (_list == null)
        {
            await Task.Yield();
        }

        foreach (var item in _list)
        {
            var newOwnedNFTButton = Instantiate(ownedNFTButton, transform.position, transform.rotation);
            newOwnedNFTButton.Init(item.title, item.tokenType, item.contract.address, item.tokenId, item.balance, _ownerAddress);

            newOwnedNFTButton.transform.SetParent(parentPanel.transform);

        }

        Debug.Log("LoadNFTsCollection excecuted");
    }

    public void RetrieveOfffers()
    {
        var selectedOffers = sdk.session.SelectedOffers;
        Debug.Log($"Selected Offers: {selectedOffers.Count}");
        foreach (var item in selectedOffers)
        {
            Debug.Log($"{item}");
        }
    }


    private string ShortenAddress(string address)
    {
        if (address.Length != 42)
            throw new ArgumentException("Invalid Address Length.");
        return $"{address[..6]}...{address[38..]}";
    }

    public void ShortenRequestAddress()
    {
        RequestAddress.text = ShortenAddress(RequestAddress.text);
    }
}

