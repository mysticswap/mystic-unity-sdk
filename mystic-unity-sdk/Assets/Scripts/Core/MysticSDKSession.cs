using System.Collections.Generic;

namespace Core
{
    public class MysticSDKSession
    {
        public string WalletAddress { get; set; }
        public string AuthenticationToken { get; private set; }
        public int ChainId { get; private set; }
        public List<NFT> SelectedNFTs { get; set; }
        public List<SwapItem> SelectedOffers { get; set; } = new List<SwapItem>();
        public List<SwapItem> SelectedConsiderations { get; set; } = new List<SwapItem>();
        public string OfferAddress { get; set; }
        public string RequestAddress { get; set; }

        public MysticSDKSession(string _walletAddress, string _authenticationToken, int _chainId)
        {
            WalletAddress = _walletAddress;
            AuthenticationToken = _authenticationToken;
            ChainId = _chainId;
        }
    }
}