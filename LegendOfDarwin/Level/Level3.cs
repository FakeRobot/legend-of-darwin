using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using LegendOfDarwin.GameObject;
using LegendOfDarwin.MenuObject;


namespace LegendOfDarwin
{
    public class Level3
    {

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        private GameState gameState;
        private GameStart gameStart;

        private FastZombie fastZombie1;
        private Leaf leaf, leaf2, leaf3, leaf4, leaf5, leaf6, leaf7, leaf8, leaf9, leaf10, leaf11, leaf12, leaf13, leaf14, leaf15, leaf16, leaf17, leaf18, leaf19, leaf20, leaf21, leaf22, leaf23, leaf24, leaf25, leaf26, leaf27, leaf28, leaf29, leaf30, leaf31, leaf32, leaf33, leaf34, leaf35, leaf36, leaf37, leaf38, leaf39, leaf40;

        private Darwin darwin;
        private Zombie firstZombie;
        private Switch firstSwitch, secondSwitch;
        private Brain brain;
        private GameBoard board;
        public GraphicsDevice device;
        public SpriteFont messageFont;
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;

        private int counter;
        private int counterReady;
        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;
        private Stairs secondStair;

        // things for managing message boxes
        bool messageMode = false;
        int messageModeCounter = 0;
        private MessageBox zombieMessage;
        private MessageBox darwinMessage;
        private MessageBox switchMessage;
        private MessageBox brainMessage;
        private MessageBox fastMessage;

        private ZombieTime zTime;
        private ZombieTime zTimeReset; //what zTime should reset to

        public Song song;
        public Game1 mainGame;

        public Level3(Game1 myMainGame)
        {
            mainGame = myMainGame;
        }

        public void Initialize()
        {
            gameOverPosition.X = 320;
            gameOverPosition.Y = 130;

            device = graphics.GraphicsDevice;

            gameState = new GameState();
            gameState.setState(GameState.state.Level);
            gameStart = new GameStart(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);

            board = new GameBoard(new Vector2(33, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board);
            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);
            //secondZombie = new Zombie(10, 16, 15, 5, 15, 5, board);
            //thirdZombie = new Zombie(16, 10, 15, 5, 15, 5, board);

            fastZombie1 = new FastZombie(15, 15, 15, 0, 15, 0, board);
            leaf = new Leaf(board, fastZombie1);
            leaf2 = new Leaf(board, fastZombie1);
            leaf3 = new Leaf(board, fastZombie1);
            leaf4 = new Leaf(board, fastZombie1);
            leaf5 = new Leaf(board, fastZombie1);
            leaf6 = new Leaf(board, fastZombie1);
            leaf7 = new Leaf(board, fastZombie1);
            leaf8 = new Leaf(board, fastZombie1);
            leaf9 = new Leaf(board, fastZombie1);
            leaf10 = new Leaf(board, fastZombie1);
            leaf11 = new Leaf(board, fastZombie1);
            leaf12 = new Leaf(board, fastZombie1);
            leaf13 = new Leaf(board, fastZombie1);
            leaf14 = new Leaf(board, fastZombie1);
            leaf15 = new Leaf(board, fastZombie1);
            leaf16 = new Leaf(board, fastZombie1);
            leaf17 = new Leaf(board, fastZombie1);
            leaf18 = new Leaf(board, fastZombie1);
            leaf19 = new Leaf(board, fastZombie1);
            leaf20 = new Leaf(board, fastZombie1);
            leaf21 = new Leaf(board, fastZombie1);
            leaf22 = new Leaf(board, fastZombie1);
            leaf23 = new Leaf(board, fastZombie1);
            leaf24 = new Leaf(board, fastZombie1);
            leaf25 = new Leaf(board, fastZombie1);
            leaf26 = new Leaf(board, fastZombie1);
            leaf27 = new Leaf(board, fastZombie1);
            leaf28 = new Leaf(board, fastZombie1);
            leaf29 = new Leaf(board, fastZombie1);
            leaf30 = new Leaf(board, fastZombie1);
            leaf31 = new Leaf(board, fastZombie1);
            leaf32 = new Leaf(board, fastZombie1);
            leaf33 = new Leaf(board, fastZombie1);
            leaf34 = new Leaf(board, fastZombie1);
            leaf35 = new Leaf(board, fastZombie1);
            leaf36 = new Leaf(board, fastZombie1);
            leaf37 = new Leaf(board, fastZombie1);
            leaf38 = new Leaf(board, fastZombie1);
            leaf39 = new Leaf(board, fastZombie1);
            leaf40 = new Leaf(board, fastZombie1);
            
            String zombieString = "This a zombie,\n don't near him \nas a human!!";
            zombieMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, zombieString);

