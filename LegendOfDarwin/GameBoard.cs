﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfDarwin
{
    public class GameBoard
    {
        // Length and width of each square/rectangle
        private int squareLength, squareWidth;

        // 2d array representation of the game floor. 
        // Each rectangle upon each grid position contains the necessary information for each tile to be placed on
        private Rectangle[,] grid;

        // A flag of each grid to see if an object occupies it
        private Boolean[,] hasObject;

        // Number of squares going down
        private int gridLength;

        // Number of squares going right
        private int gridWidth;

        // The background, has the same representation compared to the grid and hasObject vars
        private Texture2D[,] background;

        /*
         * boardSize contains the x and y length and width of the whole gameboard.
         * boardSize.X = 25 and boardSize.Y = 25 would translate to a 25 X 25 board
         * windowSize contains the x and y length and width of the game's window's size.
         * windowSize.X = 900 and windowSize.Y = 600 would translate to a 900 X 600 window size in pixels
         */
        public GameBoard(Vector2 boardSize, Vector2 windowSize)
        {
            squareLength = (int)(windowSize.Y / boardSize.Y);
            squareWidth = (int)(windowSize.X / boardSize.X);

            gridLength = (int)boardSize.Y;
            gridWidth = (int)boardSize.X;

            grid = new Rectangle[(int)boardSize.X, (int)boardSize.Y];
            hasObject = new Boolean[(int)boardSize.X, (int)boardSize.Y];
            background = new Texture2D[(int)boardSize.X, (int)boardSize.Y];

            for (int i = 0; i < boardSize.X; i++)
            {
                for (int j = 0; j < boardSize.Y; j++)
                {
                    grid[i,j] = new Rectangle(i * squareWidth, j * squareLength, squareWidth, squareLength);
                    hasObject[i,j] = false;
                    background[i, j] = null;
                }
            }
        }


        /*
         * x is the width or 'going right' position on the grid
         * y is the length or 'going down' position on the grid
         * x = 3, y = 5 would translate to 3 to the right and 5 down
         * Both start at 0
         */ 
        public Boolean isTaken(int x, int y)
        {
            if (x < 0 || x > gridWidth)
            {
                return false;
            }
            if (y < 0 || y > gridLength)
            {
                return false;
            }
            return hasObject[x,y];
        }

        /*
         * bo contains the coordinates on the grid to be checked to see if its opened or not
         * returns true on open, false on not
         */ 
        public Boolean isGridPositionOpen(BasicObject bo)
        {
            if (isTaken(bo.X, bo.Y))
            {
                return false;
            }
            return true;
        }

        public Boolean isGridPositionOpen(int x, int y)
        { 
            if(isTaken(x, y))
            {
                return false;
            }
            return true;
        }

        /*
         * PreCon: The bo's coordinates have the actually position using the function isGridPositionOpen
         * Gets the rectangle based on the bo's coordinates on the grid
         */ 
        public Rectangle getPosition(BasicObject bo)
        {
            return grid[bo.X,bo.Y];
        }
        public Rectangle getPosition(int x, int y)
        {
            return grid[x, y];
        }

        public void setGridPositionOpen(int x, int y)
        {
            if ((x > 0 && x < gridWidth) && (y > 0 && y < gridLength))
            {
                hasObject[x, y] = false;
            }
        }
        public void setGridPositionOpen(BasicObject bo)
        {
            if ((bo.X > 0 && bo.X < gridWidth) && (bo.Y > 0 && bo.Y < gridLength))
            {
                hasObject[bo.X, bo.Y] = false;
            }
        }

        public void setGridPositionOccupied(int x, int y)
        {
            if( (x>0 && x<gridWidth) && (y>0 && y<gridLength) )
            {
                hasObject[x, y] = true;
            }
        }
        public void setGridPositionOccupied(BasicObject bo)
        {
            if ((bo.X > 0 && bo.X < gridWidth) && (bo.Y > 0 && bo.Y < gridLength))
            {
                hasObject[bo.X, bo.Y] = true;
            }
        }

        public int getSquareLength()
        {
            return this.squareLength;
        }

        public int getSquareWidth()
        {
            return this.squareWidth;
        }

        // get total number of squares on x direction
        public int getNumSquaresX() 
        {
            return grid.GetLength(0);
        }

        // ge total number of squares in the y direction
        public int getNumSquaresY()
        {
            return grid.GetLength(1);
        }

        //used for astar
        private Vector2[] aStarSolution;
        private int curSpot;
        private bool[,] hasBeenChecked; 

        /**
         * Plans a path from one point on the board to another
         * startX, startY are starting point dimensions
         * goalX, goalY are ending point dimensions
         * Make sure both points are within board bounds
         * returns array of points which represents path
         * */
        public Vector2[] aStar(int startX,int startY,int goalX,int goalY)
        {
            aStarSolution = new Vector2[100];
            curSpot = 0;
            int curSpotX = startX;
            int curSpotY = startY;

            hasBeenChecked = new bool[this.getNumSquaresX(), this.getNumSquaresY()];
            
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
        private void aStarHelper(Vector2 curPt, Vector2 goalPt,double costSoFar) 
        {
            Vector2 minCostPt = new Vector2(100000,100000);
            double minCost = 7000000000;
            double curCostEst = 70000000;
            bool foundGoal = false;

            //check all neighbours to curPt
            if (this.isGridPositionOpen((int)curPt.X + 1, (int)curPt.Y) && !hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y]) 
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - (curPt.X+1)) + Math.Abs(goalPt.Y - curPt.Y));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X+1,curPt.Y);
                }

                if (goalPt.X == curPt.X + 1 && goalPt.Y == curPt.Y)
                    foundGoal = true;
                hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y] = true;
            }
            if (this.isGridPositionOpen((int)curPt.X - 1, (int)curPt.Y) && !hasBeenChecked[(int)curPt.X - 1, (int)curPt.Y]) 
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
            if (this.isGridPositionOpen((int)curPt.X, (int)curPt.Y + 1) && !hasBeenChecked[(int)curPt.X, (int)curPt.Y+1])
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - curPt.X) + Math.Abs(goalPt.Y - (curPt.Y+1)));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X, curPt.Y+1);
                }

                if (goalPt.X == curPt.X && goalPt.Y == curPt.Y+1)
                    foundGoal = true;

                hasBeenChecked[(int)curPt.X, (int)curPt.Y+1] = true;
            }
            if (this.isGridPositionOpen((int)curPt.X, (int)curPt.Y - 1) && !hasBeenChecked[(int)curPt.X, (int)curPt.Y-1])
            {
                curCostEst = (costSoFar + Math.Abs(goalPt.X - curPt.X) + Math.Abs(goalPt.Y - (curPt.Y-1)));
                if (minCost > curCostEst)
                {
                    minCost = curCostEst;
                    minCostPt = new Vector2(curPt.X, curPt.Y - 1);
                }

                if (goalPt.X == curPt.X && goalPt.Y == curPt.Y-1)
                    foundGoal = true;

                hasBeenChecked[(int)curPt.X + 1, (int)curPt.Y] = true;
            }

            if (foundGoal)
            {
                aStarSolution[curSpot] = goalPt;
            }
            else if (minCostPt.X == 100000 || curSpot>90)
            {
                //abort path cannot be found

            }
            else 
            {
                //Console.Out.WriteLine("{0},{1},{2},{3}",curSpot,minCostPt.X,minCostPt.Y,costSoFar);
                aStarSolution[curSpot] = minCostPt;
                curSpot++;
                aStarHelper(minCostPt, goalPt, costSoFar + 1);
            }
        }


        /*
         * Puts tiles onto every available grid
         */ 
        public void LoadContent(Texture2D content)
        {
            for (int i = 0; i < this.gridWidth; i++)
            {
                for (int j = 0; j < this.gridLength; j++)
                {
                    this.background[i, j] = content;
                }
            }
        }

        /*
         * content is the picture to be loaded onto the position made by x and y
         * It will do nothing if either x or y is out of bounds on the grid.
         */ 
        public void LoadGridContent(Texture2D content, int x, int y)
        {
            if (x < 0 || x > gridWidth)
            {
            }
            else if (y < 0 || y > gridLength)
            {
            }
            else
            {
                this.background[x, y] = content;
            }            
        }

        /*
         * Hardcoded menu space to draw
         */ 
        public void LoadBackgroundContent(Texture2D content)
        {
            for (int i = 0; i < this.gridWidth; i++)
            {
                this.background[i, 0] = content;
                this.background[i, this.gridLength - 1] = content;
                this.background[i, this.gridLength - 2] = content;

                this.hasObject[i, 0] = true;
                this.hasObject[i, this.gridLength - 1] = true;
                this.hasObject[i, this.gridLength - 2] = true;
            }
            for (int i = 0; i < this.gridLength; i++)
            {
                this.background[0, i] = content;
                this.background[this.gridWidth - 1, i] = content;

                this.hasObject[0, i] = true;
                this.hasObject[this.gridWidth - 1, i] = true;
            }
        }

        /*
         * Draws the background
         */ 
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.gridWidth; i++)
            {
                for (int j = 0; j < this.gridLength; j++)
                {
                    spriteBatch.Draw(background[i, j], grid[i, j], Color.White);
                }
            }
        }

        /*
         * Resets the grid's hasObjects to false in place area and true in menu area
         */
        public void reset()
        {

            for (int i = 0; i < this.gridWidth; i++)
            {
                for (int j = 0; j < this.gridLength; j++)
                {
                    this.hasObject[i, j] = false;
                }
            }


            for (int i = 0; i < this.gridWidth; i++)
            {
                this.hasObject[i, 0] = true;
                this.hasObject[i, this.gridLength - 1] = true;
                this.hasObject[i, this.gridLength - 2] = true;
            }
            for (int i = 0; i < this.gridLength; i++)
            {
                this.hasObject[0, i] = true;
                this.hasObject[this.gridWidth - 1, i] = true;
            }        
        }
    }
}
