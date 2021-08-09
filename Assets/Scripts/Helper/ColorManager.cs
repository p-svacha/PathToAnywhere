using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{

    public static Color GetRandomGreenishColor()
    {
        return new Color(Random.value * 0.5f, 0.3f + Random.value * 0.7f, 0.3f + Random.value * 0.7f);
    }

    public static Color GetRandomRedishColor()
    {
        return new Color(0.3f + Random.value * 0.7f, Random.value * 0.5f, Random.value * 0.5f);
    }
}
