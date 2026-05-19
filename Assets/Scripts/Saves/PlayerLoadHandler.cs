using UnityEngine;
using System.Collections;

public class PlayerLoadHandler : MonoBehaviour
{
    [SerializeField] private PlayerController controller;

    IEnumerator Start()
    {
        if (!SaveSystem.HasPendingLoad())
        {
            yield break;
        }

        // =====================================================
        // FREEZE PLAYER INPUT / MOVEMENT
        // =====================================================
        if (controller != null)
        {
            controller.isLoading = true;
        }

        // Wait for scene + spawn systems
        yield return null;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.05f);

        SaveSlot slot = SaveSystem.ConsumePendingLoad();

        if (slot == null)
        {
            if (controller != null)
            {
                controller.isLoading = false;
            }

            yield break;
        }

        Vector3 targetPos = new Vector3(
            slot.playerX,
            slot.playerY,
            slot.playerZ
        );

        // =====================================================
        // SAFE POSITION APPLY
        // =====================================================

        CharacterController cc = GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
            transform.position = targetPos;
            cc.enabled = true;
        }
        else
        {
            transform.position = targetPos;
        }

        // =====================================================
        // RESET PLAYER STATE (IMPORTANT FIX)
        // =====================================================

        if (controller != null)
        {
            controller.state = PlayerState.Grounded;
            controller.isLoading = false;
        }

        // =====================================================
        // RESTORE META DATA
        // =====================================================

        if (PlayTimeManager.Instance != null)
        {
            PlayTimeManager.Instance.SetPlayTime(slot.playTime);
        }

        Debug.Log("Player loaded at: " + targetPos);
    }
}