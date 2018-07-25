namespace FaceDetect
{
    partial class FaceDetect
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
            this.picShow = new System.Windows.Forms.PictureBox();
            this.sampleBox = new System.Windows.Forms.PictureBox();
            this.recognizerType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleBox)).BeginInit();
            this.SuspendLayout();
            // 
            // picShow
            // 
            this.picShow.Location = new System.Drawing.Point(46, 39);
            this.picShow.Name = "picShow";
            this.picShow.Size = new System.Drawing.Size(347, 316);
            this.picShow.TabIndex = 0;
            this.picShow.TabStop = false;
            // 
            // sampleBox
            // 
            this.sampleBox.Location = new System.Drawing.Point(422, 39);
            this.sampleBox.Name = "sampleBox";
            this.sampleBox.Size = new System.Drawing.Size(260, 192);
            this.sampleBox.TabIndex = 1;
            this.sampleBox.TabStop = false;
            // 
            // recognizerType
            // 
            this.recognizerType.DropDownHeight = 120;
            this.recognizerType.FormattingEnabled = true;
            this.recognizerType.IntegralHeight = false;
            this.recognizerType.ItemHeight = 21;
            this.recognizerType.Location = new System.Drawing.Point(422, 252);
            this.recognizerType.Name = "recognizerType";
            this.recognizerType.Size = new System.Drawing.Size(255, 29);
            this.recognizerType.TabIndex = 2;
            // 
            // FaceDetect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 450);
            this.Controls.Add(this.recognizerType);
            this.Controls.Add(this.sampleBox);
            this.Controls.Add(this.picShow);
            this.Name = "FaceDetect";
            this.Text = "FaceDetecting";
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picShow;
        private System.Windows.Forms.PictureBox sampleBox;
        private System.Windows.Forms.ComboBox recognizerType;
    }
}

