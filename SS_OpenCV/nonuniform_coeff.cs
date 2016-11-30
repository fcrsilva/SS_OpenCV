using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class nonuniform_coeff : Form
    {
        public int[] weight=new int[9];
        public int weight_factor;

        public nonuniform_coeff()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void nonuniform_coeff_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //good way to convert
            weight[0] = Int32.Parse(textBox1.Text.ToString());
            weight[1] = Int32.Parse(textBox2.Text.ToString());
            weight[2] = Int32.Parse(textBox3.Text.ToString());
            weight[3] = Int32.Parse(textBox4.Text.ToString());
            weight[4] = Int32.Parse(textBox5.Text.ToString());
            weight[5] = Int32.Parse(textBox6.Text.ToString());
            weight[6] = Int32.Parse(textBox7.Text.ToString());
            weight[7] = Int32.Parse(textBox8.Text.ToString());
            weight[8] = Int32.Parse(textBox9.Text.ToString());
            weight_factor = Int32.Parse(textBox10.Text.ToString());
            //how to close the window
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {//switch
            string select;

            select = comboBox1.SelectedItem.ToString();
            switch (select)
            {
                case "Mean Remove":
                    textBox1.Text = Convert.ToString(-1); //modifier public
                    textBox2.Text = Convert.ToString(-1);
                    textBox3.Text = Convert.ToString(-1);
                    textBox4.Text = Convert.ToString(-1);
                    textBox5.Text = Convert.ToString(9);
                    textBox6.Text = Convert.ToString(-1);
                    textBox7.Text = Convert.ToString(-1);
                    textBox8.Text = Convert.ToString(-1);
                    textBox9.Text = Convert.ToString(-1);
                    textBox10.Text = Convert.ToString(1);
                    break;

                case "Gaussian":
                    textBox1.Text = Convert.ToString(1); //modifier public
                    textBox2.Text = Convert.ToString(2);
                    textBox3.Text = Convert.ToString(1);
                    textBox4.Text = Convert.ToString(2);
                    textBox5.Text = Convert.ToString(4);
                    textBox6.Text = Convert.ToString(2);
                    textBox7.Text = Convert.ToString(1);
                    textBox8.Text = Convert.ToString(2);
                    textBox9.Text = Convert.ToString(1);
                    textBox10.Text = Convert.ToString(16);
                    break;

                case "Laplacian Hard":
                    textBox1.Text = Convert.ToString(1); //modifier public
                    textBox2.Text = Convert.ToString(-2);
                    textBox3.Text = Convert.ToString(1);
                    textBox4.Text = Convert.ToString(-2);
                    textBox5.Text = Convert.ToString(4);
                    textBox6.Text = Convert.ToString(-2);
                    textBox7.Text = Convert.ToString(1);
                    textBox8.Text = Convert.ToString(-2);
                    textBox9.Text = Convert.ToString(1);
                    textBox10.Text = Convert.ToString(11);
                    break;

                default:
                    break;

            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.DialogResult = DialogResult.Cancel;
        }
    }
}
