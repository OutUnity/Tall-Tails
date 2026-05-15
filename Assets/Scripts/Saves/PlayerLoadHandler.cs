using UnityEngine;

public class PlayerLoadHandler : MonoBehaviour
{
    void Start()
    {
        if (!SaveSystem.HasPendingLoad())
        {
            return;
        }

        SaveSlot slot = SaveSystem.ConsumePendingLoad();

        if (slot == null)
        {
            return;
        }

        transform.position = new Vector3(
            slot.playerX,
            slot.playerY,
            slot.playerZ
        );

        if (PlayTimeManager.Instance != null)
        {
            PlayTimeManager.Instance.SetPlayTime(slot.playTime);
        }
    }
}