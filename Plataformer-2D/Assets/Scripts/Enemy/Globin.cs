using UnityEngine;

public class Globin : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private bool isRight;
    [SerializeField] private Transform point;       //Frente
    [SerializeField] private Transform behind;      //Trás
    [SerializeField] private float speed;
    [SerializeField] private float maxVision;

    [SerializeField] private float stopDistance;

    [Header("Health Settings:")]
    [SerializeField] private int health;

    private bool isDead;                            //Se verdadeira, está morto
    private bool isFront;
    private Vector2 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (isRight) //Virado para direita
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else //Virado para esquerda
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        GetPlayer();
        OnMove();
    }

    private void OnMove()
    {
        if (isFront && !isDead)
        {
            anim.SetInteger("transition", 1);

            if (isRight) //Virado para direita
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            }
            else //Virado para esquerda
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            }
        }
    }

    private void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        if (hit.collider != null && !isDead)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance) //distancia para atacar
                {
                    isFront = false;
                    rb.linearVelocity = Vector2.zero;

                    anim.SetInteger("transition", 2);

                    hit.transform.GetComponent<Player>().OnHit();
                }
            }
            else
            {
                isFront = false;
                rb.linearVelocity = Vector2.zero;
                anim.SetInteger("transition", 0);
            }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision);

        if (behindHit.collider != null)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                //O player está nas costas do inimigo
                isRight = !isRight;
                isFront = true;
            }
        }
    }

    //Função responsavel pelo hit
    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if (health <= 0)
        {
            isDead = true;
            speed = 0;
            anim.SetTrigger("dead");//animação de morte
            Destroy(gameObject, 1f);//Destroi o inimigo
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(point.position, direction * maxVision);
        Gizmos.DrawRay(behind.position, -direction * maxVision);
    }
}
