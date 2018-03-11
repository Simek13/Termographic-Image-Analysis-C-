namespace Termographic_image_analysis
{
    partial class Form1
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
            this.imageDisplay = new System.Windows.Forms.PictureBox();
            this.histogramGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.minTrackBar = new System.Windows.Forms.TrackBar();
            this.maxTrackBar = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.coloringCheckBox = new System.Windows.Forms.CheckBox();
            this.resolutionTrackBar = new System.Windows.Forms.TrackBar();
            this.histogramLabel = new System.Windows.Forms.Label();
            this.automaticAdjustmentCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.autofocusButton = new System.Windows.Forms.Button();
            this.fetchButton = new System.Windows.Forms.Button();
            this.saveFastButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.focusNearButton = new System.Windows.Forms.Button();
            this.focusFarButton = new System.Windows.Forms.Button();
            this.imagesContainer = new System.Windows.Forms.SplitContainer();
            this.averageTemp = new System.Windows.Forms.Label();
            this.temperatureLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.inflamationDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legSymetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramDisplay = new Termographic_image_analysis.HistogramDisplay();
            this.previousImagesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.previousImages = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.imageDisplay)).BeginInit();
            this.histogramGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxTrackBar)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionTrackBar)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagesContainer)).BeginInit();
            this.imagesContainer.Panel1.SuspendLayout();
            this.imagesContainer.Panel2.SuspendLayout();
            this.imagesContainer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.previousImagesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageDisplay
            // 
            this.imageDisplay.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imageDisplay.Cursor = System.Windows.Forms.Cursors.Cross;
            this.imageDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageDisplay.Location = new System.Drawing.Point(0, 28);
            this.imageDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.imageDisplay.Name = "imageDisplay";
            this.imageDisplay.Size = new System.Drawing.Size(808, 616);
            this.imageDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageDisplay.TabIndex = 0;
            this.imageDisplay.TabStop = false;
            this.imageDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.imageDisplay_Paint);
            this.imageDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageDisplay_MouseDown);
            this.imageDisplay.MouseLeave += new System.EventHandler(this.imageDisplay_MouseLeave);
            this.imageDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageDisplay_MouseMove);
            this.imageDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageDisplay_MouseUp);
            // 
            // histogramGroupBox
            // 
            this.histogramGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.histogramGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.histogramGroupBox.Location = new System.Drawing.Point(0, 488);
            this.histogramGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.histogramGroupBox.Name = "histogramGroupBox";
            this.histogramGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.histogramGroupBox.Size = new System.Drawing.Size(807, 187);
            this.histogramGroupBox.TabIndex = 1;
            this.histogramGroupBox.TabStop = false;
            this.histogramGroupBox.Text = "Prikaz";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.42424F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.57576F));
            this.tableLayoutPanel1.Controls.Add(this.minTrackBar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.maxTrackBar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 19);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(799, 164);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // minTrackBar
            // 
            this.minTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.minTrackBar.Enabled = false;
            this.minTrackBar.Location = new System.Drawing.Point(4, 4);
            this.minTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.minTrackBar.Maximum = 65535;
            this.minTrackBar.Name = "minTrackBar";
            this.minTrackBar.Size = new System.Drawing.Size(791, 47);
            this.minTrackBar.TabIndex = 0;
            this.minTrackBar.Scroll += new System.EventHandler(this.minTrackBar_Scroll);
            this.minTrackBar.ValueChanged += new System.EventHandler(this.minTrackBar_ValueChanged);
            // 
            // maxTrackBar
            // 
            this.maxTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maxTrackBar.Enabled = false;
            this.maxTrackBar.Location = new System.Drawing.Point(4, 59);
            this.maxTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.maxTrackBar.Maximum = 65535;
            this.maxTrackBar.Name = "maxTrackBar";
            this.maxTrackBar.Size = new System.Drawing.Size(791, 47);
            this.maxTrackBar.TabIndex = 1;
            this.maxTrackBar.Value = 65535;
            this.maxTrackBar.Scroll += new System.EventHandler(this.maxTrackBar_Scroll);
            this.maxTrackBar.ValueChanged += new System.EventHandler(this.maxTrackBar_ValueChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel3.Controls.Add(this.coloringCheckBox, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.resolutionTrackBar, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.histogramLabel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.automaticAdjustmentCheckBox, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 114);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(791, 46);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // coloringCheckBox
            // 
            this.coloringCheckBox.AutoSize = true;
            this.coloringCheckBox.Checked = true;
            this.coloringCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coloringCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coloringCheckBox.Enabled = false;
            this.coloringCheckBox.Location = new System.Drawing.Point(597, 4);
            this.coloringCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.coloringCheckBox.Name = "coloringCheckBox";
            this.coloringCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.coloringCheckBox.Size = new System.Drawing.Size(73, 38);
            this.coloringCheckBox.TabIndex = 4;
            this.coloringCheckBox.Text = "Prikaz boja";
            this.coloringCheckBox.UseVisualStyleBackColor = true;
            this.coloringCheckBox.CheckedChanged += new System.EventHandler(this.coloringCheckBox_CheckedChanged);
            // 
            // resolutionTrackBar
            // 
            this.resolutionTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resolutionTrackBar.Location = new System.Drawing.Point(88, 4);
            this.resolutionTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.resolutionTrackBar.Maximum = 256;
            this.resolutionTrackBar.Minimum = 2;
            this.resolutionTrackBar.Name = "resolutionTrackBar";
            this.resolutionTrackBar.Size = new System.Drawing.Size(501, 38);
            this.resolutionTrackBar.TabIndex = 3;
            this.resolutionTrackBar.Value = 256;
            this.resolutionTrackBar.ValueChanged += new System.EventHandler(this.resolutionTrackBar_ValueChanged);
            // 
            // histogramLabel
            // 
            this.histogramLabel.AutoSize = true;
            this.histogramLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.histogramLabel.Location = new System.Drawing.Point(4, 0);
            this.histogramLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.histogramLabel.Name = "histogramLabel";
            this.histogramLabel.Size = new System.Drawing.Size(76, 46);
            this.histogramLabel.TabIndex = 5;
            this.histogramLabel.Text = "Histogram:";
            // 
            // automaticAdjustmentCheckBox
            // 
            this.automaticAdjustmentCheckBox.AutoSize = true;
            this.automaticAdjustmentCheckBox.Checked = true;
            this.automaticAdjustmentCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.automaticAdjustmentCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.automaticAdjustmentCheckBox.Location = new System.Drawing.Point(678, 4);
            this.automaticAdjustmentCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.automaticAdjustmentCheckBox.Name = "automaticAdjustmentCheckBox";
            this.automaticAdjustmentCheckBox.Size = new System.Drawing.Size(109, 38);
            this.automaticAdjustmentCheckBox.TabIndex = 6;
            this.automaticAdjustmentCheckBox.Text = "Automatska prilagodba";
            this.automaticAdjustmentCheckBox.UseVisualStyleBackColor = true;
            this.automaticAdjustmentCheckBox.CheckedChanged += new System.EventHandler(this.automaticAdjustmentCheckBox_CheckedChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 7;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.Controls.Add(this.autofocusButton, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.fetchButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.saveFastButton, 6, 0);
            this.tableLayoutPanel4.Controls.Add(this.saveButton, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.openButton, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.focusNearButton, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.focusFarButton, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1620, 59);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // autofocusButton
            // 
            this.autofocusButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.autofocusButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autofocusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.autofocusButton.Location = new System.Drawing.Point(235, 4);
            this.autofocusButton.Margin = new System.Windows.Forms.Padding(4);
            this.autofocusButton.Name = "autofocusButton";
            this.autofocusButton.Size = new System.Drawing.Size(223, 51);
            this.autofocusButton.TabIndex = 6;
            this.autofocusButton.Text = "Autofokus";
            this.autofocusButton.UseVisualStyleBackColor = false;
            this.autofocusButton.Click += new System.EventHandler(this.autofocusButton_Click);
            // 
            // fetchButton
            // 
            this.fetchButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.fetchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fetchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fetchButton.Location = new System.Drawing.Point(4, 4);
            this.fetchButton.Margin = new System.Windows.Forms.Padding(4);
            this.fetchButton.Name = "fetchButton";
            this.fetchButton.Size = new System.Drawing.Size(223, 51);
            this.fetchButton.TabIndex = 2;
            this.fetchButton.Text = "Pokreni";
            this.fetchButton.UseVisualStyleBackColor = false;
            this.fetchButton.Click += new System.EventHandler(this.fetchButton_Click);
            // 
            // saveFastButton
            // 
            this.saveFastButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveFastButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveFastButton.Enabled = false;
            this.saveFastButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.saveFastButton.Location = new System.Drawing.Point(1390, 4);
            this.saveFastButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveFastButton.Name = "saveFastButton";
            this.saveFastButton.Size = new System.Drawing.Size(226, 51);
            this.saveFastButton.TabIndex = 1;
            this.saveFastButton.Text = "Spremi analizu";
            this.saveFastButton.UseVisualStyleBackColor = false;
            this.saveFastButton.Click += new System.EventHandler(this.saveFastButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveButton.Enabled = false;
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.saveButton.Location = new System.Drawing.Point(1159, 4);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(223, 51);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Spremi sliku";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // openButton
            // 
            this.openButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.openButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.openButton.Location = new System.Drawing.Point(928, 4);
            this.openButton.Margin = new System.Windows.Forms.Padding(4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(223, 51);
            this.openButton.TabIndex = 3;
            this.openButton.Text = "Učitaj sliku";
            this.openButton.UseVisualStyleBackColor = false;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // focusNearButton
            // 
            this.focusNearButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.focusNearButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.focusNearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.focusNearButton.Location = new System.Drawing.Point(466, 4);
            this.focusNearButton.Margin = new System.Windows.Forms.Padding(4);
            this.focusNearButton.Name = "focusNearButton";
            this.focusNearButton.Size = new System.Drawing.Size(223, 51);
            this.focusNearButton.TabIndex = 4;
            this.focusNearButton.Text = "+";
            this.focusNearButton.UseVisualStyleBackColor = false;
            this.focusNearButton.Click += new System.EventHandler(this.focusNearButton_Click);
            this.focusNearButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.focusNearButton_MouseDown);
            // 
            // focusFarButton
            // 
            this.focusFarButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.focusFarButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.focusFarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.focusFarButton.Location = new System.Drawing.Point(697, 4);
            this.focusFarButton.Margin = new System.Windows.Forms.Padding(4);
            this.focusFarButton.Name = "focusFarButton";
            this.focusFarButton.Size = new System.Drawing.Size(223, 51);
            this.focusFarButton.TabIndex = 5;
            this.focusFarButton.Text = "-";
            this.focusFarButton.UseVisualStyleBackColor = false;
            this.focusFarButton.Click += new System.EventHandler(this.focusFarButton_Click);
            this.focusFarButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.focusFarButton_MouseDown);
            // 
            // imagesContainer
            // 
            this.imagesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagesContainer.Location = new System.Drawing.Point(0, 59);
            this.imagesContainer.Margin = new System.Windows.Forms.Padding(4);
            this.imagesContainer.Name = "imagesContainer";
            // 
            // imagesContainer.Panel1
            // 
            this.imagesContainer.Panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imagesContainer.Panel1.Controls.Add(this.averageTemp);
            this.imagesContainer.Panel1.Controls.Add(this.imageDisplay);
            this.imagesContainer.Panel1.Controls.Add(this.temperatureLabel);
            this.imagesContainer.Panel1.Controls.Add(this.menuStrip1);
            // 
            // imagesContainer.Panel2
            // 
            this.imagesContainer.Panel2.Controls.Add(this.histogramDisplay);
            this.imagesContainer.Panel2.Controls.Add(this.previousImagesPanel);
            this.imagesContainer.Panel2.Controls.Add(this.histogramGroupBox);
            this.imagesContainer.Size = new System.Drawing.Size(1620, 675);
            this.imagesContainer.SplitterDistance = 808;
            this.imagesContainer.SplitterWidth = 5;
            this.imagesContainer.TabIndex = 4;
            // 
            // averageTemp
            // 
            this.averageTemp.AutoSize = true;
            this.averageTemp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.averageTemp.Location = new System.Drawing.Point(462, 4);
            this.averageTemp.Name = "averageTemp";
            this.averageTemp.Size = new System.Drawing.Size(224, 23);
            this.averageTemp.TabIndex = 3;
            this.averageTemp.Text = "Prosječna temperatura tijela";
            this.averageTemp.Visible = false;
            this.averageTemp.MouseLeave += new System.EventHandler(this.averageTemp_MouseLeave);
            this.averageTemp.MouseHover += new System.EventHandler(this.averageTemp_MouseHover);
            // 
            // temperatureLabel
            // 
            this.temperatureLabel.AutoSize = true;
            this.temperatureLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.temperatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.temperatureLabel.Location = new System.Drawing.Point(0, 644);
            this.temperatureLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.temperatureLabel.Name = "temperatureLabel";
            this.temperatureLabel.Size = new System.Drawing.Size(23, 31);
            this.temperatureLabel.TabIndex = 1;
            this.temperatureLabel.Text = "-";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inflamationDetectionToolStripMenuItem,
            this.legSymetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(808, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // inflamationDetectionToolStripMenuItem
            // 
            this.inflamationDetectionToolStripMenuItem.Checked = true;
            this.inflamationDetectionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.inflamationDetectionToolStripMenuItem.Name = "inflamationDetectionToolStripMenuItem";
            this.inflamationDetectionToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            this.inflamationDetectionToolStripMenuItem.Text = "Detekcija upale";
            this.inflamationDetectionToolStripMenuItem.Click += new System.EventHandler(this.inflamationDetectionToolStripMenuItem_Click);
            // 
            // legSymetToolStripMenuItem
            // 
            this.legSymetToolStripMenuItem.Name = "legSymetToolStripMenuItem";
            this.legSymetToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            this.legSymetToolStripMenuItem.Text = "Simetrija";
            this.legSymetToolStripMenuItem.Click += new System.EventHandler(this.legSymetToolStripMenuItem_Click);
            // 
            // histogramDisplay
            // 
            this.histogramDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.histogramDisplay.Location = new System.Drawing.Point(0, 137);
            this.histogramDisplay.Margin = new System.Windows.Forms.Padding(5);
            this.histogramDisplay.MaxLine = 65535;
            this.histogramDisplay.MinLine = 0;
            this.histogramDisplay.Name = "histogramDisplay";
            this.histogramDisplay.Resolution = 500;
            this.histogramDisplay.Size = new System.Drawing.Size(807, 351);
            this.histogramDisplay.TabIndex = 1;
            // 
            // previousImagesPanel
            // 
            this.previousImagesPanel.AutoScroll = true;
            this.previousImagesPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.previousImagesPanel.Controls.Add(this.previousImages);
            this.previousImagesPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.previousImagesPanel.Location = new System.Drawing.Point(0, 0);
            this.previousImagesPanel.Margin = new System.Windows.Forms.Padding(4);
            this.previousImagesPanel.Name = "previousImagesPanel";
            this.previousImagesPanel.Size = new System.Drawing.Size(807, 137);
            this.previousImagesPanel.TabIndex = 2;
            this.previousImagesPanel.WrapContents = false;
            // 
            // previousImages
            // 
            this.previousImages.AutoScroll = true;
            this.previousImages.AutoSize = true;
            this.previousImages.ColumnCount = 1;
            this.previousImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.previousImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previousImages.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.previousImages.Location = new System.Drawing.Point(4, 4);
            this.previousImages.Margin = new System.Windows.Forms.Padding(4);
            this.previousImages.Name = "previousImages";
            this.previousImages.RowCount = 1;
            this.previousImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.previousImages.Size = new System.Drawing.Size(0, 0);
            this.previousImages.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1620, 734);
            this.Controls.Add(this.imagesContainer);
            this.Controls.Add(this.tableLayoutPanel4);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Infracrvene slike";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageDisplay)).EndInit();
            this.histogramGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxTrackBar)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionTrackBar)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.imagesContainer.Panel1.ResumeLayout(false);
            this.imagesContainer.Panel1.PerformLayout();
            this.imagesContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagesContainer)).EndInit();
            this.imagesContainer.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.previousImagesPanel.ResumeLayout(false);
            this.previousImagesPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imageDisplay;
        private System.Windows.Forms.GroupBox histogramGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TrackBar minTrackBar;
        private System.Windows.Forms.TrackBar maxTrackBar;
        private HistogramDisplay histogramDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TrackBar resolutionTrackBar;
        private System.Windows.Forms.CheckBox coloringCheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button saveFastButton;
        private System.Windows.Forms.SplitContainer imagesContainer;
        private System.Windows.Forms.Label histogramLabel;
        private System.Windows.Forms.CheckBox automaticAdjustmentCheckBox;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Label temperatureLabel;
        private System.Windows.Forms.FlowLayoutPanel previousImagesPanel;
        private System.Windows.Forms.TableLayoutPanel previousImages;
        private System.Windows.Forms.Button focusNearButton;
        private System.Windows.Forms.Button focusFarButton;
        private System.Windows.Forms.Button autofocusButton;
        private System.Windows.Forms.Button fetchButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem inflamationDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legSymetToolStripMenuItem;
        private System.Windows.Forms.Label averageTemp;
    }
}

