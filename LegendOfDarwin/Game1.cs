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
using LegendOfDarwin.GameObject;

namespace LegendOfDarwin
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Level1 level1;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            level1 = new Level1(this);
        }

        protected override void Initialize()
        {
            
            level1.graphics = graphics;
            InitializeGraphics();
            level1.Initialize();
            
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            level1.spriteBatch = spriteBatch;
            

            

            level1.song = Content.Load<Song>("thriller");
            level1.LoadContent();
        }

        private void InitializeGraphics()
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.ApplyChanges();
        }
            
        

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            level1.Update(gameTime);
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            level1.Draw(gameTime);
            base.Draw(gameTime);
            
        }

    }
}
