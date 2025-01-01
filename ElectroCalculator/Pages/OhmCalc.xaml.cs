using ElectroCalculator.Helpers;


namespace ElectroCalculator.Pages;

public partial class OhmCalc : ContentPage
{
    public OhmCalc()
    {
        InitializeComponent();
    }
    private async void OnComputeClicked(object sender, EventArgs e)
    {
        string currentValue = currentEntry.Text;
        string voltageValue = voltageEntry.Text;
        string resistanceValue = resistanceEntry.Text;

        bool current = string.IsNullOrWhiteSpace(currentValue);
        bool voltage = string.IsNullOrWhiteSpace(voltageValue);
        bool resistance = string.IsNullOrWhiteSpace(resistanceValue);

        float ampere = 0, volt = 0, res = 0;

        try
        {
            if (!current) ampere = MetricStringConverter.ConvertToFloat(currentValue);
            if (!voltage) volt = MetricStringConverter.ConvertToFloat(voltageValue);
            if (!resistance) res = MetricStringConverter.ConvertToFloat(resistanceValue);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Nieprawidłowe dane wejściowe: {ex.Message}", "OK");
            return;
        }

        // Logika obliczeń
        if (current && !voltage && !resistance)
        {
            currentEntry.Text = MetricStringConverter.ConvertToMetricString((volt / res));
        }
        else if (!current && voltage && !resistance)
        {
            voltageEntry.Text = MetricStringConverter.ConvertToMetricString((ampere * res));
        }
        else if (!current && !voltage && resistance)
        {
            resistanceEntry.Text = MetricStringConverter.ConvertToMetricString((volt / ampere));
        }
        else
        {
            await DisplayAlert("Błąd", "Tylko jedno pole może być puste", "OK");
        }
    }
}
