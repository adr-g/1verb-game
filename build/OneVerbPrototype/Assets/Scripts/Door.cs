using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject door;
    [SerializeField] private Material doorMat;

    public void Unlock()
    {   
        if(door == null)
        {
            return;
        }

        door.tag = "Door_Unlocked";
        Renderer doorRenderer = door.GetComponent<Renderer>();
        if(doorRenderer != null)
        {
            doorRenderer.material = doorMat;
        }
    }
}
