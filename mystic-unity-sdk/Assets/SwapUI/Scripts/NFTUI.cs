using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTUI : MonoBehaviour
{
    public GameObject coinsPanel, NFTCollectionPanel;
    public GameObject nftImageToSpawn;
     //public Sprite nftImage;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i <5; i++)
        {
        GameObject newImage=Instantiate(nftImageToSpawn, transform.position, transform.rotation) as GameObject;
        newImage.transform.SetParent(NFTCollectionPanel.transform);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoinsButtonClick()
    {
        coinsPanel.SetActive(true);
        NFTCollectionPanel.SetActive(false);

    }

    public void NFTButtonClick()
    {
        coinsPanel.SetActive(false);
        NFTCollectionPanel.SetActive(true);
    }
}
