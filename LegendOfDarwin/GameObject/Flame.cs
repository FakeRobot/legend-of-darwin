using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class Flame : BasicObject
    {
        // static int for how long this flame should live
        private static int FLAME_LIFE = 150;

        // texture for the flame
        public Texture2D flameTex;

        // counter for how long this flame will burn a tile
        private int flameCounter;

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

        public void setAlive(Boolean state)
        {
            this.alive = state;
        }

        public Boolean isAlive()
        {
            return this.alive;
        }

        public void LoadContent(Texture2D flameTexture)
        {
            this.flameTex = flameTexture;
        }

        public void Update()
        {
            if (this.flameCounter > FLAME_LIFE)
            {
                // flame can die
                //this.alive = false;
            }

            this.flameCounter++;
        }
    }
}
