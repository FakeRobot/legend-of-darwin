using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using LegendOfDarwin.MenuObject;
using LegendOfDarwin.GameObject;

namespace LegendOfDarwin.GameObject
{
    // potion for healing humanity
    class Potion : BasicObject
    {
        private Texture2D potionTex;
        private bool isConsumed;
        private const int healthReplenished = 20;
        
        public SoundEffect potionSound;

        public Potion(GameBoard board) : base(board)
        {
            isConsumed = false;
            board.setGridPositionOpen(this);
        }

        public void LoadContent(Texture2D potTex, SoundEffect sound)
        {
            potionTex = potTex;
            potionSound = sound;
        }

        public void Update(GameTime gameTime, KeyboardState ks, Darwin darwin, ZombieTime zTime)
        { 
            if(this.isOnTop(darwin) && !isConsumed)
            {
                consumePotion(zTime);
            }
        }

        private void consumePotion(ZombieTime zTime)
        {
            board.setGridPositionOpen(this);
            isConsumed = true;

            int updateTime = zTime.getTime() - healthReplenished;
            zTime.setTime(updateTime);

            potionSound.Play();

        }

        public void reset()
        {
            isConsumed = false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (isConsumed == false)
            {
                spriteBatch.Draw(potionTex, this.destination, Color.White);
            }
        }
    }
}
