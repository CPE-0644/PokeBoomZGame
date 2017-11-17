using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokeBoomz
{
    class Obstacle
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        public Rectangle drawRec;
        public Vector2 position, originPos;
        private Texture2D texture;
        private float alpha = 1.0f;
        private float rotation = 0.0f;
        private Vector2 origin;
        private float scale = 1.0f;
        private SpriteEffects spriteEffect = SpriteEffects.None;
        private float zDepth = 0.1f;
        public MouseState lastMouseState, mouseState;

        public Obstacle(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent(ContentManager Content, String objectImag, float positionX, float positionY)
        {
            texture = Content.Load<Texture2D>(objectImag);
            drawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(0, 0);
            position = new Vector2(positionX, positionY);
            originPos = position;
        }
        public void Update()
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public void Draw()
        {
            spriteBatch.Draw(texture, position, drawRec,
                Color.White * alpha, rotation, origin, scale, spriteEffect,
                zDepth);
        }

        public bool checkMouseInBox()
        {
            Console.WriteLine(mouseState.X + " " + mouseState.Y);
            Console.WriteLine(originPos.X + " " +originPos.Y);
            if (mouseState.X >= originPos.X &&
                mouseState.X <= position.X + drawRec.Width &&
                mouseState.Y >= originPos.Y &&
                mouseState.Y <= position.Y + drawRec.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void togglePosition()
        {
            if (checkMouseInBox())
            {
                if (position.Y <= originPos.Y + 10)
                {
                    position.Y+=5;
                }
            }
            else
            {
                if (position.Y >= originPos.Y)
                    position.Y -= 5;
            }
        }

        public bool isClick()
        {
            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (checkMouseInBox())
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}