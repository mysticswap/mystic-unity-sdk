using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Wallet : MonoBehaviour
{
    [SerializeField]
    private StringVariable walletAddress;
    [SerializeField]
    private StringVariable authenticationToken;
    [SerializeField]
    private string chainId = "5";
    [SerializeField]
    private string endpoint = "https://mystic-swap.herokuapp.com/marketplace-api/get-balance";


    public void GetBalance()
    {
        StartCoroutine(GetRequest($"{endpoint}?address={walletAddress.Value}&chainId={chainId}"));
    }

    private IEnumerator GetRequest(string uri)
    {
        Debug.Log($"Get Request: {uri}");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            AttachHeader(webRequest, "Authorization", authenticationToken.Value);
            yield return webRequest.SendWebRequest();

            Debug.Log($"Response code: {webRequest.responseCode}");
            Debug.Log($"Result status: {webRequest.result}");
            Debug.Log($"Response text: {webRequest.downloadHandler.text}");
        }
    }

    private void AttachHeader(UnityWebRequest webRequest, string key, string value)
    {
        webRequest.SetRequestHeader(key, value);
    }

}
