using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Enums
{
    public static class DirectionParser
    {
        public static Direction ParseDirection(string stroke)
        {
            stroke = stroke.ToUpper();
            switch (stroke)
            {
                case "RIGHT":
                    return Direction.RIGHT;
                case "LEFT":
                    return Direction.LEFT;
                case "UP":
                    return Direction.UP;
                case "DOWN":
                    return Direction.DOWN;
                default:
                    return Direction.NOT_DEFINED;

            }
        }
    }
    public enum Direction
    {
        NOT_DEFINED = -1,
        RIGHT = 0,
        UP = 1,
        LEFT = 2,
        DOWN = 3
    }
}
