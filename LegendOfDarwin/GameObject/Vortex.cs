using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin.GameObject
{
    // spaces to kill darwin, double as pits
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

        // can kill zombies as well
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
                Console.Out.WriteLine("collision with box");
                board.setGridPositionOpen(this.X, this.Y);
                bo.setGridPosition(0, 0);
                bo.setVisible(false);
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
