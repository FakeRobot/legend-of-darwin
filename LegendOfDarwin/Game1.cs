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

        GameState gameState;
        GameStart gameStart;

        Darwin darwin;
        Zombie firstZombie;
        Switch firstSwitch;
        GameBoard board;
        GraphicsDevice device;
        bool keyIsHeldDown = false;
        bool gameOver = false;
        bool gameWin = false;

        private int counter;
        private int counterReady;
        Texture2D gameOverTexture;
        Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;
        Stairs firstStair, secondStair;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            gameOverPosition.X = 320;
            gameOverPosition.Y = 130;

            device = graphics.GraphicsDevice;
            InitializeGraphics();

            gameState = new GameState();
            gameStart = new GameStart(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);

            board = new GameBoard(new Vector2(25, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board); 
            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);

            firstStair = new Stairs(board);
            secondStair = new Stairs(board);

            //later add an x and y to the constructor
            BasicObject s1 = new BasicObject(board);
            s1.X = 20;
            s1.Y = 19;

            BasicObject s2 = new BasicObject(board);
            s2.X = 20;
            s2.Y = 20;

            BasicObject s3 = new BasicObject(board);
            s3.X = 20;
            s3.Y = 21;

            BasicObject s4 = new BasicObject(board);
            s4.X = 20;
            s4.Y = 22;

            BasicObject s5 = new BasicObject(board);
            s5.X = 20;
            s5.Y = 19;

            BasicObject s6 = new BasicObject(board);
            s6.X = 21;
            s6.Y = 19;

            BasicObject s7 = new BasicObject(board);
            s7.X = 22;
            s7.Y = 19;

            BasicObject s8 = new BasicObject(board);
            s8.X = 23;
            s8.Y = 19;

            BasicObject[] squares = new BasicObject[8] {s1, s2, s3, s4, s5, s6, s7, s8};

            BasicObject switchSquare = new BasicObject(board);
            switchSquare.X = 10;
            switchSquare.Y = 3;

            firstSwitch = new Switch(switchSquare, board, squares);

            // Initial starting position
            darwin.setGridPosition(5, 5);

            if(board.isGridPositionOpen(darwin))
            {
                board.setGridPositionOccupied(darwin.X, darwin.Y);
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;


            if (board.isGridPositionOpen(20, 2))
            {
                firstStair.setGridPosition(20, 2);
                firstStair.setDestination(board.getPosition(20, 2));
            }
            if (board.isGridPositionOpen(21, 20))
            {
                secondStair.setGridPosition(21, 20);
                secondStair.setDestination(board.getPosition(21, 20));
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D darwinTex = Content.Load<Texture2D>("DarwinPic/Darwin");

            Texture2D darwinUpTex = Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = Content.Load<Texture2D>("DarwinPic/ZombieDarwin");

            Texture2D zombieTex = Content.Load<Texture2D>("ZombiePic/Zombie");

            // Test
            Texture2D basicGridTex = Content.Load<Texture2D>("StaticPic/grid_outline");
            Texture2D basicMenuTex = Content.Load<Texture2D>("StaticPic/grid_menu_outline");

            Texture2D basicStairUpTex = Content.Load<Texture2D>("StaticPic/stairsUp");
            Texture2D basicStairDownTex = Content.Load<Texture2D>("StaticPic/stairsDown");
            
            // Texture for the wall and switch
            Texture2D wallTex = Content.Load<Texture2D>("StaticPic/Wall");
            Texture2D switchTex = Content.Load<Texture2D>("StaticPic/Switch");

            gameOverTexture = Content.Load<Texture2D>("gameover");
            gameWinTexture = Content.Load<Texture2D>("gamewin");

            firstStair.LoadContent(basicStairUpTex, basicStairDownTex, "Up");
            secondStair.LoadContent(basicStairUpTex, basicStairDownTex, "Down");
            
            firstSwitch.LoadContent(wallTex, switchTex);

            // Test
            board.LoadContent(basicGridTex);
            board.LoadBackgroundContent(basicMenuTex);

            //darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, darwinRightTex, darwinLeftTex, zombieDarwinTex);
            firstZombie.LoadContent(zombieTex);

            gameStart.LoadContent(Content.Load<Texture2D>("gamestart"));
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            switch (gameState.getState())
            { 
                case GameState.state.Start:
                    UpdateStartState();
                    break;
                case GameState.state.Level:
                    UpdateLevelState(gameTime);
                    break;
                case GameState.state.End:
                    UpdateEndState();
                    break;
            
            }
            base.Update(gameTime);
        }
        private void UpdateStartState()
        {
            KeyboardState ks = Keyboard.GetState();
            if(ks.IsKeyDown(Keys.Enter))
            {
                gameState.setState(GameState.state.Level);
            }
        }

        private void UpdateLevelState(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            checkForExitGame(ks);

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

            firstStair.Update(gameTime, darwin);
            secondStair.Update(gameTime, darwin);

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime, darwin);

            firstSwitch.Update(gameTime, ks, darwin);

            checkForGameOver();
            checkForGameWin();

            if (gameOver || gameWin)
            {
                gameState.setState(GameState.state.End);
            }

        }

        private void UpdateEndState()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q)) 
            {
                this.Exit();
            }
            if (ks.IsKeyDown(Keys.R))
            {
                gameOver = false;
                gameWin = false;

                board.setGridPositionOpen(darwin);
                darwin.setAbsoluteDestination(2, 2);

                gameState.setState(GameState.state.Level);
            }
        
        }

        private void checkForExitGame(KeyboardState ks)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (ks.IsKeyDown(Keys.Q) || ks.IsKeyDown(Keys.Space))
            {
                this.Exit();
            }
        }

        private void checkForGameOver()
        {
            if (darwin.isOnTop(firstZombie))
            {
                gameOver = true;
            }
  
            if (darwin.collision)
            {
                Rectangle rightSideOfDarwin = darwin.destination;
                rightSideOfDarwin.X = rightSideOfDarwin.X + board.getSquareWidth();

                Rectangle leftSideOfDarwin = darwin.destination;
                leftSideOfDarwin.X = leftSideOfDarwin.X - board.getSquareWidth();

                Rectangle onTopOfDarwin = darwin.destination;
                onTopOfDarwin.Y = onTopOfDarwin.Y - board.getSquareLength();

                Rectangle onBottomOfDarwin = darwin.destination;
                onBottomOfDarwin.Y = onBottomOfDarwin.Y + board.getSquareLength();

                if (rightSideOfDarwin == firstZombie.destination || 
                    leftSideOfDarwin == firstZombie.destination || 
                    onTopOfDarwin == firstZombie.destination || 
                    onBottomOfDarwin == firstZombie.destination)
                {
                    gameOver = true;
                }
            }
        }

        private void checkForGameWin()
        {
            if (darwin.isOnTop(secondStair))
            {
                gameWin = true;
            }
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
            switch (gameState.getState())
            {
                case GameState.state.Start:
                    DrawStartState();
                    break;
                case GameState.state.Level:
                    DrawLevelState(gameTime);
                    break;
                case GameState.state.End:
                    DrawEndState();
                    break;
            }
            base.Draw(gameTime);
        }

        private void DrawStartState()
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            gameStart.Draw(spriteBatch);
            spriteBatch.End();
            
        }

        private void DrawLevelState(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            board.Draw(spriteBatch);

            firstStair.Draw(spriteBatch);
            secondStair.Draw(spriteBatch);

            darwin.Draw(spriteBatch);
            firstZombie.Draw(spriteBatch);
            firstSwitch.Draw(spriteBatch);



            spriteBatch.End();    
        }

        private void DrawEndState()
        {
            spriteBatch.Begin();
            if (gameOver)
            {
                gameOverPosition.X = 320;
                gameOverPosition.Y = 130;
                spriteBatch.Draw(gameOverTexture, gameOverPosition, Color.White);
            }

            if (gameWin)
            {
                spriteBatch.Draw(gameWinTexture, gameOverPosition, Color.White);
            }
            spriteBatch.End();
        }

        private void InitializeGraphics()
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.ApplyChanges();
        }     
    }
}
