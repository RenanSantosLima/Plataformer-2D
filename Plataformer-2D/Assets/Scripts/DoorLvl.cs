using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLvl : MonoBehaviour
{
    [SerializeField] private int lvlIndex;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            SceneManager.LoadScene(lvlIndex);
        }
    }
}
