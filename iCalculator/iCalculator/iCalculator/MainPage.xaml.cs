using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:ExportFont("digital-7.ttf")]
namespace iCalculator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private decimal firstNumber;
        private string OperationName;
        private bool isOperationClicked = false;

        private void BtnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if(CLbResult.Text== "0" || isOperationClicked)
            {
                CLbResult.Text = button.Text;
                isOperationClicked = false;
            }
            else
            {
                CLbResult.Text += button.Text;
            }

                       
        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            CLbResult.Text = "0";
            isOperationClicked = false;
        }

        private void BtnX_Clicked(object sender, EventArgs e)
        {
            isOperationClicked = false;
            string number = CLbResult.Text;

            if(number!="0")
            {
                number = number.Remove(number.Length-1, 1);
                if(string.IsNullOrEmpty(number))
                {
                    CLbResult.Text = "0";
                }
                else
                {
                    CLbResult.Text = number;
                } 
            }
        }

        private async void BtnPercentage_Clicked(object sender, EventArgs e)
        {
            try
            {
                isOperationClicked = false;

                string number = CLbResult.Text;
                if (number != "0")
                {
                    decimal percentValue = Convert.ToDecimal(number);
                    string result = (percentValue / 100).ToString("0.##");
                    CLbResult.Text = result;
                }
            }
            catch (Exception ex)
            {
               await DisplayAlert("Error", ex.Message, "Ok");
            }

            
        }

        void BtnCommonOperation_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            isOperationClicked = true;
            OperationName = button.Text;
            firstNumber = Convert.ToDecimal(CLbResult.Text);

        }

        private void BtnEqual_Clicked(object sender, EventArgs e)
        {
            try
            {
                isOperationClicked = false;

                if (firstNumber != 0)
                {
                    decimal secondNumber = Convert.ToDecimal(CLbResult.Text);
                    decimal result = Calculate(firstNumber, secondNumber);
                    CLbResult.Text = result.ToString("0.##");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public decimal Calculate(decimal firstNumber, decimal secondNumber)
        {
            decimal result = 0;
            if (OperationName == "+" )
            {
                result = firstNumber + secondNumber;
            }
            else if (OperationName == "-")
            {
                result = firstNumber - secondNumber;
            }
            else if (OperationName == "*")
            {
                result = firstNumber * secondNumber;
            }
            else if (OperationName == "/")
            {
                result = firstNumber / secondNumber;
            }

            isOperationClicked = false;

            return result;
        }
    }
}
