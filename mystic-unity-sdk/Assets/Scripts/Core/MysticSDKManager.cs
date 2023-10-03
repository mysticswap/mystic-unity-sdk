using UnityEngine;

namespace Core
{
    public class MysticSDKManager : MonoBehaviour
    {
        public MysticSDK sdk;

        public static MysticSDKManager Instance { get; private set; }

        [SerializeField] private string walletAddress;
        [SerializeField] private string authenticationToken;
        [SerializeField] private string chainId;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            Initialize();


        }

        private void Initialize()
        {
            Debug.Log("sdk init");
            sdk = new MysticSDK(walletAddress, authenticationToken, chainId);
        }
    }
}