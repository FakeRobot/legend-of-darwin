using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace LegendOfDarwin
{
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // counter and counterReady used for the movement timing of darwin
        private int counter;
        private int counterReady;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Main character of the game
        Darwin darwin;

        // First Zombie
        Zombie firstZombie;

        // The grid board
        GameBoard board;

        GraphicsDevice device;

        // Test subject atm
        Darwin d2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Testing
            d2 = new Darwin();

            darwin = new Darwin();
            //darwin.setPosition(0.0f, 0.0f);
            device = graphics.GraphicsDevice;

            // Initializing board
            board = new GameBoard(new Vector2(25, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));

            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);

            // Initial starting position
            darwin.setGridPosition(5, 5);
            if(board.isGridPositionOpen(darwin))
            {
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 15;

            // Test
            d2.setGridPosition(10, 10);
            if (board.isGridPositionOpen(d2))
            {
                d2.setPosition(board.getPosition(d2).X, board.getPosition(d2).Y);
            }
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D darwinTex = Content.Load<Texture2D>("Darwin");
            Texture2D zombieDarwinTex = Content.Load<Texture2D>("ZombieDarwin");

            darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
            firstZombie.LoadContent(zombieDarwinTex);
            
            // Test
            d2.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            // Check the counter
            if (counter > counterReady)
            {
                detectDarwinMovement(ks);
                detectDarwinTransform(ks);
                darwin.setPictureSize(board.getSquareWidth(), board.getSquareLength());
                counter = 0;
            }
            else
            {
                counter++;
            }


            d2.setPictureSize(board.getSquareWidth(), board.getSquareLength());

            //currently does nothing
            darwin.Update(gameTime);

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime);

            base.Update(gameTime);
        }

        private void detectDarwinMovement(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Right))
            {
                darwin.setGridPosition(darwin.X + 1, darwin.Y);
                if (board.isGridPositionOpen(darwin))
                {
                    board.setGridPositionOpen(darwin.X - 1, darwin.Y);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
                else
                {
                    darwin.setGridPosition(darwin.X - 1, darwin.Y);
                }
                //darwin.SetPosition((darwin.position.X + 1.0f), darwin.position.Y);
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                darwin.setGridPosition(darwin.X - 1, darwin.Y);
                if (board.isGridPositionOpen(darwin))
                {
                    board.setGridPositionOpen(darwin.X + 1, darwin.Y);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
                else
                {
                    darwin.setGridPosition(darwin.X + 1, darwin.Y);
                }
                //darwin.SetPosition((darwin.position.X - 1.0f), darwin.position.Y);
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                darwin.setGridPosition(darwin.X, darwin.Y - 1);
                if (board.isGridPositionOpen(darwin))
                {
                    board.setGridPositionOpen(darwin.X, darwin.Y + 1);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
                else
                {
                    darwin.setGridPosition(darwin.X, darwin.Y + 1);
                }
                //darwin.SetPosition(darwin.position.X, (darwin.position.Y - 1.0f));
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                darwin.setGridPosition(darwin.X, darwin.Y + 1);
                if (board.isGridPositionOpen(darwin))
                {
                    board.setGridPositionOpen(darwin.X, darwin.Y - 1);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
                else
                {
                    darwin.setGridPosition(darwin.X, darwin.Y - 1);
                }
                //darwin.SetPosition(darwin.position.X, (darwin.position.Y + 1.0f));
            }
        }

        private void detectDarwinTransform(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Z))
            {
                darwin.Transform();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            darwin.Draw(spriteBatch);
            d2.Draw(spriteBatch);
            firstZombie.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}