using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PokeBoomz
{
    public class PokeBoomzGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        public static Texture2D dummyTexture;
        private SpriteBatch spriteBatch;
        private KeyboardState keyboardState, lastKeyboardState;
        public Random rand = new Random();
        public static double gravity = 0.5f;
        private int player_role = 1;
        public int millisec = 0, sec = 0, turnTimeLimit = 15;
        public bool timesUp = false;
        public static float windPower;
        public float windDirection;


        public static int displayWidth = 1600, displayHeight = 1200;
        public static int groundY = 895;

        private float alpha = 1.0f, textAlpha = 1.0f, windAlpha = 1.0f;
        private float rotation = 0.0f, textRotation = 0.0f;

        private Vector2 origin = new Vector2(0, 0),
            timerPosition = new Vector2(1450, 1100),
            anglePosition = new Vector2(100, 1100),
            windPosition = new Vector2(750, 50),
            turnPosition = new Vector2(700, 1000),
            powerBarPosition = new Vector2(0, 1050);

        public Rectangle windRectangle, powerBarRectangle, EndGameRectangle = new Rectangle(100, 100, 1400, 1000);

        public Texture2D card1, card2, wind, powerBar, endGameTexture;
        public Vector2 card1Position = new Vector2(10, 10), card2Position = new Vector2(10, 250);
        public float p1Trans, p2Trans;

        private float scale = 2.0f, textScale = 0.8f, timerScale = 2.0f, windScale = 1.0f;
        private SpriteEffects spriteEffect, textEffects, windEffects;
        private float zDepth = 0.1f;

        private Pokemon pokemon1, pokemon2, pokemon3, pokemon4, pokemon5, hittedPokemon;
        private Player player1, player2;
        private Obstacle ground1, block1, bg1;
        public Obstacle maxP1, maxP2;
        public Vector2 maxPositionPokemonWin, maxPositionPokemonLose;

        private Dictionary<int, Player> Players = new Dictionary<int, Player>();
        private LinkedList<Pokemon> Pokemons = new LinkedList<Pokemon>();
        private LinkedList<Obstacle> Obstacles = new LinkedList<Obstacle>();

        public static float moveFactor = 1.0f;
        private SpriteFont spriteFont;
        public bool isEffectFinished = false, choosing = false;

        public PokeBoomzGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            //            displayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //            displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            displayWidth = 1600;
            displayHeight = 1200;
            graphics.PreferredBackBufferWidth = displayWidth;
            graphics.PreferredBackBufferHeight = displayHeight;

            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] {Color.OrangeRed});

            spriteFont = Content.Load<SpriteFont>("Arial");

            endGameTexture = new Texture2D(GraphicsDevice, 1, 1);
            endGameTexture.SetData(new Color[] {Color.Black * 0.5f});
            maxP1 = new Obstacle(graphics, spriteBatch);
            maxP2 = new Obstacle(graphics, spriteBatch);

            bg1 = new Obstacle(graphics, spriteBatch);
            bg1.LoadContent(Content, "map/bg01", 0, 0);

            ground1 = new Obstacle(graphics, spriteBatch);
            ground1.LoadContent(Content, "map/ground01", 0, groundY);

            block1 = new Obstacle(graphics, spriteBatch);
            block1.LoadContent(Content, "map/block01", 100, 100);

            card1 = Content.Load<Texture2D>("player_card1");
            card2 = Content.Load<Texture2D>("player_card2");

            wind = Content.Load<Texture2D>("wind");
            windRectangle = new Rectangle(0, 0, wind.Width, wind.Height);
            //  Obstacles.AddLast(block1);

            powerBar = Content.Load<Texture2D>("powerBar");
            powerBarRectangle = new Rectangle(0, 0, powerBar.Width, powerBar.Height);


            for (int i = 0; i < 3; i++)
            {
                int randTypePokemon = rand.Next() % 6;
                Pokemon newPokemon = RandomNewPokemon(randTypePokemon);
                Pokemons.AddLast(newPokemon);
            }


            player1 = new Player(graphics, spriteBatch);
            player1.LoadContent(Content, "character/female/stand",
                "character/female/walk", "character/female/throw",
                "character/female/lose", 1, 3);

            player2 = new Player(graphics, spriteBatch);
            player2.LoadContent(Content, "character/male/stand",
                "character/male/walk", "character/male/throw",
                "character/male/lose", 1, 3);

            player1.myTurn = true;
            player_role = 1;
            p1Trans = 0.8f;
            p2Trans = 0.4f;
            player1.turnRound = 1;
            player2.turnRound = 0;

            Players.Add(1, player1);
            Players.Add(2, player2);


            if (Math.Floor(rand.NextDouble() * 2) == 0)
            {
                windDirection = -1;
                windEffects = SpriteEffects.FlipHorizontally;
            }
            else windDirection = 1;
            windPower = windDirection * (float) Math.Floor(rand.NextDouble() * 5.0f);
        }

        public Pokemon RandomNewPokemon(int type)
        {
            Pokemon newPokemon = new Pokemon(graphics, spriteBatch);
            int randCp = rand.Next() % 100;
            switch (type)
            {
                case 1:
                    newPokemon.initStat("Bird", "fly", randCp);
                    newPokemon.LoadContent(Content, "pokemon/bird_21", 21, 1);
                    break;
                case 2:
                    newPokemon.initStat("Lucia", "fly", randCp);
                    newPokemon.LoadContent(Content, "pokemon/lucia_18", 19, 1);
                    break;

                case 3:
                    newPokemon.initStat("Ho-oh", "fly", randCp);
                    newPokemon.LoadContent(Content, "pokemon/ho-on_15", 15, 1);
                    break;

                case 4:
                    newPokemon.initStat("Crab", "normal", randCp);
                    newPokemon.LoadContent(Content, "pokemon/crab_20", 20, 1);
                    break;

                default:
                    newPokemon.initStat("Monkey", "normal", randCp);
                    newPokemon.LoadContent(Content, "pokemon/monkey_22", 22, 1);
                    break;
            }
            return newPokemon;
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            millisec++;


            if (millisec % 60 == 0)
            {
                millisec = 0;
                sec++;
            }


            TimeUp();
            Pokemons.Remove(hittedPokemon);
            PokemonMoveByType();
            PlayerActivity();

            base.Update(gameTime);
        }

        public void TimeUp() // new Turn
        {
            if (turnTimeLimit - sec == 0)
            {
                Players[1].ballSize = 0.5f;
                Players[2].ballSize = 0.5f;
                moveFactor = 1.0f;
                int randTypePokemon = rand.Next() % 6;
                Pokemon newPokemon = RandomNewPokemon(randTypePokemon);
                Pokemons.AddLast(newPokemon);
                timesUp = true;
                sec = 0;
                isEffectFinished = true;
                swapPlayer();
                foreach (var player in Players)
                {
                    foreach (var valueSkill in player.Value.Skills)
                    {
                        valueSkill.used = false;
                    }
                }
            }
        }

        public bool isEndGame()
        {
            if (playerOutOfBall() || player1.turnRound == 6 || player2.turnRound == 6)
            {
                return true;
            }
            return false;
        }

        public bool playerOutOfBall()
        {
            if (player1.remainedPokeball == 0 && player2.remainedPokeball == 0)
            {
                if (player1.maxCP > player2.maxCP)
                {
                    player2.lose = true;
                    return true;
                }
                else
                {
                    player1.lose = true;
                    return true;
                }
            }
            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            bg1.Draw();
            ground1.Draw();

            foreach (var player in Players)
            {
                player.Value.Draw();


                spriteBatch.DrawString(spriteFont, "Player " + player.Key, player.Value.infoPosition,
                    Color.White * alpha, textRotation, origin, textScale, textEffects, zDepth);
                if (player.Value.remainedPokeball > 0)
                    player.Value.Pokeballs[player.Value.pokeballToUse].Draw(player.Value.ballSize);
            }
            foreach (var pokemon in Pokemons)
            {
                pokemon.Draw();
            }
            foreach (var obstacle in Obstacles)
            {
                obstacle.Draw();
            }
            spriteBatch.Draw(powerBar, powerBarPosition, powerBarRectangle, Color.White * alpha,
                rotation, origin, windScale, spriteEffect, zDepth);

            spriteBatch.Draw(card1, card1Position, Color.White * p1Trans);
            spriteBatch.Draw(card2, card2Position, Color.White * p2Trans);


            for (int i = 0; i < Players[1].Skills.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        spriteBatch.DrawString(spriteFont,
                            "Q",
                            new Vector2(Players[1].Skills[i].position.X - 20, Players[1].Skills[i].position.Y - 90),
                            Color.White * alpha, textRotation, origin, textScale + 1, textEffects, zDepth);
                        break;
                    case 1:
                        spriteBatch.DrawString(spriteFont,
                            "W",
                            new Vector2(Players[1].Skills[i].position.X - 20, Players[1].Skills[i].position.Y - 90),
                            Color.White * alpha, textRotation, origin, textScale + 1, textEffects, zDepth);
                        break;
                    case 2:
                        spriteBatch.DrawString(spriteFont,
                            "E",
                            new Vector2(Players[1].Skills[i].position.X - 20, Players[1].Skills[i].position.Y - 90),
                            Color.White * alpha, textRotation, origin, textScale + 1, textEffects, zDepth);
                        break;
                    case 3:
                        spriteBatch.DrawString(spriteFont,
                            "R",
                            new Vector2(Players[1].Skills[i].position.X - 20, Players[1].Skills[i].position.Y - 90),
                            Color.White * alpha, textRotation, origin, textScale + 1, textEffects, zDepth);
                        break;
                }
            }

            foreach (var player in Players)
            {
                Player thisPlayer = player.Value;
                if (thisPlayer.myTurn)
                {
                    spriteBatch.DrawString(spriteFont,
                        thisPlayer.angle + "",
                        anglePosition, Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "Turn " + thisPlayer.turnRound,
                        turnPosition, Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);
                }


                for (int i = 0; i < thisPlayer.OwnPokemons.Count; i++)
                {
                    thisPlayer.maxCP = Math.Max(thisPlayer.maxCP, thisPlayer.OwnPokemons[i].cp);
                    if (thisPlayer.OwnPokemons[i].cp >= thisPlayer.maxCP)
                    {
                        thisPlayer.maxCP = thisPlayer.OwnPokemons[i].cp;
                        thisPlayer.maximumCpPokemon = thisPlayer.OwnPokemons[i];
                    }
                    if (player.Key == 1)
                    {
                        int posX = i, posY = 0;
                        if (i > 4)
                        {
                            posX = 1;
                            posY = 1;
                        }
                        Vector2 numberPokePosition = new Vector2(card1Position.X + 150 + 40 * posX,
                            card1Position.Y + 90 + 60 * posY);

                        if (choosing)
                        {
                            switch (i)
                            {
                                case 0:
                                    spriteBatch.DrawString(spriteFont,
                                        "Z",
                                        new Vector2(numberPokePosition.X + 10, numberPokePosition.Y - 20),
                                        Color.Black * alpha, textRotation, origin, textScale, textEffects, zDepth);
                                    break;
                                case 1:
                                    spriteBatch.DrawString(spriteFont,
                                        "X",
                                        new Vector2(numberPokePosition.X + 10, numberPokePosition.Y - 20),
                                        Color.Black * alpha, textRotation, origin, textScale, textEffects, zDepth);
                                    break;
                                case 2:
                                    spriteBatch.DrawString(spriteFont,
                                        "C",
                                        new Vector2(numberPokePosition.X + 10, numberPokePosition.Y - 20),
                                        Color.Black * alpha, textRotation, origin, textScale, textEffects, zDepth);
                                    break;
                                case 3:
                                    spriteBatch.DrawString(spriteFont,
                                        "V",
                                        new Vector2(numberPokePosition.X + 10, numberPokePosition.Y - 20),
                                        Color.Black * alpha, textRotation, origin, textScale, textEffects, zDepth);
                                    break;
                                case 4:
                                    spriteBatch.DrawString(spriteFont,
                                        "B",
                                        new Vector2(numberPokePosition.X + 10, numberPokePosition.Y - 20),
                                        Color.Black * alpha, textRotation, origin, textScale, textEffects, zDepth);
                                    break;
                            }
                        }

                        Pokeball pokeball = thisPlayer.Pokeballs[thisPlayer.OwnPokemons[i].caughtByBallNO];
                        spriteBatch.Draw(pokeball.texture,
                            numberPokePosition,
                            pokeball.drawRec,
                            Color.White * p1Trans, rotation, origin, 0.4f, spriteEffect,
                            zDepth);
                    }
                    else if (player.Key == 2)
                    {
                        int posX = i, posY = 0;
                        if (i > 4)
                        {
                            posX = 1;
                            posY = 1;
                        }
                        Vector2 numberPokePosition = new Vector2(card2Position.X + 150 + 40 * posX,
                            card2Position.Y + 90 + 60 * posY);

                        Pokeball pokeball = thisPlayer.Pokeballs[thisPlayer.OwnPokemons[i].caughtByBallNO];
                        spriteBatch.Draw(pokeball.texture,
                            numberPokePosition,
                            pokeball.drawRec,
                            Color.White * p2Trans, rotation, origin, 0.4f, spriteEffect,
                            zDepth);
                    }
                }
            }


            spriteBatch.Draw(wind, windPosition, windRectangle, Color.White * windAlpha,
                rotation, origin, windScale, windEffects, zDepth);


            if (windDirection == -1)
            {
                spriteBatch.DrawString(spriteFont,
                    Math.Abs(windPower).ToString(),
                    new Vector2(windPosition.X + 120, windPosition.Y + 20),
                    Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);
            }

            else
            {
                spriteBatch.DrawString(spriteFont,
                    Math.Abs(windPower).ToString(),
                    new Vector2(windPosition.X - 10, windPosition.Y + 20),
                    Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);
            }

            spriteBatch.DrawString(spriteFont,
                "" + (turnTimeLimit - sec),
                timerPosition, Color.White * alpha, textRotation, origin, timerScale, textEffects, zDepth);

            spriteBatch.DrawString(spriteFont, "SP: " + Players[1].sp,
                new Vector2(card1Position.X + 200 - 10, card1Position.Y + 200 - 10),
                Color.Black * p1Trans, textRotation, origin, textScale + 0.5f, textEffects, zDepth);

            spriteBatch.DrawString(spriteFont, "SP: " + Players[2].sp,
                new Vector2(card2Position.X + 200 - 10, card2Position.Y + 200 - 10),
                Color.Black * p2Trans, textRotation, origin, textScale + 0.5f, textEffects, zDepth);


            if (isEndGame())
            {
                spriteBatch.Draw(endGameTexture, EndGameRectangle, Color.White);
                maxPositionPokemonWin = new Vector2(250, 400);
                maxPositionPokemonLose = new Vector2(1000, 400);
                float X = maxPositionPokemonWin.X;
                float Y = maxPositionPokemonWin.Y;
                float x = maxPositionPokemonLose.X;
                float y = maxPositionPokemonLose.Y;
                Console.WriteLine(Players[1].maximumCpPokemon.name + Players[2].maximumCpPokemon.name);
                if (Players[1].maxCP > Players[2].maxCP)
                {
                    maxP2.scale = 0.5f;
                    switch (Players[1].maximumCpPokemon.name)
                    {
                        case "Bird":
                            maxP1.LoadContent(Content, "pokemon/poke1", X, Y);
                            break;
                        case "Lucia":
                            maxP1.LoadContent(Content, "pokemon/poke2", X, Y);
                            break;
                        case "Ho-oh":
                            maxP1.LoadContent(Content, "pokemon/poke3", X, Y);
                            break;
                        case "Crab":
                            maxP1.LoadContent(Content, "pokemon/poke4", X, Y);
                            break;
                        case "Monkey":
                            maxP1.LoadContent(Content, "pokemon/poke5", X, Y);
                            break;
                    }
                    switch (Players[2].maximumCpPokemon.name)
                    {
                        case "Bird":
                            maxP2.LoadContent(Content, "pokemon/poke1", x, y);
                            break;
                        case "Lucia":
                            maxP2.LoadContent(Content, "pokemon/poke2", x, y);
                            break;
                        case "Ho-oh":
                            maxP2.LoadContent(Content, "pokemon/poke3", x, y);
                            break;
                        case "Crab":
                            maxP2.LoadContent(Content, "pokemon/poke4", x, y);
                            break;
                        case "Monkey":
                            maxP2.LoadContent(Content, "pokemon/poke5", x, y);
                            break;
                    }
                    spriteBatch.DrawString(spriteFont,
                        "PLAYER 1 \n    WIN",
                        new Vector2(200, 200), Color.Blue * alpha, textRotation, origin, 3.0f, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "MAX. CP. :\n     " + Players[1].maxCP,
                        new Vector2(200, 800), Color.White * alpha, textRotation, origin, 3.0f, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "PLAYER 2 \n    LOSE",
                        new Vector2(950, 200), Color.Red * alpha, textRotation, origin, 3.0f, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "MAX. CP. :\n     " + Players[2].maxCP,
                        new Vector2(950, 800), Color.White * alpha, textRotation, origin, 3.0f, textEffects, zDepth);
                }
                else
                {
                    maxP1.scale = 0.5f;
                    switch (Players[1].maximumCpPokemon.name)
                    {
                        case "Bird":
                            maxP1.LoadContent(Content, "pokemon/poke1", x, y);
                            break;
                        case "Lucia":
                            maxP1.LoadContent(Content, "pokemon/poke2", x, y);
                            break;
                        case "Ho-oh":
                            maxP1.LoadContent(Content, "pokemon/poke3", x, y);
                            break;
                        case "Crab":
                            maxP1.LoadContent(Content, "pokemon/poke4", x, y);
                            break;
                        case "Monkey":
                            maxP1.LoadContent(Content, "pokemon/poke5", x, y);
                            break;
                    }
                    switch (Players[2].maximumCpPokemon.name)
                    {
                        case "Bird":
                            maxP2.LoadContent(Content, "pokemon/poke1", X, Y);
                            break;
                        case "Lucia":
                            maxP2.LoadContent(Content, "pokemon/poke2", X, Y);
                            break;
                        case "Ho-oh":
                            maxP2.LoadContent(Content, "pokemon/poke3", X, Y);
                            break;
                        case "Crab":
                            maxP2.LoadContent(Content, "pokemon/poke4", X, Y);
                            break;
                        case "Monkey":
                            maxP2.LoadContent(Content, "pokemon/poke5", X, Y);
                            break;
                    }
                    spriteBatch.DrawString(spriteFont,
                        "PLAYER 1 \n   LOSE",
                        new Vector2(200, 200), Color.Blue * alpha, textRotation, origin, 3.0f, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "MAX. CP. :\n     " + Players[1].maxCP,
                        new Vector2(200, 800), Color.White * alpha, textRotation, origin, 3.0f, textEffects, zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "PLAYER 2 \n    WIN",
                        new Vector2(950, 200), Color.CornflowerBlue * alpha, textRotation, origin, 3.0f, textEffects,
                        zDepth);

                    spriteBatch.DrawString(spriteFont,
                        "MAX. CP. :\n     " + Players[2].maxCP,
                        new Vector2(950, 800), Color.White * alpha, textRotation, origin, 3.0f, textEffects, zDepth);
                }

                if (maxP1 != null && maxP1.texture != null && maxP1.drawRec != null && maxP1.position != null)
                    spriteBatch.Draw(maxP1.texture, maxP1.position, maxP1.drawRec, Color.White * alpha, rotation,
                        origin, 0.8f, spriteEffect, zDepth);
                if (maxP2 != null && maxP2.texture != null && maxP2.drawRec != null && maxP2.position != null)
                    spriteBatch.Draw(maxP2.texture, maxP2.position, maxP2.drawRec, Color.White * alpha, rotation,
                        origin, 0.8f, spriteEffect, zDepth);
            }


            spriteBatch.End();

            //             spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
        }

        public void PlayerActivity()
        {
            foreach (var player in Players)
            {
                //                if (player_role == player.Key) 
                Player thisPlayer = player.Value;
                if (thisPlayer.positionY + thisPlayer.destinationRectangle.Height < ground1.position.Y)
                {
                    thisPlayer.positionY += thisPlayer.accerelate;
                    thisPlayer.accerelate += PokeBoomzGame.gravity;
                }
                else
                {
                    thisPlayer.positionY = ground1.position.Y - thisPlayer.destinationRectangle.Height;
                    thisPlayer.accerelate = 0;
                }
                if (thisPlayer.myTurn && thisPlayer.remainedPokeball == 0)
                {
                    swapPlayer();
                }
                player.Value.Update();

                foreach (var pokemon in Pokemons)
                {
                    if (thisPlayer.remainedPokeball > 0 && thisPlayer.isThrown)
                    {
                        Pokeball thisPokeball = thisPlayer.Pokeballs[thisPlayer.pokeballToUse];
                        if (thisPokeball.initialized && thisPokeball.isThrown)
                        {
                            Vector2 lPokeball = new Vector2(thisPokeball.positionCenter.X,
                                thisPokeball.positionCenter.Y);
                            if (lPokeball.X >= pokemon.left && lPokeball.X <= pokemon.right &&
                                lPokeball.Y >= pokemon.top && lPokeball.Y <= pokemon.btm)
                            {
                                thisPokeball.hitPokemon = true;
                                pokemon.isHit = true;
                                pokemon.caughtByBallNO = thisPlayer.pokeballToUse;
                                hittedPokemon = pokemon;
                                thisPlayer.OwnPokemons.Add(hittedPokemon);
                            }
                        }
                    }
                }

                Pokemon choosePokemon;

                foreach (var thisPlayerSkill in thisPlayer.Skills)
                {
                    if (thisPlayerSkill.effectON)
                    {
                        isEffectFinished = false;
                        Console.WriteLine(thisPlayerSkill.effect);
                        switch (thisPlayerSkill.effect)
                        {
                            case "remove":
                                RemovePokemonFromOpponent(player.Key);
                                choosing = true;
                                break;
                            case "increase time":
                                IncreasingTime(player.Key);
                                break;
                            case "freeze":
                                FreezingPokemon(player.Key);
                                break;
                            case "double":
                                DoubleSizePokeball(player.Key);
                                break;
                        }
                        if (isEffectFinished) thisPlayerSkill.effectON = false;
                    }
                }
            }
        }

        public void IncreasingTime(int playerNo)
        {
            sec -= 15;
            Players[playerNo].sp -= 20;
            isEffectFinished = true;
        }

        public void sortOwnPokemon(int playerNO, int pokeIndex)
        {
            for (int i = pokeIndex; i < Players[playerNO].OwnPokemons.Count; i++)
            {
                Players[playerNO].OwnPokemons.RemoveAt(i);
            }
            if (playerNO == 1)
                Players[2].sp -= 50;
            else
                Players[1].sp -= 50;
            isEffectFinished = true;
            choosing = false;
        }

        public void RemovePokemonFromOpponent(int playerNO)
        {
            Pokemon removePokemon = new Pokemon();
            if (playerNO == 1)
            {
                if (keyboardState.IsKeyDown(Keys.Z))
                    sortOwnPokemon(2, 0);

                else if (keyboardState.IsKeyDown(Keys.X))
                    sortOwnPokemon(2, 1);

                else if (keyboardState.IsKeyDown(Keys.C))
                    sortOwnPokemon(2, 2);

                else if (keyboardState.IsKeyDown(Keys.V))
                    sortOwnPokemon(2, 3);

                else if (keyboardState.IsKeyDown(Keys.B))
                    sortOwnPokemon(2, 4);
            }
            else if (playerNO == 2)
            {
                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    sortOwnPokemon(1, 0);
                }


                else if (keyboardState.IsKeyDown(Keys.X))
                    sortOwnPokemon(1, 1);

                else if (keyboardState.IsKeyDown(Keys.C))
                    sortOwnPokemon(1, 2);

                else if (keyboardState.IsKeyDown(Keys.V))
                    sortOwnPokemon(1, 3);

                else if (keyboardState.IsKeyDown(Keys.B))
                    sortOwnPokemon(1, 4);
            }
        }


        public void FreezingPokemon(int playerNO)
        {
            Players[playerNO].sp -= 20;
            moveFactor = 0.0f;
            isEffectFinished = true;
        }

        public void DoubleSizePokeball(int playerNO)
        {
            Players[playerNO].sp -= 10;
            Players[playerNO].ballSize = Players[playerNO].ballSize * 1.5f;
            isEffectFinished = true;
        }

        public void PokemonMoveByType()
        {
            foreach (var pokemon in Pokemons)
            {
                if (pokemon.type == "fly")
                {
                    if (pokemon.positionY + pokemon.destinationRectangle.Height > ground1.position.Y)
                    {
                        pokemon.positionY = ground1.position.Y - pokemon.destinationRectangle.Height;
                    }
                }
                else
                {
                    if (pokemon.positionY + pokemon.destinationRectangle.Height < ground1.position.Y)
                    {
                        pokemon.positionY += pokemon.accerelate;
                        pokemon.accerelate += PokeBoomzGame.gravity;
                    }
                    else
                    {
                        pokemon.positionY = ground1.position.Y - pokemon.destinationRectangle.Height;
                        pokemon.accerelate = 0;
                    }
                }
                pokemon.Update();
            }
        }

        public void swapPlayer()
        {
            if (!isEndGame())
            {
                if (player_role == 1 && player2.remainedPokeball > 0)
                {
                    player1.myTurn = false;
                    player2.myTurn = true;
                    player_role = 2;
                    p1Trans = 0.4f;
                    p2Trans = 0.8f;
                    player2.turnRound++;
                    foreach (var skill in Players[1].Skills)
                    {
                        skill.effectON = false;
                    }
                }
                else if (player_role == 1 && player2.remainedPokeball == 0)
                {
                    player1.turnRound++;
                    foreach (var skill in Players[1].Skills)
                    {
                        skill.effectON = false;
                    }
                }
                else if (player_role == 2 && player1.remainedPokeball > 0)
                {
                    player1.myTurn = true;
                    player2.myTurn = false;
                    player_role = 1;
                    p2Trans = 0.4f;
                    p1Trans = 0.8f;
                    player1.turnRound++;
                    foreach (var skill in Players[2].Skills)
                    {
                        skill.effectON = false;
                    }
                }
                else if (player_role == 2 && player1.remainedPokeball == 0)
                {
                    player2.turnRound++;
                    foreach (var skill in Players[2].Skills)
                    {
                        skill.effectON = false;
                    }
                }
                sec = 0;
                if (Math.Floor(rand.NextDouble() * 2) == 0)
                {
                    windDirection = -1;
                    windEffects = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    windDirection = 1;
                    windEffects = SpriteEffects.None;
                }
                windPower = windDirection * (float) Math.Floor(rand.NextDouble() * 5.0f);
                timesUp = false;
            }
        }
    }
}