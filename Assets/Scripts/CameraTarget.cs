using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform playerTransform;

    [SerializeField]
    private float cameraLookaheadRatio;

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cameraTargetPosition =
            (mousePosition + (cameraLookaheadRatio - 1) * (Vector2)playerTransform.position)
            / cameraLookaheadRatio;

        transform.position = cameraTargetPosition;
    }
}
