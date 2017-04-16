using UnityEngine;

public class GameProcessSI : MonoBehaviour
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
    private int[,] black;//массив для проверки черных фишек
    private int[,] white;//массив для проверки белых фишек
    int size = 15;//размер доски
    int valuation_factor = 3;
    int last_x;
    int last_y;

    void Start()
    {
        board = new GameObject[size, size];//создание массива карты
        black = new int[size, size];//массив для черных
        white = new int[size, size];//массив для белых
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
                cells[x, y].GetComponent<MouseClickSI>().Var = this;//передача компонента MouseClick клетке
                cells[x, y].GetComponent<MouseClickSI>().cellID = id;//передача номера id клетке
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
                    if (cells[x, y].GetComponent<MouseClickSI>().cellID == ball_id && board[x, y] == null)//если место пустое
                    {
                        if (turn == true)//если ход черных
                        {
                            SI(black);
                            //board[x, y] = Instantiate(blackSprite, cells[x, y].transform.position, Quaternion.identity) as GameObject;//копирование черной фишки
                            turn = false;//передача хода белым
                            black[x, y] = 1;//записываем в массив 1, значит поставили фишку
                            FindLines(black);//вызов функции для проверки 5 фишек в ряд для черных
                        }
                        else//если ход белых
                        {
                            board[x, y] = Instantiate(whiteSprite, cells[x, y].transform.position, Quaternion.identity) as GameObject;//копирование черной фишки
                            turn = true;//передача хода черным
                            white[x, y] = 1;//записываем в массив 1, значит поставили фишку
                            FindLines(white);//вызов функции для проверки 5 фишек в ряд для белых
                        }
                    }
                }
            }
        }
    }

    void FindLines(int[,] mass)//проверка на 5 фишек
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (mass[i, j] == 0) continue;//пропускаем пустую клетку
                int cur = mass[i, j];//значение текущей клетки
                int lenght = 0;//текущая длина ряда
                int count;//дополнительный счетчик
                for (int k = j; k < j + 5; k++)//вправо от текущей клетки
                {
                    if ((k == size) || (mass[i, k] != 1))//если нет продолжения ряда
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
                    if ((k == size) || (count == size) || (mass[count, k] != 1))//если нет продолжения ряда
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
                for (int k = j; k > j - 5; k--)//вниз и влево от текущей клетки
                {
                    if ((k == -1) || (count == size) || (mass[count, k] != 1))//если нет продолжения ряда
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
                    if ((k == size) || (mass[k, j] != 1))//если нет продолжения ряда
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

    void SI(int[,] mass)
    {
        float max = -1;//Максимальное значение оценочной функции
        int cur_x = 0, cur_y = 0;//Текущие x и у
        int povtor_num = 0;//Количество повторов одинаковых значений оценочной функции
        int cur_povtor = 0;//Номер текущего повтора
                           //Рассчитываем оценочную функцию для всех клеток
        long[,] calc_fields = new long[size, size]; 
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (mass[i, j] == 0)
                {
                    //Расчет оценочной функции
                    calc_fields[i, j] = Calculate(2, i, j, mass) + Calculate(1, i, j, mass);//Calculate(1, i, j)*(float)attack_factor
                                                                                            //Берем в расчет уровень (для профессионала случайности нет)
                                                                                            //if (comp_level == 1)//Для любителя (небольшая случайность)
                                                                                            //{
                    int temp;
                    temp = Random.Range(0, 100);
                    //calc_fields[i, j] *= (1 + Random.Range(100, 10000)/ 32767)/ 2;
                    //}
                    //if (comp_level == 2)//Для новичка (максимальная случайность)
                    //{
                    //    calc_fields[i][j] *= ((float)rand() / 32767);
                    //}
                    if (calc_fields[i, j] == max)
                    {
                        //Еще одна клетка с максимальным значением оценочной функции
                        povtor_num++;
                    }
                    if (calc_fields[i, j] > max)
                    {
                        //Клетка с максимальным значением оценочной функции
                        max = calc_fields[i, j];
                        povtor_num = 0;
                        cur_x = i;
                        cur_y = j;
                    }
                }
            }
        }
        //Проверяем, есть ли вообще свободные клетки на поле
        if (max == -1)
        {
            return;
        }
        //Выбираем куда сделать ход
        if (povtor_num > 0)
        {
            //Выбираем куда ходить случайным образом из клеток с одинаковыми значениями оценочной функции
            cur_povtor = Random.Range(0, 100) / (32767 / povtor_num);//Номер элемента, куда надо ходить
                                                       //Ищем его по полю
            int buf_povtor = -1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (calc_fields[i, j] == max)
                    {
                        buf_povtor++;
                        if (buf_povtor == cur_povtor) //Клетка найдена
                        {
                            board[i, j] = Instantiate(blackSprite, cells[i, j].transform.position, Quaternion.identity) as GameObject;//копирование черной фишки
                            //mass[i, j] = 2;//Ставим крестик
                            last_x = i;//Запоминаем координаты последнего хода
                            last_y = j;
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            //Одна клетка с максимальным знаечением
            board[cur_x, cur_y] = Instantiate(blackSprite, cells[cur_x, cur_y].transform.position, Quaternion.identity) as GameObject;//копирование черной фишки
            //mass[cur_x, cur_y] = 2;//Ставим крестик
            last_x = cur_x;//Запоминаем координаты последнего хода
            last_y = cur_y;
        }
    }
    //Функция расчета оценочной функции
    long Calculate(int id, int x, int y, int[,] mass)
    {
        //Подсчет оценочной функции
        //Ставим в массиве временно значение == id
        mass[x, y] = id;
        int series_length = 0;//Текущая длина ряда
        long sum = 0;//Общее значение оценочной функции
                     ///////////Расчет сверху вниз/////////
                     //Проход по каждой клетки, которая может входить в ряд
        for (int i = 0; i < 5; i++)
        {
            //Проверка, не вышли ли за границы поля
            if ((x - 4 + i) < 0) continue;
            if ((x + i) > (size - 1)) break;
            //Проход по всем возможным рядам, отстоящим от клетки не более чем на 5
            for (int j = 0; j < 5; j++)
            {
                if ((mass[x - 4 + i + j, y] != id) && (mass[x - 4 + i + j, y] != 0))
                {
                    //Конец ряда
                    series_length = 0;
                    break;
                }
                if (mass[x - 4 + i + j, y] != 0) series_length++; //Ряд увеличивается
            }
            if (series_length == 1) series_length = 0;//Ряд из самой клетки не учитываем
            if (series_length == 5) series_length = 100; //Выигрышная ситуация, ставим большое значение
                                                         //Плюсуем серию к общей сумме
            long pow_st = valuation_factor;
            if (series_length == 100)
            {
                if (id == 2)
                    pow_st = 10000;//Большое значение при своем выигрыше
                else
                    pow_st = 1000; //Большое значение при выигрыше соперника, но меньшее, чем при своем
            }
            else
            {
                for (int j = 0; j < series_length; j++)//Возводим оценочный коэффициент в степень длины серии
                {
                    pow_st *= valuation_factor;
                }
            }
            sum += pow_st;
            series_length = 0;
        }
        ///////////Расчет слева направо/////////
        //Проход по каждой клетки, которая может входить в ряд
        for (int i = 0; i < 5; i++)
        {
            //Проверка, не вышли ли за границы поля
            if ((y - 4 + i) < 0) continue;
            if ((y + i) > (size - 1)) break;
            //Проход по всем возможным рядам, отстоящим от клетки не более чем на 5
            for (int j = 0; j < 5; j++)
            {
                if ((mass[x, y - 4 + i + j] != id) && (mass[x, y - 4 + i + j] != 0))
                {
                    //Конец ряда
                    series_length = 0;
                    break;
                }
                if (mass[x, y - 4 + i + j] != 0) series_length++; //Ряд увеличивается
            }
            if (series_length == 1) series_length = 0; //Ряд из самой клетки не учитываем
            if (series_length == 5) series_length = 100; //Выигрышная ситуация, ставим большое значение
                                                         //Плюсуем серию к общей сумме
            long pow_st = valuation_factor;
            if (series_length == 100)
            {
                if (id == 2)
                    pow_st = 10000;//Большое значение при своем выигрыше
                else
                    pow_st = 1000; //Большое значение при выигрыше соперника, но меньшее, чем при своем
            }
            else
            {
                for (int j = 0; j < series_length; j++)//Возводим оценочный коэффициент в степень длины серии
                {
                    pow_st *= valuation_factor;
                }
            }
            sum += pow_st;
            series_length = 0;
        }
        ///////////Расчет по диагонали с левого верхнего/////////
        //Проход по каждой клетки, которая может входить в ряд
        for (int i = 0; i < 5; i++)
        {
            //Проверка, не вышли ли за границы поля
            if ((y - 4 + i) < 0) continue;
            if ((x - 4 + i) < 0) continue;
            if ((x + i) > (size - 1)) break;
            if ((y + i) > (size - 1)) break;
            //Проход по всем возможным рядам, отстоящим от клетки не более чем на 5
            for (int j = 0; j < 5; j++)
            {
                if ((mass[x - 4 + i + j, y - 4 + i + j] != id) && (mass[x - 4 + i + j, y - 4 + i + j] != 0))
                {
                    //Конец ряда
                    series_length = 0;
                    break;
                }
                if (mass[x - 4 + i + j, y - 4 + i + j] != 0) series_length++; //Ряд увеличивается
            }
            if (series_length == 1) series_length = 0; //Ряд из самой клетки не учитываем
            if (series_length == 5) series_length = 100; //Выигрышная ситуация, ставим большое значение
                                                         //Плюсуем серию к общей сумме
            long pow_st = valuation_factor;
            if (series_length == 100)
            {
                if (id == 2)
                    pow_st = 10000;//Большое значение при своем выигрыше
                else
                    pow_st = 1000; //Большое значение при выигрыше соперника, но меньшее, чем при своем
            }
            else
            {
                for (int j = 0; j < series_length; j++)//Возводим оценочный коэффициент в степень длины серии
                {
                    pow_st *= valuation_factor;
                }
            }
            sum += pow_st;
            series_length = 0;
        }
        ///////////Расчет по диагонали с левого нижнего/////////
        //Проход по каждой клетки, которая может входить в ряд
        for (int i = 0; i < 5; i++)
        {
            //Проверка, не вышли ли за границы поля
            if ((y - 4 + i) < 0) continue;
            if ((x + 4 - i) > (size - 1)) continue;
            if ((x - i) < 0) break;
            if ((y + i) > (size - 1)) break;
            //Проход по всем возможным рядам, отстоящим от клетки не более чем на 5
            for (int j = 0; j < 5; j++)
            {
                if ((mass[x + 4 - i - j, y - 4 + i + j] != id) && (mass[x + 4 - i - j, y - 4 + i + j] != 0))
                {
                    //Конец ряда
                    series_length = 0;
                    break;
                }
                if (mass[x + 4 - i - j, y - 4 + i + j] != 0) series_length++; //Ряд увеличивается
            }
            if (series_length == 1) series_length = 0; //Ряд из самой клетки не учитываем
            if (series_length == 5) series_length = 100; //Выигрышная ситуация, ставим большое значение
                                                         //Плюсуем серию к общей сумме
            long pow_st = valuation_factor;
            if (series_length == 100)
            {
                if (id == 2)
                    pow_st = 10000;//Большое значение при своем выигрыше
                else
                    pow_st = 1000; //Большое значение при выигрыше соперника, но меньшее, чем при своем
            }
            else
            {
                for (int j = 0; j < series_length; j++)//Возводим оценочный коэффициент в степень длины серии
                {
                    pow_st *= valuation_factor;
                }
            }
            sum += pow_st;
            series_length = 0;
        }
        //Возвращаем исходное значение
        mass[x, y] = 0;
        return sum;
    }
}
