
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private PlayerTeleport playerTeleport;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float shootRange = 8f;

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            //enviar impacto ao sistema de tp
            if (playerTeleport != null)
            {
                playerTeleport.TryTeleport(hit, ray.origin);
            }
        }
        else
        {
            Debug.Log("Miss");
        }
    }
}