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
        private int maxBabies;
        private Texture2D nurseTex;
        private BabyZombie[] babies;

        public Nursery(GameBoard gb, Darwin darwin)
            : base(gb)
        {
            maxBabies = 7;
            babies = new BabyZombie[maxBabies];

            foreach (BabyZombie b in babies)
            {
                b = new BabyZombie(0, 0, 15, 5, 15, 5, darwin, gb);
            }
        }

        public void LoadContent(Texture2D nurseTexIn, Texture2D babyTexIn)
        {
            nurseTex = nurseTexIn;

            foreach (BabyZombie b in babies)
            {
                b.LoadContent(babyTexIn);
            }
        }

        public void Update(GameTime gameTime)
        { 
        
        }
    }
}
