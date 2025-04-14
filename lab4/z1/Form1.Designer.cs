﻿using OpenTK.GLControl;


namespace z1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        
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
            glControl1 = new GLControl();
            SuspendLayout();
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new Version(3, 3);
            glControl1.Dock = DockStyle.Fill;
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.IsEventDriven = true;
            glControl1.Location = new Point(0, 0);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl1.SharedContext = null;
            glControl1.Size = new Size(882, 853);
            glControl1.TabIndex = 0;
            glControl1.Load += GlControlLoad;
            glControl1.Paint += GlControlPaint;
            glControl1.Resize += GlControlResize;
            glControl1.KeyDown += Form1_KeyDown;
            glControl1.KeyUp += Form1_KeyUp;
   
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 853);
            Controls.Add(glControl1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion
        private OpenTK.GLControl.GLControl glControl1;
    }
}
