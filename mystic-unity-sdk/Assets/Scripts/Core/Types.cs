#nullable enable
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
    public struct NFT
    {
        public object contract;
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
    }

    [System.Serializable]
    public struct Consideration
    {
        public string itemtype;
        public string token;
        public string identifier;
        public string amount;
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
    public struct SwapData
    {
        public string signature;
        public string swapId;
        public string takerAddress;
    }

}