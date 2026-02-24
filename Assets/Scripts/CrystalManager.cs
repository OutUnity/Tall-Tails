using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class RegionData
{
    public int regionID;
    public int collected;
    public int maxCrystals;
    public bool unlocked;

    public TextMeshProUGUI mapText; // assign in inspector
}

public class CrystalManager : MonoBehaviour
{
    public int currentRegionID = 1; // default starting region

    public static CrystalManager Instance;

    [Header("Regions")]
    public List<RegionData> regions = new List<RegionData>();

    [Header("HUD")]
    public TextMeshProUGUI totalCrystalText;

  

    void Awake()
    {
        Instance = this;
        regions = new List<RegionData>
        {
            new RegionData { regionID = 1, maxCrystals = 6, collected = 0, unlocked = true },
            new RegionData { regionID = 2, maxCrystals = 7, collected = 0, unlocked = false },
            new RegionData { regionID = 3, maxCrystals = 7, collected = 0, unlocked = false },
            new RegionData { regionID = 4, maxCrystals = 7, collected = 0, unlocked = false },
            new RegionData { regionID = 5, maxCrystals = 7, collected = 0, unlocked = false },
            new RegionData { regionID = 6, maxCrystals = 8, collected = 0, unlocked = false },
            new RegionData { regionID = 7, maxCrystals = 8, collected = 0, unlocked = false }
        };
    }

    void Start()
    {
        UpdateAllUI();
    }

    public void SetCurrentRegion(int regionID)
    {
        currentRegionID = regionID;
        UpdateHUD();
    }
    public void AddCrystal(int regionID)
    {
        RegionData region = regions.Find(r => r.regionID == regionID);

        if (region == null || !region.unlocked)
            return;

        if (region.collected >= region.maxCrystals)
            return; // already reached max, ignore

        region.collected++;
        UpdateRegionUI(region);

        // Update HUD only if in this region
        if (currentRegionID == regionID)
            UpdateHUD();
    }

    void UpdateHUD()
    {
        if (totalCrystalText != null)
        {
            RegionData region = regions.Find(r => r.regionID == currentRegionID);

            if (region != null)
                totalCrystalText.text = "Crystals: " + region.collected + " / " + region.maxCrystals;
        }
    }

    void UpdateRegionUI(RegionData region)
    {
        if (region.mapText != null)
            region.mapText.text = region.collected + " / " + region.maxCrystals;
    }

    void UpdateAllUI()
    {
        UpdateHUD();

        foreach (RegionData region in regions)
        {
            UpdateRegionUI(region);
        }
    }

    public void UnlockRegion(int regionID)
    {
        RegionData region = regions.Find(r => r.regionID == regionID);
        if (region != null)
            region.unlocked = true;
    }
}