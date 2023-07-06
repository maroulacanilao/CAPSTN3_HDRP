using UnityEngine;

namespace TestScripts
{
    public class TestIndicator : MonoBehaviour
    {
public Transform targetTransform;   // Reference to the target's transform
    public Camera mainCamera;           // Reference to the main camera
    public RectTransform indicator;     // Reference to the indicator's RectTransform
    public float indicatorPadding = 20f; // Padding around the edges of the screen

    private void Update()
    {
        // Check if the target is visible on the screen
        if (IsTargetVisible())
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        // Calculate the target's position on the screen
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(targetTransform.position);

        // Clamp the target's screen position to the screen bounds
        Vector3 clampedScreenPos = new Vector3(
            Mathf.Clamp(targetScreenPos.x, indicatorPadding, Screen.width - indicatorPadding),
            Mathf.Clamp(targetScreenPos.y, indicatorPadding, Screen.height - indicatorPadding),
            targetScreenPos.z
        );

        // Update the indicator's position
        indicator.position = clampedScreenPos;
        indicator.gameObject.SetActive(true);

        // Calculate the direction to the target from the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 direction = (clampedScreenPos - screenCenter).normalized;

        // Rotate the indicator to point towards the target
        indicator.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    private bool IsTargetVisible()
    {
        // Check if the target is behind the camera
        Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(targetTransform.position);
        if (targetViewportPos.z < 0f)
            return false;

        // Check if the target is within the view frustum of the camera
        return targetViewportPos.x >= 0f && targetViewportPos.x <= 1f &&
               targetViewportPos.y >= 0f && targetViewportPos.y <= 1f;
    }
    }
}