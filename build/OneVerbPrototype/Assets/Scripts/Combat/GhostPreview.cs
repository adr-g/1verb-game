using UnityEngine;

public class GhostPreview : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private Material playerGhostMaterial;
    [SerializeField] private Material enemyGhostMaterial;
    [SerializeField] private float playerGhostHeight = 1f;
    [SerializeField] private float enemyGhostHeight = 0f;

    private GameObject playerGhost;
    private GameObject[] enemyGhosts;
    private EnemyBehaviour[] enemies;

    void Start()
    {
        // Obtém os inimigos registados no TurnManager.
        enemies = turnManager.GetEnemies();

        // Cria o fantasma do jogador.
        playerGhost = CreateGhost("Player Ghost", Vector3.one, playerGhostMaterial);

        // Cria um fantasma para cada inimigo.
        enemyGhosts = new GameObject[enemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
                continue;

            enemyGhosts[i] = CreateGhost("Enemy Ghost", enemies[i].transform.lossyScale, enemyGhostMaterial);
        }
    }

    void Update()
    {
        // Verifica continuamente a ação para onde o jogador aponta.
        if (!playerShoot.TryGetAction(out GameObject shootTarget, out Vector3 teleportDestination, out float actionDistance))
        {
            HideGhosts();
            return;
        }

        // O fantasma do jogador só aparece durante a previsão de teleporte.
        bool isTeleport = shootTarget == null;
        playerGhost.SetActive(isTeleport);

        if (isTeleport)
        {
            playerGhost.transform.position = teleportDestination + Vector3.up * playerGhostHeight;
        }

        // Atualiza a posição prevista de cada inimigo.
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemyGhosts[i] == null)
                continue;

            bool enemyActive = enemies[i] != null && enemies[i].gameObject.activeInHierarchy;
            enemyGhosts[i].SetActive(enemyActive);

            if (enemyActive)
            {
                enemyGhosts[i].transform.position = turnManager.GetEnemyPrediction(enemies[i], actionDistance) + Vector3.up * enemyGhostHeight;
            }
        }
    }

    GameObject CreateGhost(string ghostName, Vector3 ghostScale, Material ghostMaterial)
    {
        // Cria uma cápsula para representar a posição futura.
        GameObject ghost = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        ghost.name = ghostName;
        ghost.transform.localScale = ghostScale;

        // Remove o collider para não interferir com os raycasts.
        Collider ghostCollider = ghost.GetComponent<Collider>();
        ghostCollider.enabled = false;
        Destroy(ghostCollider);

        // Aplica o material transparente.
        if (ghostMaterial != null)
            ghost.GetComponent<Renderer>().sharedMaterial = ghostMaterial;

        ghost.SetActive(false);

        return ghost;
    }

    void HideGhosts()
    {
        // Esconde os fantasmas quando não existe uma ação válida.
        playerGhost.SetActive(false);

        for (int i = 0; i < enemyGhosts.Length; i++)
        {
            if (enemyGhosts[i] != null)
                enemyGhosts[i].SetActive(false);
        }
    }
}