namespace ElectroCalculator.Pages;

public partial class OhmCalc : ContentPage
{
    public OhmCalc()
    {
        InitializeComponent();
    }
    private void OnComputeClicked(object sender, EventArgs e)
    {
        string currentValue = currentEntry.Text;
        string voltageValue = voltageEntry.Text;
        string resistanceValue = resistanceEntry.Text;
        bool current = false;
        bool voltage = false;
        bool resistance = false;
        if (string.IsNullOrWhiteSpace(currentValue))
        { current = true; }
        if (string.IsNullOrWhiteSpace(voltageValue))
        { voltage = true; }
        if (string.IsNullOrWhiteSpace(resistanceValue))
        { resistance = true; }

        if (current && !voltage && !resistance)
        {
            currentEntry.Text = (float.Parse(voltageValue) / float.Parse(resistanceValue)).ToString();
        }
        else if (!current && voltage && !resistance)
        {
            voltageEntry.Text = (float.Parse(currentValue) * float.Parse(resistanceValue)).ToString();
        }
        else if (!current && !voltage && resistance)
        {
            resistanceEntry.Text = (float.Parse(voltageValue) / float.Parse(currentValue)).ToString();
        }
        else
        {
            DisplayAlert("Error", "Only one field can be empty", "OK");
        }


    }
}
