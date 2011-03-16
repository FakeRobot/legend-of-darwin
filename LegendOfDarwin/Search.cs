using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin
{
    class Search
    {
        GameBoard board;

        //used for pseudo-astar
        private Vector2[] aStarSolution;
        private int curSpot;
        private bool[,] hasBeenChecked;
        private bool foundSolution;

        public Search(GameBoard myboard) 
        {
            board = myboard;
            foundSolution = true;
        }


        /**
         * Plans a path from one point on the board to another
         * startX, startY are starting point dimensions
         * goalX, goalY are ending point dimensions
         * Make sure both points are within board bounds
         * returns array of points which represents path
         * This is not a true a-star, really more of a pseudo-astar
         * */
        public Vector2[] aStar(int startX, int startY, int goalX, int goalY)
        {
            aStarSolution = new Vector2[100];
            curSpot = 0;
            int curSpotX = startX;
            int curSpotY = startY;

            hasBeenChecked = new bool[board.getNumSquaresX(), board.getNumSquaresY()];

            for (int i = 0; i < hasBeenChecked.GetLength(0); i++)
            {
                for (int j = 0; j < hasBeenChecked.GetLength(1); j++)
                {
                    hasBeenChecked[i, j] = false;
                }
            }

            hasBeenChecked[curSpotX, curSpotY] = true;

            double costSoFar = 0;
            //double costToPoint = Math.Sqrt(Math.Pow((double)(goalX-curSpotX),2.0) + Math.Pow((double)(goalY-curSpotY),2.0));
            double costToPoint = Math.Abs(goalX - curSpotX) + Math.Abs(goalY - curSpotY);

            aStarHelper(new Vector2(curSpotX, curSpotY), new Vector2(goalX, goalY), costSoFar);
            return aStarSolution;
        }

        // used for recursion
        private void aStarHelper(Vector2 curPt, Vector2 goalPt, double costSoFar)
        {
            Vector2 minCostPt = new Vector2(100000, 100000);
            double minCost = 7000000000;
            double curCostEst = 70000000;
            bool foundGoal = false;

            //check all neighbours to curPt
            if (board.isGridPositionOpen((int)curPt.X + 1, (int)curPt.Y) && !hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y])
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - (curPt.X + 1)) + Math.Abs(goalPt.Y - curPt.Y));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X + 1, curPt.Y);
                }

                if (goalPt.X == curPt.X + 1 && goalPt.Y == curPt.Y)
                    foundGoal = true;
                hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y] = true;
            }
            if (board.isGridPositionOpen((int)curPt.X - 1, (int)curPt.Y) && !hasBeenChecked[(int)curPt.X - 1, (int)curPt.Y])
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - (curPt.X - 1)) + Math.Abs(goalPt.Y - curPt.Y));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X - 1, curPt.Y);
                }

                if (goalPt.X == curPt.X - 1 && goalPt.Y == curPt.Y)
                    foundGoal = true;
                hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y] = true;
            }
            if (board.isGridPositionOpen((int)curPt.X, (int)curPt.Y + 1) && !hasBeenChecked[(int)curPt.X, (int)curPt.Y + 1])
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - curPt.X) + Math.Abs(goalPt.Y - (curPt.Y + 1)));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X, curPt.Y + 1);
                }

                if (goalPt.X == curPt.X && goalPt.Y == curPt.Y + 1)
                    foundGoal = true;

                hasBeenChecked[(int)curPt.X, (int)curPt.Y + 1] = true;
            }
            if (board.isGridPositionOpen((int)curPt.X, (int)curPt.Y - 1) && !hasBeenChecked[(int)curPt.X, (int)curPt.Y - 1])
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - curPt.X) + Math.Abs(goalPt.Y - (curPt.Y - 1)));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X, curPt.Y - 1);
                }

                if (goalPt.X == curPt.X && goalPt.Y == curPt.Y - 1)
                    foundGoal = true;

                hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y] = true;
            }

            if (foundGoal)
            {
                aStarSolution[curSpot] = goalPt;
            }
            else if (minCostPt.X == 100000 || curSpot > 90)
            {
                //abort path cannot be found
                foundSolution = false;

            }
            else
            {
                //Console.Out.WriteLine("{0},{1},{2},{3}",curSpot,minCostPt.X,minCostPt.Y,costSoFar);
                aStarSolution[curSpot] = minCostPt;
                curSpot++;
                aStarHelper(minCostPt, goalPt, costSoFar + 1);
            }
        }

        public bool isSolution() 
        {
            return foundSolution;
        }

        public int getLength() 
        {
            return curSpot+1;
        }

    }
}
