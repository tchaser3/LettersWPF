/* Title:           View Letters
 * Date:            10-9-17
 * Author:          Terry Holmes
 * 
 * Description:     This form will allow the user to view a letter that has been created */

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
using NewEventLogDLL;
using LettersDLL;
using KeyWordDLL;

namespace LettersWPF
{
    /// <summary>
    /// Interaction logic for ViewLetters.xaml
    /// </summary>
    public partial class ViewLetters : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        LettersClass TheLettersClass = new LettersClass();
        MDULettersClass TheMDULettersClass = new MDULettersClass();
        KeyWordClass TheKeyWordClass = new KeyWordClass();
        
        FindSortedLettersDataSet TheFindSortedLettersDataSet = new FindSortedLettersDataSet();
        string gstrLetterName;

        public ViewLetters()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this will load up the combo box
            //setting local variables
            int intCounter;
            int intNumberOfRecords;

            try
            {
                TheFindSortedLettersDataSet = TheLettersClass.FindSortedLetters();

                intNumberOfRecords = TheFindSortedLettersDataSet.FindSortedLetters.Rows.Count - 1;
                cboSelectLetter.Items.Add("Select Letter");

                if(intNumberOfRecords > -1)
                {
                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectLetter.Items.Add(TheFindSortedLettersDataSet.FindSortedLetters[intCounter].LetterDescription);
                    }
                }

                cboSelectLetter.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Letters WPF // View Letters // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            bool blnKeyWordNotFound = true;

            blnKeyWordNotFound = TheKeyWordClass.FindKeyWord("MDU DROP ACCEPTANCE", gstrLetterName);

            if(blnKeyWordNotFound == false)
            {
                TheMDULettersClass.CreateMDUDropAcceptanceLetter("TESTING", "Letters WPF // ");
            }
        }

        private void cboSelectLetter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboSelectLetter.SelectedIndex > 0)
            {
                gstrLetterName = cboSelectLetter.SelectedItem.ToString();

                MainWindow.gintLetterID = TheFindSortedLettersDataSet.FindSortedLetters[cboSelectLetter.SelectedIndex - 1].LetterID;
            }
        }
    }
}
