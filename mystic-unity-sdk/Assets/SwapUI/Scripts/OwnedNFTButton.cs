using Core;
using UnityEngine;

public class OwnedNFTButton : MonoBehaviour
{
    public string Title { get; private set; }
    public string ItemType { get; private set; }
    public string Token { get; private set; }
    public int Identifier { get; private set; }
    public int Balance { get; private set; }
    public string OwnerAddress { get; private set; }
    private bool isSelected;
    [SerializeField] private GameObject selectedBackground;

    public void Init(string title, string itemType, string token, int identifier, int balance, string ownerAddress)
    {
        Title = title;
        ItemType = itemType;
        Token = token;
        Identifier = identifier;
        Balance = balance;
        OwnerAddress = ownerAddress;
        isSelected = false;
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

        if (isSelected) AddSwapItem(swapItem);
        else RemoveSwapItem(swapItem);

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
        }
        else
        {
            var alreadyExist = sdk.session.SelectedConsiderations.Contains(_swapItem);
            if (!alreadyExist)
                sdk.session.SelectedConsiderations.Add(_swapItem);
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
        }
        else
        {
            var alreadyExist = sdk.session.SelectedConsiderations.Contains(_swapItem);
            if (alreadyExist)
                sdk.session.SelectedConsiderations.Remove(_swapItem);
        }
    }

    private bool IsOfferItem(string _ownerAddress)
    {
        var connectedAddress = MysticSDKManager.Instance.sdk.GetAddress();
        return connectedAddress == _ownerAddress;
    }


}


