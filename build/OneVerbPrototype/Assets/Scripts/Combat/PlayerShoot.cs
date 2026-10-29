using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private PlayerTeleport playerTeleport;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private float shootRange = 8f;
    [SerializeField] private float shootActionDistance = 3f;
    [SerializeField] private Material greenMat;

    [SerializeField] private int maxAmmo = 10;

    private int currentAmmo;

    //start ammo é = maxAmmo
    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

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

        // Verifica o objeto para onde o jogador est� a apontar.
        if (!Physics.Raycast(ray, out RaycastHit hit, shootRange))
            return false;

        // Identifica os objetos que podem ser atingidos por um disparo.
        bool isEnemy = hit.collider.GetComponentInParent<EnemyBehaviour>() != null;
        bool isButton = hit.collider.CompareTag("Button_Normal");
        bool isDoor = hit.collider.CompareTag("Door_Unlocked");

        if (isEnemy || isButton || isDoor)
        {
            // Disparar utiliza sempre uma dist�ncia de a��o fixa.
            shootTarget = hit.collider.gameObject;
            actionDistance = shootActionDistance;
            return true;
        }

        // Se n�o for um alvo de disparo, verifica se o teleporte � v�lido.
        return playerTeleport.TryGetTeleportDestination(hit, ray.origin, out teleportDestination, out actionDistance);
    }

    void PerformAction()
    {
        //para action quando o player esta sem balas
        if (currentAmmo <= 0)
            return;

        // Obt�m a a��o atualmente prevista.
        if (!TryGetAction(out GameObject shootTarget, out Vector3 teleportDestination, out float actionDistance))
            return;

        //diminuir ammo a cada shot
        currentAmmo--;

        //update do count no texto da ammo ui
        UpdateAmmoUI();

        Debug.Log("Ammo: " + currentAmmo);

        Vector3 playerOldPosition = playerTeleport.transform.position;

        if (shootTarget == null)
        {
            // Teletransporta o jogador para o destino previsto.
            playerTeleport.Teleport(teleportDestination);
        }
        else
        {
            EnemyBehaviour enemy = shootTarget.GetComponentInParent<EnemyBehaviour>();

            if (enemy != null && enemy.enabled)
            {
                Renderer rend = enemy.GetComponent<Renderer>();
                // mudar cor para black
                if (rend != null)
                    rend.material.color = Color.black;

                // mudar a scale e posição dos enemy quando o player "mata"
                enemy.transform.localScale = new Vector3(1.5f, 0.15f, 1.5f);
                enemy.transform.position += Vector3.down * 0.85f;

                Collider col = enemy.GetComponent<Collider>();

                if (col != null)
                    col.enabled = false;
                //dar disabled ao enemy
                enemy.enabled = false;

                // Regista o disparo contra o inimigo.
                Debug.Log("Shot enemy: " + enemy.name);
            }

            else if (shootTarget.CompareTag("Button_Normal"))
            {
                // Ativa o bot�o e altera o seu material.
                shootTarget.tag = "Button_Pressed";

                Renderer buttonRenderer = shootTarget.GetComponent<Renderer>();
                if (buttonRenderer != null)
                    buttonRenderer.material = greenMat;

                // Desbloqueia a porta associada ao bot�o.
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

        // Executa o movimento dos inimigos ap�s a a��o do jogador.
        turnManager.ResolveTurn(actionDistance, playerOldPosition);
    }

    //ammo pickup
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);

        UpdateAmmoUI();

        Debug.Log("Ammo: " + currentAmmo);
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
}
