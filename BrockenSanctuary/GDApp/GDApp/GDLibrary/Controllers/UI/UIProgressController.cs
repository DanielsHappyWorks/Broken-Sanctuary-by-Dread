using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class UIProgressController : Controller
    {
        #region Fields
        private int maxValue, startValue, currentValue;
        private UITextureObject parentUITextureActor;
        #endregion

        #region Properties
        public int CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                this.currentValue = ((value >= 0) && (value <= maxValue)) ? value : 0;
            }
        }
        public int MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = (value >= 0) ? value : 0;
            }
        }
        public int StartValue
        {
            get
            {
                return this.startValue;
            }
            set
            {
                this.startValue = (value >= 0) ? value : 0;
            }
        }
        #endregion

        public UIProgressController(string id, ControllerType controllerType,
            int startValue, int maxValue)
            : base(id, controllerType)
        {
            this.StartValue = startValue;
            this.MaxValue = maxValue;
            this.CurrentValue = startValue;

            //add event handling...
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            this.parentUITextureActor = actor as UITextureObject;
            //set the source rectangle according to whatever start value the user supplies
            UpdateSourceRectangle();
            base.Update(gameTime, actor);
        }
        private void UpdateSourceRectangle()
        {
            //how much of a percentage of the width of the image does the current value represent?
            float widthMultiplier = (float)this.currentValue / this.maxValue;

            //now set the amount of visible rectangle using the current value
            this.parentUITextureActor.SourceRectangleWidth
                = (int)(widthMultiplier * this.parentUITextureActor.OriginalSourceRectangle.Width);
        } 
    }
}
