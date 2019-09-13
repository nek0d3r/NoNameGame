using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NoNameGame
{
    class Ranges
    {
        private int coorX;
        private int coorY;
        private int range;

        public Ranges()
        {
            coorX = 0;
            coorY = 0;
            range = 0;
        }

        public void setRange(int k)
        {
            range = k;
        }

        public void setCoordinates(int x, int y)
        {
            coorX = x;
            coorY = y;
        }

        public void moveX(int x)
        {
            coorX += x;
        }

        public void moveY(int y)
        {
            coorY += y;
        }

        public bool withinRange(int x, int y)
        {
            if (Math.Abs(coorX - x) + Math.Abs(coorY - y) <= range)
            {
                return true;
            }
            return false;
        }
    }
}