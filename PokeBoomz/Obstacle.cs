﻿using System;
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
        private Rectangle drawRec;
        public Vector2 position;
        private Texture2D texture;
        private float alpha = 1.0f;
        private float rotation = 0.0f;
        private Vector2 origin;
        private float scale = 1.0f;
        private SpriteEffects spriteEffect = SpriteEffects.None;
        private float zDepth = 0.1f;

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
        }

        public void Update()
        {
        }

        public void Draw()
        {
            spriteBatch.Draw(texture, position, drawRec,
                Color.White * alpha, rotation, origin, scale, spriteEffect,
                zDepth);
        }
    }
}