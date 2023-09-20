using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimKiemAnh
{
    public partial class Form1 : Form
    {
        string fileName = String.Empty;
        Kmeans km = new Kmeans();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    km.input(open.FileName);
                    MessageBox.Show("Success", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    btnKmean.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackgroundImage = Image.FromFile(open.FileName);
                fileName = open.SafeFileName;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                km.search(fileName, listView1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnKmean_Click(object sender, EventArgs e)
        {
            try
            {
                km.kmean();
                km.ghiFile();
                txtDBPS.Text = Convert.ToString(Math.Round(km.SSE, 2));
                MessageBox.Show("Success", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                btnLoadImg.Enabled = btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

