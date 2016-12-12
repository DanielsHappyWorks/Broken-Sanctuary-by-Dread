using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UITextObject : DrawnActor2D
    {
        #region Fields
        private string text;
        private SpriteFont spriteFont;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = (value.Length >= 0) ? value : "Default";
            }
        }
        public SpriteFont SpriteFont
        {
            get
            {
                return this.spriteFont;
            }
            set
            {
                this.spriteFont = value;
            }
        }
        #endregion

        public UITextObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, string text, SpriteFont spriteFont)
            : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth)
        {
            this.spriteFont = spriteFont;
            this.text = text;
        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.DrawString(this.spriteFont, this.text, this.Transform2D.Translation, this.Color, this.Transform2D.Rotation,
                this.Transform2D.Origin, this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);
        }
    }
}
