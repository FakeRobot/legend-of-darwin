using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class GameStart
    {
        private Texture2D screenTex;
        private Rectangle position;

        public GameStart(int winWidth, int winLength)
        {
            position = new Rectangle(0, 0, winWidth, winLength);
        }

        public void LoadContent(Texture2D startScreen)
        {
            screenTex = startScreen;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(screenTex, position, Color.White);
        }

    }
}
