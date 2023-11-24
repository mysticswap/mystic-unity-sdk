using System;
using System.Collections.Generic;
using Core;
using Samples.SwapInGameSample.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OwnedNFTButton : MonoBehaviour
{
    public string Title { get; private set; }
    public string ItemType { get; private set; }
    public string Token { get; private set; }
    public int Identifier { get; private set; }
    public int Balance { get; private set; }
    public string OwnerAddress { get; private set; }
    public string ImageUrl { get; private set; }
    private bool isSelected;
    [SerializeField] private GameObject selectedBackground;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text textTitle;

    private NftItem _nftItem;
    [SerializeField] private GameEvent tradeBoxRefreshItemsOffer;
    [SerializeField] private GameEvent tradeBoxRefreshItemsRequest;


    public void Init(string title, string itemType, string token, int identifier, int balance, string imageUrl,
        string ownerAddress)
    {
        Title = title;
        ItemType = itemType;
        Token = token;
        Identifier = identifier;
        Balance = balance;
        ImageUrl = imageUrl;
        OwnerAddress = ownerAddress;
        isSelected = false;

        InitNftItem(title, imageUrl);
        LoadImage();
        LoadText();
    }

    private void InitNftItem(string title, string imageUrl)
    {
        _nftItem = new NftItem()
        {
            Guid = Guid.NewGuid(),
            Title = title,
            ImageUrl = imageUrl,
        };
    }

    private async void LoadImage()
    {
        var _texture = await GetRemoteTexture.GetTextures(ImageUrl);

        Sprite newSprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = newSprite;
    }

    private void LoadText()
    {
        textTitle.text = Title;
    }

    public void DebugOnClick()
    {
        isSelected = !isSelected;
        selectedBackground.SetActive(isSelected);

        Debug.Log($"Title: {Title}\n" +
                  $"ItemType: {ItemType}\n" +
                  $"Token: {Token}\n" +
                  $"Identifier: {Identifier}\n" +
                  $"Balance: {Balance}");

        var swapItem = new SwapItem()
        {
            itemtype = ItemType,
            token = Token,
            identifier = Identifier.ToString(),
            amount = Balance.ToString(),
        };

        AddNftItem(_nftItem);

        if (isSelected)
        {
            AddSwapItem(swapItem);
            // AddNftItem();
        }
        else
        {
            RemoveSwapItem(swapItem);
            // RemoveNftItem();
        }
    }

    private void AddNftItem(NftItem nftItem)
    {
        var sdk = MysticSDKManager.Instance.sdk;
        sdk.session.NftItem = nftItem;
    }

    private void AddSwapItem(SwapItem _swapItem)
    {
        var sdk = MysticSDKManager.Instance.sdk;

        var isOfferItem = IsOfferItem(OwnerAddress);
        if (isOfferItem)
        {
            var alreadyExist = sdk.session.SelectedOffers.Contains(_swapItem);
            if (!alreadyExist)
                sdk.session.SelectedOffers.Add(_swapItem);
            tradeBoxRefreshItemsOffer.Raise();
        }
        else
        {
            var alreadyExist = sdk.session.SelectedConsiderations.Contains(_swapItem);
            if (!alreadyExist)
                sdk.session.SelectedConsiderations.Add(_swapItem);
            tradeBoxRefreshItemsRequest.Raise();
        }
    }

    private void RemoveSwapItem(SwapItem _swapItem)
    {
        var sdk = MysticSDKManager.Instance.sdk;

        var isOfferItem = IsOfferItem(OwnerAddress);
        if (isOfferItem)
        {
            var alreadyExist = sdk.session.SelectedOffers.Contains(_swapItem);
            if (alreadyExist)
                sdk.session.SelectedOffers.Remove(_swapItem);
            tradeBoxRefreshItemsOffer.Raise();
        }
        else
        {
            var alreadyExist = sdk.session.SelectedConsiderations.Contains(_swapItem);
            if (alreadyExist)
                sdk.session.SelectedConsiderations.Remove(_swapItem);
            tradeBoxRefreshItemsRequest.Raise();
        }
    }

    // private void AddNftItem()
    // {
    //     var sdk = MysticSDKManager.Instance.sdk;
    //     var itemOffer = sdk.session.NftItemOffer;
    //     var itemRequest = sdk.session.NftItemRequest;
    //     
    //     var isOfferItem = IsOfferItem(OwnerAddress);
    //     if (isOfferItem)
    //     {
    //         var isAlreadyExist = itemOffer.Contains(_nftItem);
    //         if (!isAlreadyExist)
    //             itemOffer.Add(_nftItem);
    //     }
    //     else
    //     {
    //         var isAlreadyExist = itemRequest.Contains(_nftItem);
    //         if (!isAlreadyExist)
    //             itemRequest.Add(_nftItem);
    //     }
    // }
    //
    // private void RemoveNftItem()
    // {
    //     var sdk = MysticSDKManager.Instance.sdk;
    //     var itemOffer = sdk.session.NftItemOffer;
    //     var itemRequest = sdk.session.NftItemRequest;
    //     
    //     var isOfferItem = IsOfferItem(OwnerAddress);
    //     if (isOfferItem)
    //     {
    //         var isAlreadyExist = itemOffer.Contains(_nftItem);
    //         if (!isAlreadyExist)
    //             itemOffer.Remove(_nftItem);
    //     }
    //     else
    //     {
    //         var isAlreadyExist = itemRequest.Contains(_nftItem);
    //         if (!isAlreadyExist)
    //             itemRequest.Remove(_nftItem);
    //     }
    // }

    private bool IsOfferItem(string _ownerAddress)
    {
        var connectedAddress = MysticSDKManager.Instance.sdk.GetAddress();
        return connectedAddress == _ownerAddress;
    }

    //private void LoadSelectedNFTIntoSwapPanel()
    //{
    //    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

    //    gm.LoadSelectedNFTs(OwnerAddress, this);
    //}
}