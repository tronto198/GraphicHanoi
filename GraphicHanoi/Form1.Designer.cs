namespace GraphicHanoi
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_start = new System.Windows.Forms.Button();
            this.tb_hanoiSize = new System.Windows.Forms.TextBox();
            this.lb_info = new System.Windows.Forms.Label();
            this.bt_play = new System.Windows.Forms.Button();
            this.bt_retry = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_start
            // 
            this.bt_start.Location = new System.Drawing.Point(432, 230);
            this.bt_start.Name = "bt_start";
            this.bt_start.Size = new System.Drawing.Size(60, 25);
            this.bt_start.TabIndex = 1;
            this.bt_start.Text = "start";
            this.bt_start.UseVisualStyleBackColor = true;
            this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
            // 
            // tb_hanoiSize
            // 
            this.tb_hanoiSize.Location = new System.Drawing.Point(341, 230);
            this.tb_hanoiSize.Name = "tb_hanoiSize";
            this.tb_hanoiSize.Size = new System.Drawing.Size(85, 25);
            this.tb_hanoiSize.TabIndex = 0;
            this.tb_hanoiSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_hanoiSize_KeyDown);
            this.tb_hanoiSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_hanoiSize_KeyPress);
            // 
            // lb_info
            // 
            this.lb_info.AutoSize = true;
            this.lb_info.Location = new System.Drawing.Point(296, 233);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(49, 15);
            this.lb_info.TabIndex = 2;
            this.lb_info.Text = "size : ";
            // 
            // bt_play
            // 
            this.bt_play.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_play.Location = new System.Drawing.Point(636, 336);
            this.bt_play.Name = "bt_play";
            this.bt_play.Size = new System.Drawing.Size(91, 61);
            this.bt_play.TabIndex = 2;
            this.bt_play.Text = "play";
            this.bt_play.UseVisualStyleBackColor = true;
            this.bt_play.Visible = false;
            this.bt_play.Click += new System.EventHandler(this.bt_play_Click);
            // 
            // bt_retry
            // 
            this.bt_retry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_retry.Location = new System.Drawing.Point(636, 403);
            this.bt_retry.Name = "bt_retry";
            this.bt_retry.Size = new System.Drawing.Size(91, 58);
            this.bt_retry.TabIndex = 3;
            this.bt_retry.Text = "retry";
            this.bt_retry.UseVisualStyleBackColor = true;
            this.bt_retry.Visible = false;
            this.bt_retry.Click += new System.EventHandler(this.bt_retry_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 540);
            this.Controls.Add(this.bt_retry);
            this.Controls.Add(this.bt_play);
            this.Controls.Add(this.tb_hanoiSize);
            this.Controls.Add(this.lb_info);
            this.Controls.Add(this.bt_start);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_start;
        private System.Windows.Forms.TextBox tb_hanoiSize;
        private System.Windows.Forms.Label lb_info;
        private System.Windows.Forms.Button bt_play;
        private System.Windows.Forms.Button bt_retry;
    }
}

