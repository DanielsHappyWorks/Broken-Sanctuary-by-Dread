/*
Function: 		Stores common hard-coded variable values used within the game e.g. key mappings, mouse sensitivity
Author: 		NMCG
Version:		1.0
Date Updated:	5/10/16
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace GDLibrary
{
    public class AppData
    {
        #region Mouse
        //defines how much the mouse has to move in pixels before a movement is registered - see MouseManager::HasMoved()
        public static float MouseSensitivity = 1;
        #endregion

        #region Common
        public static int IndexMoveForward = 0;
        public static int IndexMoveBackward = 1;
        public static int IndexStrafeLeft = 2;
        public static int IndexStrafeRight = 3;
        public static int IndexRotateLeft = 2;
        public static int IndexRotateRight = 3;
        #endregion
        
        #region Camera
        public static float CameraRotationSpeed = 0.01f;
        public static float CameraMoveSpeed = 0.01f;
        public static float CameraStrafeSpeed = 0.01f;

        public static Keys[] CameraMoveKeys = { Keys.W, Keys.S, Keys.A, Keys.D };
        public static Keys[] CameraMoveKeys_Alt1 = { Keys.T, Keys.G, Keys.F, Keys.H };

        public static float CameraThirdPersonLerpSpeedSlow = 0.05f;
        public static float CameraThirdPersonLerpSpeedMedium = 0.1f;
        public static float CameraThirdPersonLerpSpeedFast = 0.2f;
        public static float CameraThirdPersonScrollSpeedDistanceMultiplier = 0.01f;
        public static float CameraThirdPersonScrollSpeedElevatationMultiplier = 0.1f;
        #endregion

        #region Player
        public static Keys[] PlayerMoveKeys = { Keys.U, Keys.J, Keys.H, Keys.K };
        public static float PlayerMoveSpeed = 0.1f;
        public static float PlayerStrafeSpeed = 0.07f;
        public static float PlayerRotationSpeed = 0.04f;
        public static int IndexJump;
        public static int IndexJumpCrouch;


        #endregion


    }
}
