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
}