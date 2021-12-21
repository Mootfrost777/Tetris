using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

// один язык в другой
using Microsoft.VisualBasic;

namespace SuperTetris_v10._0
{
    public partial class Form1 : Form
    {
        Shape f1 = new Shape();
        int[,] map = new int[15, 10];    // строки - столбцы
        int size = 40;
        int Score = 0;
        int HighScore = 0;
        string name;

        public Form1()
        {
            InitializeComponent();
            timer1.Stop();
            name = Interaction.InputBox("Enter Name:");
            if (name == "")
            {
                name = "Жора";
            }
            Init();
            f1.x = 4;
            f1.y = 0;
        }
        public void Init ()
        {
            f1.ResetShape(4, 0);
            timer1.Start(); 
            Invalidate();
            StreamReader HS = new StreamReader("HS.txt.txt");
            HighScore = Convert.ToInt32(HS.ReadLine());
            HS.Close();
            label4.Text = Convert.ToString(HighScore);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;    // инструмент рисования

            DrawGrid(g);
            DrawMap(g);
        }

        public void Merge()    // синхронизация (объединение двумерных массивов)
        {
            int x = f1.x;
            int y = f1.y;

            for (int i = y; i < y + f1.matrix.GetLength(0); i++)   // ребята
            {
                for (int j = x; j < x + f1.matrix.GetLength(1); j++)
                {
                    if (f1.matrix[i - f1.y, j - f1.x] != 0)
                    {
                        map[i, j] = f1.matrix[i - f1.y, j - f1.x];
                    }
                }
            }
        }

