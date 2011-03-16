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

            destination = board.getPosition(startX, startY);
        }

        public void Update(GameTime gameTime, KeyboardState ks)
        {
            base.Update(gameTime);
        }

        public void CollisionWithZombie(Zombie zombie)
        {
            if (this.isOnTop(zombie))
            {
                zombie.setZombieAlive(false);
            }
        }

        public void CollisionWithBO(BasicObject bo, GameBoard board)
        {
            if (this.isOnTop(bo))
            {
                board.setGridPositionOpen(this.X, this.Y);
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
