using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;

/* Version:     3.1
 Description:   Minor change to Camera3D::Clone(). 
                Add some "To Do" requirements. 
                Added folders under Objects to organise 3D and 2D game objects.
 Date:          7/11/16
 Author:        NMCG
 Bugs:          4/11/16 - Bug on scale of Box collision primitive.
 To Do:         Override clone method for each controller, controllers for rail, and flight camera types.
                Call Dispose() on GenericDictionaries used to store assets in Main::UnloadContent().
                Add ObjectManager::Remove().
                Add EventDispatcher and EventData.
                Add SoundManager for 2D and 3D sounds.
                Add HUDManager to add UI text (e.g. debug information, FPS).
                Add hierarchy of objects (i.e. Actor2D, DrawnActor2D, ButtonActor2D, TextActor2D) to support 2D UI elements (e.g. button, text, progress control).
 */
/* Version:     3.0
 Description:   Added JigLibX collision and physics engine.
 Date:          4/11/16
 Author:        NMCG
 Bugs:          4/11/16 - Bug on scale of Box collision primitive.
 To Do:         Override clone method for each controller, controllers for rail, and flight camera types.
 */
/* Version:     2.9
 Description:   Added ThirdPersonController, Camera3D::viewport, and split screen demo
                Added TargetController as a base for any controller (e.g. 3rd Person or Rail) that bases its movement on a target object.
                Made CameraManager implement IEnumberable to support foreach() loop - see Main::Draw()
 Date:          3/11/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add JigLibX, override clone method for each controller, controllers for rail, and flight camera types.
 */
/* Version:     2.8
 Description:   Added DriveController as first step towards ThirdPersonController.
                Added FlightController to allow unconstrained movement.
                Added PlayStateType to capture play states (Play, Pause, Stop, Reset) for TrackController.
 Date:          28/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for 3rd, rail, and flight camera types.
 */
/* Version:     2.7
 Description:   Added TrackController and locked Y-movement on FirstPersonController
 Date:          24/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for 3rd, rail, and flight camera types.
 */
/* Version:     2.6
 Description:   Added Curve folder and containing classes to support TrackCamera.
 Date:          24/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for 3rd, rail, track, flight camera types.
 */
/* Version:     2.5
 Description:   Added IController functionality to Actor3D and added introductory controller examples. Added Controller class as base class for all controllers 
				to enable each specific controller to have id and controller type, and access to game handle.
 Date:          21/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for 3rd, rail, track, flight camera types.
 */
/* Version:     2.4
 Description:   Added helper methods to CameraManager 
                WORK IN PROGRESS.
 Date:          21/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for drawn actors and camera.
 */
/* Version:     2.3
 Description:   Added a CameraManager class to support multi-layout. 
                Added simple 1st Person camera behaviour on Camera3D.
                WORK IN PROGRESS.
 Date:          17/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for drawn actors and camera.
 */
/* Version:     2.2
 Description:   WORK IN PROGRESS...TO COMPLETE IN CLASS
                Begin to refactor Camera2D class to inherit from Actor and use Transform3D and PresentationParameters.
                Added ModelObject to support rendering of 3DS Max and Maya FBS format models.
                Added additional static formats to Presentation Parameters.
                Added Utility folder with some useful classes containing static methods.
 Date:          17/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for drawn actors and camera.
 */
/* Version:     2.1
 Description:   Adds IVertexData support to PrimitiveObject and TexturedPrimitiveObject
 Date:          14/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Add controllers for drawn actors and camera.
 */
/* Version:     2.0
 Description:   Begins to define an inheritance hierarchy for actors (i.e. collidable, non-collidable, helpers, and zones) in the game world.
                Introduce the concept of an IActor and IFilter.
 Date:          13/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         Migrate to IActor based hierarchy and add controllers for drawn actors and camera.
 */
/* Version:     1.0
 Description:   Illustrate the creation of textured quad objects, texture addressing, render states, and alpha blending
 Date:          7/10/16
 Author:        NMCG
 Bugs:          None
 To Do:         
 */

namespace GDApp
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect wireframeEffect, texturedPrimitiveEffect, texturedModelEffect;
        private ObjectManager objectManager;
        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, IVertexData> vertexDictionary;
        private GenericDictionary<string, DrawnActor3D> objectDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private Vector2 screenCentre;
        private GDLibrary.MouseManager mouseManager;
        private  GenericDictionary<string,SpriteFont> fontDictionary;
        private GDLibrary.KeyboardManager keyboardManager;
        private CameraManager cameraManager;
        private Curve1D curve1D;
        private GenericDictionary<string, Transform3DCurve> curveDictionary;
