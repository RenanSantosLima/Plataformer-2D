using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;        //Vida do player
    [SerializeField] private int heartsCount;   //Quantidade de coração que aparece

    [SerializeField] private Image[] hearts;    //Imagens dos corações
    [SerializeField] private Sprite fullHeart;  //Coração cheio
    [SerializeField] private Sprite noHeart;    //Coração vazio



    private void Update()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            //VErifica se o coração está cheio ou não
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = noHeart;
            }



            //Quantos corações aparece
            if(i < heartsCount)
            {
                //Mostra o coração
                hearts[i].enabled = true;
            }
            else
            {
                //Não mostra o coração
                hearts[i].enabled = false;
            }
        }
    }







}
