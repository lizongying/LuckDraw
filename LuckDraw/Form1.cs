using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LuckDraw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool work = false;
        bool pause = false;
        String sound = null;
        IList<string> temp_list = new List<string>();
        IList<string> return_list = new List<string>();
        String huo = null;
        int arrIndex = 0;
        private AutoDo smsSender = new AutoDo();

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            smsSender.DoSomethingEvent += new AutoDo.DoSomething(smsSender_DoSomethingEvent);
            label1.Location = new Point((this.Width - label1.Width) / 2, (this.Height - label1.Height) / 2);
            add();
        }
        void smsSender_DoSomethingEvent(object sender, EventArgs e)
        {
            stp();
            Thread.Sleep(50);
        }

        private void play(String s)
        {
            System.Media.SoundPlayer sp = new SoundPlayer();
            if (s == "start")
            {
               sp.SoundLocation = "start.wav";
               sp.Load(); 
               sp.PlayLooping();
            }
            else
            {
                sp.SoundLocation = "stop.wav";
                sp.Load(); 
                sp.Play();
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Space:
                    if (pause && work && temp_list.Count > 0)
                    {
                        smsSender.Continue();
                        sound = "start";
                        play(sound);
                        pause = false;
                    }
                    else if (!pause && work)
                    {
                        smsSender.Pause();
                        addre();
                        sound = "stopt";
                        play(sound);
                        pause = true;
                    }
                    break;
                case Keys.Enter:
                    if (work)
                    {
                        smsSender.Stop();
                        sound = "stopt";
                        int c = return_list.Count;
                        for (int i = 0; i < c; i++)
                        {
                            huo += return_list[i] + "  ";
                        }
                        label2.Text = "获奖名单：" + huo;
                        play(sound);
                        work = false;
                    }
                    else if (!work && temp_list.Count > 0)
                    {
                        pause = false;
                        clean();
                        huo = null;
                        label2.Text = "";
                        smsSender.Start();
                        sound = "start";
                        play(sound);
                        work = true;
                    }
                    break;
            }
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;
            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        //this.Close();
                        reset();
                        break;
                }
            }
            return false;
        }

        private void addre()
        {
            if (arrIndex > -1)
            {
                return_list.Add(temp_list[arrIndex]);
                temp_list.RemoveAt(arrIndex);
            }
        }
		
		private string fullString(string str)
		{
			string strRes = "";
			switch(str.Length)
			{
				case 2:
					strRes = str.Substring(0, 1).PadRight(3) + str.Substring(1, 1);
					break;
				case 4:
                    strRes = str.Substring(2, 1).PadRight(3) + str.Substring(3, 1);
					break;
				default:
                    strRes = str;
					break;
			}
			return strRes;
		}

        private string[] getArray() 
        {
            string[] sArray = Properties.Resources.name.Split(Environment.NewLine.ToCharArray());
            return sArray;
        }

        private void add() 
        {
            //String[] constant = { "肖林生", "王  方", "郭洪亮", "袁  勃", "魏学进", "田久岳", "刘英男", "李  帅", "陈朝兴", "时晓静", "宋英豪", "李宗英", "解学渊", "温  航", "朱志航", "傅  彬", "郎海英", "温俊雄", "刘  超", "程俊雅", "李  浛", "张  静", "王  跃", "张建刚", "于  倩", "李南南", "韦春艳", "傅飞龙", "李  强", "杨晓清", "颜红英", "许正文", "张  凡", "王春锐", "胡  琨", "许海峰", "郭金龙", "张建宁", "陈国清", "吴  伟", "黄国强", "赵京博", "陈建峰", "王晶男", "陈天贵", "施胜坤", "杨玉海", "李娅茹", "吴俊峰", "姜  锋", "翁  强", "李  宁", "郭金龙", "张占龙", "梁海新", "胡淑芳", "左志梅", "范雪丽", "段雪婷", "朱丕罕", "王淑敏", "游昌华", "李传森", "肖  劼", "张兰亭", "李文元", "邢文明", "许相文", "王长生", "娄艳军", "赵宏丹", "梁起来", "张丽娜", "吕国庆", "夏冬旗", "李玉华", "侯春燕", "丁美华", "薛  飞", "韩军响", "赵天亮", "韩文明", "万  鹏", "贺天友", "徐  伟", "孙东江", "王爱静", "李  峰", "田一帆", "邓益民", "马悦程", "白梦薇", "王艳芬", "王贵胜", "李俊儒", "李涛艳", "齐  丹", "高基富", "张  潇", "卢鹏飞", "曹  维", "张德祥", "职小伟", "袁  圆", "唐益文", "李儒鹏", "潘慧燕", "谢东涛", "杨延春", "肖  丹", "单春岩", "刘丹丹", "马伊娜", "林志影", "薛  飘", "盖琳琳", "郭循双", "郭  兵", "董广文", "郭旭红", "韩金序", "南  苗", "韦改改", "朱艳玲", "杨  帆", "毕晓明", "李银娟", "赵志玲", "赵亚芬", "张欢欢", "穆惠芳", "杨玉冰", "卓明霞", "王碧文", "赵  楠", "孙梓惠", "石  静", "李  蒙", "张  岩", "邢爱玉", "冯秀丽", "王  婷", "付  艳", "夏俊芳", "孙雪婷", "刘彦秋", "陈锦瑞", "李君娥", "候  玉", "魏梦璐", "王甜甜", "蒋莹珍", "窦丽华", "彭文明", "彭迎娜", "王小丽", "陈学妍", "高  倩", "李洁雅", "郑素华", "王俊杰", "蔡克荣", "陈晓华", "贺  枭", "黄  敏", "徐  丹", "刘慧玲", "董凡茜", "王洪义", "李静静", "董志财", "李  生", "赵  军", "刘  丽", "毛  丹", "王晶女", "姜  颖", "曹丽君", "王文贵", "李冬梅", "张金泉", "陆翠香", "陈勇聪", "杜金霞", "赵  凯", "吴智伶", "刘  敏", "潘益新", "吴建新", "徐金刚", "安  忠", "杨淑杰", "张  春", "马淑香", "崔廷建", "刘  学", "温忠东", "韩青见", "钟凤印", "邵玉平", "郭  盈", "王厚才", "王文巧", "郭恩强", "郭恩胜", "韩书霞", "候玉娥", "常军委", "陈立冬", "杨从现", "张继业" };
            String[] constant = getArray();
            int m = constant.GetLength(0);
            for (int i = 0; i < m; i++)
            {
                if (constant[i].Length > 0)
                {
                    temp_list.Add(fullString(constant[i]));
                }
            }
        }

        private void clean()
        {
            return_list.Clear();
        }

        private void reset() 
        {
            temp_list.Clear();
            String[] constant = getArray();
            int m = constant.GetLength(0);
            for (int i = 0; i < m; i++)
            {
                if (constant[i].Length > 0)
                {
                    temp_list.Add(fullString(constant[i]));
                }
            }
        }

        private void stp()
        {
            int ishal = 1;
            Random rd = new Random();
            StringBuilder sb = new StringBuilder(ishal, 4);
            for (int i = 0; i < ishal; i++)
            {
                if (temp_list.Count > 0)
                {
                    arrIndex = rd.Next(0, temp_list.Count);
                    sb.Append(temp_list[arrIndex]);
                }
                else
                {
                    break;
                }
            }
            SetText(sb.ToString());
        }

        delegate void Changelabel(string str);

        void SetText(string str)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Changelabel(SetText), str);
            }
            else
            {
                this.label1.Text = str;
            }
        }
    }
}
