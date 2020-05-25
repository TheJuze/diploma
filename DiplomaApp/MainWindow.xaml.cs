using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DiplomaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            string args ="";
            args += inputName_input.Text+" ";
            args += inputSize_input.Text+ " ";
            args += taskType_input.Text;
            Diploma.Program.Main(new string[] { inputName_input.Text, inputSize_input.Text, taskType_input.Text });
           // Process.Start("Diploma.exe",args);
        }
    }
}
