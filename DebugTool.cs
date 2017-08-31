using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Horn_War_II
{
    partial class DebugTool : Form
    {
        private HornWarII _game;
        public DebugTool(HornWarII Game)
        {
            this._game = Game;
            InitializeComponent();
            RefreshComponents();

            if (Game.SceneManager.ActiveScene.GetType() == typeof(Scenes.GameScene))
                pG_GameScene.SelectedObject = (Scenes.GameScene)Game.SceneManager.ActiveScene;
        }

        private void DebugTool_Load(object sender, EventArgs e)
        {

        }

        private void blToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshComponents();
        }

        public void RefreshComponents()
        {
            lV_Components.Items.Clear();
            foreach(var comp in _game.Components)
            {
                var item = new ListViewItem(comp.ToString());
                item.Tag = comp;
                lV_Components.Items.Add(item);
            }
        }

        private void lV_Components_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            List<object> Objects = new List<object>();
            foreach(var selectedItem in ((ListView)sender).SelectedItems)
                Objects.Add(((ListViewItem)selectedItem).Tag);
            pG_Component.SelectedObjects = Objects.ToArray();
        }

        private GameObjects.Player GetPlayer()
        {
            foreach (var component in _game.Components)
                if (typeof(GameObjects.Player) == component.GetType())
                    return ((GameObjects.Player)component);

            MessageBox.Show("No player found");
            return null;
        }

        private void axeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var player = GetPlayer();
            if(player != null)
            {
                if (player.Weapon != null)
                    player.Weapon.Dispose();

                new GameObjects.Weapons.Axe(player.GameScene, player.PhysicEngine).Attach(player);
            }
        }

        private void swordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var player = GetPlayer();
            if (player != null)
            {
                if (player.Weapon != null)
                    player.Weapon.Dispose();

                new GameObjects.Weapons.Sword(player.GameScene, player.PhysicEngine).Attach(player);
            }
        }

        private void hornToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var player = GetPlayer();
            if (player != null)
            {
                if (player.Weapon != null)
                    player.Weapon.Dispose();

                new GameObjects.Weapons.Horn(player.GameScene, player.PhysicEngine).Attach(player);
            }
        }

        private void flailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var player = GetPlayer();
            if (player != null)
            {
                if (player.Weapon != null)
                    player.Weapon.Dispose();

                new GameObjects.Weapons.Flail(player.GameScene, player.PhysicEngine).Attach(player);
            }
        }

        private void staffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var player = GetPlayer();
            if (player != null)
            {
                if (player.Weapon != null)
                    player.Weapon.Dispose();

                new GameObjects.Weapons.Staff(player.GameScene, player.PhysicEngine).Attach(player);
            }
        }
    }
}
