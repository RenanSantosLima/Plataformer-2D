using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAudio playerAudio;
    [SerializeField] private Animator anim;
    private Health healthSystem;

    [Header("Move Settings:")]
    [SerializeField] private float speed;           //VElocidade do player

    [Header("Jump Settings:")]
    [SerializeField] private float jumpForce;       //Força do pulo

    [Header("Attak Settings:")]
    [SerializeField] private Transform point;       //Ponto do ataque
    [SerializeField] private float radius;          //Tamanho do colisor de ataque
    [SerializeField] private LayerMask enemyLayer;  //Layer do inimigo


    [Header("UI Settings:")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOver;


    private bool isAttacking;                       //Se verdadeira, está atacando
    private bool isJumping;                         //Se verdadeira, está pulando
    private bool doubleJump;                        //Se verdadeira, pulo duplo 

    [SerializeField] private float recoveryTime;
    private bool recovery;


    public static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
        healthSystem = GetComponent<Health>();
    }

    private void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }


    //função responsavel por movimentar o player
    private void Move()
    {
        //Se nada for presionado, retorna 0.
        //Se for presionado para direita, retorna 1. Se presionado para esquerda, retrona -1
        float movement = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                //Animação de andar
                anim.SetInteger("transition", 1);
            }
            //Faz o flip
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                //Animação de andar
                anim.SetInteger("transition", 1);
            }
            //Faz o flip
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && !isJumping && !isAttacking)
        {
            //Animação de parado
            anim.SetInteger("transition", 0);
        }
    }

    //Função responsavel pelo pulo
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                anim.SetInteger("transition", 2); //Animação de pular
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if (doubleJump)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                anim.SetInteger("transition", 2);   //Animação de pular
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }

        }
    }


    //Função responsavel pelo ataque
    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("transition", 3);
            //Se colidui com algo
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            playerAudio.PlaySFX(playerAudio.attackSound);

            if (hit != null)
            {
                if (hit.GetComponent<Slime>())
                {
                    //Dano no inimigo
                    hit.GetComponent<Slime>().OnHit();
                }

                if (hit.GetComponent<Globin>())
                {
                    hit.GetComponent<Globin>().OnHit();
                }

            }

            StartCoroutine(FinishAttack());
        }
    }

    //Finaliza o ataque
    private IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(0.35f);    //Tempo da animação
        isAttacking = false;
    }

    //Função responsavel pelo hit
    public void OnHit()
    {
        if (!recovery)
        {
            anim.SetTrigger("hit");
            healthSystem.health--;

            if (healthSystem.health <= 0)
            {
                recovery = true;
                anim.SetTrigger("dead");
                //game over aqui
                GameController.instance.ShowGameOver();
            }
            else
            {
                StartCoroutine(Recover());
            }
        }
    }

    private IEnumerator Recover()
    {
        recovery = true;
        yield return new WaitForSeconds(recoveryTime);
        recovery = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 6 || col.gameObject.layer == 10)
        {
            isJumping = false;
        }

        //---- Recomenda-se fazer em um script diferente---------
        if (col.gameObject.layer == 9)
        {
            PlayerPos.instance.CheckPoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            //Chama o hit
            OnHit();
        }

        if (collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.gameObject.GetComponent<Animator>().SetTrigger("hit"); //animação de hit
            GameController.instance.GetCoin(); //autaliza a ui de moedas
            Destroy(collision.gameObject, 0.42f); //Destroi o objeto depois da animação
        }

    }

}
