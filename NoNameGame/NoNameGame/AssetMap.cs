using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoNameGame
{
    class AssetMap
    {
        private List<List<Assets>> map = new List<List<Assets>>();

        private int world_Width;
        private int world_Height;

        Random rand = new Random();

        public AssetMap(int tileWidth, int tileHeight)
        {
            world_Width = tileWidth;
            world_Height = tileHeight;

            for (int i = 0; i < world_Height; i++)
            {
                List<Assets> newRow = new List<Assets>();
                for (int j = 0; j < world_Width; j++)
                {
                    newRow.Add(Assets.Null);
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
                    int i = rand.Next(100);

                    if (i <= 9)
                    {
                        map[y][x] = Assets.Tree;
                    }
                    else if (i >= 10 && i <= 19)
                    {
                        map[y][x] = Assets.Mountain;
                    }
                    else if (i >= 20 && i <= 29)
                    {
                        map[y][x] = Assets.Castle;
                    }
                    else
                    {
                        map[y][x] = Assets.Null;
                    }
                }
            }
        }

        public void openMap(List<List<Assets>> asset, int width, int height)
        {
            map = asset;
            world_Width = width;
            world_Height = height;
        }

        public void expandNorth()
        {
            world_Height++;
            List<Assets> newRow = new List<Assets>();

            for (int x = 0; x < world_Width; x++)
            {
                int i = rand.Next(100);
                if (i <= 9)
                {
                    newRow.Add(Assets.Tree);
                }
                else if (i >= 10 && i <= 19)
                {
                    newRow.Add(Assets.Mountain);
                }
                else if (i >= 20 && i <= 29)
                {
                    newRow.Add(Assets.Castle);
                }
                else
                {
                    newRow.Add(Assets.Null);
                }
            }
            map.Insert(0, newRow);
        }

        public void expandEast()
        {
            world_Width++;
            for (int x = 0; x < world_Height; x++)
            {
                int i = rand.Next(100);
                if (i <= 9)
                {
                    map[x].Add(Assets.Tree);
                }
                else if (i >= 10 && i <= 19)
                {
                    map[x].Add(Assets.Mountain);
                }
                else if (i >= 20 && i <= 29)
                {
                    map[x].Add(Assets.Castle);
                }
                else
                {
                    map[x].Add(Assets.Null);
                }
            }
        }

        public void expandWest()
        {
            world_Width++;
            for (int x = 0; x < world_Height; x++)
            {
                int i = rand.Next(100);
                if (i <= 9)
                {
                    map[x].Insert(0, Assets.Tree);
                }
                else if (i >= 10 && i <= 19)
                {
                    map[x].Insert(0, Assets.Mountain);
                }
                else if (i >= 20 && i <= 29)
                {
                    map[x].Insert(0, Assets.Castle);
                }
                else
                {
                    map[x].Insert(0, Assets.Null);
                }
            }
        }

        public void expandSouth()
        {
            world_Height++;
            List<Assets> newRow = new List<Assets>();

            for (int x = 0; x < world_Width; x++)
            {
                int i = rand.Next(100);
                if (i <= 9)
                {
                    newRow.Add(Assets.Tree);
                }
                else if (i >= 10 && i <= 19)
                {
                    newRow.Add(Assets.Mountain);
                }
                else if (i >= 20 && i <= 29)
                {
                    newRow.Add(Assets.Castle);
                }
                else
                {
                    newRow.Add(Assets.Null);
                }
            }
            map.Add(newRow);
        }

        public void killAsset(int x, int y)
        {
            map[y][x] = Assets.Null;
        }

        public int worldWidth()
        {
            return world_Width;
        }

        public int worldHeight()
        {
            return world_Height;
        }

        public Assets assetAt(int x, int y)
        {
            if (x >= 0 && x < world_Width && y >= 0 && y < world_Height)
            {
                return map[y][x];
            }
            else
            {
                return Assets.Null;
            }
        }

        public void buildAsset(int x, int y, Assets z)
        {
            map[y][x] = z;
        }
    }
}
