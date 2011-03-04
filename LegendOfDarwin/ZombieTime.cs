using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    class ZombieTime : BasicObject
    {
        Texture2D bar;
        Rectangle source;
        bool timeOut;

        private const int x = 5;
        private const int y = 0;
        private const int width = 60;
        private const int height = 20;

        private const int barLength = 5;
        private const int eventLag = 10;

        private const int downTime = 60;

        public ZombieTime(GameBoard board) : base(board)
        {
            destination = new Rectangle(board.getPosition(x, y).X, board.getPosition(x, y).Y, board.getPosition(x,y).Width*barLength, board.getPosition(x, y).Height);
            source = new Rectangle(y,y,width,height);
            timeOut = false;

            this.setEventLag(eventLag);
        }

        public void LoadContent(Texture2D inBar)
        {
            bar = inBar;
        }

        public void reset()
        {
            source = new Rectangle(y, y, width, height);
            timeOut = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bar, destination, source, Color.White);
        }

        public bool isTimedOut()
        {
            return timeOut;
        }

        public void UpdateBar(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.canEventHappen())
            {
                source.X++;
                this.setEventFalse();
            }

            if (source.X > downTime)
            {
                timeOut = true;
            }
        }
    }
}
