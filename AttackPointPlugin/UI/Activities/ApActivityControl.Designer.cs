namespace GK.SportTracks.AttackPoint.UI.Activities
{
    partial class ApActivityControl
    {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblClimb = new System.Windows.Forms.Label();
            this.lblCourseName = new System.Windows.Forms.Label();
            this.comboTechnicalIntensity = new System.Windows.Forms.ComboBox();
            this.lblTechnicalIntensity = new System.Windows.Forms.Label();
            this.lblTotalControls = new System.Windows.Forms.Label();
            this.lblSpikedControls = new System.Windows.Forms.Label();
            this.tbSpiked = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbTotal = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbDistance = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbClimb = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.lblKm = new System.Windows.Forms.Label();
            this.lblMeters = new System.Windows.Forms.Label();
            this.comboWorkout = new System.Windows.Forms.ComboBox();
            this.lblWorkoutType = new System.Windows.Forms.Label();
            this.lblOrienteering = new System.Windows.Forms.Label();
            this.lblWorkout = new System.Windows.Forms.Label();
            this.tbPrivateNote = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.lblPrivateNote = new System.Windows.Forms.Label();
            this.pIntensity = new System.Windows.Forms.Panel();
            this.bClear = new ZoneFiveSoftware.Common.Visuals.Button();
            this.bCalculateIntensity = new ZoneFiveSoftware.Common.Visuals.Button();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.lblTotalCaption = new System.Windows.Forms.Label();
            this.lTimePerIntensity = new System.Windows.Forms.Label();
            this.lI5 = new System.Windows.Forms.Label();
            this.lI4 = new System.Windows.Forms.Label();
            this.lI3 = new System.Windows.Forms.Label();
            this.lI1 = new System.Windows.Forms.Label();
            this.lI2 = new System.Windows.Forms.Label();
            this.lI0 = new System.Windows.Forms.Label();
            this.tbI5 = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbI4 = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbI3 = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbI2 = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbI1 = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.tbI0 = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.comboBoxCourseName = new System.Windows.Forms.ComboBox();
            this.lblTipSubtype = new System.Windows.Forms.Label();
            this.tbSubtype = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.lblActivitySubtype = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboGpsTrackVisibility = new System.Windows.Forms.ComboBox();
            this.pIntensity.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClimb
            // 
            this.lblClimb.AutoSize = true;
            this.lblClimb.Location = new System.Drawing.Point(2, 202);
            this.lblClimb.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClimb.Name = "lblClimb";
            this.lblClimb.Size = new System.Drawing.Size(76, 13);
            this.lblClimb.TabIndex = 9;
            this.lblClimb.Text = "Course Specs:";
            // 
            // lblCourseName
            // 
            this.lblCourseName.AutoSize = true;
            this.lblCourseName.Location = new System.Drawing.Point(2, 159);
            this.lblCourseName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCourseName.Name = "lblCourseName";
            this.lblCourseName.Size = new System.Drawing.Size(74, 13);
            this.lblCourseName.TabIndex = 8;
            this.lblCourseName.Text = "Course Name:";
            // 
            // comboTechnicalIntensity
            // 
            this.comboTechnicalIntensity.BackColor = System.Drawing.SystemColors.Window;
            this.comboTechnicalIntensity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTechnicalIntensity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboTechnicalIntensity.FormattingEnabled = true;
            this.comboTechnicalIntensity.Location = new System.Drawing.Point(100, 176);
            this.comboTechnicalIntensity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboTechnicalIntensity.Name = "comboTechnicalIntensity";
            this.comboTechnicalIntensity.Size = new System.Drawing.Size(109, 21);
            this.comboTechnicalIntensity.TabIndex = 15;
            this.comboTechnicalIntensity.Validated += new System.EventHandler(this.ControlEdited);
            // 
            // lblTechnicalIntensity
            // 
            this.lblTechnicalIntensity.AutoSize = true;
            this.lblTechnicalIntensity.Location = new System.Drawing.Point(2, 179);
            this.lblTechnicalIntensity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTechnicalIntensity.Name = "lblTechnicalIntensity";
            this.lblTechnicalIntensity.Size = new System.Drawing.Size(98, 13);
            this.lblTechnicalIntensity.TabIndex = 4;
            this.lblTechnicalIntensity.Text = "Technical intensity:";
            // 
            // lblTotalControls
            // 
            this.lblTotalControls.AutoSize = true;
            this.lblTotalControls.Location = new System.Drawing.Point(127, 224);
            this.lblTotalControls.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalControls.Name = "lblTotalControls";
            this.lblTotalControls.Size = new System.Drawing.Size(34, 13);
            this.lblTotalControls.TabIndex = 1;
            this.lblTotalControls.Text = "out of";
            // 
            // lblSpikedControls
            // 
            this.lblSpikedControls.AutoSize = true;
            this.lblSpikedControls.Location = new System.Drawing.Point(2, 222);
            this.lblSpikedControls.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSpikedControls.Name = "lblSpikedControls";
            this.lblSpikedControls.Size = new System.Drawing.Size(83, 13);
            this.lblSpikedControls.TabIndex = 0;
            this.lblSpikedControls.Text = "Spiked controls:";
            // 
            // tbSpiked
            // 
            this.tbSpiked.AcceptsReturn = false;
            this.tbSpiked.AcceptsTab = false;
            this.tbSpiked.BackColor = System.Drawing.Color.White;
            this.tbSpiked.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbSpiked.ButtonImage = null;
            this.tbSpiked.Location = new System.Drawing.Point(100, 222);
            this.tbSpiked.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSpiked.MaxLength = 32767;
            this.tbSpiked.Multiline = false;
            this.tbSpiked.Name = "tbSpiked";
            this.tbSpiked.ReadOnly = false;
            this.tbSpiked.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbSpiked.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbSpiked.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbSpiked.Size = new System.Drawing.Size(26, 15);
            this.tbSpiked.TabIndex = 18;
            this.tbSpiked.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbSpiked.Validated += new System.EventHandler(this.ControlEdited);
            this.tbSpiked.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingNumberInteger);
            // 
            // tbTotal
            // 
            this.tbTotal.AcceptsReturn = false;
            this.tbTotal.AcceptsTab = false;
            this.tbTotal.BackColor = System.Drawing.Color.White;
            this.tbTotal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbTotal.ButtonImage = null;
            this.tbTotal.Location = new System.Drawing.Point(161, 222);
            this.tbTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbTotal.MaxLength = 32767;
            this.tbTotal.Multiline = false;
            this.tbTotal.Name = "tbTotal";
            this.tbTotal.ReadOnly = false;
            this.tbTotal.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbTotal.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbTotal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbTotal.Size = new System.Drawing.Size(26, 15);
            this.tbTotal.TabIndex = 19;
            this.tbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbTotal.Validated += new System.EventHandler(this.ControlEdited);
            this.tbTotal.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingNumberInteger);
            // 
            // tbDistance
            // 
            this.tbDistance.AcceptsReturn = false;
            this.tbDistance.AcceptsTab = false;
            this.tbDistance.BackColor = System.Drawing.Color.White;
            this.tbDistance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbDistance.ButtonImage = null;
            this.tbDistance.Location = new System.Drawing.Point(100, 202);
            this.tbDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDistance.MaxLength = 32767;
            this.tbDistance.Multiline = false;
            this.tbDistance.Name = "tbDistance";
            this.tbDistance.ReadOnly = false;
            this.tbDistance.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbDistance.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbDistance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbDistance.Size = new System.Drawing.Size(36, 15);
            this.tbDistance.TabIndex = 16;
            this.tbDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbDistance.Validated += new System.EventHandler(this.ControlEdited);
            this.tbDistance.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingNumberDouble);
            // 
            // tbClimb
            // 
            this.tbClimb.AcceptsReturn = false;
            this.tbClimb.AcceptsTab = false;
            this.tbClimb.BackColor = System.Drawing.Color.White;
            this.tbClimb.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbClimb.ButtonImage = null;
            this.tbClimb.Location = new System.Drawing.Point(161, 202);
            this.tbClimb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbClimb.MaxLength = 32767;
            this.tbClimb.Multiline = false;
            this.tbClimb.Name = "tbClimb";
            this.tbClimb.ReadOnly = false;
            this.tbClimb.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbClimb.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbClimb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbClimb.Size = new System.Drawing.Size(35, 15);
            this.tbClimb.TabIndex = 17;
            this.tbClimb.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbClimb.Validated += new System.EventHandler(this.ControlEdited);
            this.tbClimb.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingNumberDouble);
            // 
            // lblKm
            // 
            this.lblKm.AutoSize = true;
            this.lblKm.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblKm.Location = new System.Drawing.Point(136, 204);
            this.lblKm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblKm.Name = "lblKm";
            this.lblKm.Size = new System.Drawing.Size(21, 13);
            this.lblKm.TabIndex = 14;
            this.lblKm.Text = "km";
            // 
            // lblMeters
            // 
            this.lblMeters.AutoSize = true;
            this.lblMeters.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMeters.Location = new System.Drawing.Point(196, 204);
            this.lblMeters.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMeters.Name = "lblMeters";
            this.lblMeters.Size = new System.Drawing.Size(15, 13);
            this.lblMeters.TabIndex = 15;
            this.lblMeters.Text = "m";
            // 
            // comboWorkout
            // 
            this.comboWorkout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWorkout.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboWorkout.FormattingEnabled = true;
            this.comboWorkout.Location = new System.Drawing.Point(100, 20);
            this.comboWorkout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboWorkout.Name = "comboWorkout";
            this.comboWorkout.Size = new System.Drawing.Size(109, 21);
            this.comboWorkout.TabIndex = 0;
            this.comboWorkout.Validated += new System.EventHandler(this.ControlEdited);
            // 
            // lblWorkoutType
            // 
            this.lblWorkoutType.AutoSize = true;
            this.lblWorkoutType.Location = new System.Drawing.Point(2, 23);
            this.lblWorkoutType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWorkoutType.Name = "lblWorkoutType";
            this.lblWorkoutType.Size = new System.Drawing.Size(34, 13);
            this.lblWorkoutType.TabIndex = 7;
            this.lblWorkoutType.Text = "Type:";
            // 
            // lblOrienteering
            // 
            this.lblOrienteering.AutoSize = true;
            this.lblOrienteering.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOrienteering.Location = new System.Drawing.Point(2, 139);
            this.lblOrienteering.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOrienteering.Name = "lblOrienteering";
            this.lblOrienteering.Size = new System.Drawing.Size(76, 13);
            this.lblOrienteering.TabIndex = 16;
            this.lblOrienteering.Text = "Orienteering";
            // 
            // lblWorkout
            // 
            this.lblWorkout.AutoSize = true;
            this.lblWorkout.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWorkout.Location = new System.Drawing.Point(2, 4);
            this.lblWorkout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWorkout.Name = "lblWorkout";
            this.lblWorkout.Size = new System.Drawing.Size(55, 13);
            this.lblWorkout.TabIndex = 24;
            this.lblWorkout.Text = "Workout";
            // 
            // tbPrivateNote
            // 
            this.tbPrivateNote.AcceptsReturn = false;
            this.tbPrivateNote.AcceptsTab = false;
            this.tbPrivateNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrivateNote.BackColor = System.Drawing.Color.White;
            this.tbPrivateNote.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbPrivateNote.ButtonImage = null;
            this.tbPrivateNote.Location = new System.Drawing.Point(2, 294);
            this.tbPrivateNote.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPrivateNote.MaxLength = 32767;
            this.tbPrivateNote.Multiline = true;
            this.tbPrivateNote.Name = "tbPrivateNote";
            this.tbPrivateNote.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tbPrivateNote.ReadOnly = false;
            this.tbPrivateNote.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbPrivateNote.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbPrivateNote.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbPrivateNote.Size = new System.Drawing.Size(266, 120);
            this.tbPrivateNote.TabIndex = 20;
            this.tbPrivateNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbPrivateNote.Validated += new System.EventHandler(this.ControlEdited);
            // 
            // lblPrivateNote
            // 
            this.lblPrivateNote.AutoSize = true;
            this.lblPrivateNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrivateNote.Location = new System.Drawing.Point(2, 277);
            this.lblPrivateNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPrivateNote.Name = "lblPrivateNote";
            this.lblPrivateNote.Size = new System.Drawing.Size(82, 13);
            this.lblPrivateNote.TabIndex = 22;
            this.lblPrivateNote.Text = "Private Note:";
            // 
            // pIntensity
            // 
            this.pIntensity.Controls.Add(this.bClear);
            this.pIntensity.Controls.Add(this.bCalculateIntensity);
            this.pIntensity.Controls.Add(this.lblTotalTime);
            this.pIntensity.Controls.Add(this.lblTotalCaption);
            this.pIntensity.Controls.Add(this.lTimePerIntensity);
            this.pIntensity.Controls.Add(this.lI5);
            this.pIntensity.Controls.Add(this.lI4);
            this.pIntensity.Controls.Add(this.lI3);
            this.pIntensity.Controls.Add(this.lI1);
            this.pIntensity.Controls.Add(this.lI2);
            this.pIntensity.Controls.Add(this.lI0);
            this.pIntensity.Controls.Add(this.tbI5);
            this.pIntensity.Controls.Add(this.tbI4);
            this.pIntensity.Controls.Add(this.tbI3);
            this.pIntensity.Controls.Add(this.tbI2);
            this.pIntensity.Controls.Add(this.tbI1);
            this.pIntensity.Controls.Add(this.tbI0);
            this.pIntensity.Location = new System.Drawing.Point(2, 74);
            this.pIntensity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pIntensity.Name = "pIntensity";
            this.pIntensity.Size = new System.Drawing.Size(266, 65);
            this.pIntensity.TabIndex = 2;
            // 
            // bClear
            // 
            this.bClear.BackColor = System.Drawing.Color.Transparent;
            this.bClear.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.bClear.CenterImage = null;
            this.bClear.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bClear.HyperlinkStyle = false;
            this.bClear.ImageMargin = 2;
            this.bClear.LeftImage = null;
            this.bClear.Location = new System.Drawing.Point(218, 3);
            this.bClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bClear.Name = "bClear";
            this.bClear.PushStyle = true;
            this.bClear.RightImage = null;
            this.bClear.Size = new System.Drawing.Size(40, 20);
            this.bClear.TabIndex = 3;
            this.bClear.Text = "Clear";
            this.bClear.TextAlign = System.Drawing.StringAlignment.Center;
            this.bClear.TextLeftMargin = 2;
            this.bClear.TextRightMargin = 2;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // bCalculateIntensity
            // 
            this.bCalculateIntensity.BackColor = System.Drawing.Color.Transparent;
            this.bCalculateIntensity.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.bCalculateIntensity.CenterImage = null;
            this.bCalculateIntensity.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bCalculateIntensity.HyperlinkStyle = false;
            this.bCalculateIntensity.ImageMargin = 2;
            this.bCalculateIntensity.LeftImage = null;
            this.bCalculateIntensity.Location = new System.Drawing.Point(166, 3);
            this.bCalculateIntensity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCalculateIntensity.Name = "bCalculateIntensity";
            this.bCalculateIntensity.PushStyle = true;
            this.bCalculateIntensity.RightImage = null;
            this.bCalculateIntensity.Size = new System.Drawing.Size(46, 20);
            this.bCalculateIntensity.TabIndex = 2;
            this.bCalculateIntensity.Text = "Recalc";
            this.bCalculateIntensity.TextAlign = System.Drawing.StringAlignment.Center;
            this.bCalculateIntensity.TextLeftMargin = 2;
            this.bCalculateIntensity.TextRightMargin = 2;
            this.bCalculateIntensity.Click += new System.EventHandler(this.bCalculateIntensity_Click);
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Location = new System.Drawing.Point(206, 51);
            this.lblTotalTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(27, 13);
            this.lblTotalTime.TabIndex = 36;
            this.lblTotalTime.Text = "N/A";
            // 
            // lblTotalCaption
            // 
            this.lblTotalCaption.AutoSize = true;
            this.lblTotalCaption.Location = new System.Drawing.Point(172, 51);
            this.lblTotalCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalCaption.Name = "lblTotalCaption";
            this.lblTotalCaption.Size = new System.Drawing.Size(37, 13);
            this.lblTotalCaption.TabIndex = 35;
            this.lblTotalCaption.Text = "Total: ";
            // 
            // lTimePerIntensity
            // 
            this.lTimePerIntensity.AutoSize = true;
            this.lTimePerIntensity.Location = new System.Drawing.Point(0, 6);
            this.lTimePerIntensity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lTimePerIntensity.Name = "lTimePerIntensity";
            this.lTimePerIntensity.Size = new System.Drawing.Size(92, 13);
            this.lTimePerIntensity.TabIndex = 34;
            this.lTimePerIntensity.Text = "Time per intensity:";
            // 
            // lI5
            // 
            this.lI5.AutoSize = true;
            this.lI5.Location = new System.Drawing.Point(228, 38);
            this.lI5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lI5.Name = "lI5";
            this.lI5.Size = new System.Drawing.Size(13, 13);
            this.lI5.TabIndex = 33;
            this.lI5.Text = "5";
            // 
            // lI4
            // 
            this.lI4.AutoSize = true;
            this.lI4.Location = new System.Drawing.Point(188, 38);
            this.lI4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lI4.Name = "lI4";
            this.lI4.Size = new System.Drawing.Size(13, 13);
            this.lI4.TabIndex = 32;
            this.lI4.Text = "4";
            // 
            // lI3
            // 
            this.lI3.AutoSize = true;
            this.lI3.Location = new System.Drawing.Point(146, 38);
            this.lI3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lI3.Name = "lI3";
            this.lI3.Size = new System.Drawing.Size(13, 13);
            this.lI3.TabIndex = 31;
            this.lI3.Text = "3";
            // 
            // lI1
            // 
            this.lI1.AutoSize = true;
            this.lI1.Location = new System.Drawing.Point(63, 38);
            this.lI1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lI1.Name = "lI1";
            this.lI1.Size = new System.Drawing.Size(13, 13);
            this.lI1.TabIndex = 30;
            this.lI1.Text = "1";
            // 
            // lI2
            // 
            this.lI2.AutoSize = true;
            this.lI2.Location = new System.Drawing.Point(104, 38);
            this.lI2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lI2.Name = "lI2";
            this.lI2.Size = new System.Drawing.Size(13, 13);
            this.lI2.TabIndex = 29;
            this.lI2.Text = "2";
            // 
            // lI0
            // 
            this.lI0.AutoSize = true;
            this.lI0.Location = new System.Drawing.Point(21, 38);
            this.lI0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lI0.Name = "lI0";
            this.lI0.Size = new System.Drawing.Size(13, 13);
            this.lI0.TabIndex = 28;
            this.lI0.Text = "0";
            // 
            // tbI5
            // 
            this.tbI5.AcceptsReturn = false;
            this.tbI5.AcceptsTab = false;
            this.tbI5.BackColor = System.Drawing.Color.White;
            this.tbI5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbI5.ButtonImage = null;
            this.tbI5.Location = new System.Drawing.Point(214, 23);
            this.tbI5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbI5.MaxLength = 32767;
            this.tbI5.Multiline = false;
            this.tbI5.Name = "tbI5";
            this.tbI5.ReadOnly = false;
            this.tbI5.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbI5.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbI5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbI5.Size = new System.Drawing.Size(43, 15);
            this.tbI5.TabIndex = 10;
            this.tbI5.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbI5.Leave += new System.EventHandler(this.ValidateTimePerIntensity);
            // 
            // tbI4
            // 
            this.tbI4.AcceptsReturn = false;
            this.tbI4.AcceptsTab = false;
            this.tbI4.BackColor = System.Drawing.Color.White;
            this.tbI4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbI4.ButtonImage = null;
            this.tbI4.Location = new System.Drawing.Point(173, 23);
            this.tbI4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbI4.MaxLength = 32767;
            this.tbI4.Multiline = false;
            this.tbI4.Name = "tbI4";
            this.tbI4.ReadOnly = false;
            this.tbI4.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbI4.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbI4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbI4.Size = new System.Drawing.Size(43, 15);
            this.tbI4.TabIndex = 9;
            this.tbI4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbI4.Leave += new System.EventHandler(this.ValidateTimePerIntensity);
            // 
            // tbI3
            // 
            this.tbI3.AcceptsReturn = false;
            this.tbI3.AcceptsTab = false;
            this.tbI3.BackColor = System.Drawing.Color.White;
            this.tbI3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbI3.ButtonImage = null;
            this.tbI3.Location = new System.Drawing.Point(132, 23);
            this.tbI3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbI3.MaxLength = 32767;
            this.tbI3.Multiline = false;
            this.tbI3.Name = "tbI3";
            this.tbI3.ReadOnly = false;
            this.tbI3.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbI3.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbI3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbI3.Size = new System.Drawing.Size(43, 15);
            this.tbI3.TabIndex = 8;
            this.tbI3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbI3.Leave += new System.EventHandler(this.ValidateTimePerIntensity);
            // 
            // tbI2
            // 
            this.tbI2.AcceptsReturn = false;
            this.tbI2.AcceptsTab = false;
            this.tbI2.BackColor = System.Drawing.Color.White;
            this.tbI2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbI2.ButtonImage = null;
            this.tbI2.Location = new System.Drawing.Point(91, 23);
            this.tbI2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbI2.MaxLength = 32767;
            this.tbI2.Multiline = false;
            this.tbI2.Name = "tbI2";
            this.tbI2.ReadOnly = false;
            this.tbI2.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbI2.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbI2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbI2.Size = new System.Drawing.Size(43, 15);
            this.tbI2.TabIndex = 7;
            this.tbI2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbI2.Leave += new System.EventHandler(this.ValidateTimePerIntensity);
            // 
            // tbI1
            // 
            this.tbI1.AcceptsReturn = false;
            this.tbI1.AcceptsTab = false;
            this.tbI1.BackColor = System.Drawing.Color.White;
            this.tbI1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbI1.ButtonImage = null;
            this.tbI1.Location = new System.Drawing.Point(49, 23);
            this.tbI1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbI1.MaxLength = 32767;
            this.tbI1.Multiline = false;
            this.tbI1.Name = "tbI1";
            this.tbI1.ReadOnly = false;
            this.tbI1.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbI1.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbI1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbI1.Size = new System.Drawing.Size(43, 15);
            this.tbI1.TabIndex = 6;
            this.tbI1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbI1.Leave += new System.EventHandler(this.ValidateTimePerIntensity);
            // 
            // tbI0
            // 
            this.tbI0.AcceptsReturn = false;
            this.tbI0.AcceptsTab = false;
            this.tbI0.BackColor = System.Drawing.Color.White;
            this.tbI0.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbI0.ButtonImage = null;
            this.tbI0.Location = new System.Drawing.Point(7, 23);
            this.tbI0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbI0.MaxLength = 32767;
            this.tbI0.Multiline = false;
            this.tbI0.Name = "tbI0";
            this.tbI0.ReadOnly = false;
            this.tbI0.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbI0.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbI0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbI0.Size = new System.Drawing.Size(43, 15);
            this.tbI0.TabIndex = 5;
            this.tbI0.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbI0.Leave += new System.EventHandler(this.ValidateTimePerIntensity);
            // 
            // comboBoxCourseName
            // 
            this.comboBoxCourseName.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxCourseName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxCourseName.FormattingEnabled = true;
            this.comboBoxCourseName.Items.AddRange(new object[] {
            "Long",
            "Middle",
            "Sprint",
            "Relay",
            "Ultra long",
            "Score-O",
            "Rogaine",
            "Blue",
            "Red",
            "Green",
            "Brown",
            "Orange",
            "Yellow",
            "White"});
            this.comboBoxCourseName.Location = new System.Drawing.Point(100, 151);
            this.comboBoxCourseName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxCourseName.MaxLength = 100;
            this.comboBoxCourseName.Name = "comboBoxCourseName";
            this.comboBoxCourseName.Size = new System.Drawing.Size(109, 21);
            this.comboBoxCourseName.TabIndex = 14;
            this.comboBoxCourseName.Validated += new System.EventHandler(this.ControlEdited);
            // 
            // lblTipSubtype
            // 
            this.lblTipSubtype.AutoSize = true;
            this.lblTipSubtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTipSubtype.Location = new System.Drawing.Point(97, 62);
            this.lblTipSubtype.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTipSubtype.Name = "lblTipSubtype";
            this.lblTipSubtype.Size = new System.Drawing.Size(134, 12);
            this.lblTipSubtype.TabIndex = 19;
            this.lblTipSubtype.Text = "Override mapped subtype here.";
            // 
            // tbSubtype
            // 
            this.tbSubtype.AcceptsReturn = false;
            this.tbSubtype.AcceptsTab = false;
            this.tbSubtype.BackColor = System.Drawing.Color.White;
            this.tbSubtype.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbSubtype.ButtonImage = null;
            this.tbSubtype.Location = new System.Drawing.Point(100, 46);
            this.tbSubtype.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSubtype.MaxLength = 25;
            this.tbSubtype.Multiline = false;
            this.tbSubtype.Name = "tbSubtype";
            this.tbSubtype.ReadOnly = false;
            this.tbSubtype.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbSubtype.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbSubtype.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbSubtype.Size = new System.Drawing.Size(108, 15);
            this.tbSubtype.TabIndex = 1;
            this.tbSubtype.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbSubtype.Validated += new System.EventHandler(this.ControlEdited);
            this.tbSubtype.Leave += new System.EventHandler(this.ControlEdited);
            // 
            // lblActivitySubtype
            // 
            this.lblActivitySubtype.AutoSize = true;
            this.lblActivitySubtype.Location = new System.Drawing.Point(2, 46);
            this.lblActivitySubtype.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblActivitySubtype.Name = "lblActivitySubtype";
            this.lblActivitySubtype.Size = new System.Drawing.Size(89, 13);
            this.lblActivitySubtype.TabIndex = 17;
            this.lblActivitySubtype.Text = "Activity Sub-type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(2, 253);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "GPS track visible to:";
            // 
            // comboGpsTrackVisibility
            // 
            this.comboGpsTrackVisibility.BackColor = System.Drawing.SystemColors.Window;
            this.comboGpsTrackVisibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboGpsTrackVisibility.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboGpsTrackVisibility.FormattingEnabled = true;
            this.comboGpsTrackVisibility.Items.AddRange(new object[] {
            "Long",
            "Middle",
            "Sprint",
            "Relay",
            "Ultra long",
            "Score-O",
            "Rogaine",
            "Blue",
            "Red",
            "Green",
            "Brown",
            "Orange",
            "Yellow",
            "White"});
            this.comboGpsTrackVisibility.Location = new System.Drawing.Point(130, 247);
            this.comboGpsTrackVisibility.Margin = new System.Windows.Forms.Padding(2);
            this.comboGpsTrackVisibility.MaxLength = 100;
            this.comboGpsTrackVisibility.Name = "comboGpsTrackVisibility";
            this.comboGpsTrackVisibility.Size = new System.Drawing.Size(129, 21);
            this.comboGpsTrackVisibility.TabIndex = 26;
            // 
            // ApActivityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboGpsTrackVisibility);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSubtype);
            this.Controls.Add(this.tbPrivateNote);
            this.Controls.Add(this.comboBoxCourseName);
            this.Controls.Add(this.lblPrivateNote);
            this.Controls.Add(this.lblTotalControls);
            this.Controls.Add(this.tbDistance);
            this.Controls.Add(this.lblWorkout);
            this.Controls.Add(this.tbTotal);
            this.Controls.Add(this.lblActivitySubtype);
            this.Controls.Add(this.lblCourseName);
            this.Controls.Add(this.pIntensity);
            this.Controls.Add(this.tbClimb);
            this.Controls.Add(this.lblWorkoutType);
            this.Controls.Add(this.tbSpiked);
            this.Controls.Add(this.lblTipSubtype);
            this.Controls.Add(this.comboTechnicalIntensity);
            this.Controls.Add(this.comboWorkout);
            this.Controls.Add(this.lblMeters);
            this.Controls.Add(this.lblClimb);
            this.Controls.Add(this.lblKm);
            this.Controls.Add(this.lblTechnicalIntensity);
            this.Controls.Add(this.lblSpikedControls);
            this.Controls.Add(this.lblOrienteering);
            this.MinimumSize = new System.Drawing.Size(272, 354);
            this.Name = "ApActivityControl";
            this.Size = new System.Drawing.Size(272, 418);
            this.pIntensity.ResumeLayout(false);
            this.pIntensity.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTotalControls;
        private System.Windows.Forms.Label lblSpikedControls;
        private System.Windows.Forms.Label lblTechnicalIntensity;
        private System.Windows.Forms.Label lblClimb;
        private System.Windows.Forms.Label lblCourseName;
        private System.Windows.Forms.ComboBox comboTechnicalIntensity;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbTotal;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbSpiked;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbDistance;
        private System.Windows.Forms.Label lblMeters;
        private System.Windows.Forms.Label lblKm;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbClimb;
        private System.Windows.Forms.ComboBox comboWorkout;
        private System.Windows.Forms.Label lblWorkoutType;
        private System.Windows.Forms.Label lblOrienteering;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbSubtype;
        private System.Windows.Forms.Label lblActivitySubtype;
        private System.Windows.Forms.Label lblTipSubtype;
        private System.Windows.Forms.Panel pIntensity;
        private System.Windows.Forms.Label lI0;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbI5;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbI4;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbI3;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbI2;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbI1;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbI0;
        private System.Windows.Forms.Label lTimePerIntensity;
        private System.Windows.Forms.Label lI5;
        private System.Windows.Forms.Label lI4;
        private System.Windows.Forms.Label lI3;
        private System.Windows.Forms.Label lI1;
        private System.Windows.Forms.Label lI2;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Label lblPrivateNote;
        private System.Windows.Forms.Label lblWorkout;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbPrivateNote;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.ComboBox comboBoxCourseName;
        private ZoneFiveSoftware.Common.Visuals.Button bCalculateIntensity;
        private ZoneFiveSoftware.Common.Visuals.Button bClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboGpsTrackVisibility;
    }
}
