/* Title:           Create New Paragraph
 * Date:            10-6-17
 * Author:          Terry Holmes
 * 
 * Description:     This form is used to create paragraphs */

using DataValidationDLL;
using LettersDLL;
using NewEventLogDLL;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LettersWPF
{
    /// <summary>
    /// Interaction logic for CreateNewParagraph.xaml
    /// </summary>
    public partial class CreateNewParagraph : Window
    {
        //setting classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        LettersClass TheLettersClass = new LettersClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        FindLettersByLetterIDDataSet TheFindLettersByLetterIDDataSet = new FindLettersByLetterIDDataSet();
        FindLetterParagraphByLetterIDDataSet TheFindLetterParagraphByLetterIDDataSet = new FindLetterParagraphByLetterIDDataSet();
        FindLetterByDescriptionDataSet TheFindLetterByDescriptionDataSet = new FindLetterByDescriptionDataSet();

        FindLetter FindLetter = new FindLetter();

        int gintParagraphNumber;
        
        public CreateNewParagraph()
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
            //setting up controls during form load
            if(MainWindow.gintLetterID != 0)
            {
                btnFind.IsEnabled = false;

                gintParagraphNumber = 0;

                TheFindLettersByLetterIDDataSet = TheLettersClass.FindLettersByLetterID(MainWindow.gintLetterID);

                txtLetterDescription.Text = TheFindLettersByLetterIDDataSet.FindLettersByLetterID[0].LetterDescription;

                FindLetterParagraph();
            }

            FindLetter.Show();
        }
        private void FindLetterParagraph()
        {
            int intCounter;
            int intNumberOfRecords;

            TheFindLetterParagraphByLetterIDDataSet = TheLettersClass.FindLetterParagraphByLetterID(MainWindow.gintLetterID);

            intNumberOfRecords = TheFindLetterParagraphByLetterIDDataSet.FindLetterParagraphByLetterID.Rows.Count - 1;

            if(intNumberOfRecords > -1)
            {
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    if(TheFindLetterParagraphByLetterIDDataSet.FindLetterParagraphByLetterID[intCounter].ParagraphNumber > gintParagraphNumber)
                    {
                        gintParagraphNumber = TheFindLetterParagraphByLetterIDDataSet.FindLetterParagraphByLetterID[intCounter].ParagraphNumber;
                    }
                }

                gintParagraphNumber++;
            }
            else
            {
                gintParagraphNumber = 1;
            }

            txtParagraphNo.Text = Convert.ToString(gintParagraphNumber);
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //setting variable
            int intRecordsReturned;
            string strLetterName;

            strLetterName = txtLetterDescription.Text;
            if(strLetterName == "")
            {
                TheMessagesClass.ErrorMessage("Letter Name Was Not Entered");
                return;
            }

            TheFindLetterByDescriptionDataSet = TheLettersClass.FindLetterByDescription(strLetterName);

            intRecordsReturned = TheFindLetterByDescriptionDataSet.FindLetterByDescription.Rows.Count;

            if(intRecordsReturned == 0)
            {
                TheMessagesClass.ErrorMessage("No Records Were Found");
                return;
            }
            else if(intRecordsReturned > 1)
            {
                TheMessagesClass.InformationMessage("To Many Records Were Returned, Please Refine The Information");
                return;
            }
            else if(intRecordsReturned == 1)
            {
                gintParagraphNumber = 0;

                MainWindow.gintLetterID = TheFindLetterByDescriptionDataSet.FindLetterByDescription[0].LetterID;
                MainWindow.gstrLetterDescription = strLetterName;

                FindLetterParagraph();
            }
        }

        private void txtLetterDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.gstrLetterDescription = txtLetterDescription.Text;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strParagraph;
            DateTime datTransactionDate = DateTime.Now;
            string strValueForValidation;
            bool blnFatalError = false;
            string strErrorMessage = "";
            int intCounter;
            int intNumberOfRecords;
            bool blnParagraphExists = false;

            try
            {
                strValueForValidation = txtParagraphNo.Text;
                blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnFatalError == true)
                {
                    strErrorMessage += "The Paragraph Number is not an Integer\n";
                }
                else
                {
                    gintParagraphNumber = Convert.ToInt32(strValueForValidation);
                }

                strParagraph = txtParagraphText.Text;
                if (strParagraph == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Paragraph Was Not Entered\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                //checking to see if the string or paragraph number exist
                TheFindLetterParagraphByLetterIDDataSet = TheLettersClass.FindLetterParagraphByLetterID(MainWindow.gintLetterID);

                intNumberOfRecords = TheFindLetterParagraphByLetterIDDataSet.FindLetterParagraphByLetterID.Rows.Count - 1;

                if(intNumberOfRecords > -1)
                {
                    
                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        if(gintParagraphNumber == TheFindLetterParagraphByLetterIDDataSet.FindLetterParagraphByLetterID[intCounter].ParagraphNumber)
                        {
                            blnParagraphExists = true;
                        }
                        else if (strParagraph.ToUpper() == TheFindLetterParagraphByLetterIDDataSet.FindLetterParagraphByLetterID[intCounter].ParagraphText.ToUpper())
                        {
                            blnParagraphExists = true;
                        }
                    } 
                    
                }

                if(blnParagraphExists == true)
                {
                    TheMessagesClass.ErrorMessage("Either the Paragraph Number Exists or\nthe Paragraph Text Exists For This Letter");
                    return;
                }

                blnFatalError = TheLettersClass.InsertParagraph(gintParagraphNumber, MainWindow.gintLetterID, strParagraph, MainWindow.TheVerifyLogonDataSet.VerifyLogon[0].EmployeeID);

                if (blnFatalError == true)
                    throw new Exception();

                gintParagraphNumber++;
                txtParagraphNo.Text = Convert.ToString(gintParagraphNumber);
                txtParagraphText.Text = "";
                TheMessagesClass.InformationMessage("Paragraph Saved");
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Letters WPF // Create New Paragraph // Add Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void rdoYes_Checked(object sender, RoutedEventArgs e)
        {
            btnAdd.IsEnabled = true;
        }

        private void rdoNo_Checked(object sender, RoutedEventArgs e)
        {
            btnAdd.IsEnabled = false;
        }
    }
}
