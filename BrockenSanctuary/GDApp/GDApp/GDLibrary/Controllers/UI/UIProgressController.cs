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
        private float maxValue, startValue, currentValue;
        private UITextureObject parentUITextureActor;
        private bool paused;
        private float incrementSpeed;
        #endregion

        #region Properties
        public float CurrentValue
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
        public float MaxValue
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
        public float StartValue
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
            this.paused = false;
            this.incrementSpeed = 0.01f;

            //add event handling...
        }

        public override void Update(GameTime gameTime, IActor actor)
        {
            if (paused == false)
            {
                this.parentUITextureActor = actor as UITextureObject;
                //set the source rectangle according to whatever start value the user supplies
                if (game.CameraManager.ActiveCamera.StatusType != 0)
                {
                    this.currentValue += incrementSpeed;

                    //if more then 100 Loose

                    if (this.currentValue >= 93)
                    {
                        EventDispatcher.Publish(new EventData("Lose", this, EventActionType.LoseGame, EventCategoryType.Interaction));
                        this.currentValue = 0;
                    }

                    if (this.currentValue < 0)
                    {
                        this.currentValue = 0;
                    }
                    
                    if(incrementSpeed < 0.005f)
                    {
                        incrementSpeed = 0.005f;
                    }


                    UpdateSourceRectangle();
                }
            }
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

        public void Reset()
        {
            this.currentValue = 0;
        }

        public void incrementProgressBar(float incrementBy)
        {
            this.currentValue += incrementBy;
        }

        public void pauseProgressBar()
        {
            this.paused = true;
        }

        public void playProgressBar()
        {
            this.paused = false;
        }

        public void incrementProgressBarFillSpeed(float incrementBy)
        {
            this.incrementSpeed += incrementBy;
        }
    }
}
