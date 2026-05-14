using UnityEngine;

[CreateAssetMenu(menuName = "UI/Text Style")]
public class UITextStyle : ScriptableObject
{
    [Header("Colors")]
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color activeColor = Color.green;

    [Header("Scale")]
    [Range(0f, 2.0f)]
    public float hoverScale = 1.05f;

    [Header("Glow")]
    public Color glowColor = Color.yellow;

    [Range(0f, 3.0f)]
    public float glowIntensity = 1f;
}