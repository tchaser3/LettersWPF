/* Title:           Add Letter
 * Date:            10-5-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the form to add a Letter */

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

namespace LettersWPF
{
    /// <summary>
    /// Interaction logic for AddLetter.xaml
    /// </summary>
    public partial class AddLetter : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        LettersClass TheLettersClass = new LettersClass();

        FindLetterByDateMatchDataSet TheFindLetterByDateMatchDataSet = new FindLetterByDateMatchDataSet();
        FindLetterByDescriptionDataSet TheFindLetterByDescriptionDataSet = new FindLetterByDescriptionDataSet();

        FindLetter FindLetter = new FindLetter();

        public AddLetter()
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

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            FindLetter.Close();
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FindLetter.Show();

            txtLetterName.Focus();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //this will add the record
            bool blnFatalError;
            DateTime datLetterDate = DateTime.Now;
            string strLetterDescription;
            int intRecordsReturned;

            try
            {
                strLetterDescription = txtLetterName.Text;
                if(strLetterDescription == "")
                {
                    TheMessagesClass.ErrorMessage("The Letter Description Was Not Entered");
                    return;
                }

                TheFindLetterByDescriptionDataSet = TheLettersClass.FindLetterByDescription(strLetterDescription);

                intRecordsReturned = TheFindLetterByDescriptionDataSet.FindLetterByDescription.Rows.Count;

                if(intRecordsReturned > 0)
                {
                    TheMessagesClass.ErrorMessage("The Letter Exists");
                    return;
                }

                blnFatalError = TheLettersClass.InsertLetter(strLetterDescription, datLetterDate);

                if (blnFatalError == true)
                    throw new Exception();

                TheFindLetterByDateMatchDataSet = TheLettersClass.FindLetterByDateMatch(datLetterDate);

                MainWindow.gintLetterID = TheFindLetterByDateMatchDataSet.FindLetterByDateMatch[0].LetterID;

                FindLetter.Close();
                CreateNewParagraph createNewParagraph = new CreateNewParagraph();
                createNewParagraph.Show();
                Close();
                
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Letters WPF // Add Letter // Add Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void txtLetterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.gstrLetterDescription = txtLetterName.Text;
        }
    }
}
