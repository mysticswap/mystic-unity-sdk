using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.SwapInGameSample.Scripts
{
    public class TradeItem : MonoBehaviour
    {
        public Guid Guid { get; private set; }
        private string _title;
        private string _imageUrl;
        public string Title { get; set; }
        [SerializeField] private TMP_Text textTitle;
        [SerializeField] private Image image;



        public void Init(string title, string imageUrl, Guid guid)
        {
            _title = title;
            _imageUrl = imageUrl;
            Guid = guid;

            LoadImage();
            LoadText();
        }



        private async void LoadImage()
        {
            var _texture = await GetRemoteTexture.GetTextures(_imageUrl);

            Sprite newSprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height),
                new Vector2(0.5f, 0.5f));
            image.sprite = newSprite;
        }

        private void LoadText()
        {
            textTitle.text = _title;
        }
    }
}