using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;
using System;
using Microsoft.Xna.Framework.Audio;

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
        private GenericDictionary<string, Transform3DCurve> curveDictionary;
        private PhysicsManager physicsManager;
        private SoundEffect soundEngine;
        private SoundEffectInstance soundEngineInstance;
        private MenuManager menuManager;
        private UIManager uiManager;
        private Microsoft.Xna.Framework.Rectangle screenRectangle;
        private EventDispatcher eventDispatcher;
        #endregion

        #region Properties
        public EventDispatcher EventDispatcher
        {
            get
            {
                return this.eventDispatcher;
            }
        }
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
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

        public Microsoft.Xna.Framework.Rectangle ScreenRectangle
        {
            get
            {
                if (this.screenRectangle == Microsoft.Xna.Framework.Rectangle.Empty)
                    this.screenRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0,
                        (int)graphics.PreferredBackBufferWidth,
                        (int)graphics.PreferredBackBufferHeight);

                return this.screenRectangle;
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
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
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

            InitializeEventDispatcher();
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

            InitialzieMenu();

            
            //to do...


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
            InitializeSound();
            InitializeUI();
            

            base.Initialize();
        }

        private void InitializeEventDispatcher()
        {
            this.eventDispatcher = new EventDispatcher(this, 10);
            Components.Add(this.eventDispatcher);

        }

        private void InitialzieMenu()
        {
            Texture2D[] menuTexturesArray = {this.textureDictionary["mainmenu"],
                this.textureDictionary["audiomenu"],
                this.textureDictionary["controlsmenu"],
                this.textureDictionary["exitmenu"]};

            this.menuManager = new MenuManager(this, menuTexturesArray, this.fontDictionary["menu"], MenuData.MenuTexturePadding, MenuData.MenuTextureColor);
            this.menuManager.DrawOrder = 3; //always draw after ui manager(2)
            Components.Add(this.menuManager);
        }

        private void InitializeSound()
        {
            this.soundEngine = Content.Load<SoundEffect>("Assets\\Audio\\horror_ambiance");
            //SoundEffect soundTestMusicXNAActivated = Content.Load<SoundEffect>("Assets\\Audio\\testmusicXNA");
            this.soundEngineInstance = soundEngine.CreateInstance();
            soundEngineInstance.Volume = 0.1f;
            soundEngineInstance.IsLooped = true;
            soundEngineInstance.Play();
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
           this.texturedModelEffect.EnableDefaultLighting();
            this.texturedModelEffect.PreferPerPixelLighting = true;
            this.texturedModelEffect.SpecularPower = 50;

        }
        private void InitializeManagers()
        {
            //CD-CR
            this.physicsManager = new PhysicsManager(this);
            Components.Add(physicsManager);

            bool bDebugMode = false; //show wireframe CD-CR surfaces
            this.objectManager = new ObjectManager(this, "gameObjects", bDebugMode);
            Components.Add(this.objectManager);

            this.mouseManager = new MouseManager(this, true);
            this.mouseManager.SetPosition(this.ScreenCentre);
            Components.Add(this.mouseManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.KeyboardManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);

            this.uiManager = new UIManager(this, "ui manager", 10, true);
            this.uiManager.DrawOrder = 1;
            Components.Add(this.uiManager);

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
        #region UI
        private void InitializeUI()
        {
            InitializeUIMousePointer();
            InitializeUIProgress();
        }
        private void InitializeUIMousePointer()
        {
            Transform2D transform = null;
            Texture2D texture = null;
            Microsoft.Xna.Framework.Rectangle sourceRectangle;

            //texture
            texture = this.textureDictionary["mouseicons"];
            transform = new Transform2D(Vector2.One);

            //show first of three images from the file
            sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 128, 128);

            UITextureObject texture2DObject = new UIMouseObject("mouse icon",
                ActorType.UITexture,
                StatusType.Drawn | StatusType.Updated,
                transform, new Color(127, 127, 127, 50),
                SpriteEffects.None, 1, texture,
                sourceRectangle,
                new Vector2(sourceRectangle.Width / 2.0f, sourceRectangle.Height / 2.0f),
                true);
            this.uiManager.Add(texture2DObject);
        }
        private void InitializeUIInventoryMenu()
        {
            Transform2D transform = null;
            SpriteFont font = null;
            Texture2D texture = null;

            //text
            font = this.fontDictionary["ui"];
            String text = "help me!";
            Vector2 dimensions = font.MeasureString(text);
            transform = new Transform2D(new Vector2(50, 600), 0, Vector2.One, Vector2.Zero, new Integer2(dimensions));
            UITextObject textObject = new UITextObject("test1",
                ActorType.UIText,
                StatusType.Drawn | StatusType.Updated,
                transform, new Color(15, 15, 15, 150), SpriteEffects.None, 0, "help", font, true);
            this.uiManager.Add(textObject);

            //texture
            texture = this.textureDictionary["white"];
            transform = new Transform2D(new Vector2(40, 590), 0, new Vector2(4, 4), Vector2.Zero, new Integer2(texture.Width, texture.Height));
            UITextureObject texture2DObject = new UITextureObject("texture1",
                 ActorType.UITexture,
                StatusType.Drawn | StatusType.Updated,
                transform, new Color(127, 127, 127, 50),
                SpriteEffects.None, 1, texture, true);
            this.uiManager.Add(texture2DObject);
        }
        private void InitializeUIProgress()
        {
            float separation = 25; //spacing between progress bars

            Transform2D transform = null;
            Texture2D texture = null;
            UITextureObject textureObject = null;
            Vector2 position = Vector2.Zero;
            Vector2 scale = Vector2.Zero;
            float verticalOffset = 700;

            texture = this.textureDictionary["progress_gradient"];
            scale = new Vector2(1, 0.75f);

            #region Player 1 Progress Bar
            position = new Vector2(graphics.PreferredBackBufferWidth / 2.0f - texture.Width * scale.X - separation, verticalOffset);
            transform = new Transform2D(position, 0, scale, Vector2.Zero, new Integer2(texture.Width, texture.Height));

            textureObject = new UITextureObject("leftprogress",
                    ActorType.UITexture,
                    StatusType.Drawn | StatusType.Updated,
                    transform, Color.Green,
                    SpriteEffects.None,
                    0,
                    texture, true);

            //add a controller which listens for pickupeventdata send when the player (or red box) collects the box on the left
            textureObject.AttachController(new UIProgressController("player1", ControllerType.UIProgressController, 2, 10));

            this.uiManager.Add(textureObject);
            #endregion


            #region Player 2 Progress Bar
            position = new Vector2(graphics.PreferredBackBufferWidth / 2.0f + 2 * separation,
                verticalOffset + texture.Height * scale.Y);
            transform = new Transform2D(position, -90,
                scale, Vector2.Zero, new Integer2(texture.Width, texture.Height));

            textureObject = new UITextureObject("rightprogress",
                    ActorType.UITexture,
                    StatusType.Drawn | StatusType.Updated,
                    transform, Color.Red,
                    SpriteEffects.None,
                    0,
                    texture, true);

            //add a controller which listens for pickupeventdata send when the player (or red box) collects the box on the left
            textureObject.AttachController(new UIProgressController("player2", ControllerType.UIProgressController, 8, 10));
            this.uiManager.Add(textureObject);
            #endregion
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

            #region Layout 1x1 Collidable
            cameraLayout = "1x1 Collidable";

            transform = new Transform3D(new Vector3(0,4,6), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            camera.AttachController(new CollidableFirstPersonController(
                camera + " controller",
                ControllerType.FirstPersonCollidable,
                AppData.CameraMoveKeys, AppData.CollidableCameraMoveSpeed / 4f,
                AppData.CollidableCameraStrafeSpeed/3f, AppData.CollidableCameraRotationSpeed,
                0.72f, 4.5f, 1, 1, 1, Vector3.Zero, camera));
            camera.StatusType = StatusType.Drawn;

            this.cameraManager.Add(cameraLayout, camera);
            #endregion

            //finally, set the active layout
            this.cameraManager.SetActiveCameraLayout("1x1 Collidable");

        }
        #endregion

        #region Assets
        private void LoadFonts()
        {
            this.fontDictionary.Add("ui", Content.Load<SpriteFont>("Assets\\Fonts\\UI"));
            this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets\\Fonts\\menu"));
        }
        private void LoadModels()
        {
         //   Model m = Content.Load<Model>("Assets/Models/box");
            this.modelDictionary.Add("level", Content.Load<Model>("Assets/Models/level"));
            //sitting room
            this.modelDictionary.Add("sofa", Content.Load<Model>("Assets/Models/sofa"));
            this.modelDictionary.Add("armChair", Content.Load<Model>("Assets/Models/armChair"));
            this.modelDictionary.Add("tv", Content.Load<Model>("Assets/Models/tv"));
            this.modelDictionary.Add("tvstand", Content.Load<Model>("Assets/Models/tvstand"));
            this.modelDictionary.Add("bookcase", Content.Load<Model>("Assets/Models/bookcase"));
            this.modelDictionary.Add("cabinet", Content.Load<Model>("Assets/Models/cabinet"));
            this.modelDictionary.Add("radio", Content.Load<Model>("Assets/Models/radio"));
            this.modelDictionary.Add("table", Content.Load<Model>("Assets/Models/table"));
            this.modelDictionary.Add("remote", Content.Load<Model>("Assets/Models/remote"));
            this.modelDictionary.Add("key", Content.Load<Model>("Assets/Models/key"));
            this.modelDictionary.Add("winebottle", Content.Load<Model>("Assets/Models/winebottle"));
            //kitchen
            this.modelDictionary.Add("counter", Content.Load<Model>("Assets/Models/counter"));
            this.modelDictionary.Add("fridge", Content.Load<Model>("Assets/Models/fridge"));
            this.modelDictionary.Add("oven", Content.Load<Model>("Assets/Models/oven"));
            this.modelDictionary.Add("toaster", Content.Load<Model>("Assets/Models/toaster"));
            this.modelDictionary.Add("microwave", Content.Load<Model>("Assets/Models/microwave"));
            this.modelDictionary.Add("ktable", Content.Load<Model>("Assets/Models/ktable"));
            this.modelDictionary.Add("kchair", Content.Load<Model>("Assets/Models/kchair"));
            this.modelDictionary.Add("kettle", Content.Load<Model>("Assets/Models/kettle"));
            //Bathroom
            this.modelDictionary.Add("mirror", Content.Load<Model>("Assets/Models/mirror"));
            this.modelDictionary.Add("toilet", Content.Load<Model>("Assets/Models/toilet"));
            this.modelDictionary.Add("sink", Content.Load<Model>("Assets/Models/sink"));
            this.modelDictionary.Add("toiletroll", Content.Load<Model>("Assets/Models/toiletroll"));
            this.modelDictionary.Add("towel", Content.Load<Model>("Assets/Models/towel"));
            this.modelDictionary.Add("bath", Content.Load<Model>("Assets/Models/bath"));
            //Hallway
            this.modelDictionary.Add("clock", Content.Load<Model>("Assets/Models/clock"));
            this.modelDictionary.Add("door", Content.Load<Model>("Assets/Models/door"));
            this.modelDictionary.Add("painting", Content.Load<Model>("Assets/Models/painting"));
            this.modelDictionary.Add("phone", Content.Load<Model>("Assets/Models/phone"));
            //Add more models...
        }
        private void LoadTextures()
        {
            #region debug
            this.textureDictionary.Add("ml", Content.Load<Texture2D>("Assets/Textures/Debug/ml"));
            this.textureDictionary.Add("checkerboard", Content.Load<Texture2D>("Assets/Textures/Debug/checkerboard"));
            this.textureDictionary.Add("gray1", Content.Load<Texture2D>("Assets/Textures/Debug/gray1"));
            this.textureDictionary.Add("gray2", Content.Load<Texture2D>("Assets/Textures/Debug/gray2"));
            this.textureDictionary.Add("gray3", Content.Load<Texture2D>("Assets/Textures/Debug/gray3"));
            this.textureDictionary.Add("wood", Content.Load<Texture2D>("Assets/Textures/Debug/wood"));
            this.textureDictionary.Add("obsidian", Content.Load<Texture2D>("Assets/Textures/Debug/obsidian"));
            this.textureDictionary.Add("material", Content.Load<Texture2D>("Assets/Textures/Debug/material"));
            this.textureDictionary.Add("metal", Content.Load<Texture2D>("Assets/Textures/Debug/metal"));
            this.textureDictionary.Add("marble", Content.Load<Texture2D>("Assets/Textures/Debug/marble"));
            this.textureDictionary.Add("p1", Content.Load<Texture2D>("Assets/Textures/Debug/painting1"));
            this.textureDictionary.Add("p2", Content.Load<Texture2D>("Assets/Textures/Debug/painting2"));
            this.textureDictionary.Add("plastic", Content.Load<Texture2D>("Assets/Textures/Debug/plastic"));
            #endregion

            //menu
            this.textureDictionary.Add("mainmenu", Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu"));
            this.textureDictionary.Add("audiomenu", Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu"));
            this.textureDictionary.Add("controlsmenu", Content.Load<Texture2D>("Assets/Textures/Menu/controlsmenu"));
            this.textureDictionary.Add("exitmenuwithtrans", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));
            this.textureDictionary.Add("exitmenu", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenu"));

            #region UI
            this.textureDictionary.Add("white", Content.Load<Texture2D>("Assets\\Textures\\UI\\white"));
            this.textureDictionary.Add("progress_gradient", Content.Load<Texture2D>("Assets\\Textures\\UI\\progress_gradient"));
            this.textureDictionary.Add("progress_white", Content.Load<Texture2D>("Assets\\Textures\\UI\\progress_white"));
            this.textureDictionary.Add("mouseicons", Content.Load<Texture2D>("Assets/Textures/UI/mouseicons"));
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
            #region level
            CollidableObject collidableLevel = null;
            Transform3D transform3DLevel = null;

            transform3DLevel = new Transform3D(new Vector3(0, -0.2f, 0),
                Vector3.Zero, 0.1f * new Vector3(1,1.4f,1), Vector3.UnitX*200, Vector3.UnitY);
            collidableLevel = new TriangleMeshObject("level", ActorType.CollidableGround,
            transform3DLevel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["gray1"], this.modelDictionary["level"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableLevel.Enable(true, 1);
            this.objectManager.Add(collidableLevel);
            #endregion

            #region Sitting room
            #region Sofa
            CollidableObject collidableSofa = null;
            Transform3D transform3DSofa = null;

            transform3DSofa = new Transform3D(new Vector3(6, 1.26f, 9),
                new Vector3(0,180,0), 0.4f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableSofa = new TriangleMeshObject("sofa", ActorType.CollidableProp,
            transform3DSofa, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["material"], this.modelDictionary["sofa"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableSofa.Enable(true, 1);
            this.objectManager.Add(collidableSofa);
            #endregion

            #region arm chair
            CollidableObject collidableArmChair = null;
            Transform3D transform3DArmChair = null;

            transform3DArmChair = new Transform3D(new Vector3(6, 1.26f, 9),
                new Vector3(0, 180, 0), 0.4f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableArmChair = new TriangleMeshObject("armChair", ActorType.CollidableProp,
            transform3DArmChair, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["material"], this.modelDictionary["armChair"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableArmChair.Enable(true, 1);
            this.objectManager.Add(collidableArmChair);
            #endregion

            #region tv
            CollidableObject collidableTv = null;
            Transform3D transform3DTv = null;

            transform3DTv = new Transform3D(new Vector3(1, 2f, -11),
                Vector3.Zero, 0.1f * Vector3.One/5, Vector3.UnitX, Vector3.UnitY);
            collidableTv = new TriangleMeshObject("tv", ActorType.CollidableProp,
            transform3DTv, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["obsidian"], this.modelDictionary["tv"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTv.Enable(true, 1);
            this.objectManager.Add(collidableTv);
            #endregion

            #region tvstand
            CollidableObject collidableTvstand = null;
            Transform3D transform3DTvstand = null;

            transform3DTvstand = new Transform3D(new Vector3(0.9f, 0.5f, -11.1f),
                 new Vector3(0, -90, 0), 0.01f * new Vector3(1.9f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableTvstand = new TriangleMeshObject("tvstand", ActorType.CollidableProp,
            transform3DTvstand, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["tvstand"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTvstand.Enable(true, 1);
            this.objectManager.Add(collidableTvstand);
            #endregion

            #region bookcase
            CollidableObject collidableBookcase = null;
            Transform3D transform3DBookcase = null;

            transform3DBookcase = new Transform3D(new Vector3(9, 1f, -11),
                Vector3.Zero, 0.0065f * new Vector3(1.5f,1,1) , Vector3.UnitX, Vector3.UnitY);
            collidableBookcase = new TriangleMeshObject("bookcase", ActorType.CollidableProp,
            transform3DBookcase, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["bookcase"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableBookcase.Enable(true, 1);
            this.objectManager.Add(collidableBookcase);
            #endregion

            #region cabinet
            CollidableObject collidableCabinet = null;
            Transform3D transform3DCabinet = null;

            transform3DCabinet = new Transform3D(new Vector3(-9, 0.5f, 9),
               new Vector3(0, 135, 0), 0.02f * new Vector3(1, 1, 2), Vector3.UnitX, Vector3.UnitY);
            collidableCabinet = new TriangleMeshObject("cabinet", ActorType.CollidableProp,
            transform3DCabinet, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["cabinet"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableCabinet.Enable(true, 1);
            this.objectManager.Add(collidableCabinet);
            #endregion

            #region radio
            CollidableObject collidableRadio = null;
            Transform3D transform3DRadio = null;

            transform3DRadio = new Transform3D(new Vector3(-9, 2.23f, 9),
             new Vector3(0, -10, 0), 0.003f * new Vector3(1, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableRadio = new TriangleMeshObject("radio", ActorType.CollidableProp,
            transform3DRadio, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["obsidian"], this.modelDictionary["radio"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableRadio.Enable(true, 1);
            this.objectManager.Add(collidableRadio);
            #endregion

            #region table
            CollidableObject collidableTable = null;
            Transform3D transform3DTable = null;

            transform3DTable = new Transform3D(new Vector3(-1, 1.25f, 3),
             new Vector3(0, 0, 0), 0.5f * new Vector3(0.7f, 0.6f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableTable = new TriangleMeshObject("table", ActorType.CollidableProp,
            transform3DTable, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["table"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTable.Enable(true, 1);
            this.objectManager.Add(collidableTable);
            #endregion

            #region remote
            CollidableObject collidableRemote = null;
            Transform3D transform3DRemote = null;

            transform3DRemote = new Transform3D(new Vector3(-2, 1.95f, 3.5f),
             new Vector3(0, 30, 0), 0.07f * new Vector3(1f, 1f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableRemote = new TriangleMeshObject("remote", ActorType.CollidableProp,
            transform3DRemote, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["obsidian"], this.modelDictionary["remote"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableRemote.Enable(true, 1);
            this.objectManager.Add(collidableRemote);
            #endregion

            #region key
            CollidableObject collidableKey = null;
            Transform3D transform3DKey = null;

            transform3DKey = new Transform3D(new Vector3(1, 2f, 3.2f),
                new Vector3(0, 0, 0), 0.3f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableKey = new TriangleMeshObject("key", ActorType.CollidableProp,
            transform3DKey, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["key"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableKey.Enable(true, 1);
            this.objectManager.Add(collidableKey);
            #endregion

            #region winebottle
            CollidableObject collidableWineBottle = null;
            Transform3D transform3DWineBottle = null;

            transform3DWineBottle = new Transform3D(new Vector3(-1, 2.2f, 3.5f),
                new Vector3(180, 0, 0), 0.25f * new Vector3(1.2f, 0.7f, 1.2f), Vector3.UnitX, Vector3.UnitY);
            collidableWineBottle = new TriangleMeshObject("winebottle", ActorType.CollidableProp,
            transform3DWineBottle, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["marble"], this.modelDictionary["winebottle"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableWineBottle.Enable(true, 1);
            this.objectManager.Add(collidableWineBottle);
            #endregion

            #endregion

            #region Kitchen
            #region counter
            CollidableObject collidableCounter = null;
            Transform3D transform3DCounter = null;

            transform3DCounter = new Transform3D(new Vector3(28.5f, 0.4f, 1.1f),
             new Vector3(0, -90, 0), 0.13f * new Vector3(1f, 0.85f, 1f), Vector3.UnitX, Vector3.UnitY);
            collidableCounter = new TriangleMeshObject("counter", ActorType.CollidableProp,
            transform3DCounter, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["counter"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableCounter.Enable(true, 1);
            this.objectManager.Add(collidableCounter);
            #endregion

            #region oven
            CollidableObject collidableOven = null;
            Transform3D transform3DOven = null;

            transform3DOven = new Transform3D(new Vector3(27.5f, 0.1f, 0.255f),
             new Vector3(0, -90, 0), 0.5f * new Vector3(0.915f, 0.8f, 1f), Vector3.UnitX, Vector3.UnitY);
            collidableOven = new TriangleMeshObject("oven", ActorType.CollidableProp,
            transform3DOven, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["oven"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableOven.Enable(true, 1);
            this.objectManager.Add(collidableOven);
            #endregion

            #region fridge
            CollidableObject collidableFridge = null;
            Transform3D transform3DFridge = null;

            transform3DFridge = new Transform3D(new Vector3(28f, 0.5f, 5.6f),
             new Vector3(0, -90, 0), 0.13f * new Vector3(1f, 0.8f, 1.1f), Vector3.UnitX, Vector3.UnitY);
            collidableFridge = new TriangleMeshObject("fridge", ActorType.CollidableProp,
            transform3DFridge, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["fridge"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableFridge.Enable(true, 1);
            this.objectManager.Add(collidableFridge);
            #endregion

            #region toaster
            CollidableObject collidableToaster = null;
            Transform3D transform3DToaster = null;

            transform3DToaster = new Transform3D(new Vector3(27.5f, 3f, 10f),
             new Vector3(0, 60, 0), 0.13f * new Vector3(1f, 0.8f, 1.1f), Vector3.UnitX, Vector3.UnitY);
            collidableToaster = new TriangleMeshObject("toaster", ActorType.CollidableProp,
            transform3DToaster, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["toaster"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableToaster.Enable(true, 1);
            this.objectManager.Add(collidableToaster);
            #endregion

            #region microwave
            CollidableObject collidableMicrowave = null;
            Transform3D transform3DMicrowave = null;

            transform3DMicrowave = new Transform3D(new Vector3(22.2f, 3.3f, 11.5f),
                new Vector3(0, 180, 0), 1.7f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableMicrowave = new TriangleMeshObject("microwave", ActorType.CollidableProp,
            transform3DMicrowave, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["microwave"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableMicrowave.Enable(true, 1);
            this.objectManager.Add(collidableMicrowave);
            #endregion

            #region kitchentable
            CollidableObject collidableKtable = null;
            Transform3D transform3DKtable = null;

            transform3DKtable = new Transform3D(new Vector3(20, 0.5f, -11f),
             new Vector3(0, -90, 0), 0.019f * new Vector3(1.5f, 0.7f, 1.5f), Vector3.UnitX, Vector3.UnitY);
            collidableKtable = new TriangleMeshObject("ktable", ActorType.CollidableProp,
            transform3DKtable, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["ktable"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableKtable.Enable(true, 1);
            this.objectManager.Add(collidableKtable);
            #endregion

            #region kchair
            CollidableObject collidableKchair = null;
            Transform3D transform3DKchair = null;

            transform3DKchair = new Transform3D(new Vector3(22.25f, 0.5f, -10f),
            new Vector3(0, -120, 0), 0.011f * new Vector3(1f, 0.8f, 1.1f), Vector3.UnitX, Vector3.UnitY);
            collidableKchair = new TriangleMeshObject("kchair", ActorType.CollidableProp,
            transform3DKchair, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["kchair"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableKchair.Enable(true, 1);
            this.objectManager.Add(collidableKchair);
            #endregion

            #region kettle
            CollidableObject collidableKettle = null;
            Transform3D transform3DKettle = null;

            transform3DKettle = new Transform3D(new Vector3(26.5f, 2.7f, 12f),
             new Vector3(0, 120, 0), 0.05f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableKettle = new TriangleMeshObject("kettle", ActorType.CollidableProp,
            transform3DKettle, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["kettle"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableKettle.Enable(true, 1);
            this.objectManager.Add(collidableKettle);
            #endregion

            #endregion

            #region Bathroom
            #region mirror
            CollidableObject collidableMirror = null;
            Transform3D transform3DMirror = null;

            transform3DMirror = new Transform3D(new Vector3(8.5f, 4f, -34),
                new Vector3(0, 90, 0), new Vector3(2, 2, 4), Vector3.UnitX, Vector3.UnitY);
            collidableMirror = new TriangleMeshObject("mirror", ActorType.CollidableProp,
            transform3DMirror, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["metal"], this.modelDictionary["mirror"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableMirror.Enable(true, 1);
            this.objectManager.Add(collidableMirror);
            #endregion

            #region toilet
            CollidableObject collidableToilet = null;
            Transform3D transform3DToilet = null;

            transform3DToilet = new Transform3D(new Vector3(14.5f, .5f, -32),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableToilet = new TriangleMeshObject("toilet", ActorType.CollidableProp,
            transform3DToilet, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["marble"], this.modelDictionary["toilet"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableToilet.Enable(true, 1);
            this.objectManager.Add(collidableToilet);
            #endregion

            #region sink
            CollidableObject collidableSink = null;
            Transform3D transform3DSink = null;

            transform3DSink = new Transform3D(new Vector3(8.5f, .4f, -33.25f),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableSink = new TriangleMeshObject("sink", ActorType.CollidableProp,
            transform3DSink, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["marble"], this.modelDictionary["sink"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableSink.Enable(true, 1);
            this.objectManager.Add(collidableSink);
            #endregion

            #region toiletroll
            CollidableObject collidableToiletRoll = null;
            Transform3D transform3DToiletRoll = null;

            transform3DToiletRoll = new Transform3D(new Vector3(12.25f, 2.5f, -34),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableToiletRoll = new TriangleMeshObject("toiletroll", ActorType.CollidableProp,
            transform3DToiletRoll, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["marble"], this.modelDictionary["toiletroll"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableToiletRoll.Enable(true, 1);
            this.objectManager.Add(collidableToiletRoll);
            #endregion

            #region towel-rack
            CollidableObject collidableTowel = null;
            Transform3D transform3DTowel = null;

            transform3DTowel = new Transform3D(new Vector3(18.5f, 2.5f, -34),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableTowel = new TriangleMeshObject("marble", ActorType.CollidableProp,
            transform3DTowel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["material"], this.modelDictionary["towel"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTowel.Enable(true, 1);
            this.objectManager.Add(collidableTowel);

            transform3DTowel = new Transform3D(new Vector3(21.5f, 2.5f, -34),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableTowel = new TriangleMeshObject("marble", ActorType.CollidableProp,
            transform3DTowel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["checkerboard"], this.modelDictionary["towel"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTowel.Enable(true, 1);
            this.objectManager.Add(collidableTowel);
            #endregion

            #region Bath
            CollidableObject collidableBath = null;
            Transform3D transform3DBath = null;

            transform3DBath = new Transform3D(new Vector3(24f, 0.4f, -28.5f),
                new Vector3(0, 90, 0), 0.07f * new Vector3(1.4f, 0.8f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableBath = new TriangleMeshObject("bath", ActorType.CollidableProp,
            transform3DBath, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["marble"], this.modelDictionary["bath"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableBath.Enable(true, 1);
            this.objectManager.Add(collidableBath);
            #endregion

            #endregion

            #region Hallway
            #region clock
            CollidableObject collidableClock = null;
            Transform3D transform3DClock = null;

            transform3DClock = new Transform3D(new Vector3(12.75f, .5f, -19.75f),
                new Vector3(0, -90, 0), 0.019f * new Vector3(2f, 1, 2), Vector3.UnitX, Vector3.UnitY);
            collidableClock = new TriangleMeshObject("clock", ActorType.CollidableProp,
            transform3DClock, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["clock"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableClock.Enable(true, 1);
            this.objectManager.Add(collidableClock);
            #endregion

            #region door
            CollidableObject collidableDoor = null;
            Transform3D transform3DDoor = null;

            transform3DDoor = new Transform3D(new Vector3(-12.6f, .5f, -15.3f),
                new Vector3(0, 90, 0), 0.019f * new Vector3(2f, 1.1f, 2), Vector3.UnitX, Vector3.UnitY);
            collidableDoor = new TriangleMeshObject("door", ActorType.CollidableProp,
            transform3DDoor, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["door"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableDoor.Enable(true, 1);
            this.objectManager.Add(collidableDoor);
            #endregion

            #region HallCabinet
            CollidableObject collidableHallCabinet = null;
            Transform3D transform3DHallCabinet = null;

            transform3DHallCabinet = new Transform3D(new Vector3(0.9f, 0.5f, -20.1f),
                 new Vector3(0, -90, 0), 0.01f * new Vector3(1.9f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallCabinet = new TriangleMeshObject("hallcabinet", ActorType.CollidableProp,
            transform3DHallCabinet, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["wood"], this.modelDictionary["tvstand"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallCabinet.Enable(true, 1);
            this.objectManager.Add(collidableHallCabinet);
            #endregion

            #region paintings
            CollidableObject collidableHallPainting = null;
            Transform3D transform3DHallPainting = null;

            transform3DHallPainting = new Transform3D(new Vector3(-2f, 02f, -13.3f),
                 new Vector3(0, -90, 0), 0.2f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallPainting = new TriangleMeshObject("painting", ActorType.CollidableProp,
            transform3DHallPainting, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["p1"], this.modelDictionary["painting"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallPainting.Enable(true, 1);
            this.objectManager.Add(collidableHallPainting);

            transform3DHallPainting = new Transform3D(new Vector3(2f, 02.5f, -13.3f),
                 new Vector3(0, -90, 0), 0.1f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallPainting = new TriangleMeshObject("painting", ActorType.CollidableProp,
            transform3DHallPainting, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["ml"], this.modelDictionary["painting"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallPainting.Enable(true, 1);
            this.objectManager.Add(collidableHallPainting);

            transform3DHallPainting = new Transform3D(new Vector3(6f, 02f, -13.3f),
                 new Vector3(0, -90, 0), 0.2f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallPainting = new TriangleMeshObject("painting", ActorType.CollidableProp,
            transform3DHallPainting, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["p2"], this.modelDictionary["painting"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallPainting.Enable(true, 1);
            this.objectManager.Add(collidableHallPainting);
            #endregion

            #region Phone
            CollidableObject collidablePhone = null;
            Transform3D transform3DPhone = null;

            transform3DPhone = new Transform3D(new Vector3(0.9f, 1.93f, -20.1f),
                 new Vector3(0, 90, 0), 0.1f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidablePhone = new TriangleMeshObject("phone", ActorType.CollidableProp,
            transform3DPhone, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["plastic"], this.modelDictionary["phone"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidablePhone.Enable(true, 1);
            this.objectManager.Add(collidablePhone);
            #endregion

            #endregion

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
            if (this.keyboardManager.IsFirstKeyPress(Keys.Escape)) { 
                this.cameraManager.ActiveCamera.StatusType = StatusType.Drawn;
            }

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
