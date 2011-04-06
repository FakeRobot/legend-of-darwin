using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LegendOfDarwin
{
    public class Darwin : BasicObject
    {
        GraphicsDevice graphics;

        // an enum type for the direction 
        public enum Dir { Up, Down, Left, Right };

        //Darwin's height in pixels
        public const int DARWIN_HEIGHT = 64;
        //Darwin's width in pixels
        public const int DARWIN_WIDTH = 64;


        //Which direction is Darwin facing
        public Dir facing = Dir.Down;

        //flag to show whether darwin is a zombie or not
        private bool zombieFlag;
        private bool isZombieButtonBeingPressed = false;

        //start darwin's humanity at 100
        public int humanityLevel = 100;

        // The frame or cell of the sprite to show
        private Rectangle source;
        private Rectangle[] downSource;
        private int downCount;

        // The location to draw the sprite on the screen.
        //public Rectangle destination;

        public bool collision = false;


        // The current position of Darwin on the floor
        // Might have to be changed to coordinates depending on the floor layout
        public Vector2 position = Vector2.Zero;


        // Textures 
        Texture2D darwinUpTex;
        Texture2D darwinDownTex;
        Texture2D darwinRightTex;
        Texture2D darwinLeftTex;
        Texture2D zombieDarwinTex;

        Texture2D deadDarwinTex;
        bool isDead = false;

        private int widthLength, heightLength, darwinLag, zombieLag, darwinCount, zombieCount;
        private int darwinWidthLength, darwinHeightLength, zombieWidthLength, zombieHeightLength;
        private bool inMotion;
        private Dir moveDirection;

        //BasicObject potentialGridPosition;

        //constructor
        public Darwin(GameBoard myboard) : base(myboard)
        {
            // Init the frame or cell of the animation that will be shown. 
            source = new Rectangle();
            source.Width = DARWIN_HEIGHT;
            source.Height = DARWIN_WIDTH;
            source.X = 0;
            source.Y = 0;

            downSource = new Rectangle[3];
            downSource[0] = new Rectangle(0, 0, DARWIN_WIDTH, DARWIN_HEIGHT);
            downSource[1] = new Rectangle(DARWIN_WIDTH, 0, DARWIN_WIDTH, DARWIN_HEIGHT);
            downSource[2] = new Rectangle(DARWIN_WIDTH * 2, 0, DARWIN_WIDTH, DARWIN_HEIGHT);
            downCount = 0;

            darwinLag = 10;
            zombieLag = 15;
            darwinCount = 0;
            zombieCount = 0;

            widthLength = board.getPosition(10, 10).X - board.getPosition(9, 10).X;
            heightLength = board.getPosition(10, 10).Y - board.getPosition(10, 9).Y;

            darwinWidthLength = widthLength / darwinLag;
            darwinHeightLength = heightLength / darwinLag;
            zombieWidthLength = widthLength / zombieLag;
            zombieHeightLength = heightLength / zombieLag;

            this.setEventLag(darwinLag);

            inMotion = false;


        }

        public void setSource(Rectangle rec)
        {
            source.Width = rec.Width;
            source.Height = rec.Height;
            source.X = rec.X;
            source.Y = rec.Y;
        }

        public void setPictureSize(int width, int height)
        {
            destination.Width = width;
            destination.Height = height;
        }

        /*
        public void setPosition(int startX, int startY)
        {
            // Set the initial position of Darwin
            position.X = (float)startX;
            position.Y = (float)startY;

            // Update the destination
            destination = new Rectangle();
            destination.Height = DARWIN_HEIGHT;
            destination.Width = DARWIN_WIDTH;
            destination.X = (int)Math.Round(position.X);
            destination.Y = (int)Math.Round(position.Y);
        }
        */

        public void LoadContent(GraphicsDevice newGraphics, Texture2D humanUp, Texture2D humanDown, 
            Texture2D humanRight, Texture2D humanLeft, Texture2D zombieTex,Texture2D deadTex)
        {
            graphics = newGraphics;
            darwinUpTex = humanUp;
            darwinDownTex = humanDown;
            darwinRightTex = humanRight;
            darwinLeftTex = humanLeft;
            zombieDarwinTex = zombieTex;
            deadDarwinTex = deadTex;
        }

        public void Update(GameTime gameTime, KeyboardState ks, GameBoard board, int currentDarwinX, int currentDarwinY)
        {
            base.Update(gameTime);
            if (inMotion)
            {
                if (isZombie())
                {
                    zombieCount++;
                }
                else
                {
                    darwinCount++;
                }
                if (isZombie())
                {
                    switch (moveDirection)
                    {
                        case Dir.Left:
                            this.destination.X = this.destination.X - zombieWidthLength;
                            break;
                        case Dir.Down:
                            this.destination.Y = this.destination.Y + zombieHeightLength;
                            break;
                        case Dir.Right:
                            this.destination.X = this.destination.X + zombieWidthLength;
                            break;
                        case Dir.Up:
                            this.destination.Y = this.destination.Y - zombieHeightLength;
                            break;
                    }

                    if (zombieCount > zombieLag)
                    {
                        inMotion = false;
                        zombieCount = 0;

                        switch (moveDirection)
                        {
                            case Dir.Left:
                                this.MoveLeft();
                                break;
                            case Dir.Down:
                                this.MoveDown();
                                break;
                            case Dir.Right:
                                this.MoveRight();
                                break;
                            case Dir.Up:
                                this.MoveUp();
                                break;
                        }
                    }
                }
                else if (!isZombie())
                {
                    if (darwinCount > darwinLag / 2)
                    {
                        downCount = 2;
                    }

                    switch (moveDirection)
                    {
                        case Dir.Left:
                            this.destination.X = this.destination.X - darwinWidthLength;
                            break;
                        case Dir.Down:
                            this.destination.Y = this.destination.Y + darwinHeightLength;
                            break;
                        case Dir.Right:
                            this.destination.X = this.destination.X + darwinWidthLength;
                            break;
                        case Dir.Up:
                            this.destination.Y = this.destination.Y - darwinHeightLength;
                            break;
                    }

                    if (darwinCount > darwinLag)
                    {
                        inMotion = false;
                        darwinCount = 0;
                        switch (moveDirection)
                        {
                            case Dir.Left:
                                this.MoveLeft();
                                break;
                            case Dir.Down:
                                this.MoveDown();
                                downCount = 0;
                                break;
                            case Dir.Right:
                                this.MoveRight();
                                break;
                            case Dir.Up:
                                this.MoveUp();
                                break;
                        }
                    }
                }
            }
            else
            {
                moveDarwin(ks, board, currentDarwinX, currentDarwinY);
            }

            //if (canEventHappen())
            //{
            //    updateDarwinTransformState(ks);
            //    this.setEventFalse();
            //}

            if (!isZombieButtonBeingPressed)
                updateDarwinTransformState(ks);

            if (ks.IsKeyUp(Keys.Z))
                isZombieButtonBeingPressed = false;
        }

        private void moveDarwin(KeyboardState ks, GameBoard board, int currentDarwinX, int currentDarwinY)
        {
            // has a direction been picked to go in, used to get rid of diagonal movement
            bool hasPicked = false;

            if (ks.IsKeyDown(Keys.Right))
            {
                if (board.isGridPositionOpen(currentDarwinX + 1, currentDarwinY) && !hasPicked && facing == Dir.Right)
                {
                    hasPicked = true;
                    moveDirection = Dir.Right;
                    inMotion = true;
                }
                else
                {
                    facing = Dir.Right;
                }
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                if (board.isGridPositionOpen(currentDarwinX - 1, currentDarwinY) && !hasPicked && facing == Dir.Left)
                {
                    hasPicked = true;
                    moveDirection = Dir.Left;
                    inMotion = true;
                }
                else
                {
                    facing = Dir.Left;
                }
                
            }
            if (ks.IsKeyDown(Keys.Up))
            {

                if (board.isGridPositionOpen(currentDarwinX, currentDarwinY - 1) && !hasPicked && facing == Dir.Up)
                {
                    hasPicked = true;
                    moveDirection = Dir.Up;
                    inMotion = true;
                }
                else
                {
                    facing = Dir.Up;
                }
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                if (board.isGridPositionOpen(currentDarwinX, currentDarwinY + 1) && !hasPicked && facing == Dir.Down)
                {
                    /*
                    if (downCount > 1)
                    {
                        downCount = 1;
                    }
                    else
                    {
                        downCount++;
                    }
                    
                    this.MoveDown();
                     * */
                    hasPicked = true;
                    moveDirection = Dir.Down;
                    inMotion = true;
                    downCount = 1;
                }
                else
                {
                    facing = Dir.Down;
                    downCount = 0;
                }
            }
        }

        private void updateDarwinTransformState(KeyboardState ks)
        {
            
            if (ks.IsKeyDown(Keys.Z))
            {
                isZombieButtonBeingPressed = true;
                if (zombieFlag == true)
                {
                    zombieFlag = false;
                }
                else
                {
                    zombieFlag = true;
                }
            }
        }

        // Check if Darwin is intersecting something
        public bool Intersects(/*Zombie enemy*/)
        {
            /*
            if Darwin intersects with a zombie
                if he's human
                    return true
                else
                    return false

             otherwise if there is no intersection
                return false
            */
             
            return false;
        }

        // Collisions
        public bool Collision(/*Zombie enemy*/)
        {
            /*
            if (Intersects(enemy))
            {
                Collision Logic
                return true;
            }
            else
            {
                return false;
            }*/
            return false;
        }

        public bool isZombie()
        {
            return this.zombieFlag;
        }

        public void setHuman()
        {
            this.zombieFlag = false;
        }

        public void setZombie()
        {
            this.zombieFlag = true;
        }

        public void setDarwinDead() 
        {
            isDead = true;
        }

        public void setDarwinAlive() 
        {
            isDead = false;
        }

        public bool isDarwinAlive() 
        {
            return !isDead;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDead)
                spriteBatch.Draw(deadDarwinTex, destination, source, Color.White);
            else if (zombieFlag == false)
            {
                if (facing == Dir.Up)
                {
                    spriteBatch.Draw(darwinUpTex, destination, source, Color.White);
                }
                else if (facing == Dir.Down)
                {
                    spriteBatch.Draw(darwinDownTex, destination, downSource[downCount], Color.White);
                }
                else if (facing == Dir.Left)
                {
                    spriteBatch.Draw(darwinLeftTex, destination, source, Color.White);
                }
                else
                {
                    spriteBatch.Draw(darwinRightTex, destination, source, Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(zombieDarwinTex, destination, source, Color.White);
            }
        }
    }
}
