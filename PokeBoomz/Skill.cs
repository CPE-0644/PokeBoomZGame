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
    class Skill
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        public Rectangle drawRec;
        public Vector2 position;
        private Texture2D texture;

        private float alpha = 1.0f;
        private float rotation = 0.0f;
        public Vector2 origin;
        private float scale = 0.8f;
        private SpriteEffects spriteEffect = SpriteEffects.None;
        private float zDepth = 0.1f;

        public bool used;
        public string effect;
        public int sp;
        public Keys key;
        public bool effectON;
        

        public Skill()
        {
        }

        public Skill(GraphicsDeviceManager graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent(ContentManager Content, String imag, float X, float Y)
        {
            texture = Content.Load<Texture2D>(imag);
            drawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            position = new Vector2(X,Y);
        }

        public void LoadEffect(string name, int sp, Keys keys)
        {
            this.effect = name;
            this.sp = sp;
            this.key = keys;
        }

        public void Update()
        {
            if (used)
            {
                Console.WriteLine("wfj");
                alpha = 0.5f;
            }
        }

        public void Draw()
        {
            spriteBatch.Draw(texture, position, drawRec,
                Color.White * alpha, rotation, origin, scale, spriteEffect,
                zDepth);
        }
    }
}