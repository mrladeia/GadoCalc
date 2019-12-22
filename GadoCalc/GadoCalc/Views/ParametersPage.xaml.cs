using GadoCalc.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using Xamarin.Forms;

namespace GadoCalc.Views
{
    [DesignTimeVisible(false)]
    public partial class ParametersPage : ContentPage
    {
        public ParametersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Parameters parametersItem = await App.Database.GetItemAsync(1);
            if (parametersItem != null)
            {
                EntryComissao.Text = parametersItem.Comissao.ToString();
                EntryValorFrete.Text = parametersItem.Frete.ToString();
            }                
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            try
            {
                checkFields();
                int valorFrete = Int32.Parse(EntryValorFrete.Text); //R$ (int)
                double comissao = Double.Parse(EntryComissao.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture); //% (double)

                Parameters item = new Parameters
                {
                    Id = 1,
                    Comissao = comissao,
                    Frete = valorFrete,
                };

                await App.Database.UpdateItemAsync(item);
            }
            catch (FormatException e)
            {
                await DisplayAlert("Atenção", "Verifique os campos!\n\n" + e.Message, "OK");
            }
            catch (System.InvalidOperationException e)
            {
                await DisplayAlert("Atenção", "Verifique os campos!\n\n" + e.Message, "OK");
            }
        }

        void checkFields()
        {
            if (EntryComissao.Text == null)
            {
                throw new System.InvalidOperationException("O campo Valor do Frete não pode ficar em branco!");
            }
            else if (EntryValorFrete.Text == null)
            {
                throw new System.InvalidOperationException("O campo Comissão não pode ficar em branco!");
            }
        }
    }
}