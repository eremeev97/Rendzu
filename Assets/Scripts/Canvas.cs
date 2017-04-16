using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public GameProcess Var;//обращение к переменным скрипта Game.cs
    public Text turnText;//объявление переменной turntext, которая является текстовым компонентом
    public Text winText;//объявление переменной wintext, которая является текстовым компонентом
    public Image image;//объявление переменной image, которая является графическим компонентом

    void Update()
    {
        if (Var.turn == true)//если ход черных
        {
            turnText.text = "Ход" + "\n" + "черных";//отображение текста "Ход черных"
            image.color = Color.black;//изменение цвета изображения на черный
        }
        else//если ход белых
        {
            turnText.text = "Ход" + "\n" + "белых";//отображение текста "Ход белых"
            image.color = Color.white;//изменение цвета изображения на белый
        }
        if (Var.gameOver == true)//если конец игры
        {
            if (Var.turn == true)//если ход черных
            {
                winText.text = "Победа белых";//отображение текста "Ход черных"
            }
            else//если ход белых
            {
                winText.text = "Победа черных";//отображение текста "Ход белых"
            }
        }
    }
}
