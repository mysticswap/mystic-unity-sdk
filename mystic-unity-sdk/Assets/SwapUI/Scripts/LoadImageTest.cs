using UnityEngine;
using UnityEngine.UI;

public class LoadImageTest : MonoBehaviour
{
    [SerializeField] private string _imageUrl;
    [SerializeField] private Image _image;
    async void Start()
    {
        Texture2D _texture;
        //_texture = await GetRemoteTextures(_imageUrl);
        _texture = await GetRemoteTexture.GetTextures(_imageUrl);
        //_material.mainTexture = _texture;

        Sprite newSprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        _image.sprite = newSprite;
    }
}
