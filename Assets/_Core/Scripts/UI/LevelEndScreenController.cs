using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndScreenController : WidgetController
{
    public override void Open()
    {
        base.Open();

        gameObject.SetActive(true);
    }
}
