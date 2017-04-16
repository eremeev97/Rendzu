using UnityEngine;
using System.Collections;

public class MouseClickSI : MonoBehaviour {

    public int cellID;//номер клетки
    public GameProcessSI Var;//обращение к переменным скрипта Rendzu.cs
    public AudioClip Clip;//звуковой компонент

    void OnMouseDown()//обрабокта клика мышки
    {
        Var.FindBall(cellID);//передача в функцию FindBall номера клетки cellID
        AudioSource.PlayClipAtPoint(Clip, Vector3.zero, 1.0f);//воспроизведение звука клика
    }
}
