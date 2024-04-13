/* Title:           Find Letter
 * Date:            10-6-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the form that shows existing letters */

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
using System.Windows.Threading;
using NewEventLogDLL;
using LettersDLL;

namespace LettersWPF
{
    /// <summary>
    /// Interaction logic for FindLetter.xaml
    /// </summary>
    public partial class FindLetter : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        LettersClass TheLettersClass = new LettersClass();

        FindLetterByDescriptionDataSet TheFindLetterByDescriptionDataSet = new FindLetterByDescriptionDataSet();

        DispatcherTimer MyTimer = new DispatcherTimer();

        public FindLetter()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyTimer.Tick += new EventHandler(BeginTheProcess);
            MyTimer.Interval = new TimeSpan(0, 0, 1);
            MyTimer.Start();
        }
        private void BeginTheProcess(object sender, EventArgs e)
        {
            TheFindLetterByDescriptionDataSet = TheLettersClass.FindLetterByDescription(MainWindow.gstrLetterDescription);

            dgrResults.ItemsSource = TheFindLetterByDescriptionDataSet.FindLetterByDescription;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
