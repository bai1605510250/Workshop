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

namespace YFK
{
    public partial class BackHome : Form
    {

        Form1 m_MovementMode = new Form1();
        public delegate void LogAppendDelegate(Color color, string text);

        int m_nBackHomeCount = 0;
        int nPriority;
        bool IsAxisBackHomeFinish = false;
        readonly int HOME_OFFSET = 2000;
        int[] sBackHomeSatus = new int[8];
        bool[] IsBackHomeReverseflag = new bool[8];
        public BackHome()
        {
            InitializeComponent();
            IsAxisBackHomeFinish = false;
            initData();

        }
        public void initData()
        {
            if (m_MovementMode.CheckIsMoving())
            {
                LogMessage("存在运动的轴，回零指令无效！", 1);
                return;
            }
            button_ok.Enabled = false;
            button_Cancel.Enabled = false;
            //LogMessage("回零开始。", 1);

            timerBackHome.Start();
           // timerWeldingProgress.Start();

            for (int i = 0; i < 8; ++i)
            {
                IsBackHomeReverseflag[i] = false;
                sBackHomeSatus[i] = 0;
            }
            nPriority = Form1.m_nRadioBackHomePriority;//回零优先
            BackHomeInit(nPriority);
        }


        public void BackHomeInit(int nPriority)
        {
            if (nPriority == 0)
            {
                BackHomeInitX(false);
                BackHomeInitY(false);
                BackHomeInitZ(false);
              
            }
            else if (nPriority == 1)
            {
                BackHomeInitX(false);
                timerPriority.Start();
            }
            else if (nPriority == 2)
            {
                BackHomeInitY(false);
                timerPriority.Start();
            }
            else if (nPriority == 3)
            {
                BackHomeInitZ(false);
                timerPriority.Start();
            }
            
        }
        short BackHomeStart(int AXIS, bool IsReverse)
        {
            short dir = m_MovementMode.GetBackHomeDir(AXIS);
            if (0 == dir)
            {
                return dir;
            }

            short sRtn;
            double dis;
            sRtn = mc.GT_ZeroPos((short)AXIS);// 位置清零
            if (IsReverse)//设置捕获电平
                sRtn = mc.GT_SetCaptureSense((short)AXIS, mc.CAPTURE_HOME, 1);
            else
            {
                sRtn = mc.GT_SetCaptureSense((short)AXIS, mc.CAPTURE_HOME, 0);
            }
            sRtn = mc.GT_SetCaptureMode((short)AXIS, mc.CAPTURE_HOME);// 启动Home捕获
            dis = GetMaxDitance(AXIS);//最大的回零
            dis *= dir;//方向

            if (IsReverse)//反向回零
            {
                dis *= -1;
            }
            m_MovementMode.MoveBackHome(AXIS, dis);

            return sRtn;
        }
        public double GetMaxDitance(int Axis)
        {
            return (double)Form1.setValue.m_nMaxDiatance; ;//²ÉÓÃ¶¨³¤800mm
        }


        public void BackHomeInitX(bool IsReverse)
        {
            short sRtn;
            sRtn = BackHomeStart(Form1.AXIS_X, IsReverse);
            String msg = "";
            if (IsReverse)//反向回零开始
            {
                switch (sRtn)
                {
                    case +1:
                        msg = "X轴反向回零开始,回零方向:-1\r\n";
                        break;
                    case -1:
                        msg = "X轴反向回零开始,回零方向:+1\r\n";
                        break;
                }
            }
            else
            {
                switch (sRtn)
                {
                    case +1:
                        msg = "X轴回零开始,回零方向:+1\r\n";
                        break;
                    case 0:
                        msg = "X轴回零开始,回零方向:0\r\n";
                        sBackHomeSatus[Form1.AXIS_X - 1] = 1;//表示该轴回零结束
                        break;
                    case -1:
                        msg = "X轴回零开始,回零方向:-1\r\n";
                        break;
                }
            }

            LogMessage(msg, 1);
            if (sRtn == 1)
            {
                //创建一个线程
                IsBackHomeReverseflag[Form1.AXIS_X - 1] = IsReverse;
                Thread myNewThread = new Thread(() => CheckProcX());
                myNewThread.IsBackground = true;
                myNewThread.Start();
            }

        }


