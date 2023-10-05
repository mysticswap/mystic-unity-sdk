#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core
{

    [System.Serializable]
    public struct NFTMetadata
    {
        public long date;
        public string image;
        public string external_url;
        public string name;
        public string description;
        public int edition;
        public object attributes;
    }

    [System.Serializable]
    public struct NFTContract
    {
        public string address;
        public string name;
        public string symbol;
        public string totalSupply;
        public string tokenType;
    }

    [System.Serializable]
    public struct NFT
    {
        public NFTContract contract;
        public int tokenId;
        public string tokenType;
        public string title;
        public string description;
        public string timeLastUpdated;
        public NFTMetadata rawMetadata;
        public object tokenUri;
        public object media;
        public int balance;
    }

    [System.Serializable]
    public struct OwnedNFT
    {
        public List<NFT> ownedNfts;
        public int totalCount;
        public string blockHash;
    }

    [System.Serializable]
    public struct Fees
    {
        public string recipient;
        public int basisPoints;
    }

    [System.Serializable]
    public struct Offer
    {
        public string itemtype;
        public string token;
        public string identifier;
        public string amount;

        public override string ToString()
        {
            return $"itemtype: {itemtype}\n" +
                $"token: {token}\n" +
                $"identifier: {identifier}\n" +
                $"amount: {amount}";
        }
    }

    [System.Serializable]
    public struct Consideration
    {
        public string itemtype;
        public string token;
        public string identifier;
        public string amount;

        public override string ToString()
        {
            return $"itemtype: {itemtype}\n" +
                $"token: {token}\n" +
                $"identifier: {identifier}\n" +
                $"amount: {amount}";
        }
    }

    [System.Serializable]
    public struct CreateSwap
    {
        public int? endTime;
        public int chainId;
        public string offerer;
        public string creatorAddress;
        public string contractAddress;
        public string? takerAddress;
        public List<Fees>? fees;
        public List<Offer> offer;
        public List<Consideration> consideration;
    }

    [System.Serializable]
    public struct DomainData
    {
        public string name;
        public string version;
        public int chainId;
        public string verifyingContract;
    }

    [System.Serializable]
    public struct TypesComponents
    {
        public string name;
        public string type;
    }

    [System.Serializable]
    public struct ValueComponents
    {
        public int itemType;
        public string token;
        public string identifierOrCriteria;
        public string startAmount;
        public string endAmount;
        public string recipient;
    }

    [System.Serializable]
    public struct Value
    {
        public string offerer;
        public string zone;
        public string zoneHash;
        public string startTime;
        public string endTime;
        public int orderType;
        public List<ValueComponents> offer;
        public List<ValueComponents> consideration;
        public int totalOriginalConsiderationItems;
        public string salt;
        public string conduitKey;
        public int counter;
    }


    [System.Serializable]
    public struct TransactionData
    {
        public string data;
        public string to;
        public TransactionValue value;

        public static TransactionData[]? DeserializedJson(string responseJson)
        {
            return JsonConvert.DeserializeObject<TransactionData[]>(responseJson);
        }
    }

    [System.Serializable]
    public struct TransactionValue
    {
        public string type;
        public string hex;
    }

    [JsonArray]
    public class ListTransactionData
    {
        public List<TransactionData> listTransactionData;
    }

    [System.Serializable]
    public struct SignTypedMessage
    {
        public DomainData domainData;
        public SignatureTypes types;
        public Value value;
    }

    [System.Serializable]
    public class SwapResponse
    {
        public SignTypedMessage signTypedMessage;
        public List<TransactionData> approvalsNeeded;
        public string swapId;
    }

    [System.Serializable]
    public struct SwapData
    {
        public string signature;
        public string swapId;
        public string takerAddress;
    }

    [System.Serializable]
    public struct SignatureTypes
    {
        public List<TypesComponents> OrderComponents;
        public List<TypesComponents> OfferItem;
        public List<TypesComponents> ConsiderationItem;
        public List<TypesComponents> EIP712Domain;
    }

    [System.Serializable]
    public struct SignatureData
    {
        public DomainData domain;
        public SignatureTypes types;
        public Value message;
        public string primaryType;
        public string swapId;
    }

    [System.Serializable]
    public struct MetaMaskData
    {
        public string id;
        public string jsonrpc;
        public string result;
    }

    [System.Serializable]
    public class MetaMaskResponse
    {
        public string name;
        public MetaMaskData data;
    }

    [System.Serializable]
    public struct MetadataArray
    {
        public string type;
        public string contractAddress;
        public string tokenId;
    }

    [System.Serializable]
    public struct Metadata
    {
        public int chainId;
        public List<MetadataArray> metadataArray;
    }

}