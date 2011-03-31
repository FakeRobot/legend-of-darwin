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

        private static int FLAME_LIFE = 150;

        // texture for the flame
        public Texture2D flameTex;

        // counter for how long this flame will burn a tile
        public int flameCounter;

        // is this flame currently burning a tile?
        private Boolean burning;

        public Flame(GameBoard myboard, int x, int y)
            : base(myboard)
        {
            this.X = x;
            this.Y = y;
            this.flameCounter = 1;
            this.burning = true;
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
                this.burning = false;
                this.flameCounter = 0;
            }

            this.flameCounter++;
        }
    }
}