        public void BackHomeInitY(bool IsReverse)
        {
            short sRtn;
            sRtn = BackHomeStart(Form1.AXIS_Y, IsReverse);

            String msg = "";
            if (IsReverse)
            {
                switch (sRtn)
                {
                    case +1:
                        msg = "Y轴反向回零开始,回零方向:-1\r\n";
                        break;
                    case -1:
                        msg = "Y轴反向回零开始,回零方向:+1\r\n";
                        break;
                }
            }
            else
            {
                switch (sRtn)
                {
                    case +1:
                        msg = "Y轴回零开始,回零方向:+1\r\n";
                        break;
                    case 0:
                        msg = "Y轴回零开始,回零方向:0\r\n";
                        sBackHomeSatus[Form1.AXIS_Y - 1] = 1;//表示该轴回零结束
                        break;
                    case -1:
                        msg = "Y轴回零开始,回零方向:-1\r\n";
                        break;
                }
            }

            LogMessage(msg, 1);
            if (sRtn == 1)
            {
                //创建一个线程
                IsBackHomeReverseflag[Form1.AXIS_Y - 1] = IsReverse;
                Thread myNewThread = new Thread(() => CheckProcY());
                myNewThread.IsBackground = true;
                myNewThread.Start();
            }

        }


        public void BackHomeInitZ(bool IsReverse)
        {
            short sRtn;
            sRtn = BackHomeStart(Form1.AXIS_Z, IsReverse);

            String msg = "";
            if (IsReverse)
            {
                switch (sRtn)
                {
                    case +1:
                        msg = "Z轴反向回零开始,回零方向:-1\r\n";
                        break;
                    case -1:
                        msg = "Z轴反向回零开始,回零方向:+1\r\n";
                        break;
                }
            }
            else
            {
                switch (sRtn)
                {
                    case +1:
                        msg = "Z轴回零开始,回零方向:+1\r\n";
                        break;
                    case 0:
                        msg = "Z轴回零开始,回零方向:0\r\n";
                        sBackHomeSatus[Form1.AXIS_Z - 1] = 1;//表示该轴回零结束
                        break;
                    case -1:
                        msg = "Z轴回零开始,回零方向:-1\r\n";
                        break;
                }
            }

            LogMessage(msg, 1);
            if (sRtn == 1)
            {
                //创建一个线程
                IsBackHomeReverseflag[Form1.AXIS_Z - 1] = IsReverse;
                Thread myNewThread = new Thread(() => CheckProcZ());
                myNewThread.IsBackground = true;
                myNewThread.Start();
            }

        }
    


