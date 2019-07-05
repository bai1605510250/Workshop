using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YFK
{
    public partial class Axislimit : Form
    {

        public Axislimit()
        {
            InitializeComponent();
            setData();
        }
        private void setData()
        {
            
            textBox_xMinLimit.Text = Convert.ToString(Form1.Axis_limit.m_xMinLimit);
            textBox_xMaxLimit.Text = Convert.ToString(Form1.Axis_limit.m_xMaxLimit);

            textBox_yMinLimit.Text = Convert.ToString(Form1.Axis_limit.m_yMinLimit);
            textBox_yMaxLimit.Text = Convert.ToString(Form1.Axis_limit.m_yMaxLimit);

            textBox_zMinLimit.Text = Convert.ToString(Form1.Axis_limit.m_zMinLimit);
            textBox_zMaxLimit.Text = Convert.ToString(Form1.Axis_limit.m_zMaxLimit);

        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            int count = Form1.stringToDouble(textBox_xMinLimit.Text, ref Form1.Axis_limit.m_xMinLimit);
            count += Form1.stringToDouble(textBox_xMaxLimit.Text, ref Form1.Axis_limit.m_xMaxLimit);

            count += Form1.stringToDouble(textBox_yMinLimit.Text, ref Form1.Axis_limit.m_yMinLimit);
            count += Form1.stringToDouble(textBox_yMaxLimit.Text, ref Form1.Axis_limit.m_yMaxLimit);

            count += Form1.stringToDouble(textBox_zMinLimit.Text, ref Form1.Axis_limit.m_zMinLimit);
            count += Form1.stringToDouble(textBox_zMaxLimit.Text, ref Form1.Axis_limit.m_zMaxLimit);

            


            if (count == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void Axislimit_Load(object sender, EventArgs e)
        {

        }
    }
}
