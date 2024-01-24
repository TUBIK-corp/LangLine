using IspolnitelCherepashka.Enums;
using System;
using System.Collections.Generic;
using System.Windows;
using Timer = System.Timers.Timer;

namespace IspolnitelCherepashka.Models
{
    public class FieldModel
    { 
        public class UserModel
        {

            public int XPosition { get; set; }
            public int YPosition { get; set; }

            public UserModel(int xPosition, int yPosition)
            {
                XPosition = xPosition;
                YPosition = yPosition;
            }
        }


        public int Width { get; set; }
        public int Height { get; set; }
        public UserModel User { get; set; }
        public List<Point> Positions { get; set; }


        public FieldModel(int width, int height)
        {
            Width = width;
            Height = height;
            User = new UserModel(0, 0);
            Positions = new List<Point>();
        }

        public List<Point> GetPositions() => Positions;
        
        /// <summary>
        /// Функция фиксации текущего расположения.
        /// </summary>
        public void FixPosition()
        {
            Positions.Add(new Point(User.XPosition, User.YPosition));
        }

        /// <summary>
        /// Очищает позиции и возвращает в начало.
        /// </summary>
        public void ClearPositions()
        {
            User = new UserModel(0, 0);
            Positions.Clear();
        }

        /// <summary>
        /// Делает шаг в сторону указанного направления.
        /// </summary>
        /// <param name="direction">Направление.</param>
        public void UpdatePosition(Direction direction)
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    User.XPosition++;
                    break;
                case Direction.LEFT:
                    User.XPosition--;
                    break;
                case Direction.UP:
                    User.YPosition--;
                    break;
                case Direction.DOWN:
                    User.YPosition++;
                    break;
            }
        }

        /// <summary>
        /// Функция запуска пошагового движения в указанном направлении с указанным количеством шагов. 
        /// </summary>
        /// <param name="direction">Направление.</param>
        /// <param name="steps">Количество шагов.</param>
        /// <exception cref="Exception"></exception>
        public void MoveUser(Direction direction, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                UpdatePosition(direction);

                if (IsWall())
                {
                    ThrowException();
                    return;
                }
                FixPosition();
            }
        }

        public void ThrowException()
        {
            throw new Exception("WALL!");
        }
       

        /// <summary>
        /// Проверка на нахождение в стене
        /// </summary>
        /// <returns></returns>
        public bool IsWall()
        {
            if (User.XPosition >= Width || User.XPosition < 0 || User.YPosition >= Height || User.YPosition < 0)
                return true;
            else return false;
        }

        public bool IsWallRight() =>
            User.XPosition+1 >= Width;
        public bool IsWallLeft() =>
            User.XPosition-1 < 0;
        public bool IsWallUp() =>
            User.YPosition-1 < 0;
        public bool IsWallDown() =>
            User.YPosition+1 >= Height;
    }
}
