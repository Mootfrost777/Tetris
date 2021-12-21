using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTetris_v10._0
{
    class Shape
    {
        // Свойства
        public int x;
        public int y;
        public int[,] matrix;
        public int sizeMatrix;

        public int[,] nextMatrix;
        public int sizeNextMatrix;

        public int[,] figure1 = new int[3, 3]
        {
            {0,1,0 },
            {0,1,1 },
            {0,0,1 }
        };

        public int[,] figure2 = new int[3, 3]
        {
            {0,0,0 },
            {2,2,2 },
            {0,2,0 }
        };

        public int[,] figure3 = new int[2, 2]
        {
            {3,3 },
            {3,3 }
        };

        public int[,] figure4 = new int[4, 4]
        {
            {0,0,4,0 },
            {0,0,4,0 },
            {0,0,4,0 },
            {0,0,4,0 }
        };

        public int[,] figure5 = new int[3, 3]
        {
            {5,0,0 },
            {5,0,0 },
            {5,5,0 }
        };
        public int[,] figure6 = new int[3, 3]
       {
            {0,5,0 },
            {0,5,0 },
            {5,5,0 }
       };
        public int[,] figure7 = new int[3, 3]
        {
            {0,0,1 },
            {0,1,1 },
            {0,1,0 }
        };


        // Конструктор = какие свойства получит объект, когда он будет создан
        public Shape()
        {
            x = 0;
            y = 0;

            matrix = GenerateMatrix();
            sizeMatrix = matrix.GetLength(0);

            nextMatrix = GenerateMatrix();
            sizeNextMatrix = nextMatrix.GetLength(0);
        }

        public void ResetShape(int x, int y)
        {
            this.x = x;
            this.y = y;

            matrix = nextMatrix;
            sizeMatrix = sizeNextMatrix;

            nextMatrix = GenerateMatrix();
            sizeNextMatrix = nextMatrix.GetLength(0);
        }

        public int[,] GenerateMatrix()
        {
            int[,] localMatrix = figure1;
            Random r = new Random();
            int num = r.Next(1, 8);
            if (num == 1)
            {
                localMatrix = figure1;
            }
            else if (num == 2)
            {
                localMatrix = figure2;
            }
            else if (num == 3)
            {
                localMatrix = figure3;
            }
            else if (num == 4)
            {
                localMatrix = figure4;
            }
            else if (num == 5)
            {
                localMatrix = figure5;
            }
            else if (num == 6)
            {
                localMatrix = figure6;
            }
            else
            {
                localMatrix = figure7;
            }

            return localMatrix;
        }

        public void RotateShape()
        {
            int[,] tempMatrix = new int[sizeMatrix, sizeMatrix];

            for (int i = 0; i < sizeMatrix; i++)
            {
                for (int j = 0; j < sizeMatrix; j++)
                {
                    tempMatrix[i, j] = matrix[j, sizeMatrix - i-1];
                }
            }

            matrix = tempMatrix;

            int offset = 10 - (x + sizeMatrix);
            if (offset < 0)   // если фигура ушла правее
            {
                //  Отодвинуть левее фигуру, чтобы влезла
                for (int i = 0; i < offset*(-1); i++)
                {
                    MoveLeft();
                }
            }

            if (x < 0)  // если фигура ушла левее
            {
                for (int i = 0; i < x*(-1) + 1; i++)
                {
                    MoveRight();
                }
            }

        }

        // Методы 
        public void MoveRight()
        {
            x++;
        }

        public void MoveLeft()
        {
            x--;
        }

    }
}
