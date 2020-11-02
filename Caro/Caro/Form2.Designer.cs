namespace Caro
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.ptb_Info_2 = new System.Windows.Forms.PictureBox();
            this.ptb_Info_1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Info_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Info_1)).BeginInit();
            this.SuspendLayout();
            // 
            // ptb_Info_2
            // 
            this.ptb_Info_2.BackgroundImage = global::Caro.Properties.Resources.Info_2;
            this.ptb_Info_2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptb_Info_2.Location = new System.Drawing.Point(12, 140);
            this.ptb_Info_2.Name = "ptb_Info_2";
            this.ptb_Info_2.Size = new System.Drawing.Size(275, 70);
            this.ptb_Info_2.TabIndex = 0;
            this.ptb_Info_2.TabStop = false;
            // 
            // ptb_Info_1
            // 
            this.ptb_Info_1.BackgroundImage = global::Caro.Properties.Resources.Info_1;
            this.ptb_Info_1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ptb_Info_1.Location = new System.Drawing.Point(12, 45);
            this.ptb_Info_1.Name = "ptb_Info_1";
            this.ptb_Info_1.Size = new System.Drawing.Size(275, 69);
            this.ptb_Info_1.TabIndex = 0;
            this.ptb_Info_1.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 236);
            this.Controls.Add(this.ptb_Info_2);
            this.Controls.Add(this.ptb_Info_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Member";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Info_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Info_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptb_Info_1;
        private System.Windows.Forms.PictureBox ptb_Info_2;
    }
}