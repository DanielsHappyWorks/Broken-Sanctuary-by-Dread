using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class UIMouseObject : UITextureObject
    {

        #region Variables
        private string text;
        private SpriteFont spriteFont;
        private Vector2 textOffsetPosition;
        private Color textColor;
        private Vector2 textDimensions;
        private Vector2 textOrigin;
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
                this.text = value;
                this.textDimensions = this.spriteFont.MeasureString(this.text);
                this.textOrigin = new Vector2(this.textDimensions.X / 2, this.textDimensions.Y / 2);
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

        public UIMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
        Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, bool isVisible)
            : this(id, actorType, statusType, transform, color, spriteEffects,
                  null, null, Vector2.Zero, Color.White,
                  layerDepth, texture,
                new Rectangle(0, 0, texture.Width, texture.Height),
                    new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), isVisible)
        {

        }

        //backward compatability for mouse objects with no text
        public UIMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, Rectangle sourceRectangle, Vector2 origin, bool isVisible)
            : this(id, actorType, statusType, transform, color, spriteEffects,
                  null, null, Vector2.Zero, Color.White,
                  layerDepth, texture, sourceRectangle, origin, isVisible)
        {

        }

        public UIMouseObject(string id, ActorType actorType, StatusType statusType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, SpriteFont spriteFont,
            string text, Vector2 textOffsetPosition, Color textColor,
            float layerDepth, Texture2D texture, Rectangle sourceRectangle, Vector2 origin, bool isVisible)
            : base(id, actorType, statusType, transform, color, spriteEffects, layerDepth, texture, sourceRectangle, origin)
        {
            this.spriteFont = spriteFont;
            this.Text = text;
            this.textOffsetPosition = textOffsetPosition;
            this.textColor = textColor;

            this.Transform2D.Translation = game.ScreenCentre;
        }

        public override void Draw(GameTime gameTime)
        {
            //draw icon
            game.SpriteBatch.Draw(this.Texture, this.Transform2D.Translation, //bug - 22/4/16
                this.SourceRectangle, this.Color, this.Transform2D.Rotation, this.Origin,
                    this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);

            //draw any additional text
            if (this.text != null)
                game.SpriteBatch.DrawString(this.spriteFont, this.text,
                    ((this.Transform2D.Translation - this.textOrigin) - this.textOffsetPosition), this.textColor);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMouseObject(gameTime);
            DoMousePick(gameTime);
            base.Update(gameTime);
        }

        private void UpdateMouseObject(GameTime gameTime)
        {
            //move the texture for the mouse object to be where the mouse pointer is
            //this.Transform2D.Translation = game.MouseManager.Position;
        }


        public virtual void DoMousePick(GameTime gameTime)
        {
            if (game.CameraManager.ActiveCamera != null)
            {
                float distance = 7;
                float startDistance = 1; //if 1st person collidable then start picking outside CD/CR surface
                Vector3 pos, normal;

                //add code for mouse picking from the previous week....
                CollidableObject collidableObject = game.MouseManager.GetPickedObjectFromCenter(
                    game.CameraManager.ActiveCamera,
                    startDistance, distance, out pos, out normal) as CollidableObject;

                if (collidableObject != null) //&& (collidableObject.ActorType == ActorType.Pickup))
                {
                    this.Text = collidableObject.ID;

                    if (collidableObject.ActorType == ActorType.CollidableInteractableProp)
                    {
                        text += " : Interactable Object";
                        this.textDimensions = this.spriteFont.MeasureString(this.text);
                        this.textOrigin = new Vector2(this.textDimensions.X / 2, this.textDimensions.Y / 2);
                        textColor = Color.DeepPink;
                        this.Color = Color.DeepPink;
                        this.Transform2D.Rotation += 0.05f;

                        if (game.MouseManager.IsLeftButtonClickedOnce() || game.KeyboardManager.IsFirstKeyPress(Microsoft.Xna.Framework.Input.Keys.E))
                        {
                            //call event on interaction
                            // PickupCollidableObject pickupObject = collidableObject as PickupCollidableObject;
                            // EventDispatcher.Publish(pickupObject.EventParameters.EventData);
                            if (collidableObject.ID == "door" && game.playerHasKey)
                            {
                                EventDispatcher.Publish(new EventData("Locks Door", this, EventActionType.WinGame, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "phone")
                            {
                                EventDispatcher.Publish(new EventData("Phone", this, EventActionType.PhoneInteraction, EventCategoryType.Interaction));
                            }
                            if (collidableObject.ID == "clock")
                            {
                                EventDispatcher.Publish(new EventData("Clock", this, EventActionType.ClockInteraction, EventCategoryType.Interaction));
                            }
                            if (collidableObject.ID == "painting1")
                            {
                                EventDispatcher.Publish(new EventData("painting1", this, EventActionType.PaintingGoodInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "painting2")
                            {
                                EventDispatcher.Publish(new EventData("painting2", this, EventActionType.PaintingOtherInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "painting3")
                            {
                                EventDispatcher.Publish(new EventData("painting3", this, EventActionType.PaintingBadInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "kettle")
                            {
                                EventDispatcher.Publish(new EventData("Kettle", this, EventActionType.KettleInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "microwave")
                            {
                                EventDispatcher.Publish(new EventData("Microwave", this, EventActionType.MicrowaveInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "chair")
                            {
                                EventDispatcher.Publish(new EventData("Chair", this, EventActionType.ChairInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "toaster")
                            {
                                EventDispatcher.Publish(new EventData("Toaster", this, EventActionType.ToasterInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "fridge")
                            {
                                EventDispatcher.Publish(new EventData("Fridge", this, EventActionType.FridgeInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "oven")
                            {
                                EventDispatcher.Publish(new EventData("Oven", this, EventActionType.OvenInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "sink")
                            {
                                EventDispatcher.Publish(new EventData("Sink", this, EventActionType.SinkInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "mirror")
                            {
                                EventDispatcher.Publish(new EventData("Mirror", this, EventActionType.MirrorInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "toilet")
                            {
                                EventDispatcher.Publish(new EventData("Toilet", this, EventActionType.ToiletInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "radio")
                            {
                                EventDispatcher.Publish(new EventData("Radio", this, EventActionType.RadioInteraction, EventCategoryType.Interaction));
                            }
                            if (collidableObject.ID == "tv")
                            {
                                EventDispatcher.Publish(new EventData("Television", this, EventActionType.TVInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "remote")
                            {
                                EventDispatcher.Publish(new EventData("Remote", this, EventActionType.RemoteInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                            if (collidableObject.ID == "winebottle")
                            {
                                EventDispatcher.Publish(new EventData("Vino", this, EventActionType.VinoInteraction, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }
                        }
                    }
                    else if (collidableObject.ActorType == ActorType.Pickup)
                    {
                        text += " : Pickup";
                        this.textDimensions = this.spriteFont.MeasureString(this.text);
                        this.textOrigin = new Vector2(this.textDimensions.X / 2, this.textDimensions.Y / 2);
                        textColor = Color.Yellow;
                        this.Transform2D.Rotation -= 0.05f;
                        this.Color = Color.Yellow;
                        if (game.MouseManager.IsLeftButtonClickedOnce() || game.KeyboardManager.IsFirstKeyPress(Microsoft.Xna.Framework.Input.Keys.E))
                        {
                            if (collidableObject.ID == "key")
                            {
                               /* bool aniRun = true;
                                collidableObject.AttachController(new KeyPickupController("keyPick", 
                                    ControllerType.KeyPickupController, new Vector3(0, .01f, 0), 
                                    new Vector3(.01f, 0, .01f)));
                                long startTime = gameTime.ElapsedGameTime.Milliseconds;
                                long currentTime;
                                long endTime = startTime + 3500;
                                while (aniRun)
                                {
                                    currentTime = gameTime.ElapsedGameTime.Milliseconds;
                                    if (currentTime >= endTime)
                                    {
                                        aniRun = false;
                                        //collidableObject.DetachController("keyPick");
                                        game.ObjectManager.Remove(collidableObject);
                                        
                                    }
                                }*/
                                game.ObjectManager.Remove(collidableObject);
                                EventDispatcher.Publish(new EventData("Has Key", this, EventActionType.PickUpKey, EventCategoryType.Interaction));
                                collidableObject.ActorType = ActorType.Decorator;
                            }

                            //call event on pickup
                            // PickupCollidableObject pickupObject = collidableObject as PickupCollidableObject;
                            // EventDispatcher.Publish(pickupObject.EventParameters.EventData);
                        }
                    }
                    else
                    {
                        text = "";
                        textColor = Color.White;
                        this.Color = Color.White;
                        this.Transform2D.Rotation = 0;
                        //do something when not colliding...
                    }
                }
                else
                {
                    text = "";
                    textColor = Color.White;
                    this.Color = Color.White;
                    this.Transform2D.Rotation = 0;
                    //do something when not colliding...
                }
            }
        }
    }
}
