using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move Settings:")]
    [SerializeField] private float speed;

    [Header("Ground Settings:")]
    [SerializeField] private Transform point;       //Posição do ponto
    [SerializeField] private float radius;          //Tamanho do raio 
    [SerializeField] private LayerMask groundLayer; //Ground layer

    [Header("Health Settings")]
    [SerializeField] private int health;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        CheckGround();
    }

    //Função responsavel pelo detectar a colisão com a parede
    private void CheckGround()
    {
        Collider2D wallGround = Physics2D.OverlapCircle(point.position, radius, groundLayer);

        if(wallGround != null)
        {
            //Inverte a velocidade
            speed = -speed;

            //Flip o inimigo
            if(transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }


    //Função responsavel pelo hit
    public void OnHit()
    {
        anim.SetTrigger("hit");     //Animação de hit
        health--;

        if(health <= 0)
        {
            speed = 0;
            //Animação de morte
            anim.SetTrigger("dead");
            Destroy(gameObject, 1f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

}
