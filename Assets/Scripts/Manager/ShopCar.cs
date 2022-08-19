using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCar : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private int _id;
    [SerializeField] private GameObject _priceParent;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private GameObject[] _hideImages;


    void Start()
    {
        Shop.instance.ShowIngo.AddListener(UpdateInfo);
        GetComponent<Button>().onClick.AddListener(Buy);
    }

    private void UpdateInfo()
    {
        if (_id == 0) return;
        if (Game.instance.CountOpenCar >= _id)
        {
            _priceParent.SetActive(true);
            foreach (var item in _hideImages)
            {
                item.SetActive(false);
            }
        }
        if (CheckedItem())
        {
            _priceParent.SetActive(false);
            foreach (var item in _hideImages)
            {
                item.SetActive(false);
            }
        }
    }

    private void Buy()
    {
        if (_id != 0 && Game.instance.CountOpenCar >= _id)
        {
            if (CheckedItem())
            {
                _priceParent.SetActive(false);
            }
            else
            {
                if (Game.instance.CountGem >= _price)
                {
                    Game.instance.CountGem -= _price;
                    _priceParent.SetActive(false);
                    Game.instance.SetShopItem(_id);
                }
            }
        }
    }

    private bool CheckedItem()
    {
        bool check = false;
        foreach (var item in Game.instance.ShopItems)
        {
            if (item == _id)
            {
                check = true;
                break;
            }
        }
        return check;
    }
}
