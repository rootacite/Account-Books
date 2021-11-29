using System;
using System.Collections.Generic;
using System.Linq;
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Account_Books
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (e, v) =>
            {
                Stic SticObj = new Stic();

                Sticbac.Content = SticObj.Content;

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
                SticObj.A1.Value = sc["A1"].Count();
                SticObj.A2.Value = sc["A2"].Count();
                SticObj.A3.Value = sc["A3"].Count();
                SticObj.A4.Value = sc["A4"].Count();
            };
        }
    }
}
