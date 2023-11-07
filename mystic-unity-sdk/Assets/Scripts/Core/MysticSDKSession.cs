using System.Collections.Generic;
using Samples.SwapInGameSample.Scripts;

namespace Core
{
    public class MysticSDKSession
    {
        public string WalletAddress { get; set; }
        public string AuthenticationToken { get; private set; }
        public int ChainId { get; private set; }
        public List<NFT> SelectedNFTs { get; set; }
        public NftItem NftItem { get; set; }
        public List<NftItem> NftItemOffer { get; } = new List<NftItem>();
        public List<NftItem> NftItemRequest { get; } = new List<NftItem>();
        public List<SwapItem> SelectedOffers { get; set; } = new List<SwapItem>();
        public List<SwapItem> SelectedConsiderations { get; set; } = new List<SwapItem>();
        public double EthBalance { get; set; }
        public double WethBalance { get; set; }
        public string OfferAddress { get; set; }
        public string RequestAddress { get; set; }
        public bool IsWalletConnected { get; set; }

        public MysticSDKSession(string _walletAddress, string _authenticationToken, int _chainId)
        {
            WalletAddress = _walletAddress;
            AuthenticationToken = _authenticationToken;
            ChainId = _chainId;
        }
    }
}