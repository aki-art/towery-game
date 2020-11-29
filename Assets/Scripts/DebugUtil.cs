using UnityEngine;

public class DebugUtil : MonoBehaviour
{
    public static TextMesh DrawText(Vector3 location, string message)
    {
        return DrawText(location, message, Color.white);
    }

    public static TextMesh DrawText(Vector3 location, string message, Color color)
    {
        GameObject go = new GameObject("text", typeof(TextMesh));
        go.transform.position = location;
        var text = go.GetComponent<TextMesh>();
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.color = color;
        text.fontSize = 42;
        text.text = message;
        text.characterSize = 0.07f;

        return text;
    }
}
