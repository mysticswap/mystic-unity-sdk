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

        var offer = new SwapItem()
        {
            itemtype = ItemType,
            token = Token,
            identifier = Identifier.ToString(),
            amount = Balance.ToString(),
        };

        if (isSelected) AddNFT(offer);
        else RemoveExistingNFT(offer);

    }

    private void AddNFT(SwapItem _offer)
    {
        var sdk = MysticSDKManager.Instance.sdk;
        var connectedAddress = sdk.GetAddress();

        // Only add item if item didn't exist on the list.
        var alreadyExist = sdk.session.SelectedOffers.Contains(_offer);
        if (!alreadyExist)
        {
            if (OwnerAddress == connectedAddress)
            {
                sdk.session.SelectedOffers.Add(_offer);
            }
            else
            {
                sdk.session.SelectedConsiderations.Add(
                    new SwapItem()
                    {
                        itemtype = ItemType,
                        token = Token,
                        identifier = Identifier.ToString(),
                        amount = Balance.ToString(),
                    });
            }
        }
    }

    private void RemoveExistingNFT(SwapItem _offer)
    {
        var sdk = MysticSDKManager.Instance.sdk;
        var connectedAddress = sdk.GetAddress();

        // Only add item if item didn't exist on the list.
        var alreadyExist = sdk.session.SelectedOffers.Contains(_offer);

        if (alreadyExist) sdk.session.SelectedOffers.Remove(_offer);

    }


}
