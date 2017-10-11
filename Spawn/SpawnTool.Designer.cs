namespace Horn_War_II.Spawn
{
    partial class SpawnTool
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
            this.lV_Objects = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lV_Objects
            // 
            this.lV_Objects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lV_Objects.Location = new System.Drawing.Point(0, 0);
            this.lV_Objects.Name = "lV_Objects";
            this.lV_Objects.Size = new System.Drawing.Size(540, 537);
            this.lV_Objects.TabIndex = 0;
            this.lV_Objects.UseCompatibleStateImageBehavior = false;
            this.lV_Objects.View = System.Windows.Forms.View.Tile;
            this.lV_Objects.SelectedIndexChanged += new System.EventHandler(this.lV_Objects_SelectedIndexChanged);
            // 
            // EditorTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 537);
            this.Controls.Add(this.lV_Objects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EditorTools";
            this.Text = "Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lV_Objects;
    }
}