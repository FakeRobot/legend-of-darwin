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

namespace LegendOfDarwin.Level
{
    public class Level4
    {

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GraphicsDevice device;

        private GameState gameState;
        private GameStart gameStart;

        private Darwin darwin;
        private Zombie firstZombie, secondZombie, thirdZombie;
        private CongaLeaderZombie leaderZombie;
        private Switch firstSwitch;
        private GameBoard board;

        private ZombieTime zTime;
        private ZombieTime zTimeReset; //what zTime should reset to

        private Potion potion;
        private Stairs stairs;

        private BasicObject[] walls;
        private Texture2D wallTex;

        public SpriteFont messageFont;
        public bool keyIsHeldDown = false;
        public bool gameOver = false;
        public bool gameWin = false;

        private int counter;
        private int counterReady;
        public Texture2D gameOverTexture;
        public Texture2D gameWinTexture;

        Vector2 gameOverPosition = Vector2.Zero;


        // things for managing message boxes
        bool messageMode = false;
        int messageModeCounter = 0;
        private MessageBox zombieMessage;
        private MessageBox darwinMessage;
        private MessageBox switchMessage;

        public Song song;
        public Game1 mainGame;

        public Level4(Game1 myMainGame)
        {
            mainGame = myMainGame;
        }

        public void Initialize()
        {
            gameOverPosition.X = 320;
            gameOverPosition.Y = 130;

            device = graphics.GraphicsDevice;

            gameState = new GameState();
            gameStart = new GameStart(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
            gameState.setState(GameState.state.Level);

            board = new GameBoard(new Vector2(33, 25), new Vector2(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
            darwin = new Darwin(board);
            //firstZombie = new Zombie(10, 10, 15, 5, 15, 5, board);
            //secondZombie = new Zombie(10, 16, 15, 5, 15, 5, board);
            //thirdZombie = new Zombie(12, 10, 15, 5, 15, 5, board);

            Vector2[] myPath = new Vector2[4]; 
            myPath[0] = new Vector2(7,7);
            myPath[1] = new Vector2(7,19);
            myPath[2] = new Vector2(20,19);
            myPath[3] = new Vector2(20,7);

            leaderZombie = new CongaLeaderZombie(7,7,board.getNumSquaresX(),0,board.getNumSquaresY(),0,myPath,darwin,board);

            String zombieString = "This a zombie,\n don't near him \nas a human!!";
            zombieMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, zombieString);

            String darwinString = "This is darwin,\n move with arrows, \n z to transform, \n a for actions";
            darwinMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, darwinString);

            String switchString = "This is a switch\n face it and press A\n to see what happens!!";
            switchMessage = new MessageBox(board.getPosition(12, 8).X, board.getPosition(10, 10).Y, switchString);

            stairs = new Stairs(board);

            BasicObject[] removableWalls = setRemovableWallsInLevelOne();

            BasicObject switchSquare = new BasicObject(board);
            switchSquare.X = 5;
            switchSquare.Y = 10;

            firstSwitch = new Switch(switchSquare, board, removableWalls);

            // Initial starting position
            darwin.setGridPosition(16, 22);

            if (board.isGridPositionOpen(darwin))
            {
                board.setGridPositionOccupied(darwin.X, darwin.Y);
                darwin.setPosition(board.getPosition(darwin).X, board.getPosition(darwin).Y);
            }

            // Darwin's lag movement
            counterReady = counter = 5;

            if (board.isGridPositionOpen(16, 1))
            {
                stairs.setGridPosition(16, 1);
                stairs.setDestination(board.getPosition(16, 1));
            }

            zTime = new ZombieTime(board);
            zTimeReset = new ZombieTime(board);

            setPotionPosition(25, 4);

            setWallsInLevelFour();
        }

        private BasicObject[] setRemovableWallsInLevelOne()
        {

            BasicObject s1 = new BasicObject(board);
            s1.X = 28;
            s1.Y = 19;

            BasicObject[] removableWalls = new BasicObject[1] {s1};
            return removableWalls;
        }

        private void setPotionPosition(int x, int y)
        {
            potion = new Potion(board);
            potion.setDestination(board.getPosition(x, y));
            potion.setGridPosition(x, y);
            board.setGridPositionOccupied(x, y);
        }

        private void setWalls()
        {
            BasicObject w1 = new BasicObject(board);
            w1.setGridPosition(11, 1);
            board.setGridPositionOccupied(11, 1);
            walls = new BasicObject[1] { w1 };
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
            darwin.LoadContent(graphics.GraphicsDevice, darwinUpTex, darwinDownTex, darwinRightTex, darwinLeftTex, zombieDarwinTex);

            gameOverTexture = mainGame.Content.Load<Texture2D>("gameover");
            gameWinTexture = mainGame.Content.Load<Texture2D>("gamewin");

            stairs.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/stairsUp"),
                mainGame.Content.Load<Texture2D>("StaticPic/stairsDown"), "Down");

            firstSwitch.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/Wall"),
                mainGame.Content.Load<Texture2D>("StaticPic/Switch"));

