using System;

namespace CutPicture
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ScalingStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.S10StripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.S30StripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.S50StripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.S100StripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.S200StripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.DiaphaneityStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.D10StripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.D30StripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.D50StripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.D100tripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.MoveStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DestroyStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
            this.ToolStripMenuItem1.Text = "分组1";
            this.ToolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyStripMenuItem1,
            this.SaveStripMenuItem1,
            this.ScalingStripMenuItem1,
            this.DiaphaneityStripMenuItem1,
            this.MoveStripMenuItem1,
            this.CloseStripMenuItem1,
            this.DestroyStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(215, 216);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // CopyStripMenuItem1
            // 
            this.CopyStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("CopyStripMenuItem1.Image")));
            this.CopyStripMenuItem1.Name = "CopyStripMenuItem1";
            this.CopyStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.CopyStripMenuItem1.Text = "复制";
            this.CopyStripMenuItem1.Click += new System.EventHandler(this.CopyStripMenuItem1_Click);
            // 
            // SaveStripMenuItem1
            // 
            this.SaveStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("SaveStripMenuItem1.Image")));
            this.SaveStripMenuItem1.Name = "SaveStripMenuItem1";
            this.SaveStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.SaveStripMenuItem1.Text = "另存为";
            this.SaveStripMenuItem1.Click += new System.EventHandler(this.SaveStripMenuItem1_Click);
            // 
            // ScalingStripMenuItem1
            // 
            this.ScalingStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.S10StripMenuItem2,
            this.S30StripMenuItem3,
            this.S50StripMenuItem4,
            this.S100StripMenuItem5,
            this.S200StripMenuItem6});
            this.ScalingStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("ScalingStripMenuItem1.Image")));
            this.ScalingStripMenuItem1.Name = "ScalingStripMenuItem1";
            this.ScalingStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.ScalingStripMenuItem1.Text = "缩放";
            this.ScalingStripMenuItem1.Click += new System.EventHandler(this.ScalingStripMenuItem1_Click);
            // 
            // S10StripMenuItem2
            // 
            this.S10StripMenuItem2.Name = "S10StripMenuItem2";
            this.S10StripMenuItem2.Size = new System.Drawing.Size(132, 26);
            this.S10StripMenuItem2.Text = "10%";
            this.S10StripMenuItem2.Click += new System.EventHandler(this.S10StripMenuItem2_Click);
            // 
            // S30StripMenuItem3
            // 
            this.S30StripMenuItem3.Name = "S30StripMenuItem3";
            this.S30StripMenuItem3.Size = new System.Drawing.Size(132, 26);
            this.S30StripMenuItem3.Text = "30%";
            this.S30StripMenuItem3.Click += new System.EventHandler(this.S30StripMenuItem3_Click);
            // 
            // S50StripMenuItem4
            // 
            this.S50StripMenuItem4.Name = "S50StripMenuItem4";
            this.S50StripMenuItem4.Size = new System.Drawing.Size(132, 26);
            this.S50StripMenuItem4.Text = "50%";
            this.S50StripMenuItem4.Click += new System.EventHandler(this.S50StripMenuItem4_Click);
            // 
            // S100StripMenuItem5
            // 
            this.S100StripMenuItem5.Name = "S100StripMenuItem5";
            this.S100StripMenuItem5.Size = new System.Drawing.Size(132, 26);
            this.S100StripMenuItem5.Text = "100%";
            this.S100StripMenuItem5.Click += new System.EventHandler(this.S100StripMenuItem5_Click);
            // 
            // S200StripMenuItem6
            // 
            this.S200StripMenuItem6.Name = "S200StripMenuItem6";
            this.S200StripMenuItem6.Size = new System.Drawing.Size(132, 26);
            this.S200StripMenuItem6.Text = "200%";
            this.S200StripMenuItem6.Click += new System.EventHandler(this.S200StripMenuItem6_Click);
            // 
            // DiaphaneityStripMenuItem1
            // 
            this.DiaphaneityStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.D10StripMenuItem7,
            this.D30StripMenuItem8,
            this.D50StripMenuItem9,
            this.D100tripMenuItem10});
            this.DiaphaneityStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("DiaphaneityStripMenuItem1.Image")));
            this.DiaphaneityStripMenuItem1.Name = "DiaphaneityStripMenuItem1";
            this.DiaphaneityStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.DiaphaneityStripMenuItem1.Text = "透明度";
            // 
            // D10StripMenuItem7
            // 
            this.D10StripMenuItem7.Name = "D10StripMenuItem7";
            this.D10StripMenuItem7.Size = new System.Drawing.Size(224, 26);
            this.D10StripMenuItem7.Text = "10%";
            this.D10StripMenuItem7.Click += new System.EventHandler(this.D10StripMenuItem7_Click);
            // 
            // D30StripMenuItem8
            // 
            this.D30StripMenuItem8.Name = "D30StripMenuItem8";
            this.D30StripMenuItem8.Size = new System.Drawing.Size(224, 26);
            this.D30StripMenuItem8.Text = "30%";
            this.D30StripMenuItem8.Click += new System.EventHandler(this.D30StripMenuItem8_Click);
            // 
            // D50StripMenuItem9
            // 
            this.D50StripMenuItem9.Name = "D50StripMenuItem9";
            this.D50StripMenuItem9.Size = new System.Drawing.Size(224, 26);
            this.D50StripMenuItem9.Text = "50%";
            this.D50StripMenuItem9.Click += new System.EventHandler(this.D50StripMenuItem9_Click);
            // 
            // D100tripMenuItem10
            // 
            this.D100tripMenuItem10.Name = "D100tripMenuItem10";
            this.D100tripMenuItem10.Size = new System.Drawing.Size(224, 26);
            this.D100tripMenuItem10.Text = "100%";
            this.D100tripMenuItem10.Click += new System.EventHandler(this.D100tripMenuItem10_Click);
            // 
            // MoveStripMenuItem1
            // 
            this.MoveStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2,
            this.ToolStripMenuItem3});
            this.MoveStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("MoveStripMenuItem1.Image")));
            this.MoveStripMenuItem1.Name = "MoveStripMenuItem1";
            this.MoveStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.MoveStripMenuItem1.Text = "移动到分组";
            this.MoveStripMenuItem1.Click += new System.EventHandler(this.MoveStripMenuItem1_Click);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(224, 26);
            this.ToolStripMenuItem2.Text = "分组2";
            this.ToolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(224, 26);
            this.ToolStripMenuItem3.Text = "分组3";
            this.ToolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3_Click);
            // 
            // CloseStripMenuItem1
            // 
            this.CloseStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("CloseStripMenuItem1.Image")));
            this.CloseStripMenuItem1.Name = "CloseStripMenuItem1";
            this.CloseStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.CloseStripMenuItem1.Text = "关闭";
            this.CloseStripMenuItem1.Click += new System.EventHandler(this.CloseStripMenuItem1_Click_1);
            // 
            // DestroyStripMenuItem1
            // 
            this.DestroyStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("DestroyStripMenuItem1.Image")));
            this.DestroyStripMenuItem1.Name = "DestroyStripMenuItem1";
            this.DestroyStripMenuItem1.Size = new System.Drawing.Size(214, 26);
            this.DestroyStripMenuItem1.Text = "销毁";
            this.DestroyStripMenuItem1.Click += new System.EventHandler(this.DestroyStripMenuItem1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 400);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form2";
            this.Closed += new System.EventHandler(this.MyFormClosed);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void CloseStripMenuItem1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CopyStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SaveStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ScalingStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem DiaphaneityStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MoveStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem DestroyStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CloseStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem S10StripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem S30StripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem S50StripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem S100StripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem S200StripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem D10StripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem D30StripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem D50StripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem D100tripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
    }
}