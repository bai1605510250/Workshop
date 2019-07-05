using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using System.IO;

namespace YFK
{
    public partial class Form1 : Form
    {
        /***************************************CCD 参数************************************/

        public string ImagePath = System.IO.Directory.GetCurrentDirectory()+@"\Picture";
        public string Modle_Image = @"C:/Users/admin/Desktop/Picture/Value.png";
        public string ImageFile = "";
        public static HObject ho_Image, ho_ModelRegion, ho_TemplateImage, ho_ModelContours, ho_TransContours;
        public static HTuple m_Width = null, m_Height = null;
        public static HTuple hv_AcqHandle = null;
        public static bool m_bShow = false;
        public static Thread m_ShowImage = null;
        public static Thread m_startTest = null;
        public static Thread m_CheckIO= null;
        double x_change = 0.0;
        double y_change = 0.0;
        bool NO_Pic = false;








        /**************************************************************************************/


        public static Settingdefine setValue = null;
        public static Settingdefine Axis_limit = null;
        public int step = 5;
        public bool Thread_tmp=false;
        public bool trimm = false;
        public static int m_nRadioBackHomePriority;
        public double m_dx;
        public double m_dy;
        public double m_dz;
        public double m_da;
        public bool isFirstTimeAxisIO = true;
        String[] m_strEditPosition;
        String[] m_strEditSpeed;
        public double m_nStepSpeedRatio = 100;
        public double m_epsilon = 0.001;
        public long m_lAxisStatus_X;
        public long m_lAxisStatus_Y;
        public long m_lAxisStatus_Z;
        static long m_lAxisOldStatus_X = 0;
        static long m_lAxisOldStatus_Y = 0;
        static long m_lAxisOldStatus_Z = 0;
        public int m_lStatusInput;
        public int m_lStatusOutput;
        public int m_lStatusHome;
        static long m_lStatusInputOld = 65535;           //输入状态
        static long m_lStatusOutputOld = 65535;          //输出状态
        static long m_lStatusHomeOld = 65535;
        public static int m_nComboBackHomeDirX = 0;
        public static int m_nComboBackHomeDirY = 0;
        public static int m_nComboBackHomeDirZ = 0;
        State m_CurretState = State.Waitting;
        public bool isLaserChecked = false;
        public bool isGasChecked = false;

        public enum State
        {
            Working,
            Error,
            Stop,
            Waitting,
        }
        public enum LightSate
        {
            RedLight,
            GreenLight,
            YellowLight,
            Bee
        }
        #region define
        public static String baseDir;
        public const int AXIS_X = 1;
        public const int AXIS_Y = 2;
        public const int AXIS_Z = 3;

        public const short READY_ON = 0;
        public const short READY_OFF = 1;
        public const short READY_BIT = 5;  // 4 IN CARD
        /*****************************   IO定义************************************************/
        //输出
        public const short Bee_Out= 8;
        public const short LEDR_Out = 7;
        public const short LEDY_Out = 6;
        public const short LEDG_Out = 5;
        public const short Door_Close_Out = 4;
        public const short Gas_Out = 3;
        public const short Door_Open_Out = 1;
        public const short Laser_Out = 0;

        //输入
        public const short Door_left_Open_IN = 7;
        public const short Door_left_Close_IN = 6;
        public const short Door_right_Open_IN = 5;
        public const short Door_right_Close_IN = 4;
        public const short Safe_Raster_IN = 3;
        public const short Pedal_Switch_IN = 1;
        public const short Weld_Start_IN = 0;




        /*************************************************************************************/
        public const short ALARM_ON = 0;
        public const short ALARM_OFF = 1;
        public const short ALARM_BIT = 6;

        public const short FINISH_ON = 0;
        public const short FINISH_OFF = 1;
        public const short FINISH_BIT = 13;

        public const short BUSY_ON = 0;
        public const short BUSY_OFF = 1;
        public const short BUSY_BIT = 12;

        public const short STARTPROCESS_ON = 0;
        public const short STARTPROCESS_OFF = 1;
        public const short STARTPROCESS_BIT = 9;

        public const short EMERGENCYSTOP_ON = 0;
        public const short EMERGENCYSTOP_OFF = 1;
        public const short EMERGENCYSTOP_BIT = 10;


        public const short PANEL_FOOT = 0;
        public const short INPUT_PAUSE = 1;
        public const short INPUT_OVER = 2;

        public const short PANEL_OFF = 0;
        public const short PANEL_X = 1;
        public const short PANEL_Y = 2;
        public const short PANEL_Z = 3;
        public const short PANEL_A = 4; //C is A      



        public const short PANEL_X1 = 0;
        public const short PANEL_X10 = 1;
        public const short PANEL_X100 = 2;

        public const short PANEL_ENC = 11;//NDI

        public const short INPUT_SUBPROGRAM2 = 11;

        public const short INPUT_ERROR = 15;

        public const short IO_ON = 0;
        public const short IO_OFF = 1;

        public const short LASER_ON = 0;
        public const short LASER_OFF = 1;
        public const short LASER_BIT = 1;

        public const short VALVE_ON = 0;
        public const short VALVE_OFF = 1;
        public const short VALVE_BIT = 2;

        public const short SENSOR_PROTECTOR_ON = 0;
        public const short SENSOR_PROTECTOR_OFF = 1;
        public const short SENSOR_PROTECTOR_BIT = 3;

        public const short CCD_LIGHT1_BIT = 16;

        public const short LIGHT2_ON = 0;
        public const short LIGHT2_OFF = 1;
        public const short LIGHT2_BIT = 4;

        public const short LASER_OUTPUT_OFF = 1;
        public const short LASER_OUTPUT_ON = 0;
        public const short LASER_OUTPUT_BIT = 5;

        public const short WAIT_SIGNAL_BIT = 6;

        public const short START_END_SIGNAL = 7;
        public const short START_END_SIGNAL_ON = 0;
        public const short START_END_SIGNAL_OFF = 1;

        public const short IO_BIT_8 = 8;
        public const short IO_BIT_9 = 9;
        public const short IO_BIT_10 = 10;
        public const short IO_BIT_11 = 11;
        public const short IO_BIT_12 = 12;
        public const short IO_BIT_13 = 13;
        public const short IO_BIT_14 = 14;
        public const short IO_BIT_15 = 15;
        public const short IO_BIT_16 = 16;

        public const short ERROR_OUT = 16;

        public const short VIEW_3D = 0;
        public const short VIEW_XY = 1;
        public const short VIEW_YZ = 2;
        public const short VIEW_XZ = 3;


        public const short IPG_CODE_ERROR = -1;
        public const short IPG_CODE_RCS = 1;
        public const short IPG_CODE_ROP = 2;
        public const short IPG_CODE_RPP = 3;
        public const short IPG_CODE_RPRR = 4;
        public const short IPG_CODE_RPW = 5;
        public const short IPG_CODE_RCT = 6;
        public const short IPG_CODE_STA = 7;
        public const short IPG_CODE_SDC = 8;
        public const short IPG_CODE_SPRR = 9;
        public const short IPG_CODE_SPW = 10;

        public const short DATA_LASERPARA = 0x01;
        public const short DATA_ASKFOR_PAPR = 0x02;
        public const short DATA_SENDDATA = 0x03;

        public const short TIMER_LIMIT = 1;
        public const short TIMER_LIMIT_TIME = 50;

        public const short TIMER_POSVEL = 2;
        public const short TIMER_POSVEL_TIME = 50;

        public const short TIMER_POSVEL_C = 9;
        public const short TIMER_POSVEL_C_TIME = 50;

        public const short TIMER_CROSS = 3;
        public const short TIMER_CROSS_TIME = 100;

        public const short TIMER_BTOSTARTPOINT = 4;
        public const short TIMER_BTOSTARTPOINT_TIME = 300;

        public const short TIMER_VIDEO = 5;
        public const short TIMER_PROGRAMME = 6;
        public const short TIMER_PROGRAMME_STOP = 7;
        public const short TIMER_PROGRAMME_UPDATE = 8;

        public const short TIMER_WAITDLG_END = 10;
        public const short TIMER_WAITDLG_PROGRESS = 11;

        public const short TIMER_CURFLAG = 12;
        public const short TIMER_CURFLAG_TIME = 5;

        public const short TIMER_REPEAT = 13;
        public const short TIMER_REPEAT_TIME = 100;

        public const short TIMER_CURLINE = 14;
        public const short TIMER_CURLINE_TIME = 10;

        public const short TIMER_BACKHOME = 15;
        public const short TIMER_BACKHOME_TIME = 10;
        public const short TIMER_BACKHOME_PRIORITY = 16;
        public const short TIMER_BACKHOME_PRIORITY_TIME = 10;


        public const short TIMER_CAMERA = 17;
        public const short TIMER_CAMERA_TIME = 50;

        public const short TIMER_RUN_TIMER = 18;
        public const short TIMER_RUN_TIMER_TIME = 50;

        public const short TIMER_BYCODE = 19;
        public const short TIMER_BYCODE_TIME = 50;

