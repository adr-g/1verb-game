using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private float teleportRange = 5f;
    [SerializeField] private LayerMask groundLayer;

    public bool TryGetTeleportDestination(RaycastHit hit, Vector3 shotOrigin, out Vector3 destination, out float actionDistance)
    {
        destination = transform.position;
        actionDistance = 0f;

        // Verifica se o objeto atingido pertence à layer Ground.
        if ((groundLayer.value & (1 << hit.collider.gameObject.layer)) == 0)
            return false;

        // Verifica se o destino está dentro do alcance do teleporte.
        if (Vector3.Distance(shotOrigin, hit.point) > teleportRange)
            return false;

        destination = hit.point;

        // Calcula a distância horizontal que o jogador vai percorrer.
        Vector3 movement = destination - transform.position;
        movement.y = 0f;
        actionDistance = movement.magnitude;

        return true;
    }

    public void Teleport(Vector3 destination)
    {
        // Move o jogador para o destino já validado.
        transform.position = destination;

        Debug.Log("Teleport successful");
    }
}
