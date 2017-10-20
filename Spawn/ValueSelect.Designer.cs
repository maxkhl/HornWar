namespace Horn_War_II.Spawn
{
    partial class ValueSelect
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
            this.b_abort = new System.Windows.Forms.Button();
            this.b_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // b_abort
            // 
            this.b_abort.Location = new System.Drawing.Point(12, 41);
            this.b_abort.Name = "b_abort";
            this.b_abort.Size = new System.Drawing.Size(75, 23);
            this.b_abort.TabIndex = 1;
            this.b_abort.Text = "Abort";
            this.b_abort.UseVisualStyleBackColor = true;
            this.b_abort.Click += new System.EventHandler(this.b_abort_Click);
            // 
            // b_ok
            // 
            this.b_ok.Location = new System.Drawing.Point(266, 41);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(75, 23);
            this.b_ok.TabIndex = 2;
            this.b_ok.Text = "OK";
            this.b_ok.UseVisualStyleBackColor = true;
            this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
            // 
            // ValueSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 81);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.b_abort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ValueSelect";
            this.Text = "-";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button b_abort;
        private System.Windows.Forms.Button b_ok;
    }
}