            board.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/Level4/metal_tile_medium"));
            board.LoadBackgroundContent(mainGame.Content.Load<Texture2D>("StaticPic/Level4/side_wall_purple"));

            wallTex = mainGame.Content.Load<Texture2D>("StaticPic/Level4/side_wall_purple");

            //darwin.LoadContent(graphics.GraphicsDevice, darwinTex, zombieDarwinTex);

            //firstZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            //secondZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));
            //thirdZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/Zombie"));

            leaderZombie.LoadContent(mainGame.Content.Load<Texture2D>("ZombiePic/CongaLeaderZombie"));
        
            zombieMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            darwinMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));
            switchMessage.LoadContent(mainGame.Content.Load<Texture2D>("messageBox"));

            gameStart.LoadContent(mainGame.Content.Load<Texture2D>("startScreen"));
            zTime.LoadContent(mainGame.Content.Load<Texture2D>("humanities_bar"));
            potion.LoadContent(mainGame.Content.Load<Texture2D>("StaticPic/potion"));

            
        }

        private void setWallsInLevelFour()
        {
            //lower area wall
            BasicObject w0 = new BasicObject(board);
            BasicObject w1 = new BasicObject(board);
            BasicObject w2 = new BasicObject(board);
            BasicObject w3 = new BasicObject(board);
            BasicObject w4 = new BasicObject(board);
            BasicObject w5 = new BasicObject(board);
            BasicObject w6 = new BasicObject(board);
            BasicObject w7 = new BasicObject(board);
            BasicObject w8 = new BasicObject(board);
            BasicObject w9 = new BasicObject(board);
            BasicObject w9p = new BasicObject(board);
            BasicObject w10 = new BasicObject(board);
            BasicObject w11 = new BasicObject(board);
            BasicObject w12 = new BasicObject(board);
            BasicObject w13 = new BasicObject(board);
            BasicObject w14 = new BasicObject(board);
            BasicObject w15 = new BasicObject(board);
            BasicObject w16 = new BasicObject(board);
            BasicObject w17 = new BasicObject(board);
            BasicObject w18 = new BasicObject(board);
            BasicObject w19 = new BasicObject(board);
            BasicObject w20 = new BasicObject(board);
            BasicObject w21 = new BasicObject(board);
            BasicObject w22 = new BasicObject(board);
            BasicObject w23 = new BasicObject(board);
            BasicObject w24 = new BasicObject(board);
            BasicObject w25 = new BasicObject(board);
            BasicObject w26 = new BasicObject(board);
            BasicObject w27 = new BasicObject(board);
            BasicObject w28 = new BasicObject(board);
            BasicObject w29 = new BasicObject(board);
            BasicObject w30 = new BasicObject(board);
            BasicObject w31 = new BasicObject(board);
            BasicObject w32 = new BasicObject(board);
            BasicObject w33 = new BasicObject(board);
            BasicObject w34 = new BasicObject(board);
            BasicObject w35 = new BasicObject(board);
            BasicObject w36 = new BasicObject(board);
            BasicObject w37 = new BasicObject(board);
            BasicObject w38 = new BasicObject(board);
            BasicObject w39 = new BasicObject(board);
            BasicObject w40 = new BasicObject(board);
            BasicObject w41 = new BasicObject(board);
            BasicObject w42 = new BasicObject(board);
            BasicObject w43 = new BasicObject(board);
            BasicObject w44 = new BasicObject(board);
            BasicObject w45 = new BasicObject(board);
            BasicObject w46 = new BasicObject(board);
            BasicObject w47 = new BasicObject(board);
            BasicObject w48 = new BasicObject(board);
            BasicObject w49 = new BasicObject(board);
            BasicObject w50 = new BasicObject(board);
            BasicObject w51 = new BasicObject(board);
            BasicObject w52 = new BasicObject(board);
            BasicObject w53 = new BasicObject(board);
            BasicObject w54 = new BasicObject(board);
            BasicObject w55 = new BasicObject(board);
            BasicObject w56 = new BasicObject(board);
            BasicObject w57 = new BasicObject(board);
            BasicObject w58 = new BasicObject(board);
            BasicObject w59 = new BasicObject(board);
            BasicObject w60 = new BasicObject(board);
            BasicObject w61 = new BasicObject(board);
            BasicObject w62 = new BasicObject(board);
            BasicObject w63 = new BasicObject(board);
            BasicObject w64 = new BasicObject(board);
            BasicObject w65 = new BasicObject(board);
            BasicObject w66 = new BasicObject(board);
            BasicObject w67 = new BasicObject(board);
            BasicObject w68 = new BasicObject(board);
            BasicObject w69 = new BasicObject(board);
            BasicObject w70 = new BasicObject(board);
            BasicObject w71 = new BasicObject(board);
            BasicObject w72 = new BasicObject(board);
            BasicObject w73 = new BasicObject(board);
            BasicObject w74 = new BasicObject(board);
            BasicObject w75 = new BasicObject(board);
            BasicObject w76 = new BasicObject(board);
            BasicObject w77 = new BasicObject(board);
            BasicObject w78 = new BasicObject(board);
            BasicObject w79 = new BasicObject(board);
            BasicObject w80 = new BasicObject(board);
            BasicObject w81 = new BasicObject(board);
            BasicObject w82 = new BasicObject(board);
            BasicObject w83 = new BasicObject(board);
            BasicObject w84 = new BasicObject(board);
            BasicObject w85 = new BasicObject(board);

            // first lower row
            w0.setGridPosition(1, 22);
            w1.setGridPosition(2, 22);
            w2.setGridPosition(3, 22);
            w3.setGridPosition(4, 22);
            w4.setGridPosition(5, 22);
            w5.setGridPosition(6, 22);
            w6.setGridPosition(7, 22);
            w7.setGridPosition(8, 22);
            w8.setGridPosition(9, 22);
            w9p.setGridPosition(10,22);
            w9.setGridPosition(11, 22);
            w10.setGridPosition(12, 22);
            w11.setGridPosition(13, 22);
            w12.setGridPosition(14, 22);
            w13.setGridPosition(18, 22);
            w14.setGridPosition(19, 22);
            w15.setGridPosition(20, 22);
            w16.setGridPosition(21, 22);
            w17.setGridPosition(22, 22);
            w18.setGridPosition(23, 22);
            w19.setGridPosition(24, 22);
            w20.setGridPosition(25, 22);
            w21.setGridPosition(26, 22);
            w22.setGridPosition(27, 22);
            w23.setGridPosition(28, 22);
            w24.setGridPosition(29, 22);
            w25.setGridPosition(30, 22);
            w26.setGridPosition(31, 22);
            w27.setGridPosition(32, 22);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 22);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 22);
            }

            //second lower row
            w28.setGridPosition(1, 21);
            w29.setGridPosition(2, 21);
            w30.setGridPosition(3, 21);
            w31.setGridPosition(4, 21);
            w32.setGridPosition(5, 21);
            w33.setGridPosition(6, 21);
            w34.setGridPosition(7, 21);
            w35.setGridPosition(8, 21);
            w36.setGridPosition(9, 21);
            w37.setGridPosition(10, 21);
            w38.setGridPosition(11, 21);
            w39.setGridPosition(12, 21);
            w40.setGridPosition(13, 21);
            w41.setGridPosition(14, 21);
            w42.setGridPosition(18, 21);
            w43.setGridPosition(19, 21);
            w44.setGridPosition(20, 21);
            w45.setGridPosition(21, 21);
            w46.setGridPosition(22, 21);
            w47.setGridPosition(23, 21);
            w48.setGridPosition(24, 21);
            w49.setGridPosition(25, 21);
            w50.setGridPosition(26, 21);
            w51.setGridPosition(27, 21);
            w52.setGridPosition(28, 21);
            w53.setGridPosition(29, 21);
            w54.setGridPosition(30, 21);
            w55.setGridPosition(31, 21);
            w56.setGridPosition(32, 21);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 21);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 21);
            }

            //third lower row
            w57.setGridPosition(1, 20);
            w58.setGridPosition(2, 20);
            w59.setGridPosition(3, 20);
            w60.setGridPosition(4, 20);
            w61.setGridPosition(5, 20);
            w62.setGridPosition(6, 20);
            w63.setGridPosition(7, 20);
            w64.setGridPosition(8, 20);
            w65.setGridPosition(9, 20);
            w66.setGridPosition(10, 20);
            w67.setGridPosition(11, 20);
            w68.setGridPosition(12, 20);
            w69.setGridPosition(13, 20);
            w70.setGridPosition(14, 20);
            w71.setGridPosition(18, 20);
            w72.setGridPosition(19, 20);
            w73.setGridPosition(20, 20);
            w74.setGridPosition(21, 20);
            w75.setGridPosition(22, 20);
            w76.setGridPosition(23, 20);
            w77.setGridPosition(24, 20);
            w78.setGridPosition(25, 20);
            w79.setGridPosition(26, 20);
            w80.setGridPosition(27, 20);
            w81.setGridPosition(28, 20);
            w82.setGridPosition(29, 20);
            w83.setGridPosition(30, 20);
            w84.setGridPosition(31, 20);
            w85.setGridPosition(32, 20);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 20);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 20);
            }

            ////upper long horizontal wall
            //BasicObject w19 = new BasicObject(board);
            //BasicObject w20 = new BasicObject(board);
            //BasicObject w21 = new BasicObject(board);
            //BasicObject w22 = new BasicObject(board);
            //BasicObject w23 = new BasicObject(board);
            //BasicObject w24 = new BasicObject(board);
            //BasicObject w25 = new BasicObject(board);
            //BasicObject w26 = new BasicObject(board);
            //BasicObject w27 = new BasicObject(board);
            //BasicObject w28 = new BasicObject(board);
            //BasicObject w29 = new BasicObject(board);
            //BasicObject w30 = new BasicObject(board);
            //BasicObject w31 = new BasicObject(board);
            //BasicObject w32 = new BasicObject(board);
            //BasicObject w33 = new BasicObject(board);
            //BasicObject w34 = new BasicObject(board);
            //BasicObject w35 = new BasicObject(board);
            //BasicObject w36 = new BasicObject(board);
            //BasicObject w48 = new BasicObject(board);
            //BasicObject w49 = new BasicObject(board);
            //BasicObject w50 = new BasicObject(board);
            //BasicObject w51 = new BasicObject(board);
            //BasicObject w52 = new BasicObject(board);

            //w19.setGridPosition(1, 10);
            //w20.setGridPosition(2, 10);
            //w21.setGridPosition(2, 10);
            //w22.setGridPosition(3, 10);
            //w23.setGridPosition(4, 10);
            //w24.setGridPosition(5, 10);
            //w25.setGridPosition(6, 10);
            //w26.setGridPosition(7, 10);
            //w27.setGridPosition(8, 10);
            //w28.setGridPosition(9, 10);
            //w29.setGridPosition(10, 10);
            //w30.setGridPosition(11, 10);
            //w31.setGridPosition(12, 10);
            //w32.setGridPosition(13, 10);
            //w33.setGridPosition(14, 10);
            //w34.setGridPosition(15, 10);
            //w48.setGridPosition(16, 10);
            //w49.setGridPosition(17, 10);
            //w50.setGridPosition(18, 10);
            //w51.setGridPosition(19, 10);
            //w52.setGridPosition(20, 10);

            //for (int n = 1; n < 21; n++)
            //{
            //    board.setGridPositionOccupied(n, 10);
            //}

            ////short vertical wall
            //BasicObject w37 = new BasicObject(board);
            //BasicObject w38 = new BasicObject(board);
            //BasicObject w39 = new BasicObject(board);
            //BasicObject w40 = new BasicObject(board);
            //BasicObject w41 = new BasicObject(board);
            //BasicObject w42 = new BasicObject(board);

            //w37.setGridPosition(26, 1);
            //w38.setGridPosition(26, 2);
            //w39.setGridPosition(26, 3);
            //w40.setGridPosition(26, 4);
            //w41.setGridPosition(26, 5);
            //w42.setGridPosition(26, 6);

            //for (int k = 1; k < 7; k++)
            //{
            //    board.setGridPositionOccupied(26, k);
            //}

            //upper area wall
            BasicObject w86 = new BasicObject(board);
            BasicObject w87 = new BasicObject(board);
            BasicObject w88 = new BasicObject(board);
            BasicObject w89 = new BasicObject(board);
            BasicObject w90 = new BasicObject(board);
            BasicObject w91 = new BasicObject(board);
            BasicObject w92 = new BasicObject(board);
            BasicObject w93 = new BasicObject(board);
            BasicObject w94 = new BasicObject(board);
            BasicObject w95 = new BasicObject(board);
            BasicObject w96 = new BasicObject(board);
            BasicObject w97 = new BasicObject(board);
            BasicObject w98 = new BasicObject(board);
            BasicObject w99 = new BasicObject(board);
            BasicObject w100 = new BasicObject(board);
            BasicObject w101 = new BasicObject(board);
            BasicObject w102 = new BasicObject(board);
            BasicObject w103 = new BasicObject(board);
            BasicObject w104 = new BasicObject(board);
            BasicObject w105 = new BasicObject(board);
            BasicObject w106 = new BasicObject(board);
            BasicObject w107 = new BasicObject(board);
            BasicObject w108 = new BasicObject(board);
            BasicObject w109 = new BasicObject(board);
            BasicObject w110 = new BasicObject(board);
            BasicObject w111 = new BasicObject(board);
            BasicObject w112 = new BasicObject(board);
            BasicObject w113 = new BasicObject(board);
            BasicObject w114 = new BasicObject(board);
            BasicObject w115 = new BasicObject(board);
            BasicObject w116 = new BasicObject(board);
            BasicObject w117 = new BasicObject(board);
            BasicObject w118 = new BasicObject(board);
            BasicObject w119 = new BasicObject(board);
            BasicObject w120 = new BasicObject(board);
            BasicObject w121 = new BasicObject(board);
            BasicObject w122 = new BasicObject(board);
            BasicObject w123 = new BasicObject(board);
            BasicObject w124 = new BasicObject(board);
            BasicObject w125 = new BasicObject(board);
            BasicObject w126 = new BasicObject(board);
            BasicObject w127 = new BasicObject(board);
            BasicObject w128 = new BasicObject(board);
            BasicObject w129 = new BasicObject(board);
            BasicObject w130 = new BasicObject(board);
            BasicObject w131 = new BasicObject(board);
            BasicObject w132 = new BasicObject(board);
            BasicObject w133 = new BasicObject(board);
            BasicObject w134 = new BasicObject(board);
            BasicObject w135 = new BasicObject(board);
            BasicObject w136 = new BasicObject(board);
            BasicObject w137 = new BasicObject(board);
            BasicObject w138 = new BasicObject(board);
            BasicObject w139 = new BasicObject(board);
            BasicObject w140 = new BasicObject(board);
            BasicObject w141 = new BasicObject(board);
            BasicObject w142 = new BasicObject(board);
            BasicObject w143 = new BasicObject(board);
            BasicObject w144 = new BasicObject(board);
            BasicObject w145 = new BasicObject(board);
            BasicObject w146 = new BasicObject(board);
            BasicObject w147 = new BasicObject(board);
            BasicObject w148 = new BasicObject(board);
            BasicObject w149 = new BasicObject(board);
            BasicObject w150 = new BasicObject(board);
            BasicObject w151 = new BasicObject(board);
            BasicObject w152 = new BasicObject(board);
            BasicObject w153 = new BasicObject(board);
            BasicObject w154 = new BasicObject(board);
            BasicObject w155 = new BasicObject(board);
            BasicObject w156 = new BasicObject(board);
            BasicObject w157 = new BasicObject(board);
            BasicObject w158 = new BasicObject(board);
            BasicObject w159 = new BasicObject(board);
            BasicObject w160 = new BasicObject(board);
            BasicObject w161 = new BasicObject(board);
            BasicObject w162 = new BasicObject(board);
            BasicObject w163 = new BasicObject(board);
            BasicObject w164 = new BasicObject(board);
            BasicObject w165 = new BasicObject(board);
            BasicObject w166 = new BasicObject(board);
            BasicObject w167 = new BasicObject(board);
            BasicObject w168 = new BasicObject(board);
            BasicObject w169 = new BasicObject(board);
            BasicObject w170 = new BasicObject(board);
            BasicObject w171 = new BasicObject(board);
            BasicObject w172 = new BasicObject(board);

            // first lower row
            w86.setGridPosition(1, 1);
            w87.setGridPosition(2, 1);
            w88.setGridPosition(3, 1);
            w89.setGridPosition(4, 1);
            w90.setGridPosition(5, 1);
            w91.setGridPosition(6, 1);
            w92.setGridPosition(7, 1);
            w93.setGridPosition(8, 1);
            w94.setGridPosition(9, 1);
            w95.setGridPosition(10, 1);
            w96.setGridPosition(11, 1);
            w97.setGridPosition(12, 1);
            w98.setGridPosition(13, 1);
            w99.setGridPosition(14, 1);
            w100.setGridPosition(18, 1);
            w101.setGridPosition(19, 1);
            w102.setGridPosition(20, 1);
            w103.setGridPosition(21, 1);
            w104.setGridPosition(22, 1);
            w105.setGridPosition(23, 1);
            w106.setGridPosition(24, 1);
            w107.setGridPosition(25, 1);
            w108.setGridPosition(26, 1);
            w109.setGridPosition(27, 1);
            w110.setGridPosition(28, 1);
            w111.setGridPosition(29, 1);
            w112.setGridPosition(30, 1);
            w113.setGridPosition(31, 1);
            w114.setGridPosition(32, 1);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 1);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 1);
            }

            //second lower row
            w115.setGridPosition(1, 2);
            w116.setGridPosition(2, 2);
            w117.setGridPosition(3, 2);
            w118.setGridPosition(4, 2);
            w119.setGridPosition(5, 2);
            w120.setGridPosition(6, 2);
            w121.setGridPosition(7, 2);
            w122.setGridPosition(8, 2);
            w123.setGridPosition(9, 2);
            w124.setGridPosition(10, 2);
            w125.setGridPosition(11, 2);
            w126.setGridPosition(12, 2);
            w127.setGridPosition(13, 2);
            w128.setGridPosition(14, 2);
            w129.setGridPosition(18, 2);
            w130.setGridPosition(19, 2);
            w131.setGridPosition(20, 2);
            w132.setGridPosition(21, 2);
            w133.setGridPosition(22, 2);
            w134.setGridPosition(23, 2);
            w135.setGridPosition(24, 2);
            w136.setGridPosition(25, 2);
            w137.setGridPosition(26, 2);
            w138.setGridPosition(27, 2);
            w139.setGridPosition(28, 2);
            w140.setGridPosition(29, 2);
            w141.setGridPosition(30, 2);
            w142.setGridPosition(31, 2);
            w143.setGridPosition(32, 2);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 2);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 2);
            }

            //third lower row
            w144.setGridPosition(1, 3);
            w145.setGridPosition(2, 3);
            w146.setGridPosition(3, 3);
            w147.setGridPosition(4, 3);
            w148.setGridPosition(5, 3);
            w149.setGridPosition(6, 3);
            w150.setGridPosition(7, 3);
            w151.setGridPosition(8, 3);
            w152.setGridPosition(9, 3);
            w153.setGridPosition(10, 3);
            w154.setGridPosition(11, 3);
            w155.setGridPosition(12, 3);
            w156.setGridPosition(13, 3);
            w157.setGridPosition(14, 3);
            w158.setGridPosition(18, 3);
            w159.setGridPosition(19, 3);
            w160.setGridPosition(20, 3);
            w161.setGridPosition(21, 3);
            w162.setGridPosition(22, 3);
            w163.setGridPosition(23, 3);
            w164.setGridPosition(24, 3);
            w165.setGridPosition(25, 3);
            w166.setGridPosition(26, 3);
            w167.setGridPosition(27, 3);
            w168.setGridPosition(28, 3);
            w169.setGridPosition(29, 3);
            w170.setGridPosition(30, 3);
            w171.setGridPosition(31, 3);
            w172.setGridPosition(32, 3);

            for (int m = 1; m < 15; m++)
            {
                board.setGridPositionOccupied(m, 3);
            }
            for (int m = 18; m < 33; m++)
            {
                board.setGridPositionOccupied(m, 3);
            }

            walls = new BasicObject[174] {w0, w1, w2, w3, w4, w5, w6, w7, w8, w9, w9p,w10, w11, 
                w12, w13, w14, w15, w16, w17, w18, 
                 w19, w20, w21, w22, w23, w24, w25,w26,w27,
            w28, w29, w30, w31, w32, w33, w34, w35, w36, w37, w38,w39, w40, 
                w41, w42, w43, w44, w45, w46, w47, 
                 w48, w49, w50, w51, w52, w53, w54,w55,w56,
            w57, w58, w59, w60, w61, w62, w63, w64, w65, w66, w67,w68, w69, 
                w70, w71, w72, w73, w74, w75, w76, 
                 w77, w78, w79, w80, w81, w82, w83,w84,w85,
            w86, w87, w88, w89, w90, w91, w92, w93, w94, w95, w96,w97, w98, 
                w99, w100, w101, w102, w103, w104, w105, 
                 w106, w107, w108, w109, w110, w111, w112,w113,w114,
            w115, w116, w117, w118, w119, w120, w121, w122, w123, w124, w125,w126, w127, 
                w128, w129, w130, w131, w132, w133, w134, 
                 w135, w136, w137, w138, w139, w140, w141,w142,w143,
            w144, w145, w146, w147, w148, w149, w150, w151, w152, w153, w154,w155, w156, 
                w157, w158, w159, w160, w161, w162, 
                w163, w164, w165, w166, w167, w168,w169,w170, w171, w172};
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

            //zombieMessage.pointToSquare(firstZombie.X, firstZombie.Y, board);
            darwinMessage.pointToSquare(darwin.X, darwin.Y, board);
            switchMessage.pointToSquare(firstSwitch.X, firstSwitch.Y, board);

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
                //MediaPlayer.Play(song);
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
                checkForGameOver(leaderZombie);
            }

            darwin.Update(gameTime, ks, board, darwin.X, darwin.Y);

            stairs.Update(gameTime, darwin);

            //firstZombie.Update(gameTime, darwin);
            //secondZombie.Update(gameTime, darwin);
            //thirdZombie.Update(gameTime, darwin);
            leaderZombie.Update(gameTime, darwin);

            firstSwitch.Update(gameTime, ks, darwin);

            potion.Update(gameTime, ks, darwin, zTime);

            

            //checkForGameWin();
            checkForSwitchToLevelFive();

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

                board.setGridPositionOpen(darwin);
                darwin.setGridPosition(16, 20);

                gameOver = false;
                gameWin = false;

                //board.setGridPositionOpen(firstZombie);
                //board.setGridPositionOpen(secondZombie);
                //board.setGridPositionOpen(thirdZombie);
                board.setGridPositionOpen(potion);

                //firstZombie.setGridPosition(10, 10);
                //board.setGridPositionOccupied(firstZombie.X, firstZombie.Y);
                //firstZombie.setZombieAlive(true);

                //secondZombie.setGridPosition(10, 16);
                //board.setGridPositionOccupied(secondZombie.X, secondZombie.Y);
                //secondZombie.setZombieAlive(true);

                //thirdZombie.setGridPosition(12, 10);
                //board.setGridPositionOccupied(thirdZombie.X, thirdZombie.Y);
                //thirdZombie.setZombieAlive(true);

                leaderZombie.Reset(7, 7);

                potion.setGridPosition(25, 4);
                board.setGridPositionOccupied(potion.X, potion.Y);

                potion.reset();
                darwin.setHuman();
                gameState.setState(GameState.state.Level);
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

        private void checkForGameWin()
        {
            if (darwin.isOnTop(stairs))
            {
                gameWin = true;
            }
        }

        private void checkForSwitchToLevelFive()
        {
            if (darwin.isOnTop(stairs)) 
            {
                mainGame.setCurLevel(Game1.LevelState.Level5);
                mainGame.setZTimeLevel(zTime,Game1.LevelState.Level5);

                darwin.setHuman();
                gameState.setState(GameState.state.Level);
                gameOver = false;
                gameWin = false;
            }
        }

        public void setZTime(ZombieTime mytime)
        {
            zTime = mytime;

            zTimeReset = new ZombieTime(board);
            zTimeReset.reset();
            zTimeReset.setTime(mytime.getTime());
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
            stairs.Draw(spriteBatch);

            darwin.Draw(spriteBatch);
            //firstZombie.Draw(spriteBatch);
            //secondZombie.Draw(spriteBatch);
            //thirdZombie.Draw(spriteBatch);
            leaderZombie.Draw(spriteBatch);

            firstSwitch.Draw(spriteBatch);
            zTime.Draw(spriteBatch);
            potion.Draw(spriteBatch);

            if (messageMode)
            {
                zombieMessage.Draw(spriteBatch, messageFont);
                darwinMessage.Draw(spriteBatch, messageFont);
                //switchMessage.Draw(spriteBatch, messageFont);
            }

            foreach(BasicObject a in walls)
            {
                spriteBatch.Draw(wallTex, board.getPosition(a.X, a.Y), Color.White);
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




