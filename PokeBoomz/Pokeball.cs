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
    class Pokeball
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        public Rectangle drawRec;
        public Vector2 position;
        public Texture2D texture;

        private float alpha = 1.0f;
        private float rotation = 0.0f;
        public Vector2 origin, positionCenter;
        public float scale = 0.5f;
        private SpriteEffects spriteEffect = SpriteEffects.None;
        private float zDepth = 0.1f;
        private float angle = 45, power = 0;
        public bool isThrown = false, initialized = false, turnLeft, hitPokemon = false;
        private const float degtorad = (float)Math.PI / 180;
        private Vector2 ivelocity, fvelocity;

        public MouseState mouseState;

        public Pokeball()
        {
        }

        public Pokeball(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent(ContentManager Content, String imag)
        {
            texture = Content.Load<Texture2D>(imag);
            drawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Update(bool isThrown, float rotation, float angle, float _power, bool turnLeft)
        {
            this.turnLeft = turnLeft;
            this.isThrown = isThrown;
            this.angle = angle * degtorad;
            this.rotation = rotation;
            mouseState = Mouse.GetState();

            positionCenter.X = position.X + texture.Width / 2;
            positionCenter.Y = position.Y + texture.Height / 2;

            if (isThrown)
            {
                this.power = _power;
                this.ivelocity.X = (float)(power / 3) * (float)Math.Cos(this.angle);
                this.ivelocity.Y = (float)(power / 3) * (float)Math.Sin(this.angle);
                if (!this.initialized)
                {
                    this.initialized = true;
                    this.fvelocity.X = this.ivelocity.X;
                    this.fvelocity.Y = this.ivelocity.Y;
                }
                else
                {
                    this.fvelocity.X += (float)PokeBoomzGame.windPower / 100.0f;
                    this.fvelocity.Y -= (float)PokeBoomzGame.gravity / 2;
                }
                if (fvelocity.Y >= 0)
                {
                    this.position.Y -= this.fvelocity.Y;
                }
                else
                {
                    this.position.Y -= this.fvelocity.Y;
                }
                if (turnLeft) this.position.X -= this.fvelocity.X;
                else
                    this.position.X += this.fvelocity.X;
            }
        }

        public bool isCollapsed()
        {
            if (this.position.Y >= PokeBoomzGame.groundY)
            {
                return true;
            }
            if (this.position.X >= PokeBoomzGame.displayWidth || this.position.X < 0)
            {
                return true;
            }
            if (hitPokemon) return true;
            return false;
        }

        public void Draw(float ballSize)
        {
            spriteBatch.Draw(texture, position, drawRec,
                Color.White * alpha, rotation, origin, ballSize, spriteEffect,
                zDepth);
        }
    }
}