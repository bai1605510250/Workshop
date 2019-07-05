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
    public partial class Setting : Form
    {
        


        public Setting()
        {
            InitializeComponent();
        }

      

        public void YES_Click(object sender, EventArgs e)
        {
          
            Form1.setValue.alpha_X_Value = Convert.ToInt16(alpha_X.Text);
            Form1.setValue.alpha_Y_Value = Convert.ToInt16(alpha_Y.Text);
            Form1.setValue.alpha_Z_Value = Convert.ToInt16(alpha_Z.Text);
            Form1.setValue.beta_X_Value  = Convert.ToInt16(beta_X.Text);
            Form1.setValue.beta_Y_Value  = Convert.ToInt16(beta_Y.Text);
            Form1.setValue.beta_Z_Value  = Convert.ToInt16(beta_Z.Text);

            Form1.setValue.PlanXA_Value = Convert.ToInt16(textBox_PlanXA.Text);
            Form1.setValue.PlanYA_Value = Convert.ToInt16(textBox_PlanYA.Text);
            Form1.setValue.PlanZA_Value = Convert.ToInt16(textBox_PlanZA.Text);
            Form1.setValue.PlanXB_Value = Convert.ToInt16(textBox_PlanXB.Text);
            Form1.setValue.PlanYB_Value = Convert.ToInt16(textBox_PlanYB.Text);
            Form1.setValue.PlanZB_Value = Convert.ToInt16(textBox_PlanZB.Text);

            Form1.setValue.m_dStepEquivalentX = Convert.ToDouble(textBox_StepEquivalentX.Text);
            Form1.setValue.m_dStepEquivalentY = Convert.ToDouble(textBox_StepEquivalentY.Text);
            Form1.setValue.m_dStepEquivalentZ = Convert.ToDouble(textBox_StepEquivalentZ.Text);

            Form1.setValue.m_nAccX = Convert.ToInt32(textBox_AccX.Text.ToString());
            Form1.setValue.m_nAccY = Convert.ToInt32(textBox_AccY.Text.ToString());
            Form1.setValue.m_nAccZ = Convert.ToInt32(textBox_AccZ.Text.ToString());

            Form1.setValue.m_dEditBackHomeAccX = Convert.ToDouble(textBox_BackHomeAccX.Text);
            Form1.setValue.m_dEditBackHomeAccY = Convert.ToDouble(textBox_BackHomeAccY.Text);
            Form1.setValue.m_dEditBackHomeAccZ = Convert.ToDouble(textBox_BackHomeAccZ.Text);
            Form1.setValue.m_nVelEmpty= Convert.ToDouble(IDC_EDIT_EMPTY_VEL_RIB.Text);

            Form1.m_nComboBackHomeDirX = comboBox_BackHomeDirX.SelectedIndex;
            Form1.m_nComboBackHomeDirY = comboBox_BackHomeDirY.SelectedIndex;
            Form1.m_nComboBackHomeDirZ = comboBox_BackHomeDirZ.SelectedIndex;

            Form1.setValue.m_nMaxDiatance = Convert.ToInt32(textBox_MaxDistance.Text.ToString());

            Form1.setValue.m_dEditBackHomeVelX = Convert.ToDouble(textBox_BackHomeVelX.Text);
            Form1.setValue.m_dEditBackHomeVelY = Convert.ToDouble(textBox_BackHomeVelY.Text);
            Form1.setValue.m_dEditBackHomeVelZ = Convert.ToDouble(textBox_BackHomeVelZ.Text);
            Hide();
            Close();
        }

        private void NO_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            alpha_X.Text = Form1.setValue.alpha_X_Value.ToString();
            alpha_Y.Text = Form1.setValue.alpha_Y_Value.ToString();
            alpha_Z.Text = Form1.setValue.alpha_Z_Value.ToString();
            beta_X.Text = Form1.setValue.beta_X_Value.ToString();
            beta_Y.Text = Form1.setValue.beta_Y_Value.ToString();
            beta_Z.Text = Form1.setValue.beta_Z_Value.ToString();

            textBox_PlanXA.Text = Form1.setValue.PlanXA_Value.ToString();
            textBox_PlanYA.Text = Form1.setValue.PlanYA_Value.ToString();
            textBox_PlanZA.Text = Form1.setValue.PlanZA_Value.ToString();
            textBox_PlanXB.Text = Form1.setValue.PlanXB_Value.ToString();
            textBox_PlanYB.Text = Form1.setValue.PlanYB_Value.ToString();
            textBox_PlanZB.Text = Form1.setValue.PlanZB_Value.ToString();

            textBox_StepEquivalentX.Text = Form1.setValue.m_dStepEquivalentX.ToString();
            textBox_StepEquivalentY.Text = Form1.setValue.m_dStepEquivalentY.ToString();
            textBox_StepEquivalentZ.Text = Form1.setValue.m_dStepEquivalentZ.ToString();

            textBox_AccX.Text = Convert.ToString(Form1.setValue.m_nAccX);
            textBox_AccY.Text = Convert.ToString(Form1.setValue.m_nAccY);
            textBox_AccZ.Text = Convert.ToString(Form1.setValue.m_nAccZ);

            textBox_BackHomeAccX.Text = Convert.ToString(Form1.setValue.m_dEditBackHomeAccX);
            textBox_BackHomeAccY.Text = Convert.ToString(Form1.setValue.m_dEditBackHomeAccY);
            textBox_BackHomeAccZ.Text = Convert.ToString(Form1.setValue.m_dEditBackHomeAccZ);

            IDC_EDIT_EMPTY_VEL_RIB.Text = Convert.ToString(Form1.setValue.m_nVelEmpty);

            comboBox_BackHomeDirX.SelectedIndex = Convert.ToInt32(Form1.m_nComboBackHomeDirX);
            comboBox_BackHomeDirY.SelectedIndex = Convert.ToInt32(Form1.m_nComboBackHomeDirY);
            comboBox_BackHomeDirZ.SelectedIndex = Convert.ToInt32(Form1.m_nComboBackHomeDirZ);

            textBox_BackHomeVelX.Text = Convert.ToString(Form1.setValue.m_dEditBackHomeVelX);
            textBox_BackHomeVelY.Text = Convert.ToString(Form1.setValue.m_dEditBackHomeVelY);
            textBox_BackHomeVelZ.Text = Convert.ToString(Form1.setValue.m_dEditBackHomeVelZ);

            textBox_MaxDistance.Text = Convert.ToString(Form1.setValue.m_nMaxDiatance);
        }
    }
}
