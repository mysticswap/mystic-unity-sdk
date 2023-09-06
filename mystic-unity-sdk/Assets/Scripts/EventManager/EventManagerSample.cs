using Core;
using UnityEngine;

public class EventManagerSample : MonoBehaviour
{
    public void TestEvent()
    {
        Debug.Log("Test Event is Raised.");
        MysticSDK sdk = MysticSDKManager.Instance.sdk;
        sdk.GetBalance();
        // sdk.GetNfts();
        // Debugger.Instance.Log("Testing debugger", "This is debugger's description");
    }
}
