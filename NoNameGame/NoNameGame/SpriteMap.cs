using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NoNameGame
{
    class SpriteMap
    {
        private List<List<Sprites>> map = new List<List<Sprites>>();
        private List<List<List<int>>> stats = new List<List<List<int>>>();
        
        private int world_Width = 25;
        private int world_Height = 12;

        private Vector2 startCoordinates = new Vector2(0, 0);
        private Vector2 endCoordinates = new Vector2(0, 0);

        private TileMap tileEngine;
        private AssetMap assetEngine;

        /*************************************************************
         * This is the length of the third dimension in the stats List.
         * **********************************************************/
        private const int statAmt = 4;

        public SpriteMap(int tileWidth, int tileHeight)
        {
            world_Width = tileWidth;
            world_Height = tileHeight;
        }

        public void loadMap(TileMap tile, AssetMap asset)
        {
            tileEngine = tile;
            assetEngine = asset;
        }

        public void newMap()
        {
            for(int i = 0; i < world_Height; i++)
            {
                List<Sprites> newRow = new List<Sprites>();
                for(int j = 0; j < world_Width; j++)
                {
                    newRow.Add(Sprites.Null);
                }
                map.Add(newRow);
            }

            Random rand = new Random();

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    if (assetEngine.assetAt(x, y) == Assets.Null)
                    {
                        /********************************************************************************************************************
                         * IMPORTANT: If you add a sprite, you must do the following:
                         * 1) In the rand.Next(?) section, whatever number is inside, add 1 to it.
                         * 2) After the last case but before the default, add a case with the next number that contains map[y][x] = Sprites.?
                         * If you do not follow these steps correctly, who knows what arbitrary stuff is going to happen to you?
                         * ******************************************************************************************************************/
                        int i = rand.Next(4);
                        switch (i)
                        {
                            case 1:
                                map[y][x] = Sprites.SwordsMan;
                                break;
                            case 2:
                                map[y][x] = Sprites.AxeMan;
                                break;
                            case 3:
                                map[y][x] = Sprites.Knight;
                                break;
                            default:
                                map[y][x] = Sprites.Null;
                                break;
                        }
                    }
                    else
                    {
                        map[y][x] = Sprites.Null;
                    }
                }
            }

            for (int y = 0; y < world_Height; y++)
            {
                List<List<int>> newRow = new List<List<int>>();
                for (int x = 0; x < world_Width; x++)
                {
                    List<int> newNewRow = new List<int>();
                    for (int z = 0; z < statAmt; z++)
                    {
                        switch (map[y][x])
                        {
                            case Sprites.SwordsMan:
                                newNewRow.Add(rand.Next(2));
                                newNewRow.Add(500);
                                newNewRow.Add(350);
                                newNewRow.Add(410);
                                break;
                            case Sprites.AxeMan:
                                newNewRow.Add(rand.Next(2));
                                newNewRow.Add(420);
                                newNewRow.Add(400);
                                newNewRow.Add(450);
                                break;
                            case Sprites.Knight:
                                newNewRow.Add(rand.Next(2));
                                newNewRow.Add(350);
                                newNewRow.Add(450);
                                newNewRow.Add(400);
                                break;
                            default:
                                newNewRow.Add(0);
                                newNewRow.Add(0);
                                newNewRow.Add(0);
                                newNewRow.Add(0);
                                break;
                        }
                        newNewRow.Add(0);
                    }
                    newRow.Add(newNewRow);
                }
                stats.Add(newRow);
            }
        }

        public void openMap(List<List<Sprites>> sprite, int width, int height)
        {
            map = sprite;
            world_Width = width;
            world_Height = height;
        }

        public void openStats(List<List<List<int>>> stat)
        {
            stats = stat;
        }

        public int countSprite(Sprites c)
        {
            int count = 0;
            for (int y = 0; y < world_Height; y++)
            {
                for (int x = 0; x < world_Width; x++)
                {
                    if (map[y][x] == c && stats[y][x][0] == 0)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void expandNorth()
        {
            Random rand = new Random();
            world_Height++;
            List<Sprites> newRow = new List<Sprites>();
            List<List<int>> statNewRow = new List<List<int>>();
            for (int x = 0; x < world_Width; x++)
            {
                if (assetEngine.assetAt(x, 0) == Assets.Null)
                {
                    /********************************************************************************************************************
                     * IMPORTANT: If you add a sprite, you must do the following:
                     * 1) In the rand.Next(?) section, whatever number is inside, add 1 to it.
                     * 2) After the last case but before the default, add a case with the next number that contains map[y][x] = Sprites.?
                     * If you do not follow these steps correctly, who knows what arbitrary stuff is going to happen to you?
                     * ******************************************************************************************************************/
                    int i = rand.Next(4);
                    switch (i)
                    {
                        case 1:
                            newRow.Add(Sprites.SwordsMan);
                            break;
                        case 2:
                            newRow.Add(Sprites.AxeMan);
                            break;
                        case 3:
                            newRow.Add(Sprites.Knight);
                            break;
                        default:
                            newRow.Add(Sprites.Null);
                            break;
                    }
                }
                else
                {
                    newRow.Add(Sprites.Null);
                }
                for (int y = 0; y < world_Width; y++)
                {
                    List<int> statNewNewRow = new List<int>();
                    for (int z = 0; z < statAmt; z++)
                    {
                        if (newRow[x] == Sprites.SwordsMan)
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(500);
                            statNewNewRow.Add(350);
                            statNewNewRow.Add(410);
                        }
                        else if (newRow[x] == Sprites.AxeMan)
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(420);
                            statNewNewRow.Add(400);
                            statNewNewRow.Add(450);
                        }
                        else if (newRow[x] == Sprites.Knight)
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(350);
                            statNewNewRow.Add(450);
                            statNewNewRow.Add(400);
                        }
                        else
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(0);
                            statNewNewRow.Add(0);
                            statNewNewRow.Add(0);
                        }
                    }
                    statNewRow.Add(statNewNewRow);
                }
            }
            map.Insert(0, newRow);
            stats.Insert(0, statNewRow);
        }

        public void expandEast()
        {
            Random rand = new Random();
            List<int> statNewRow = new List<int>();
            world_Width++;
            for (int y = 0; y < world_Height; y++)
            {
                int i = rand.Next(4);
                switch (i)
                {
                    case 1:
                        map[y].Add(Sprites.SwordsMan);
                        statNewRow.Add(1);
                        statNewRow.Add(500);
                        statNewRow.Add(350);
                        statNewRow.Add(410);
                        stats[y].Add(statNewRow);
                        break;
                    case 2:
                        map[y].Add(Sprites.AxeMan);
                        statNewRow.Add(1);
                        statNewRow.Add(420);
                        statNewRow.Add(400);
                        statNewRow.Add(450);
                        stats[y].Add(statNewRow);
                        break;
                    case 3:
                        map[y].Add(Sprites.Knight);
                        statNewRow.Add(1);
                        statNewRow.Add(350);
                        statNewRow.Add(450);
                        statNewRow.Add(400);
                        stats[y].Add(statNewRow);
                        break;
                    default:
                        map[y].Add(Sprites.Null);
                        statNewRow.Add(1);
                        statNewRow.Add(0);
                        statNewRow.Add(0);
                        statNewRow.Add(0);
                        stats[y].Add(statNewRow);
                        break;
                }

            }
        }

        public void expandWest()
        {
            Random rand = new Random();
            List<int> statNewRow = new List<int>();
            world_Width++;
            for (int y = 0; y < world_Height; y++)
            {
                int i = rand.Next(4);
                switch (i)
                {
                    case 1:
                        map[y].Insert(0, Sprites.SwordsMan);
                        statNewRow.Add(1);
                        statNewRow.Add(500);
                        statNewRow.Add(350);
                        statNewRow.Add(410);
                        stats[y].Insert(0, statNewRow);
                        break;
                    case 2:
                        map[y].Insert(0, Sprites.AxeMan);
                        statNewRow.Add(1);
                        statNewRow.Add(420);
                        statNewRow.Add(400);
                        statNewRow.Add(450);
                        stats[y].Insert(0, statNewRow);
                        break;
                    case 3:
                        map[y].Insert(0, Sprites.Knight);
                        statNewRow.Add(1);
                        statNewRow.Add(350);
                        statNewRow.Add(450);
                        statNewRow.Add(400);
                        stats[y].Insert(0, statNewRow);
                        break;
                    default:
                        map[y].Insert(0, Sprites.Null);
                        statNewRow.Add(1);
                        statNewRow.Add(0);
                        statNewRow.Add(0);
                        statNewRow.Add(0);
                        stats[y].Insert(0, statNewRow);
                        break;
                }

            }
        }

        public void expandSouth()
        {
            Random rand = new Random();
            world_Height++;
            List<Sprites> newRow = new List<Sprites>();
            List<List<int>> statNewRow = new List<List<int>>();
            for (int x = 0; x < world_Width; x++)
            {
                if (assetEngine.assetAt(x, 0) == Assets.Null)
                {
                    /********************************************************************************************************************
                     * IMPORTANT: If you add a sprite, you must do the following:
                     * 1) In the rand.Next(?) section, whatever number is inside, add 1 to it.
                     * 2) After the last case but before the default, add a case with the next number that contains map[y][x] = Sprites.?
                     * If you do not follow these steps correctly, who knows what arbitrary stuff is going to happen to you?
                     * ******************************************************************************************************************/
                    int i = rand.Next(4);
                    switch (i)
                    {
                        case 1:
                            newRow.Add(Sprites.SwordsMan);
                            break;
                        case 2:
                            newRow.Add(Sprites.AxeMan);
                            break;
                        case 3:
                            newRow.Add(Sprites.Knight);
                            break;
                        default:
                            newRow.Add(Sprites.Null);
                            break;
                    }
                }
                else
                {
                    newRow.Add(Sprites.Null);
                }
                for (int y = 0; y < world_Width; y++)
                {
                    List<int> statNewNewRow = new List<int>();
                    for (int z = 0; z < statAmt; z++)
                    {
                        if (newRow[x] == Sprites.SwordsMan)
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(500);
                            statNewNewRow.Add(350);
                            statNewNewRow.Add(410);
                        }
                        else if (newRow[x] == Sprites.AxeMan)
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(420);
                            statNewNewRow.Add(400);
                            statNewNewRow.Add(450);
                        }
                        else if (newRow[x] == Sprites.Knight)
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(350);
                            statNewNewRow.Add(450);
                            statNewNewRow.Add(400);
                        }
                        else
                        {
                            statNewNewRow.Add(1);
                            statNewNewRow.Add(0);
                            statNewNewRow.Add(0);
                            statNewNewRow.Add(0);
                        }
                    }
                    statNewRow.Add(statNewNewRow);
                }
            }
            map.Add(newRow);
            stats.Add(statNewRow);
        }

        public int worldWidth()
        {
            return world_Width;
        }

        public int worldHeight()
        {
            return world_Height;
        }

        public Sprites spriteAt(int x, int y)
        {
            if (x >= 0 && x < world_Width && y >= 0 && y < world_Height)
            {
                return map[y][x];
            }
            else
            {
                return Sprites.Null;
            }
        }

        public int getStat(int x, int y, int z)
        {
            if (x >= 0 && x < world_Width && y >= 0 && y < world_Height)
            {
                return stats[y][x][z];
            }
            else
            {
                return 0;
            }
        }

        public void buildSprite(int x, int y, Sprites sprite, int z)
        {
            map[y][x] = sprite;
            stats[y][x][0] = z;
            switch (sprite)
            {
                case Sprites.SwordsMan:
                    stats[y][x][1] = 500;
                    stats[y][x][2] = 350;
                    stats[y][x][3] = 410;
                    break;
                case Sprites.AxeMan:
                    stats[y][x][1] = 420;
                    stats[y][x][2] = 400;
                    stats[y][x][3] = 450;
                    break;
                case Sprites.Knight:
                    stats[y][x][1] = 350;
                    stats[y][x][2] = 450;
                    stats[y][x][3] = 400;
                    break;
            }
        }

        public void startMove(int x, int y)
        {
            startCoordinates.X = x;
            startCoordinates.Y = y;
        }

        public void moveSprite(int x, int y)
        {
            if (x >= 0 && x <= world_Width && y >= 0 && y <= world_Height)
            {
                endCoordinates.X = x;
                endCoordinates.Y = y;

                map[(int)endCoordinates.Y][(int)endCoordinates.X] = map[(int)startCoordinates.Y][(int)startCoordinates.X];

                map[(int)startCoordinates.Y][(int)startCoordinates.X] = 0;
            }
        }

        public void attackSprite(int x, int y, int xx, int yy)
        {
            if (xx >= 0 && xx <= world_Width && yy >= 0 && yy <= world_Height)
            {
                stats[yy][xx][1] -= stats[y][x][2];
                if (stats[yy][xx][1] <= 0)
                {
                    map[yy][xx] = 0;
                    stats[yy][xx][1] = 0;
                    stats[yy][xx][2] = 0;
                    stats[yy][xx][3] = 0;
                }
            }
        }
    }
}