        public const short TIMER_DRAW_DIHE = 20;
        public const short TIMER_DRAW_DIHE_TIME = 10;

        public const short TIMER_PANEL = 21;
        public const short TIMER_PANEL_TIME = 10;

        //public const short TIMER_FOOT = 22;
        //public const short TIMER_FOOT_TIME = 10;


        public const short TIMER_STOP_SLEEP = 23;
        public const short TIMER_STOP_SLEEP_TIME = 5000;

        public const short TIMER_LASEROUTPUT_TYPE = 25;
        public const short TIMER_LASEROUTPUT_TYPE_TIME = 1;

        public const short TIMER_LASEROUTPUT_PULSE = 26;

        public const short TIMER_WAIT = 27;
        public readonly short TIMER_WAIT_TIME = 10;

        public readonly short TIMER_WAIT_FOR = 28;
        public readonly short TIMER_WAIT_FOR_TIME = 10;

        public readonly short TIMER_SUBPROGRAM = 29;
        public readonly short TIMER_SUBPROGRAM_TIME = 20;

        public readonly short TIMER_OUT_ERROR = 30;
        public readonly short TIMER_OUT_ERROR_TIME = 10;

        public readonly short TIMER_PROGRAM_PANEL = 31;
        public readonly short TIMER_PROGRAM_PANEL_TIME = 10;

        public readonly short TIMER_START_END_SIGNAL = 32;
        public readonly short TIMER_START_END_SIGNAL_TIME = 20;

        public readonly short TIMER_POSVEL_ASSISTAXIS = 33;
        public readonly short TIMER_POSVEL_ASSISTAXIS_TIME = 50;

        public readonly short TIMER_GET_WORKSTATUS = 34;
        public readonly short TIMER_GET_WORKSTATUS_TIME = 50;

        public readonly short TIMER_SET_BTNSTATUS = 35;
        public readonly short TIMER_SET_BTNSTATUS_TIME = 50;

        public readonly short TIMER_IPG_ATATUS = 36;
        public readonly short TIMER_IPG_ATATUS_TIME = 1000;

        public readonly short TIMER_LASER_STATUS = 37;
        public readonly short TIMER_LASER_STATUS_TIME = 100;

        public const int MAX_ZOOM = 128;
        public const int MAX_STEPLENGTH = 40;

        public const short NOTYPE = 0;
        public const short SI = 1;
        public const short EN = 2;
        #endregion

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "THLaser welding";
            InitCard();
            setValue = new Settingdefine();
            Axis_limit = new Settingdefine();
            m_strEditPosition = new String[4] { "0.00", "0.00", "0.00", "0.00" };
            m_strEditSpeed = new String[4] { "0.00", "0.00", "0.00", "0.00" };
            m_lStatusHome = 0;
            m_lStatusOutput = 0;
            m_lStatusInput = 0;
            m_lAxisStatus_X = 0;
            m_lAxisStatus_Y = 0;
            m_lAxisStatus_Z = 0;
            
            Time_Start();

            //m_startTest = new Thread(new ThreadStart(StartTest));
            //m_startTest.IsBackground = true;
            //m_startTest.Start();

            m_CheckIO= new Thread(CheckIO);
            m_CheckIO.IsBackground = true;
            m_CheckIO.Start();

            
            //OpenCamera();
            //collector();//连续采集
            //  comboBox_stepSelect.ControlAdded()
             mc.GT_SetDoBit(mc.MC_GPO, Door_Open_Out, 0);//开安全门
            mc.GT_SetDoBit(mc.MC_GPO, Door_Close_Out, 1);
        }
        public void CheckIO()
        {
            while (true)
            {
                Thread.Sleep(1000);
               // LogMessage("GO_ON");
                //if(ribbonCheckBox_Input3.Checked)//光栅
                //{
                //    ChangeState(State.Stop);
                //    stopAllAxes();
                //}
                //if (ribbonCheckBox_Input1.Checked)
                //{
                //    //  ChangeState(State.Working);
                //}
                //if (m_CurretState==State.Working)
                //{
                //    StartTest();
                //}
            }
        }

