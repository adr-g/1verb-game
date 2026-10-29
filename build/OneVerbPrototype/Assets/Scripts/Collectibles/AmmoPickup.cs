using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 10;
    //na interação do player com o collider do ammo pickup
    private void OnTriggerEnter(Collider other)
    {
        PlayerShoot player = other.GetComponent<PlayerShoot>();

        if (player != null)
        {
            //add ammo to player
            player.AddAmmo(ammoAmount);

            //remover o colectable dps de apanhar
            gameObject.SetActive(false);
        }
    }
}
