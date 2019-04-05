using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicHanoi
{
    public partial class Form1 : Form
    {
        Mainprogram mp;
        bool pause = false;
        public Form1()
        {
            InitializeComponent();
            mp = new Mainprogram(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb_hanoiSize.Focus();
        }

        private void bt_start_Click(object sender, EventArgs e)
        {
            string sizestr = tb_hanoiSize.Text;
            try
            {
                int size = int.Parse(sizestr);
                bt_start.Visible = false;
                lb_info.Visible = false;
                tb_hanoiSize.Visible = false;

                mp.setHanoi(size);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("숫자를 입력하세요");
            }
        }
        
        public void calend()
        {
            this.Invoke(new Action(delegate ()
            {
                bt_play.Visible = true;
                bt_retry.Visible = true;
                bt_play.Focus();
            }));
            
        }
        public void showend()
        {
            this.Invoke(new Action(delegate ()
            {
                bt_play.Visible = false;
            }));
        }

        private void bt_play_Click(object sender, EventArgs e)
        {
            //bt_play.Visible = false;
            mp.playHanoi();   
        }
        private void bt_pause_Click(object sender, EventArgs e)
        {
            pause = !pause;
            mp.playpause(pause);
            if (pause)
            {
                bt_play.Text = "resume";
            }
            else
            {
                bt_play.Text = "pause";
            }
        }

        public void playHanoi()
        {
            bt_play.Click -= bt_play_Click;
            bt_play.Click += bt_pause_Click;
            bt_play.Text = "pause";
        }

        private void bt_retry_Click(object sender, EventArgs e)
        {
            bt_play.Visible = false;
            bt_retry.Visible = false;
            bt_start.Visible = true;
            lb_info.Visible = true;
            tb_hanoiSize.Text = "";
            tb_hanoiSize.Visible = true;
            tb_hanoiSize.Focus();

            bt_play.Text = "play";
            bt_play.Click -= bt_pause_Click;
            bt_play.Click += bt_play_Click;
            mp.retry();
            
        }

        private void tb_hanoiSize_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                bt_start_Click(null, null);
            }
        }

        private void tb_hanoiSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            
        }
    }
}
