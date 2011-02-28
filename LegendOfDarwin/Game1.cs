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
    /// This is the main type for your game
    /// This is a test comment.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Darwin darwin;

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
            // TODO: Add your initialization logic here

            darwin = new Darwin();
            darwin.SetPosition(0.0f, 0.0f);

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

            detectDarwinMovement(ks);
            detectDarwinTransform(ks);

            //currently does nothing
            darwin.Update(gameTime);

            base.Update(gameTime);
        }

        private void detectDarwinMovement(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Right))
            {
                darwin.SetPosition((darwin.position.X + 1.0f), darwin.position.Y);
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                darwin.SetPosition((darwin.position.X - 1.0f), darwin.position.Y);
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                darwin.SetPosition(darwin.position.X, (darwin.position.Y - 1.0f));
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                darwin.SetPosition(darwin.position.X, (darwin.position.Y + 1.0f));
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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