            String darwinString = "This is darwin,\n move with arrows, \n z to transform, \n a for actions";
            darwinMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, darwinString);

            String switchString = "This is a switch\n face it and press A\n to see what happens!!";
            switchMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, switchString);

            String brainString = "Move the brain as a \nzombie.\n Zombie's like brains!!";
            brainMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, brainString);

            String fastString = "This one likes\n to sleep.\n Be careful\n not to wake him!!";
            fastMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, fastString);

            secondStair = new Stairs(board);

            brain = new Brain(board, 5, 18);

            BasicObject[] removableWallsAroundStairs = setRemovableWallsAroundStairs();

            BasicObject[] removableWallsAroundSwitch = setRemovableWallsAroundSwitch();

            BasicObject switchSquareOne = new BasicObject(board);
            switchSquareOne.X = 30;
            switchSquareOne.Y = 2;
            firstSwitch = new Switch(switchSquareOne, board, removableWallsAroundSwitch);

            BasicObject switchSquareTwo = new BasicObject(board);
            switchSquareTwo.X = 2;
            switchSquareTwo.Y = 21;
            secondSwitch = new Switch(switchSquareTwo, board, removableWallsAroundStairs);

            darwin.setGridPosition(2, 2);

            if (board.isGridPositionOpen(darwin))
            {
                board.setGridPositionOccupied(darwin.X, darwin.Y);
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;

            if (board.isGridPositionOpen(27, 21))
            {
                secondStair.setGridPosition(30, 21);
                secondStair.setDestination(board.getPosition(30, 21));
            }

            leaf.setGridPosition(7, 7);
            leaf2.setGridPosition(5, 15);
            leaf3.setGridPosition(4, 2);
            leaf4.setGridPosition(19, 7);
            leaf5.setGridPosition(11, 21);
            leaf6.setGridPosition(7, 8);
            leaf7.setGridPosition(8, 17);
            leaf8.setGridPosition(19, 2);
            leaf9.setGridPosition(19, 1);
            leaf10.setGridPosition(10, 14);
            leaf11.setGridPosition(13, 4);
            leaf12.setGridPosition(13, 3);
            leaf13.setGridPosition(19, 16);
            leaf14.setGridPosition(21, 7);
            leaf15.setGridPosition(2, 16);
            leaf16.setGridPosition(10, 18);
            leaf17.setGridPosition(3, 16);
            leaf18.setGridPosition(16, 15);
            leaf19.setGridPosition(18, 8);
            leaf20.setGridPosition(8, 5);
            leaf21.setGridPosition(5, 7);
            leaf22.setGridPosition(9, 5);
            leaf23.setGridPosition(2, 6);
            leaf24.setGridPosition(8, 8);
            leaf25.setGridPosition(14, 6);
            leaf26.setGridPosition(15, 7);
            leaf27.setGridPosition(15, 8);
            leaf28.setGridPosition(14, 10);
            leaf29.setGridPosition(16, 18);
            leaf30.setGridPosition(14, 22);
            leaf31.setGridPosition(24, 2);
            leaf32.setGridPosition(24, 3);
            leaf33.setGridPosition(25, 6);
            leaf34.setGridPosition(22, 8);
            leaf35.setGridPosition(26, 6);
            leaf36.setGridPosition(25, 10);
            leaf37.setGridPosition(24, 11);
            leaf38.setGridPosition(23, 14);
            leaf39.setGridPosition(22, 17);
            leaf40.setGridPosition(26, 20);

            zTime = new ZombieTime(board);
            zTimeReset = new ZombieTime(board);
        }

        private BasicObject[] setRemovableWallsAroundStairs()
        {
            //later add an x and y to the constructor
            BasicObject s1 = new BasicObject(board);
            s1.X = 28;
            s1.Y = 19;

            BasicObject s2 = new BasicObject(board);
            s2.X = 28;
            s2.Y = 20;

            BasicObject s3 = new BasicObject(board);
            s3.X = 28;
            s3.Y = 21;

            BasicObject s4 = new BasicObject(board);
            s4.X = 28;
            s4.Y = 22;

            BasicObject s5 = new BasicObject(board);
            s5.X = 28;
            s5.Y = 19;

            BasicObject s6 = new BasicObject(board);
            s6.X = 29;
            s6.Y = 19;

            BasicObject s7 = new BasicObject(board);
            s7.X = 30;
            s7.Y = 19;

            BasicObject s8 = new BasicObject(board);
            s8.X = 31;
            s8.Y = 19;

            BasicObject[] squares = new BasicObject[8] { s1, s2, s3, s4, s5, s6, s7, s8 };
            return squares;
        }

        private BasicObject[] setRemovableWallsAroundSwitch()
        {
            BasicObject s1 = new BasicObject(board);
            s1.X = 1;
            s1.Y = 19;

            BasicObject s2 = new BasicObject(board);
            s2.X = 2;
            s2.Y = 19;

            BasicObject s3 = new BasicObject(board);
            s3.X = 3;
            s3.Y = 19;

            BasicObject s4 = new BasicObject(board);
            s4.X = 4;
            s4.Y = 19;

            //BasicObject s5 = new BasicObject(board);
            //s5.X = 4;
            //s5.Y = 22;

            BasicObject s6 = new BasicObject(board);
            s6.X = 4;
            s6.Y = 20;

            BasicObject s7 = new BasicObject(board);
            s7.X = 4;
            s7.Y = 21;

            BasicObject s8 = new BasicObject(board);
            s8.X = 4;
            s8.Y = 22;

            BasicObject[] squares = new BasicObject[7] { s1, s2, s3, s4, s6, s7, s8 };
            return squares;
        }

        public void LoadContent()
        {
            messageFont = mainGame.Content.Load<SpriteFont>("TimesNewRoman");

            Texture2D darwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");

            Texture2D darwinUpTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/ZombieDarwin");

            Texture2D zombieTex = mainGame.Content.Load<Texture2D>("ZombiePic/Zombie");
            Texture2D messagePic = mainGame.Content.Load<Texture2D>("messageBox");

            Texture2D zombieFastTex = mainGame.Content.Load<Texture2D>("ZombiePic/FastZombie");
            Texture2D wholeLeafTex = mainGame.Content.Load<Texture2D>("leaf");
            Texture2D brokeLeafTex = mainGame.Content.Load<Texture2D>("brokenleaf");

            // Test
            Texture2D basicGridTex = mainGame.Content.Load<Texture2D>("StaticPic/Level3/metal_tile_light");
            Texture2D basicMenuTex = mainGame.Content.Load<Texture2D>("StaticPic/Level3/side_wall_yellow");

            Texture2D basicStairUpTex = mainGame.Content.Load<Texture2D>("StaticPic/stairsUp");
            Texture2D basicStairDownTex = mainGame.Content.Load<Texture2D>("StaticPic/stairsDown");

            // Texture for the wall and switch
            Texture2D wallTex = mainGame.Content.Load<Texture2D>("StaticPic/Wall");
            Texture2D switchTex = mainGame.Content.Load<Texture2D>("StaticPic/Switch");

            Texture2D brainTex = mainGame.Content.Load<Texture2D>("brain");

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");

            secondStair.LoadContent(basicStairUpTex, basicStairDownTex, "Down");

            firstSwitch.LoadContent(wallTex, switchTex);
            secondSwitch.LoadContent(wallTex, switchTex);

            brain.LoadContent(brainTex);

            // Test
            board.LoadContent(basicGridTex);
            board.LoadBackgroundContent(basicMenuTex);

            //darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, darwinRightTex, darwinLeftTex, zombieDarwinTex);
            firstZombie.LoadContent(zombieTex);

            fastZombie1.LoadContent(zombieFastTex);
            leaf.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf2.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf3.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf4.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf5.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf6.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf7.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf8.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf9.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf10.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf11.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf12.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf13.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf14.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf15.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf16.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf17.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf18.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf19.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf20.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf21.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf22.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf23.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf24.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf25.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf26.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf27.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf28.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf29.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf30.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf31.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf32.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf33.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf34.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf35.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf36.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf37.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf38.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf39.LoadContent(brokeLeafTex, wholeLeafTex);
            leaf40.LoadContent(brokeLeafTex, wholeLeafTex);

            //secondZombie.LoadContent(zombieTex);
            //thirdZombie.LoadContent(zombieTex);
            zombieMessage.LoadContent(messagePic);
            darwinMessage.LoadContent(messagePic);
            switchMessage.LoadContent(messagePic);
            brainMessage.LoadContent(messagePic);
            fastMessage.LoadContent(messagePic);

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("startScreen"));

            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));

        }


        //protected override void UnloadContent() { }

        public void Update(GameTime gameTime)
        {
            switch (gameState.getState())
            {
                case GameState.state.Start:
                    UpdateStartState();
                    break;
                case GameState.state.Level:
                    if (!messageMode)
                        UpdateLevelState(gameTime);
                    else
                        UpdateMessageMode();
                    break;
                case GameState.state.End:
                    UpdateEndState();
                    break;
            }

        }

        private void UpdateMessageMode()
        {
            KeyboardState ks = Keyboard.GetState();
            messageModeCounter++;

            zombieMessage.pointToSquare(firstZombie.X, firstZombie.Y, board);
            darwinMessage.pointToSquare(darwin.X, darwin.Y, board);
            switchMessage.pointToSquare(firstSwitch.X, firstSwitch.Y, board);
            brainMessage.pointToSquare(brain.X, brain.Y, board);
            fastMessage.pointToSquare(fastZombie1.X, fastZombie1.Y, board);
            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = false;
                messageModeCounter = 0;
            }
        }

        private void UpdateStartState()
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Enter))
            {
                MediaPlayer.Play(song);
                gameState.setState(GameState.state.Level);
            }
        }

        private void UpdateLevelState(GameTime gameTime)
        {
            if (darwin.isZombie())
            {
                if (zTime.isTimedOut())
                {
                    gameOver = true;
                }
                else
                {
                    zTime.Update(gameTime);
                }
            }

            KeyboardState ks = Keyboard.GetState();

            checkForExitGame(ks);

            updateKeyHeldDown(ks);
            /*
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
            }*/

            if (!darwin.isZombie())
            {
                checkForGameOver(firstZombie);
                checkForGameOver(fastZombie1);
            }

            darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            secondStair.Update(gameTime, darwin);

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime, darwin, brain);

            leaf.Update(darwin);
            leaf2.Update(darwin);
            leaf3.Update(darwin);
            leaf4.Update(darwin);
            leaf5.Update(darwin);
            leaf6.Update(darwin);
            leaf7.Update(darwin);
            leaf8.Update(darwin);
            leaf9.Update(darwin);
            leaf10.Update(darwin);
            leaf11.Update(darwin);
            leaf12.Update(darwin);
            leaf13.Update(darwin);
            leaf14.Update(darwin);
            leaf15.Update(darwin);
            leaf16.Update(darwin);
            leaf17.Update(darwin);
            leaf18.Update(darwin);
            leaf19.Update(darwin);
            leaf20.Update(darwin);
            leaf21.Update(darwin);
            leaf22.Update(darwin);
            leaf23.Update(darwin);
            leaf24.Update(darwin);
            leaf25.Update(darwin);
            leaf26.Update(darwin);
            leaf27.Update(darwin);
            leaf28.Update(darwin);
            leaf29.Update(darwin);
            leaf30.Update(darwin);
            leaf31.Update(darwin);
            leaf32.Update(darwin);
            leaf33.Update(darwin);
            leaf34.Update(darwin);
            leaf35.Update(darwin);
            leaf36.Update(darwin);
            leaf37.Update(darwin);
            leaf38.Update(darwin);
            leaf39.Update(darwin);
            leaf40.Update(darwin);

            fastZombie1.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            fastZombie1.Update(gameTime, darwin, brain);
            
            //secondZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            //secondZombie.Update(gameTime, darwin, brain);
            //thirdZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            //thirdZombie.Update(gameTime, darwin, brain);

            firstSwitch.Update(gameTime, ks, darwin);
            secondSwitch.Update(gameTime, ks, darwin);

            brain.Update(gameTime, ks, darwin);

            checkForSwitchToLevelFour();
            //checkForGameWin();

            if (gameOver || gameWin)
            {
                gameState.setState(GameState.state.End);
            }

            if (ks.IsKeyDown(Keys.H) && messageModeCounter > 10)
            {
                messageMode = true;
                messageModeCounter = 0;
            }
            messageModeCounter++;

        }

        private void UpdateEndState()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q))
            {
                mainGame.Exit();
            }
            if (ks.IsKeyDown(Keys.R))
            {
                board.setGridPositionOpen(darwin);
                darwin.setGridPosition(2, 2);

                if (gameWin)
                {
                    zTime.reset();
                    mainGame.setCurLevel(Game1.LevelState.Level1);
                }
                else if (gameOver)
                {
                    zTime = new ZombieTime(board);
                    zTime.reset();
                    zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
                    zTime.setTime(zTimeReset.getTime());
                }

                board.setGridPositionOpen(firstZombie);

                firstZombie.setGridPosition(10, 10);
                //secondZombie.setAbsoluteDestination(10, 16);
                //thirdZombie.setAbsoluteDestination(16, 10); 

                leaf.resetLeaf();
                leaf2.resetLeaf();
                leaf3.resetLeaf();
                leaf4.resetLeaf();
                leaf5.resetLeaf();
                leaf6.resetLeaf();
                leaf7.resetLeaf();
                leaf8.resetLeaf();
                leaf9.resetLeaf();
                leaf10.resetLeaf();
                leaf11.resetLeaf();
                leaf12.resetLeaf();
                leaf13.resetLeaf();
                leaf14.resetLeaf();
                leaf15.resetLeaf();
                leaf16.resetLeaf();
                leaf17.resetLeaf();
                leaf18.resetLeaf();
                leaf19.resetLeaf();
                leaf20.resetLeaf();
                leaf21.resetLeaf();
                leaf22.resetLeaf();
                leaf23.resetLeaf();
                leaf24.resetLeaf();
                leaf25.resetLeaf();
                leaf26.resetLeaf();
                leaf27.resetLeaf();
                leaf28.resetLeaf();
                leaf29.resetLeaf();
                leaf30.resetLeaf();
                leaf31.resetLeaf();
                leaf32.resetLeaf();
                leaf33.resetLeaf();
                leaf34.resetLeaf();
                leaf35.resetLeaf();
                leaf36.resetLeaf();
                leaf37.resetLeaf();
                leaf38.resetLeaf();
                leaf39.resetLeaf();
                leaf40.resetLeaf();

                board.setGridPositionOpen(fastZombie1);
                fastZombie1.chasingDarwin = false;
                fastZombie1.goBackToSleep();
                fastZombie1.setGridPosition(15, 15);

                darwin.setHuman();
                firstSwitch.turnOn();
                secondSwitch.turnOn();

                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
            }

        }

        private void checkForExitGame(KeyboardState ks)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                mainGame.Exit();

            if (ks.IsKeyDown(Keys.Q))
            {
                mainGame.Exit();
            }
        }

        private void checkForGameOver(Zombie myZombie)
        {
            if (darwin.isOnTop(myZombie))
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


                if (rightSideOfDarwin == myZombie.destination ||
                    leftSideOfDarwin == myZombie.destination ||
                    onTopOfDarwin == myZombie.destination ||
                    onBottomOfDarwin == myZombie.destination)
                {
                    gameOver = true;
                }
            }
        }

        private void checkForSwitchToLevelFour()
        {
            if (darwin.isOnTop(secondStair))
            {
                mainGame.setCurLevel(Game1.LevelState.Level4);
                mainGame.setZTimeLevel(zTime, Game1.LevelState.Level4);

                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
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

        public void setZTime(ZombieTime mytime)
        {
            zTime = mytime;

            zTimeReset = new ZombieTime(board);
            zTimeReset.reset();
            zTimeReset.setTime(mytime.getTime());
        }

        public void Draw(GameTime gameTime)
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

        }

        private void DrawStartState()
        {
            mainGame.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            gameStart.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawLevelState(GameTime gameTime)
        {
            mainGame.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            board.Draw(spriteBatch);

            secondStair.Draw(spriteBatch);

            leaf.Draw(spriteBatch);
            leaf2.Draw(spriteBatch);
            leaf3.Draw(spriteBatch);
            leaf4.Draw(spriteBatch);
            leaf5.Draw(spriteBatch);
            leaf6.Draw(spriteBatch);
            leaf7.Draw(spriteBatch);
            leaf8.Draw(spriteBatch);
            leaf9.Draw(spriteBatch);
            leaf10.Draw(spriteBatch);
            leaf11.Draw(spriteBatch);
            leaf12.Draw(spriteBatch);
            leaf13.Draw(spriteBatch);
            leaf14.Draw(spriteBatch);
            leaf15.Draw(spriteBatch);
            leaf16.Draw(spriteBatch);
            leaf17.Draw(spriteBatch);
            leaf18.Draw(spriteBatch);
            leaf19.Draw(spriteBatch);
            leaf20.Draw(spriteBatch);
            leaf21.Draw(spriteBatch);
            leaf22.Draw(spriteBatch);
            leaf23.Draw(spriteBatch);
            leaf24.Draw(spriteBatch);
            leaf25.Draw(spriteBatch);
            leaf26.Draw(spriteBatch);
            leaf27.Draw(spriteBatch);
            leaf28.Draw(spriteBatch);
            leaf29.Draw(spriteBatch);
            leaf30.Draw(spriteBatch);
            leaf31.Draw(spriteBatch);
            leaf32.Draw(spriteBatch);
            leaf33.Draw(spriteBatch);
            leaf34.Draw(spriteBatch);
            leaf35.Draw(spriteBatch);
            leaf36.Draw(spriteBatch);
            leaf37.Draw(spriteBatch);
            leaf38.Draw(spriteBatch);
            leaf39.Draw(spriteBatch);
            leaf40.Draw(spriteBatch);

            darwin.Draw(spriteBatch);
            firstZombie.Draw(spriteBatch);

            fastZombie1.Draw(spriteBatch);
            

            //secondZombie.Draw(spriteBatch);
            //thirdZombie.Draw(spriteBatch);
            firstSwitch.Draw(spriteBatch);
            secondSwitch.Draw(spriteBatch);
            brain.Draw(spriteBatch);
            zTime.Draw(spriteBatch);

            if (messageMode)
            {
                //zombieMessage.Draw(spriteBatch, messageFont);
                darwinMessage.Draw(spriteBatch, messageFont);
                //brainMessage.Draw(spriteBatch, messageFont);
                fastMessage.Draw(spriteBatch, messageFont);
                //switchMessage.Draw(spriteBatch, messageFont);
            }

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

    }
}
