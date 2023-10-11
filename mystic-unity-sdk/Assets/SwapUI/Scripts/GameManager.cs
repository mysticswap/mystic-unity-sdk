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

    [SerializeField] private SwapsPanel swapsPanel;

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


    #region Test GetNFTsCollection
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
    #endregion

    #region Get and Load NFTsCollection
    public async void GetLoadNFTsCollection(string _ownerAddress, GameObject _parentPanel)
    {
        Debug.Log("GetLoadNFTsCollection");
        Debug.Log($"address: {_ownerAddress}");
        List<NFT> list = new List<NFT>();

        list = await sdk.GetOwnedNFTs(_ownerAddress);
        Debug.Log($"list count: {list.Count}");

        LoadNFTsCollection(_ownerAddress, list, _parentPanel);

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
    #endregion

    #region Retrieve Offer and Request Test
    public void RetrieveOfffers()
    {
        var selectedOffers = sdk.session.SelectedOffers;
        Debug.Log($"Selected Offers: {selectedOffers.Count}");
        foreach (var item in selectedOffers)
        {
            Debug.Log($"{item}");
        }
    }

    public void RetrieveRequest()
    {
        var selectedConsiderations = sdk.session.SelectedConsiderations;
        Debug.Log($"Selected Offers: {selectedConsiderations.Count}");
        foreach (var item in selectedConsiderations)
        {
            Debug.Log($"{item}");
        }
    }
    #endregion

    #region Address Handling
    public void SetRequestAddress()
    {
        sdk.session.RequestAddress = RequesterAddress.text;
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
    #endregion

    #region Create Swap
    public async void CreateSwap()
    {
        var swap = new CreateSwap()
        {
            chainId = sdk.session.ChainId,
            offerer = sdk.session.OfferAddress,
            creatorAddress = sdk.session.OfferAddress,
            contractAddress = sdk.ContractAddress,
            offer = sdk.session.SelectedOffers,
            consideration = sdk.session.SelectedConsiderations,
            takerAddress = sdk.session.RequestAddress,
        };

        var result = await sdk.CreateSwap(swap);
        Debug.Log($"Created Swap: {result}");

    }
    #endregion

    #region My Swaps
    public async void MySwaps()
    {
        var result = await sdk.RetrieveMySwaps();
        Debug.Log($"My Swaps: {result}");
        AllSwapsData allSwapsData = JsonUtility.FromJson<AllSwapsData>(result);
        foreach (var swap in allSwapsData.data)
        {
            Debug.Log($"_id: {swap._id}");
        }
        Debug.Log($"Total: {allSwapsData.data.Count}");
        Debug.Log($"Total Items: {allSwapsData.metadata.totalItems}");

        var creatorAddress = allSwapsData.data[0].creatorAddress;
        var takerAddress = allSwapsData.data[0].takerAddress;
        var swapId = allSwapsData.data[0]._id;
        var countOffer = allSwapsData.data[0].orderComponents.offer.Count;
        var countConsideration = allSwapsData.data[0].orderComponents.consideration.Count;
        var nftsOffer = allSwapsData.data[0].metadata.nftsMetadata.GetRange(0, countOffer);
        var nftsConsideration = allSwapsData.data[0].metadata.nftsMetadata.GetRange(countOffer, countConsideration);
        var swapStatus = allSwapsData.data[0].status;

        swapsPanel.Init(creatorAddress, takerAddress, swapId, nftsOffer, nftsConsideration, swapStatus);

    }
    #endregion
}

