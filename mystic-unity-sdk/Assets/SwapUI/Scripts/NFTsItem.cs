using TMPro;
using UnityEngine;

public class NFTsItem : MonoBehaviour
{
    public string Title { get; private set; }
    public string UrlImage { get; private set; }
    [SerializeField] private TextMeshProUGUI titleText;

    public void Init(string title, string urlImage)
    {
        Title = title;
        UrlImage = urlImage;
        titleText.text = Title;
    }
}
