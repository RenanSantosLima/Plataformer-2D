using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private Transform player;

    public static PlayerPos instance;

    private void Start()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            CheckPoint();
        }

    }

    //Função responsavel pelo checkpoint
    public void CheckPoint()
    {
        /*Vector3 playerPos = transform.position;
            playerPos.z = 0f;*/


        player.position = transform.position;
        //player.position = playerPos;
    }
}
