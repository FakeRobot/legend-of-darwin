using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfDarwin
{
    // for completing levels
    class Stairs : BasicObject
    {
        private Texture2D stairUpTex;
        private Texture2D stairDownTex;

        private enum Dir { Up, Down };

        private Dir view;

        public Stairs(GameBoard myBoard)
            : base(myBoard)
        {
            // DO nothing
        }

        public void LoadContent(Texture2D stairUp, Texture2D stairDown, String orientation)
        {
            stairUpTex = stairUp;
            stairDownTex = stairDown;
            view = new Dir();

            if (orientation.Equals("Up"))
            {
                view = Dir.Up;
            }
            else if (orientation.Equals("Down"))
            {
                view = Dir.Down;
            }
            else
            {
                throw new Exception("Orientation needs either 'Up' or 'Down'");
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (view)
            { 
                case Dir.Down:
                    spriteBatch.Draw(stairDownTex, destination, Color.White);
                    break;
                case Dir.Up:
                    spriteBatch.Draw(stairUpTex, destination, Color.White);
                    break;
                default:
                    throw new Exception("Something happened with the view");
            }

        }

        // check for darwin contact
        public void Update(GameTime gameTime, Darwin darwin)
        {
            base.Update(gameTime);

            if (darwin.isOnTop(this) && this.view.Equals(Dir.Up))
            {
                darwin.setGridPosition(2, 2);
                board.setGridPositionOpen(this);
            }
        }
    }
}
