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

        BasicObject potentialGridPosition;

        bool keyIsHeldDown = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            darwin = new Darwin();
            potentialGridPosition = new BasicObject();
            //darwin.setPosition(0.0f, 0.0f);
            device = graphics.GraphicsDevice;

            InitializeGraphics();

            // Initializing board
            board = new GameBoard(new Vector2(25, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));

            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);

            // Initial starting position
            darwin.setGridPosition(5, 5);
            potentialGridPosition.setGridPosition(5,5);

            if(board.isGridPositionOpen(darwin))
            {
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D darwinTex = Content.Load<Texture2D>("Darwin");
            Texture2D zombieDarwinTex = Content.Load<Texture2D>("ZombieDarwin");
            Texture2D zombieTex = Content.Load<Texture2D>("Zombie");

            // Test
            Texture2D basicGridTex = Content.Load<Texture2D>("grid_outline");
            Texture2D basicMenuTex = Content.Load<Texture2D>("grid_menu_outline");

            // Test
            board.LoadContent(basicGridTex);
            board.LoadBackgroundContent(basicMenuTex);

            darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
            firstZombie.LoadContent(zombieTex);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            updateKeyHeldDown(ks);

            if (keyIsHeldDown)
            {
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

            }
            else{
                detectDarwinMovement(ks);
                detectDarwinTransform(ks);
                darwin.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            }

            //currently does nothing
            darwin.Update(gameTime);

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime);

            base.Update(gameTime);
        }

        private void updateKeyHeldDown(KeyboardState ks)
        {
            if (ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Up) && ks.IsKeyUp(Keys.Down))
            {
                keyIsHeldDown = false;
            }
            else
            {
                keyIsHeldDown = true;
            }
        }

        private void detectDarwinMovement(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Right))
            {
                potentialGridPosition.setGridPosition(darwin.X + 1, darwin.Y);
                if (board.isGridPositionOpen(potentialGridPosition))
                {
                    darwin.setGridPosition(potentialGridPosition.X, potentialGridPosition.Y);
                    board.setGridPositionOpen(darwin.X - 1, darwin.Y);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                potentialGridPosition.setGridPosition(darwin.X - 1, darwin.Y);
                if (board.isGridPositionOpen(potentialGridPosition))
                {
                    darwin.setGridPosition(potentialGridPosition.X, potentialGridPosition.Y);
                    board.setGridPositionOpen(darwin.X + 1, darwin.Y);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                potentialGridPosition.setGridPosition(darwin.X, darwin.Y - 1);
                if (board.isGridPositionOpen(potentialGridPosition))
                {
                    darwin.setGridPosition(potentialGridPosition.X, potentialGridPosition.Y);
                    board.setGridPositionOpen(darwin.X, darwin.Y + 1);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                potentialGridPosition.setGridPosition(darwin.X, darwin.Y + 1);
                if (board.isGridPositionOpen(potentialGridPosition))
                {
                    darwin.setGridPosition(potentialGridPosition.X, potentialGridPosition.Y);
                    board.setGridPositionOpen(darwin.X, darwin.Y - 1);
                    darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
                }
            }
        }

        private void detectDarwinTransform(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Z))
            {
                darwin.Transform();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            board.Draw(spriteBatch);


            darwin.Draw(spriteBatch);
            firstZombie.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitializeGraphics()
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.ApplyChanges();
        }

    }
}
