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
        private GameBoard board;
        public GraphicsDevice device;
        public SpriteFont messageFont;

        private FastZombie fastZombie1;
        private LinkedList<Leaf> leaves;

        // nice and big
        private int leafCount = 137;

        private Darwin darwin;
        private Zombie firstZombie;

        private Switch firstSwitch, secondSwitch;
        private Brain brain;
        
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;
        private int gameOverCounter = 0;

        private SoundEffect deathScreamSound;
        private bool playDeathSound = true;

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

        //public Song song;
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
            gameState.setState(GameState.state.Start);
            gameStart = new GameStart(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);

            board = new GameBoard(new Vector2(33, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board);
            firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);
            //secondZombie = new Zombie(10, 16, 15, 5, 15, 5, board);
            //thirdZombie = new Zombie(16, 10, 15, 5, 15, 5, board);

            fastZombie1 = new FastZombie(15, 15, board.getNumSquaresX(), 0, board.getNumSquaresY(), 0, board);

            this.leaves = new LinkedList<Leaf>();

            for (int i = 0; i < leafCount; i++)
            {
                this.leaves.AddLast(new Leaf(board, fastZombie1));
            }

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
            
            // add all the leaves to the map...
            leaves.ElementAt(0).setGridPosition(7, 7);
            leaves.ElementAt(1).setGridPosition(5, 15);
            leaves.ElementAt(2).setGridPosition(4, 2);
            leaves.ElementAt(3).setGridPosition(19, 7);
            leaves.ElementAt(4).setGridPosition(11, 21);
            leaves.ElementAt(5).setGridPosition(7, 8);
            leaves.ElementAt(6).setGridPosition(8, 17);
            leaves.ElementAt(7).setGridPosition(19, 2);
            leaves.ElementAt(8).setGridPosition(19, 1);
            leaves.ElementAt(9).setGridPosition(10, 14);
            leaves.ElementAt(10).setGridPosition(13, 4);
            leaves.ElementAt(11).setGridPosition(13, 3);
            leaves.ElementAt(12).setGridPosition(19, 16);
            leaves.ElementAt(13).setGridPosition(21, 7);
            leaves.ElementAt(14).setGridPosition(2, 16);
            leaves.ElementAt(15).setGridPosition(10, 18);
            leaves.ElementAt(16).setGridPosition(3, 16);
            leaves.ElementAt(17).setGridPosition(16, 15);
            leaves.ElementAt(18).setGridPosition(18, 8);
            leaves.ElementAt(19).setGridPosition(8, 5);
            leaves.ElementAt(20).setGridPosition(5, 7);
            leaves.ElementAt(21).setGridPosition(9, 5);
            leaves.ElementAt(22).setGridPosition(2, 6);
            leaves.ElementAt(23).setGridPosition(8, 8);
            leaves.ElementAt(24).setGridPosition(14, 6);
            leaves.ElementAt(25).setGridPosition(15, 7);
            leaves.ElementAt(26).setGridPosition(15, 8);
            leaves.ElementAt(27).setGridPosition(14, 10);
            leaves.ElementAt(28).setGridPosition(16, 18);
            leaves.ElementAt(29).setGridPosition(14, 22);
            leaves.ElementAt(30).setGridPosition(24, 2);
            leaves.ElementAt(31).setGridPosition(24, 3);
            leaves.ElementAt(32).setGridPosition(25, 6);
            leaves.ElementAt(33).setGridPosition(22, 8);
            leaves.ElementAt(34).setGridPosition(26, 6);
            leaves.ElementAt(35).setGridPosition(25, 10);
            leaves.ElementAt(36).setGridPosition(24, 11);
            leaves.ElementAt(37).setGridPosition(23, 14);
            leaves.ElementAt(38).setGridPosition(22, 17);
            leaves.ElementAt(39).setGridPosition(26, 20);
            leaves.ElementAt(41).setGridPosition(2, 22);
            leaves.ElementAt(42).setGridPosition(2, 20);
            leaves.ElementAt(43).setGridPosition(3, 21);
            leaves.ElementAt(44).setGridPosition(1, 21);
            leaves.ElementAt(45).setGridPosition(2, 18);
            leaves.ElementAt(46).setGridPosition(1, 18);
            leaves.ElementAt(47).setGridPosition(3, 18);
            leaves.ElementAt(48).setGridPosition(4, 18);
            leaves.ElementAt(49).setGridPosition(5, 19);
            leaves.ElementAt(50).setGridPosition(5, 20);
            leaves.ElementAt(51).setGridPosition(5, 21);
            leaves.ElementAt(52).setGridPosition(5, 22);
            leaves.ElementAt(53).setGridPosition(4, 1);
            leaves.ElementAt(54).setGridPosition(4, 2);
            leaves.ElementAt(55).setGridPosition(4, 3);
            leaves.ElementAt(56).setGridPosition(4, 4);
            leaves.ElementAt(57).setGridPosition(4, 5);
            leaves.ElementAt(58).setGridPosition(4, 6);
            leaves.ElementAt(59).setGridPosition(4, 7);
            leaves.ElementAt(60).setGridPosition(4, 8);
            leaves.ElementAt(61).setGridPosition(4, 9);
            leaves.ElementAt(62).setGridPosition(4, 10);
            leaves.ElementAt(63).setGridPosition(4, 11);
            leaves.ElementAt(64).setGridPosition(4, 12);
            leaves.ElementAt(65).setGridPosition(4, 13);
            leaves.ElementAt(66).setGridPosition(10, 4);
            leaves.ElementAt(67).setGridPosition(10, 5);
            leaves.ElementAt(68).setGridPosition(10, 6);
            leaves.ElementAt(69).setGridPosition(10, 7);
            leaves.ElementAt(70).setGridPosition(10, 8);
            leaves.ElementAt(71).setGridPosition(10, 9);
            leaves.ElementAt(72).setGridPosition(10, 10);
            leaves.ElementAt(73).setGridPosition(10, 11);
            leaves.ElementAt(74).setGridPosition(10, 12);
            leaves.ElementAt(75).setGridPosition(10, 13);
            leaves.ElementAt(76).setGridPosition(10, 14);
            leaves.ElementAt(77).setGridPosition(10, 15);
            leaves.ElementAt(78).setGridPosition(10, 16);
            leaves.ElementAt(79).setGridPosition(10, 17);
            leaves.ElementAt(80).setGridPosition(10, 18);
            leaves.ElementAt(81).setGridPosition(10, 19);
            leaves.ElementAt(82).setGridPosition(11, 19);
            leaves.ElementAt(83).setGridPosition(12, 19);
            leaves.ElementAt(84).setGridPosition(13, 19);
            leaves.ElementAt(85).setGridPosition(14, 19);
            leaves.ElementAt(86).setGridPosition(15, 19);
            leaves.ElementAt(87).setGridPosition(16, 19);
            leaves.ElementAt(88).setGridPosition(17, 19);
            leaves.ElementAt(89).setGridPosition(18, 19);
            leaves.ElementAt(90).setGridPosition(19, 19);
            leaves.ElementAt(91).setGridPosition(20, 19);
            leaves.ElementAt(92).setGridPosition(21, 19);
            leaves.ElementAt(93).setGridPosition(22, 19);
            leaves.ElementAt(94).setGridPosition(23, 19);
            leaves.ElementAt(95).setGridPosition(23, 18);
            leaves.ElementAt(96).setGridPosition(23, 17);
            leaves.ElementAt(97).setGridPosition(23, 16);
            leaves.ElementAt(98).setGridPosition(23, 15);
            leaves.ElementAt(99).setGridPosition(23, 14);
            leaves.ElementAt(100).setGridPosition(23, 13);
            leaves.ElementAt(101).setGridPosition(23, 12);
            leaves.ElementAt(102).setGridPosition(23, 11);
            leaves.ElementAt(103).setGridPosition(23, 10);
            leaves.ElementAt(104).setGridPosition(23, 9);
            leaves.ElementAt(105).setGridPosition(23, 8);
            leaves.ElementAt(106).setGridPosition(23, 7);
            leaves.ElementAt(107).setGridPosition(23, 6);
            leaves.ElementAt(108).setGridPosition(23, 5);
            leaves.ElementAt(109).setGridPosition(23, 4);
            leaves.ElementAt(110).setGridPosition(22, 4);
            leaves.ElementAt(111).setGridPosition(21, 4);
            leaves.ElementAt(112).setGridPosition(20, 4);
            leaves.ElementAt(113).setGridPosition(19, 4);
            leaves.ElementAt(114).setGridPosition(18, 4);
            leaves.ElementAt(115).setGridPosition(17, 4);
            leaves.ElementAt(116).setGridPosition(16, 4);
            leaves.ElementAt(117).setGridPosition(15, 4);
            leaves.ElementAt(118).setGridPosition(14, 4);
            leaves.ElementAt(119).setGridPosition(13, 4);
            leaves.ElementAt(120).setGridPosition(12, 4);
            leaves.ElementAt(121).setGridPosition(11, 4);

            leaves.ElementAt(134).setGridPosition(27, 1);
            leaves.ElementAt(135).setGridPosition(27, 2);
            leaves.ElementAt(136).setGridPosition(27, 3);

            leaves.ElementAt(122).setGridPosition(27, 4);
            leaves.ElementAt(123).setGridPosition(27, 5);
            leaves.ElementAt(124).setGridPosition(27, 6);
            leaves.ElementAt(125).setGridPosition(27, 7);
            leaves.ElementAt(126).setGridPosition(27, 8);
            leaves.ElementAt(127).setGridPosition(27, 9);
            leaves.ElementAt(128).setGridPosition(27, 10);
            leaves.ElementAt(129).setGridPosition(27, 11);
            leaves.ElementAt(130).setGridPosition(27, 12);
            leaves.ElementAt(131).setGridPosition(27, 13);
            leaves.ElementAt(132).setGridPosition(27, 14);
            leaves.ElementAt(133).setGridPosition(27, 15);

            zTime = new ZombieTime(board);
            zTimeReset = new ZombieTime(board);
        }

        public void setLevelState()
        {
            board.setGridPositionOpen(darwin);
            darwin.setGridPosition(2, 2);

            board.setGridPositionOpen(firstZombie);

            firstZombie.setGridPosition(10, 10);

            // reset each leaf for new level
            foreach (Leaf leaf in this.leaves)
            {
                leaf.resetLeaf();
            }

            board.setGridPositionOpen(fastZombie1);
            fastZombie1.chasingDarwin = false;
            fastZombie1.goBackToSleep();
            fastZombie1.setGridPosition(15, 15);

            board.setGridPositionOpen(brain);
            brain.setGridPosition(5, 18);

            darwin.setHuman();
            darwin.setDarwinAlive();
            playDeathSound = true;
            gameOverCounter = 0;
            firstSwitch.turnOn();
            secondSwitch.turnOn();
            board.setGridPositionOccupied(firstSwitch);
            board.setGridPositionOccupied(secondSwitch);

            gameOver = false;
            gameWin = false;

            Keyboard.GetState();
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

            deathScreamSound = mainGame.Content.Load<SoundEffect>("chewScream");

            Texture2D darwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");

            Texture2D darwinUpTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinUp");
            Texture2D darwinDownTex = mainGame.Content.Load<Texture2D>("DarwinPic/Darwin");
            Texture2D darwinRightTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinRight");
            Texture2D darwinLeftTex = mainGame.Content.Load<Texture2D>("DarwinPic/DarwinLeft");
            Texture2D zombieDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/ZombieDarwin");
            Texture2D deadDarwinTex = mainGame.Content.Load<Texture2D>("DarwinPic/DeadDarwin");

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
            Texture2D menuBarTexture = mainGame.Content.Load<Texture2D>("menubar");

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");

            secondStair.LoadContent(basicStairUpTex, basicStairDownTex, "Down");

            SoundEffect switchSound = mainGame.Content.Load<SoundEffect>("switch");
            firstSwitch.LoadContent(wallTex, switchTex, switchSound);
            secondSwitch.LoadContent(wallTex, switchTex, switchSound);

            brain.LoadContent(brainTex);

            board.LoadContent(basicGridTex);
            board.LoadBackgroundContent(basicMenuTex);
            board.loadMenuBar(menuBarTexture);

            SoundEffect transformSound = mainGame.Content.Load<SoundEffect>("transform");
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex,
                darwinRightTex, darwinLeftTex, zombieDarwinTex,deadDarwinTex, transformSound);

            firstZombie.LoadContent(zombieTex, mainGame.Content.Load<Texture2D>("ZombieSkull"));

            fastZombie1.LoadContent(zombieFastTex);

            SoundEffect leafSound = mainGame.Content.Load<SoundEffect>("leaves");

            // load leaf texture for all leaves
            foreach(Leaf leaf in this.leaves)
            {
                leaf.LoadContent(brokeLeafTex, wholeLeafTex, leafSound);
            }

            zombieMessage.LoadContent(messagePic);
            darwinMessage.LoadContent(messagePic);
            switchMessage.LoadContent(messagePic);
            brainMessage.LoadContent(messagePic);
            fastMessage.LoadContent(messagePic);

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("SplashScreens/Level3"));

            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));

        }

        public void Update(GameTime gameTime)
        {
            switch (gameState.getState())
            {
                case GameState.state.Start:
                    UpdateStartState();
                    break;
                case GameState.state.Level:
                    if (!messageMode && !gameOver)
                        UpdateLevelState(gameTime);
                    else if (messageMode)
                        UpdateMessageMode();
                    else
                    {
                        if (playDeathSound)
                        {
                            deathScreamSound.Play();
                            playDeathSound = false;
                        }

                        // for freezing screen when darwin dies
                        darwin.setDarwinDead();
                        darwin.setZombie();
                        UpdateLevelState(gameTime);
                        gameOverCounter++;
                        if (gameOverCounter > 200)
                        {
                            gameState.setState(GameState.state.End);
                            gameOverCounter = 0;
                        }
                    }
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
                setLevelState();
                gameState.setState(GameState.state.Level);
            }
        }

        private void UpdateLevelState(GameTime gameTime)
        {
            if (darwin.isZombie() && darwin.isDarwinAlive())
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

            if (darwin.isDarwinAlive())
            {
                if (!darwin.isZombie())
                {
                    checkForGameOver(firstZombie);
                    checkForGameOver(fastZombie1);
                }

                darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);
            }

            secondStair.Update(gameTime, darwin);

            firstZombie.setPictureSize(board.getSquareWidth(), board.getSquareLength());
            firstZombie.Update(gameTime, darwin, brain);

            foreach (Leaf leaf in this.leaves)
            {
                leaf.Update(darwin);
            }

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
                mainGame.setCurLevel(Game1.LevelState.Start);
                setLevelState();
                gameState.setState(GameState.state.Start);
            }
            if (ks.IsKeyDown(Keys.R))
            {
                setLevelState();
                gameState.setState(GameState.state.Level);
                mainGame.DEATH_COUNTER++;
            }

        }

        private void checkForExitGame(KeyboardState ks)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                mainGame.Exit();

            if (ks.IsKeyDown(Keys.Escape))
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
                zTime.reset();
                mainGame.setZTimeLevel(zTime, Game1.LevelState.Level4);

                darwin.setHuman();
                gameState.setState(GameState.state.Start);
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

            foreach (Leaf leaf in this.leaves)
            {
                leaf.Draw(spriteBatch);
            }

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

            spriteBatch.DrawString(messageFont, "HUMANITY: ",
                new Vector2(board.getPosition(zTime.X, zTime.Y).X + board.getSquareWidth() * 3 / 2, board.getPosition(zTime.X, 24).Y),
                Color.Black);

            spriteBatch.DrawString(messageFont, "DEATH COUNT: " + mainGame.DEATH_COUNTER.ToString(),
                new Vector2(board.getPosition(23, 24).X, board.getPosition(23, 24).Y),
                Color.Black);

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