        private void Time_Start()
        {
            timerIOState.Start();
            timer_PosVel2.Start();
        }
        private void closeCard()
        {
            mc.GT_Stop(15, 0);
            mc.GT_Close();
        }
        private void InitCard()
        {
            try
            {
                short rtn = mc.GT_Open();
                if (rtn != 0)
                {
                    throw new Exception("Couldn't connnect to GTS motion control");
                }


                rtn = mc.GT_Reset();
                rtn = mc.GT_SetDo(12, 0xFFFF);//设置数字 IO 输出状态
                rtn = mc.GT_ZeroPos(1);//设置机械原点，
                rtn = mc.GT_ZeroPos(2);
                rtn = mc.GT_ZeroPos(3);
                rtn = mc.GT_ZeroPos(4);
                // init input config
                baseDir = System.Environment.CurrentDirectory;
                string strPathPath = baseDir + @"\CFG\GTS.cfg";
                rtn = mc.GT_LoadConfig(strPathPath);//加载配置
                if (rtn != 0)
                {
                    throw new Exception("Can't load GTS.cfg file!");
                }

                uint clock = 0;

                double enc = 0;
                short rt = mc.GT_GetEncPos(11, out enc, 1, out clock);// 读取编码器位置 

                // 设置控制轴的编码器当量变换值
                mc.GT_EncScale((short)AXIS_X, (short)setValue.alpha_X_Value, (short)setValue.beta_X_Value);
                mc.GT_EncScale((short)AXIS_Y, (short)setValue.alpha_Y_Value, (short)setValue.beta_Y_Value);
                mc.GT_EncScale((short)AXIS_Z, (short)setValue.alpha_Z_Value, (short)setValue.beta_Z_Value);
                //////设置控制轴的规划器当量变换值。
                mc.GT_ProfileScale((short)AXIS_X, (short)Form1.setValue.PlanXA_Value, (short)Form1.setValue.PlanXB_Value);
                mc.GT_ProfileScale((short)AXIS_Y, (short)Form1.setValue.PlanYA_Value, (short)Form1.setValue.PlanYB_Value);
                mc.GT_ProfileScale((short)AXIS_Z, (short)Form1.setValue.PlanZA_Value, (short)Form1.setValue.PlanZB_Value);
                rtn = mc.GT_ClrSts(1, 8);// 清除捕获状态
                rtn = mc.GT_AxisOn(1);//打开驱动器使能 参数轴号
                if (rtn != 0)
                {
                    throw new Exception(" Axis 1 not On");
                }
                rtn = mc.GT_AxisOn(2);
                if (rtn != 0)
                {
                    throw new Exception(" Axis 2 not On");
                }
                rtn = mc.GT_AxisOn(3);
                if (rtn != 0)
                {
                    throw new Exception(" Axis 3 not On");
                }
                rtn = mc.GT_AxisOn(4);
                if (rtn != 0)
                {
                    throw new Exception(" Axis 4 not On");
                }
                if (rtn == 0)
                {
                   // readySignal(true);
                }
            }
            catch (Exception e)
            {
                String error_str = "Error:" + e.Message;
                LogMessage(error_str, 1);
                //Log.WriteInfo(e);
            }
        }
        public void LogMessage(string text, int iColorMode = 1)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke((Action)delegate ()
                {
                    text = DateTime.Now.ToString("[HH:mm:ss ]--") + text+"\r\n";
                    MessageShow_richTextBox.Text = MessageShow_richTextBox.Text.Insert(0, text);
                });
            }
        }
        void readySignal(bool isON)
        {
            //if (isON)
            //{
            //    mc.GT_SetDoBit(mc.MC_GPO, READY_BIT, READY_ON);
            //}
            //else
            //{
            //    mc.GT_SetDoBit(mc.MC_GPO, READY_BIT, READY_OFF);
            //}
        }
        public void MoveBackHome(int Axis, double dis)
        {
            mc.TTrapPrm trap = GetTrap(Axis, false);
            double speed = GetSpeedBackHome(Axis);
            long m_lPlusPos = TransDistanceToPlus(dis, Axis);
            MoveTrap(ref Axis, ref m_lPlusPos, ref speed, ref trap);
        }
        public bool CheckLimitTrigger(int Axis, int dir = 0)
        {
            bool result = false;
            int lAxisStatus = 0;
            short count = 1;
            uint clock;
            mc.GT_GetSts((short)Axis, out lAxisStatus, count, out clock);
            switch (dir)
            {
                case 1:
                    if ((lAxisStatus & 0x20) != 0)
                    {
                        result = true;
                    }
                    break;
                case -1:
                    if ((lAxisStatus & 0x40) != 0)
                    {
                        result = true;
                    }
                    break;
                case 0:
                    if ((lAxisStatus & 0x20) != 0 || (lAxisStatus & 0x40) != 0)
                    {
                        result = true;
                    }
                    break;
                default:
                    LogMessage("限位查询参数错误");
                    break;
            }
            return result;
        }
        public double GetSpeedBackHome(int Axis)
        {
            double speed = 0;
            switch (Axis)
            {
                case (short)Form1.AXIS_X:
                    speed = Form1.setValue.m_dEditBackHomeVelX;
                    break;
                case (short)Form1.AXIS_Y:
                    speed = Form1.setValue.m_dEditBackHomeVelY;
                    break;
                case (short)Form1.AXIS_Z:
                    speed = Form1.setValue.m_dEditBackHomeVelZ;
                    break;

            }
            speed = TransSpeedToPlus(speed, Axis);
            return speed;
        }
        private void Setting_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.Show(this);
        }
        private void timer_PosVel2_Tick( object sender, EventArgs e)
        {
            double[] axisEncPos = new double[4] { 0.0, 0.0, 0.0, 0.0 };
            GetCurPos(ref axisEncPos);
            m_dx = axisEncPos[0];
            m_dy = axisEncPos[1];
            m_dz = axisEncPos[2];
            //位置
            m_strEditPosition[0] = Convert.ToString(m_dx);
            m_strEditPosition[1] = Convert.ToString(m_dy);
            m_strEditPosition[2] = Convert.ToString(m_dz);
            double[] axisEncVel = new double[4] { 0, 0, 0, 0 };
            short count = 1;
            uint clock = 0;
            //速度
            mc.GT_GetAxisEncVel(AXIS_X, out axisEncVel[0], count, out clock);
            mc.GT_GetAxisEncVel(AXIS_Y, out axisEncVel[1], count, out clock);
            mc.GT_GetAxisEncVel(AXIS_Z, out axisEncVel[2], count, out clock);

            double ThreadPitch = 1.6;
            for (int i = 0; i < 3; i++)
            {
                if (2 == i)
                {
                    axisEncVel[i] *= -1;
                }
                int AXIS = i + 1;
                ThreadPitch = GetThreadPitch(AXIS);
                axisEncVel[i] = axisEncVel[i] * ThreadPitch;//速度 脉冲转化mm/s
            }
            m_strEditSpeed[0] = Convert.ToString(axisEncVel[0]);
            m_strEditSpeed[1] = Convert.ToString(axisEncVel[1]);
            m_strEditSpeed[2] = Convert.ToString(axisEncVel[2]);

            NegativeZero(ref m_strEditSpeed);

            ribbonTextBox_PosX.Text = m_strEditPosition[0];
            ribbonTextBox_PosY.Text = m_strEditPosition[1];
            ribbonTextBox_PosZ.Text = m_strEditPosition[2];

            ribbonTextBox_SpeedX.Text = m_strEditSpeed[0];
            ribbonTextBox_SpeedY.Text = m_strEditSpeed[1];
            ribbonTextBox_SpeedZ.Text = m_strEditSpeed[2];

        }
        void NegativeZero(ref String[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == "-0.000")
                {
                    str[i] = "0.000";
                }
                if (str[i] == "-0.0000")
                {
                    str[i] = "0.0000";
                }
            }
        }
        public void GetCurPos(ref double[] axisEncPos)
        {
            uint clock = 0;
            double x, y, z, a;
            mc.GT_GetAxisEncPos((short)AXIS_X, out x, 1, out clock);
            mc.GT_GetAxisEncPos((short)AXIS_Y, out y, 1, out clock);
            mc.GT_GetAxisEncPos((short)AXIS_Z, out z, 1, out clock);//读取轴编码器的位置

            axisEncPos[0] = TransPlusToDistance(ref x, AXIS_X);
            axisEncPos[1] = TransPlusToDistance(ref y, AXIS_Y);
            axisEncPos[2] = TransPlusToDistance(ref z, AXIS_Z);
        }
        /******************************************************************************************************/
        public double TransPlusToDistance(ref double plus, int Axis)
        {
            return plus * GetThreadPitch(Axis) / 1000;
        }
        public double GetThreadPitch(int Axis)
        {
            double ThreadPitch = 1.6;
            switch (Axis)
            {
                case (int)Form1.AXIS_X:
                    ThreadPitch = Form1.setValue.m_dStepEquivalentX;
                    break;
                case (int)Form1.AXIS_Y:
                    ThreadPitch = Form1.setValue.m_dStepEquivalentY;
                    break;
                case (int)Form1.AXIS_Z:
                    ThreadPitch = Form1.setValue.m_dStepEquivalentZ;
                    break;

            }
            return ThreadPitch;
        }


        /***********************************************************************************/
        private void button2_Click(object sender, EventArgs e)
        {
            OnButtonLocationClear();
        }
        public void OnButtonLocationClear()
        {

            short rtn = mc.GT_ZeroPos(1);
            rtn = mc.GT_ZeroPos(2);
            rtn = mc.GT_ZeroPos(3);

            mc.GT_SetPrfPos(AXIS_X, 0);
            mc.GT_SetPrfPos(AXIS_Y, 0);
            mc.GT_SetPrfPos(AXIS_Z, 0);

            mc.GT_CrdClear(1, 0);
            PanelClear();
        }
        public void PanelClear()
        {
            mc.GT_SetEncPos(PANEL_ENC, 0);

        }

        private void button_xneg_Click_Click(object sender, EventArgs e)
        {
            StepManual(AXIS_X, false);
        }
        private void button_xpos_Click_Click(object sender, EventArgs e)
        {
            StepManual(AXIS_X, true);
        }

        private void button_Ypos_Click_Click(object sender, EventArgs e)
        {
            StepManual(AXIS_Y, true);
        }

        private void button_Yneg_Click_Click(object sender, EventArgs e)
        {
            StepManual(AXIS_Y, false);
        }

        private void button_ZPos_Click_Click(object sender, EventArgs e)
        {
            StepManual(AXIS_Z, true);
        }

        private void button_Zneg_Click_Click(object sender, EventArgs e)
        {
            StepManual(AXIS_Z, false);
        }
        public bool StepManual(int Axis, bool Direction)
        {
            if (CheckIsMoving(Axis))
            {
                return false;
            }
            double steplength = 0.0;
            double ThreadPitch = GetThreadPitch(Axis);
            int status = 0;
            if (radioButton_MovewayComboSel.Checked)
            {
                status = stringToDouble(comboBox_stepSelect.SelectedItem.ToString(), ref steplength);
            }
            else
            {
                status = stringToDouble(textBox_stepInput.Text, ref steplength);
            }
            bool rt = checkLimits(Axis, steplength);
            if (!rt)
            {
                string error = "Axis reached limit";
                LogMessage(error);
                return rt;
            }

            long m_lAxisPos = TransDistanceToPlus(steplength, Axis);
            if (!Direction)
            {
                m_lAxisPos *= -1;
            }
            
            double speed = GetPlusSpeedEmpty(Axis, m_nStepSpeedRatio);
            MoveTrap(Axis, m_lAxisPos, speed);
            return true;
        }
        public void MoveTrap(int Axis, long m_lPlusPos, double speed, bool IsNotAbs = true)
        {
            mc.TTrapPrm trap = new mc.TTrapPrm();
            trap = GetTrap(Axis);
            MoveTrap(ref Axis, ref m_lPlusPos, ref speed, ref trap, IsNotAbs);
        }
        public void CommandHandle(short error)
        {
            if (error != 0)
            {
                if (error == 7)
                {
                    return;
                }

                switch (error)
                {
                    case -6:
                        LogMessage("打开控制器失败，请重启!\n请检查. 板卡是否插好. 是否正确安装运动控制器驱动程序； 其他程序是否已经打开运动控制器，或进程中是否还驻留着打开控制器的程序");
                        break;
                    case 1:
                        LogMessage("指令执行错.检查当前指令的执行条件是否满足");
                        break;
                    case 2:
                        LogMessage("license不支持  ");
                        break;
                    case 7:
                        LogMessage("指令参数错误  ！");
                        break;
                    case -1:
                        LogMessage("主机和运动控制器通讯失败 ！");
                        break;
                    case -7:
                        LogMessage("运动控制器没有响应！更换运动控制器");
                        break;
                }
            }
        }
        public void MoveTrap(ref int Axis, ref long m_lPlusPos, ref double speed, ref mc.TTrapPrm trap, bool IsNotAbs = true)
        {
            short sRtn;
            sRtn = mc.GT_AxisOn((short)Axis);//打开驱动使能
            CommandHandle(sRtn);
            if (IsNotAbs)
            {
                sRtn = mc.GT_SetPrfPos((short)Axis, 0);//修改指定轴的规划位置。禁止在运动状态下修改规划位置
            }
            CommandHandle(sRtn);
            sRtn = mc.GT_PrfTrap((short)Axis);//设置指定轴为点位模式。 
            CommandHandle(sRtn);

            sRtn = mc.GT_SetTrapPrm((short)Axis, ref trap);//设置点位模式运动参数
            CommandHandle(sRtn);

            sRtn = mc.GT_SetPos((short)Axis, (int)m_lPlusPos);//设置目标位置。 
            CommandHandle(sRtn);

            sRtn = mc.GT_SetVel((short)Axis, speed);//设置目标速度
            CommandHandle(sRtn);

            sRtn = mc.GT_Update(1 << (Axis - 1));//启动点位运动或 Jog 运动。 
            CommandHandle(sRtn);
        }
        public double GetAcc(int Axis)
        {
            double acc = 0.0;
            switch (Axis)
            {
                case (int)Form1.AXIS_X:
                    acc = Form1.setValue.m_nAccX;
                    break;
                case (int)Form1.AXIS_Y:
                    acc = Form1.setValue.m_nAccY;
                    break;
                case (int)Form1.AXIS_Z:
                    acc = Form1.setValue.m_nAccZ;
                    break;

            }

            acc /= 1000.0;
            acc /= GetThreadPitch(Axis);

            return acc;
        }
        mc.TTrapPrm GetTrap(int Axis, bool IsNotBackHome = true)
        {
            mc.TTrapPrm trap = new mc.TTrapPrm();
            if (IsNotBackHome)
            {
                trap.acc = GetAcc(Axis) * 0.7;
            }
            else
            {
                trap.acc = GetAccBackHome(Axis);
            }
            trap.dec = trap.acc;
            trap.smoothTime = 25;
            trap.velStart = 0;
            return trap;
        }
        public double GetAccBackHome(int Axis)
        {
            double acc = 0.0;
            switch (Axis)
            {
                case (int)Form1.AXIS_X:
                    acc = Form1.setValue.m_dEditBackHomeAccX;
                    break;
                case (int)Form1.AXIS_Y:
                    acc = Form1.setValue.m_dEditBackHomeAccY;
                    break;
                case (int)Form1.AXIS_Z:
                    acc = Form1.setValue.m_dEditBackHomeAccZ;
                    break;

            }

            acc /= 1000.0;
            acc /= GetThreadPitch(Axis);

            return acc;
        }
        public double GetPlusSpeedEmpty(int Axis = Form1.AXIS_X, double SpeedRatio = 100)
        {
            double speed = 0;
            speed = TransSpeedToPlus(Form1.setValue.m_nVelEmpty * (double)SpeedRatio / 100.0, Axis);
            return speed;
        }
        public double TransSpeedToPlus(double speed, int Axis = (int)Form1.AXIS_X)
        {
            return speed / GetThreadPitch(Axis);
        }
        public long TransDistanceToPlus(double distance, int Axis)
        {
            return (long)(distance * 1000 / GetThreadPitch(Axis));
        }
        public bool checkLimits(int axis, double position)//检查是否超过限位
        {
            double accumulatedPosition = 0;
            switch (axis)
            {
                case AXIS_X:
                    accumulatedPosition = m_dx + position;//当前位置+输入距离
                    if (accumulatedPosition < Form1.Axis_limit.m_xMinLimit
                        || accumulatedPosition > Form1.Axis_limit.m_xMaxLimit)
                    {
                        return false;
                    }
                    break;
                case AXIS_Y:
                    accumulatedPosition = m_dy + position;
                    if (accumulatedPosition < Form1.Axis_limit.m_yMinLimit
                        || accumulatedPosition > Form1.Axis_limit.m_yMaxLimit)
                    {
                        return false;
                    }
                    break;

                case AXIS_Z:
                    accumulatedPosition = m_dz + position;
                    if (accumulatedPosition < Form1.Axis_limit.m_zMinLimit
                        || accumulatedPosition > Form1.Axis_limit.m_zMaxLimit)
                    {
                        return false;
                    }
                    break;

                default:
                    break;
            }
            return true;
        }
        public static int stringToDouble(string strText, ref double value)
        {
            if (!double.TryParse(strText, out value)
                || strText.Trim() == string.Empty)
            {
                string error = "The value " + strText;
                error += " cannot be parsed";
                MessageBox.Show(error);
                return 1;
            }
            return 0;
        }
        public bool CheckIsMoving(int Axis)     //true  启动，false 停止
        {
            int status = 0;
            short sRtn = 0;
            short count = 1;
            uint clock;
            short ax = (short)Axis;
            sRtn = mc.GT_GetSts(ax, out status, count, out clock);//读取轴状态
            if (0 == (status & 0x400))
            {
                return false;       // 规划器已停止标志 
            }
            else
            {

                return true;         // 规划器运动
            }
        }

        private void Axis_limitSetting_Click(object sender, EventArgs e)
        {
            Axislimit dlgAVisionLimits = new Axislimit();
            DialogResult dr = dlgAVisionLimits.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                //m_thDefineVisionLimits.SaveToXmlFile(m_thDlgVisionLimitsXMLFilePath);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void timerIOState_Tick(object sender, EventArgs e)
        {

            short sRtn;
            //轴部分

            short rt = mc.GT_ClrSts(1, 8);
            if (isFirstTimeAxisIO)
            {
                m_lAxisOldStatus_X = m_lAxisStatus_X; //  X轴旧状态保存 
                m_lAxisOldStatus_Y = m_lAxisStatus_Y; //  y轴旧状态保存 
                m_lAxisOldStatus_Z = m_lAxisStatus_Z; //  z轴旧状态保存 

                isFirstTimeAxisIO = false;
            }

            int m_lAxisStatus_X_Local = (int)m_lAxisStatus_X;
            int m_lAxisStatus_Y_Local = (int)m_lAxisStatus_Y;
            int m_lAxisStatus_Z_Local = (int)m_lAxisStatus_Z;
            short count = 1;
            uint clock = 0;
            sRtn = mc.GT_GetSts(AXIS_X, out m_lAxisStatus_X_Local, count, out clock);
            sRtn = mc.GT_GetSts(AXIS_Y, out m_lAxisStatus_Y_Local, count, out clock);
            sRtn = mc.GT_GetSts(AXIS_Z, out m_lAxisStatus_Z_Local, count, out clock);
            m_lAxisStatus_X = (long)m_lAxisStatus_X_Local;

            m_lAxisStatus_Y = (long)m_lAxisStatus_Y_Local;

            m_lAxisStatus_Z = (long)m_lAxisStatus_Z_Local;

            if ((m_lAxisStatus_Y & (1 << 5)) != (m_lAxisOldStatus_Y & (1 << 5)))
                UpdateLimitStatus(ribbonCheckBox_LimitYN, (m_lAxisStatus_Y & (1 << 5)) >> 5);
            if ((m_lAxisStatus_Y & (1 << 6)) != (m_lAxisOldStatus_Y & (1 << 6)))
                UpdateLimitStatus(ribbonCheckBox_LimitYN, (m_lAxisStatus_Y & (1 << 6)) >> 6);
            if ((m_lAxisStatus_X & (1 << 5)) != (m_lAxisOldStatus_X & (1 << 5)))
                UpdateLimitStatus(ribbonCheckBox_LimitXP, (m_lAxisStatus_X & (1 << 5)) >> 5);
            if ((m_lAxisStatus_X & (1 << 6)) != (m_lAxisOldStatus_X & (1 << 6)))
                UpdateLimitStatus(ribbonCheckBox_LimitXN, (m_lAxisStatus_X & (1 << 6)) >> 6);
            if ((m_lAxisStatus_Z & (1 << 5)) != (m_lAxisOldStatus_Z & (1 << 5)))
                UpdateLimitStatus(ribbonCheckBox_LimitZP, (m_lAxisStatus_Z & (1 << 5)) >> 5);
            if ((m_lAxisStatus_Z & (1 << 6)) != (m_lAxisOldStatus_Z & (1 << 6)))
                UpdateLimitStatus(ribbonCheckBox_LimitZN, (m_lAxisStatus_Z & (1 << 6)) >> 6);
            m_lAxisOldStatus_X = m_lAxisStatus_X; //  X轴旧状态保存 
            m_lAxisOldStatus_Y = m_lAxisStatus_Y; //  y轴旧状态保存 
            m_lAxisOldStatus_Z = m_lAxisStatus_Z; //  z轴旧状态保存 
            //IO部分
            mc.GT_GetDo(mc.MC_GPO, out m_lStatusOutput);

            mc.GT_GetDi(mc.MC_GPI, out m_lStatusInput);

            if (m_lStatusInputOld != m_lStatusInput)
            {
                int index = 0;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input0, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input1, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input2, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input3, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input4, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input5, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input6, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input7, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input8, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                {
                    UpdateIOStatus(ribbonCheckBox_Input9, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                    if (ribbonCheckBox_Input9.Checked)
                    {
                        LogMessage("紧急停止中，请排除故障后复位...");
                    }
                }
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input10, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input11, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input12, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input13, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input14, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusInputOld & (1 << index)) != (m_lStatusInput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Input15, ((m_lStatusInput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                m_lStatusInputOld = m_lStatusInput;
            }


            if (m_lStatusOutputOld != m_lStatusOutput)
            {
                int index = 0;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output0, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output1, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output2, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output3, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output4, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output5, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output6, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output7, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output8, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output9, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output10, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output11, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output12, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output13, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output14, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusOutputOld & (1 << index)) != (m_lStatusOutput & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_Output15, ((m_lStatusOutput & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                m_lStatusOutputOld = m_lStatusOutput;
            }

            //原点信号
            mc.GT_GetDi(mc.MC_HOME, out m_lStatusHome);
            if (m_lStatusHome != m_lStatusHomeOld)
            {
                int index = 0;
                if ((m_lStatusHomeOld & (1 << index)) != (m_lStatusHome & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_LimitX0, ((m_lStatusHome & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusHomeOld & (1 << index)) != (m_lStatusHome & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_LimitY0, ((m_lStatusHome & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;
                if ((m_lStatusHomeOld & (1 << index)) != (m_lStatusHome & (1 << index)))
                    UpdateIOStatus(ribbonCheckBox_LimitZ0, ((m_lStatusHome & (1 << index)) >> index) == 1 ? 0 : 1);
                index++;

                m_lStatusHomeOld = m_lStatusHome;
            }
            /*******************************警告*********************************************/

            
        }


            
        private void LightChange(LightSate lightSate)
        {
            short sRtn;
            switch (lightSate)
            {
                case LightSate.RedLight:

                    //sRtn =mc.GT_SetDoBit(mc.MC_GPO,7,0);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDR_Out, 0);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDG_Out, 1);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDY_Out, 1);

                    break;
                case LightSate.GreenLight:
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDG_Out, 0);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDR_Out, 1);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDY_Out, 1);
                    break;
                case LightSate.YellowLight:
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDY_Out, 0);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDG_Out, 1);
                    sRtn = mc.GT_SetDoBit(mc.MC_GPO, LEDR_Out, 1);

                    break;
                case LightSate.Bee:
                    for (int i = 0; i < 3; i++)
                    {
                        mc.GT_SetDoBit(mc.MC_GPO, Bee_Out, 0);
                        Thread.Sleep(1000);
                        mc.GT_SetDoBit(mc.MC_GPO, Bee_Out, 1);
                    }
                
                    break;
                default:
                    break;
            }
        }
        public void UpdateLimitStatus(System.Windows.Forms.CheckBox ribbonCheckBox, long ImageIndex)//更新限位状态的显示图标
        {
            if (ImageIndex == 1)
            {
                ribbonCheckBox.Checked = true;
                mc.GT_SetDoBit(mc.MC_GPO, LASER_BIT, LASER_OFF);
                mc.GT_SetDoBit(mc.MC_GPO, VALVE_BIT, VALVE_OFF);
            }
            else
            {
                ribbonCheckBox.Checked = false;
            }
        }
        public void UpdateIOStatus(System.Windows.Forms.CheckBox ribbonCheckBox, long ImageIndex)
        {
            if (ImageIndex == 1)
            {
                ribbonCheckBox.Checked = true;
            }
            else
            {
                ribbonCheckBox.Checked = false;
            }
        }

        private void ribbonButton_SetPort_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ribbonTextBox_Port.Text))
            {
                int port = 0;
                int rt = stringToInt32(ribbonTextBox_Port.Text.ToString(), ref port);
                if (rt == 0)
                {
                   // port += 1;
                    mc.GT_SetDoBit(mc.MC_GPO, (short)port, 0); //ON
                }
            }
        }

        private void ribbonButton_UnsetPort_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ribbonTextBox_Port.Text))
            {
                int port = 0;
                int rt = stringToInt32(ribbonTextBox_Port.Text.ToString(), ref port);
                if (rt == 0)
                {
                    //port += 1;
                    mc.GT_SetDoBit(mc.MC_GPO, (short)port, 1); //OFF
                }
            }
        }
        public static int stringToInt32(string strText, ref int value)
        {
            if (!int.TryParse(strText, out value)
                || strText.Trim() == string.Empty)
            {
                // THMainForm.Instance.LogError("The value " + "\"" + strText + "\"" + " cannot be parsed");
                MessageBox.Show("TO Int Error");
                return 1;
            }
            return 0;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            if (ribbonCheckBox_Input9.Checked)
            {
                return;
            }
            onHome();
        }
        public bool CheckIsMoving()
        {
            if (CheckIsMoving(Form1.AXIS_X))       // 规划器启动  true  启动，false 停止
            {
                return true;
            }
            if (CheckIsMoving(Form1.AXIS_Y))
            {
                return true;
            }
            if (CheckIsMoving(Form1.AXIS_Z))
            {
                return true;
            }

            return false;
        }

        public void onHome()
        {
            try
            {
                if (CheckIsMoving())
                {
                    LogMessage("当前有正在运动的轴，回起点指令无效！");
                    return;
                }

                //mc.GT_SetDoBit(mc.MC_GPO, LASER_BIT, LASER_OFF);
                //mc.GT_SetDoBit(mc.MC_GPO, VALVE_BIT, VALVE_OFF);

                BackToStratPoint();
                mc.GT_CrdClear(1, 0);

                //if (cogDisplay1 != null)
                //{
                //    cogDisplay1.InteractiveGraphics.Clear();
                //    cogDisplay1.StaticGraphics.Clear();
                //}
            }
            catch (Exception ex)
            {
                string m_strError = ex.Message;
                LogMessage(m_strError);

                return;
            }
        }
        public void BackToStratPoint(int Axis = 0, double speed = 50)
        {
            short count = 1;
            uint clock;
            double x, y, z, a;
            //读取 encoder 输出值经过当量变换之后的编码器位置值
            mc.GT_GetAxisEncPos((short)Form1.AXIS_X, out x, count, out clock);//x  为获取的位置
            mc.GT_GetAxisEncPos((short)Form1.AXIS_Y, out y, count, out clock);
            mc.GT_GetAxisEncPos((short)Form1.AXIS_Z, out z, count, out clock);


            if (x != 0 && ((Axis == 0) || (Axis == (int)Form1.AXIS_X))) //NDI
            {
                MoveTrap((int)Form1.AXIS_X, (long)x * (-1), GetPlusSpeedEmpty(Form1.AXIS_X));
            }
            if (y != 0 && ((Axis == 0) || (Axis == (int)Form1.AXIS_Y)))
            {
                MoveTrap(Form1.AXIS_Y, (long)y * (-1), GetPlusSpeedEmpty(Form1.AXIS_Y));
            }
            if (z != 0 && ((Axis == 0) || (Axis == (int)Form1.AXIS_Z)))
            {
                MoveTrap(Form1.AXIS_Z, (long)z * (-1), GetPlusSpeedEmpty(Form1.AXIS_Z));
            }


        }
        public void stopAllAxes()
        {

            if (CheckIsMoving(AXIS_X))
            {
                StopCurAxis(AXIS_X, false);
            }

            if (CheckIsMoving(AXIS_Y))
            {
                StopCurAxis(AXIS_Y, false);
            }

            if (CheckIsMoving(AXIS_Z))
            {
                StopCurAxis(AXIS_Z, false);
            }
        }
        public void StopCurAxis(int Axis, bool IsFastStop = true)
        {
            int mask;
            int option = 0;
            mask = 1 << (Axis - 1);
            if (IsFastStop)
            {
                option = mask;
            }
            mc.GT_Stop(mask, option);
        }
        private void button_Stop_Click(object sender, EventArgs e)
        {
            button_Start.Enabled = true;
            ChangeState(State.Stop);
            timerIOState.Stop();
            m_CheckIO.Suspend();
            Thread_tmp = true;

            //timerRunTimer.Stop();
            //timerStartAndEndSignal.Stop();
            //m_stopWatch.Stop();
            //onStopProcess();
            stopAllAxes();
        }

        private void button_MoveToCCDPosition_Click(object sender, EventArgs e)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            double a = 0;
            int count = stringToDouble(textBox_ccdStartPosX.Text, ref x);
            count += stringToDouble(textBox_ccdStartPosY.Text, ref y);
            count += stringToDouble(textBox_ccdStartPosZ.Text, ref z);

            if (0 == count)
            {
                moveToPosition(x, y, z, a);
            }
            timer_clearBiasPos.Start();
        }
        private void moveToPosition(double positionX, double positionY, double positionZ, double positionA = 0)
        {
            double[] axisEncPos = new double[4] { 0.0, 0.0, 0.0, 0.0 };
            GetCurPos(ref axisEncPos);
            double x = axisEncPos[0];
            double y = axisEncPos[1];
            double z = axisEncPos[2];
            double a = axisEncPos[3];

            double diffX = positionX - x;
            double diffY = positionY - y;
            double diffZ = positionZ - z;
            if (Math.Abs(diffX) > m_epsilon)
            {
                moveToLocation(AXIS_X, diffX);
            }
            if (Math.Abs(diffY) > m_epsilon)
            {
                moveToLocation(AXIS_Y, diffY);
            }
            if (Math.Abs(diffZ) > m_epsilon)
            {
                moveToLocation(AXIS_Z, diffZ);
            }
            else
            {
                LogMessage("输入的位置与目前的位置相差小于0.01");
            }

        }
        public void moveToLocation(int Axis, double steplength)
        {
            if (CheckIsMoving(Axis))
            {
                LogMessage("Axis is already moving.Please wait");
            }
            double ThreadPitch = GetThreadPitch(Axis);//步进当量
            long m_lAxisPos = TransDistanceToPlus(steplength, Axis);
            double speed = GetPlusSpeedEmpty(Axis, m_nStepSpeedRatio);
            MoveTrap(Axis, m_lAxisPos, speed);
        }

        private void timer_clearBiasPos_Tick(object sender, EventArgs e)
        {
            if (!CheckIsMoving())
            {
                OnButtonLocationClear();
                timer_clearBiasPos.Stop();
            }
        }

        private void ribbonButton_backToZero_Click(object sender, EventArgs e)
        {
            if (!ribbonCheckBox_Input9.Checked)
            {
                DialogResult res = MessageBox.Show("是否确定要回零？", "回零确认", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel)
                {
                    return;
                }

                BackHome m_DlgBackHome = new BackHome();
                if (DialogResult.OK == m_DlgBackHome.ShowDialog())
                {
                    OnButtonLocationClear();
                }

            }
        }
        public short GetBackHomeDir(int Axis)
        {
            short dir = 0;
            switch (Axis)
            {
                case (int)Form1.AXIS_X:
                    dir = (short)Form1.m_nComboBackHomeDirX;
                    break;
                case (int)Form1.AXIS_Y:
                    dir = (short)Form1.m_nComboBackHomeDirY;
                    break;
                case (int)Form1.AXIS_Z:
                    dir = (short)Form1.m_nComboBackHomeDirZ;
                    break;

            }
            switch (dir)
            {
                case 0:
                    dir = +1;
                    break;
                case 1:
                    dir = 0;
                    break;
                case 2:
                    dir = -1;
                    break;
            }

            return dir;
        }

        private void button_Ypos_Click_MouseDown(object sender, MouseEventArgs e)
        {
            StepManual(AXIS_Y, true);
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            Cricle();

        }
        void Cricle()
        {
            mc.TCrdPrm crdPrm = new mc.TCrdPrm();
            short sRtn;
            // 坐标系运动状态查询变量  
            short run;
            // 坐标系运动完成段查询变量 
            int segment;
            int space;
            crdPrm.dimension = 2;  // 坐标系为二维坐标系  
            crdPrm.synVelMax = 500;  // 最大合成速度：500pulse/ms 
            crdPrm.synAccMax = 1;   // 最大加速度：1pulse/ms^2  
            crdPrm.evenTime = 50;   // 最小匀速时间：50ms  

            crdPrm.profile1 = 1;   // 规划器1对应到X轴  
            crdPrm.profile2 = 2;   // 规划器2对应到Y轴 
            crdPrm.setOriginFlag = 1;  // 表示需要指定坐标系的原点坐标的规划位置  
            crdPrm.originPos1 = 100;  // 坐标系的原点坐标的规划位置为（100, 100）  
            crdPrm.originPos2 = 100;

            double Circle_center= Convert.ToInt32(textBox_circle_Center.Text);
            int Circle_Start= Convert.ToInt32(textBox_circle_Start.Text);

            long m_lAxisPos_Circle_center = TransDistanceToPlus(Circle_center, AXIS_X);
            int m_lAxisPos_circle_Start  =(int)TransDistanceToPlus(Circle_Start, AXIS_X);

            sRtn = mc.GT_SetCrdPrm(1, ref crdPrm);
            sRtn = mc.GT_CrdClear(1, 0);
            sRtn = mc.GT_ArcXYC(1, m_lAxisPos_circle_Start, 0, m_lAxisPos_Circle_center, 0, 0, 100, 0.1, 0, 0);
            sRtn = mc.GT_CrdSpace(1, out space, 0);
            sRtn = mc.GT_CrdStart(1, 0);
            do
            {   // 查询坐标系1的FIFO的插补运动状态  
                // 坐标系是坐标系1  // 读取插补运动状态// 读取当前已经完成的插补段数  
                sRtn = mc.GT_CrdStatus(1, out run, out segment, 0);   // 查询坐标系1的FIFO0缓存区  // 坐标系在运动, 查询到的run的值为1 
            } while (run == 1);
        }
        private void button_Start_Click(object sender, EventArgs e)
        {
            ChangeState(State.Working);
            StartTest();
            if(Thread_tmp)
            {
                m_CheckIO.Resume();
            }
        }
        public void StartTest()
        {
            this.Invoke((Action)delegate ()
            {
                button_Start.Enabled = false;
            });
           
            bool frist = true, END = false;
            short sRtn;
           

            Task.Factory.StartNew(new Action(() =>
            {
                while (m_CurretState == State.Working)
                {
                    mc.GT_SetDoBit(mc.MC_GPO, Door_Open_Out, 1);
                    mc.GT_SetDoBit(mc.MC_GPO, Door_Close_Out, 0);
                    switch (step)
                    {
                        case 5:
                            //sRtn = mc.GT_ClrSts(1, 8);
                            //sRtn = mc.GT_AxisOn(AXIS_X);
                            //sRtn = mc.GT_AxisOn(AXIS_Y);
                            //sRtn = mc.GT_AxisOn(AXIS_Z);

                            step = 10;
                            break;
                        case 10:
                            //collector_once();

                            //step = 45;
                            //onHome();
                            step = 15;
                            LogMessage("回到原点");
                            break;
                        case 15:
                            //移动到拍照位置
                            //Point("");
                            LogMessage("移动到（1）拍照位置");
                            step = 20;
                            break;

                        case 20:
                            //进行定位

                            Location_Blob();
                            LogMessage("进行定位");
                            if (NO_Pic)
                            {
                                step = 100;

                            }
                            else if (frist == true)
                            {
                                if (trimm)
                                {
                                    step = 25;
                                }
                                else
                                {
                                    step = 30;
                                }
                            }
                            else
                            {
                                if (trimm)
                                {
                                    step = 45;
                                }
                                else
                                {
                                    step = 50;
                                }
                            }
                            
                            break;
                        case 25:
                            //移动到焊接位置
                           // Point("");
                            LogMessage("移动到（1）焊接位置");
                            step = 35;
                            break;
                        case 30:
                            //进行微调
                            trimming();
                           // Point("");
                            LogMessage("定位不准确，进行微调");
                            step = 25;
                            break;
                        case 35:
                            //进行焊接圆
                            //Cricle();

                            if (END)
                            {
                                //Point("");//结束退出
                                LogMessage("焊接完成，准备退出");
                                ChangeState(State.Waitting);
                                step = 5;
                            }
                            else
                            {
                                LogMessage("第一个焊接完成");
                                step = 40;

                            }
                            break;
                        case 40:
                            //移动到第二个拍照点
                           // Point("");
                            LogMessage("移动第二个到拍照点");
                            step = 20;
                            frist = false;
                            break;
                        case 45:
                            //第二个移动到焊接位置
                            //Point("");
                            LogMessage("第二次运动到焊接点");
                            step = 35;
                            END = true;

                            break;
                        case 50:
                            //进行（2）微调
                            trimming();
                            LogMessage("第二次微调");
                            step = 45;
                            break;
                        case 100:
                            LogMessage("定位失败");
                            ChangeState(State.Error);
                            break;

                    }
                }
            }));
        }
        private void ChangeState(State machinestate)
        {
            m_CurretState = machinestate;
            this.Invoke(new Action(() => {
                switch (machinestate)
                {
                    case State.Working:
                        LightChange(LightSate.GreenLight);
                        break;
                    case State.Waitting:
                        LightChange(LightSate.GreenLight);
                        break;
                    case State.Stop:

                        LightChange(LightSate.RedLight);
                        break;
                    case State.Error:
                        LightChange(LightSate.RedLight);
                        break;
                   
                    default:
                        break;
                }

            }));

        }

        private void button_Laser_Click(object sender, EventArgs e)
        {
            short retn;
            if (!isLaserChecked)
            {
                retn = mc.GT_SetDoBit(mc.MC_GPO, Laser_Out, VALVE_ON);
                isLaserChecked = true;
                button_Laser.Text = "激光器已开";
            }
            else
            {
                retn = mc.GT_SetDoBit(mc.MC_GPO, Laser_Out, VALVE_OFF);
                isLaserChecked = false;
                button_Laser.Text = "激光器已关";
            }
        }

        

        private void button_Gas_Click(object sender, EventArgs e)
        {
            short retn;
            if (!isGasChecked)
            {
                retn = mc.GT_SetDoBit(mc.MC_GPO, Gas_Out, VALVE_ON);
                isGasChecked = true;
                button_Gas.Text = "气阀已开";
            }
            else
            {
                retn = mc.GT_SetDoBit(mc.MC_GPO, Gas_Out, VALVE_OFF);
                isGasChecked = false;
                button_Gas.Text = "气阀已关";
            }
        }

        private void button_point_Click(object sender, EventArgs e)
        {
            Point(textBox_Point.Text);
        }
        public void Point(string str_length)
        {
            short sRtn;
            int length_Value;
            double length;
            sRtn = mc.GT_ClrSts(1, 8);
            sRtn = mc.GT_AxisOn(AXIS_X);
            sRtn = mc.GT_ZeroPos(AXIS_X);
            // AXIS轴规划位置清零 
            sRtn = mc.GT_SetPrfPos(AXIS_X, 0);
            // 将AXIS轴设为点位模式  
            sRtn = mc.GT_PrfTrap(AXIS_X);
            // 读取点位运动参数 
            mc.TTrapPrm trap = new mc.TTrapPrm();
            trap = GetTrap(AXIS_X);

            sRtn = mc.GT_GetTrapPrm(AXIS_X, out trap);
            trap.acc = 0.25;
            trap.dec = 0.125;
            trap.smoothTime = 25;
            sRtn = mc.GT_SetTrapPrm(AXIS_X, ref trap);

            //int length = Convert.ToInt32(textBox_Point.Text);
             length = Convert.ToDouble(str_length);
            
            length_Value = (int)TransDistanceToPlus(length, AXIS_X);
            sRtn = mc.GT_SetPos(AXIS_X, length_Value);

            // 设置AXIS轴的目标速度 
            sRtn = mc.GT_SetVel(AXIS_X, 50);
            // 启动AXIS轴的运动 
            sRtn = mc.GT_Update(1 << (AXIS_X - 1));
        }

        private void trackBar_StepSpeed_ValueChanged(object sender, EventArgs e)
        {
            TrackBar slide = sender as TrackBar;
            SpeedValue.Text = slide.Value.ToString();
            int value = 1;
            int status =stringToInt32(SpeedValue.Text.ToString(), ref value);
            if (status == 0)
            {
                m_nStepSpeedRatio = value;
            }
        }


        private void SpeedValue_TextChanged(object sender, EventArgs e)
        {
            int value = 1;
            int status = stringToInt32(SpeedValue.Text.ToString(), ref value);
            if (status == 0)
            {
                if (value >= trackBar_StepSpeed.Minimum && value <= trackBar_StepSpeed.Maximum)
                {
                    trackBar_StepSpeed.Value = value;
                    m_nStepSpeedRatio = value;
                }
                else
                {
                    LogMessage("Invalid entry!!");
                }
            }
        }
        /***************************************CCD******************************************************/
        private void button_OpenCamera_Click(object sender, EventArgs e)
        {
            if (button_OpenCamera.Text == "打开相机")
            {
                button_OpenCamera.Text = "关闭相机";
                Thread.Sleep(500);
                OpenCamera();
            }
            else
            {
                button_OpenCamera.Text = "打开相机";
                StopCamera();
            }
            //OpenCamera();      
        }
        

        private void button_con_collector_Click(object sender, EventArgs e)
        {
            collector();
        }
        private void collector()
        {
            m_bShow = true;
            m_ShowImage = new Thread(new ThreadStart(ShowImage));
            m_ShowImage.IsBackground = true;
            m_ShowImage.Start();
        }
     
        public static void OpenCamera()
        {
            hv_AcqHandle = null;
            try
            {
                //自带相机采图
               // HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, "false", "default", "Cam11", 0, -1, out hv_AcqHandle);
                HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", "default", 0, -1, out hv_AcqHandle);

                if (hv_AcqHandle)
                    HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                else
                    return;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void StopCamera()
        {
            try
            {
                if (m_ShowImage != null)
                {
                    m_ShowImage.Abort();
                }
                if (hv_AcqHandle != null)
                {
                    HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                    hv_AcqHandle = null;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_location_Click(object sender, EventArgs e)
        {
            //location_ROI();
            Location_Blob();
            trimming();//微调
        }
        public void trimming()
        {
            this.Invoke((Action)delegate ()
            {
                textBox_stepInput.Text = Math.Abs(x_change).ToString();
            });
           
            if (x_change > 0.1)
            {
                StepManual(AXIS_X, true);
            }
            else if (x_change < -0.1)
            {
                StepManual(AXIS_X, false);
            }
            else
            {
                trimm = true;
            }
            textBox_stepInput.Text = Math.Abs(y_change).ToString();
            if (y_change > 0)
            {
                StepManual(AXIS_Y, true);
            }
            else
            {
                StepManual(AXIS_Y, false);
            }

        }

        public static void SnapImage()
        {
            try
            {
                if (hv_AcqHandle != null)
                {
                    HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                } 
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int i = 0;
        private void button_collector_Click(object sender, EventArgs e)
        {


            //MessageBox.Show(ImagePath);
            collector_once();

        }

        private void button_home_Click(object sender, EventArgs e)
        {
            home_pos();
        }
        public void home_pos()
        {
            short sRtn, capture;
            int status, pos;
            uint clock;
            mc.TTrapPrm trapPrm;
            double prfPos, encPos, axisPrfPos, axisEncPos;
            sRtn = mc.GT_SetCaptureMode(AXIS_X, 20000);//搜索距离
            sRtn = mc.GT_PrfTrap(AXIS_X);
            sRtn = mc.GT_GetTrapPrm(AXIS_X, out trapPrm);
            trapPrm.acc = 0.25;
            trapPrm.dec = 0.25;
            sRtn = mc.GT_SetTrapPrm(AXIS_X, ref trapPrm);
            sRtn = mc.GT_SetVel(AXIS_X, 10);
            sRtn = mc.GT_Update(1 << (AXIS_X - 1));
            do
            {
                sRtn = mc.GT_GetSts(AXIS_X, out status, 1, out clock);
                sRtn = mc.GT_GetCaptureStatus(AXIS_X, out capture, out pos, 1, out clock);
                sRtn = mc.GT_GetPrfPos(AXIS_X, out prfPos, 1, out clock);
                sRtn = mc.GT_GetEncPos(AXIS_X, out encPos, 1, out clock);
                if (0 == (status & 0x400))
                {
                    LogMessage("no home found");

                }
                Thread.Sleep(500);
            } while (0 != capture);
            LogMessage("捕获位置" + pos);
            sRtn = mc.GT_SetPos(AXIS_X, pos + 2000);//设置捕获+偏移距离
            sRtn = mc.GT_Update(1 << (AXIS_X - 1));
            do
            {
                sRtn = mc.GT_GetSts(AXIS_X, out status, 1, out clock);
                sRtn = mc.GT_GetPrfPos(AXIS_X, out prfPos, 1, out clock);
                sRtn = mc.GT_GetEncPos(AXIS_X, out encPos, 1, out clock);
            }
            while ((status & 0x400) == 1);
            if (prfPos != pos + 2000)
            {
                LogMessage("move to home pos error");
                
            }
            sRtn = mc.GT_ZeroPos(AXIS_X);
            sRtn = mc.GT_GetPrfPos(AXIS_X, out prfPos, 1, out clock);
            sRtn = mc.GT_GetEncPos(AXIS_X, out encPos, 1, out clock);
            sRtn = mc.GT_GetAxisPrfPos(AXIS_X, out axisPrfPos, 1, out clock);
            sRtn = mc.GT_GetAxisEncPos(AXIS_X, out axisEncPos, 1, out clock);

        }

        private void button_autoHome_Click(object sender, EventArgs e)
        {
            short rtn;
            ushort status;
            rtn = mc.GT_ClrSts(1, 8);     // 清楚状态 
            rtn = mc.GT_HomeInit();     // 初始化自动回原点功能 
            rtn = mc.GT_AxisOn(1);     // 使能轴1  
            rtn = mc.GT_AxisOn(2);     // 使能轴2
            rtn = mc.GT_Index(1, 20000, 2000);   // 轴1为Home+Index回零模式
            rtn = mc.GT_Home(1, 200000, 50, 0.5, 2000);
            rtn = mc.GT_Home(2, 200000, 50, 0.5, 3000); // 轴2为Home回零模式 

            while (true)
            {
                mc.GT_HomeSts(AXIS_X, out status);
                if(status==1)
                {
                    break;
                }
            }
               

        }

        private void button_xneg_Click_MouseDown(object sender, MouseEventArgs e)
        {
            StepManual(AXIS_X, false);
        }

        private void button_xpos_Click_MouseDown(object sender, MouseEventArgs e)
        {
            StepManual(AXIS_X, true);
        }

        private void button_Yneg_Click_MouseDown(object sender, MouseEventArgs e)
        {
            StepManual(AXIS_Y, false);
        }

        private void button_ZPos_Click_MouseDown(object sender, MouseEventArgs e)
        {
            StepManual(AXIS_Z, true);
        }

        private void button_Zneg_Click_MouseDown(object sender, MouseEventArgs e)
        {
            StepManual(AXIS_Z, false);
        }

        public void collector_once()
        {
            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }
            ImageFile = ImagePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            // ImageFile = ImagePath + "\\" + DateTime.Now.ToString();
            //i++;
            if (hv_AcqHandle == null)
            {
                MessageBox.Show("Pls open the camera first!", "Error", MessageBoxButtons.OKCancel);
                return;
            }
            try
            {
                SnapImage();
                //HOperatorSet.ReadImage(out ho_Image, @"C:\Users\admin\Pictures\0.jpg");
                HOperatorSet.GetImageSize(ho_Image, out m_Width, out m_Height);
                HOperatorSet.WriteImage(ho_Image, "png", 0, ImageFile);
                HOperatorSet.SetWindowAttr("background_color", "black");
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, m_Height, m_Width);
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button__Click(object sender, EventArgs e)
        {
            m_bShow = false;
        }

        public  void ShowImage()
        {
            try
            {
                if (hv_AcqHandle == null)
                {
                    MessageBox.Show("Pls open the camera first!", "Error", MessageBoxButtons.OKCancel);
                    return;
                }
                while (m_bShow)
                {
                   
                    SnapImage();
                    HOperatorSet.GetImageSize(ho_Image, out m_Width, out m_Height);
                    HOperatorSet.SetWindowAttr("background_color", "black");
                    HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, m_Height - 1, m_Width - 1);
                  
                    HOperatorSet.SetSystem("flush_graphic", "false");
                    HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                    HOperatorSet.SetSystem("flush_graphic", "true");
                  
                    Thread.Sleep(120);
                   
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void location_ROI()
        {
            double x_collector = 0;
            double y_collector = 0;

            HTuple hv_ModelID = null, hv_ModelRegionArea = null;
            HTuple hv_RefRow = null, hv_RefColumn = null, hv_HomMat2D = null;
            HTuple hv_TestImages = null, hv_T = null, hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
            HTuple hv_Score = new HTuple(), hv_I = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, Modle_Image);

            HOperatorSet.GenEmptyObj(out ho_ModelRegion);
            ho_ModelRegion.Dispose();

            // HOperatorSet.GenCircle(out ho_ModelRegion, 300.427, 320.277, 38.9934);
            HOperatorSet.GenCircle(out ho_ModelRegion, 524.3, 659.127, 237.711);
           

            HOperatorSet.GenEmptyObj(out ho_TemplateImage);
            ho_TemplateImage.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_ModelRegion, out ho_TemplateImage);
            HOperatorSet.CreateShapeModel(ho_TemplateImage, 4, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), (new HTuple(2.979)).TupleRad(), (new HTuple("none")).TupleConcat(
        "no_pregeneration"), "use_polarity", ((new HTuple(5)).TupleConcat(26)).TupleConcat(
        4), 3, out hv_ModelID);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);

            ho_ModelContours.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
            //
            //Matching 01: Get the reference position
            HOperatorSet.AreaCenter(ho_ModelRegion, out hv_ModelRegionArea, out hv_RefRow,
                out hv_RefColumn);
            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RefRow, hv_RefColumn, 0, out hv_HomMat2D);
            HOperatorSet.GenEmptyObj(out ho_TransContours);
            ho_TransContours.Dispose();
            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours, hv_HomMat2D);
            double X_value = hv_RefRow;
            double Y_value = hv_RefColumn;



            //hv_TestImages = "D:/TestWork/新建文件夹/YFK - addCCD_X64/YFK/bin/x64/Debug/Picture/2019-06-26-14-45-34.png";
            hv_TestImages = ImageFile + ".png";
            for (hv_T = 0; (int)hv_T <= 0; hv_T = (int)hv_T + 1)
            {
                //
                //Matching 01: Obtain the test image
                ho_Image.Dispose();
                HOperatorSet.ReadImage(out ho_Image, hv_TestImages.TupleSelect(hv_T));
                //
                //Matching 01: Find the model
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), 0.4, 1, 0.5, "least_squares", (new HTuple(4)).TupleConcat(
                    1), 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                //
                //Matching 01: Transform the model contours into the detected positions
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2D);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2D, hv_Angle.TupleSelect(hv_I), 0, 0,
                        out hv_HomMat2D);
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2D, hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(
                        hv_I), out hv_HomMat2D);
                    ho_TransContours.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_TransContours,
                        hv_HomMat2D);
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        //   HOperatorSet.DispObj(ho_TransContours, HDevWindowStack.GetActive());
                        HOperatorSet.DispObj(ho_TransContours, hWindowControl1.HalconWindow);
                    }
                    // stop(...); only in hdevelop
                }
            }
            //if(hv_Row!=""&& hv_Column!="")
           if(hv_Row!=null&& hv_Column!=null)
            {
                 x_collector = hv_Row;
                 y_collector = hv_Column;
                x_change = X_value - x_collector;
                y_change = Y_value - y_collector;
                textBox_Xchange.Text = x_change.ToString();
                textBox_Ychange.Text = y_change.ToString();
            }
            

            HOperatorSet.ClearShapeModel(hv_ModelID);
            ho_Image.Dispose();
            ho_ModelRegion.Dispose();
            ho_TemplateImage.Dispose();
            ho_ModelContours.Dispose();
            ho_TransContours.Dispose();
            x_change = 0;
            X_value  = 0;
            x_collector = 0;
            y_change = 0;
            Y_value  = 0;
            y_collector = 0;
        }
        void Location_Blob()
        {
            HObject ho_Image, ho_Region, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionFillUp, ho_RegionDilation;
            HObject ho_ImageReduced, ho_Model, ho_ModelTrans, ho_ImageSearch;

            HTuple hv_TestImages = null;
           HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = null;
            HTuple hv_ModelID = null, hv_Area = null, hv_RowRef = null;
            HTuple hv_ColumnRef = null, hv_HomMat2D = null, hv_Row = null;
            HTuple hv_Column = null, hv_Angle = null, hv_Scale = null;
            HTuple hv_Score = null, hv_I = null, hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple();

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Model);
            HOperatorSet.GenEmptyObj(out ho_ModelTrans);
            HOperatorSet.GenEmptyObj(out ho_ImageSearch);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, Modle_Image);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_Image, out ho_Region, 80, 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 140000, 150000);
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationCircle(ho_RegionFillUp, out ho_RegionDilation, 5.5);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation, out ho_ImageReduced);
            HOperatorSet.CreateScaledShapeModel(ho_ImageReduced, 5, (new HTuple(-45)).TupleRad()
                , (new HTuple(90)).TupleRad(), "auto", 0.8, 1.0, "auto", "none", "ignore_global_polarity",
                40, 10, out hv_ModelID);
            ho_Model.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_Model, hv_ModelID, 1);
            HOperatorSet.AreaCenter(ho_ImageReduced, out hv_Area, out hv_RowRef, out hv_ColumnRef);
            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_RowRef, hv_ColumnRef, 0, out hv_HomMat2D);
            double X_value = hv_RowRef;
            double Y_value = hv_ColumnRef;

            ho_ModelTrans.Dispose();
            HOperatorSet.AffineTransContourXld(ho_Model, out ho_ModelTrans, hv_HomMat2D);
            ho_ImageSearch.Dispose();
            HOperatorSet.ReadImage(out ho_ImageSearch, @"D:/TestWork/新建文件夹/YFK - addCCD_X32/YFK/bin/x86/Debug/Picture/2019-06-27-14-04-14.png");
            hv_TestImages = ImageFile + ".png";
            //HOperatorSet.ReadImage(out ho_ImageSearch, hv_TestImages);

            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_ImageSearch, HDevWindowStack.GetActive());
            }
            HOperatorSet.FindScaledShapeModel(ho_ImageSearch, hv_ModelID, (new HTuple(-45)).TupleRad()
                , (new HTuple(90)).TupleRad(), 0.8, 1.0, 0.5, 0, 0.5, "least_squares", 5,
                0.8, out hv_Row, out hv_Column, out hv_Angle, out hv_Scale, out hv_Score);
            for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_I = (int)hv_I + 1)
            {
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row.TupleSelect(hv_I),
                    hv_Column.TupleSelect(hv_I), out hv_HomMat2DTranslate);
                HOperatorSet.HomMat2dRotate(hv_HomMat2DTranslate, hv_Angle.TupleSelect(hv_I),
                    hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(hv_I), out hv_HomMat2DRotate);
                HOperatorSet.HomMat2dScale(hv_HomMat2DRotate, hv_Scale.TupleSelect(hv_I), hv_Scale.TupleSelect(
                    hv_I), hv_Row.TupleSelect(hv_I), hv_Column.TupleSelect(hv_I), out hv_HomMat2DScale);
                ho_ModelTrans.Dispose();
                HOperatorSet.AffineTransContourXld(ho_Model, out ho_ModelTrans, hv_HomMat2DScale);
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_ModelTrans, HDevWindowStack.GetActive());
                }
            }
            try
            {
                if (hv_I > 0)
                {
                    double x_collector = hv_Row.D;
                    double y_collector = hv_Column.D;
                    double x_change = X_value - x_collector;
                    double y_change = Y_value - y_collector;
                    this.Invoke((Action)delegate ()
                    {
                        textBox_Xchange.Text = x_change.ToString();
                        textBox_Ychange.Text = y_change.ToString();
                    });
                    //MessageBox.Show(x_change.ToString());
                    //MessageBox.Show(y_change.ToString());
                }
                else
                {
                    NO_Pic = true;
                }

             
            }
            catch (Exception e)
            {
                step = 100;
                MessageBox.Show(e.ToString());
                //Log.WriteInfo(e);
            }

           
          
            //MessageBox.Show(x_change.ToString());
            //MessageBox.Show(y_change.ToString());
            HOperatorSet.ClearShapeModel(hv_ModelID);
            ho_Image.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionDilation.Dispose();
            ho_ImageReduced.Dispose();
            ho_Model.Dispose();
            ho_ModelTrans.Dispose();
            ho_ImageSearch.Dispose();

        }

        /**********************************************END*****************************************************/
    }
}
