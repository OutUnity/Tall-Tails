using System.Collections;
using UnityEngine;

public class PlayerLoadHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;

    [SerializeField] private Rigidbody rb;

    private IEnumerator Start()
    {
        if (controller != null)
        {
            controller.isLoading = true;
        }

        yield return null;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.05f);

        // =====================================================
        // APPLY SAVE
        // =====================================================

        if (SaveSystem.HasPendingLoad())
        {
            SaveSlot slot = SaveSystem.ConsumePendingLoad();

            if (slot != null)
            {
                Vector3 targetPos = new Vector3(
                    slot.playerX,
                    slot.playerY,
                    slot.playerZ
                );

                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.position = targetPos;
                }
                else
                {
                    transform.position = targetPos;
                }

                if (PlayTimeManager.Instance != null)
                {
                    PlayTimeManager.Instance.SetPlayTime(slot.playTime);
                }

                Debug.Log("Loaded player at: " + targetPos);
            }
        }

        // =====================================================
        // RESTORE PLAYER
        // =====================================================

        if (controller != null)
        {
            controller.state = PlayerState.Grounded;
            controller.isLoading = false;
        }

        // =====================================================
        // FADE OUT
        // =====================================================

        if (LoadingUI.Instance != null)
        {
            yield return LoadingUI.Instance.FadeOut();
        }
    }
}