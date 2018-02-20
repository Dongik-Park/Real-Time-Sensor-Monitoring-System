namespace WindowForm_Dongik
{
    partial class SetConfigForm
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
            this.TempGroup = new System.Windows.Forms.GroupBox();
            this.TempActiveCheck = new System.Windows.Forms.CheckBox();
            this.core4 = new System.Windows.Forms.RadioButton();
            this.core3 = new System.Windows.Forms.RadioButton();
            this.core2 = new System.Windows.Forms.RadioButton();
            this.core1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.CPUGroup = new System.Windows.Forms.GroupBox();
            this.CpuActiveCheck = new System.Windows.Forms.CheckBox();
            this.cpu_current = new System.Windows.Forms.RadioButton();
            this.cpu_total = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ModbusGroup = new System.Windows.Forms.GroupBox();
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ModbusActiveCheck = new System.Windows.Forms.CheckBox();
            this.SizeTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ConfigSaveBtn = new System.Windows.Forms.Button();
            this.ConfigCancelBtn = new System.Windows.Forms.Button();
            this.MemoryGroup = new System.Windows.Forms.GroupBox();
            this.MemoryUsageCheck = new System.Windows.Forms.CheckBox();
            this.TempGroup.SuspendLayout();
            this.CPUGroup.SuspendLayout();
            this.ModbusGroup.SuspendLayout();
            this.MemoryGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // TempGroup
            // 
            this.TempGroup.Controls.Add(this.TempActiveCheck);
            this.TempGroup.Controls.Add(this.core4);
            this.TempGroup.Controls.Add(this.core3);
            this.TempGroup.Controls.Add(this.core2);
            this.TempGroup.Controls.Add(this.core1);
            this.TempGroup.Controls.Add(this.label1);
            this.TempGroup.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TempGroup.Location = new System.Drawing.Point(12, 12);
            this.TempGroup.Name = "TempGroup";
            this.TempGroup.Size = new System.Drawing.Size(288, 115);
            this.TempGroup.TabIndex = 0;
            this.TempGroup.TabStop = false;
            this.TempGroup.Text = "Temperature";
            // 
            // TempActiveCheck
            // 
            this.TempActiveCheck.AutoSize = true;
            this.TempActiveCheck.Checked = true;
            this.TempActiveCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TempActiveCheck.Location = new System.Drawing.Point(24, 38);
            this.TempActiveCheck.Name = "TempActiveCheck";
            this.TempActiveCheck.Size = new System.Drawing.Size(64, 16);
            this.TempActiveCheck.TabIndex = 6;
            this.TempActiveCheck.Text = "Active";
            this.TempActiveCheck.UseVisualStyleBackColor = true;
            // 
            // core4
            // 
            this.core4.AutoSize = true;
            this.core4.Location = new System.Drawing.Point(231, 71);
            this.core4.Name = "core4";
            this.core4.Size = new System.Drawing.Size(30, 16);
            this.core4.TabIndex = 4;
            this.core4.Text = "4";
            this.core4.UseVisualStyleBackColor = true;
            // 
            // core3
            // 
            this.core3.AutoSize = true;
            this.core3.Location = new System.Drawing.Point(195, 71);
            this.core3.Name = "core3";
            this.core3.Size = new System.Drawing.Size(30, 16);
            this.core3.TabIndex = 3;
            this.core3.Text = "3";
            this.core3.UseVisualStyleBackColor = true;
            // 
            // core2
            // 
            this.core2.AutoSize = true;
            this.core2.Location = new System.Drawing.Point(159, 71);
            this.core2.Name = "core2";
            this.core2.Size = new System.Drawing.Size(30, 16);
            this.core2.TabIndex = 2;
            this.core2.Text = "2";
            this.core2.UseVisualStyleBackColor = true;
            // 
            // core1
            // 
            this.core1.AutoSize = true;
            this.core1.Checked = true;
            this.core1.Location = new System.Drawing.Point(123, 71);
            this.core1.Name = "core1";
            this.core1.Size = new System.Drawing.Size(30, 16);
            this.core1.TabIndex = 1;
            this.core1.TabStop = true;
            this.core1.Text = "1";
            this.core1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Core Number";
            // 
            // CPUGroup
            // 
            this.CPUGroup.Controls.Add(this.CpuActiveCheck);
            this.CPUGroup.Controls.Add(this.cpu_current);
            this.CPUGroup.Controls.Add(this.cpu_total);
            this.CPUGroup.Controls.Add(this.label2);
            this.CPUGroup.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CPUGroup.Location = new System.Drawing.Point(12, 133);
            this.CPUGroup.Name = "CPUGroup";
            this.CPUGroup.Size = new System.Drawing.Size(288, 115);
            this.CPUGroup.TabIndex = 1;
            this.CPUGroup.TabStop = false;
            this.CPUGroup.Text = "CPU Occupied";
            // 
            // CpuActiveCheck
            // 
            this.CpuActiveCheck.AutoSize = true;
            this.CpuActiveCheck.Checked = true;
            this.CpuActiveCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CpuActiveCheck.Location = new System.Drawing.Point(24, 38);
            this.CpuActiveCheck.Name = "CpuActiveCheck";
            this.CpuActiveCheck.Size = new System.Drawing.Size(64, 16);
            this.CpuActiveCheck.TabIndex = 7;
            this.CpuActiveCheck.Text = "Active";
            this.CpuActiveCheck.UseVisualStyleBackColor = true;
            // 
            // cpu_current
            // 
            this.cpu_current.AutoSize = true;
            this.cpu_current.Location = new System.Drawing.Point(168, 71);
            this.cpu_current.Name = "cpu_current";
            this.cpu_current.Size = new System.Drawing.Size(71, 16);
            this.cpu_current.TabIndex = 6;
            this.cpu_current.Text = "Current";
            this.cpu_current.UseVisualStyleBackColor = true;
            // 
            // cpu_total
            // 
            this.cpu_total.AutoSize = true;
            this.cpu_total.Checked = true;
            this.cpu_total.Location = new System.Drawing.Point(106, 71);
            this.cpu_total.Name = "cpu_total";
            this.cpu_total.Size = new System.Drawing.Size(56, 16);
            this.cpu_total.TabIndex = 5;
            this.cpu_total.TabStop = true;
            this.cpu_total.Text = "Total";
            this.cpu_total.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Process";
            // 
            // ModbusGroup
            // 
            this.ModbusGroup.Controls.Add(this.AddressTextBox);
            this.ModbusGroup.Controls.Add(this.label6);
            this.ModbusGroup.Controls.Add(this.ModbusActiveCheck);
            this.ModbusGroup.Controls.Add(this.SizeTextBox);
            this.ModbusGroup.Controls.Add(this.label5);
            this.ModbusGroup.Controls.Add(this.PortTextBox);
            this.ModbusGroup.Controls.Add(this.label4);
            this.ModbusGroup.Controls.Add(this.IPTextBox);
            this.ModbusGroup.Controls.Add(this.label3);
            this.ModbusGroup.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ModbusGroup.Location = new System.Drawing.Point(12, 340);
            this.ModbusGroup.Name = "ModbusGroup";
            this.ModbusGroup.Size = new System.Drawing.Size(288, 208);
            this.ModbusGroup.TabIndex = 2;
            this.ModbusGroup.TabStop = false;
            this.ModbusGroup.Text = "Modbus";
            this.ModbusGroup.Enter += new System.EventHandler(this.ModbusGroup_Enter);
            // 
            // AddressTextBox
            // 
            this.AddressTextBox.Location = new System.Drawing.Point(92, 128);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.Size = new System.Drawing.Size(172, 21);
            this.AddressTextBox.TabIndex = 14;
            this.AddressTextBox.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "Address : ";
            // 
            // ModbusActiveCheck
            // 
            this.ModbusActiveCheck.AutoSize = true;
            this.ModbusActiveCheck.Checked = true;
            this.ModbusActiveCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ModbusActiveCheck.Location = new System.Drawing.Point(24, 36);
            this.ModbusActiveCheck.Name = "ModbusActiveCheck";
            this.ModbusActiveCheck.Size = new System.Drawing.Size(64, 16);
            this.ModbusActiveCheck.TabIndex = 8;
            this.ModbusActiveCheck.Text = "Active";
            this.ModbusActiveCheck.UseVisualStyleBackColor = true;
            this.ModbusActiveCheck.CheckedChanged += new System.EventHandler(this.ModbusActiveCheck_CheckedChanged);
            // 
            // SizeTextBox
            // 
            this.SizeTextBox.Location = new System.Drawing.Point(92, 161);
            this.SizeTextBox.Name = "SizeTextBox";
            this.SizeTextBox.Size = new System.Drawing.Size(172, 21);
            this.SizeTextBox.TabIndex = 12;
            this.SizeTextBox.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "Size : ";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(92, 95);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(172, 21);
            this.PortTextBox.TabIndex = 10;
            this.PortTextBox.Text = "502";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Port : ";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(92, 62);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(172, 21);
            this.IPTextBox.TabIndex = 8;
            this.IPTextBox.Text = "127.0.0.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = " I P : ";
            // 
            // ConfigSaveBtn
            // 
            this.ConfigSaveBtn.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ConfigSaveBtn.Location = new System.Drawing.Point(135, 554);
            this.ConfigSaveBtn.Name = "ConfigSaveBtn";
            this.ConfigSaveBtn.Size = new System.Drawing.Size(80, 27);
            this.ConfigSaveBtn.TabIndex = 3;
            this.ConfigSaveBtn.Text = "Save";
            this.ConfigSaveBtn.UseVisualStyleBackColor = true;
            this.ConfigSaveBtn.Click += new System.EventHandler(this.ConfigSaveBtn_Click);
            // 
            // ConfigCancelBtn
            // 
            this.ConfigCancelBtn.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ConfigCancelBtn.Location = new System.Drawing.Point(220, 554);
            this.ConfigCancelBtn.Name = "ConfigCancelBtn";
            this.ConfigCancelBtn.Size = new System.Drawing.Size(80, 27);
            this.ConfigCancelBtn.TabIndex = 4;
            this.ConfigCancelBtn.Text = "Cancel";
            this.ConfigCancelBtn.UseVisualStyleBackColor = true;
            this.ConfigCancelBtn.Click += new System.EventHandler(this.ConfigCancelBtn_Click);
            // 
            // MemoryGroup
            // 
            this.MemoryGroup.Controls.Add(this.MemoryUsageCheck);
            this.MemoryGroup.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MemoryGroup.Location = new System.Drawing.Point(12, 254);
            this.MemoryGroup.Name = "MemoryGroup";
            this.MemoryGroup.Size = new System.Drawing.Size(288, 80);
            this.MemoryGroup.TabIndex = 8;
            this.MemoryGroup.TabStop = false;
            this.MemoryGroup.Text = "Memory Usage";
            // 
            // MemoryUsageCheck
            // 
            this.MemoryUsageCheck.AutoSize = true;
            this.MemoryUsageCheck.Checked = true;
            this.MemoryUsageCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MemoryUsageCheck.Location = new System.Drawing.Point(24, 38);
            this.MemoryUsageCheck.Name = "MemoryUsageCheck";
            this.MemoryUsageCheck.Size = new System.Drawing.Size(64, 16);
            this.MemoryUsageCheck.TabIndex = 7;
            this.MemoryUsageCheck.Text = "Active";
            this.MemoryUsageCheck.UseVisualStyleBackColor = true;
            // 
            // SetConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 588);
            this.Controls.Add(this.MemoryGroup);
            this.Controls.Add(this.ConfigCancelBtn);
            this.Controls.Add(this.ConfigSaveBtn);
            this.Controls.Add(this.ModbusGroup);
            this.Controls.Add(this.CPUGroup);
            this.Controls.Add(this.TempGroup);
            this.Name = "SetConfigForm";
            this.Text = "Configuration";
            this.TempGroup.ResumeLayout(false);
            this.TempGroup.PerformLayout();
            this.CPUGroup.ResumeLayout(false);
            this.CPUGroup.PerformLayout();
            this.ModbusGroup.ResumeLayout(false);
            this.ModbusGroup.PerformLayout();
            this.MemoryGroup.ResumeLayout(false);
            this.MemoryGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TempGroup;
        private System.Windows.Forms.RadioButton core4;
        private System.Windows.Forms.RadioButton core3;
        private System.Windows.Forms.RadioButton core2;
        private System.Windows.Forms.RadioButton core1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox CPUGroup;
        private System.Windows.Forms.RadioButton cpu_current;
        private System.Windows.Forms.RadioButton cpu_total;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox ModbusGroup;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SizeTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox TempActiveCheck;
        private System.Windows.Forms.CheckBox CpuActiveCheck;
        private System.Windows.Forms.CheckBox ModbusActiveCheck;
        private System.Windows.Forms.Button ConfigSaveBtn;
        private System.Windows.Forms.Button ConfigCancelBtn;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox MemoryGroup;
        private System.Windows.Forms.CheckBox MemoryUsageCheck;


    }
}