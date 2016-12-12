using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    //used to send data to objects that render changeable text i.e. TextRendererController
    public class TextEventData : EventData
    {
        #region Variables
        private string text;
        private SpriteFont font;
        private Color textColor, backgroundColor;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return text;
            }
        }
        public SpriteFont Font
        {
            get
            {
                return font;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
        }
        public Color TextColor
        {
            get
            {
                return textColor;
            }
        }
        #endregion

        public TextEventData(string id, object sender, EventActionType eventActionType, EventCategoryType eventCategoryType, SpriteFont font, string text, Color textColor, Color backgroundColor)
            : base(id, sender, eventActionType, eventCategoryType)
        {
            this.text = text;           //e.g. alarm in sector 2
            this.font = font;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
        }

        public override string ToString()
        {
            return base.ToString() + "TextRendererEventData - Text: " + this.text;
        }

        //add GetHashCode and Equals
        public override bool Equals(object obj)
        {
            TextEventData other = obj as TextEventData;
            return base.Equals(obj) && this.text.Equals(other.Text) && this.font.Equals(other.Font) && this.textColor == other.TextColor;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 11 + this.text.GetHashCode();
            hash = hash * 31 + this.textColor.GetHashCode();
            hash = hash * 47 + base.GetHashCode();
            return hash;
        }
    }
}
