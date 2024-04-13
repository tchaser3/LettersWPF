/* Title:           Main Menu
 * Date:            10-5-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the main menu */

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

namespace LettersWPF
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        //setting up the class
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About About = new About();
            About.ShowDialog();
        }

        private void btnAddLetter_Click(object sender, RoutedEventArgs e)
        {
            AddLetter AddLetter = new AddLetter();
            AddLetter.Show();
            Close();
        }

        private void btnAddParagraph_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.gintLetterID = 0;
            CreateNewParagraph CreateNewParagraph = new CreateNewParagraph();
            CreateNewParagraph.Show();
            Close();
        }

        private void btnViewLetter_Click(object sender, RoutedEventArgs e)
        {
            ViewLetters ViewLetters = new ViewLetters();
            ViewLetters.Show();
            Close();
        }
    }
}
