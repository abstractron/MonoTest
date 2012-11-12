using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// Manual using for SoundEffect class
using Microsoft.Xna.Framework.Audio;

namespace MonoTest
{
    /// <summary>
    /// MonoTestGame is a class that represents a test of the MonoGame framework.
    /// This project exists as a core template for games using the framework.
    /// 
    /// MONOGAME NOTES
    /// --------------
    /// 
    /// Game flow
    /// ---------
    /// MonoTestGame class is instantiated, firing constructor (creates graphics context, points at content)
    /// Initialize():
    /// 
    /// LoadContent():
    /// 
    /// Update(): Per-frame call to update game state
    /// Draw(): Per-frame call to draw updated objects to display
    /// 
    /// UnloadContent():
    /// 
    /// ----------------
    /// Content Pipeline
    /// ----------------
    /// As part of the content pipeline of MonoGame which is slightly different from
    /// stock XNA, it's required to create a separate VS2010 project to use for generating
    /// the content xnb files. Please see 'GameThumbnail' image and 'lazer1' sound in this game project
    /// as an example (Generated in MonoTestContent VS2010 project).
    /// </summary>
    public class MonoTestGame : Game
    {
        // XNA graphics resources
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Sprite tex/pos/size/speed
        Texture2D bgTexture;    // Static 'ABSTRACTRON' bg text graphic
        Vector2 bgPosition;

        Texture2D texture1;
        Texture2D texture2;
        Vector2 spritePosition1;
        Vector2 spritePosition2;
        Vector2 spriteSpeed1 = new Vector2(50.0f, 50.0f);
        Vector2 spriteSpeed2 = new Vector2(100.0f, 100.0f);

        int sprite1Height;
        int sprite1Width;
        int sprite2Height;
        int sprite2Width;

        // SoundEffect not part of core 
        SoundEffect soundEffect;



        public MonoTestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // SpriteBatch is used to render textures
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            bgTexture = Content.Load<Texture2D>("defaultbg");
            bgPosition.X = _graphics.GraphicsDevice.Viewport.Width / 2 - (bgTexture.Width / 2);
            bgPosition.Y = _graphics.GraphicsDevice.Viewport.Height / 2 - (bgTexture.Height / 2);


            texture1 = Content.Load<Texture2D>("GameThumbnail");
            texture2 = Content.Load<Texture2D>("GameThumbnail");

            soundEffect = Content.Load<SoundEffect>("Laser");

            spritePosition1.X = 20;
            spritePosition1.Y = 20;

            spritePosition2.X = _graphics.GraphicsDevice.Viewport.Width - texture1.Width - 20;
            spritePosition2.Y = _graphics.GraphicsDevice.Viewport.Height - texture1.Height - 20;

            sprite1Height = texture1.Bounds.Height;
            sprite1Width = texture1.Bounds.Width;

            sprite2Height = texture2.Bounds.Height;
            sprite2Width = texture2.Bounds.Width;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            texture1.Dispose();
            texture2.Dispose();
            bgTexture.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdateSprite(gameTime, ref spritePosition1, ref spriteSpeed1);
            UpdateSprite(gameTime, ref spritePosition2, ref spriteSpeed2);

            CheckForCollision();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear display to magenta
            _graphics.GraphicsDevice.Clear(Color.RoyalBlue);

            // Draw sprites
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.Draw(bgTexture, bgPosition, Color.White);
            _spriteBatch.Draw(texture1, spritePosition1, Color.White);
            _spriteBatch.Draw(texture2, spritePosition2, Color.White);
            _spriteBatch.End();
        }

        void UpdateSprite(GameTime gameTime, ref Vector2 spritePosition, ref Vector2 spriteSpeed)
        {
            // Move sprite by speed, scaled by time elapsed
            spritePosition +=
                spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            int MaxX = _graphics.GraphicsDevice.Viewport.Width - texture1.Width;
            int MinX = 0;

            int MaxY = _graphics.GraphicsDevice.Viewport.Height - texture2.Height;
            int MinY = 0;

            // Sprite 1 wrap at screen edges
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }
            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }
            if (spritePosition.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }
            else if (spritePosition.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }

            base.Update(gameTime);
        }

        void CheckForCollision()
        {
            // Bounding boxes for sprite current bounds
            BoundingBox bb1 = new BoundingBox(new Vector3(spritePosition1.X - (sprite1Width / 2), spritePosition1.Y - (sprite1Height / 2), 0), 
                                            new Vector3(spritePosition1.X + (sprite1Width / 2), spritePosition1.Y + (sprite1Height / 2), 0));
            BoundingBox bb2 = new BoundingBox(new Vector3(spritePosition2.X - (sprite2Width / 2), spritePosition2.Y - (sprite2Height / 2), 0),
                                               new Vector3(spritePosition2.X + (sprite2Width / 2), spritePosition2.Y + (sprite2Height / 2), 0));

            // Intersecting, play sound
            if (bb1.Intersects(bb2))
            {
                soundEffect.Play(0.3f, 1.0f, 0.5f);
            }
        }
    }
}
