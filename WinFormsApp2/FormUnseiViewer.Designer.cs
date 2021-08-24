
namespace WinFormsApp2
{
    partial class FormUnseiViewer
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
            this.grdViewNenUn = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewNenUn)).BeginInit();
            this.SuspendLayout();
            // 
            // grdViewNenUn
            // 
            this.grdViewNenUn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdViewNenUn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdViewNenUn.Location = new System.Drawing.Point(12, 12);
            this.grdViewNenUn.Name = "grdViewNenUn";
            this.grdViewNenUn.RowTemplate.Height = 25;
            this.grdViewNenUn.Size = new System.Drawing.Size(776, 426);
            this.grdViewNenUn.TabIndex = 0;
            // 
            // FormUnseiViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grdViewNenUn);
            this.Name = "FormUnseiViewer";
            this.Text = "FormUnseiViewer";
            this.Load += new System.EventHandler(this.FormUnseiViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdViewNenUn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdViewNenUn;
    }
}