// Use this code inside a project created with the Visual C# > Windows Desktop > Console Application template.
// Replace the code in Program.cs with this code.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Threading.Timer;
using System.Drawing;
using HWND = System.IntPtr;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    public static class PortChat
    {
        static bool _continue;
        static SerialPort? _serialPort;
        private static Queue<byte> _recievedData = new();
        private static double _value;
        private static int _oldValue = -1;
        private static int _minute;
        private static int _hours;
        static AutoResetEvent waitHandler = new AutoResetEvent(true);

        public static void Main()
        {
             _serialPort = new SerialPort();
             _serialPort.PortName = "com5";
             _serialPort.BaudRate = 9600;
             _serialPort.Parity = Parity.None;
             _serialPort.DataBits = 8;
             _serialPort.StopBits = StopBits.One;
             _serialPort.DataReceived += SerialPort_DataReceived;
             _serialPort.ReadTimeout = 2000;
            //
            //
             _serialPort.Open();
             _continue = true;
             //while (_continue)
             //{
             //}
            //
             //_serialPort.Close();

            font = new Font("Calibri", 9F, FontStyle.Regular);
            TimerCallback tm = timer_Elapsed;
            brush = new SolidBrush(Color.Black);
            Console.Title = "Аналоговые часы";
            // for (int i = 1500; i < 100000; i++)
            // {
            //     _value = i;
            //     huita((int)_value);
            //     i += 60;
            // }
            // Timer timer = new Timer(tm, (int)_value, 1000, 1000);
            Console.ReadKey(true);
        }

        static void huita(int t)
        {
            timer_Elapsed(t);
            Thread.Sleep(1000);
        }
        
        static void SerialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[_serialPort.BytesToRead];
            _serialPort.Read(data, 0, data.Length);
            data.ToList().ForEach(b => _recievedData.Enqueue(b));
            ProcessData();
        }

        static void ProcessData()
        {
            if (_recievedData.Count <= 300) return;
            var packet = Enumerable.Range(0, 300).Select(i => _recievedData.Dequeue());

            var lol = packet.ToArray();
            var mes = Encoding.ASCII.GetString(lol);
            var sss = mes.Split('\0');
            List<string> res_str = new List<string>();
            foreach (var w in sss)
            {
                if (w.Length == 4)
                {
                    res_str.Add(w);
                }
            }

            // var res = new List<double>();
            // foreach (var str in res_str)
            // {
            //     res.Add(Convert.ToDouble(str));
            // }
            //
            // double value = 0;
            // foreach (var sRe in res)
            // {
            //     value += sRe;
            // }
            //
            // value /= 30;
            // value = Math.Round(value);
            var value = Convert.ToDouble(sss.Last()); 

            _value = value;
        }
        

        static Graphics graphics;
        static Rect rect;
        static HWND hWnd;
        static Font font;
        static SolidBrush brush;

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rect rect);

        static void timer_Elapsed(object? obj)
        {
            /*
             * Рисуем цифер блэт.
             */

            hWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            graphics = Graphics.FromHwnd(hWnd);
            rect = new Rect();
            graphics.Clear(Color.White);
            GetWindowRect(hWnd, ref rect);
            int height = rect.Bottom - rect.Top;
            int width = rect.Right - rect.Left;
            int xCenter = width >> 1;
            int yCenter = (height >> 1) - 25;
            int r = 120;
            int d = 120 << 1;
            graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(xCenter - 5, yCenter - 5, 10, 10));
            graphics.DrawEllipse(new Pen(Color.Black), new Rectangle(xCenter - r, yCenter - r, d, d));

            for (int i = 1; i < 61; i++)
            {
                if (i % 5 == 0)
                {
                    // x = r*cos(a) + u; 
                    // y = r*sin(a) + v
                    Point pV = new Point((int)((r - 15) * Math.Cos((i * 6) * Math.PI / 180) + xCenter),
                        (int)((r - 15) * Math.Sin((i * 6) * Math.PI / 180) + yCenter));
                    Point pL = new Point((int)(r * Math.Cos((i * 6) * Math.PI / 180) + xCenter),
                        (int)(r * Math.Sin((i * 6) * Math.PI / 180) + yCenter));
                    graphics.DrawLine(new Pen(Color.Blue, 2), pL, pV);
                }
                else
                {
                    Point pV = new Point((int)((r - 5) * Math.Cos((i * 6) * Math.PI / 180) + xCenter),
                        (int)((r - 5) * Math.Sin((i * 6) * Math.PI / 180) + yCenter));
                    Point pL = new Point((int)(r * Math.Cos((i * 6) * Math.PI / 180) + xCenter),
                        (int)(r * Math.Sin((i * 6) * Math.PI / 180) + yCenter));
                    graphics.DrawLine(new Pen(Color.Green, 2), pL, pV);
                }
            }

            graphics.DrawString("12", font, brush,
                new Point(xCenter - ((int)graphics.MeasureString("12", font).Width >> 1), yCenter - r - 15));
            graphics.DrawString("6", font, brush,
                new Point(xCenter - ((int)graphics.MeasureString("6", font).Width >> 1), yCenter + r + 15));
            graphics.DrawString("3", font, brush,
                new Point(xCenter + r + 15, yCenter - ((int)graphics.MeasureString("3", font).Height >> 1)));
            graphics.DrawString("9", font, brush,
                new Point(xCenter - r - 15, yCenter - ((int)graphics.MeasureString("9", font).Height >> 1)));

            Point pNumberPos = new Point((int)((r + 15) * Math.Cos(-60 * Math.PI / 180) + xCenter),
                (int)((r + 15) * Math.Sin(-60 * Math.PI / 180) + yCenter));
            graphics.DrawString("1", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 15) * Math.Cos(-30 * Math.PI / 180) + xCenter),
                (int)((r + 15) * Math.Sin(-30 * Math.PI / 180) + yCenter));
            graphics.DrawString("2", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 15) * Math.Cos(30 * Math.PI / 180) + xCenter),
                (int)((r + 15) * Math.Sin(30 * Math.PI / 180) + yCenter));
            graphics.DrawString("4", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 15) * Math.Cos(60 * Math.PI / 180) + xCenter),
                (int)((r + 15) * Math.Sin(60 * Math.PI / 180) + yCenter));
            graphics.DrawString("5", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 15) * Math.Cos(120 * Math.PI / 180) + xCenter),
                (int)((r + 15) * Math.Sin(120 * Math.PI / 180) + yCenter));
            graphics.DrawString("7", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 15) * Math.Cos(150 * Math.PI / 180) + xCenter),
                (int)((r + 15) * Math.Sin(150 * Math.PI / 180) + yCenter));
            graphics.DrawString("8", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 19) * Math.Cos(210 * Math.PI / 180) + xCenter),
                (int)((r + 19) * Math.Sin(210 * Math.PI / 180) + yCenter));
            graphics.DrawString("10", font, brush, pNumberPos);

            pNumberPos = new Point((int)((r + 19) * Math.Cos(240 * Math.PI / 180) + xCenter),
                (int)((r + 19) * Math.Sin(240 * Math.PI / 180) + yCenter));
            graphics.DrawString("11", font, brush, pNumberPos);
            /*
             * Рисуем стрелки
             */
            int secsec = (int)obj;
            int second = DateTime.Now.Second;
            int minute = secsec / 60;
            int hour = minute / 60;
            // Console.WriteLine(secsec);
            // Console.WriteLine(_oldValue);
            if (secsec == _oldValue)
            {
                if (second == 59)
                {
                    _minute++;
                    if (minute == 59)
                    {
                        _hours++;
                    }
                }
            }
            else
            {
                // second = DateTime.Now.Second;
                // minute = DateTime.Now.Minute;
                // hour = DateTime.Now.Hour;
            }


            int secondAngle = second * 6 - 90;
            int minuteAngle = (minute + _minute) * 6 - 90;
            int hourAngle = (hour + _hours) * 30 - 90;

            /* Стрелка секундная */
            graphics.DrawLine(new Pen(Color.Green, 1), new Point(xCenter, yCenter),
                new Point((int)((r - 25) * Math.Cos(secondAngle * Math.PI / 180) + xCenter),
                    (int)((r - 25) * Math.Sin(secondAngle * Math.PI / 180) + yCenter)));
            /* Минутная стрелка */
            graphics.DrawLine(new Pen(Color.Blue, 2), new Point(xCenter, yCenter),
                new Point((int)((r - 30) * Math.Cos(minuteAngle * Math.PI / 180) + xCenter),
                    (int)((r - 30) * Math.Sin(minuteAngle * Math.PI / 180) + yCenter)));
            /* Часовая стрелка */
            graphics.DrawLine(new Pen(Color.Red, 3), new Point(xCenter, yCenter),
                new Point((int)((r - 25) * Math.Cos(hourAngle * Math.PI / 180) + xCenter),
                    (int)((r - 25) * Math.Sin(hourAngle * Math.PI / 180) + yCenter)));
            
            if ((int)obj! != 0 && _oldValue != (int)obj)
            {
                _oldValue = (int)obj;
                _minute = 0;
                _hours = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}