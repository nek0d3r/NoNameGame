using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NoNameGame
{
    class TileMap
    {
        private List<List<Minerals>> map = new List<List<Minerals>>();

        private int world_Width;
        private int world_Height;

        Random rand = new Random();

        public TileMap(int tileWidth, int tileHeight)
        {
            world_Width = tileWidth;
            world_Height = tileHeight;

            for (int i = 0; i < world_Height; i++)
            {
                List<Minerals> newRow = new List<Minerals>();
                for (int j = 0; j < world_Width; j++)
                {
                    newRow.Add(Minerals.Dirt);
                }
                map.Add(newRow);
            }

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    /********************************************************************************************************************
                     * IMPORTANT: If you add a tile, you must do the following:
                     * 1) In the rand.Next(?) section, whatever number is inside, add 1 to it.
                     * 2) After the last case but before the default, add a case with the next number that contains map[y][x] = Minerals.?
                     * If you do not follow these steps correctly, who knows what arbitrary stuff is going to happen to you?
                     * ******************************************************************************************************************/
                    int i = rand.Next(5);
                    if (i == 1)
                    {
                        map[y][x] = Minerals.Grass;
                    }
                    else if (i == 2)
                    {
                        map[y][x] = Minerals.Coal;
                    }
                    else if (i == 3)
                    {
                        map[y][x] = Minerals.Iron;
                    }
                    else if (i == 4)
                    {
                        map[y][x] = Minerals.Diamond;
                    }
                    else
                    {
                        map[y][x] = Minerals.Dirt;
                    }
                }
            }
        }

        public void openMap(List<List<Minerals>> tile, int width, int height)
        {
            map = tile;
            world_Width =  width;
            world_Height = height;
        }

        public void expandNorth()
        {
            world_Height++;
            List<Minerals> newRow = new List<Minerals>();

            for (int x = 0; x < world_Width; x++)
            {
                int i = rand.Next(5);
                if (i == 1)
                {
                    newRow.Add(Minerals.Grass);
                }
                else if (i == 2)
                {
                    newRow.Add(Minerals.Coal);
                }
                else if (i == 3)
                {
                    newRow.Add(Minerals.Iron);
                }
                else if (i == 4)
                {
                    newRow.Add(Minerals.Diamond);
                }
                else
                {
                    newRow.Add(Minerals.Dirt);
                }
            }
            map.Insert(0, newRow);
        }

        public void expandEast()
        {
            world_Width++;
            for (int x = 0; x < world_Height; x++)
            {
                int i = rand.Next(5);
                if (i == 1)
                {
                    map[x].Add(Minerals.Grass);
                }
                else if (i == 2)
                {
                    map[x].Add(Minerals.Coal);
                }
                else if (i == 3)
                {
                    map[x].Add(Minerals.Iron);
                }
                else if (i == 4)
                {
                    map[x].Add(Minerals.Diamond);
                }
                else
                {
                    map[x].Add(Minerals.Dirt);
                }
            }
        }

        public void expandWest()
        {
            world_Width++;
            for (int x = 0; x < world_Height; x++)
            {
                int i = rand.Next(5);
                if (i == 1)
                {
                    map[x].Insert(0, Minerals.Grass);
                }
                else if (i == 2)
                {
                    map[x].Insert(0, Minerals.Coal);
                }
                else if (i == 3)
                {
                    map[x].Insert(0, Minerals.Iron);
                }
                else if (i == 4)
                {
                    map[x].Insert(0, Minerals.Diamond);
                }
                else
                {
                    map[x].Insert(0, Minerals.Dirt);
                }
            }
        }

        public void expandSouth()
        {
            world_Height++;
            List<Minerals> newRow = new List<Minerals>();

            for (int x = 0; x < world_Width; x++)
            {
                int i = rand.Next(5);
                if (i == 1)
                {
                    newRow.Add(Minerals.Grass);
                }
                else if (i == 2)
                {
                    newRow.Add(Minerals.Coal);
                }
                else if (i == 3)
                {
                    newRow.Add(Minerals.Iron);
                }
                else if (i == 4)
                {
                    newRow.Add(Minerals.Diamond);
                }
                else
                {
                    newRow.Add(Minerals.Dirt);
                }
            }
            map.Add(newRow);
        }

        public void mineTile(int x, int y)
        {
            map[y][x] = Minerals.Dirt;
        }

        public int worldWidth()
        {
            return world_Width;
        }

        public int worldHeight()
        {
            return world_Height;
        }

        public Minerals tileAt(int x, int y)
        {
            if (x >= 0 && x < world_Width && y >= 0 && y < world_Height)
            {
                return map[y][x];
            }
            else
            {
                return Minerals.Dirt;
            }
        }
    }
}