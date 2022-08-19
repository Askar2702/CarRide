using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Score
{
    protected override void DisableSelf()
    {
        Game.instance.CountGem++;
        gameObject.SetActive(false);
    }
}
