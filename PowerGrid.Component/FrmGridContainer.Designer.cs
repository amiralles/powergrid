namespace PowerGrid.Component {
    partial class FrmGridContainer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsPaging = new System.Windows.Forms.ToolStrip();
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.lblShowing = new System.Windows.Forms.ToolStripLabel();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            this.grid = new PowerGrid.Component.PowerGridControl();
            this.tsPaging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // tsPaging
            // 
            this.tsPaging.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tsPaging.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnPrev,
            this.lblShowing,
            this.btnNext,
            this.btnLast});
            this.tsPaging.Location = new System.Drawing.Point(0, 394);
            this.tsPaging.Name = "tsPaging";
            this.tsPaging.Size = new System.Drawing.Size(812, 25);
            this.tsPaging.TabIndex = 1;
            this.tsPaging.Text = "toolStrip1";
            // 
            // btnFirst
            // 
            this.btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirst.Image = global::PowerGrid.Component.Properties.Resources.arrow_left_double;
            this.btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(23, 22);
            this.btnFirst.Text = "First";
            this.btnFirst.ToolTipText = "Goto First";
            // 
            // btnPrev
            // 
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrev.Image = global::PowerGrid.Component.Properties.Resources.arrow_left;
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 22);
            this.btnPrev.Text = "Previous";
            this.btnPrev.ToolTipText = "Goto previous page";
            // 
            // lblShowing
            // 
            this.lblShowing.Name = "lblShowing";
            this.lblShowing.Size = new System.Drawing.Size(64, 22);
            this.lblShowing.Text = "showing....";
            // 
            // btnNext
            // 
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNext.Image = global::PowerGrid.Component.Properties.Resources.arrow_right;
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 22);
            this.btnNext.Text = "Next";
            this.btnNext.ToolTipText = "Goto next page";
            // 
            // btnLast
            // 
            this.btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLast.Image = global::PowerGrid.Component.Properties.Resources.arrow_right_double;
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(23, 22);
            this.btnLast.Text = "Last";
            this.btnLast.ToolTipText = "Goto last page";
            // 
            // powerGridControl1
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.CleanUp = null;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.CurrentPage = 0;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid.DefaultCellStyle = dataGridViewCellStyle1;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EndConfigure = null;
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grid.Location = new System.Drawing.Point(0, 25);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.OnGotoFirst = null;
            this.grid.OnGotoLast = null;
            this.grid.OnGotoNext = null;
            this.grid.OnGotoPrev = null;
            this.grid.OnPageChange = null;
            this.grid.PageSize = 0;
            this.grid.ReadOnly = true;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(937, 434);
            this.grid.TabIndex = 2;
            this.grid.TotalPages = null;
            // 
            // FrmGridContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 419);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.tsPaging);
            this.Name = "FrmGridContainer";
            this.tsPaging.ResumeLayout(false);
            this.tsPaging.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsPaging;
        private System.Windows.Forms.ToolStripButton btnFirst;
        private System.Windows.Forms.ToolStripButton btnPrev;
        private System.Windows.Forms.ToolStripLabel lblShowing;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnLast;
        private PowerGridControl grid;
    }
}