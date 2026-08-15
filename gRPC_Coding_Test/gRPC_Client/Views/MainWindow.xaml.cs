using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GrpcCodingTest.Contracts;
using gRPC_Client.Controllers;
using gRPC_Client.Services;

namespace gRPC_Client.Views;

public partial class MainWindow : Window
{
    private readonly CalculatorController calculatorController = new(new GrpcCalculatorClient());

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SubmitButton_Click(object _sender, RoutedEventArgs _eventArgs)
    {
        var operation = GetSelectedOperation();
        var result = await calculatorController.CalculateAsync(LeftOperandTextBox.Text, RightOperandTextBox.Text, operation);
        ResponseTextBox.Text = result.Message;
    }

    private CalculationOperation? GetSelectedOperation()
    {
        return OperationComboBox.SelectedItem is ComboBoxItem { Tag: string selectedOperation }
            ? selectedOperation switch
            {
                "addition" => CalculationOperation.Addition,
                "subtraction" => CalculationOperation.Subtraction,
                "multiplication" => CalculationOperation.Multiplication,
                "division" => CalculationOperation.Division,
                _ => null
            }
            : null;
    }

    private void NumberTextBox_PreviewTextInput(object _sender, TextCompositionEventArgs _eventArgs)
    {
        if (_sender is TextBox textBox)
        {
            _eventArgs.Handled = !IsValidNumberInput(textBox, _eventArgs.Text);
        }
    }

    private void NumberTextBox_Pasting(object _sender, DataObjectPastingEventArgs _eventArgs)
    {
        if (_sender is not TextBox textBox || !_eventArgs.DataObject.GetDataPresent(DataFormats.Text))
        {
            _eventArgs.CancelCommand();
            return;
        }

        var pastedText = _eventArgs.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;
        if (!IsValidNumberInput(textBox, pastedText))
        {
            _eventArgs.CancelCommand();
        }
    }

    private static bool IsValidNumberInput(TextBox _textBox, string _newText)
    {
        var candidate = _textBox.Text.Remove(_textBox.SelectionStart, _textBox.SelectionLength)
            .Insert(_textBox.SelectionStart, _newText);
        return string.IsNullOrEmpty(candidate)
            || double.TryParse(candidate, NumberStyles.Float, CultureInfo.CurrentCulture, out _)
            || double.TryParse(candidate, NumberStyles.Float, CultureInfo.InvariantCulture, out _)
            || candidate is "-" or "+"
            || candidate.EndsWith(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal)
            || candidate.EndsWith(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal);
    }
}
