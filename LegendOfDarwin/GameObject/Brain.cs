using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace LegendOfDarwin.GameObject
{

    class Brain: BasicObject
    {
        public const int BRAIN_HEIGHT = 64;
        public const int BRAIN_WIDTH = 64;

        protected Texture2D brainTexture;

        public Brain(GameBoard myboard, int startX, int startY) : base(myboard)
        {
            this.X = startX;
            this.Y = startY;

            this.setGridPosition(startX, startY);

            if (board.isGridPositionOpen(this))
            {
                board.setGridPositionOccupied(this.X, this.Y);
            }

            destination = board.getPosition(startX, startY);
        }

        public void LoadContent(Texture2D brainTex)
        {
            brainTexture = brainTex;
        }

        public void Update()
        {



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(brainTexture, this.destination, Color.White);
        }

    }
}
