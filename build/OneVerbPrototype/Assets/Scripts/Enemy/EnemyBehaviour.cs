using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float movementMultiplier = 0.5f;

    public Vector3 GetPredictedPosition(Vector3 playerPosition, float actionDistance)
    {
        // Calcula a distância que o inimigo pode percorrer neste turno.
        float moveDistance = actionDistance * movementMultiplier;

        // Mantém a altura atual do inimigo.
        Vector3 targetPosition = new Vector3(
            playerPosition.x,
            transform.position.y,
            playerPosition.z
        );

        // Calcula a nova posição sem ultrapassar o jogador.
        return Vector3.MoveTowards(transform.position, targetPosition, moveDistance);
    }
}
