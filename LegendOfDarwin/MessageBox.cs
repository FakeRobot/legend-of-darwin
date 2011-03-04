using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin
{
    class MessageBox 
    {

        protected Rectangle destination;
        protected Rectangle source = new Rectangle(0,0,200,128);
        protected Vector2 position;

        protected Texture2D messageTex;
        protected int MESSAGE_WIDTH = 200;
        protected int MESSAGE_HEIGHT = 100;

        protected String Message;

        public MessageBox(int x, int y, String message)
        {
            destination = new Rectangle(x,y,MESSAGE_WIDTH,MESSAGE_HEIGHT);
            Message = message;
            position = new Vector2(x+20,y+7);
        }

        // used to have message focus on a given gameboard square
        public void pointToSquare(int x,int y,GameBoard board)
        {
            int newX = board.getPosition(x, y).X;
            int newY = board.getPosition(x, y).Y;

            destination.X = newX + 40;
            destination.Y = newY - 94;

            position.X = destination.X+20;
            position.Y = destination.Y+7;
        }

        public void LoadContent(Texture2D messagePic){
            messageTex = messagePic;
        }

        public void Draw(SpriteBatch spriteBatch,SpriteFont myfont) 
        {
            spriteBatch.Draw(messageTex, destination, source, Color.White);
            spriteBatch.DrawString(myfont,Message,position,Color.Black);
        }

    }
}
