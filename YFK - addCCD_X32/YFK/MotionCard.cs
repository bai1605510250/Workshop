using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace SamsungTest
{
    public class MotionCard
    {
        public short sAxis = 1;
        static object lockObj = new object();
        public Label m_LabelMessage = null;
        public bool m_bCardOpen = false;                            //运动卡是否打开

        private bool[] bAxisAlarm = new bool[8];
        private bool[] bAxisLimCW = new bool[8];
        private bool[] bAxisLimCCW = new bool[8];
        private bool[] bAxisEstop = new bool[8];
        private bool[] bAxisServo = new bool[8];
        private bool[] bAxisMoving = new bool[8];
        private bool[] bAxisHome = new bool[8];
        private bool[] bInputBit = new bool[16];
        private bool[] bOutputBit = new bool[16];

        public short[] sADcValue = new short[8];
        public double[] dADcValue = new double[8];
        public double[] dTransA = new double[8];
        public double[] dTransB = new double[8];
        public double[] dPulseToMM = new double[8];         //pulse转换为mm
        public double[] dMMToPulse = new double[8];         //mm转换为pulse

        public double[] dCurrentPos = new double[8];       //当前位置
        public double[] dCurrentSpeed = new double[8];     //当前速度



        public double dLimSearchSpd = 10;
        public double dHomeSearchSpd = 100;
        public double dHomeCatchSpd = 20;
        public int iHomeOffSet = 0;

        public bool[] bFinish = {false, false };          //各轴原点回归状态
        public bool[] bGoing = {false,false};             //正在运行中
        public bool bLimDone = false;
        public bool bSearchCWL = false;
        public bool bEstop = false;

        private FormBase formbase;

        public MotionCard(FormBase _formbase)
        {
            formbase = _formbase;
            dPulseToMM[0] = 0.001;
            dPulseToMM[1] = 0.001;
            dPulseToMM[2] = 0.001;
            dPulseToMM[3] = 0.001;
            dPulseToMM[4] = 0.001;
            dPulseToMM[5] = 0.001;
            dPulseToMM[6] = 0.001;
            dPulseToMM[7] = 0.001;
            dMMToPulse[0] = 1000.0;
            dMMToPulse[1] = 1000.0;
            dMMToPulse[2] = 1000.0;
            dMMToPulse[3] = 1000.0;
            dMMToPulse[4] = 1000.0;
            dMMToPulse[5] = 1000.0;
            dMMToPulse[6] = 1000.0;
            dMMToPulse[7] = 1000.0;
            for (int i = 0; i < 8; i++)
            {
                dTransA[i] = 0.002;
                dTransB[i] = -4.9045;
            }
        }
        ~MotionCard()
        {
            if (m_bCardOpen)
            {
                for (short i = 1; i < 9; i++)
                {
                    StopMove(i);
                }
                lock (lockObj)
                {
                    gts.mc.GT_Close();
                }
            }
        }

        /// <summary>
        /// 初始化运动控制卡
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public bool InitCard(string strFilePath)
        {
            short sRtn = 0;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_Open(0, 1);
                if (!HandleErrorMessage(sRtn))
                {
                    return false;
                }
                sRtn = gts.mc.GT_Reset();
                sRtn = gts.mc.GT_LoadConfig(strFilePath);

                if (!HandleErrorMessage(sRtn))
                {
                    return false;
                }
                sRtn = gts.mc.GT_ClrSts(1, 8);                                 //清除轴报警和限位
                if (m_LabelMessage != null)
                {
                    m_LabelMessage.Text = "打开卡成功";
                    m_LabelMessage.BackColor = Color.Green;
                }
            }
            m_bCardOpen = true;

            //启动IO读写线程
            Thread threadIoScan = new Thread(new ThreadStart(ThreadGetStatus));
            threadIoScan.IsBackground = true;
            threadIoScan.Start();                                              //开启轴状态读取线程

            return true;
        }

        public bool SetOutputBit(short iOutputBit)
        {
            short sRtn;
            if (!m_bCardOpen)
            {
                return false;
            }
            iOutputBit++;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_SetDoBit(gts.mc.MC_GPO, iOutputBit, 0);
            }
            //HandleErrorMessage(sRtn);
            return true;
        }
        public bool RstOutputBit(short iOutputBit)
        {
            short sRtn;
            if (!m_bCardOpen)
            {
                return false;
            }
            iOutputBit++;
            lock (lockObj)
            {

                sRtn = gts.mc.GT_SetDoBit(gts.mc.MC_GPO, iOutputBit, 1);
            }
            //HandleErrorMessage(sRtn);
            return true;
        }

        public double GetCurrentPos(short Axis)
        {
            return dCurrentPos[Axis - 1];
        }
        public bool GetInputBit(short iInputBit)
        {
            return bInputBit[iInputBit];
        }
        public bool GetOutputBit(short iOutputBit)
        {
            return bOutputBit[iOutputBit];
        }
        public bool GetHomeSenser(short Axis)
        {
            return bAxisHome[Axis - 1];
        }
        public bool GetAxisAlarm(short Axis)
        {
            return bAxisAlarm[Axis - 1];
        }
        public bool GetAxisLimCW(short Axis)
        {
            return bAxisLimCW[Axis - 1];
        }
        public bool GetAxisLimCCW(short Axis)
        {
            return bAxisLimCCW[Axis - 1];
        }
        public bool GetAxisEstop(short Axis)
        {
            return bAxisEstop[Axis - 1];
        }
        public bool GetAxisServoOn(short Axis)
        {
            return bAxisServo[Axis - 1];
        }
        public bool GetAxisMoving(short Axis)
        {
            return bAxisMoving[Axis - 1];
        }

        public bool IsMoveDone(short Axis)
        {

            int lAxisStatus;
            uint uIntClock;
            int lTargetPos = 0;
            int lCurrentPos = 0;

            double dValue = 0.0;
            short sRtn;

            bool bResult;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_GetSts(Axis, out lAxisStatus, 1, out uIntClock);
                if ((lAxisStatus & 0x400) != 0)
                {
                    bResult = false;
                }
                else
                {
                    sRtn = gts.mc.GT_GetPos(Axis, out lTargetPos);
                    sRtn = gts.mc.GT_GetAxisPrfPos(Axis, out dValue, 1, out uIntClock);
                    lCurrentPos = (int)dValue;
                    if (lCurrentPos == lTargetPos)
                    {
                        bResult = true;
                    }
                    else
                    {
                        bResult = false;
                    }
                }
            }
            return bResult;
        }
        public void SetAxisVel(short Axis, double dVel)
        {
            short sRtn;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_SetVel(Axis, dVel);//设置目标速度
                sRtn = gts.mc.GT_Update(1 << (Axis - 1));//更新轴运动
            }

        }
        public double GetAxisVel(short Axis)
        {
            short sRtn;
            double dVel;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_GetVel(Axis, out dVel);//设置目标速度
            }
            return dVel;
        }

        /// <summary>
        /// JOG运行
        /// </summary>
        /// <param name="Axis"></param>
        /// <param name="dAcc"></param>
        /// <param name="dDec"></param>
        /// <param name="d"></param>     
        /// <returns></returns>
        public bool JogMove(short Axis, double dAcc, double dDec, double dVel)
        {
            if (!m_bCardOpen)
            {
                return false;
            }
            short sRtn;
            lock (lockObj)
            {
                gts.mc.TJogPrm jog = new gts.mc.TJogPrm();
                jog.acc = dAcc;
                jog.dec = dDec;
                sRtn = gts.mc.GT_ClrSts(Axis, 8);//清除轴报警和限位
                sRtn = gts.mc.GT_PrfJog(Axis);//设置为jog模式
                sRtn = gts.mc.GT_SetJogPrm(Axis, ref jog);//设置jog运动参数
                sRtn = gts.mc.GT_SetVel(Axis, dVel);//设置目标速度
                sRtn = gts.mc.GT_Update(1 << (Axis - 1));//更新轴运动 << (Axis - 1)
            }
            return true;
        }

        /// <summary>
        /// 停止运行
        /// </summary>
        /// <param name="Axis"></param>
        /// <returns></returns>
        public bool StopMove(short Axis)
        {
            if (!m_bCardOpen)
            {
                return false;
            }
            lock (lockObj)
            {
                gts.mc.GT_Stop(1 << (Axis - 1), 0);
            }
            return true;
        }
        public bool HandleErrorMessage(short errorMessage)
        {
            if (errorMessage != 0)
            {
              //  MessageBox.Show("指令执行错误 ", "报警");
                if (m_LabelMessage == null)
                {
                    return false;
                }
                switch (errorMessage)
                {
                    case 1:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "指令执行错误 ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                    case 2:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "license不支持 ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                    case 7:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "指令参数错误 ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                    case -1:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "主机和运动控制器通讯失败 ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                    case -6:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "打开控制器失败  ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                    case -7:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "运动控制器没有响应 ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                    default:
                        m_LabelMessage.Text = errorMessage.ToString() + "  :  " + "未知错误 ";
                        m_LabelMessage.BackColor = Color.Yellow;
                        break;
                }
                return false;
            }
            return true;
        }


        /// <summary>
        /// 状态读取线程
        /// </summary>
        private void ThreadGetStatus()
        {
            short i = 1;
            while (true)
            {
                for (i = 1; i < 9; i++)
                {
                    GetAxisCurrentPos(i);
                    GetAxisCurrentSpeed(i);
                    System.Threading.Thread.Sleep(1);
                    GetAxisStatus(i);
                    System.Threading.Thread.Sleep(1);

                    //SetLabel(formbase.axisfrm.labelCurrentPos01, dCurrentPos[0].ToString());        //轴位置及速度监视
                    //SetLabel(formbase.axisfrm.labelMoving01, dCurrentSpeed[0].ToString());
                }
                GetIOStatus();
                System.Threading.Thread.Sleep(5);
            }
        }

        //public delegate void dSetLabel(Label lbl, string str);        //创建label委托

        //public void SetLabel(Label lbl, string str)
        //{
        //    if (lbl.InvokeRequired)
        //    {
        //        dSetLabel call = new dSetLabel(SetLabel);
        //        lbl.Invoke(call, lbl, str);
        //    }
        //    else
        //    {
        //        lbl.Text = str;
        //    }

        //}

        /// <summary>
        /// 读取当前位置
        /// </summary>
        /// <param name="Axis"></param>
        private void GetAxisCurrentPos(short Axis)
        {
            if (!m_bCardOpen)
            {
                return;
            }
            double dValue = 0.0;
            uint uIntClock;
            short sRtn;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_GetAxisPrfPos(Axis, out dValue, 1, out uIntClock);

            }
            if (sRtn == 0)
            {
                dCurrentPos[Axis - 1] = dValue * dPulseToMM[Axis - 1];                //pulse转换为mm

                return;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 读取当前位置
        /// </summary>
        /// <param name="Axis"></param>
        /// 
        private void GetAxisCurrentSpeed(short Axis)
        {

            if (!m_bCardOpen)
            {
                return;
            }
            double pValue = 0.0;
            uint uIntClock;
            short sRtn;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_GetPrfVel(Axis, out pValue, 1, out uIntClock);


            }

            if (sRtn == 0)
            {
                dCurrentSpeed[Axis - 1] = pValue * dPulseToMM[Axis - 1];                //pulse转换为mm
                return;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 轴状态读取
        /// </summary>
        /// <param name="Axis"></param>
        private void GetAxisStatus(short Axis)
        {
            if (!m_bCardOpen)
            {
                return;
            }
            int lAxisStatus;
            uint uIntClock;
            short sRtn;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_GetSts(Axis, out lAxisStatus, 1, out uIntClock);
                if ((lAxisStatus & 0x1F2) != 0)
                {
                    sRtn = gts.mc.GT_ClrSts(Axis, 1);
                }
            }
            if (sRtn == 0)
            {
                //驱动器报警
                if ((lAxisStatus & 0x2) != 0)                        
                {
                    bAxisAlarm[Axis - 1] = true;
                }
                else
                {
                    bAxisAlarm[Axis - 1] = false;
                }
                //正限位被触发
                if ((lAxisStatus & 0x20) != 0)                      
                {
                    bAxisLimCW[Axis - 1] = true;
                }
                else
                {
                    bAxisLimCW[Axis - 1] = false;
                }
                //负限位被触发
                if ((lAxisStatus & 0x40) != 0)                  
                {
                    bAxisLimCCW[Axis - 1] = true;
                }
                else
                {
                    bAxisLimCCW[Axis - 1] = false;
                }

                //IO急停触发
                if ((lAxisStatus & 0x100) != 0)                   
                {
                    bAxisEstop[Axis - 1] = true;
                }
                else
                {
                    bAxisEstop[Axis - 1] = false;
                }
                // 伺服使能
                if ((lAxisStatus & 0x200) != 0)               
                {
                    bAxisServo[Axis - 1] = true;
                }
                else
                {
                    bAxisServo[Axis - 1] = false;
                }
                // 规划器正在运动
                if ((lAxisStatus & 0x400) != 0)
                {
                    bAxisMoving[Axis - 1] = true;
                }
                else
                {
                    bAxisMoving[Axis - 1] = false;
                }

                return;
            }
            else
            {
                return;
            }
        }


        /// <summary>///////////////////////////////////////////////////////////////////////////////
        /// GET IO状态////////////////////////////////////////////////////////////////////////////
        /// </summary>////////////////////////////////////////////////////////////////////////////
        private void GetIOStatus()
        {
            if (!m_bCardOpen)
            {
                return;
            }
            uint uIntClock;
            int lGpiValueInput;
            int lGpiValueOutput;
            int lGpiValueHome;
            short sRtn;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_GetDi(gts.mc.MC_GPI, out lGpiValueInput);
                sRtn = gts.mc.GT_GetDo(gts.mc.MC_GPO, out lGpiValueOutput);
                sRtn = gts.mc.GT_GetDi(gts.mc.MC_HOME, out lGpiValueHome);
                sRtn = gts.mc.GT_GetAdcValue(1, out sADcValue[0], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(2, out sADcValue[1], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(3, out sADcValue[2], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(4, out sADcValue[3], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(5, out sADcValue[4], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(6, out sADcValue[5], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(7, out sADcValue[6], 1, out uIntClock);
                sRtn = gts.mc.GT_GetAdcValue(8, out sADcValue[7], 1, out uIntClock);
            }


            //输入读取
            if (sRtn == 0)
            {
                if ((lGpiValueInput & 0x1) != 0)
                {
                    bInputBit[0] = false;
                }
                else
                {
                    bInputBit[0] = true;
                }
                if ((lGpiValueInput & 0x2) != 0)
                {
                    bInputBit[1] = false;
                }
                else
                {
                    bInputBit[1] = true;
                }
                if ((lGpiValueInput & 0x4) != 0)
                {
                    bInputBit[2] = false;
                }
                else
                {
                    bInputBit[2] = true;
                }
                if ((lGpiValueInput & 0x8) != 0)
                {
                    bInputBit[3] = false;
                }
                else
                {
                    bInputBit[3] = true;
                }
                if ((lGpiValueInput & 0x10) != 0)
                {
                    bInputBit[4] = false;
                }
                else
                {
                    bInputBit[4] = true;
                }
                if ((lGpiValueInput & 0x20) != 0)
                {
                    bInputBit[5] = false;
                }
                else
                {
                    bInputBit[5] = true;
                }
                if ((lGpiValueInput & 0x40) != 0)
                {
                    bInputBit[6] = false;
                }
                else
                {
                    bInputBit[6] = true;
                }
                if ((lGpiValueInput & 0x80) != 0)
                {
                    bInputBit[7] = false;
                }
                else
                {
                    bInputBit[7] = true;
                }
                if ((lGpiValueInput & 0x100) != 0)
                {
                    bInputBit[8] = false;
                }
                else
                {
                    bInputBit[8] = true;
                }
                if ((lGpiValueInput & 0x200) != 0)
                {
                    bInputBit[9] = false;
                }
                else
                {
                    bInputBit[9] = true;
                }
                if ((lGpiValueInput & 0x400) != 0)
                {
                    bInputBit[10] = false;
                }
                else
                {
                    bInputBit[10] = true;
                }
                if ((lGpiValueInput & 0x800) != 0)
                {
                    bInputBit[11] = false;
                }
                else
                {
                    bInputBit[11] = true;
                }
                if ((lGpiValueInput & 0x1000) != 0)
                {
                    bInputBit[12] = false;
                }
                else
                {
                    bInputBit[12] = true;
                }
                if ((lGpiValueInput & 0x2000) != 0)
                {
                    bInputBit[13] = false;
                }
                else
                {
                    bInputBit[13] = true;
                }
                if ((lGpiValueInput & 0x4000) != 0)
                {
                    bInputBit[14] = false;
                }
                else
                {
                    bInputBit[14] = true;
                }
                if ((lGpiValueInput & 0x8000) != 0)
                {
                    bInputBit[15] = false;
                }
                else
                {
                    bInputBit[15] = true;
                }
                //输出读取
                if ((lGpiValueOutput & 0x1) == 0)
                {
                    bOutputBit[0] = true;
                }
                else
                {
                    bOutputBit[0] = false;
                }
                if ((lGpiValueOutput & 0x2) == 0)
                {
                    bOutputBit[1] = true;
                }
                else
                {
                    bOutputBit[1] = false;
                }
                if ((lGpiValueOutput & 0x4) == 0)
                {
                    bOutputBit[2] = true;
                }
                else
                {
                    bOutputBit[2] = false;
                }
                if ((lGpiValueOutput & 0x8) == 0)
                {
                    bOutputBit[3] = true;
                }
                else
                {
                    bOutputBit[3] = false;
                }
                if ((lGpiValueOutput & 0x10) == 0)
                {
                    bOutputBit[4] = true;
                }
                else
                {
                    bOutputBit[4] = false;
                }
                if ((lGpiValueOutput & 0x20) == 0)
                {
                    bOutputBit[5] = true;
                }
                else
                {
                    bOutputBit[5] = false;
                }
                if ((lGpiValueOutput & 0x40) == 0)
                {
                    bOutputBit[6] = true;
                }
                else
                {
                    bOutputBit[6] = false;
                }
                if ((lGpiValueOutput & 0x80) == 0)
                {
                    bOutputBit[7] = true;
                }
                else
                {
                    bOutputBit[7] = false;
                }
                if ((lGpiValueOutput & 0x100) == 0)
                {
                    bOutputBit[8] = true;
                }
                else
                {
                    bOutputBit[8] = false;
                }
                if ((lGpiValueOutput & 0x200) == 0)
                {
                    bOutputBit[9] = true;
                }
                else
                {
                    bOutputBit[9] = false;
                }
                if ((lGpiValueOutput & 0x400) == 0)
                {
                    bOutputBit[10] = true;
                }
                else
                {
                    bOutputBit[10] = false;
                }
                if ((lGpiValueOutput & 0x800) == 0)
                {
                    bOutputBit[11] = true;
                }
                else
                {
                    bOutputBit[11] = false;
                }
                if ((lGpiValueOutput & 0x1000) == 0)
                {
                    bOutputBit[12] = true;
                }
                else
                {
                    bOutputBit[12] = false;
                }
                if ((lGpiValueOutput & 0x2000) == 0)
                {
                    bOutputBit[13] = true;
                }
                else
                {
                    bOutputBit[13] = false;
                }
                if ((lGpiValueOutput & 0x4000) == 0)
                {
                    bOutputBit[14] = true;
                }
                else
                {
                    bOutputBit[14] = false;
                }
                if ((lGpiValueOutput & 0x8000) == 0)
                {
                    bOutputBit[15] = true;
                }
                else
                {
                    bOutputBit[15] = false;
                }


                /////////////Home读取///////////

                if ((lGpiValueHome & 0x1) != 0)
                {
                    bAxisHome[0] = false;
                }
                else
                {
                    bAxisHome[0] = true;
                }
                if ((lGpiValueHome & 0x2) != 0)
                {
                    bAxisHome[1] = false;
                }
                else
                {
                    bAxisHome[1] = true;
                }
                if ((lGpiValueHome & 0x4) != 0)
                {
                    bAxisHome[2] = false;
                }
                else
                {
                    bAxisHome[2] = true;
                }
                if ((lGpiValueHome & 0x8) != 0)
                {
                    bAxisHome[3] = false;
                }
                else
                {
                    bAxisHome[3] = true;
                }
                if ((lGpiValueHome & 0x10) != 0)
                {
                    bAxisHome[4] = false;
                }
                else
                {
                    bAxisHome[4] = true;
                }
                if ((lGpiValueHome & 0x20) != 0)
                {
                    bAxisHome[5] = false;
                }
                else
                {
                    bAxisHome[5] = true;
                }
                if ((lGpiValueHome & 0x40) != 0)
                {
                    bAxisHome[6] = false;
                }
                else
                {
                    bAxisHome[6] = true;
                }
                if ((lGpiValueHome & 0x80) != 0)
                {
                    bAxisHome[7] = false;
                }
                else
                {
                    bAxisHome[7] = true;
                }

                for (int j = 0; j < 8; j++)
                {
                    dADcValue[j] = (double)(sADcValue[j]) * dTransA[j] + dTransB[j];
                }
                return;
            }
            else
            {
                return;
            }
        }

        public void SetAxisOn(short axis)
        {
            short sRtn;

            lock (lockObj)
            {
                //运动到"捕获位置+偏移量"
                sRtn = gts.mc.GT_AxisOn(axis);
                HandleErrorMessage(sRtn);
            }
        }
        public void SetAxisOff(short axis)
        {
            short sRtn;

            lock (lockObj)
            {
                //运动到"捕获位置+偏移量"
                sRtn = gts.mc.GT_AxisOff(axis);
                HandleErrorMessage(sRtn);
            }
        }

        public void SetAxisLimOn(short axis)
        {
            short sRtn;

            lock (lockObj)
            {
                //运动到"捕获位置+偏移量"
                sRtn = gts.mc.GT_LmtsOn(axis, -1);
                HandleErrorMessage(sRtn);
            }
        }
        public void SetAxisLimOff(short axis)
        {
            short sRtn;

            lock (lockObj)
            {
                //运动到"捕获位置+偏移量"
                sRtn = gts.mc.GT_LmtsOff(axis, -1);
                HandleErrorMessage(sRtn);
            }
        }
        public void AbsPosMove(short axis, double dAcc, double dDec, double dSpeed, double pos)             //绝对移动
        {
            short sRtn;
            bGoing[axis-1] = true;
            gts.mc.TTrapPrm trapPrm;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_PrfTrap(axis);
                // 读取点位模式运动参数
                sRtn = gts.mc.GT_GetTrapPrm(axis, out trapPrm);
                trapPrm.acc = dAcc;
                trapPrm.dec = dDec;
                // 设置点位模式运动参数
                sRtn = gts.mc.GT_SetTrapPrm(axis, ref trapPrm);
                // 设置点位模式目标速度，即回原点速度
                sRtn = gts.mc.GT_SetVel(axis, dSpeed);
                // 设置点位模式目标位置，即原点搜索距离
                sRtn = gts.mc.GT_SetPos(axis, (int)(pos * dMMToPulse[axis - 1]));
                // 启动运动
                sRtn = gts.mc.GT_Update(1 << (axis - 1));
                
            }
        }

        public void ReferPosMove(short axis, double dAcc, double dDec, double dSpeed, double pos)           //相对移动
        {
            short sRtn;
            bGoing[axis-1] = true;
            gts.mc.TTrapPrm trapPrm;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_PrfTrap(axis);
                // 读取点位模式运动参数
                sRtn = gts.mc.GT_GetTrapPrm(axis, out trapPrm);
                trapPrm.acc = dAcc;
                trapPrm.dec = dDec;
                // 设置点位模式运动参数
                sRtn = gts.mc.GT_SetTrapPrm(axis, ref trapPrm);
                // 设置点位模式目标速度，即回原点速度
                sRtn = gts.mc.GT_SetVel(axis, dSpeed);
                // 设置点位模式目标位置，即原点搜索距离
                int iTargetPos = (int)(dCurrentPos[axis - 1] * dMMToPulse[axis - 1] + pos * dMMToPulse[axis - 1]);
                sRtn = gts.mc.GT_SetPos(axis, iTargetPos);
                // 启动运动
                sRtn = gts.mc.GT_Update(1 << (axis - 1));
            }

        }


        /// <summary>////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////// HOME FIND//////////////////////////////////////////////
        /// </summary>//////////////////////////////////////////////////////////////////////////////////////


        public void StartHomeReturn()
        {
            if (bGoing[sAxis-1])
            {
                return;
            }
            bGoing[sAxis - 1] = true;
            bFinish[sAxis-1] = false;
            bLimDone = false;

            if(dCurrentPos[sAxis]>0)
            {
                bSearchCWL = true;
            }
            else
            {
                bSearchCWL = false;
            }


            System.Threading.Thread HomeThread = new System.Threading.Thread(HomeReturnThread);
            HomeThread.IsBackground = true;
            HomeThread.Start();
        }
        private void HomeReturnThread()
        {
            //执行极限搜索
         
            if (bSearchCWL)
                formbase.motioncard.JogMove(sAxis, 0.5, 0.5, dLimSearchSpd);
            else
                formbase.motioncard.JogMove(sAxis, 0.5, 0.5, -dLimSearchSpd);
            System.Threading.Thread.Sleep(100);
            bLimDone = true;

          //执行HOME捕捉
    
            if (bSearchCWL)
                StartHomeSearch(sAxis, -dHomeCatchSpd);
            else
                StartHomeSearch(sAxis, dHomeCatchSpd);

            System.Threading.Thread.Sleep(1);
            int pos;
            //查询捕获
            while (true)
            {
                if (bEstop)
                {
                    bGoing[sAxis - 1] = false;
                    return;
                }
                if (CheckHomeCatch(sAxis, out pos))
                {
                    break;
                }
                System.Threading.Thread.Sleep(1);
            }

            //判断运动完成
           
            System.Threading.Thread.Sleep(100);

            ReturnToHomePos(sAxis, pos + iHomeOffSet);
            System.Threading.Thread.Sleep(50);
            //判断运动完成
           
            ZeroAxisPos(sAxis);
            bGoing[sAxis - 1] = false;
            bFinish[sAxis-1] = true;

        }

        public void StartHomeSearch(short axis, double dCatchSpeed)
        {
            short sRtn;
            gts.mc.TTrapPrm trapPrm;
            lock (lockObj)
            {
                sRtn = gts.mc.GT_SetCaptureMode(axis, gts.mc.CAPTURE_HOME);
                // 切换到点位运动模式
                sRtn = gts.mc.GT_PrfTrap(axis);
                // 读取点位模式运动参数
                sRtn = gts.mc.GT_GetTrapPrm(axis, out trapPrm);
                trapPrm.acc = 0.25;
                trapPrm.dec = 0.25;
                // 设置点位模式运动参数
                sRtn = gts.mc.GT_SetTrapPrm(axis, ref trapPrm);
                // 设置点位模式目标速度，即回原点速度
                sRtn = gts.mc.GT_SetVel(axis, Math.Abs(dCatchSpeed));
                // 设置点位模式目标位置，即原点搜索距离
                int dSearchDis = 0;
                if (dCatchSpeed > 0)
                    dSearchDis = 999999999;
                else
                    dSearchDis = -999999999;
                sRtn = gts.mc.GT_SetPos(axis, dSearchDis);
                // 启动运动
                sRtn = gts.mc.GT_Update(1 << (axis - 1));
            }

        }

        public bool CheckHomeCatch(short axis, out int pos)
        {
            short sRtn;
            short capture;
            uint clk;
            lock (lockObj)
            {
                // 读取捕获状态
                sRtn = gts.mc.GT_GetCaptureStatus(axis, out capture, out pos, 1, out clk);
                if (capture == 1)
                {
                    gts.mc.GT_Stop(1 << (axis - 1), 0);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ReturnToHomePos(short axis, int pos)
        {
            short sRtn;

            lock (lockObj)
            {
                //运动到"捕获位置+偏移量"
                sRtn = gts.mc.GT_SetPos(axis, pos);
                formbase.motioncard.HandleErrorMessage(sRtn);
                // 在运动状态下更新目标位置
                sRtn = gts.mc.GT_Update(1 << (axis - 1));
                formbase.motioncard.HandleErrorMessage(sRtn);
            }

        }

        public void ZeroAxisPos(short axis)
        {
            short sRtn;

            lock (lockObj)
            {
                //运动到"捕获位置+偏移量"
                sRtn = gts.mc.GT_ZeroPos(axis, 1);
                formbase.motioncard.HandleErrorMessage(sRtn);
            }
        }


        //public delegate void dSetLabel(Label lbl, string str);        //创建label委托

        //public void SetLabel(Label lbl, string str)
        //{
        //    if (lbl.InvokeRequired)
        //    {
        //        dSetLabel call = new dSetLabel(SetLabel);
        //        lbl.Invoke(call, lbl, str);
        //    }
        //    else
        //    {
        //        lbl.Text = str;
        //    }

        //}

    }
}