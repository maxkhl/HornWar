using Horn_War_II.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Horn_War_II.Spawn
{
    partial class SpawnTool : Form
    {
        Scene GameScene;
        IEnumerable<Type> SpriteTypes;
        IEnumerable<Type> UpdateTypes;

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public SpawnTool(Scene GameScene)
        {
            this.GameScene = GameScene;

            InitializeComponent();

            var spriteGroup = new ListViewGroup("Sprite Objects");
            lV_Objects.Groups.Add(spriteGroup);

            var goGroup = new ListViewGroup("Game Objects");
            lV_Objects.Groups.Add(goGroup);

            // Find and generate spriteTypes
            SpriteTypes = 
                (from type in Assembly.GetAssembly(typeof(GameObjects.SpriteObject)).GetTypes()
                 where
                     type.IsClass &&
                     !type.IsAbstract &&
                     type.IsSubclassOf(typeof(GameObjects.SpriteObject)) &&
                     type.GetCustomAttribute(typeof(SpawnAttribute)) != null
                 select type);


            lV_Objects.LargeImageList = GeneratePreviewImages();


            foreach (var sType in SpriteTypes)
            {
                lV_Objects.Items.Add(
                    new ListViewItem(sType.Name, spriteGroup)
                    {
                        ImageKey = sType.GetCustomAttribute<SpawnAttribute>().PreviewImage,
                        Tag = sType
                    });
            }


            // Find and generate update types
            UpdateTypes =
                (from type in Assembly.GetAssembly(typeof(GameObjects.SpriteObject)).GetTypes()
                 where
                     type.IsClass &&
                     !type.IsAbstract &&
                     type.IsSubclassOf(typeof(GameObjects.GameObject)) &&
                     !type.IsSubclassOf(typeof(GameObjects.SpriteObject)) &&
                     type.GetCustomAttribute(typeof(SpawnAttribute)) != null
                 select type);

            foreach (var uType in UpdateTypes)
            {
                lV_Objects.Items.Add(
                    new ListViewItem(uType.Name, goGroup)
                    {
                        ImageKey = uType.GetCustomAttribute<SpawnAttribute>().PreviewImage,
                        Tag = uType
                    });
            }
        }

        /// <summary>
        /// Generates a list of preview images
        /// </summary>
        private ImageList GeneratePreviewImages()
        {
            var previewImageList = new ImageList();
            previewImageList.ImageSize = new Size(85, 85);
            foreach (var sType in SpriteTypes)
            {
                var imgPath = sType.GetCustomAttribute<SpawnAttribute>().PreviewImage;
                if (!previewImageList.Images.ContainsKey(imgPath))
                {
                    var imgTexture = GameScene.Game.Content.Load<Texture2D>(imgPath);

                    var memStream = new MemoryStream();
                    imgTexture.SaveAsPng(memStream, imgTexture.Width, imgTexture.Height);
                    var img = Image.FromStream(memStream, true);
                    memStream.Dispose();

                    previewImageList.Images.Add(imgPath, img);
                }
            }

            return previewImageList;
        }

        private void lV_Objects_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lV_sender = (ListView)sender;

            if (lV_sender.SelectedItems.Count <= 0) return;

            var lV_item = lV_sender.SelectedItems[0];

            if (lV_item.Tag is Type)
            {
                var ItemType = (Type)lV_item.Tag;
                var Constructors = ItemType.GetConstructors();
                var ConstParameters = Constructors[0].GetParameters();
                var Parameters = new object[ConstParameters.Length];

                var ConstructorObjects = new Dictionary<Type, Object> {
                    { typeof(HornWarII), this.GameScene.Game },
                    { typeof(GameScene), this.GameScene },
                    { typeof(GameObjects.PhysicEngine), this.GameScene.Game.Components.FirstOrDefault( x => x is GameObjects.PhysicEngine ) },
                    { typeof(GameObjects.GameCam), this.GameScene.Game.Components.FirstOrDefault( x => x is GameObjects.GameCam ) },
                    { typeof(GameObjects.ParticleSystem.ParticleEngine), this.GameScene.Game.Components.FirstOrDefault( x => x is GameObjects.ParticleSystem.ParticleEngine ) },
                };

                for (int i = 0; i < ConstParameters.Length; i++)
                {
                    var cParam = ConstParameters[i];
                    object ParamObject = null;
                    if (ConstructorObjects.ContainsKey(cParam.ParameterType))
                        ParamObject = ConstructorObjects[cParam.ParameterType];

                    if (cParam.Name.ToUpper() == "POSITION")
                        ParamObject = new Vector2(0);

                    if (ParamObject == null)
                    {
                        var popup = new ValueSelect(
                        String.Format("{0} - {1}", ItemType.Name, cParam.Name),
                        cParam.ParameterType);

                        if (!popup.IsDisposed && popup.ShowDialog() == DialogResult.OK)
                            ParamObject = popup.Selected;
                    }

                    if (ParamObject != null)
                        Parameters[i] = ParamObject;
                    else
                    {
                        MessageBox.Show("Object could not be spawned", "Unknown object");
                        return;
                    }
                }

                Constructors[0].Invoke(Parameters);
            }
        }
    }
}
