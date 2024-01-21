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

        public Timer timer = null;

        public int Width { get; set; }
        public int Height { get; set; }
        public UserModel User { get; set; }
        public List<Point> Positions { get; set; }
        public int Tick { get; set; }
        public int Iterator { get; set; }


        public FieldModel(int width, int height)
        {
            Width = width;
            Height = height;
            User = new UserModel(0, 0);
            Positions = new List<Point>();
        }
        
        public void FixPosition()
        {
            Positions.Add(new Point(User.XPosition, User.YPosition));
        }

        public void ClearPositions()
        {
            User = new UserModel(0, 0);
            Positions.Clear();
        }

        public void MoveUser(Direction direction, int steps)
        {
            if(steps < 0)
            {
                throw new Exception("Negative value detected!!!!!!!!!!!!!!!");
            }
            switch(direction)
            {
                case Direction.RIGHT:
                    for (int i = 0; i < steps; i++)
                    {
                        User.XPosition++;
                        if (IsWall()) { 
                            ThrowException();
                            return;
                        }
                        FixPosition();
                    }
                    
                    break;
                case Direction.LEFT:
                    for (int i = 0; i < steps; i++)
                    {
                        User.XPosition--;
                        if (IsWall())
                        {
                            ThrowException();
                            return;
                        }
                        FixPosition();
                    }
                    break;
                case Direction.UP:
                    for (int i = 0; i < steps; i++)
                    {
                        User.YPosition--;
                        if (IsWall())
                        {
                            ThrowException();
                            return;
                        }

                        FixPosition();
                    }
                    break;
                case Direction.DOWN:
                    for (int i = 0; i < steps; i++)
                    {
                        User.YPosition++;
                        if (IsWall())
                        {
                            ThrowException();
                            return;
                        }
                        FixPosition();

                    }
                    break;
            }
        }

        public void ThrowException()
        {
            throw new Exception("WALL!");
        }
       

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