        public void CheckProcX()
        {
            short sRtn = 0;
            short capture;
            int status, pos;
            double prfPos;
            do
            {
                short count = 1;
                uint clock = 0;
                sRtn = mc.GT_GetSts(Form1.AXIS_X, out status, count, out clock);                      // 读取轴状态
                sRtn = mc.GT_GetCaptureStatus(Form1.AXIS_X, out capture, out pos, count, out clock);     // 读取捕获状态
                if (0 == (status & 0x400))
                {
                    sBackHomeSatus[Form1.AXIS_X - 1] = 21;
                    return;
                }
            } while (0 == capture);

            int offset = HOME_OFFSET;
            if (pos > 0)
            {
                offset *= -1;
            }
            if (IsBackHomeReverseflag[Form1.AXIS_X - 1])
            {
                offset *= -1;
            }
            pos += offset;
            pos *= (int)(Form1.setValue.PlanXA_Value / Form1.setValue.PlanXB_Value); 

            sRtn = mc.GT_SetPos((short)Form1.AXIS_X, pos);// 运动到"捕获位置+偏移量"
            sRtn = mc.GT_SetVel(Form1.AXIS_X, 20);
            sRtn = mc.GT_Update(1 << (Form1.AXIS_X - 1));// 在运动状态下更新目标位置
            do
            {
                short count = 1;
                uint clock = 0;
                sRtn = mc.GT_GetSts(Form1.AXIS_X, out status, count, out clock);
                sRtn = mc.GT_GetPrfPos(Form1.AXIS_X, out prfPos, count, out clock);
            } while ((status & 0x400) != 0);// 

            Thread.Sleep(50);
            sBackHomeSatus[Form1.AXIS_X - 1] = 20;//
            sRtn = mc.GT_ZeroPos(Form1.AXIS_X);
            return;
        }
        public void CheckProcY()
        {
            short sRtn = 0;
            short capture;
            int status, pos;
            double prfPos;
            do
            {
                short count = 1;
                uint clock = 0;
                sRtn = mc.GT_GetSts(Form1.AXIS_Y, out status, count, out clock);                      // 读取轴状态
                sRtn = mc.GT_GetCaptureStatus(Form1.AXIS_Y, out capture, out pos, count, out clock);     // 读取捕获状态
                if (0 == (status & 0x400))
                {
                    sBackHomeSatus[Form1.AXIS_Y - 1] = 21;
                    return;
                }
            } while (0 == capture);

            int offset = HOME_OFFSET;
            if (pos > 0)
            {
                offset *= -1;
            }
            if (IsBackHomeReverseflag[Form1.AXIS_Y - 1])
            {
                offset *= -1;
            }
            pos += offset;
            pos *= (int)(Form1.setValue.PlanYA_Value / Form1.setValue.PlanYB_Value);

            sRtn = mc.GT_SetPos((short)Form1.AXIS_Y, pos);// 运动到"捕获位置+偏移量"
            sRtn = mc.GT_SetVel(Form1.AXIS_Y, 20);
            sRtn = mc.GT_Update(1 << (Form1.AXIS_Y - 1));// 在运动状态下更新目标位置
            do
            {
                short count = 1;
                uint clock = 0;
                sRtn = mc.GT_GetSts(Form1.AXIS_Y, out status, count, out clock);
                sRtn = mc.GT_GetPrfPos(Form1.AXIS_Y, out prfPos, count, out clock);
            } while ((status & 0x400) != 0);// 

            Thread.Sleep(50);
            sBackHomeSatus[Form1.AXIS_Y - 1] = 20;//
            sRtn = mc.GT_ZeroPos(Form1.AXIS_Y);
            return;
        }
        public void CheckProcZ()
        {
            short sRtn = 0;
            short capture;
            int status, pos;
            double prfPos;
            do
            {
                short count = 1;
                uint clock = 0;
                sRtn = mc.GT_GetSts(Form1.AXIS_Z, out status, count, out clock);                      // 读取轴状态
                sRtn = mc.GT_GetCaptureStatus(Form1.AXIS_Z, out capture, out pos, count, out clock);     // 读取捕获状态
                if (0 == (status & 0x400))
                {
                    sBackHomeSatus[Form1.AXIS_Z - 1] = 21;
                    return;
                }
            } while (0 == capture);

            int offset = HOME_OFFSET;
            if (pos > 0)
            {
                offset *= -1;
            }
            if (IsBackHomeReverseflag[Form1.AXIS_Z - 1])
            {
                offset *= -1;
            }
            pos += offset;
            pos *= (int)(Form1.setValue.PlanZA_Value / Form1.setValue.PlanZB_Value);
            sRtn = mc.GT_SetPos((short)Form1.AXIS_Z, pos);// 运动到"捕获位置+偏移量"
            sRtn = mc.GT_SetVel(Form1.AXIS_Z, 20);
            sRtn = mc.GT_Update(1 << (Form1.AXIS_Z - 1));// 在运动状态下更新目标位置
            do
            {
                short count = 1;
                uint clock = 0;
                sRtn = mc.GT_GetSts(Form1.AXIS_Z, out status, count, out clock);
                sRtn = mc.GT_GetPrfPos(Form1.AXIS_Z, out prfPos, count, out clock);
            } while ((status & 0x400) != 0);// 

            Thread.Sleep(50);
            sBackHomeSatus[Form1.AXIS_Z - 1] = 20;//
            sRtn = mc.GT_ZeroPos(Form1.AXIS_Z);
            return;
        }
       

       

