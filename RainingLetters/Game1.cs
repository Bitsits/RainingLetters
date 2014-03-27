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

namespace RainingLetters
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int score = 0;
        int lost = 0;
        SpriteFont font;
        GameObject[] blocks;
        int maxLetter = 8;
        Random randomX;
        Random randomAlpha;
        int positionX;
        int alpha;
        char a;
        List<int> position;
        List<char> alphabet;
        Rectangle viewportRect;
        float maxLetterVelocity = 0.8f;
        float minLetterVelocity = 0.3f;
        Random random = new Random();
        KeyboardState kb;
        KeyboardState pkb = Keyboard.GetState();
        int inc =1;
        Texture2D background0,background1;
        String rank;
        String tip;

        SoundEffect[] hitSounds = new SoundEffect[3];
     
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            IsMouseVisible = true;            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            blocks = new GameObject[maxLetter];
            randomX = new Random();
            randomAlpha = new Random();
            position = new List<int>();
            alphabet = new List<char>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
            background0 = Content.Load<Texture2D>("background");
            background1 = Content.Load<Texture2D>("background1");
            for (int i = 0; i < maxLetter; i++)
            {
                blocks[i] = new GameObject();

            }
            viewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            MediaPlayer.Play(Content.Load<Song>("Audio/Back to old school"));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1.0f;

            hitSounds[0] = Content.Load<SoundEffect>("Audio/hit0");
            hitSounds[1] = Content.Load<SoundEffect>("Audio/hit1");
            hitSounds[2] = Content.Load<SoundEffect>("Audio/hit2");

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
            if (!this.IsActive) return;

            pkb = kb;
            kb = Keyboard.GetState();
            if (gameTime.TotalRealTime.TotalSeconds > 5 && lost<10)
            {
                if (gameTime.TotalRealTime.TotalSeconds>40*inc)
                {
                    maxLetterVelocity += 0.2f;
                    minLetterVelocity += 0.2f;
                    maxLetter += 1;
                    if (inc<=16)
                    {
                        inc += 1;
                    }
                    
                }
               
               
                UpdateLetters();

            }

            base.Update(gameTime);
        }

        public void UpdateLetters()
        {
            foreach (GameObject block in blocks)
            {

                if (block.alive)
                {
                    block.position -= block.velocity;
                    Keys k = (Keys)(Keys.A + block.alphabet - 'A');
                    if (!viewportRect.Contains(new Point((int)block.position.X, (int)block.position.Y))
                        || (kb.IsKeyDown(k) && pkb.IsKeyUp(k)))
                    {
                        block.alive = false;
                        position.Remove((int)block.position.X);
                        alphabet.Remove(block.alphabet);
                        if ((kb.IsKeyDown(k) && pkb.IsKeyUp(k)))
                        {
                            hitSounds[random.Next(3)].Play();
                            score += 1;
                        }

                        if (!viewportRect.Contains(new Point((int)block.position.X, (int)block.position.Y)))
                        {
                            lost += 1;
                        }
                    }
                }
                else
                {
                    block.alive = true;
                    GeneratePosition();
                    block.position = new Vector2(positionX, 60);

                    GenerateAlphabets();
                    block.alphabet = a;
                    block.velocity = new Vector2(0,
                        MathHelper.Lerp(
                        -minLetterVelocity,
                        -maxLetterVelocity,
                        (float)random.NextDouble()));
                }
            }
        }

        void GeneratePosition()
        {
            while (true)
            {
                positionX = randomX.Next() % (800 - 20 - 80) + 80;
                positionX = (positionX / 20) * 20;
                
                if (!position.Contains(positionX))
                {
                    position.Add(positionX);
                    return;
                }
            }
        }

        void GenerateAlphabets()
        {
            while (true)
            {
                alpha = randomAlpha.Next(26);
                a = (char)((int)'A' + alpha);
                if (!alphabet.Contains(a))
                {
                    alphabet.Add(a);
                    return;
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(background0, new Rectangle(0, 0, background0.Width, background0.Height), Color.White);

            if (gameTime.TotalRealTime.TotalSeconds <=5)
            {
                spriteBatch.Draw(background1, new Rectangle(0, 0, background1.Width, background1.Height), Color.White);
               
            }
            if ((gameTime.TotalRealTime.TotalSeconds > 5) &&(lost < 10))
            {
                foreach (GameObject letter in blocks)
                {
                   // color = new Color(0, 0, 0, random.Next(200));
                    if (letter.alive)
                    {
                        
                        spriteBatch.DrawString(font, letter.alphabet.ToString(), letter.position, Color.DarkBlue);
                        spriteBatch.DrawString(font, "Score = " + score.ToString(), new Vector2(90f, 10f), Color.DarkCyan);
                        spriteBatch.DrawString(font, "Letters Lost = " + lost.ToString(), new Vector2(480, 10), Color.Violet);
                    }
                }
                                
            }

            if (lost>=10)
            {
                if (score<=100)
                {
                    rank = "Novice";
                    tip = "Need 1 hr. practice everyday";
                }
                if (score>=100 && score<=250)
                {
                    rank = "Intermediate";
                    tip = "Need half hr. practice everyday";
                }
                if (score>250)
                {
                    rank = "Advanced";
                    tip = "U are awesome";
                }
                spriteBatch.DrawString(font, "Game Over ", new Vector2(90,100), Color.HotPink);
                spriteBatch.DrawString(font, "Typing speed = " + score.ToString() + " letters ", new Vector2(90, 200), Color.Green);
                spriteBatch.DrawString(font, "Rank = " + rank, new Vector2(90, 300), Color.BlueViolet);
                spriteBatch.DrawString(font, "Tips = " + tip, new Vector2(90, 400), Color.CornflowerBlue);


            }
           
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
