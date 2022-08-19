using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : Score
{

    protected override void DisableSelf()
    {
        UiManager.instance.CoinAdd();
        gameObject.SetActive(false);
    }

}
