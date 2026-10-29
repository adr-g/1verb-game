
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private PlayerTeleport playerTeleport;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float shootRange = 8f;
    [SerializeField] private Material greenMat;


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

            if(hit.collider.CompareTag("Button_Normal"))
            {
                hit.collider.gameObject.tag = "Button_Pressed";
                Renderer buttonRenderer = hit.collider.GetComponent<Renderer>();
                if(buttonRenderer != null)
                {
                    buttonRenderer.material = greenMat;
                }

                Door door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    door.Unlock();
                }
                

            }

            if(hit.collider.CompareTag("Door_Unlocked"))
            {
                hit.collider.gameObject.SetActive(false);
            }

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