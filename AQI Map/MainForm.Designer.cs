using AQIRestService;
using System;
using System.Windows.Forms;

namespace AQI_Map
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }

            controller.Dispose();
            gmap.Dispose();
            logger.Stop();

            base.Dispose( disposing );
            //
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonFind = new System.Windows.Forms.Button();
            this.buttonFit = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelMsg = new System.Windows.Forms.Panel();
            this.labelMsg = new System.Windows.Forms.Label();
            this.comboBoxMapType = new System.Windows.Forms.ComboBox();
            this.comboBoxParticleType = new System.Windows.Forms.ComboBox();
            this.labelAverageAqi = new System.Windows.Forms.Label();
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.checkBoxFilter = new System.Windows.Forms.CheckBox();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.panelMain.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMsg.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelMain.Controls.Add(this.gmap);
            this.panelMain.Location = new System.Drawing.Point(4, 35);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1174, 787);
            this.panelMain.TabIndex = 0;
            // 
            // gmap
            // 
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemory = 5;
            this.gmap.Location = new System.Drawing.Point(0, 0);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 20;
            this.gmap.MinZoom = 2;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(1174, 787);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 14D;
            this.gmap.OnTileLoadComplete += new GMap.NET.TileLoadComplete(this.gmap_OnTileLoadComplete);
            this.gmap.OnMapDrag += new GMap.NET.MapDrag(this.gmap_OnMapDrag);
            this.gmap.DoubleClick += new System.EventHandler(this.gmap_DoubleClick);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.AutoSize = true;
            this.panelButtons.Controls.Add(this.buttonFind);
            this.panelButtons.Controls.Add(this.buttonFit);
            this.panelButtons.Controls.Add(this.buttonClose);
            this.panelButtons.Location = new System.Drawing.Point(938, 828);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(243, 29);
            this.panelButtons.TabIndex = 1;
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(3, 3);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 0;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_ClickAsync);
            // 
            // buttonFit
            // 
            this.buttonFit.Location = new System.Drawing.Point(84, 3);
            this.buttonFit.Name = "buttonFit";
            this.buttonFit.Size = new System.Drawing.Size(75, 23);
            this.buttonFit.TabIndex = 0;
            this.buttonFit.Text = "Fit";
            this.buttonFit.UseVisualStyleBackColor = true;
            this.buttonFit.Click += new System.EventHandler(this.buttonFit_ClickAsync);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(165, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panelMsg
            // 
            this.panelMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMsg.Controls.Add(this.labelMsg);
            this.panelMsg.Location = new System.Drawing.Point(4, 828);
            this.panelMsg.Name = "panelMsg";
            this.panelMsg.Size = new System.Drawing.Size(928, 29);
            this.panelMsg.TabIndex = 2;
            // 
            // labelMsg
            // 
            this.labelMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMsg.Location = new System.Drawing.Point(8, 3);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(913, 21);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxMapType
            // 
            this.comboBoxMapType.FormattingEnabled = true;
            this.comboBoxMapType.Location = new System.Drawing.Point(219, 3);
            this.comboBoxMapType.Name = "comboBoxMapType";
            this.comboBoxMapType.Size = new System.Drawing.Size(157, 23);
            this.comboBoxMapType.TabIndex = 1;
            this.comboBoxMapType.SelectedIndexChanged += new System.EventHandler(this.comboBoxMapType_SelectedIndexChanged);
            // 
            // comboBoxParticleType
            // 
            this.comboBoxParticleType.FormattingEnabled = true;
            this.comboBoxParticleType.Location = new System.Drawing.Point(382, 3);
            this.comboBoxParticleType.Name = "comboBoxParticleType";
            this.comboBoxParticleType.Size = new System.Drawing.Size(146, 23);
            this.comboBoxParticleType.TabIndex = 1;
            this.comboBoxParticleType.SelectedIndexChanged += new System.EventHandler(this.comboBoxParticleType_SelectedIndexChanged);
            // 
            // labelAverageAqi
            // 
            this.labelAverageAqi.Location = new System.Drawing.Point(772, 4);
            this.labelAverageAqi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.labelAverageAqi.Name = "labelAverageAqi";
            this.labelAverageAqi.Size = new System.Drawing.Size(54, 19);
            this.labelAverageAqi.TabIndex = 3;
            this.labelAverageAqi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Location = new System.Drawing.Point(534, 3);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(121, 23);
            this.comboBoxTime.TabIndex = 4;
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.Controls.Add(this.textBoxLocation);
            this.panelControls.Controls.Add(this.comboBoxMapType);
            this.panelControls.Controls.Add(this.comboBoxParticleType);
            this.panelControls.Controls.Add(this.comboBoxTime);
            this.panelControls.Controls.Add(this.comboBoxType);
            this.panelControls.Controls.Add(this.labelAverageAqi);
            this.panelControls.Controls.Add(this.checkBoxFilter);
            this.panelControls.Location = new System.Drawing.Point(4, 1);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1177, 28);
            this.panelControls.TabIndex = 5;
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(661, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(105, 23);
            this.comboBoxType.TabIndex = 5;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // checkBoxFilter
            // 
            this.checkBoxFilter.AutoSize = true;
            this.checkBoxFilter.Location = new System.Drawing.Point(832, 3);
            this.checkBoxFilter.Name = "checkBoxFilter";
            this.checkBoxFilter.Size = new System.Drawing.Size(52, 19);
            this.checkBoxFilter.TabIndex = 6;
            this.checkBoxFilter.Text = "Filter";
            this.checkBoxFilter.UseVisualStyleBackColor = true;
            this.checkBoxFilter.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Location = new System.Drawing.Point(3, 3);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(210, 23);
            this.textBoxLocation.TabIndex = 7;
            this.textBoxLocation.TextChanged += new System.EventHandler(this.textBoxLocation_TextChanged);
            textBoxLocation.KeyUp += TextBoxLocation_KeyUp;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 861);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.panelMsg);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelMain);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelMain.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelMsg.ResumeLayout(false);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonFind;
        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.Button buttonFit;
        private System.Windows.Forms.Panel panelMsg;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.ComboBox comboBoxMapType;
        private System.Windows.Forms.ComboBox comboBoxParticleType;
        private System.Windows.Forms.Label labelAverageAqi;
        private System.Windows.Forms.ComboBox comboBoxTime;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ComboBox comboBoxType;
        private CheckBox checkBoxFilter;
        private TextBox textBoxLocation;
    }
}