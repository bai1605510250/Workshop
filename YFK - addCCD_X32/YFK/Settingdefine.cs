using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFK
{
    public class Settingdefine
    {
        /// <summary>
        /// 编码器当量
        /// </summary>
        public double alpha_X_Value = 1;
        public double alpha_Y_Value = 1;
        public double alpha_Z_Value = 1;

        public double beta_X_Value = 1;
        public double beta_Y_Value = 1;
        public double beta_Z_Value = 1;
        /// <summary>
        /// 规划器当量
        /// </summary>
        public double PlanXA_Value = 1; 
        public double PlanYA_Value = 1;
        public double PlanZA_Value = 1;

        public double PlanXB_Value =1;
        public double PlanYB_Value =1;
        public double PlanZB_Value =1;
        /// <summary>
        /// //步进当量
        /// </summary>
        public double m_dStepEquivalentX = 10;
        public double m_dStepEquivalentY = 10;
        public double m_dStepEquivalentZ = 10;

        public double m_xMinLimit = -100;
        public double m_xMaxLimit = 100;

        public double m_yMinLimit = -100;
        public double m_yMaxLimit = 100;

        public double m_zMinLimit = -100;
        public double m_zMaxLimit = 100;

        public int m_nAccX = 50;
        public int m_nAccY = 50;
        public int m_nAccZ = 50;

        public double  m_dEditBackHomeAccX = 0.0;
        public double m_dEditBackHomeAccY = 0.0;
        public double m_dEditBackHomeAccZ = 0.0;

        public double m_nVelEmpty = 50;

        public int m_nMaxDiatance = 1000;
        public double m_dEditBackHomeVelX = 0.0;
        public double m_dEditBackHomeVelY = 0.0;
        public double m_dEditBackHomeVelZ = 0.0;
    }
}
