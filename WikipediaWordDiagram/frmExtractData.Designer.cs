
namespace WikipediaWordDiagram
{
    partial class frmExtractData
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
            this.txtURL = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExtractData = new System.Windows.Forms.Button();
            this.checkBoxSpider = new System.Windows.Forms.CheckBox();
            this.checkBoxWordFreq = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeTitleWord = new System.Windows.Forms.CheckBox();
            this.checkBoxHeader = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(19, 29);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(374, 20);
            this.txtURL.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.txtURL);
            this.groupBox1.Location = new System.Drawing.Point(18, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 68);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "URL";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(403, 27);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExtractData
            // 
            this.btnExtractData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtractData.Location = new System.Drawing.Point(209, 126);
            this.btnExtractData.Name = "btnExtractData";
            this.btnExtractData.Size = new System.Drawing.Size(101, 45);
            this.btnExtractData.TabIndex = 2;
            this.btnExtractData.Text = "Extract Data";
            this.btnExtractData.UseVisualStyleBackColor = true;
            this.btnExtractData.Click += new System.EventHandler(this.btnExtractData_Click);
            // 
            // checkBoxSpider
            // 
            this.checkBoxSpider.AutoSize = true;
            this.checkBoxSpider.Location = new System.Drawing.Point(18, 109);
            this.checkBoxSpider.Name = "checkBoxSpider";
            this.checkBoxSpider.Size = new System.Drawing.Size(98, 17);
            this.checkBoxSpider.TabIndex = 3;
            this.checkBoxSpider.Text = "Spider Diagram";
            this.checkBoxSpider.UseVisualStyleBackColor = true;
            this.checkBoxSpider.CheckedChanged += new System.EventHandler(this.checkBoxSpider_CheckedChanged);
            // 
            // checkBoxWordFreq
            // 
            this.checkBoxWordFreq.AutoSize = true;
            this.checkBoxWordFreq.Checked = true;
            this.checkBoxWordFreq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWordFreq.Location = new System.Drawing.Point(18, 141);
            this.checkBoxWordFreq.Name = "checkBoxWordFreq";
            this.checkBoxWordFreq.Size = new System.Drawing.Size(147, 17);
            this.checkBoxWordFreq.TabIndex = 4;
            this.checkBoxWordFreq.Text = "Word Frequency Diagram";
            this.checkBoxWordFreq.UseVisualStyleBackColor = true;
            this.checkBoxWordFreq.CheckedChanged += new System.EventHandler(this.checkBoxWordFreq_CheckedChanged);
            // 
            // checkBoxIncludeTitleWord
            // 
            this.checkBoxIncludeTitleWord.AutoSize = true;
            this.checkBoxIncludeTitleWord.Checked = true;
            this.checkBoxIncludeTitleWord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIncludeTitleWord.Location = new System.Drawing.Point(37, 164);
            this.checkBoxIncludeTitleWord.Name = "checkBoxIncludeTitleWord";
            this.checkBoxIncludeTitleWord.Size = new System.Drawing.Size(124, 17);
            this.checkBoxIncludeTitleWord.TabIndex = 5;
            this.checkBoxIncludeTitleWord.Text = "Include Title Word(s)";
            this.checkBoxIncludeTitleWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxHeader
            // 
            this.checkBoxHeader.AutoSize = true;
            this.checkBoxHeader.Location = new System.Drawing.Point(37, 184);
            this.checkBoxHeader.Name = "checkBoxHeader";
            this.checkBoxHeader.Size = new System.Drawing.Size(99, 17);
            this.checkBoxHeader.TabIndex = 6;
            this.checkBoxHeader.Text = "Include Header";
            this.checkBoxHeader.UseVisualStyleBackColor = true;
            // 
            // frmExtractData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 217);
            this.Controls.Add(this.checkBoxHeader);
            this.Controls.Add(this.checkBoxIncludeTitleWord);
            this.Controls.Add(this.checkBoxWordFreq);
            this.Controls.Add(this.checkBoxSpider);
            this.Controls.Add(this.btnExtractData);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmExtractData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extract Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExtractData;
        private System.Windows.Forms.CheckBox checkBoxSpider;
        private System.Windows.Forms.CheckBox checkBoxWordFreq;
        private System.Windows.Forms.CheckBox checkBoxIncludeTitleWord;
        private System.Windows.Forms.CheckBox checkBoxHeader;
    }
}