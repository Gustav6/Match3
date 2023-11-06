using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Match3
{
    public static class PlayingField
    {
        static readonly PlayingFieldInteractions interaction = new();
        static readonly PlayingFieldActions action = new();

        public static void Update()
        {
            PlayingFieldActions.CheckUnderGem();
            interaction.MoveSelectedGem();
        }


        public static void ClearMatches()
        {
            List<Point[]> verticalMatches = CheckVertical();
            List<Point[]> horizontalMatches = CheckHorizontal();

            foreach (Point[] points in horizontalMatches)
            {
                foreach (Point point in points)
                {
                    Data.tileMap[point.X, point.Y].gem = null;
                }
            }
            foreach (Point[] points in verticalMatches)
            {
                foreach (Point point in points)
                {
                    Data.tileMap[point.X, point.Y].gem = null;
                }
            }

            //action.SpawnNewGems();
            PlayingFieldActions.MoveGemsDown();
        }

        public static List<Point[]> CheckVertical()
        {
            Gem currentGem = null;
            List<Point[]> totalMatches = new();
            List<Point> tempMatches = new();

            for (int x = 0; x < Data.tileMap.GetLength(0); x++)
            {
                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();

                for (int y = 0; y < Data.tileMap.GetLength(0); y++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.texutre == Data.tileMap[x, y].gem.texutre && currentGem.position.X == Data.tileMap[x, y].gem.position.X)
                    {
                        tempMatches.Add(new Point(x, y));
                    }
                    else if (tempMatches.Count >= 3)
                    {
                        totalMatches.Add(tempMatches.ToArray());
                        tempMatches.Clear();
                    }
                    else
                    {
                        tempMatches.Clear();
                        if (Data.tileMap[x, y].gem != null)
                        {
                            currentGem = Data.tileMap[x, y].gem;
                        }
                        else if (!Data.tileMap[x, y].canHaveGem)
                        {
                            currentGem = null;
                        }
                        tempMatches.Add(new Point(x, y));
                    }
                }
            }

            return totalMatches;
        }

        public static List<Point[]> CheckHorizontal()
        {
            Gem currentGem = null;
            List<Point[]> totalMatches = new();
            List<Point> tempMatches = new();

            for (int y = 0; y < Data.tileMap.GetLength(0); y++)
            {
                if (tempMatches.Count >= 3)
                {
                    totalMatches.Add(tempMatches.ToArray());
                }

                tempMatches.Clear();

                for (int x = 0; x < Data.tileMap.GetLength(0); x++)
                {
                    if (currentGem != null && Data.tileMap[x, y].gem != null && currentGem.texutre == Data.tileMap[x, y].gem.texutre && currentGem.position.Y == Data.tileMap[x, y].gem.position.Y)
                    {
                        tempMatches.Add(new Point(x, y));
                    }
                    else if (tempMatches.Count >= 3)
                    {
                        totalMatches.Add(tempMatches.ToArray());
                        tempMatches.Clear();
                    }
                    else
                    {
                        tempMatches.Clear();
                        if (Data.tileMap[x, y].gem != null)
                        {
                            currentGem = Data.tileMap[x, y].gem;
                        }
                        else if (!Data.tileMap[x, y].canHaveGem)
                        {
                            currentGem = null;
                        }
                        tempMatches.Add(new Point(x, y));
                    }
                }
            }

            return totalMatches;
        }
    }
}
