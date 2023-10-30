using Core;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private MysticSDK sdk;

    public GameObject OfferCollectionsPanel;
    public GameObject RequestCollectionsPanel;
    public GameObject SwapInfoPanel;
    public GameObject player1Panel;
    [SerializeField] private TextMeshProUGUI OfferAddress;
    [SerializeField] private TextMeshProUGUI RequestAddress;
    [SerializeField] private TMP_InputField RequesterAddress;
    [SerializeField] private TMP_InputField RequesterTokenInput;
    [SerializeField] private TMP_InputField OfferTokenInput;

    [SerializeField] private Button GetNFTsOfferButton;
    [SerializeField] private Button GetNFTsRequestButton;

    public OwnedNFTButton ownedNFTButton;

    private List<NFT> listNFTs;

    [SerializeField] private SwapsPanel swapsPanel;
    [SerializeField] private GameObject mySwapsPanelParent;
    [SerializeField] private GameObject allSwapsPanelParent;

    private void Awake()
    {
        sdk = MysticSDKManager.Instance.sdk;
    }

    private void Start()
    {
        OfferAddress.text = ShortenAddress(sdk.GetAddress());
        sdk.session.OfferAddress = sdk.GetAddress();

        GetNFTsOfferButton.onClick.AddListener(delegate
        {
            GetLoadNFTsCollection(sdk.session.OfferAddress, OfferCollectionsPanel);
        });
        GetNFTsRequestButton.onClick.AddListener(delegate
        {
            GetLoadNFTsCollection(sdk.session.RequestAddress, RequestCollectionsPanel);
        });
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
            newOwnedNFTButton.Init(item.title, item.tokenType, item.contract.address, item.tokenId, item.balance,
                item.rawMetadata.image, _ownerAddress);

            newOwnedNFTButton.transform.SetParent(parentPanel.transform);
        }

        Debug.Log("LoadNFTsCollection excecuted");
    }

    public void LoadSelectedNFTs(string _ownerAddress, OwnedNFTButton _ownedNFTButton)
    {
        SwapInfoPanel.SetActive(true);

        var newOwnedButton = Instantiate(_ownedNFTButton, transform.position, transform.rotation);
        newOwnedButton.transform.localScale = _ownedNFTButton.transform.localScale / 5;
        newOwnedButton.transform.SetParent(player1Panel.transform);

        Debug.Log("LoadSelectedNFTs excecuted");
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
    public async void AllSwaps()
    {
        var result = await sdk.RetrieveAllSwaps();
        Debug.Log($"My Swaps: {result}");
        AllSwapsData allSwapsData = JsonUtility.FromJson<AllSwapsData>(result);
        foreach (var swap in allSwapsData.data)
        {
            Debug.Log($"_id: {swap._id}");
        }

        Debug.Log($"Total: {allSwapsData.data.Count}");
        Debug.Log($"Total Items: {allSwapsData.metadata.totalItems}");

        foreach (var swap in allSwapsData.data)
        {
            var creatorAddress = swap.creatorAddress;
            var takerAddress = swap.takerAddress;
            var swapId = swap._id;
            var nftsOffer = GetNFTsMetadata(swap.orderComponents.offer, swap.metadata.nftsMetadata);
            var nftsConsideration = GetNFTsMetadata(swap.orderComponents.consideration, swap.metadata.nftsMetadata);
            var swapStatus = swap.status;

            var newSwapPanel = Instantiate(swapsPanel, transform.position, transform.rotation);
            newSwapPanel.Init(creatorAddress, takerAddress, swapId, nftsOffer, nftsConsideration, swapStatus);
            newSwapPanel.transform.SetParent(allSwapsPanelParent.transform);
        }
    }

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

        foreach (var swap in allSwapsData.data)
        {
            var creatorAddress = swap.creatorAddress;
            var takerAddress = swap.takerAddress;
            var swapId = swap._id;
            var nftsOffer = GetNFTsMetadata(swap.orderComponents.offer, swap.metadata.nftsMetadata);
            var nftsConsideration = GetNFTsMetadata(swap.orderComponents.consideration, swap.metadata.nftsMetadata);
            var swapStatus = swap.status;

            var newSwapPanel = Instantiate(swapsPanel, transform.position, transform.rotation);
            newSwapPanel.Init(creatorAddress, takerAddress, swapId, nftsOffer, nftsConsideration, swapStatus);
            newSwapPanel.transform.SetParent(mySwapsPanelParent.transform);
        }
    }

    private List<NFT> GetNFTsMetadata(List<ValueComponents> listItem, List<NFT> listMetadata)
    {
        List<NFT> listNFT = new List<NFT>();

        //List<NFT> listMetadataTemp = listMetadata;
        foreach (var item in listItem)
        {
            if (item.token == "0x0000000000000000000000000000000000000000")
            {
                var tokenNFT = GenerateTokenNFT(item.startAmount);
                listNFT.Add(tokenNFT);
                Debug.Log("token detected, skip to the next one");
                continue;
            }

            foreach (var metadata in listMetadata)
            {
                if (item.token == metadata.contract.address && item.identifierOrCriteria == metadata.tokenId.ToString())
                {
                    listNFT.Add(metadata);
                    //listMetadataTemp.Remove(metadata);
                }
            }
        }

        return listNFT;
    }

    private NFT GenerateTokenNFT(string amount)
    {
        var amountEth = WeiToEth(amount);
        var titleNFT = $"{amountEth} ETH";

        NFTMetadata nftMetadata = new NFTMetadata()
        {
            image =
                "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6f/Ethereum-icon-purple.svg/768px-Ethereum-icon-purple.svg.png",
        };
        NFT tokenNFT = new NFT()
        {
            title = titleNFT,
            rawMetadata = nftMetadata,
        };

        return tokenNFT;
    }

    private double WeiToEth(string wei, int decimals = 18)
    {
        if (!BigInteger.TryParse(wei, out BigInteger weiBigInt))
            throw new ArgumentException("Invalid wei value.");
        double eth = (double)weiBigInt / Math.Pow(10.0, decimals);
        return eth;
    }

    private string EthToWei(string eth, int decimals = 18)
    {
        if (!Double.TryParse(eth, out Double ethDouble))
            throw new ArgumentException("Invalid eth value.");
        BigInteger wei = (BigInteger)(ethDouble * Math.Pow(10.0, decimals));
        return wei.ToString();
    }

    #endregion

    public void AddTokenConsiderations()
    {
        var inputWei = RequesterTokenInput.text;
        var wei = EthToWei(inputWei);
        AddTokenToList(wei, sdk.session.SelectedConsiderations);
        Debug.Log("===========Consideration Items===========");
        ShowSwapItems(sdk.session.SelectedConsiderations);
    }

    public async void AddTokenOffer()
    {
        var inputWei = OfferTokenInput.text;
        var wei = EthToWei(inputWei);
        if (await IsBalanceSufficient(wei))
        {
            AddTokenToList(wei, sdk.session.SelectedOffers);
            Debug.Log("===========Offer Items===========");
            ShowSwapItems(sdk.session.SelectedOffers);
        }
        else
        {
            Debug.Log("Offer error: Insufficient Weth balance");
        }
    }

    private async Task<bool> IsBalanceSufficient(string inputBalance)
    {
        var balance = await sdk.GetBalance();
        var _inputBalance = WeiToEth(inputBalance);
        BalanceData _balanceData = JsonUtility.FromJson<BalanceData>(balance);
        if (!Double.TryParse(_balanceData.WETH, out Double _wethBalance))
            throw new ArgumentException("Invalid Weth balance value.");

        Debug.Log($"wethBalance: {_wethBalance}\ninputBalance: {_inputBalance}");

        bool output = (_wethBalance >= _inputBalance);
        return output;
    }

    private void AddTokenToList(string amount, List<SwapItem> swapItems)
    {
        // create token type
        var tokenItem = new SwapItem()
        {
            itemtype = "NATIVE",
            token = "0x0000000000000000000000000000000000000000",
            identifier = "0",
            amount = amount,
        };

        // checking for existing token, if so overwrite.
        int index = swapItems.FindIndex(s => s.itemtype == "NATIVE");
        if (index != -1)
            swapItems[index] = tokenItem;
        else
            swapItems.Add(tokenItem);
    }

    private void ShowSwapItems(List<SwapItem> swapItems)
    {
        foreach (var item in swapItems)
        {
            Debug.Log(item.ToString());
        }
    }

    public async void ShowBalance()
    {
        var balance = await sdk.GetBalance();
        Debug.Log($"{balance}");
        BalanceData _balanceData = JsonUtility.FromJson<BalanceData>(balance);
        Debug.Log($"ETH:{_balanceData.ETH}");
        Debug.Log($"WETH:{_balanceData.WETH}");
    }
}