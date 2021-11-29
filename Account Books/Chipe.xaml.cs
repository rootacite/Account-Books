using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Account_Books
{
    /// <summary>
    /// Interaction logic for Chipe.xaml
    /// </summary>
    public partial class Chipe : UserControl
    {
        public string Text
        {
            get { return AC1.Content.ToString(); }
            set { AC1.Content = value; }
        }

        public string Icon
        {
            get { return AC1.Icon.ToString(); }
            set { AC1.Icon = value; }
        }
        public Chipe()
        {
            InitializeComponent();
        }
    }
}
