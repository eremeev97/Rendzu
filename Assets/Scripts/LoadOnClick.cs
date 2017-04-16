using UnityEngine;

public class LoadOnClick : MonoBehaviour
{
	public void LoadScene(int level)//загрузка сцен
    {
        if (level == 5)//если вызывается 5 сцена
        {
            Application.Quit();//выход из программы
        }
        else//иначе
        {
            Application.LoadLevel(level);//загрузка соответствующей сцены
        }
    }
}