private  ModelObject drivableModelObject;
private PhysicsManager physicsManager;
        #endregion
       
        #region Properties
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return this.graphics;
            }
        }
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
            }
        }
        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager; 
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager; 
            }
        }
        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
        public PhysicsManager PhysicsManager
        {
            get
            {
                return this.physicsManager;
            }
        }
        #endregion

        #region Initialization
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }  
        
        protected override void Initialize()
        {
      
            int width = 1024, height = 768;
            int worldScale = 1000;

            InitializeStaticReferences();
            InitializeGraphics(width, height);
            InitializeEffects();

            InitializeManagers(); 
            InitializeDictionaries();

            LoadFonts();
            LoadModels();
            LoadTextures(); 
            LoadVertices();
            LoadPrimitiveArchetypes();

            #region Non-collidable Objects
            //InitializeNonCollidableGround(worldScale); //removed to add collidable surface see InitializeCollidableGround()
            InitializeNonCollidableObjects();  //3DS Max or Maya FBX format models
            #endregion

            #region Collidable Objects
            InitializeStaticTriangleMeshObjects();
            InitializeDynamicCollidableObjects();
            #endregion

            InitializeCameraTracks();
            InitializeCamera();

            base.Initialize();
        }
        
        private void InitializeStaticReferences()
        {
            Actor.game = this;
            Camera3D.game = this;
            Controller.game = this;
        }
        private void InitializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.ApplyChanges();

            //or we can set full screen
            //   this.graphics.IsFullScreen = true;
            //    this.graphics.ApplyChanges();

            //records screen centre point - used by mouse to see how much the mouse pointer has moved
            this.screenCentre = new Vector2(this.graphics.PreferredBackBufferWidth / 2.0f,
                this.graphics.PreferredBackBufferHeight / 2.0f);
        }
        private void InitializeEffects()
        {
            this.wireframeEffect = new BasicEffect(graphics.GraphicsDevice);
            this.wireframeEffect.VertexColorEnabled = true;

            this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedPrimitiveEffect.VertexColorEnabled = true;
            this.texturedPrimitiveEffect.TextureEnabled = true;

            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
        //    this.texturedModelEffect.VertexColorEnabled = true;
            this.texturedModelEffect.TextureEnabled = true;
           // this.texturedModelEffect.EnableDefaultLighting();

        }
        private void InitializeManagers()
        {
            //CD-CR
            this.physicsManager = new PhysicsManager(this);
            Components.Add(physicsManager);

            bool bDebugMode = true; //show wireframe CD-CR surfaces
            this.objectManager = new ObjectManager(this, "gameObjects", bDebugMode);
            Components.Add(this.objectManager);

            this.mouseManager = new MouseManager(this, true);
            this.mouseManager.SetPosition(this.ScreenCentre);
            Components.Add(this.mouseManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.KeyboardManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);


            //to do...
        }
        private void InitializeDictionaries()
        {
            //"grass", grass.png
            this.textureDictionary = new GenericDictionary<string, Texture2D>("texture dictionary");
            
            this.vertexDictionary = new GenericDictionary<string, IVertexData>("vertex dictionary");
            
            this.objectDictionary = new GenericDictionary<string, DrawnActor3D>("object dictionary");

            this.modelDictionary = new GenericDictionary<string, Model>("model dictionary");

            this.fontDictionary = new GenericDictionary<string, SpriteFont>("font dictionary");

            this.curveDictionary
                = new GenericDictionary<string, Transform3DCurve>("curve dictionary");
        }
        #endregion

        #region Camera
        private void InitializeCameraTracks()
        {
            Transform3DCurve curve = null;

            #region Curve1
            curve = new Transform3DCurve(CurveLoopType.Oscillate);
            curve.Add(new Vector3(0, 10, 200),
                    -Vector3.UnitZ, Vector3.UnitY, 0);

            curve.Add(new Vector3(0, 10, 20),
                   -Vector3.UnitZ, Vector3.UnitX, 2);

            this.curveDictionary.Add("room_action1", curve);
            #endregion
        }
        private void InitializeCamera()
        {
            Transform3D transform = null;
            Camera3D camera = null;
            string cameraLayout = "";

            #region Layout 1x1
            cameraLayout = "1x1";

            #region First Person Camera
            transform = new Transform3D(new Vector3(0, 5, 0), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            camera.AttachController(new FirstPersonController("firstPersControl1",
            ControllerType.FirstPerson, AppData.CameraMoveKeys,
            AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed));

            //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
            this.cameraManager.Add(cameraLayout, camera);
            #endregion
            #endregion

            #region Layout 1x2
            cameraLayout = "1x2";

            #region Left Camera - 1st Person
            transform = new Transform3D(new Vector3(0, 10, 50), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("1st", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, (int)(graphics.PreferredBackBufferWidth / 2.0f), graphics.PreferredBackBufferHeight));

            //attach a 1st person controller
            camera.AttachController(new FirstPersonController("firstPersControl1",
               ControllerType.FirstPerson, AppData.CameraMoveKeys,
               AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed));
            //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
            this.cameraManager.Add(cameraLayout, camera);
            #endregion

            #region Right Camera - 3rd Person
            //it doesnt matter what we set for translation, look, and up since these will be changed by the 3rd Person Controller
            transform = new Transform3D(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("3rd", ActorType.Camera, transform, ProjectionParameters.StandardMediumSixteenNine,
                new Viewport((int)(graphics.PreferredBackBufferWidth / 2.0f), 0, (int)(graphics.PreferredBackBufferWidth / 2.0f), graphics.PreferredBackBufferHeight));

            //attach a 3rd person controller
            camera.AttachController(new ThirdPersonController("thirdPersControl1", ControllerType.ThirdPerson,
                this.drivableModelObject, 100,
                AppData.CameraThirdPersonScrollSpeedDistanceMultiplier,
                160,
                AppData.CameraThirdPersonScrollSpeedElevatationMultiplier,
                AppData.CameraThirdPersonLerpSpeedSlow));
            //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
            this.cameraManager.Add(cameraLayout, camera);
            #endregion
            #endregion

            //finally, set the active layout - clearly there's only 1 layout (i.e. 1x2) but if we have multiple layouts then we need to set
            this.cameraManager.SetActiveCameraLayout("1x1");

        }
        #endregion

        #region Assets
        private void LoadFonts()
        {
            //to do...
        }
        private void LoadModels()
        {
         //   Model m = Content.Load<Model>("Assets/Models/box");
            this.modelDictionary.Add("level", Content.Load<Model>("Assets/Models/level"));
            //Add more models...
        }
        private void LoadTextures()
        {
            #region debug
            this.textureDictionary.Add("ml",
               Content.Load<Texture2D>("Assets/Textures/Debug/ml"));
            this.textureDictionary.Add("checkerboard",
                Content.Load<Texture2D>("Assets/Textures/Debug/checkerboard"));
            #endregion 

            #region ground
            this.textureDictionary.Add("grass1", 
                Content.Load<Texture2D>("Assets/Textures/Foliage/Ground/grass1"));
            #endregion
            
            #region sky
            this.textureDictionary.Add("skybox_back",
                Content.Load<Texture2D>("Assets/Textures/Skybox/back"));
            this.textureDictionary.Add("skybox_front",
                Content.Load<Texture2D>("Assets/Textures/Skybox/front"));
            this.textureDictionary.Add("skybox_left",
                Content.Load<Texture2D>("Assets/Textures/Skybox/left"));
            this.textureDictionary.Add("skybox_right",
                Content.Load<Texture2D>("Assets/Textures/Skybox/right"));
            this.textureDictionary.Add("skybox_sky",
                Content.Load<Texture2D>("Assets/Textures/Skybox/sky"));
            #endregion
            
            #region trees & shrubs
            for (int i = 1; i <= 5; i++)
            {
                this.textureDictionary.Add("tree"  + i,
                    Content.Load<Texture2D>("Assets/Textures/Foliage/Trees/tree" + i));
            }

            for (int i = 1; i <= 3; i++)
            {
                this.textureDictionary.Add("shrub" + i,
                    Content.Load<Texture2D>("Assets/Textures/Foliage/Shrubs/shrub" + i));
            }
            #endregion

            #region fences
            for (int i = 1; i <= 4; i++)
            {
                this.textureDictionary.Add("fence" + i,
                    Content.Load<Texture2D>("Assets/Textures/Architecture/Fence/fence" + i));
            }

            #region walls
            this.textureDictionary.Add("backwall",
                  Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/backwall"));
            this.textureDictionary.Add("sidewall",
                Content.Load<Texture2D>("Assets/Textures/Architecture/Walls/sidewall"));
            #endregion

            #region boxes
            this.textureDictionary.Add("crate1",
                   Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate1"));
            this.textureDictionary.Add("crate2",
                    Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate2"));
            #endregion

            #endregion

            #region other
            this.textureDictionary.Add("abstract1",
            Content.Load<Texture2D>("Assets/Textures/Abstract/abstract1"));
            #endregion


        }
        private void LoadVertices()
        {
            VertexPositionColor[] verticesPositionColor = null;
            VertexPositionColorTexture[] verticesPositionColorTexture = null;
            IVertexData vertexData = null;
            float halfLength = 0.5f;

            #region Textured Quad
            verticesPositionColorTexture = new VertexPositionColorTexture[4];

            //top left
            verticesPositionColorTexture[0] = new VertexPositionColorTexture(
                new Vector3(-halfLength, halfLength, 0), Color.White, new Vector2(0, 0));
            //top right
            verticesPositionColorTexture[1] = new VertexPositionColorTexture(
            new Vector3(halfLength, halfLength, 0), Color.White, new Vector2(1, 0));
            //bottom left
            verticesPositionColorTexture[2] = new VertexPositionColorTexture(
            new Vector3(-halfLength, -halfLength, 0), Color.White, new Vector2(0, 1));
            //bottom right
            verticesPositionColorTexture[3] = new VertexPositionColorTexture(
            new Vector3(halfLength, -halfLength, 0), Color.White, new Vector2(1, 1));

            vertexData = new VertexData<VertexPositionColorTexture>(verticesPositionColorTexture, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip, 2);
            this.vertexDictionary.Add("textured_quad", vertexData);
            #endregion

            #region Textured Cube
            verticesPositionColorTexture = new VertexPositionColorTexture[36];

            Vector3 topLeftFront = new Vector3(-halfLength, halfLength, halfLength);
            Vector3 topLeftBack = new Vector3(-halfLength, halfLength, -halfLength);
            Vector3 topRightFront = new Vector3(halfLength, halfLength, halfLength);
            Vector3 topRightBack = new Vector3(halfLength, halfLength, -halfLength);

            Vector3 bottomLeftFront = new Vector3(-halfLength, -halfLength, halfLength);
            Vector3 bottomLeftBack = new Vector3(-halfLength, -halfLength, -halfLength);
            Vector3 bottomRightFront = new Vector3(halfLength, -halfLength, halfLength);
            Vector3 bottomRightBack = new Vector3(halfLength, -halfLength, -halfLength);

            //uv coordinates
            Vector2 uvTopLeft = new Vector2(0, 0);
            Vector2 uvTopRight = new Vector2(1, 0);
            Vector2 uvBottomLeft = new Vector2(0, 1);
            Vector2 uvBottomRight = new Vector2(1, 1);


            //top - 1 polygon for the top
            verticesPositionColorTexture[0] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[1] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[2] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);

            verticesPositionColorTexture[3] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[4] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            verticesPositionColorTexture[5] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);

            //front
            verticesPositionColorTexture[6] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[7] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
            verticesPositionColorTexture[8] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);

            verticesPositionColorTexture[9] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[10] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
            verticesPositionColorTexture[11] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);

            //back
            verticesPositionColorTexture[12] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            verticesPositionColorTexture[13] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            verticesPositionColorTexture[14] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);

            verticesPositionColorTexture[15] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            verticesPositionColorTexture[16] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[17] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

            //left 
            verticesPositionColorTexture[18] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[19] = new VertexPositionColorTexture(topLeftFront, Color.White, uvTopRight);
            verticesPositionColorTexture[20] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

            verticesPositionColorTexture[21] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);
            verticesPositionColorTexture[22] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[23] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

            //right
            verticesPositionColorTexture[24] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[25] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[26] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            verticesPositionColorTexture[27] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[28] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            verticesPositionColorTexture[29] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            //bottom
            verticesPositionColorTexture[30] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[31] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);
            verticesPositionColorTexture[32] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            verticesPositionColorTexture[33] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[34] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            verticesPositionColorTexture[35] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

            vertexData = new VertexData<VertexPositionColorTexture>(verticesPositionColorTexture, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, 12);
            this.vertexDictionary.Add("textured_cube", vertexData);
            #endregion

            #region Wireframe Origin Helper
            verticesPositionColor = new VertexPositionColor[6];

            //x-axis
            verticesPositionColor[0] = new VertexPositionColor(new Vector3(-halfLength, 0, 0), Color.Red);
            verticesPositionColor[1] = new VertexPositionColor(new Vector3(halfLength, 0, 0), Color.Red);
            //y-axis
            verticesPositionColor[2] = new VertexPositionColor(new Vector3(0, halfLength, 0), Color.Green);
            verticesPositionColor[3] = new VertexPositionColor(new Vector3(0, -halfLength, 0), Color.Green);
            //z-axis
            verticesPositionColor[4] = new VertexPositionColor(new Vector3(0, 0, halfLength), Color.Blue);
            verticesPositionColor[5] = new VertexPositionColor(new Vector3(0, 0, -halfLength), Color.Blue);

            vertexData = new VertexData<VertexPositionColor>(verticesPositionColor, Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList, 3);            
            this.vertexDictionary.Add("wireframe_origin_helper", vertexData);
            #endregion

            #region Wireframe Triangle
            verticesPositionColor = new VertexPositionColor[3];

            verticesPositionColor[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            verticesPositionColor[1] = new VertexPositionColor(new Vector3(1, 0, 0), Color.Green);
            verticesPositionColor[2] = new VertexPositionColor(new Vector3(-1, 0, 0), Color.Blue);

            vertexData = new VertexData<VertexPositionColor>(verticesPositionColor, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip, 1);
            this.vertexDictionary.Add("wireframe_triangle", vertexData);
            #endregion
        }
        private void LoadPrimitiveArchetypes()
        {
            Transform3D transform = null;
            TexturedPrimitiveObject texturedQuad = null;

            #region Textured Quad Archetype
            transform = new Transform3D(Vector3.Zero, Vector3.Zero, Vector3.One, Vector3.UnitZ, Vector3.UnitY);
            texturedQuad = new TexturedPrimitiveObject("textured quad archetype", ActorType.Decorator,
                     transform, this.texturedPrimitiveEffect, this.vertexDictionary["textured_quad"],
                     this.textureDictionary["checkerboard"]); //or  we can leave texture null since we will replace it later

            this.objectDictionary.Add("textured_quad", texturedQuad);
            #endregion
        }
        #endregion

        #region Collidable & Non-collidable Models
        #region Non-collidable
        private void InitializeNonCollidableObjects()
        {
           
        }
        #endregion

        #region Collidable
        //Triangle mesh objects wrap a tight collision surface around complex shapes - the downside is that TriangleMeshObjects CANNOT be moved
        private void InitializeStaticTriangleMeshObjects()
        {
            CollidableObject collidableLevel = null;
            Transform3D transform3DLevel = null;

            transform3DLevel = new Transform3D(new Vector3(0, 0, 0),
                Vector3.Zero, 0.1f * Vector3.One, Vector3.UnitX*200, Vector3.UnitY);
            collidableLevel = new TriangleMeshObject("level", ActorType.CollidableGround,
            transform3DLevel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["ml"], this.modelDictionary["level"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableLevel.Enable(true, 1);
            this.objectManager.Add(collidableLevel);
        }
        
        //if you want objects to be collidable AND moveable then you must attach either a box, sphere, or capsule primitives to the object
        private void InitializeDynamicCollidableObjects()
        {
        }
        #endregion
        #endregion

        #region Game Loop & Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.Escape))
                this.objectManager.Paused = !this.objectManager.Paused;

            if (this.keyboardManager.IsKeyDown(Keys.O))
                this.objectManager.Remove(new ActorIDFilter("origin1"));

            if (this.keyboardManager.IsKeyDown(Keys.P))
                this.objectManager.Remove(new ActorTypeFilter(ActorType.Decorator));

            if (this.keyboardManager.IsKeyDown(Keys.F1))
            {
                this.cameraManager.SetActiveCameraLayout("1x1");
                Window.Title = "1x1 Camera Layout [Static]";
            }
            else if (this.keyboardManager.IsKeyDown(Keys.F2))
            {
                this.cameraManager.SetActiveCameraLayout("1x2");
                Window.Title = "1x2 Camera Layout [1st Person][3rd Person]";
            }
         

            #region Demo
            //float x = this.curve1D.Evaluate((float)gameTime.TotalGameTime.TotalMilliseconds, 2);
            //System.Diagnostics.Debug.WriteLine("X:" + x);

            #endregion

            // this.camera.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            foreach(Camera3D camera in this.cameraManager)
            {
                //set the viewport based on the current camera
                graphics.GraphicsDevice.Viewport = camera.Viewport;
                base.Draw(gameTime);

                //set which is the active camera (remember that our objects use the CameraManager::ActiveCamera property to access View and Projection for rendering
                this.cameraManager.ActiveCameraIndex++;
            }
        }
        #endregion

    }
}
