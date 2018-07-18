/*
 * Student: Alejandro Gomez
 * Student Number: 045251129
 * Program: LAB4, Calculator
 * 
 * Note: For a list of specific additions to improve the calculator, please refer to the bottom
 * of the code in this file.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
        }
        //Form1 variables for operations, numbers and string holding
        double firstNum, secondNum, memory; //Used for calculations and storage for memory buttons.
        string operation; //Used to store the operator value, ie: +, -, *, etc
        string copy = "0"; //Used for the copy function on the Edit pulldown menu. Initialized to 0 to avoid abuse of the button before any copy is done.
        const int max = 16; //Max amount of numbers the user can input per operation 
        bool opInPr = false, displayReset = false, opBackToBackPress = false; //Flags, one for 'Operation In Progress' and one to indicate the display needs to be cleared.

        //Function to input number strings using the buttons. Called by every number button press
        void numberInput(string num)
        {
            if (txtDisplay.Text == "0")     //If '0' on display, replaces that with pressed number
            {
                txtDisplay.Text = num;
                displayReset = false; //Added to clear issues when clearing everything right after pressing '='. Confused the bool flags.
            }

            else if (displayReset)      //If display needs to reset, it replaces the number on it. Used after pressing the '=' sign 
            {
                txtDisplay.Clear();
                txtDisplay.Text = num;
                displayReset = false;
                
            }

            else if (txtDisplay.Text.Length < max)      //Makes sure user doesn't input more than 12 numbers. 
                txtDisplay.Text = txtDisplay.Text + num;
            opBackToBackPress = false;

        }

        //This here is the operator function, called when an operator button is pressed. It is set up so that the operators can be chained
        //and it works just like the Windows 7 calculator, chaining operations and showing previous steps until the '=' is used.
        void basicOperation(string op)  
        {

            if (opInPr == true) //If an operation is in progress and another operator is pressed, this executes the operation (like with '=' button) but
            {                   //doesn't clear output. Allows you to chain operations.
                if (opBackToBackPress == true)
                {
                    operation = op;
                    txtPreviousOps.Text = Convert.ToString(firstNum) + " " + op;
                    return;
                }
                txtPreviousOps.Text += " " + txtDisplay.Text + " " + op + " ";
                equalsFunc();
                firstNum = Convert.ToDouble(txtDisplay.Text);
                operation = op;
                opBackToBackPress = true;
            }
            else                //If no operaton is in progress, stores value for number, activates opInPr flag and waits for input.
            {
                firstNum = Convert.ToDouble(txtDisplay.Text);
                operation = op;
                txtPreviousOps.Text += Convert.ToString(firstNum) + " " + op;
                opInPr = true;
                opBackToBackPress = true;
            }
            displayReset = true;//Raises this flag so the next operation can clear the value previously set.
        }

        void equalsFunc()    //Function to execute operations. Main calculations happen here. Called by operation buttons and '=' sign
        {
            secondNum = Convert.ToDouble(txtDisplay.Text);
            txtDisplay.Clear();

            switch (operation)  //chooses operation to perform
            {
                case "+":
                    {
                        txtDisplay.Text = Convert.ToString(firstNum + secondNum);
                        break;
                    }

                case "-":
                    {
                        txtDisplay.Text = Convert.ToString(firstNum - secondNum);
                        break;
                    }

                case "*":
                    {
                        txtDisplay.Text = Convert.ToString(firstNum * secondNum);
                        break;
                    }

                case "/":
                    {
                        if (secondNum == 0) //Error handling for division by zero.
                        {
                            MessageBox.Show("Error, trying to divide by 0");
                            displayReset = true;
                            txtDisplay.Text = "∞";
                            break; //Exits and resets display when dividing by 0
                        }
                        else
                            txtDisplay.Text = Convert.ToString(firstNum / secondNum);
                        break;
                    }

            }

        }

        //Function for single operation buttons (1/x, Square root)
        void singleOperation(string op)
        {
            firstNum = Convert.ToDouble(txtDisplay.Text);
            switch (op)
            {
                case "SR":
                    {
                        secondNum = Convert.ToDouble(txtDisplay.Text);
                        if (secondNum < 0)  //Error handling for squaring a negative number.
                        {
                            MessageBox.Show("Error, trying to find the root of a negative number");
                            displayReset = true;
                            txtDisplay.Text = "Invalid Input";
                            break;
                        }
                        else
                             txtDisplay.Text = Convert.ToString(Math.Sqrt(firstNum));
                        break;
                    }
                case "1x":
                    {
                        if (firstNum == 0)  //Error handling for division by zero.
                        {
                            MessageBox.Show("Error, trying to divide by zero");
                            displayReset = true;
                            txtDisplay.Text = "∞";
                            break;
                        }
                        else
                            txtDisplay.Text = Convert.ToString(1 / firstNum);
                        break;
                    }
            }
            displayReset = true;

        }

        //Function for message box for all unused tool strip buttons
        void future()
        {
            MessageBox.Show("Will be developed!");
        }


        //MC buttons operations start here:
        private void btnMC_Click(object sender, EventArgs e)
        {
            memory = 0;
            lblM.Visible = false;   //Clears memory variable and removes 'M' label.
        }

        private void btnMS_Click(object sender, EventArgs e)
        {
            memory = Convert.ToDouble(txtDisplay.Text); 
            lblM.Visible = true;    //Stores value in memory, enables 'M' label and resets display on next input.
            displayReset = true; 
        }

        private void btnMR_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = Convert.ToString(memory); //Recalls memory stored number to display.
        }

        private void btnMplus_Click(object sender, EventArgs e)
        {
            if(memory != 0)
                 memory = memory + Convert.ToDouble(txtDisplay.Text); //Adds amount to memory stored number
            displayReset = true;
        }

        private void btnMminus_Click(object sender, EventArgs e)
        {
            if(memory != 0)
                memory = memory - Convert.ToDouble(txtDisplay.Text);  //Substracts from memory stored number
            displayReset = true;
        }
        //End of MC Buttons code.

        //Number buttons code starts here. They all basically call my number input function and send the string value of their number.
        private void btn0_Click(object sender, EventArgs e) 
        {
            numberInput("0");  
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            numberInput("1");
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            numberInput("2");
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            numberInput("3");
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            numberInput("4");
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            numberInput("5");
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            numberInput("6");
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            numberInput("7");
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            numberInput("8");
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            numberInput("9");
        }
        //Number buttons code ends here.

        //Erase buttons code here:
        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();     //Clears text field only.
            txtDisplay.Text = "0";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();     //Clears everything on calculator
            txtDisplay.Text = "0";
            txtPreviousOps.Clear();
            firstNum = 0;
            secondNum = 0;
        }

        private void btnDeleteOne_Click(object sender, EventArgs e)
        {
            if (!displayReset) //Button is disable if the calculator is showing the result of an operation. Like Win7 calculator.
            {
                if (txtDisplay.Text != "0" && txtDisplay.Text.Length > 1)
                {
                    txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1); //Deletes last digit
                }
                else if (txtDisplay.Text.Length == 1) //Displays zero when you delete last digit
                    txtDisplay.Text = "0";
            }
        }
        //End of erase buttons code.

        //Code for sign change:
        private void btnChangeSign_Click(object sender, EventArgs e)
        {
            
            if (txtDisplay.Text != "0") //No need for sign change 
            {
                if(txtDisplay.Text[0] == '-')
                    txtDisplay.Text = txtDisplay.Text.Remove(0,1);
                else
                txtDisplay.Text = txtDisplay.Text.Insert(0,"-");
            }
        }

        //Adds decimal point if there's none already.
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Contains(".") == false)
            {
                if (opInPr || displayReset)
                {
                    txtDisplay.Text = "0.";
                    displayReset = false;
                }
                else
                    txtDisplay.Text = txtDisplay.Text + ".";
            }
            else if (txtDisplay.Text == "0")
                txtDisplay.Text = "0.";
            else if (opInPr || displayReset) //Mimics Win7 Calc, when pressing '.' its a short for '0.'
            {
                txtDisplay.Text = "0.";
                displayReset = false;
            }
        }

        //Operations code here. Just calls the basic operation function for 2 input operations.
        private void btnPlus_Click(object sender, EventArgs e)
        {
            basicOperation("+");
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            basicOperation("-");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            basicOperation("*");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            basicOperation("/");
        }

        //This is the Equals button code. Slightly different from the operations ones, tweaked to reset some things like the Win7 Calculator does.
        private void btnEquals_Click(object sender, EventArgs e)
        {
            equalsFunc();       //Calls for operations to execute
            displayReset = true; 
            opInPr = false;     //Ends operation in progress flag.
            txtPreviousOps.Clear(); //Finally, '=' clears text fields to allow for other operations.
        }

        //These buttons call for single input operations, so they call a different function ('singleOperation()').
        private void btnSqrt_Click(object sender, EventArgs e)
        {
            singleOperation("SR");
        }

        private void btnDivide1by_Click(object sender, EventArgs e)
        {
            singleOperation("1x");
        }
        
        //Button for % sign. It's a bit of a special case so I coded it here.
        private void btnPercentage_Click(object sender, EventArgs e)
        {
            if (opInPr)
            {   //This button gets the user put percentage of the first number used and performs an operation with it (+,-,*,/, etc). AN OPERATION MUST BE IN PROGRESS!
                secondNum = (Convert.ToDouble(txtDisplay.Text) / 100) * firstNum;
                txtDisplay.Text = Convert.ToString(secondNum);
                txtPreviousOps.Text = Convert.ToString(firstNum) + " " + operation + " " + Convert.ToString(secondNum);
            }
            else //This is to mimic the Win7 Calc. When button is pressed without no previous operation, it defaults to 0.
            {
                txtPreviousOps.Text = "0";
                txtDisplay.Text = "0";
            }

        }

        //Tool Strip code down here. Only About, Copy and Paste are functional.
        private void tstrAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program by Alejandro Gomez.\nMade for PRG455.");
        }

        private void tstrCopy_Click(object sender, EventArgs e)
        {
            copy = txtDisplay.Text;
            tstrPaste.ForeColor = SystemColors.ControlText; //Paste button starts grayed out so user knows there's nothing to paste
        }                                               //Once a copy is made, paste button changes color to indicate as such.
        
        private void tstrPaste_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = copy;
        }

        //All unused tool strip click event code is down here.
        private void tstrStandard_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrScientific_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrProgrammer_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrStatistics_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrViewHistory_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrDigitGrouping_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrBasic_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrUnitConversion_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrDateCalculation_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrWorksheets_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrMortgage_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrVehicleLease_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrFuelEconMPG_Click(object sender, EventArgs e)
        {
            future();
        }

        private void tstrFuelEconLKm_Click(object sender, EventArgs e)
        {
            future();
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            future();
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            future();
        }

        //Code for context menus is here:
        private void witMC_Click(object sender, EventArgs e)
        {
            MessageBox.Show("MC button clears the value that is stored in the memory using the MS button.");
        }

        private void witMR_Click(object sender, EventArgs e)
        {
            MessageBox.Show("MR button recalls the value stored in the memory to the display.");
        }

        private void wisMS_Click(object sender, EventArgs e)
        {
            MessageBox.Show("MS button saves the value currently on the display to the memory, for later use.");
        }

        private void wisMPlus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("M+ button adds the value currently on the display to the value stored on the memory.");
        }

        private void wisMMinus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("M- button substracts the value currently on the display from the value stored on the memory.");
        }

    }
}

/*
 * Additions to the basic code:
 * -User can chain operations together while pressing the operator buttons. This emulates the Win7 calculator.
 *  Operations keep the chain going until the user presses the '=' button. It's all updated on the top text box.
 *
 * -Paste button in the toolbar is greyed out to indicate no Copy has been made. Once a copy is made it changes to an active color
 *  to indicate that. The button is always usable, however.
 *  
 * -Created color images for the form and buttons to better match the Win7 calculator color scheme (light blues, with soft gradients). Didn't
 *  match very well but it's still pleasant. Was not able to figure out the gold highlights on button presses.
 * 
 * -Disabled the 'maximize window' control and the ability for the user to resize the form. Mimics Win7 calculator.
 * 
 * -Bug: When chaining operations after using % button, the % value repeats itself in the previous operations text, but the total value is 
 *  correct on the total display box.
 */
