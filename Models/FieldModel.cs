﻿using LangLine.Enums;
using LangLine;
using LangLine.Exceptions;
using LangLine.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using Timer = System.Timers.Timer;

namespace LangLine.Models
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

        public LangLineCore Context;

        public FieldModel(LangLineCore context, int width, int height)
        {
            Width = width;
            Height = height;
            Context = context;
            User = new UserModel((int)Context.SpawnPoint.X, (int)Context.SpawnPoint.Y);
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
            User = new UserModel((int)Context.SpawnPoint.X, (int)Context.SpawnPoint.Y);
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
            if(steps > Context.MaxValueOfVariables)
            {
                var log = new ExceptionLog(Context.GetCurrentIndex(), new OutOfMaxVariablesValueException(Context.MaxValueOfVariables));
                Context.LogException(log);
            }

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

        /// <summary>
        /// Вызывает ошибку для трассировка стека 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ThrowException()
        {
            throw new Exception("Исполнитель упёрся в стенку!");
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
