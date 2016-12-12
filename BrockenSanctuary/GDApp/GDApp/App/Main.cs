using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GDLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;
using System;
#region VersionHistory
// Based on GDApp code C# & XNA by Niall McGuinness from course 3D Game Development
#region Team Dread
/*
 Author @ Team Dread
 * Daniel Foth (DF) - Lead Programmer
 * Peter Fitzmaurice (PF) - ScrumMaster, Assistant Project Lead
 * Kayleigh Grumley (KG) - Assistant Lead Programmer
 * Patrick Collins (PC) - Project Lead, Animator, Researcher
 * All team members created/sourced assets, implemented assets & features (programming)
 * tested, "bug-hunted", and researched.
 */
#endregion
#region TO DO
/*remote or tv should access tvOn bool and "turn tv on/off". Best case off is normal billboard
with "tvoff" texture, on is scroller with "static", eventually to be replaced with credits texture*/
#endregion
#region v18.75
/*  Version:     v18.75
Description:   Tweak Winebottle audio, finish implementation and instantiate billboard
Date:          09/12/16
Author:        Dread
Latest Handler: PC & DF
Bugs:           Done/
Notes:          billboard doesn't react to remote/tv clicks yet...code for that included but not working
                not disposing of old billboard correctly*/
#endregion
#region v18.6
/*  Version:     v18.6 <Abandoned Branch>
Description:   Break out bathroom as separate space or level inside game for finer texturing control
Date:          09/12/16
Author:        Dread
Latest Handler: PC
Bugs:           Collide in bathroom doorway; couldn't resolve; Abandoned*/
#endregion
#region v18.5
/*  Version:     v18.5
Description:   Billboard base code added to code base, bugs hunted and terminated
Date:          09/12/16
Author:        Dread
Latest Handler: DF
Bugs:           Stage Completed*/
#endregion
#region v18
/*  Version:     18!
Description:   Mouse Handler Improved! Clock Pendulum animation on Clock interaction!
Date:          09/12/16
Author:        Dread
Latest Handler: PC(animation) DF(mouse tweaking)
Bugs:           Stage Completed*/
#endregion
#region v17.85
/*  Version:     17.85
Description:   In-Progress: Improving Mouse Handling
Date:          08/12/16
Author:        Dread
Latest Handler: DF
Bugs:           In Progress*/
#endregion
#region v17.75
/*  Version:     17.75
    Description:   In-Progress: Adding 3DS Max animations, pendulum model, animated key model
    Date:          07/12/16
    Author:        Dread
    Latest Handler: PC
    Bugs:           In Progress*/
#endregion
#region v17.5
/*  Version:     17.5
   Description:   Work-around for 17.25 issue is to attach controller when key spawns. This worked.
   Date:          07/12/16
   Author:        Dread
   Latest Handler: PC
   Bugs:           Working; Mouse wonky, sound clashy, but functional*/
#endregion
#region v17.25
/* Version:     17.25
   Description:   Attempting to animate key on Pickup. Added KeyPickUpController (+ enum).
                   Attempt to call from UiMouseObject, but logic is failing; want to run animation (controller)
                   for 3.5 seconds, then detach controller and remove item.
   Date:          07/12/16
   Author:        Dread
   Latest Handler: PC
   Bugs:           Permaloop on pickup, crash
   **
   (20+ previous versions, each with minor or major iterative changes including functionality, bug fixes, events
   and asset loads)*/
