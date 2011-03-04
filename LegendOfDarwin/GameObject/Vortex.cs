using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class Vortex : BasicObject
    {
        Texture2D vortexTex;

        public Vortex(GameBoard gameBoard)
            : base(gameBoard)
        {
        }

        public void Update()
        { 
        
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
