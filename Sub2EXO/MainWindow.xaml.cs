using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sub2EXO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InputBox_LostFocus(null, null);
            //InputBox.Text = "1\n00:00:04,299--> 00:00:06,290\n歌曲：The Day You Went Away\n2\n00:00:06,299--> 00:00:22,990\n歌手：M2M\n3\n00:00:22,999--> 00:00:27,880\nWell I wonder could it be\n4\n00:00:27,889--> 00:00:30,750\nWhen I was dreaming about you baby\n5\n00:00:30,759--> 00:00:33,020\nYou were dreaming of me";
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            double fps = double.Parse(fpsBox.Text);
            double timeLength = double.Parse(timeBox.Text);


            OutputBox.Clear();
            OutputBox.AppendText("[exedit]\nwidth=1920\nheight=1080\nrate=60\nscale=1\nlength=0\naudio_rate=48000\naudio_ch=2\n");
            string input = InputBox.Text;

            string[] lines = input.Split('\n');

            int i = 0;
            foreach (string line in lines)
            {
                double startTime = i * timeLength;
                double endTime = (i + 1) * timeLength;
                Console.WriteLine("Time in second: " + startTime + " - " + endTime);

                uint startFrame = uint.Parse((Math.Floor(startTime * fps) + 1).ToString());
                uint endFrame = uint.Parse(Math.Floor(endTime * fps).ToString());
                Console.WriteLine("Time in frame: " + startFrame + " - " + endFrame);

                string content = line;
                Console.WriteLine("Content: " + content + "\n");

                Encoding utf8 = Encoding.GetEncoding("utf-16");
                byte[] temp = utf8.GetBytes(content);
                string contentHex = "";
                foreach (byte b in temp)
                {
                    contentHex += b.ToString("X2");
                }
                contentHex = contentHex.PadRight(4096, '0');
                Console.WriteLine("ContentHex: " + contentHex + "\n");

                //
                string block = String.Format("[{0}]\nstart={1}\nend={2}\nlayer=1\noverlay=1\ncamera=0\n" +
                    "[{0}.0]\n_name=Text\nSize=34\nvDisplay=0.0\n1char1obj=0\nShow on motion coordinate=0\nAutomatic scrolling=0\n" +
                    "B=0\nI=0\ntype=0\nautoadjust=0\nsoft=1\nmonospace=0\nalign=0\nspacing_x=0\nspacing_y=0\nprecision=1\n" +
                    "color=ffffff\ncolor2=000000\nfont=MS UI Gothic\ntext={3}\n" +
                    "[{0}.1]\n_name=Standard drawing\nX=0.0\nY=0.0\nZ=0.0\nZoom%=100.00\nClearness=0.0\nRotation=0.00\nblend=0\n", i, startFrame, endFrame, contentHex);
                OutputBox.AppendText(block);

                i++;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "exo File|*.exo",
                FileName = "New_Sub",
                AddExtension = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.OpenFile(), Encoding.UTF8);
                streamWriter.Write(OutputBox.Text);
                streamWriter.Close();
            }

        }

        //private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    OutputBox.Clear();
        //    OutputBox.AppendText("[exedit]\nwidth=1920\nheight=1080\nrate=60\nscale=1\nlength=1667\naudio_rate=48000\naudio_ch=2\n");
        //    string input = InputBox.Text + '\n';

        //    MatchCollection matchCollection = Regex.Matches(input, "(?<sh>[0-9]+):(?<sm>[0-9]+):(?<ss>[0-9]+),(?<sms>[0-9]+).*-->.*(?<eh>[0-9]+):(?<em>[0-9]+):(?<es>[0-9]+),(?<ems>[0-9]+).*\n(?<c>.*)\n");
        //    Console.WriteLine("MatchCount: " + matchCollection.Count + "\n");

        //    int i = 0;
        //    foreach(Match match in matchCollection)
        //    {
        //        uint sh = uint.Parse(match.Groups["sh"].Value);
        //        uint sm = uint.Parse(match.Groups["sm"].Value);
        //        uint ss = uint.Parse(match.Groups["ss"].Value);
        //        double sms = uint.Parse(match.Groups["sms"].Value);

        //        uint eh = uint.Parse(match.Groups["eh"].Value);
        //        uint em = uint.Parse(match.Groups["em"].Value);
        //        uint es = uint.Parse(match.Groups["es"].Value);
        //        double ems = uint.Parse(match.Groups["ems"].Value);
        //        Console.WriteLine("Format Time: " + sh + ":"+sm+ ":" + ss + "," + sms + " - " + eh + ":" + em + ":" + es + "," + ems);

        //        double startTime = sh * 3600 + sm * 60 + ss + sms / 1000;
        //        double endTime = eh * 3600 + em * 60 + es + ems / 1000;
        //        Console.WriteLine("Time in second: " + startTime + " - " + endTime);

        //        uint startFrame = uint.Parse((Math.Floor(startTime * fps) + 1).ToString());
        //        uint endFrame = uint.Parse(Math.Floor(endTime * fps).ToString());
        //        Console.WriteLine("Time in frame: " + startFrame + " - " + endFrame);

        //        string content = match.Groups["c"].Value.TrimEnd(new char[] { '\r', '\n' });
        //        Console.WriteLine("Content: " + content + "\n");

        //        Encoding utf8 = Encoding.GetEncoding("utf-16");
        //        byte[] temp = utf8.GetBytes(content);
        //        string contentHex = "";
        //        foreach(byte b in temp)
        //        {
        //            contentHex += b.ToString("X2");
        //        }
        //        contentHex = contentHex.PadRight(4096, '0');
        //        Console.WriteLine("ContentHex: " + contentHex + "\n");

        //        //
        //        string block = String.Format("[{0}]\nstart={1}\nend={2}\nlayer=1\noverlay=1\ncamera=0\n" +
        //            "[{0}.0]\n_name=Text\nSize=34\nvDisplay=0.0\n1char1obj=0\nShow on motion coordinate=0\nAutomatic scrolling=0\n" +
        //            "B=0\nI=0\ntype=0\nautoadjust=0\nsoft=1\nmonospace=0\nalign=0\nspacing_x=0\nspacing_y=0\nprecision=1\n" +
        //            "color=ffffff\ncolor2=000000\nfont=MS UI Gothic\ntext={3}\n" +
        //            "[{0}.1]\n_name=Standard drawing\nX=0.0\nY=0.0\nZ=0.0\nZoom%=100.00\nClearness=0.0\nRotation=0.00\nblend=0\n", i, startFrame, endFrame, contentHex);
        //        OutputBox.AppendText(block);

        //        i++;
        //    }

        //    SaveFileDialog saveFileDialog = new SaveFileDialog()
        //    {
        //        Filter = "exo File|*.exo",
        //        FileName = "New_Sub",
        //        AddExtension = true
        //    };
        //    if(saveFileDialog.ShowDialog() == true)
        //    {
        //        StreamWriter streamWriter = new StreamWriter(saveFileDialog.OpenFile(), Encoding.UTF8);
        //        streamWriter.Write(OutputBox.Text);
        //        streamWriter.Close();
        //    }

        //}

        private void InputBox_PreviewDragOver(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }

        }

        private void InputBox_PreviewDrop(object sender, DragEventArgs e)
        {
            InputBox.Focus();
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            StreamReader streamReader = new StreamReader(path);
            InputBox.Text = streamReader.ReadToEnd();
            streamReader.Close();
            e.Handled = true;
        }

        //string alertText = "输入SRT字幕脚本 或 拖入SRT文件";
        string alertText = "请在此输入字幕文本\n每行为一个文本物件";
        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(InputBox.Text == alertText)
            {
                InputBox.Clear();
                InputBox.Foreground = (Brush)new BrushConverter().ConvertFromString("#FF000000");
                InputBox.FontStyle = FontStyles.Normal;
            }
        }

        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (InputBox.Text == "")
            {
                InputBox.Text = alertText;
                InputBox.Foreground = (Brush)new BrushConverter().ConvertFromString("#FF666666");
                InputBox.FontStyle = FontStyles.Italic;
            }
        }
    }
}
