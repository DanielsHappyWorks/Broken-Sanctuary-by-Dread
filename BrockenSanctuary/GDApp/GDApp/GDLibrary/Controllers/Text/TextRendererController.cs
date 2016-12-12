using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class TextRendererController : Controller
    {
        #region Fields
        private string text;
        private SpriteFont font;
        private RenderTarget2D textRenderTarget;
        private Color textColor, backgroundColor;
        private Vector2 dimensions;
        private bool bUpdated;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
            }
        }
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }
        #endregion

        #region Properties
        #endregion

        public TextRendererController(string id, ControllerType controllerType, SpriteFont font, string text, Color textColor, Color backgroundColor)
            : base(id, controllerType)
        {
            //register for events
            game.EventDispatcher.TextRenderChanged += EventDispatcher_TextRender;

            //set initial conditions
            Set(text, font, textColor, backgroundColor);

        }

        public virtual void EventDispatcher_TextRender(EventData eventData)
        {
            TextEventData e = eventData as TextEventData;
            if (e.ID.Equals(this.ID)) //use the ID to send event to a particular controller based on controller ID
            Set(e.Text, e.Font, e.TextColor, e.BackgroundColor);
        }

        public void Set(string textToRender, SpriteFont font, Color textColor, Color backgroundColor)
        {
            this.text = textToRender;
            this.font = font;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;

            this.dimensions = font.MeasureString(this.text);
            this.textRenderTarget = new RenderTarget2D(game.GraphicsDevice, 
                (int)dimensions.X, (int)dimensions.Y, true, SurfaceFormat.Rgba64, 
                DepthFormat.None);

            this.bUpdated = true;
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            DrawnActor3D parentActor = actor as DrawnActor3D;

            if(bUpdated)
            {
                game.GraphicsDevice.SetRenderTarget(this.textRenderTarget);
                game.GraphicsDevice.Clear(this.backgroundColor);
                game.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, DepthStencilState.Default, null);
                game.SpriteBatch.DrawString(font, this.text, Vector2.Zero, textColor);
                game.SpriteBatch.End();
                game.GraphicsDevice.SetRenderTarget(null);

                //set texture in drawn object
                if (parentActor is TexturedPrimitiveObject) //its either a billboard, a simple textured primitive, or a model
                    (parentActor as TexturedPrimitiveObject).Texture = this.textRenderTarget;
                else
                    (parentActor as ModelObject).Texture = this.textRenderTarget;

                //set to false to prevent repeat each update
                bUpdated = false;
            }
        }


        public void Dispose()
        {
            this.textRenderTarget.Dispose();
        }
    }
}
