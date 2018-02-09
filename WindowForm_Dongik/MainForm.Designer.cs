namespace WindowForm_Dongik
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
            this.label1 = new System.Windows.Forms.Label();
            this.realTimeBtn = new System.Windows.Forms.Button();
            this.lastDataBtn = new System.Windows.Forms.Button();
            this.vibrationBtn = new System.Windows.Forms.Button();
            this.sensorBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(427, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "PC데이터 모니터링 시스템";
            // 
            // realTimeBtn
            // 
            this.realTimeBtn.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.realTimeBtn.Location = new System.Drawing.Point(431, 176);
            this.realTimeBtn.Name = "realTimeBtn";
            this.realTimeBtn.Size = new System.Drawing.Size(306, 50);
            this.realTimeBtn.TabIndex = 1;
            this.realTimeBtn.Text = "실시간 정보 조회";
            this.realTimeBtn.UseVisualStyleBackColor = true;
            this.realTimeBtn.Click += new System.EventHandler(this.realTimeBtn_Click);
            // 
            // lastDataBtn
            // 
            this.lastDataBtn.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lastDataBtn.Location = new System.Drawing.Point(431, 262);
            this.lastDataBtn.Name = "lastDataBtn";
            this.lastDataBtn.Size = new System.Drawing.Size(306, 50);
            this.lastDataBtn.TabIndex = 5;
            this.lastDataBtn.Text = "지난 데이터 조회";
            this.lastDataBtn.UseVisualStyleBackColor = true;
            // 
            // vibrationBtn
            // 
            this.vibrationBtn.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.vibrationBtn.Location = new System.Drawing.Point(431, 353);
            this.vibrationBtn.Name = "vibrationBtn";
            this.vibrationBtn.Size = new System.Drawing.Size(306, 50);
            this.vibrationBtn.TabIndex = 6;
            this.vibrationBtn.Text = "진동 센서 정보 조회";
            this.vibrationBtn.UseVisualStyleBackColor = true;
            // 
            // sensorBtn
            // 
            this.sensorBtn.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sensorBtn.Location = new System.Drawing.Point(431, 445);
            this.sensorBtn.Name = "sensorBtn";
            this.sensorBtn.Size = new System.Drawing.Size(306, 50);
            this.sensorBtn.TabIndex = 7;
            this.sensorBtn.Text = "센서 관리";
            this.sensorBtn.UseVisualStyleBackColor = true;
            this.sensorBtn.Click += new System.EventHandler(this.sensorBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 662);
            this.Controls.Add(this.sensorBtn);
            this.Controls.Add(this.vibrationBtn);
            this.Controls.Add(this.lastDataBtn);
            this.Controls.Add(this.realTimeBtn);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Winform기반 모니터링 시스템";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button realTimeBtn;
        private System.Windows.Forms.Button lastDataBtn;
        private System.Windows.Forms.Button vibrationBtn;
        private System.Windows.Forms.Button sensorBtn;
    }
}