        private void ClearMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        private void DrawMap(Graphics g)
        {
            int n = map.GetLength(0);
            int m = map.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int x = size * j;
                    int y = size * i;
                    int width = size;
                    int height = size;

                    if (map[i,j] == 1)
                    {
                        g.FillRectangle(Brushes.Red, new Rectangle(x + 1, y + 1, width - 1, height - 1));
                    }
                    else if (map[i, j] == 2)
                    {
                        g.FillRectangle(Brushes.Yellow, new Rectangle(x + 1, y + 1, width - 1, height - 1));
                    }
                    else if (map[i, j] == 3)
                    {
                        g.FillRectangle(Brushes.Blue, new Rectangle(x + 1, y + 1, width - 1, height - 1));
                    }
                    else if (map[i, j] == 4)
                    {
                        g.FillRectangle(Brushes.Green, new Rectangle(x + 1, y + 1, width - 1, height - 1));
                    }
                    else if (map[i, j] == 5)
                    {
                        g.FillRectangle(Brushes.Pink, new Rectangle(x + 1, y + 1, width - 1, height - 1));
                    }
                }
            }
        }
        public void Slice()
        {
            for (int i = 0; i < 15; i++)
            {
                bool flag = true;
                
                for (int j = 0; j < 10; j++)
                {
                    if (map[i, j] == 0)
                    {
                        flag = false;
                    }
                    
                }
                if (flag)
                {
                    for (int k = i; k > 0; k--)
                    {
                        for (int t = 0; t < 10; t++)
                        {
                            map[k, t] = map[k - 1, t];
                        }
                    }
                }
            }
        }
        public bool IsIntersected()
        {
            for (int i = f1.y; i < f1.y + f1.sizeMatrix; i++)
            {
                for (int j = f1.x; j < f1.x + f1.sizeMatrix; j++)
                {
                    if (j >= 0 && j <= 9)
                    {
                        if (map[i,j] != 0 && f1.matrix[i-f1.y , j-f1.x] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool Collide()
        {
            for (int i = f1.y + f1.sizeMatrix - 1; i >= f1.y; i--)
            {
                for (int j = f1.x; j < f1.x + f1.sizeMatrix; j++)
                {
                    int a = i - f1.y;
                    int b = j - f1.x;
                        
                    if (f1.matrix[a,b] != 0)
                    {
                        if (i + 1 == 15)
                        {
                            return true;
                        }
                        if (map[i+1,j] != 0)
                        {
                            return true;
                        }
                    }
                    
                }
            }
            return false;
        }
        private void DrawMap1(Graphics g)    // !!!
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (f1.matrix[i,j] == 1)
                    {
                        int x = size * j;
                        int y = size * i;
                        int width = size;
                        int height = size;
                        g.FillRectangle(Brushes.Red, new Rectangle(x+1, y+1, width-1, height-1));
                    }
                }
            }
        }

        private void DrawGrid(Graphics myG)
        {
            int n = map.GetLength(0);
            int m = map.GetLength(1);

            for (int i = 0; i <= n; i++)
            {
                myG.DrawLine(Pens.Black, new Point(0, size * i), new Point(m * size, size * i));
            }
            for (int i = 0; i <= m; i++)
            {
                myG.DrawLine(Pens.Black, new Point(size * i, 0), new Point(size * i, n * size));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ResetArea();

            
            if (Collide() == false)
            {
                f1.y++;
            }
            else
            {
                Merge();
                Slice();
                timer1.Interval = 500;
                f1.ResetShape(3,0);
                Score += 10;
                label2.Text = Convert.ToString(Score);
                if (Collide())
                {
                    timer1.Stop();
                    string text = "Жаль, " + name + " но вы проиграли! \n";
                    text += "Ваш счёт: " + Score + "! \n";
                    text += "Хотите попробовать еще раз?";
                    DialogResult Dialog = MessageBox.Show(text,"Окончание игры", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (Dialog == DialogResult.Yes)
                    {
                    ClearMap();
                   
                        if (Score > HighScore)
                        {
                            StreamWriter HS_Write = new StreamWriter("HS.txt.txt");
                            HS_Write.WriteLine(Score);
                            HS_Write.Close();
                        }
                    Score = 0;
                    label2.Text = Convert.ToString(Score);
                        Init();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
               
               
            }

            Merge();
            Invalidate();    // draw
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                if (CollideHorizontal(1) == false)  // не столкнулся
                {
                    ResetArea();
                    f1.MoveRight();
                    Merge();
                    Invalidate();
                }   
            }

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                if (CollideHorizontal(-1) == false)  // не столкнулся
                {
                    ResetArea();
                    f1.MoveLeft();
                    Merge();
                    Invalidate();
                }
            }

            if (e.KeyCode == Keys.Q || e.KeyCode == Keys.Space)
            {
                if (IsIntersected() == false)
                {
                ResetArea();
                f1.RotateShape();
                Merge();
                Invalidate();
                }
                
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                timer1.Interval = 100;
            }
        }

        public void ResetArea()
        {
            for (int i = f1.y; i < f1.y + f1.sizeMatrix; i++)
            {
                for (int j = f1.x; j < f1.x + f1.sizeMatrix; j++)
                {
                    if (f1.matrix[i-f1.y,j-f1.x] != 0)    // если 1
                    {
                        map[i, j] = 0;
                    }
                }
            }
        }

        public bool CollideHorizontal(int direction)
        {
            // direction - направление 1 - направо / -1 - налево

            for (int i = f1.y; i < f1.y + f1.sizeMatrix; i++)
            {
                for (int j = f1.x; j < f1.x + f1.sizeMatrix; j++)
                {
                    if (f1.matrix[i - f1.y,j - f1.x] != 0)  // думаем как только 1
                    {
                        // Стенки
                        if (j + 1*direction > 10 - 1 || j + 1*direction < 0)
                        {
                            return true;
                        }
                        if (map[i,j + 1*direction] != 0)
                        {
                            if (j - f1.x + 1*direction >= f1.sizeMatrix || j - f1.x < 0)
                            {
                                return true;
                            }

                            if (f1.matrix[i - f1.y, j - f1.x + 1*direction] == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void паузаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                паузаToolStripMenuItem.Text = "Resume";
            }
            else
            {
                timer1.Start();
                паузаToolStripMenuItem.Text = "Pause";
            }
        }

        private void зановоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            ClearMap();
            Init();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "Добро пожаловать в Super Tetриц!\n\n";
            text += "Для цправления используй кнопки A/D(Left/Right) \n";
            text += "Для поворота тайла используй Q(Space) \n";
            text += "Для ускоренного спкска тайла используй S(Down) \n\n";
            text += "Удачи! \n";
            MessageBox.Show(text, "Info");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
