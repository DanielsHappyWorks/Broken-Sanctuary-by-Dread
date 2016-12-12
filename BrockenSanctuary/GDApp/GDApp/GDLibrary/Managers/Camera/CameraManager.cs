/*
Function: 		Provide mouse input functions
Author: 		
Version:		1.0
Date Updated:	
Bugs:			None
Fixes:			None
*/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;
using System.Collections;
using System;

namespace GDLibrary
{
    public class CameraManager : GameComponent, IEnumerable<Camera3D>
    {
        #region Variables
        private Dictionary<string, List<Camera3D>> dictionary;
        private List<Camera3D> activeCameraList;
        private string activeCameraLayout;
        private Camera3D activeCamera;
        private int activeCameraIndex;
        private bool bPaused = false;
        #endregion

        #region Properties
        public Camera3D ActiveCamera
        {
            get
            {
                return this.activeCameraList[this.activeCameraIndex];
            }
        }
        public int ActiveCameraIndex
        {
            set
            {
                this.activeCameraIndex = ((value >= 0)
                    && (value < this.activeCameraList.Count)) ? value : 0;
            }
            get
            {
                return this.activeCameraIndex;
            }
        }
        public string ActiveCameraLayout
        {
            get
            {
                return this.activeCameraLayout;
            }
        }
        public List<Camera3D> this[string cameraLayout]
        {
            get
            {
                if(this.dictionary.ContainsKey(cameraLayout))
                    return this.dictionary[cameraLayout];
                else
                    return null;
            }
        }
        public int Count
        {
            get
            {
                return this.activeCameraList.Count;
            }
        }
        public bool Paused //to do...
        {
            get
            {
                return this.bPaused;
            }
            set
            {
                this.bPaused = value;
            }
        }
        #endregion

        public CameraManager(Main game) : base(game)
        {
            this.dictionary = new Dictionary<string, List<Camera3D>>();

            #region Event Handling
            //pause/unpause events
            game.EventDispatcher.MenuChanged += EventDispatcher_MenuChanged;
            #endregion
        }

        private void EventDispatcher_MenuChanged(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnPlay)
            {
                this.bPaused = false;
            }
            else if (eventData.EventType == EventActionType.OnPause)
            {
                this.bPaused = true;
            }
        }

        public bool SetCamera(string cameraLayout, string cameraID)
        {
            cameraLayout = cameraLayout.ToLower().Trim();
            cameraID = cameraID.ToLower().Trim();

            SetActiveCameraLayout(cameraLayout); //set to the appropriate layout
            int index = 0;
            Camera3D camera = null;

            Find(cameraLayout, cameraID, out camera, out index); //find the camera
            ActiveCameraIndex = index; //set to be active

            return (camera != null); //true if we found the camera
        }


        public bool SetActiveCameraLayout(string cameraLayout)
        {
            cameraLayout = cameraLayout.ToLower().Trim();

            //if first time and NULL or not the same as current
            if((this.activeCameraList == null) || (!this.activeCameraLayout.Equals(cameraLayout)))
            {
                //if layout exists in the dictionary
                if(this.dictionary.ContainsKey(cameraLayout))
                {
                    this.activeCameraList = this.dictionary[cameraLayout];

                    //sort camera by drawDepth (ascending) in case we have picture-in-picture
                    this.activeCameraList.Sort((x, y) => x.DrawDepth.CompareTo(y.DrawDepth));

                    this.ActiveCameraIndex = 0;
                    this.activeCameraLayout = cameraLayout;
                    return true;
                }
            }
            return false;
        }

        public void CycleCamera()
        {
            this.ActiveCameraIndex += 1;
        }

        public void Add(string cameraLayout, Camera3D camera)
        {
            cameraLayout = cameraLayout.ToLower().Trim();

            if (this.dictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.dictionary[cameraLayout];
                list.Add(camera);
            }
            else
            {
                List<Camera3D> list = new List<Camera3D>();
                list.Add(camera);
                this.dictionary.Add(cameraLayout, list);
            }
        }
        public bool Remove(string cameraLayout, IFilter<Actor> filter)
        {
            cameraLayout = cameraLayout.ToLower().Trim();

            Camera3D camera = Find(cameraLayout, filter);
            if (camera != null)
            {
                this.activeCameraList.Remove(camera);
                return true;
            }

            return false;
        }

        public Camera3D Find(string cameraLayout, IFilter<Actor> filter)
        {
            if (this.dictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.dictionary[cameraLayout];
                for (int i = 0; i < list.Count; i++)
                {
                    if (filter.Matches(list[i]))
                    {
                        return list[i];
                    }
                } //for
            } //if
            return null;
        }

        //another form of Find method that uses a Predicate (this is functional programming)
        public Camera3D Find(string cameraLayout, Predicate<Camera3D> predicate)
        {
            if (this.dictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.dictionary[cameraLayout];
                return list.Find(predicate);
            } //if
            return null;
        }


        public void Find(string cameraLayout, string cameraID, out Camera3D camera, out int index)
        {
            camera = null;
            index = -1;

            List<Camera3D> list = this.dictionary[cameraLayout];

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ID.Equals(cameraID))
                    {
                        camera = list[i];
                        index = i;
                        break;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.bPaused)
            {
                foreach (Camera3D camera in this.activeCameraList)
                {
                    this.activeCamera = camera; //do this do anything?

                    //was the updated enum value set?
                    if ((camera.StatusType & StatusType.Updated) == StatusType.Updated)
                        camera.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        public IEnumerator<Camera3D> GetEnumerator()
        {
            return this.activeCameraList.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