        public void LogMessage(string text, int iColorMode)
        {

            BackHome.LogAppendDelegate method = new BackHome.LogAppendDelegate(this.LogAppend);
            if (iColorMode == 0)
            {
                if (text != null && text != "")
                {
                    richTextBox_backHome.ForeColor = Color.Black;
                    richTextBox_backHome.AppendText(DateTime.Now.ToString("HH:mm:ss ") + text);
                }

            }
            else if (iColorMode == 1)
            {
                if (text != null && text != "")
                {
                    richTextBox_backHome.ForeColor = Color.Green;
                    richTextBox_backHome.AppendText(DateTime.Now.ToString("HH:mm:ss ") + text);
                }

            }
        }
        public void LogAppend(Color color, string text)
        {
            this.richTextBox_backHome.AppendText("\n");
            this.richTextBox_backHome.SelectionColor = color;
            this.richTextBox_backHome.AppendText(text);
            this.richTextBox_backHome.ScrollToCaret();
        }
        private void timerPriority_Tick(object sender, EventArgs e)
        {
           

        }

        private void timerBackHome_Tick_1(object sender, EventArgs e)
        {
            String msg = "";
            switch (sBackHomeSatus[Form1.AXIS_X - 1])
            {
                case 20:
                    msg += "X轴回零成功";
                    break;
                case 21://运动停止，最可能原因限位触发
                    msg += "X轴运动停止";
                    sBackHomeSatus[Form1.AXIS_X - 1] = 11;
                    break;
                case 22://回零位置不准确
                    msg += "X轴回零失败";
                    break;
                case 11:
                    {
                        if (m_MovementMode.CheckLimitTrigger(Form1.AXIS_X))//确认限位触发
                        {
                            msg += "X轴限位触发";
                            sBackHomeSatus[Form1.AXIS_X - 1] = 0;
                            BackHomeInitX(true);
                        }
                        else
                        {
                            msg += "X轴回零失败";
                        }
                    }
                    break;
            }
            if (sBackHomeSatus[Form1.AXIS_X - 1] > 19)
            {
                sBackHomeSatus[Form1.AXIS_X - 1] = 1;//表示该轴回零结束			
            }

            switch (sBackHomeSatus[Form1.AXIS_Y - 1])
            {
                case 20:
                    msg += "Y轴回零成功";
                    break;
                case 21://运动停止，最可能原因限位触发
                    msg += "Y轴运动停止";
                    sBackHomeSatus[Form1.AXIS_Y - 1] = 11;
                    break;
                case 22:
                    msg += "Y轴回零失败";
                    break;
                case 11:
                    {
                        if (m_MovementMode.CheckLimitTrigger(Form1.AXIS_Y))//确认限位触发
                        {
                            msg += "Y轴限位触发";
                            sBackHomeSatus[Form1.AXIS_Y - 1] = 0;
                            BackHomeInitY(true);
                        }
                        else
                        {
                            msg += "Y轴回零失败";
                        }
                    }
                    break;
            }
            if (sBackHomeSatus[Form1.AXIS_Y - 1] > 19)
            {
                sBackHomeSatus[Form1.AXIS_Y - 1] = 1;//表示该轴回零结束	
            }

            switch (sBackHomeSatus[Form1.AXIS_Z - 1])
            {
                case 20:
                    msg += "Z轴回零成功";
                    break;
                case 21://运动停止，最可能原因限位触发
                    msg += "Z轴运动停止";
                    sBackHomeSatus[Form1.AXIS_Z - 1] = 11;
                    break;
                case 22:
                    msg += "Z轴回零失败";
                    break;
                case 11:
                    {
                        if (m_MovementMode.CheckLimitTrigger(Form1.AXIS_Z))//确认限位触发
                        {
                            msg += "Z轴限位触发";
                            sBackHomeSatus[Form1.AXIS_Z - 1] = 0;
                            BackHomeInitZ(true);
                        }
                        else
                        {
                            msg += "Z轴回零失败";
                        }
                    }
                    break;
            }
            if (sBackHomeSatus[Form1.AXIS_Z - 1] > 19)
            {
                sBackHomeSatus[Form1.AXIS_Z - 1] = 1;//表示该轴回零结束
            }

            

            if (sBackHomeSatus[Form1.AXIS_X - 1] == 1 &&
                sBackHomeSatus[Form1.AXIS_Y - 1] == 1 &&
                sBackHomeSatus[Form1.AXIS_Z - 1] == 1 
                )
            {
                if (m_MovementMode.CheckIsMoving())
                {
                    return;
                }
                msg += "回零结束";
                mc.GT_SetPrfPos(Form1.AXIS_X, 0);//此处必须把规划位置置零，否则会导致bug
                mc.GT_SetPrfPos(Form1.AXIS_Y, 0);
                mc.GT_SetPrfPos(Form1.AXIS_Z, 0);
               
              

                timerBackHome.Stop();
                //timerWeldingProgress.Stop();

                //progressBar_backHome.Value = 100;
                button_ok.Enabled = true;
               // button_abort.Text = "继续回零";
                button_Cancel.Enabled = true;
            }
            LogMessage(msg, 1);
        }

