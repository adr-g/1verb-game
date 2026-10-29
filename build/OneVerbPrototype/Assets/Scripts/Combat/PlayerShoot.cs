using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private PlayerTeleport playerTeleport;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private float shootRange = 8f;
    [SerializeField] private float shootActionDistance = 3f;
    [SerializeField] private Material greenMat;

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PerformAction();
        }
    }

    public bool TryGetAction(out GameObject shootTarget, out Vector3 teleportDestination, out float actionDistance)
    {
        shootTarget = null;
        teleportDestination = playerTeleport.transform.position;
        actionDistance = 0f;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Verifica o objeto para onde o jogador está a apontar.
        if (!Physics.Raycast(ray, out RaycastHit hit, shootRange))
            return false;

        // Identifica os objetos que podem ser atingidos por um disparo.
        bool isEnemy = hit.collider.GetComponentInParent<EnemyBehaviour>() != null;
        bool isButton = hit.collider.CompareTag("Button_Normal");
        bool isDoor = hit.collider.CompareTag("Door_Unlocked");

        if (isEnemy || isButton || isDoor)
        {
            // Disparar utiliza sempre uma distância de ação fixa.
            shootTarget = hit.collider.gameObject;
            actionDistance = shootActionDistance;
            return true;
        }

        // Se não for um alvo de disparo, verifica se o teleporte é válido.
        return playerTeleport.TryGetTeleportDestination(hit, ray.origin, out teleportDestination, out actionDistance);
    }

    void PerformAction()
    {
        // Obtém a ação atualmente prevista.
        if (!TryGetAction(out GameObject shootTarget, out Vector3 teleportDestination, out float actionDistance))
            return;

        Vector3 playerOldPosition = playerTeleport.transform.position;

        if (shootTarget == null)
        {
            // Teletransporta o jogador para o destino previsto.
            playerTeleport.Teleport(teleportDestination);
        }
        else
        {
            EnemyBehaviour enemy = shootTarget.GetComponentInParent<EnemyBehaviour>();

            if (enemy != null)
            {
                // Regista o disparo contra o inimigo.
                Debug.Log("Shot enemy: " + enemy.name);
            }
            else if (shootTarget.CompareTag("Button_Normal"))
            {
                // Ativa o botão e altera o seu material.
                shootTarget.tag = "Button_Pressed";

                Renderer buttonRenderer = shootTarget.GetComponent<Renderer>();
                if (buttonRenderer != null)
                    buttonRenderer.material = greenMat;

                // Desbloqueia a porta associada ao botão.
                Door door = shootTarget.GetComponent<Door>();
                if (door != null)
                    door.Unlock();
            }
            else if (shootTarget.CompareTag("Door_Unlocked"))
            {
                // Remove a porta depois de ser atingida.
                shootTarget.SetActive(false);
            }
        }

        // Executa o movimento dos inimigos após a ação do jogador.
        turnManager.ResolveTurn(actionDistance, playerOldPosition);
    }
}