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
using System.Windows.Shapes;
using t_fit.ViewModels;

namespace t_fit.Views
{
    /// <summary>
    /// Interaction logic for TFit.xaml
    /// </summary>
    public partial class TFit : Window
    {
        public TFit()
        {
            IJsonService jsonService = new JsonService();
            
            InitializeComponent();
            DataContext = new TFitViewModels(jsonService);
        }
    }
}
