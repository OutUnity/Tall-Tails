using UnityEngine;

public class RegionMapManager : MonoBehaviour
{
    public static RegionMapManager Instance;

    public Texture2D regionMap;

    public Vector2 worldMin = new Vector2(0, 0);
    public Vector2 worldMax = new Vector2(2000, 1000);

    private void Awake()
    {
        Instance = this;
    }

    public int GetRegionFromPosition(Vector3 worldPos)
    {
        float u = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x); // 0–1
        float v = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.z); // 0–1
        int x = Mathf.Clamp(Mathf.FloorToInt(u * regionMap.width), 0, regionMap.width - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt(v * regionMap.height), 0, regionMap.height - 1);

        Color color = regionMap.GetPixel(x, y);

        return ColorToRegion(color);
    }

    int ColorToRegion(Color color)
    {
        // Convert to 0–255 range
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);

        // Debug (optional)
        // Debug.Log($"RGB: {r}, {g}, {b}");

        if (IsColor(r, g, b, 255, 0, 0)) return 1;       // Red
        if (IsColor(r, g, b, 222, 0, 255)) return 2;     // Magenta
        if (IsColor(r, g, b, 0, 18, 255)) return 3;       // Blue
        if (IsColor(r, g, b, 0, 252, 255)) return 4;     // Cyan
        if (IsColor(r, g, b, 24, 255, 0)) return 5;       // Green
        if (IsColor(r, g, b, 245, 255, 0)) return 6;     // Yellow
        if (IsColor(r, g, b, 255, 156, 0)) return 7;     // Orange

        return -1;
    }
    bool IsColor(int r, int g, int b, int tr, int tg, int tb, int tolerance = 10)
    {
        return Mathf.Abs(r - tr) <= tolerance &&
               Mathf.Abs(g - tg) <= tolerance &&
               Mathf.Abs(b - tb) <= tolerance;
    }
}