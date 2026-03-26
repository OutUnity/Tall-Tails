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
        int x = Mathf.FloorToInt(u * regionMap.width);
        int y = Mathf.FloorToInt(v * regionMap.height);

        Color color = regionMap.GetPixel(x, y);

        return ColorToRegion(color);
    }

    int ColorToRegion(Color color)
    {
        if (Vector4.Distance(color, Color.red) < 0.1f) return 1;
        if (Vector4.Distance(color, Color.magenta) < 0.1f) return 2;
        if (Vector4.Distance(color, Color.blue) < 0.1f) return 3;
        if (Vector4.Distance(color, Color.cyan) < 0.1f) return 4;
        if (Vector4.Distance(color, Color.green) < 0.1f) return 5;
        if (Vector4.Distance(color, Color.yellow) < 0.1f) return 6;
        if (Vector4.Distance(color, Color.orange) < 0.1f) return 7;

        return -1; // unknown
    }
}