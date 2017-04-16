using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public GameObject cellSprite;//клетка
    public GameObject blackSprite;//черная фишка
    public GameObject whiteSprite;//белая фишка 
    public AudioClip Clip;//звук победы
    public bool turn = true;//логическая переменная для передачи хода
    public bool gameOver;//логическая переменная для контроля конца игры
    private int id;//номер клетки
    private GameObject[,] cells;//массив клеток
    private GameObject[,] board;//массив для фишек   
    private bool[,] black;//массив для проверки черных фишек
    private bool[,] white;//массив для проверки белых фишек
    private int size = 15;//размер доски

    void Start()
    {
        board = new GameObject[size, size];//создание массива карты
        black = new bool[size, size];//массив для черных
        white = new bool[size, size];//массив для белых
        id = 0;//номер равен 0
        float posX;//переменная для хранения начальной позиции
        float posY = 8;//начальная позиция клетки по У
        cells = new GameObject[size, size];//объявление массив для хранения клеток
        for (int y = 0; y < size; y++)
        {
            posX = -10;//задание начальной позиции клетки по Х
            posY -= 1f;//смещение клетки по У
            for (int x = 0; x < size; x++)
            {
                posX += 1f;//смещение клетки по Х
                cells[x, y] = Instantiate(cellSprite, new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;//копирование клетки
                cells[x, y].name = "Cells" + x + "." + y;//задание имени клетки
                cells[x, y].GetComponent<MouseClick>().Var = this;//передача компонента MouseClick клетке
                cells[x, y].GetComponent<MouseClick>().cellID = id;//передача номера id клетке
                id++;//увеличение номера на 1
            }
        }
        gameOver = false;//говорим, что игра не окончена
    }

    public void FindBall(int ball_id)//поиск шара
    {
        if (!gameOver)//проверка на окончание игры
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (cells[x, y].GetComponent<MouseClick>().cellID == ball_id && board[x, y] == null)//если место пустое
                    {
                        if (turn == true)//если ход черных
                        {
                            board[x, y] = Instantiate(blackSprite, cells[x, y].transform.position, Quaternion.identity) as GameObject;//копирование черной фишки
                            turn = false;//передача хода белым
                            black[x, y] = true;//записываем в массив 1, значит поставили фишку
                            FindLines(black);//вызов функции для проверки 5 фишек в ряд для черных
                        }
                        else//если ход белых
                        {
                            board[x, y] = Instantiate(whiteSprite, cells[x, y].transform.position, Quaternion.identity) as GameObject;//копирование черной фишки
                            turn = true;//передача хода черным
                            white[x, y] = true;//записываем в массив 1, значит поставили фишку
                            FindLines(white);//вызов функции для проверки 5 фишек в ряд для белых
                        }
                    }
                }
            }
        }
    }

    void FindLines(bool [,] mass)//проверка на 5 фишек
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (mass[i, j] == false) continue;//пропускаем пустую клетку
                int lenght = 0;//текущая длина ряда
                int count=0;//дополнительный счетчик
                for (int k = j; k < j + 5; k++)//вправо от текущей клетки
                {
                    if ((k == size) || (mass[i, k] != true))//если нет продолжения ряда
                    {
                        break;//прерываем
                    }
                    //если есть
                    lenght++;//увеличиваем длину ряда на 1
                }

                if (lenght == 5)//есть ряд из 5
                {
                    EndGame();//вызов функции окончания игры
                }
                lenght = 0;//обнуляем длину ряда
                count = i;//присваеваем счетчику значение строки для диагонального поиска
                for (int k = j; k < j + 5; k++)//вниз и вправо от текущей клетки
                {
                    if ((k == size) || (count == size) || (mass[count, k] != true))//если нет продолжения ряда
                    {
                        break;//прерываем
                    }
                    //если есть
                    lenght++;//увеличиваем длину ряда на 1
                    count++;//увеличиваем счетчик на 1
                }
                if (lenght == 5)//есть ряд из 5
                {
                    EndGame();//вызов функции окончания игры
                }
                lenght = 0;//обнуляем длину ряда
                count = i;//присваеваем счетчику значение строки для диагонального поиска
                for (int k = j; k >j-5; k--)//вниз и влево от текущей клетки
                {
                    if ((k == -1) || (count == size) || (mass[count, k] != true))//если нет продолжения ряда
                    {
                        break;//прерываем
                    }
                    //если есть
                    lenght++;//увеличиваем длину ряда на 1
                    count++;//увеличиваем счетчик на 1
                }
                if (lenght == 5)//есть ряд из 5
                {
                    EndGame();//вызов функции окончания игры
                }
                lenght = 0;//обнуляем длину ряда
                for (int k = i; k < i + 5; k++)//вниз от текущей клетки
                {
                    if ((k == size) || (mass[k, j] != true))//если нет продолжения ряда
                    {
                        break;//прерываем
                    }
                    //если есть
                    lenght++;//увеличиваем длину ряда на 1
                }
                if (lenght == 5)//есть ряд из 5
                {
                    EndGame();//вызов функции окончания игры
                }
            }
        }
    }  
    void EndGame()//функция окончания игры
    {
        gameOver = true;//объявляем конец игры
        AudioSource.PlayClipAtPoint(Clip, Vector3.zero, 1.0f);//воспроизведение звука победы
    }
}
