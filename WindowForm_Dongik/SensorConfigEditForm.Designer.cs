namespace WindowForm_Dongik
{
    partial class SensorConfigEditForm
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
            this.SensorComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SensorNameText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TemperatureGroup = new System.Windows.Forms.GroupBox();
            this.CoreFourCheck = new System.Windows.Forms.CheckBox();
            this.CoreThreeCheck = new System.Windows.Forms.CheckBox();
            this.CoreTwoCheck = new System.Windows.Forms.CheckBox();
            this.CoreOneCheck = new System.Windows.Forms.CheckBox();
            this.TempActiveCheck = new System.Windows.Forms.CheckBox();
            this.CpuGroupBox = new System.Windows.Forms.GroupBox();
            this.CpuCurrentCheck = new System.Windows.Forms.CheckBox();
            this.CpuTotalCheck = new System.Windows.Forms.CheckBox();
            this.CpuCheckBox = new System.Windows.Forms.CheckBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.ModbusGroupBox = new System.Windows.Forms.GroupBox();
            this.SizeTextBox = new System.Windows.Forms.TextBox();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IpTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ModbusActiveCheck = new System.Windows.Forms.CheckBox();
            this.MemoyUsageGroup = new System.Windows.Forms.GroupBox();
            this.MemoryActiveCheck = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.TemperatureGroup.SuspendLayout();
            this.CpuGroupBox.SuspendLayout();
            this.ModbusGroupBox.SuspendLayout();
            this.MemoyUsageGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SensorComboBox
            // 
            this.SensorComboBox.FormattingEnabled = true;
            this.SensorComboBox.Items.AddRange(new object[] {
            "---선택하세요---",
            "temperature",
            "cpu occupied",
            "memory usage",
            "modbus"});
            this.SensorComboBox.Location = new System.Drawing.Point(80, 63);
            this.SensorComboBox.Margin = new System.Windows.Forms.Padding(1);
            this.SensorComboBox.Name = "SensorComboBox";
            this.SensorComboBox.Size = new System.Drawing.Size(134, 20);
            this.SensorComboBox.TabIndex = 0;
            this.SensorComboBox.Text = "---선택하세요---";
            this.SensorComboBox.SelectedIndexChanged += new System.EventHandler(this.SensorComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SensorComboBox);
            this.groupBox1.Controls.Add(this.SensorNameText);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(20, 22);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(283, 98);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor Info";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Type : ";
            // 
            // SensorNameText
            // 
            this.SensorNameText.Location = new System.Drawing.Point(80, 29);
            this.SensorNameText.Margin = new System.Windows.Forms.Padding(1);
            this.SensorNameText.Name = "SensorNameText";
            this.SensorNameText.Size = new System.Drawing.Size(134, 21);
            this.SensorNameText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name : ";
            // 
            // TemperatureGroup
            // 
            this.TemperatureGroup.Controls.Add(this.CoreFourCheck);
            this.TemperatureGroup.Controls.Add(this.CoreThreeCheck);
            this.TemperatureGroup.Controls.Add(this.CoreTwoCheck);
            this.TemperatureGroup.Controls.Add(this.CoreOneCheck);
            this.TemperatureGroup.Controls.Add(this.TempActiveCheck);
            this.TemperatureGroup.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TemperatureGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TemperatureGroup.Location = new System.Drawing.Point(20, 132);
            this.TemperatureGroup.Margin = new System.Windows.Forms.Padding(1);
            this.TemperatureGroup.Name = "TemperatureGroup";
            this.TemperatureGroup.Padding = new System.Windows.Forms.Padding(1);
            this.TemperatureGroup.Size = new System.Drawing.Size(236, 169);
            this.TemperatureGroup.TabIndex = 2;
            this.TemperatureGroup.TabStop = false;
            this.TemperatureGroup.Text = "Temperature Info";
            // 
            // CoreFourCheck
            // 
            this.CoreFourCheck.AutoSize = true;
            this.CoreFourCheck.Location = new System.Drawing.Point(26, 130);
            this.CoreFourCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CoreFourCheck.Name = "CoreFourCheck";
            this.CoreFourCheck.Size = new System.Drawing.Size(72, 16);
            this.CoreFourCheck.TabIndex = 8;
            this.CoreFourCheck.Text = " Core 4";
            this.CoreFourCheck.UseVisualStyleBackColor = true;
            // 
            // CoreThreeCheck
            // 
            this.CoreThreeCheck.AutoSize = true;
            this.CoreThreeCheck.Location = new System.Drawing.Point(26, 106);
            this.CoreThreeCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CoreThreeCheck.Name = "CoreThreeCheck";
            this.CoreThreeCheck.Size = new System.Drawing.Size(72, 16);
            this.CoreThreeCheck.TabIndex = 7;
            this.CoreThreeCheck.Text = " Core 3";
            this.CoreThreeCheck.UseVisualStyleBackColor = true;
            // 
            // CoreTwoCheck
            // 
            this.CoreTwoCheck.AutoSize = true;
            this.CoreTwoCheck.Location = new System.Drawing.Point(26, 82);
            this.CoreTwoCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CoreTwoCheck.Name = "CoreTwoCheck";
            this.CoreTwoCheck.Size = new System.Drawing.Size(72, 16);
            this.CoreTwoCheck.TabIndex = 6;
            this.CoreTwoCheck.Text = " Core 2";
            this.CoreTwoCheck.UseVisualStyleBackColor = true;
            // 
            // CoreOneCheck
            // 
            this.CoreOneCheck.AutoSize = true;
            this.CoreOneCheck.Location = new System.Drawing.Point(26, 57);
            this.CoreOneCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CoreOneCheck.Name = "CoreOneCheck";
            this.CoreOneCheck.Size = new System.Drawing.Size(72, 16);
            this.CoreOneCheck.TabIndex = 5;
            this.CoreOneCheck.Text = " Core 1";
            this.CoreOneCheck.UseVisualStyleBackColor = true;
            // 
            // TempActiveCheck
            // 
            this.TempActiveCheck.AutoSize = true;
            this.TempActiveCheck.Location = new System.Drawing.Point(28, 33);
            this.TempActiveCheck.Margin = new System.Windows.Forms.Padding(1);
            this.TempActiveCheck.Name = "TempActiveCheck";
            this.TempActiveCheck.Size = new System.Drawing.Size(69, 16);
            this.TempActiveCheck.TabIndex = 4;
            this.TempActiveCheck.Text = " Active";
            this.TempActiveCheck.UseVisualStyleBackColor = true;
            // 
            // CpuGroupBox
            // 
            this.CpuGroupBox.Controls.Add(this.CpuCurrentCheck);
            this.CpuGroupBox.Controls.Add(this.CpuTotalCheck);
            this.CpuGroupBox.Controls.Add(this.CpuCheckBox);
            this.CpuGroupBox.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CpuGroupBox.Location = new System.Drawing.Point(20, 132);
            this.CpuGroupBox.Margin = new System.Windows.Forms.Padding(1);
            this.CpuGroupBox.Name = "CpuGroupBox";
            this.CpuGroupBox.Padding = new System.Windows.Forms.Padding(1);
            this.CpuGroupBox.Size = new System.Drawing.Size(236, 169);
            this.CpuGroupBox.TabIndex = 9;
            this.CpuGroupBox.TabStop = false;
            this.CpuGroupBox.Text = "CPU Info";
            // 
            // CpuCurrentCheck
            // 
            this.CpuCurrentCheck.AutoSize = true;
            this.CpuCurrentCheck.Location = new System.Drawing.Point(26, 82);
            this.CpuCurrentCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CpuCurrentCheck.Name = "CpuCurrentCheck";
            this.CpuCurrentCheck.Size = new System.Drawing.Size(225, 16);
            this.CpuCurrentCheck.TabIndex = 2;
            this.CpuCurrentCheck.Text = " Get current process occupied";
            this.CpuCurrentCheck.UseVisualStyleBackColor = true;
            // 
            // CpuTotalCheck
            // 
            this.CpuTotalCheck.AutoSize = true;
            this.CpuTotalCheck.Location = new System.Drawing.Point(26, 57);
            this.CpuTotalCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CpuTotalCheck.Name = "CpuTotalCheck";
            this.CpuTotalCheck.Size = new System.Drawing.Size(207, 16);
            this.CpuTotalCheck.TabIndex = 1;
            this.CpuTotalCheck.Text = " Get total process occupied";
            this.CpuTotalCheck.UseVisualStyleBackColor = true;
            // 
            // CpuCheckBox
            // 
            this.CpuCheckBox.AutoSize = true;
            this.CpuCheckBox.Location = new System.Drawing.Point(26, 32);
            this.CpuCheckBox.Margin = new System.Windows.Forms.Padding(1);
            this.CpuCheckBox.Name = "CpuCheckBox";
            this.CpuCheckBox.Size = new System.Drawing.Size(69, 16);
            this.CpuCheckBox.TabIndex = 0;
            this.CpuCheckBox.Text = " Active";
            this.CpuCheckBox.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CancelBtn.Location = new System.Drawing.Point(245, 309);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(1);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(58, 24);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "취 소";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SaveBtn.Location = new System.Drawing.Point(177, 309);
            this.SaveBtn.Margin = new System.Windows.Forms.Padding(1);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(58, 24);
            this.SaveBtn.TabIndex = 4;
            this.SaveBtn.Text = "저 장";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // ModbusGroupBox
            // 
            this.ModbusGroupBox.Controls.Add(this.SizeTextBox);
            this.ModbusGroupBox.Controls.Add(this.SizeLabel);
            this.ModbusGroupBox.Controls.Add(this.AddressTextBox);
            this.ModbusGroupBox.Controls.Add(this.label5);
            this.ModbusGroupBox.Controls.Add(this.PortTextBox);
            this.ModbusGroupBox.Controls.Add(this.label4);
            this.ModbusGroupBox.Controls.Add(this.IpTextBox);
            this.ModbusGroupBox.Controls.Add(this.label3);
            this.ModbusGroupBox.Controls.Add(this.ModbusActiveCheck);
            this.ModbusGroupBox.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ModbusGroupBox.Location = new System.Drawing.Point(20, 132);
            this.ModbusGroupBox.Margin = new System.Windows.Forms.Padding(1);
            this.ModbusGroupBox.Name = "ModbusGroupBox";
            this.ModbusGroupBox.Padding = new System.Windows.Forms.Padding(1);
            this.ModbusGroupBox.Size = new System.Drawing.Size(236, 169);
            this.ModbusGroupBox.TabIndex = 10;
            this.ModbusGroupBox.TabStop = false;
            this.ModbusGroupBox.Text = "Modbus Info";
            // 
            // SizeTextBox
            // 
            this.SizeTextBox.Location = new System.Drawing.Point(62, 144);
            this.SizeTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.SizeTextBox.Name = "SizeTextBox";
            this.SizeTextBox.Size = new System.Drawing.Size(133, 21);
            this.SizeTextBox.TabIndex = 8;
            this.SizeTextBox.Text = "1";
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Location = new System.Drawing.Point(18, 145);
            this.SizeLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(44, 12);
            this.SizeLabel.TabIndex = 7;
            this.SizeLabel.Text = "Size :";
            // 
            // AddressTextBox
            // 
            this.AddressTextBox.Location = new System.Drawing.Point(62, 115);
            this.AddressTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.Size = new System.Drawing.Size(133, 21);
            this.AddressTextBox.TabIndex = 6;
            this.AddressTextBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 116);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "Addr :";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(62, 86);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(133, 21);
            this.PortTextBox.TabIndex = 4;
            this.PortTextBox.Text = "508";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 87);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Port :";
            // 
            // IpTextBox
            // 
            this.IpTextBox.Location = new System.Drawing.Point(62, 57);
            this.IpTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.IpTextBox.Name = "IpTextBox";
            this.IpTextBox.Size = new System.Drawing.Size(133, 21);
            this.IpTextBox.TabIndex = 2;
            this.IpTextBox.Text = "127.0.0.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "I P :";
            // 
            // ModbusActiveCheck
            // 
            this.ModbusActiveCheck.AutoSize = true;
            this.ModbusActiveCheck.Location = new System.Drawing.Point(26, 32);
            this.ModbusActiveCheck.Margin = new System.Windows.Forms.Padding(1);
            this.ModbusActiveCheck.Name = "ModbusActiveCheck";
            this.ModbusActiveCheck.Size = new System.Drawing.Size(69, 16);
            this.ModbusActiveCheck.TabIndex = 0;
            this.ModbusActiveCheck.Text = " Active";
            this.ModbusActiveCheck.UseVisualStyleBackColor = true;
            // 
            // MemoyUsageGroup
            // 
            this.MemoyUsageGroup.Controls.Add(this.MemoryActiveCheck);
            this.MemoyUsageGroup.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MemoyUsageGroup.Location = new System.Drawing.Point(20, 132);
            this.MemoyUsageGroup.Margin = new System.Windows.Forms.Padding(1);
            this.MemoyUsageGroup.Name = "MemoyUsageGroup";
            this.MemoyUsageGroup.Padding = new System.Windows.Forms.Padding(1);
            this.MemoyUsageGroup.Size = new System.Drawing.Size(236, 169);
            this.MemoyUsageGroup.TabIndex = 11;
            this.MemoyUsageGroup.TabStop = false;
            this.MemoyUsageGroup.Text = "Memory Usage Info";
            // 
            // MemoryActiveCheck
            // 
            this.MemoryActiveCheck.AutoSize = true;
            this.MemoryActiveCheck.Location = new System.Drawing.Point(26, 32);
            this.MemoryActiveCheck.Margin = new System.Windows.Forms.Padding(1);
            this.MemoryActiveCheck.Name = "MemoryActiveCheck";
            this.MemoryActiveCheck.Size = new System.Drawing.Size(69, 16);
            this.MemoryActiveCheck.TabIndex = 0;
            this.MemoryActiveCheck.Text = " Active";
            this.MemoryActiveCheck.UseVisualStyleBackColor = true;
            // 
            // SensorConfigForm_ver3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 348);
            this.Controls.Add(this.ModbusGroupBox);
            this.Controls.Add(this.MemoyUsageGroup);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.TemperatureGroup);
            this.Controls.Add(this.CpuGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "SensorConfigForm_ver3";
            this.Text = "SensorConfigForm_ver3";
            this.Load += new System.EventHandler(this.SensorAddForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TemperatureGroup.ResumeLayout(false);
            this.TemperatureGroup.PerformLayout();
            this.CpuGroupBox.ResumeLayout(false);
            this.CpuGroupBox.PerformLayout();
            this.ModbusGroupBox.ResumeLayout(false);
            this.ModbusGroupBox.PerformLayout();
            this.MemoyUsageGroup.ResumeLayout(false);
            this.MemoyUsageGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox SensorComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SensorNameText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox TemperatureGroup;
        private System.Windows.Forms.GroupBox CpuGroupBox;
        private System.Windows.Forms.CheckBox CpuCurrentCheck;
        private System.Windows.Forms.CheckBox CpuTotalCheck;
        private System.Windows.Forms.CheckBox CpuCheckBox;
        private System.Windows.Forms.CheckBox CoreFourCheck;
        private System.Windows.Forms.CheckBox CoreThreeCheck;
        private System.Windows.Forms.CheckBox CoreTwoCheck;
        private System.Windows.Forms.CheckBox CoreOneCheck;
        private System.Windows.Forms.CheckBox TempActiveCheck;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.GroupBox ModbusGroupBox;
        private System.Windows.Forms.TextBox SizeTextBox;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox IpTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ModbusActiveCheck;
        private System.Windows.Forms.GroupBox MemoyUsageGroup;
        private System.Windows.Forms.CheckBox MemoryActiveCheck;
    }
}