#endregion
#endregion

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
        private GenericDictionary<string, SpriteFont> fontDictionary;
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
        public SoundManager soundManager;
        private string telly="tvoff";

        //event bools
        public bool playerHasKey;
        public bool keyCreated;
        private bool interactedWithKettle;
        private bool interactedWithMicrowave;
        private bool interactedWithToaster;
        private bool interactedWithFridge;
        private bool interactedWithOven;
        private UIProgressController anxietyBar;
        public bool interactedWithClock;
        private bool interactedWithSink;
        private bool interactedWithMirror;
        private bool interactedWithToilet;
        private bool interactedWithVino;
        private bool interactedWithRadio;
        private bool interactedWithTV;
        private bool interactedWithRemote;
        private bool tvOn;
        private int key;
        private bool interactedWithPhone;
        private CollidableFirstPersonController playerController;
        private Effect billboardEffect;
        BillboardPrimitiveObject cloneBillboardObject;
        //private object billboardArchetypeObject;
        BillboardPrimitiveObject billboardArchetypeObject;

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
            InitializeBooleans();

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
            InitializeNonCollidableBillboards();
            #endregion

            #region Collidable Objects
            InitializeStaticTriangleMeshObjects();
            InitializeDynamicCollidableObjects();
            #endregion

            InitializeCameraTracks();
            InitializeCamera();
            InitializeUI();

            #region Event Handling
            this.eventDispatcher.MenuChanged += EventDispatcher_MenuChanged;
            this.eventDispatcher.Interaction += EventDispatcher_Interaction;
            #endregion

            base.Initialize();
        }

        private void InitializeBooleans()
        {
            playerHasKey = false;
            keyCreated = false;
            interactedWithKettle = false;
            interactedWithMicrowave = false;
            interactedWithToaster = false;
            interactedWithFridge = false;
            interactedWithOven = false;
            interactedWithClock = false;
            interactedWithRadio = false;
            // this.interactedWithPhone = false;
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
                this.textureDictionary["exitmenu"],
                this.textureDictionary["restartmenu"],
                this.textureDictionary["losemenu"],
                this.textureDictionary["winmenu"]};

            this.menuManager = new MenuManager(this, menuTexturesArray, this.fontDictionary["menu"], MenuData.MenuTexturePadding, MenuData.MenuTextureColor);
            this.menuManager.DrawOrder = 3; //always draw after ui manager(2)
            Components.Add(this.menuManager);
        }

        public void RestartGame()
        {
            //this.cameraManager.ActiveCamera.Transform3D = this.cameraManager.ActiveCamera.Transform3D.OriginalTransform3D;

            //Initialize();
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
            this.billboardEffect = Content.Load<Effect>("Assets/Effects/Billboard");

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

            this.soundManager = new SoundManager(this, "Content/Assets/Audio/Demo2DSound.xgs", "Content/Assets/Audio/WaveBank1.xwb", "Content/Assets/Audio/SoundBank1.xsb");
            Components.Add(this.soundManager);

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
            sourceRectangle = AppData.MouseRectangleThirdIcon;

            UITextureObject texture2DObject = new UIMouseObject("mouse icon",
                ActorType.UITexture,
                StatusType.Drawn | StatusType.Updated,
                transform, new Color(127, 127, 127, 50),
                SpriteEffects.None,
                this.fontDictionary["mouse"], "", new Vector2(0, 80), Color.White,
                1, texture,
                sourceRectangle,
                new Vector2(sourceRectangle.Width / 2.0f, sourceRectangle.Height / 2.0f),
                true);
            this.uiManager.Add(texture2DObject);
        }
        private void InitializeUIProgress()
        {

            Transform2D transform = null;
            Texture2D texture = null;
            UITextureObject textureObject = null;
            Vector2 position = Vector2.Zero;
            Vector2 scale = Vector2.Zero;
            float verticalOffset = 2;
            float horizontalOffset = 5;

            texture = this.textureDictionary["barWhite"];
            scale = new Vector2(1, 0.75f);

            #region Player 1 Progress Bar
            position = new Vector2(horizontalOffset, verticalOffset);
            transform = new Transform2D(position, 0, scale, Vector2.Zero, new Integer2(texture.Width, texture.Height));

            texture = this.textureDictionary["barOutline"];
            scale = new Vector2(1, 0.75f);

            textureObject = new UITextureObject("background",
                    ActorType.UITexture,
                    StatusType.Drawn | StatusType.Updated,
                    transform, Color.White,
                    SpriteEffects.None,
                    0,
                    texture);
            this.uiManager.Add(textureObject);

            this.anxietyBar = new UIProgressController("anxiety", ControllerType.UIProgressController, 10, 100);
            texture = this.textureDictionary["barColour"];
            scale = new Vector2(1, 0.75f);
            textureObject = new UITextureObject("colour",
                    ActorType.UITexture,
                    StatusType.Drawn | StatusType.Updated,
                    transform, Color.White,
                    SpriteEffects.None,
                    0.1f,
                    texture);
            textureObject.AttachController(this.anxietyBar);
            this.uiManager.Add(textureObject);
            //add a controller which listens for pickupeventdata send when the player (or red box) collects the box on the left

            texture = this.textureDictionary["barWhite"];
            scale = new Vector2(1, 0.75f);
            textureObject = new UITextureObject("foreground",
                    ActorType.UITexture,
                    StatusType.Drawn | StatusType.Updated,
                    transform, Color.White,
                    SpriteEffects.None,
                    0.2f,
                    texture);

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

            transform = new Transform3D(new Vector3(0, 4, 6), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            this.playerController = new CollidableFirstPersonController(
                camera + " controller",
                ControllerType.FirstPersonCollidable,
                AppData.CameraMoveKeys, AppData.CollidableCameraMoveSpeed / 4f,
                AppData.CollidableCameraStrafeSpeed / 3f, AppData.CollidableCameraRotationSpeed,
                0.72f, 4.5f, 1, 1, 1, Vector3.Zero, camera);
            camera.AttachController(playerController);
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
            this.fontDictionary.Add("mouse", Content.Load<SpriteFont>("Assets\\Fonts\\mouse"));
        }
        private void LoadModels()
        {
            //   Model m = Content.Load<Model>("Assets/Models/box");
            this.modelDictionary.Add("levelfloor", Content.Load<Model>("Assets/Models/floors"));
            this.modelDictionary.Add("levelceling", Content.Load<Model>("Assets/Models/ceilings"));
            this.modelDictionary.Add("levelwall", Content.Load<Model>("Assets/Models/walls"));
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
            this.modelDictionary.Add("pendulum", Content.Load<Model>("Assets/Models/pendulum"));
            this.modelDictionary.Add("door", Content.Load<Model>("Assets/Models/door"));
            this.modelDictionary.Add("painting", Content.Load<Model>("Assets/Models/painting"));
            this.modelDictionary.Add("phone", Content.Load<Model>("Assets/Models/phone"));
            //Add more models...
        }
        private void LoadTextures()
        {
            #region game
            this.textureDictionary.Add("beech", Content.Load<Texture2D>("Assets/Textures/Game/beech"));
            this.textureDictionary.Add("bglass", Content.Load<Texture2D>("Assets/Textures/Game/bglass"));
            this.textureDictionary.Add("bplastic", Content.Load<Texture2D>("Assets/Textures/Game/bplastic"));
            this.textureDictionary.Add("burloak", Content.Load<Texture2D>("Assets/Textures/Game/burloak"));
            this.textureDictionary.Add("burlwalnut", Content.Load<Texture2D>("Assets/Textures/Game/burlwalnut"));
            this.textureDictionary.Add("ceiling", Content.Load<Texture2D>("Assets/Textures/Game/ceiling"));
            this.textureDictionary.Add("cloth", Content.Load<Texture2D>("Assets/Textures/Game/cloth"));
            this.textureDictionary.Add("cloth1", Content.Load<Texture2D>("Assets/Textures/Game/cloth1"));
            this.textureDictionary.Add("gglass", Content.Load<Texture2D>("Assets/Textures/Game/gglass"));
            this.textureDictionary.Add("gold", Content.Load<Texture2D>("Assets/Textures/Game/gold"));
            this.textureDictionary.Add("grayplastic", Content.Load<Texture2D>("Assets/Textures/Game/grayplastic"));
            this.textureDictionary.Add("mag1", Content.Load<Texture2D>("Assets/Textures/Game/mag1"));
            this.textureDictionary.Add("oak", Content.Load<Texture2D>("Assets/Textures/Game/oak"));
            this.textureDictionary.Add("oakfloor", Content.Load<Texture2D>("Assets/Textures/Game/oakfloor"));
            this.textureDictionary.Add("porcelain", Content.Load<Texture2D>("Assets/Textures/Game/porcelain"));
            this.textureDictionary.Add("red-metal", Content.Load<Texture2D>("Assets/Textures/Game/red-metal"));
            this.textureDictionary.Add("shinywhite", Content.Load<Texture2D>("Assets/Textures/Game/shinywhite"));
            this.textureDictionary.Add("silver", Content.Load<Texture2D>("Assets/Textures/Game/silver"));
            this.textureDictionary.Add("sofa", Content.Load<Texture2D>("Assets/Textures/Game/sofa"));
            this.textureDictionary.Add("sofa2", Content.Load<Texture2D>("Assets/Textures/Game/sofa2"));
            this.textureDictionary.Add("steph", Content.Load<Texture2D>("Assets/Textures/Game/steph"));
            this.textureDictionary.Add("teak", Content.Load<Texture2D>("Assets/Textures/Game/teak"));
            this.textureDictionary.Add("walfloor", Content.Load<Texture2D>("Assets/Textures/Game/walfloor"));
            this.textureDictionary.Add("wallpaper", Content.Load<Texture2D>("Assets/Textures/Game/wallpaper"));
            this.textureDictionary.Add("wallpaper2", Content.Load<Texture2D>("Assets/Textures/Game/wallpaper2"));
            this.textureDictionary.Add("white", Content.Load<Texture2D>("Assets/Textures/Game/white"));
            this.textureDictionary.Add("munch", Content.Load<Texture2D>("Assets/Textures/Game/munch"));
            this.textureDictionary.Add("monet", Content.Load<Texture2D>("Assets/Textures/Game/monet"));
            this.textureDictionary.Add("mahog", Content.Load<Texture2D>("Assets/Textures/Game/mahog"));
            this.textureDictionary.Add("pine", Content.Load<Texture2D>("Assets/Textures/Game/pine"));
            this.textureDictionary.Add("walnut", Content.Load<Texture2D>("Assets/Textures/Game/walnut"));
            this.textureDictionary.Add("credits", Content.Load<Texture2D>("Assets/Textures/Game/credits"));
            this.textureDictionary.Add("bikkie", Content.Load<Texture2D>("Assets/Textures/Game/bikkie"));
            this.textureDictionary.Add("bikkie1", Content.Load<Texture2D>("Assets/Textures/Game/bikkie1"));
            #endregion
            #region debug
            /* this.textureDictionary.Add("ml", Content.Load<Texture2D>("Assets/Textures/Debug/ml"));
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
             this.textureDictionary.Add("steph", Content.Load<Texture2D>("Assets/Textures/Debug/steph"));
             this.textureDictionary.Add("plastic", Content.Load<Texture2D>("Assets/Textures/Debug/plastic"));
             this.textureDictionary.Add("gold", Content.Load<Texture2D>("Assets/Textures/Debug/gold"));
             this.textureDictionary.Add("porcelain", Content.Load<Texture2D>("Assets/Textures/Debug/porcelain"));
             this.textureDictionary.Add("blackplastic", Content.Load<Texture2D>("Assets/Textures/Debug/blackplastic"));
             this.textureDictionary.Add("burloak", Content.Load<Texture2D>("Assets/Textures/Debug/burloak"));
             this.textureDictionary.Add("burlwalnut", Content.Load<Texture2D>("Assets/Textures/Debug/burlwalnut"));
             this.textureDictionary.Add("static", Content.Load<Texture2D>("Assets/Textures/Debug/static"));
             this.textureDictionary.Add("tvoff", Content.Load<Texture2D>("Assets/Textures/Debug/tvoff"));
             this.textureDictionary.Add("credits", Content.Load<Texture2D>("Assets/Textures/Debug/credits"));*/
            #endregion

            //level
            /* this.textureDictionary.Add("floor", Content.Load<Texture2D>("Assets/Textures/Level/floor"));
             this.textureDictionary.Add("wallBad", Content.Load<Texture2D>("Assets/Textures/Level/tiles3"));
             this.textureDictionary.Add("ceiling", Content.Load<Texture2D>("Assets\\Textures\\Level\\ceiling"));*/

            //menu
            this.textureDictionary.Add("mainmenu", Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu"));
            this.textureDictionary.Add("audiomenu", Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu"));
            this.textureDictionary.Add("controlsmenu", Content.Load<Texture2D>("Assets/Textures/Menu/controlsmenu"));
            this.textureDictionary.Add("exitmenuwithtrans", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));
            this.textureDictionary.Add("exitmenu", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenu"));
            this.textureDictionary.Add("restartmenu", Content.Load<Texture2D>("Assets/Textures/Menu/restartmenu"));
            this.textureDictionary.Add("losemenu", Content.Load<Texture2D>("Assets/Textures/Menu/losemenu"));
            this.textureDictionary.Add("winmenu", Content.Load<Texture2D>("Assets/Textures/Menu/winmenu"));

            #region UI
            this.textureDictionary.Add("progress_gradient", Content.Load<Texture2D>("Assets\\Textures\\UI\\progress_gradient"));
            this.textureDictionary.Add("progress_white", Content.Load<Texture2D>("Assets\\Textures\\UI\\progress_white"));
            this.textureDictionary.Add("mouseicons", Content.Load<Texture2D>("Assets/Textures/UI/mouseicons"));
            this.textureDictionary.Add("barColour", Content.Load<Texture2D>("Assets/Textures/UI/barColour"));
            this.textureDictionary.Add("barOutline", Content.Load<Texture2D>("Assets/Textures/UI/barOutline"));
            this.textureDictionary.Add("barWhite", Content.Load<Texture2D>("Assets/Textures/UI/barWhite"));
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

        }
        #endregion

        #region Collidable & Non-collidable Models
        #region Non-collidable
        private void InitializeNonCollidableObjects()
        {

        }
        private void InitializeNonCollidableBillboards()
        {
            BillboardPrimitiveObject billboardArchetypeObject = null;
            BillboardPrimitiveObject cloneBillboardObject = null;

            //archetype - clone from this
            billboardArchetypeObject = new BillboardPrimitiveObject("billboard", ActorType.Billboard,
                Transform3D.Zero, //transform reset in clones
                this.billboardEffect,
                this.vertexDictionary["textured_quad"],
                this.textureDictionary["credits"],
                Color.White, 1,
                StatusType.Drawn | StatusType.Updated,
                BillboardType.Normal); //texture reset in clones

             #region Normal
             /*cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
             cloneBillboardObject.BillboardType = BillboardType.Normal;
             cloneBillboardObject.Transform3D = new Transform3D(new Vector3(.95f, 1.7f, -10.2f), new Vector3(-10, 0, 0), new Vector3(3.75f, 2, 1), Vector3.UnitX, Vector3.UnitY);
             cloneBillboardObject.Texture = this.textureDictionary[telly];
             this.objectManager.Add(cloneBillboardObject);*/
             #endregion


            #region Normal Scrolling
            cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
            cloneBillboardObject.Transform3D = new Transform3D(new Vector3(.95f, 1.7f, -10.2f), new Vector3(-10, 0, 0), new Vector3(3.75f, 2, 1), Vector3.UnitX, Vector3.UnitY);
            cloneBillboardObject.Alpha = 0.8f; //remember we can set alpha
            cloneBillboardObject.Texture = this.textureDictionary["credits"];
            cloneBillboardObject.BillboardParameters.SetScrolling(true);
            cloneBillboardObject.BillboardParameters.SetScrollRate(new Vector2(0, 25));
            this.objectManager.Add(cloneBillboardObject);
        }
            #endregion
            #endregion

            #region Collidable
            //Triangle mesh objects wrap a tight collision surface around complex shapes - the downside is that TriangleMeshObjects CANNOT be moved
        private void InitializeStaticTriangleMeshObjects()
        {
            #region level
            CollidableObject collidableLevel = null;
            Transform3D transform3DLevel = null;

            transform3DLevel = new Transform3D(new Vector3(0, -0.2f, 0),
                Vector3.Zero, 0.1f * new Vector3(1, 1.4f, 1), Vector3.UnitX * 200, Vector3.UnitY);
            collidableLevel = new TriangleMeshObject("levelfloor", ActorType.CollidableGround,
            transform3DLevel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["oakfloor"], this.modelDictionary["levelfloor"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableLevel.Enable(true, 1);
            this.objectManager.Add(collidableLevel);

            transform3DLevel = new Transform3D(new Vector3(0, -0.2f, 0),
                Vector3.Zero, 0.1f * new Vector3(1, 1.4f, 1), Vector3.UnitX * 200, Vector3.UnitY);
            collidableLevel = new TriangleMeshObject("levelwall", ActorType.CollidableGround,
            transform3DLevel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["bikkie"], this.modelDictionary["levelwall"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableLevel.Enable(true, 1);
            this.objectManager.Add(collidableLevel);

            transform3DLevel = new Transform3D(new Vector3(0, -0.2f, 0),
                Vector3.Zero, 0.1f * new Vector3(1, 1.4f, 1), Vector3.UnitX * 200, Vector3.UnitY);
            collidableLevel = new TriangleMeshObject("levelceling", ActorType.CollidableGround,
            transform3DLevel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["ceiling"], this.modelDictionary["levelceling"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableLevel.Enable(true, 1);
            this.objectManager.Add(collidableLevel);
            #endregion

            #region Sitting room
            #region Sofa
            CollidableObject collidableSofa = null;
            Transform3D transform3DSofa = null;

            transform3DSofa = new Transform3D(new Vector3(6, 1.26f, 9),
                new Vector3(0, 180, 0), 0.4f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableSofa = new TriangleMeshObject("sofa", ActorType.CollidableProp,
            transform3DSofa, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["sofa"], this.modelDictionary["sofa"],
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
            this.textureDictionary["sofa2"], this.modelDictionary["armChair"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableArmChair.Enable(true, 1);
            this.objectManager.Add(collidableArmChair);
            #endregion

            #region tv
            CollidableObject collidableTv = null;
            Transform3D transform3DTv = null;

            transform3DTv = new Transform3D(new Vector3(1, 2f, -11),
                Vector3.Zero, 0.1f * Vector3.One / 5, Vector3.UnitX, Vector3.UnitY);
            collidableTv = new TriangleMeshObject("tv", ActorType.CollidableInteractableProp,
            transform3DTv, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["bplastic"], this.modelDictionary["tv"],
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
            this.textureDictionary["burloak"], this.modelDictionary["tvstand"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTvstand.Enable(true, 1);
            this.objectManager.Add(collidableTvstand);
            #endregion

            #region bookcase
            CollidableObject collidableBookcase = null;
            Transform3D transform3DBookcase = null;

            transform3DBookcase = new Transform3D(new Vector3(9, 1f, -11),
                Vector3.Zero, 0.0065f * new Vector3(1.5f, 1, 1), Vector3.UnitX, Vector3.UnitY);
            collidableBookcase = new TriangleMeshObject("bookcase", ActorType.CollidableProp,
            transform3DBookcase, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["pine"], this.modelDictionary["bookcase"],
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
            this.textureDictionary["oak"], this.modelDictionary["cabinet"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableCabinet.Enable(true, 1);
            this.objectManager.Add(collidableCabinet);
            #endregion

            #region radio
            CollidableObject collidableRadio = null;
            Transform3D transform3DRadio = null;

            transform3DRadio = new Transform3D(new Vector3(-9, 2.23f, 9),
             new Vector3(0, -10, 0), 0.003f * new Vector3(1, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableRadio = new TriangleMeshObject("radio", ActorType.CollidableInteractableProp,
            transform3DRadio, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["grayplastic"], this.modelDictionary["radio"],
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
            this.textureDictionary["walnut"], this.modelDictionary["table"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTable.Enable(true, 1);
            this.objectManager.Add(collidableTable);
            #endregion

            #region remote
            CollidableObject collidableRemote = null;
            Transform3D transform3DRemote = null;

            transform3DRemote = new Transform3D(new Vector3(-2, 1.95f, 3.5f),
             new Vector3(0, 30, 0), 0.07f * new Vector3(1f, 1f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableRemote = new TriangleMeshObject("remote", ActorType.CollidableInteractableProp,
            transform3DRemote, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["bplastic"], this.modelDictionary["remote"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableRemote.Enable(true, 1);
            this.objectManager.Add(collidableRemote);
            #endregion

            #region winebottle
            CollidableObject collidableWineBottle = null;
            Transform3D transform3DWineBottle = null;

            transform3DWineBottle = new Transform3D(new Vector3(-1, 2.2f, 3.5f),
                new Vector3(180, 0, 0), 0.25f * new Vector3(1.2f, 0.7f, 1.2f), Vector3.UnitX, Vector3.UnitY);
            collidableWineBottle = new TriangleMeshObject("winebottle", ActorType.CollidableInteractableProp,
            transform3DWineBottle, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["gglass"], this.modelDictionary["winebottle"],
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
            this.textureDictionary["mahog"], this.modelDictionary["counter"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableCounter.Enable(true, 1);
            this.objectManager.Add(collidableCounter);
            #endregion

            #region oven
            CollidableObject collidableOven = null;
            Transform3D transform3DOven = null;

            transform3DOven = new Transform3D(new Vector3(27.5f, 0.1f, 0.255f),
             new Vector3(0, -90, 0), 0.5f * new Vector3(0.915f, 0.8f, 1f), Vector3.UnitX, Vector3.UnitY);
            collidableOven = new TriangleMeshObject("oven", ActorType.CollidableInteractableProp,
            transform3DOven, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["bglass"], this.modelDictionary["oven"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableOven.Enable(true, 1);
            this.objectManager.Add(collidableOven);
            #endregion

            #region fridge
            CollidableObject collidableFridge = null;
            Transform3D transform3DFridge = null;

            transform3DFridge = new Transform3D(new Vector3(28f, 0.5f, 5.6f),
             new Vector3(0, -90, 0), 0.13f * new Vector3(1f, 0.8f, 1.1f), Vector3.UnitX, Vector3.UnitY);
            collidableFridge = new TriangleMeshObject("fridge", ActorType.CollidableInteractableProp,
            transform3DFridge, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["red-metal"], this.modelDictionary["fridge"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableFridge.Enable(true, 1);
            this.objectManager.Add(collidableFridge);
            #endregion

            #region toaster
            CollidableObject collidableToaster = null;
            Transform3D transform3DToaster = null;

            transform3DToaster = new Transform3D(new Vector3(27.5f, 3f, 10f),
             new Vector3(0, 60, 0), 0.13f * new Vector3(1f, 0.8f, 1.1f), Vector3.UnitX, Vector3.UnitY);
            collidableToaster = new TriangleMeshObject("toaster", ActorType.CollidableInteractableProp,
            transform3DToaster, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["grayplastic"], this.modelDictionary["toaster"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableToaster.Enable(true, 1);
            this.objectManager.Add(collidableToaster);
            #endregion

            #region microwave
            CollidableObject collidableMicrowave = null;
            Transform3D transform3DMicrowave = null;

            transform3DMicrowave = new Transform3D(new Vector3(22.2f, 3.3f, 11.5f),
                new Vector3(0, 180, 0), 1.7f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableMicrowave = new TriangleMeshObject("microwave", ActorType.CollidableInteractableProp,
            transform3DMicrowave, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["bglass"], this.modelDictionary["microwave"],
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
            this.textureDictionary["pine"], this.modelDictionary["ktable"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableKtable.Enable(true, 1);
            this.objectManager.Add(collidableKtable);
            #endregion

            #region kchair
            CollidableObject collidableKchair = null;
            Transform3D transform3DKchair = null;

            transform3DKchair = new Transform3D(new Vector3(22.25f, 0.5f, -10f),
            new Vector3(0, -120, 0), 0.011f * new Vector3(1f, 0.8f, 1.1f), Vector3.UnitX, Vector3.UnitY);
            collidableKchair = new TriangleMeshObject("kchair", ActorType.CollidableInteractableProp,
            transform3DKchair, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["teak"], this.modelDictionary["kchair"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableKchair.Enable(true, 1);
            this.objectManager.Add(collidableKchair);
            #endregion

            #region kettle
            CollidableObject collidableKettle = null;
            Transform3D transform3DKettle = null;

            transform3DKettle = new Transform3D(new Vector3(26.5f, 2.7f, 12f),
             new Vector3(0, 120, 0), 0.05f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            collidableKettle = new TriangleMeshObject("kettle", ActorType.CollidableInteractableProp,
            transform3DKettle, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["silver"], this.modelDictionary["kettle"],
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
            collidableMirror = new TriangleMeshObject("mirror", ActorType.CollidableInteractableProp,
            transform3DMirror, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["silver"], this.modelDictionary["mirror"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableMirror.Enable(true, 1);
            this.objectManager.Add(collidableMirror);
            #endregion

            #region toilet
            CollidableObject collidableToilet = null;
            Transform3D transform3DToilet = null;

            transform3DToilet = new Transform3D(new Vector3(14.5f, .5f, -32),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableToilet = new TriangleMeshObject("toilet", ActorType.CollidableInteractableProp,
            transform3DToilet, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["porcelain"], this.modelDictionary["toilet"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableToilet.Enable(true, 1);
            this.objectManager.Add(collidableToilet);
            #endregion

            #region sink
            CollidableObject collidableSink = null;
            Transform3D transform3DSink = null;

            transform3DSink = new Transform3D(new Vector3(8.5f, .4f, -33.25f),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableSink = new TriangleMeshObject("sink", ActorType.CollidableInteractableProp,
            transform3DSink, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["porcelain"], this.modelDictionary["sink"],
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
            this.textureDictionary["porcelain"], this.modelDictionary["toiletroll"],
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
            this.textureDictionary["cloth"], this.modelDictionary["towel"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableTowel.Enable(true, 1);
            this.objectManager.Add(collidableTowel);

            transform3DTowel = new Transform3D(new Vector3(21.5f, 2.5f, -34),
                new Vector3(0, 0, 0), 0.045f * new Vector3(1.2f, 0.7f, 1), Vector3.UnitX, Vector3.UnitY);
            collidableTowel = new TriangleMeshObject("marble", ActorType.CollidableProp,
            transform3DTowel, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["cloth1"], this.modelDictionary["towel"],
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
            this.textureDictionary["porcelain"], this.modelDictionary["bath"],
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
            collidableClock = new TriangleMeshObject("clock", ActorType.CollidableInteractableProp,
            transform3DClock, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["burlwalnut"], this.modelDictionary["clock"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableClock.Enable(true, 1);
            this.objectManager.Add(collidableClock);
            #endregion
            #region pendulum
            CollidableObject collidablePendulum = null;
            Transform3D transform3DPendulum = null;

            transform3DPendulum = new Transform3D(new Vector3(12.14f, 2.9f, -19.75f),
                new Vector3(-15, 0, 0), 0.009f * new Vector3(1, 1, 1), Vector3.UnitX, Vector3.UnitY);
            collidablePendulum = new TriangleMeshObject("pendulum", ActorType.CollidableInteractableProp,
            transform3DPendulum, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["gold"], this.modelDictionary["pendulum"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidablePendulum.Enable(true, 1);
            collidablePendulum.AttachController(new PendulumController("Pendulum",
                        ControllerType.PendulumController));
            this.objectManager.Add(collidablePendulum);
            #endregion

            #region door
            CollidableObject collidableDoor = null;
            Transform3D transform3DDoor = null;

            transform3DDoor = new Transform3D(new Vector3(-12.6f, .5f, -15.3f),
                new Vector3(0, 90, 0), 0.019f * new Vector3(2f, 1.1f, 2), Vector3.UnitX, Vector3.UnitY);
            collidableDoor = new TriangleMeshObject("door", ActorType.CollidableInteractableProp,
            transform3DDoor, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["teak"], this.modelDictionary["door"],
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
            this.textureDictionary["mahog"], this.modelDictionary["tvstand"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallCabinet.Enable(true, 1);
            this.objectManager.Add(collidableHallCabinet);
            #endregion

            #region paintings
            CollidableObject collidableHallPainting = null;
            Transform3D transform3DHallPainting = null;

            transform3DHallPainting = new Transform3D(new Vector3(-2f, 02f, -13.3f),
                 new Vector3(0, -90, 0), 0.2f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallPainting = new TriangleMeshObject("painting1", ActorType.CollidableInteractableProp,
            transform3DHallPainting, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["monet"], this.modelDictionary["painting"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallPainting.Enable(true, 1);
            this.objectManager.Add(collidableHallPainting);

            transform3DHallPainting = new Transform3D(new Vector3(2f, 02.5f, -13.3f),
                 new Vector3(0, -90, 0), 0.1f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallPainting = new TriangleMeshObject("painting2", ActorType.CollidableInteractableProp,
            transform3DHallPainting, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["steph"], this.modelDictionary["painting"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallPainting.Enable(true, 1);
            this.objectManager.Add(collidableHallPainting);

            transform3DHallPainting = new Transform3D(new Vector3(6f, 02f, -13.3f),
                 new Vector3(0, -90, 0), 0.2f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidableHallPainting = new TriangleMeshObject("painting3", ActorType.CollidableInteractableProp,
            transform3DHallPainting, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["munch"], this.modelDictionary["painting"],
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            collidableHallPainting.Enable(true, 1);
            this.objectManager.Add(collidableHallPainting);
            #endregion

            #region Phone
            CollidableObject collidablePhone = null;
            Transform3D transform3DPhone = null;

            transform3DPhone = new Transform3D(new Vector3(0.9f, 1.93f, -20.1f),
                 new Vector3(0, 90, 0), 0.1f * new Vector3(1f, 1f, 1f) / 5, Vector3.UnitX, Vector3.UnitY);
            collidablePhone = new TriangleMeshObject("phone", ActorType.CollidableInteractableProp,
            transform3DPhone, this.texturedModelEffect,
            Color.White, 1,
            this.textureDictionary["grayplastic"], this.modelDictionary["phone"],
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
            if (this.keyboardManager.IsFirstKeyPress(Keys.Escape))
            {
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


            foreach (Camera3D camera in this.cameraManager)
            {
                //set the viewport based on the current camera
                graphics.GraphicsDevice.Viewport = camera.Viewport;
                base.Draw(gameTime);

                //set which is the active camera (remember that our objects use the CameraManager::ActiveCamera property to access View and Projection for rendering
                this.cameraManager.ActiveCameraIndex++;
            }
        }
        #endregion

        private void EventDispatcher_MenuChanged(EventData eventData)
        {
            if (eventData.EventType == EventActionType.OnExit)
                this.Exit();
            else if (eventData.EventType == EventActionType.OnRestart)
                this.RestartGame();

        }
        #region Events
        private void EventDispatcher_Interaction(EventData eventData)
        {
            if (eventData.EventType == EventActionType.PickUpKey)
            {
                if (!playerHasKey)
                {
                    //animation uses KeyPickupController, called when key spawns
                    this.playerHasKey = true;
                    this.anxietyBar.incrementProgressBarFillSpeed(-0.02f);
                    this.soundManager.PlayCue("key");
                    this.soundManager.PlayCue("johnm7");
                }
            }
            else if (eventData.EventType == EventActionType.WinGame)
            {
                //run win menu
                //reset game
                //this.menuManager.ShowWinMenuScreen();
                if (this.cameraManager.ActiveCamera.StatusType != 0)
                {
                    this.soundManager.PlayCue("johnm11");
                    this.cameraManager.ActiveCamera.StatusType = 0;
                    this.menuManager.ShowMenu();
                    this.menuManager.ShowWinMenuScreen();
                    this.soundManager.PlayCue("win");
                }
            }
            else if (eventData.EventType == EventActionType.LoseGame)
            {
                //run loose menu
                //reset game
                if (this.cameraManager.ActiveCamera.StatusType != 0)
                {
                    this.cameraManager.ActiveCamera.StatusType = 0;
                    this.menuManager.ShowMenu();
                    this.menuManager.ShowLoseMenuScreen();
                    this.soundManager.PlayCue("lose");
                }
            }
            else if (eventData.EventType == EventActionType.PhoneInteraction)
            {
                //spawn the key
                //dialog etc
                if (this.keyCreated == false)
                {
                    this.soundManager.StopCue("phoneRing", Microsoft.Xna.Framework.Audio.AudioStopOptions.Immediate);
                    #region key
                    Random rnd = new Random();
                    this.key = rnd.Next(1, 5);
                    float x, y, z;
                    if (key == 1)//toaster
                    {
                        x = 27.8f;
                        y = 3.26f;
                        z = 11.1f;
                    }
                    else if (key == 2)//sink
                    {
                        x = 9;
                        y = 3.07f;
                        z = -33.2f;
                    }
                    else if (key == 3)
                    {
                        //under Table
                        x = 20.8f;
                        y = 1.05f;
                        z = -11.4f;
                    }
                    else if (key == 4)//radio
                    {
                        x = -9.69f;
                        y = 2.77f;
                        z = 9.2f;
                    }
                    else
                    {
                        x = -9.69f;
                        y = 2.77f;
                        z = 9.2f;
                    }
                    CollidableObject collidableKey = null;
                    Transform3D transform3DKey = null;
                    //..................behind microwave........(27.8f, 2.76f, 11.1f)
                    //..................in bathroom sink........(9, 2.57f, -33.2f)
                    transform3DKey = new Transform3D(new Vector3(x, y, z),
                        new Vector3(0, 0, 0), 0.3f * Vector3.One, Vector3.UnitX, Vector3.UnitY);
                    collidableKey = new TriangleMeshObject("key", ActorType.Pickup,
                    transform3DKey, this.texturedModelEffect,
                    Color.White, 1,
                    this.textureDictionary["gold"], this.modelDictionary["key"],
                        new MaterialProperties(0.2f, 0.8f, 0.7f));
                    collidableKey.AttachController(new KeyPickupController("KeyPickup", 
                        ControllerType.KeyPickupController, 
                        new Vector3(.07f, 0, .03f)));//Handler for procedural key animation
                    collidableKey.Enable(true, 1);
                    this.objectManager.Add(collidableKey);
                    #endregion
                    this.soundManager.PlayCue("pickupphone");
                    this.anxietyBar.incrementProgressBarFillSpeed(0.022f);
                    this.keyCreated = true;

                }
                else
                {
                    if (!this.interactedWithPhone)
                    {
                        //   this.interactedWithPhone = true;
                        if (key == 1)
                        {
                            if (this.anxietyBar.CurrentValue > 70)
                            {
                                this.soundManager.PlayCue("phoneconversation1");
                                this.anxietyBar.incrementProgressBar(-45);
                                this.anxietyBar.incrementProgressBarFillSpeed(-0.015f);
                                this.interactedWithPhone = true;
                            }
                        }
                        else if (key == 2)
                        {
                            if (this.anxietyBar.CurrentValue > 70)
                            {
                                this.soundManager.PlayCue("phoneconversation3");
                                this.anxietyBar.incrementProgressBar(-45);
                                this.anxietyBar.incrementProgressBarFillSpeed(-0.015f);
                                this.interactedWithPhone = true;
                            }
                        }
                        else if (key == 3)
                        {
                            if (this.anxietyBar.CurrentValue > 70)
                            {
                                this.soundManager.PlayCue("phoneconversation2");
                                this.anxietyBar.incrementProgressBar(-45);
                                this.anxietyBar.incrementProgressBarFillSpeed(-0.015f);
                                this.interactedWithPhone = true;
                            }
                        }
                        else if (key == 4)
                        {
                            if (this.anxietyBar.CurrentValue > 70)
                            {
                                this.soundManager.PlayCue("phoneconversation4");
                                this.anxietyBar.incrementProgressBar(-45);
                                this.anxietyBar.incrementProgressBarFillSpeed(-0.015f);
                                this.interactedWithPhone = true;
                            }
                        }

                    }

                }

            }
            else if (eventData.EventType == EventActionType.ClockInteraction)
            {
                if (this.interactedWithClock)
                {
                    this.soundManager.StopCue("clock", AudioStopOptions.Immediate);
                    this.anxietyBar.incrementProgressBarFillSpeed(0.01f);
                    this.interactedWithClock = false;
                }
                else
                {
                    this.soundManager.PlayCue("clock");
                    this.anxietyBar.incrementProgressBarFillSpeed(-0.01f);
                    this.interactedWithClock = true;
                    /*this.ObjectManager().AttachController(new PendulumController("Pendulum",
                        ControllerType.PendulumController,
                        new Vector3(.07f, 0, .03f)));*///
                }
            }
            else if (eventData.EventType == EventActionType.PaintingGoodInteraction)
            {
                this.soundManager.PlayCue("p1");
                this.anxietyBar.incrementProgressBar(-8);
            }
            else if (eventData.EventType == EventActionType.PaintingBadInteraction)
            {
                if (this.anxietyBar.CurrentValue <= 50)
                {
                    this.soundManager.PlayCue("p3");
                    this.anxietyBar.incrementProgressBar(5);
                }
                else
                {
                    this.soundManager.PlayCue("scream");
                    this.anxietyBar.incrementProgressBar(8);
                }
            }
            else if (eventData.EventType == EventActionType.PaintingOtherInteraction)
            {
                this.soundManager.PlayCue("p2");
                this.anxietyBar.incrementProgressBar(-5);
            }
            else if (eventData.EventType == EventActionType.KettleInteraction)
            {
                if (!this.interactedWithKettle)
                {
                    interactedWithKettle = true;
                    this.soundManager.PlayCue("kettle");
                }
            }
            else if (eventData.EventType == EventActionType.MicrowaveInteraction)
            {
                if (!this.interactedWithMicrowave)
                {
                    interactedWithMicrowave = true;
                    this.soundManager.PlayCue("microwave");
                    this.anxietyBar.incrementProgressBar(5);
                }
            }
            else if (eventData.EventType == EventActionType.ChairInteraction)
            {

            }
            else if (eventData.EventType == EventActionType.ToasterInteraction)
            {
                if (!this.interactedWithToaster)
                {
                    interactedWithToaster = true;
                    this.soundManager.PlayCue("toaster");
                    this.anxietyBar.incrementProgressBar(8);
                }
            }
            else if (eventData.EventType == EventActionType.FridgeInteraction)
            {
                if (!this.interactedWithFridge)
                {
                    interactedWithFridge = true;
                    this.soundManager.PlayCue("fridge");
                    this.anxietyBar.incrementProgressBar(-10);
                }
            }
            else if (eventData.EventType == EventActionType.OvenInteraction)
            {
                if (!this.interactedWithOven)
                {
                    interactedWithOven = true;
                    this.soundManager.PlayCue("oven");
                }
            }

            else if (eventData.EventType == EventActionType.SinkInteraction)
            {
                if (!this.interactedWithSink)
                {
                    interactedWithSink = true;
                    this.soundManager.PlayCue("sink");

                }
            }
            else if (eventData.EventType == EventActionType.MirrorInteraction)
            {
                if (!this.interactedWithMirror)
                {
                    interactedWithMirror = true;
                    this.soundManager.PlayCue("mirror");
                    this.anxietyBar.incrementProgressBar(10);

                }
            }
            else if (eventData.EventType == EventActionType.ToiletInteraction)
            {
                if (!this.interactedWithToilet)
                {
                    interactedWithToilet = true;
                    this.soundManager.PlayCue("toilet");
                    this.anxietyBar.incrementProgressBar(-12);
                    this.anxietyBar.incrementProgressBarFillSpeed(-0.01f);

                }
            }
            else if (eventData.EventType == EventActionType.RadioInteraction)
            {
                if (this.interactedWithRadio)
                {
                    this.soundManager.StopCue("click", AudioStopOptions.Immediate);
                    this.soundManager.StopCue("music", AudioStopOptions.Immediate);
                    this.anxietyBar.incrementProgressBarFillSpeed(0.01f);
                    this.interactedWithRadio = false;
                }
                else
                {
                    this.soundManager.PlayCue("click");
                    this.soundManager.PlayCue("music");
                    this.anxietyBar.incrementProgressBarFillSpeed(-0.01f);
                    this.interactedWithRadio = true;
                }
            }
            else if (eventData.EventType == EventActionType.RemoteInteraction)
            {
                if (!this.tvOn)
                {
                    //cloneBillboardObject = null;
                    //this.objectManager.Remove(cloneBillboardObject);
                    //cloneBillboardObject = null;
                    //telly = "static";
                    tvOn = true;
                    /*cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
                    cloneBillboardObject.Transform3D = new Transform3D(new Vector3(.95f, 1.7f, -10.2f), new Vector3(-10, 0, 0), new Vector3(3.75f, 2, 1), Vector3.UnitX, Vector3.UnitY);
                    cloneBillboardObject.Alpha = 0.8f; //remember we can set alpha
                    cloneBillboardObject.Texture = this.textureDictionary[telly];
                    cloneBillboardObject.BillboardParameters.SetScrolling(true);
                    cloneBillboardObject.BillboardParameters.SetScrollRate(new Vector2(0, -50));
                    this.objectManager.Add(cloneBillboardObject);*/
                    this.soundManager.PlayCue("click");
                }
                else
                {
                    //this.objectManager.Remove(cloneBillboardObject);
                   // telly = "tvoff";
                    tvOn = false;
                   /* cloneBillboardObject = (BillboardPrimitiveObject)billboardArchetypeObject.Clone();
                    cloneBillboardObject.BillboardType = BillboardType.Normal;
                    cloneBillboardObject.Transform3D = new Transform3D(new Vector3(.95f, 1.7f, -10.2f), new Vector3(-10, 0, 0), new Vector3(3.75f, 2, 1), Vector3.UnitX, Vector3.UnitY);
                    cloneBillboardObject.Texture = this.textureDictionary[telly];
                    this.objectManager.Add(cloneBillboardObject);*/
                    this.soundManager.PlayCue("click");
                }
            }
            else if (eventData.EventType == EventActionType.TVInteraction)
            {
                if (!this.tvOn)
                {
                    telly="static";
                    tvOn = true;
                    this.soundManager.PlayCue("click");
                }
                else
                {
                    telly = "tvoff";
                    tvOn = false;
                    this.soundManager.PlayCue("click");
                    
                }
            }
            else if (eventData.EventType == EventActionType.VinoInteraction)
            {
                if (!this.interactedWithVino)
                {
                    interactedWithVino = true;
                    this.soundManager.PlayCue("bottle");
                    this.anxietyBar.incrementProgressBar(-6);
                }
            }
        }
        #endregion
    }
}
