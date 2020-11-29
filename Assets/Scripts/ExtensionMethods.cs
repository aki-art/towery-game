
using UnityEngine;


public static class ExtensionMethods
{
    public static Vector3 Transpose(this Vector3 original)
    {
        return Game.Instance?.Tower ? Game.Instance.Tower.TranslatePosition(original) : original;
    }
}
