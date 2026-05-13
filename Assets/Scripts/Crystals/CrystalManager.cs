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

    [Header("Map Fog Reference")]
    public List<GameObject> regionFogObjects; // assign 7 fog images

  

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {

        // Initialize fog based on the unlocked boolean
        for (int i = 0; i < regions.Count; i++)
        {
            if (regionFogObjects[i] != null)
                regionFogObjects[i].SetActive(!regions[i].unlocked);
        }

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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (regions == null || regionFogObjects == null) return;

        for (int i = 0; i < regions.Count && i < regionFogObjects.Count; i++)
        {
            if (regionFogObjects[i] != null)
                regionFogObjects[i].SetActive(!regions[i].unlocked);
        }
    }
#endif

    public void UpdateHUD()
    {
        if (totalCrystalText != null)
        {
            RegionData region = regions.Find(r => r.regionID == currentRegionID);

            if (region != null)
                totalCrystalText.text = region.collected + " / " + region.maxCrystals;
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
        {
            region.unlocked = true;

            // remove fog
            if (regionFogObjects != null && regionID - 1 < regionFogObjects.Count)
            {
                regionFogObjects[regionID - 1].SetActive(false);
            }
        }
    }
}