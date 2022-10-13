using System.Windows.Forms;

namespace Comparatively
{
    partial class TipsForm : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipsForm));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.ShowTips = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(468, 201);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // ShowTips
            // 
            this.ShowTips.AutoSize = true;
            this.ShowTips.Checked = true;
            this.ShowTips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowTips.Location = new System.Drawing.Point(12, 222);
            this.ShowTips.Name = "ShowTips";
            this.ShowTips.Size = new System.Drawing.Size(130, 19);
            this.ShowTips.TabIndex = 1;
            this.ShowTips.Text = "Show tips at startup";
            this.ShowTips.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(405, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TipsForm
            // 
            this.ClientSize = new System.Drawing.Size(492, 253);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ShowTips);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TipsForm";
            this.Text = "Tips";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private RichTextBox richTextBox1;
        private CheckBox ShowTips;
        private Button button1;
    }
}