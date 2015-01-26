#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Pong
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D paddleTexture, ballTexture;
        private Vector2 ballPosition, LPaddlePos, RPaddlePos, centreBall;
        private KeyboardState keyboard;
        private float paddleSpeed, dX, dY;
        private int P1score, P2score;
        private Rectangle leftPaddleRect, rightPaddleRect, ballRect;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            paddleSpeed = 10.5f;
            dY = dX = 5.5f;
            P1score = P2score = 0;

            LPaddlePos.X = 20;
            LPaddlePos.Y = (GraphicsDevice.Viewport.Height / 2) - (paddleTexture.Height / 2);

            RPaddlePos.X = GraphicsDevice.Viewport.Width - paddleTexture.Width - 20;
            RPaddlePos.Y = (GraphicsDevice.Viewport.Height / 2) - (paddleTexture.Height / 2);

            centreBall.X = (GraphicsDevice.Viewport.Width + ballTexture.Width) / 2;
            centreBall.Y = (GraphicsDevice.Viewport.Height - ballTexture.Width) / 2;

            ballPosition = centreBall;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            paddleTexture = Content.Load<Texture2D>("paddle");
            ballTexture= Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.W))
                LPaddlePos.Y -= paddleSpeed;

            if (keyboard.IsKeyDown(Keys.S))
                LPaddlePos.Y += paddleSpeed;

            LPaddlePos.Y = MathHelper.Clamp(LPaddlePos.Y, 0, GraphicsDevice.Viewport.Height - paddleTexture.Height);

            if (keyboard.IsKeyDown(Keys.O))
                RPaddlePos.Y -= paddleSpeed;

            if (keyboard.IsKeyDown(Keys.L))
                RPaddlePos.Y += paddleSpeed;

            RPaddlePos.Y = MathHelper.Clamp(RPaddlePos.Y, 0, GraphicsDevice.Viewport.Height - paddleTexture.Height);

            leftPaddleRect = new Rectangle((int)LPaddlePos.X, (int)LPaddlePos.Y, paddleTexture.Width, paddleTexture.Height);
            rightPaddleRect = new Rectangle((int)RPaddlePos.X, (int)RPaddlePos.Y, paddleTexture.Width, paddleTexture.Height);
            ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, ballTexture.Width, ballTexture.Height);

            if (ballPosition.Y <= 0 || ballPosition.Y >= (GraphicsDevice.Viewport.Height - ballTexture.Height))
                dY = -dY;

            if (ballRect.Intersects(leftPaddleRect) || ballRect.Intersects(rightPaddleRect))
                dX = -dX;

            if (ballPosition.X <= 0)
            {
                P2score++;
                ballPosition = centreBall;
            }

            if (ballPosition.X >= GraphicsDevice.Viewport.Width - ballTexture.Width)
            {
                P1score++;
                ballPosition = centreBall;
            }

            ballPosition.X += dX;
            ballPosition.Y += dY;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(paddleTexture, LPaddlePos, Color.White);
            spriteBatch.Draw(paddleTexture, RPaddlePos, Color.White);
            spriteBatch.Draw(ballTexture, ballPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}