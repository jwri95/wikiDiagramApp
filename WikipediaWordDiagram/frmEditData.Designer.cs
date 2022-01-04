
namespace WikipediaWordDiagram
{
    partial class frmEditData
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtDesc1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtDesc2 = new System.Windows.Forms.TextBox();
            this.listView2 = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtDesc3 = new System.Windows.Forms.TextBox();
            this.listView3 = new System.Windows.Forms.ListView();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(14, 21);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(257, 257);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 568);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtDesc1);
            this.groupBox4.Location = new System.Drawing.Point(14, 297);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(257, 257);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Description";
            // 
            // txtDesc1
            // 
            this.txtDesc1.BackColor = System.Drawing.SystemColors.Control;
            this.txtDesc1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDesc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc1.Location = new System.Drawing.Point(3, 16);
            this.txtDesc1.Multiline = true;
            this.txtDesc1.Name = "txtDesc1";
            this.txtDesc1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDesc1.Size = new System.Drawing.Size(251, 238);
            this.txtDesc1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.listView2);
            this.groupBox2.Location = new System.Drawing.Point(313, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 568);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtDesc2);
            this.groupBox5.Location = new System.Drawing.Point(14, 297);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(257, 257);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Description";
            // 
            // txtDesc2
            // 
            this.txtDesc2.BackColor = System.Drawing.SystemColors.Control;
            this.txtDesc2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDesc2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc2.Location = new System.Drawing.Point(3, 16);
            this.txtDesc2.Multiline = true;
            this.txtDesc2.Name = "txtDesc2";
            this.txtDesc2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDesc2.Size = new System.Drawing.Size(251, 238);
            this.txtDesc2.TabIndex = 1;
            // 
            // listView2
            // 
            this.listView2.CheckBoxes = true;
            this.listView2.FullRowSelect = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(14, 21);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(257, 257);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.List;
            this.listView2.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked);
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.listView3);
            this.groupBox3.Location = new System.Drawing.Point(614, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(284, 568);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtDesc3);
            this.groupBox6.Location = new System.Drawing.Point(14, 297);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(257, 257);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Description";
            // 
            // txtDesc3
            // 
            this.txtDesc3.BackColor = System.Drawing.SystemColors.Control;
            this.txtDesc3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDesc3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc3.Location = new System.Drawing.Point(3, 16);
            this.txtDesc3.Multiline = true;
            this.txtDesc3.Name = "txtDesc3";
            this.txtDesc3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDesc3.Size = new System.Drawing.Size(251, 238);
            this.txtDesc3.TabIndex = 1;
            // 
            // listView3
            // 
            this.listView3.CheckBoxes = true;
            this.listView3.FullRowSelect = true;
            this.listView3.HideSelection = false;
            this.listView3.Location = new System.Drawing.Point(14, 21);
            this.listView3.MultiSelect = false;
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(257, 257);
            this.listView3.TabIndex = 0;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.List;
            this.listView3.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
            this.listView3.SelectedIndexChanged += new System.EventHandler(this.listView3_SelectedIndexChanged);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(401, 600);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 36);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Generate Diagram";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // frmEditData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 648);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmEditData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtDesc1;
        private System.Windows.Forms.TextBox txtDesc2;
        private System.Windows.Forms.TextBox txtDesc3;
    }
}