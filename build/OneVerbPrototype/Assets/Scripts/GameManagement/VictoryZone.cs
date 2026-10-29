using UnityEngine;

public class VictoryZone : MonoBehaviour
{

    [SerializeField] private GameObject victoryText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("VICTORY!");

            victoryText.SetActive(true);
        }
    }
}
