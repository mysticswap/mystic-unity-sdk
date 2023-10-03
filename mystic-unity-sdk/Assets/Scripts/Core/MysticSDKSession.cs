namespace Core
{
    public class MysticSDKSession
    {
        public string walletAddress { get; set; }
        public string authenticationToken { get; private set; }
        public string chainId { get; private set; }

        public MysticSDKSession(string _walletAddress, string _authenticationToken, string _chainId)
        {
            walletAddress = _walletAddress;
            authenticationToken = _authenticationToken;
            chainId = _chainId;
        }
    }
}