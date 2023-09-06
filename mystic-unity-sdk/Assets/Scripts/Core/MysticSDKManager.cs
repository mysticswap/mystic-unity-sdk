using System;
using UnityEngine;

namespace Core
{
    public class MysticSDKManager : MonoBehaviour
    {
        public MysticSDK sdk;
        
        public static MysticSDKManager Instance { get; private set; }

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
            
        }
    }
}