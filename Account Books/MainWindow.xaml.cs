using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Account_Books
{
    public class QAController
    {
        static public int Index = 0;
        static public string Post(string Url, string jsonParas)
        {
            string strURL = Url;
            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded";

            //设置参数，并进行URL编码 

            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);   

            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            while (true)
            {
                try
                {
                    writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
                    break;
                }
                catch (Exception)
                {
                    writer = null;
                    Console.Write("连接服务器失败!");
                    return null;
                }
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流
                           // String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            Stream s = response.GetResponseStream();
            //  Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            return postContent;//返回Json数据
        }
        static public string[] QS = new string[]
        {
            "set 在80386编译环境中，long型占的空间是。 8位 16位 32位 64位",
            "set 使全局变量跨文件访问的关键字是。 static extern global fixed",
            "set 8086CPU用何种方式从16位寄存器得到20位地址宽度？ Base*16+Offset (Base+16)*offset (Base+Offset)*16 Base+Offset+16",
            "set 在80386CPU的保护模式下，ring3程序所访问到的内存地址被称为： 物理地址 虚拟地址 线性地址 逻辑地址"
        };
        static public int[] AS = new int[] 
        {
            2,1,0,2
        };

        static public Dictionary<string,int> keyPlayer = new Dictionary<string,int>();
        static public Dictionary<string, int> winsPlayer = new Dictionary<string, int>();
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (e, v) =>
            {
                Stic SticObj = new Stic();

                Sticbac.Content = SticObj.Content;
                QAController.Post("http://150.158.103.209:8000/", QAController.QS[QAController.Index]);
                EventHandler bnm= (f, b) =>
                {
                    string address = "http://150.158.103.209:8000/";
                    string result = "";
                    using (var client = new HttpClient())
                        result = client.GetAsync(address).Result.Content.ReadAsStringAsync().Result;

                    var jn = result.Split('\n');
                    JObject jo = (JObject)JsonConvert.DeserializeObject(jn[0]);

                    QSX.Text = (string)jo["Question"];
                    AC1.Content = jo["A1"];
                    AC2.Content = jo["A2"];
                    AC3.Content = jo["A3"];
                    AC4.Content = jo["A4"];

                    JObject sc = (JObject)JsonConvert.DeserializeObject(jn[1]);

                    double mount = sc["A1"].Count() + sc["A2"].Count() + sc["A3"].Count() + sc["A4"].Count();

                    SticObj.A1.Value = mount == 0 ? 0 : sc["A1"].Count() / mount * 100d;
                    SticObj.A2.Value = mount == 0 ? 0 : sc["A2"].Count() / mount * 100d;
                    SticObj.A3.Value = mount == 0 ? 0 : sc["A3"].Count() / mount * 100d;
                    SticObj.A4.Value = mount == 0 ? 0 : sc["A4"].Count() / mount * 100d;

                    QAController.keyPlayer.Clear();
                    foreach (var i in sc["A1"])
                    {
                        QAController.keyPlayer.Add(i.ToString(), 0);
                    }
                    foreach (var i in sc["A2"])
                    {
                        QAController.keyPlayer.Add(i.ToString(), 1);
                    }
                    foreach (var i in sc["A3"])
                    {
                        QAController.keyPlayer.Add(i.ToString(), 2);
                    }
                    foreach (var i in sc["A4"])
                    {
                        QAController.keyPlayer.Add(i.ToString(), 3);
                    }

                   
                };
                bnm.Invoke(null,null);


                DispatcherTimer dt = new DispatcherTimer();
                dt.Interval = TimeSpan.FromSeconds(2.5d);
                dt.Tick += bnm;
                dt.Start();
                
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (QAController.Index > 0)
            {
                QAController.Index--;
            }
            QAController.Post("http://150.158.103.209:8000/", QAController.QS[QAController.Index]);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(QAController.Index < QAController.QS.Length - 1)
            {
                QAController.Index++;
            }
            QAController.Post("http://150.158.103.209:8000/", QAController.QS[QAController.Index]);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var i in QAController.keyPlayer)
            {
                if (i.Value == QAController.AS[QAController.Index])
                {
                    if (QAController.winsPlayer.ContainsKey(i.Key))
                        QAController.winsPlayer[i.Key]++;
                    else
                        QAController.winsPlayer.Add(i.Key, 1);
                }
                else
                {
                    if (!QAController.winsPlayer.ContainsKey(i.Key))
                        QAController.winsPlayer.Add(i.Key, 0);
                }
            }
            int code = 0;
            var scva = DictonarySort(QAController.winsPlayer);
            foreach (var i in scva)
            {
                PlayerList.Items.Clear();
                PlayerList.Items.Add(new { Code = code.ToString(), Name = i.Key, Description = i.Value.ToString() });
                code++;
            }
        }

        private Dictionary<string, int> DictonarySort(Dictionary<string, int> dic)
        {
            var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
            return new Dictionary<string, int>(dicSort);
        }
    }
}
