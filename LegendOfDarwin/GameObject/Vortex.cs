using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    class Vortex : BasicObject
    {
        Texture2D vortexTex;

        public Vortex(GameBoard gameBoard, int startX, int startY) : base(gameBoard)
        {
            this.X = startX;
            this.Y = startY;

            this.setGridPosition(startX, startY);
            destination = board.getPosition(startX, startY);
        }

        public void Update(GameTime gameTime, KeyboardState ks, Darwin darwin)
        {
            base.Update(gameTime);

            if (this.isOnTop(darwin))
            {
                darwin.setAbsoluteDestination(2, 2);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(vortexTex, destination, Color.White);
        }

        public void LoadContent(Texture2D tex)
        {
            vortexTex = tex;
        }
    }
}
