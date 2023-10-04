using UnityEngine;

public class OwnedNFTButton : MonoBehaviour
{
    public string Title { get; private set; }
    public string ItemType { get; private set; }
    public string Token { get; private set; }
    public int Identifier { get; private set; }
    public int Balance { get; private set; }

    public void Init(string title, string itemType, string token, int identifier, int balance)
    {
        Title = title;
        ItemType = itemType;
        Token = token;
        Identifier = identifier;
        Balance = balance;
    }

    public void DebugOnClick()
    {
        Debug.Log($"Title: {Title}\n" +
            $"ItemType: {ItemType}\n" +
            $"Token: {Token}\n" +
            $"Identifier: {Identifier}\n" +
            $"Balance: {Balance}");
    }


}
