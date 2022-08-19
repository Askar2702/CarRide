using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopCar : MonoBehaviour
{
    [SerializeField] private float _price;
    [SerializeField] private float _id;
    [SerializeField] private GameObject _priceParent;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private GameObject[] _hideImages;
    private Game _game;

    void Start()
    {
        _game = FindObjectOfType<Game>();
        Shop.instance.ShowIngo.AddListener(UpdateInfo);
    }

    private void UpdateInfo()
    {
        if (_id == 0) return;
        if (_game.CountOpenCar >= _id)
        {
            _priceParent.SetActive(true);
            foreach (var item in _hideImages)
            {
                item.SetActive(false);
            }
        }
    }


}
