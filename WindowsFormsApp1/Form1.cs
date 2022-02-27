using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int         winKrestik              = 0;              // === Счёт крестиков ======================== // 
        int         winNolik                = 0;              // === Счёт ноликов ========================== //
        byte        check                   = 0;              // === Проверка ничьи ======================== //
        string      krestikWin;                               // === Строка для вывода счёта крестиков ===== //
        string      nolikWin;                                 // === Строка для вывода счёта ноликов ======= //
        bool        playDenied              = false;          // === Запрет на нажатие после победы ======== //
        int[,]      arrayField              = new int[3, 3];  // === Состояние поля ======================== //
        bool[,]     arrayLogicFieldKrestiki = new bool[3, 3]; // === Проверка нажатия на клетку крестика === //
        bool[,]     arrayLogicFieldNoliki   = new bool[3, 3]; // === Проверка нажатия на клетку нолика ===== //
        public bool krestikiPlayer          = true;           // === Ход крестика ========================== //
        public bool nolikiPlayer            = false;          // === Ход нолика ============================ //
        public bool krestikiWin             = false;          // === Победа крестика ======================= //
        public bool nolikiWin               = false;          // === Победа нолика ========================= //
        public bool draw                    = false;

        /// <summary>
        /// Параметры запуска формы
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            this.Width                      = 1200;  // === Ширина формы ============================== //
            this.Height                     = 1000;  // === Высота формы ============================== //
            MaximizeBox                     = false; // === Запрет на раширение формы во весь экран === //

            this.MouseClick                += new MouseEventHandler(Form1_MouseClick);

            for (int i = 0; i < 3; i++)              // === Инициализация логических матриц =========== //
            {
                for (int j = 0; j < 3; j++)
                {
                    arrayLogicFieldKrestiki[i, j] = false;
                    arrayLogicFieldNoliki[i, j]   = false;
                }
            }
        }
        /// <summary>
        /// Параметры, устанавливаемые при запуске формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "0";
            richTextBox2.Text = "0";
        }
        /// <summary>
        /// Обработчик событий нажатия мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (playDenied == true) return; // === Не реагировать на мышь, если победа === //

            if (e.X >= 0 && e.X < 300)      // === Клетки по координатам === //
            {
                if (e.Y >= 0 && e.Y <= 300 && arrayField[0, 0] == 0 )
                    arrayField[0, 0] = 1;
                if (e.Y > 300 && e.Y <= 600 && arrayField[0, 1] == 0)
                    arrayField[0, 1] = 1;
                if (e.Y > 600 && e.Y <= 900 && arrayField[0, 2] == 0)
                    arrayField[0, 2] = 1;
            }

            else if (e.X >= 300 && e.X < 600)
            {
                if (e.Y >= 0 && e.Y <= 300 && arrayField[1, 0] == 0)
                    arrayField[1, 0] = 1;
                if (e.Y > 300 && e.Y <= 600 && arrayField[1, 1] == 0)
                    arrayField[1, 1] = 1;
                if (e.Y > 600 && e.Y <= 900 && arrayField[1, 2] == 0)
                    arrayField[1, 2] = 1;
            }

            else if (e.X >= 600 && e.X < 900)
            {
                if (e.Y >= 0 && e.Y <= 300 && arrayField[2, 0] == 0)
                    arrayField[2, 0] = 1;
                if (e.Y > 300 && e.Y <= 600 && arrayField[2, 1] == 0)
                    arrayField[2, 1] = 1;
                if (e.Y > 600 && e.Y <= 900 && arrayField[2, 2] == 0)
                    arrayField[2, 2] = 1;
            }

            else return;

            Invalidate();  
        }
        /// <summary>
        /// Рисование фигур и поля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
            Graphics graphics = e.Graphics;      // ==== подключение элемента graphics ==== //

            graphics.TranslateTransform(5, 5);   // ==== перенесение центра =============== //

            for (int i = 0; i < 4; i++)          // ==== вычерчивание поля ================ //
            {
                graphics.DrawLine(new Pen(Color.Black, 3.0f), 300 * i, 0, 300 * i, 900);
                graphics.DrawLine(new Pen(Color.Black, 3.0f), 0, 300 * i, 900, 300 * i);
            }
            Redraw();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {         
                    if (arrayField[i,j] == 1) // === Если была нажата кнопка === //
                    {
                        if (krestikiPlayer)
                        {
                            graphics.DrawLine(new Pen(Color.Red, 20.0f), 50 + 300 * i, 50 + 300 * j, 300 * (i + 1) - 50, 300 * (j + 1) - 50);
                            graphics.DrawLine(new Pen(Color.Red, 20.0f), 300 * (i + 1) - 50, 300 * j + 50, 50 + 300 * i, 300 * (j + 1) - 50);
                            Count(arrayLogicFieldKrestiki, i, j);
                        }
                        else if (nolikiPlayer)
                        {
                            graphics.DrawEllipse(new Pen(Color.Blue, 20.0f), 50 + 300 * i, 50 + 300 * j, 200, 200);
                            Count(arrayLogicFieldNoliki, i, j);
                        }
                    }
                }
            }
            void Count(bool [,] arrayLogic, int i, int j) // === Каждый ход проверка на победу === //
            {
                Redraw();
                arrayLogic[i, j] = true;
                arrayField[i, j] = 2;
                CheckWinner();
                krestikiPlayer = !krestikiPlayer;
                nolikiPlayer = !nolikiPlayer;
            }
            void Redraw() // === Отрисовка пропадающих фигур === //
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (arrayLogicFieldKrestiki[i, j] == true)
                        {
                            graphics.DrawLine(new Pen(Color.Red, 20.0f), 50 + 300 * i, 50 + 300 * j, 300 * (i + 1) - 50, 300 * (j + 1) - 50);
                            graphics.DrawLine(new Pen(Color.Red, 20.0f), 300 * (i + 1) - 50, 300 * j + 50, 50 + 300 * i, 300 * (j + 1) - 50);
                        }
                        if (arrayLogicFieldNoliki[i, j] == true)
                        {
                            graphics.DrawEllipse(new Pen(Color.Blue, 20.0f), 50 + 300 * i, 50 + 300 * j, 200, 200);
                        }
                    }
                }
            }
            void CheckWinner() // === Победные случаи === //
            {    
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 1, 1, 3, 1, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 1, 2, 3, 2, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 1, 3, 3, 3, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 1, 1, 1, 3, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 2, 1, 2, 3, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 3, 1, 3, 3, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 1, 1, 3, 3, out winKrestik); 
                CheckWin(arrayLogicFieldKrestiki, winKrestik, krestikWin, 3, 1, 1, 3, out winKrestik); 

                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 1, 1, 3, 1, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 1, 2, 3, 2, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 1, 3, 3, 3, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 1, 1, 1, 3, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 2, 1, 2, 3, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 3, 1, 3, 3, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 1, 1, 3, 3, out winNolik); 
                CheckWin(arrayLogicFieldNoliki, winNolik, nolikWin, 3, 1, 1, 3, out winNolik);
                if (draw & !krestikiWin & !nolikiWin)
                {
                    playDenied = true;
                    draw = true;
                    Redraw();
                    MessageBox.Show("Ничья!");
                    check = 0;
                    return;
                };
            }
            int CheckWin(bool[,] arrayLogic, int win, string winCount, int ic, int jc, int ik, int jk, out int won) // === Проверка на победу === //
            {
                won = win;
                if (jc == jk)
                {
                    if (arrayLogic[ic - 1, jc - 1] & arrayLogic[ic, jc - 1] & arrayLogic[ic + 1, jc - 1])
                    {
                        playDenied = true;
                        win++;
                        won = win;
                        winCount = Convert.ToString(win);
                        Redraw();
                        graphics.DrawLine(new Pen(Color.Yellow, 10.0f), 0, 150 * (2 * jk - 1), 900, 150 * (2 * jk - 1)); 
                        if (krestikiPlayer) 
                        { 
                            richTextBox1.Text = winCount;
                            krestikiWin = true;
                            MessageBox.Show("Выиграли крестики");
                            return won;
                        }
                        if (nolikiPlayer) 
                        { 
                            richTextBox2.Text = winCount;
                            nolikiWin = true;
                            MessageBox.Show("Выиграли нолики");
                            return won;
                        }
                        return won;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (arrayField[i, j] == 2)
                            {
                                check++;
                            }
                        }
                    }
                    if (check == 9 & !krestikiWin & !nolikiWin)
                    {
                        draw = true;
                    }
                    check = 0;
                    return won;
                }
                if (ic == ik)
                {
                    if (arrayLogic[ic - 1, jc - 1] & arrayLogic[ic - 1, jc] & arrayLogic[ic - 1, jc + 1])
                    {
                        playDenied = true;
                        win++;
                        winCount = Convert.ToString(win);
                        Redraw();
                        graphics.DrawLine(new Pen(Color.Yellow, 10.0f), 150 * (2 * ik - 1), 0, 150 * (2 * ik - 1), 900);
                        if (krestikiPlayer)
                        {
                            krestikiWin = true;
                            richTextBox1.Text = winCount;
                            MessageBox.Show("Выиграли крестики");
                            return won;
                          }
                        if (nolikiPlayer)
                        {
                            richTextBox2.Text = winCount;
                            nolikiWin = true;
                            MessageBox.Show("Выиграли нолики");
                            return won;
                        }
                        won = win;
                        return won;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (arrayField[i, j] == 2)
                            {
                                check++;
                            }
                        }
                    }
                    if (check == 9 & !krestikiWin & !nolikiWin)
                    {
                        draw = true;
                    }
                    check = 0;
                    return won;
                }
                if (ic == jc & ik == jk)
                {
                    if (arrayLogic[ic - 1, jc - 1] & arrayLogic[ic, jc] & arrayLogic[ic + 1, jc + 1])
                    {
                        playDenied = true;
                        win++;
                        winCount = Convert.ToString(win);
                        Redraw();
                        graphics.DrawLine(new Pen(Color.Yellow, 10.0f), 0, 0, 900, 900);
                        if (krestikiPlayer)
                        {
                            richTextBox1.Text = winCount;
                            krestikiWin = true;
                            MessageBox.Show("Выиграли крестики");
                            return won;
                        }
                        if (nolikiPlayer)
                        {
                            richTextBox2.Text = winCount;
                            krestikiWin = true;
                            MessageBox.Show("Выиграли нолики");
                            return won;
                        }
                        won = win;
                        return won;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (arrayField[i, j] == 2)
                            {
                                check++;
                            }
                        }
                    }
                    if (check == 9 & !krestikiWin & !nolikiWin)
                    {
                        draw = true;
                    }
                    check = 0;
                    return won;
                }
                if (ic == jk & ik == jc)
                {
                    if (arrayLogic[ic - 1, jc - 1] & arrayLogic[ic - 2, jc] & arrayLogic[ic - 3, jc + 1])
                    {
                        playDenied = true;
                        win++;
                        winCount = Convert.ToString(win);
                        Redraw();
                        graphics.DrawLine(new Pen(Color.Yellow, 10.0f), 900, 0, 0, 900);
                        if (krestikiPlayer)
                        {
                            richTextBox1.Text = winCount;
                            krestikiWin = true;
                            MessageBox.Show("Выиграли крестики");
                            return won;
                        }
                        if (nolikiPlayer)
                        {
                            richTextBox2.Text = winCount;
                            nolikiWin = true;
                            MessageBox.Show("Выиграли нолики");
                            return won;
                        }
                        won = win;
                        return won;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (arrayField[i, j] == 2)
                            {
                                check++;
                            }
                        }
                    }
                    if (check == 9 & !krestikiWin & !nolikiWin)
                    {
                        draw = true;
                    }
                    check = 0;
                }
                return won;
            }
        }
        private void button1_Click(object sender, EventArgs e) // === Новая игра === //
        {       
            this.Refresh();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arrayLogicFieldKrestiki[i, j] = false;
                    arrayLogicFieldNoliki[i, j]   = false;
                    arrayField[i, j]              = 0;
                }
            }
            krestikiPlayer = true;
            nolikiPlayer   = false;
            krestikiWin    = false;
            nolikiWin      = false;
            draw           = false;
            playDenied     = false;
            check          = 0;
            Invalidate();        
        }
        private void button2_Click(object sender, EventArgs e) // === Сбросить счёт === //
        {
            richTextBox1.Text = "0";
            richTextBox2.Text = "0";
            winNolik          = 0;
            winKrestik        = 0;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
