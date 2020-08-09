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

namespace VerletPhysicsTest1
{    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VerletObject VerletObject;

        List<VerletObject> VerletList = new List<VerletObject>();

        MouseState CurrentMouseState, PreviousMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            //VerletObject = new VerletObject();            
            VerletObject verlet = new VerletObject();
            verlet.LoadContent(Content);
            VerletList.Add(verlet);
            //VerletList.Add(new VerletObject());
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //VerletObject.LoadContent(Content);
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            CurrentMouseState = Mouse.GetState();

            if (PreviousMouseState.LeftButton == ButtonState.Released &&
                CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                VerletObject verlet = new VerletObject();
                verlet.LoadContent(Content);
                VerletList.Add(verlet);
            }

            foreach (VerletObject verlet in VerletList)
            {
                verlet.Update(gameTime);
            }
            //VerletObject.Update(gameTime);

            PreviousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (VerletObject verlet in VerletList)
            {
                verlet.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
