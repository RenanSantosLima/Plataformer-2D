using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Animator barrierAnim;

    [SerializeField] private LayerMask layer;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        OnCollision();
    }

    private void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1, layer);

        if(hit != null)
        {
            OnPressed();
            hit = null;
        }
        else
        {
            OnExit();
        }
    }

    private void OnPressed()
    {
        anim.SetBool("isPressed", true);
        barrierAnim.SetBool("down", true);
    }

    private void OnExit()
    {
        anim.SetBool("isPressed", false);
        barrierAnim.SetBool("down", false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }




    /*
    //retorna quando um objeto está em colisão com outro
    private void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Stone"))
        {
            OnPressed();
        }
    }


    //retona quando um objeto sai de colisão com outro
    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Stone"))
        {
            OnExit();
        }
    }
    */
}
