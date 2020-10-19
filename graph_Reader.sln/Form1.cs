using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;
using System.Diagnostics;

namespace Graph_Reader
{
    public partial class Form1 : Form
    {

        List<string> Time = new List<string>();
        List<string> Y1 = new List<string>();
        List<string> Y2 = new List<string>();
        List<int> Time2 = new List<int>();
        double A1, A2, F1, F2;
        private int _minutes, _seconds, _cseconds, _data;
        bool on2 = false;

        string strContents = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void 파일FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richtxtBox1.Text = "";
            strContents = "";
        }
        public void 열기OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDlg.FileName = "";
            openFileDlg.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"; // 파일열기 대화상자 확장자 설정
            richtxtBox1.Text = "";
            if (openFileDlg.ShowDialog() == DialogResult.OK) // 파일열기 대화상자를 열고 OK버튼으로 닫히면 처리
            {
                // 경로를 얻어 표시하기
                textBox1.Text = openFileDlg.FileName + " 파일을 엽니다.";
                // StreamReader를 이용한 csv 파일 읽기
                ReadCSV(openFileDlg.FileName);
                richtxtBox1.Text = strContents; // 파일 내용 표시
            }
        }
        private void 저장SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDlg.FileName = ""; // 파일 이름 초기화
            saveFileDlg.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            // 파일 저장 대화상자를 열고, OK로 닫혔을 때 처리
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = saveFileDlg.FileName + " 파일을 저장합니다."; // 경로 표시
                                                                      // StreamWriter를 이용한 txt 파일 쓰기
                using (StreamWriter sw = new StreamWriter(
                new FileStream(saveFileDlg.FileName, FileMode.Create)))
                {
                    //strContents = richtxtBox1.Text;
                    sw.WriteLine(strContents);
                }
            }
        }
        private void SetSize()
        {
            zedGraphControl1.Location = new Point(0, 0);
            // Leave a small margin around the outside of the control
            zedGraphControl1.Size = new Size(0, 0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Size the control to fill the form with a margin
            SetSize();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }
        private void ReadCSV(string data)
        {
            Debug.WriteLine("kgghk");
            //int row = 0;
            using (StreamReader gate = new StreamReader(new FileStream(data, FileMode.Open)))
            {
                while (gate.EndOfStream == false)
                {
                    //Debug.WriteLine(sr.ReadLine());

                    string s = gate.ReadLine();
                    string[] temp = s.Split(',');
                    Time.Add(temp[0]);
                    Y1.Add(temp[1]);
                    Y2.Add(temp[2]);
                }
            }
            using (StreamReader gate = new StreamReader(new FileStream(data, FileMode.Open)))
            {
                while (gate.EndOfStream == false) // 파일 끝까지 읽기
                {
                    strContents += gate.ReadLine();
                    strContents += "\n"; // 라인 단위로 읽으므로, 다음 줄로 이동 ‘\n’ 삽입
                }
                gate.Close();
            }
        }
            //return row - 1;
        public void timer1_Tick(object sender, EventArgs e)
        {
            IncreaseData();
            IncreaseCSeconds();
            ShowTime();
            zedGraphControl1.Refresh();
            CreateGraph2(zedGraphControl1);
            _data = _data + 1;
            Time2.Add(_data);
            if(_data == 201)
            {
                timer1.Stop();
            }

        }
        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "2017140141 하상우\n(data.csv 실습)";
            myPane.XAxis.Title.Text = "time";
            myPane.YAxis.Title.Text = "Result";


            // Make up some data arrays based on the Sine function
            double x, y1, y2;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            for (int i = 1; i < Y1.Count; i++)
            {
                x = double.Parse(Time[i]);
                y1 = double.Parse(Y1[i]);
                y2 = double.Parse(Y2[i]);
                list1.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("time-Y1",
                  list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            LineItem myCurve2 = myPane.AddCurve("time-Y2",
                  list2, Color.Blue, SymbolType.Circle);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }
        public void CreateGraph2(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;
            zgc.GraphPane.CurveList.Clear();
            // Set the Titles
            myPane.Title.Text = "과제\n(시간에 따른 그래프)";
            myPane.XAxis.Title.Text = "Time";
            myPane.YAxis.Title.Text = "Y";
            double x, y1, y2, y3;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();

            LineItem myCurve1 = myPane.AddCurve("Y1", list1, Color.Red, SymbolType.Diamond);
            LineItem myCurve2 = myPane.AddCurve("Y2", list2, Color.Blue, SymbolType.Circle);
            LineItem myCurve3 = myPane.AddCurve("Y1+Y2", list3, Color.Green, SymbolType.Square);

            for (int i = 0; i < Time2.Count; i++)
            {
                x = Time2[i];
                y1 = A1 * Math.Cos(2 * Math.PI * F1 * Time2[i] * Math.PI / 180);
                y2 = A2 * Math.Sin(2 * Math.PI * F2 * Time2[i] * Math.PI / 180);
                y3 = y1 + y2;
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
                if (Time2.Count == 200)
                {
                    timer1.Stop();
                }
            }
            zgc.AxisChange();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            CreateGraph(zedGraphControl1);
            zedGraphControl1.Location = new Point(350, 40);
            zedGraphControl1.Size = new Size(450, 300);
        }
        public void btnStart2_Click(object sender, EventArgs e)
       {
            timer1.Enabled = true;
            btnStart2.Enabled = false;
            A1 = double.Parse(textA1.Text);
            A2 = double.Parse(textA2.Text);
            F1 = double.Parse(textF1.Text);
            F2 = double.Parse(textF2.Text);


            richtxtBox1.Text = "";
            SetSize();
            zedGraphControl1.Refresh();
            CreateGraph2(zedGraphControl1);
            zedGraphControl1.Location = new Point(350, 40);
            zedGraphControl1.Size = new Size(450, 300);

        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = true;
            btnStart2.Enabled = true;
            if(on2 == true)
            {
                timer1.Stop();
                on2 = false;
            }
            else
            {
                timer1.Start();
                on2 = true;
                btnStart2.Enabled = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnStart2.Enabled = true;
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            _cseconds = 0;
            _minutes = 0;
            _seconds = 0;
            _data = 0;

            ShowTime();
        }
        private void IncreaseData()
        {
            int _data = _cseconds + 1;
        }
        private void IncreaseCSeconds()
        {
            if(_cseconds == 9)
            {
                _cseconds = 0;
                IncreaseSeconds();
            }
            else
            {
                _cseconds++;
            }
        }
        private void IncreaseSeconds()
        {
            if (_seconds == 59)
            {
                _seconds = 0;
                IncreaseMinutes();
            }
            else
            {
                _seconds++;
            }
        }
        private void IncreaseMinutes()
        {
            _minutes++;
        }
        private void ShowTime()
        {
            csecondText.Text = _cseconds.ToString("00");
            minuteText.Text = _minutes.ToString("00");
            secondText.Text = _seconds.ToString("00");
            dataText.Text = _data.ToString("00");


        }
    }
}
