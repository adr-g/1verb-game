using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private EnemyBehaviour[] enemies;

    public EnemyBehaviour[] GetEnemies()
    {
        return enemies;
    }

    public Vector3 GetEnemyPrediction(EnemyBehaviour enemy, float actionDistance)
    {
        // Calcula a posição prevista sem mover o inimigo.
        return enemy.GetPredictedPosition(player.position, actionDistance);
    }

    public void ResolveTurn(float actionDistance, Vector3 playerPositionBeforeAction)
    {
        Vector3[] newPositions = new Vector3[enemies.Length];

        // Calcula todas as posições antes de mover os inimigos.
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null || !enemies[i].gameObject.activeInHierarchy)
                continue;

            newPositions[i] = enemies[i].GetPredictedPosition(playerPositionBeforeAction, actionDistance);
        }

        // Move todos os inimigos no mesmo turno.
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null || !enemies[i].gameObject.activeInHierarchy)
                continue;

            enemies[i].transform.position = newPositions[i];
        }
    }
}