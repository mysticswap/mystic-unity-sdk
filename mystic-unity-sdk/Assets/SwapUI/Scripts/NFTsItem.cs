using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTsItem : MonoBehaviour
{
    public string Title { get; private set; }
    public string UrlImage { get; private set; }
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image image;

    public void Init(string title, string urlImage)
    {
        Title = title;
        UrlImage = urlImage;
        titleText.text = Title;

        LoadImage();
    }

    private async void LoadImage()
    {
        var _texture = await GetRemoteTexture.GetTextures(UrlImage);

        Sprite newSprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = newSprite;

    }
}
