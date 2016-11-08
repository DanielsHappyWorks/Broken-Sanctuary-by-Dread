using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class ModelObject : DrawnActor3D, ICloneable
    {
        #region Variables
        private Texture2D texture;
        private Model model;
        private Matrix[] boneTransforms;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        public Model Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }
        public Matrix[] BoneTransforms
        {
            get
            {
                return this.boneTransforms;
            }
            set
            {
                this.boneTransforms = value;
            }
        }
        #endregion

        public ModelObject(string id, ActorType actorType, 
            Transform3D transform, BasicEffect effect, Color color, float alpha,
            Texture2D texture, Model model)
            : base(id, actorType, transform, effect, color, alpha)
        {
            this.texture = texture;
            this.model = model;

            InitializeBoneTransforms();
        }

        private void InitializeBoneTransforms()
        {
            //load bone transforms and copy transfroms to transform array (transforms)
            if (this.model != null)
            {
                this.boneTransforms = new Matrix[this.model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.Effect.View = game.CameraManager.ActiveCamera.View;
            this.Effect.Projection = game.CameraManager.ActiveCamera.Projection;
            this.Effect.World = this.Transform3D.World;
            this.Effect.DiffuseColor = this.Color.ToVector3();
            this.Effect.Alpha = this.Alpha;
            this.Effect.Texture = this.texture;
            this.Effect.CurrentTechnique.Passes[0].Apply();

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = this.Effect;
                }
                this.Effect.World
                    = this.BoneTransforms[mesh.ParentBone.Index]
                                        * GetWorldMatrix(); //CD-CR support
                                    //* this.Transform3D.World;
                mesh.Draw();
            }

            base.Draw(gameTime);
        }

        public new object Clone()
        {
            return new ModelObject("clone - " + ID, //deep
                this.ActorType,   //deep
                (Transform3D)this.Transform3D.Clone(),  //deep
                this.Effect, //shallow i.e. a reference
                this.Color,  //deep
                this.Alpha,  //deep
                this.texture, //shallow i.e. a reference
                this.model); //shallow i.e. a reference
        }
    }
}