        private void timerWeldingProgress_Tick_1(object sender, EventArgs e)//进度表
        {
            //m_nBackHomeCount += 20;
            //if (m_nBackHomeCount > 101)
            //{
            //    m_nBackHomeCount = 0;
            //}
           // progressBar_backHome.Value = m_nBackHomeCount;
        }

        private void BackHome_Load(object sender, EventArgs e)
        {

        }

        private void button_ok_Click_1(object sender, EventArgs e)
        {
            mc.GT_ZeroPos(Form1.AXIS_X, 4);// 位置清偏置点
            //pView->m_curdPoint3D.clear();
            Close();
            DialogResult = DialogResult.OK;
        }

        private void button_Cancel_Click_1(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void timerPriority_Tick_1(object sender, EventArgs e)
        {
            if (nPriority == 1)
            {
                if (sBackHomeSatus[Form1.AXIS_X - 1] == 1)
                {
                    if (IsAxisBackHomeFinish)
                    {
                        IsAxisBackHomeFinish = false;
                        timerPriority.Stop();
                        BackHomeInitY(false);
                        BackHomeInitZ(false);
                        
                    }
                    IsAxisBackHomeFinish = true;
                }

            }
            else if (nPriority == 2)
            {
                if (sBackHomeSatus[Form1.AXIS_Y - 1] == 1)
                {
                    if (IsAxisBackHomeFinish)
                    {
                        IsAxisBackHomeFinish = false;
                        timerPriority.Stop();
                        BackHomeInitX(false);
                        BackHomeInitZ(false);
                        
                    }
                    IsAxisBackHomeFinish = true;
                }

            }
            else if (nPriority == 3)
            {
                if (sBackHomeSatus[2] == 1)
                {
                    if (IsAxisBackHomeFinish)
                    {
                        IsAxisBackHomeFinish = false;
                        timerPriority.Stop();
                        BackHomeInitX(false);
                        BackHomeInitY(false);
                        
                    }
                    IsAxisBackHomeFinish = true;
                }

            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            String str = button.Text;
            String msg = "";
            if (str == "暂停回零")
            {
                timerBackHome.Stop();
                //timerWeldingProgress.Stop();
                mc.GT_Stop(0x0FF, 0x0FF);
                button.Text = "继续回零";
                button_ok.Enabled = true;
                button_Cancel.Enabled = true;
                msg = "暂停回零\r\n";
            }
            else
            {
                button.Text = "暂停回零";
                msg = "继续回零\r\n";
                button_ok.Enabled = false;
                button_Cancel.Enabled = false;
                timerBackHome.Start();
                //timerWeldingProgress.Start();
                for (int i = 0; i < 8; ++i)
                {
                    sBackHomeSatus[i] = 0;
                }
                BackHomeInit(nPriority);
            }
            LogMessage(msg, 1);
        }
    }
}
