using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    public class Flame : BasicObject
    {
        // static int for how long this flame should live
        private static int FLAME_LIFE = 150; // TODO: actually make this work instead of eternal flames

        // counter for how long this flame will burn a tile
        private int flameCounter;

        // true if the left sprite will show
        private bool leftSprite;

        // is this flame alive
        private Boolean alive;

        public Flame(GameBoard myboard, int x, int y)
            : base(myboard)
        {
            this.X = x;
            this.Y = y;
            this.flameCounter = 0;
            this.alive = true;
        }

        /// <summary>
        /// Sets the flame to be alive and a burnin'
        /// </summary>
        /// <param name="state">True if the flame is alive, False if otherwise.</param>
        public void setAlive(Boolean state)
        {
            this.alive = state;
        }

        /// <summary>
        /// Gets the flame's current state.
        /// </summary>
        /// <returns>True if alive, False if otherwise</returns>
        public Boolean isAlive()
        {
            return this.alive;
        }

        public void Update()
        {
            if (this.flameCounter > FLAME_LIFE)
            {
                // TODO: make this work
                //this.alive = false;
            }

            // animutions
            if (flameCounter > 0 && flameCounter <= 20)
                leftSprite = true;
            else if (flameCounter > 20 && flameCounter <= 40)
                leftSprite = false;
            else
                this.flameCounter = 0;

            this.flameCounter++;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (leftSprite)
            {
                Rectangle source = new Rectangle(0, 0, 256, 256);
                spriteBatch.Draw(texture, board.getPosition(this.X, this.Y), source, Color.White);
            }
            else
            {
                Rectangle source = new Rectangle(256, 0, 256, 256);
                spriteBatch.Draw(texture, board.getPosition(this.X, this.Y), source, Color.White);
            }
        }
    }
}
