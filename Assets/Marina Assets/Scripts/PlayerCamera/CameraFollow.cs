using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.125f;
    private bool shouldFollow = false; // Flag para indicar se a câmera deve seguir o player

    private void LateUpdate()
    {
        if (shouldFollow && player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void StartFollowing()
    {
        shouldFollow = true;
    }

    public void StopFollowing()
    {
        shouldFollow = false;
        transform.position = new Vector3(0, 0, -10); // Posiciona a câmera em (0, 0, -10)
    }
}