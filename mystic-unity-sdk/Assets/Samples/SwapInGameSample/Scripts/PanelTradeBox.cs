using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Samples.SwapInGameSample.Scripts.Interfaces;
using UnityEngine;

namespace Samples.SwapInGameSample.Scripts
{
    public class PanelTradeBox : MonoBehaviour, ITradeBox
    {
        private MysticSDK _sdk;
        [SerializeField] private bool isOffer;
        [SerializeField] private GameObject parentPanel;
        [SerializeField] private TradeItem tradeItem;
        private List<NftItem> _nftItemsOffer = new List<NftItem>();
        private List<NftItem> _nftItemsRequest = new List<NftItem>();


        public void RefreshItems()
        {
            var sdk = MysticSDKManager.Instance.sdk;
            var nftItem = sdk.session.NftItem;
            Debug.Log("RefreshItems");

            if (isOffer)
            {
                bool b = _nftItemsOffer.Contains(nftItem);
                if (b)
                {
                    // remove existed item
                    Debug.Log($"removing {nftItem}");
                    _nftItemsOffer.Remove(nftItem);
                    RemoveTradeItem(nftItem, parentPanel);
                }
                else
                {
                    Debug.Log($"adding {nftItem}");
                    _nftItemsOffer.Add(nftItem);
                    GenerateTradeItem(nftItem, tradeItem, parentPanel);
                }
            }
        }

        public void RefreshToken()
        {
            throw new System.NotImplementedException();
        }

        public void LockTrade()
        {
            throw new System.NotImplementedException();
        }

        private void GenerateTradeItem(NftItem nftItem, TradeItem _tradeItem, GameObject _parentPanel)
        {
            // _tradeItem.Guid = nftItem.Guid;
            _tradeItem.Title = nftItem.Title;
            Debug.Log($"NftItem generated: {nftItem}");
            var newItem = Instantiate(_tradeItem, transform.position, transform.rotation);
            newItem.Init(nftItem.Title, nftItem.ImageUrl, nftItem.Guid);

            newItem.transform.SetParent(_parentPanel.transform);
        }

        public void TestPrintAllItemsGuid()
        {
            foreach (Transform item in parentPanel.transform)
            {
                var itemData = item.GetComponent<TradeItem>();
                Debug.Log($"itemData: {itemData.Guid.ToString()}");
            }
        }

        private void RemoveTradeItem(NftItem nftItem, GameObject _parentPanel)
        {
            var items = _parentPanel.GetComponentsInChildren<TradeItem>();

            var index = GetIndexByGuid(items, nftItem.Guid);

            Destroy(_parentPanel.transform.GetChild(index).gameObject);
        }

        private int GetIndexByGuid(TradeItem[] items, Guid guid)
        {
            int index = 0;
            foreach (var item in items)
            {
                if (item.Guid == guid)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

    }
}