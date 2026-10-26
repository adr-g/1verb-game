using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private float teleportRange = 5f;
    [SerializeField] private LayerMask groundLayer;

    public void TryTeleport(RaycastHit hit, Vector3 shotOrigin)
    {
        //verificar se o obj pertence à layer ground (obj atingido)
        if ((groundLayer.value & (1 << hit.collider.gameObject.layer)) == 0)
            return;

        //distancia entre origem do disparo e impacto
        float distance = Vector3.Distance(shotOrigin, hit.point);

        if (distance > teleportRange)
            return;

        //tp player hit point
        transform.position = hit.point;

        Debug.Log("Teleport successfull");
    }
}
