using System.Collections.Generic;

namespace Core
{
    public class MysticSDKSession
    {
        public string WalletAddress { get; set; }
        public string AuthenticationToken { get; private set; }
        public string ChainId { get; private set; }
        public List<NFT> SelectedNFTs { get; set; }

        public MysticSDKSession(string _walletAddress, string _authenticationToken, string _chainId)
        {
            WalletAddress = _walletAddress;
            AuthenticationToken = _authenticationToken;
            ChainId = _chainId;
        }
    }
}