using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin.GameObject
{
    class Nursery : BasicObject
    {
        private Texture2D nurseTex;

        public Nursery(GameBoard gb)
            : base(gb)
        { 
            
        }

        public void LoadContent(Texture2D texIn)
        {
            nurseTex = texIn;
        }

        public void Update(GameTime gameTime)
        { 
        
        }
    }
}
