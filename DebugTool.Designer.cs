namespace Horn_War_II
{
    partial class DebugTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.blToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchWeaponToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hornToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lV_Components = new System.Windows.Forms.ListView();
            this.pG_Component = new System.Windows.Forms.PropertyGrid();
            this.tP_Game = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.pG_GameScene = new System.Windows.Forms.PropertyGrid();
            this.staffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tP_Game.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 448);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(863, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blToolStripMenuItem,
            this.playerToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(863, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // blToolStripMenuItem
            // 
            this.blToolStripMenuItem.Name = "blToolStripMenuItem";
            this.blToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.blToolStripMenuItem.Text = "Refresh";
            this.blToolStripMenuItem.Click += new System.EventHandler(this.blToolStripMenuItem_Click);
            // 
            // playerToolStripMenuItem
            // 
            this.playerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchWeaponToolStripMenuItem});
            this.playerToolStripMenuItem.Name = "playerToolStripMenuItem";
            this.playerToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.playerToolStripMenuItem.Text = "Player";
            // 
            // switchWeaponToolStripMenuItem
            // 
            this.switchWeaponToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.axeToolStripMenuItem,
            this.swordToolStripMenuItem,
            this.hornToolStripMenuItem,
            this.flailToolStripMenuItem,
            this.staffToolStripMenuItem});
            this.switchWeaponToolStripMenuItem.Name = "switchWeaponToolStripMenuItem";
            this.switchWeaponToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.switchWeaponToolStripMenuItem.Text = "Switch Weapon";
            // 
            // axeToolStripMenuItem
            // 
            this.axeToolStripMenuItem.Name = "axeToolStripMenuItem";
            this.axeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.axeToolStripMenuItem.Text = "Axe";
            this.axeToolStripMenuItem.Click += new System.EventHandler(this.axeToolStripMenuItem_Click);
            // 
            // swordToolStripMenuItem
            // 
            this.swordToolStripMenuItem.Name = "swordToolStripMenuItem";
            this.swordToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.swordToolStripMenuItem.Text = "Sword";
            this.swordToolStripMenuItem.Click += new System.EventHandler(this.swordToolStripMenuItem_Click);
            // 
            // hornToolStripMenuItem
            // 
            this.hornToolStripMenuItem.Name = "hornToolStripMenuItem";
            this.hornToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hornToolStripMenuItem.Text = "Horn";
            this.hornToolStripMenuItem.Click += new System.EventHandler(this.hornToolStripMenuItem_Click);
            // 
            // flailToolStripMenuItem
            // 
            this.flailToolStripMenuItem.Name = "flailToolStripMenuItem";
            this.flailToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.flailToolStripMenuItem.Text = "Flail";
            this.flailToolStripMenuItem.Click += new System.EventHandler(this.flailToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tP_Game);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(863, 424);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(855, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Components";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lV_Components);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pG_Component);
            this.splitContainer1.Size = new System.Drawing.Size(849, 392);
            this.splitContainer1.SplitterDistance = 282;
            this.splitContainer1.TabIndex = 0;
            // 
            // lV_Components
            // 
            this.lV_Components.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lV_Components.Location = new System.Drawing.Point(0, 0);
            this.lV_Components.Name = "lV_Components";
            this.lV_Components.Size = new System.Drawing.Size(282, 392);
            this.lV_Components.TabIndex = 0;
            this.lV_Components.UseCompatibleStateImageBehavior = false;
            this.lV_Components.View = System.Windows.Forms.View.List;
            this.lV_Components.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lV_Components_ItemSelectionChanged);
            // 
            // pG_Component
            // 
            this.pG_Component.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pG_Component.Location = new System.Drawing.Point(0, 0);
            this.pG_Component.Name = "pG_Component";
            this.pG_Component.Size = new System.Drawing.Size(563, 392);
            this.pG_Component.TabIndex = 0;
            // 
            // tP_Game
            // 
            this.tP_Game.Controls.Add(this.splitContainer2);
            this.tP_Game.Location = new System.Drawing.Point(4, 22);
            this.tP_Game.Name = "tP_Game";
            this.tP_Game.Padding = new System.Windows.Forms.Padding(3);
            this.tP_Game.Size = new System.Drawing.Size(855, 398);
            this.tP_Game.TabIndex = 1;
            this.tP_Game.Text = "Game";
            this.tP_Game.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(849, 392);
            this.splitContainer2.SplitterDistance = 283;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.pG_GameScene);
            this.splitContainer4.Size = new System.Drawing.Size(562, 392);
            this.splitContainer4.SplitterDistance = 186;
            this.splitContainer4.TabIndex = 0;
            // 
            // pG_GameScene
            // 
            this.pG_GameScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pG_GameScene.Location = new System.Drawing.Point(0, 0);
            this.pG_GameScene.Name = "pG_GameScene";
            this.pG_GameScene.Size = new System.Drawing.Size(562, 186);
            this.pG_GameScene.TabIndex = 0;
            // 
            // staffToolStripMenuItem
            // 
            this.staffToolStripMenuItem.Name = "staffToolStripMenuItem";
            this.staffToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.staffToolStripMenuItem.Text = "Staff";
            this.staffToolStripMenuItem.Click += new System.EventHandler(this.staffToolStripMenuItem_Click);
            // 
            // DebugTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 470);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DebugTool";
            this.Text = "DebugTool";
            this.Load += new System.EventHandler(this.DebugTool_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tP_Game.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem blToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lV_Components;
        private System.Windows.Forms.PropertyGrid pG_Component;
        private System.Windows.Forms.TabPage tP_Game;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.PropertyGrid pG_GameScene;
        private System.Windows.Forms.ToolStripMenuItem playerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchWeaponToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hornToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem staffToolStripMenuItem;
    }
}