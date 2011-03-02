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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Darwin darwin;
        Zombie firstZombie;
        Switch firstSwitch;
        GameBoard board;
        GraphicsDevice device;
        bool keyIsHeldDown = false;
        private int counter;
        private int counterReady;

        Stairs firstStair;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            darwin = new Darwin(); 
            device = graphics.GraphicsDevice;
            InitializeGraphics();

            board = new GameBoard(new Vector2(25, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);

            //array of squares that the switch will control
            BasicObject s1 = new BasicObject(20, 20);

            BasicObject s2 = new BasicObject(21, 21);
           
            BasicObject[] squares = new BasicObject[2] {s1, s2};

            firstSwitch = new Switch(10, 3, board, squares);

            // Initial starting position
            darwin.setGridPosition(5, 5);

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

            // Texture for the wall
            Texture2D wallTex = Content.Load<Texture2D>("Wall");
            firstSwitch.LoadContent(wallTex);

            // Test
            board.LoadContent(basicGridTex);
            board.LoadBackgroundContent(basicMenuTex);

            darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
            firstZombie.LoadContent(zombieTex);
        }

        protected override void UnloadContent(){}

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
                    darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
                    counter = 0;
                }
                else
                {
                    counter++;
                }
            }
            else
            {
                darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            }

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime,darwin);

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            board.Draw(spriteBatch);


            darwin.Draw(spriteBatch);
            firstZombie.Draw(spriteBatch);
            firstSwitch.Draw(spriteBatch);

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
