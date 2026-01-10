using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private int score;
    //[SerializeField] private TextMeshProUGUI scoreText; //Referencia manual
    //SerializeField] private GameObject gameOverPanel;  //referencia manual


    private void Awake()
    {
        //instance = this;

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
        Time.timeScale = 1;

        if(PlayerPrefs.GetInt("score") > 0)
        {
            //pega os dados
            score = PlayerPrefs.GetInt("score");
            Player.instance.scoreText.text = "x " + score.ToString();
        }
    }

    public void GetCoin()
    {
        score++;
        Player.instance.scoreText.text = "x " + score.ToString();

        //Salvando dados
        PlayerPrefs.SetInt("score", score);
    }


    //-----Game Over---------
    public void ShowGameOver()
    {
        Time.timeScale = 0;
        Player.instance.gameOver.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
