using GadoCalc.Models;
using GadoCalc.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GadoCalc.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculatorPage : ContentPage
    {
        CalculatorViewModel viewModel;
        Parameters parametersItem;
        private IFormatProvider ptBR;

        public CalculatorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CalculatorViewModel();
            ptBR = CultureInfo.CreateSpecificCulture("pt-BR");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await App.Database.DeleteAllAsync();
            btnCalc.IsEnabled = false;
            parametersItem = await App.Database.GetItemAsync(1);
            if(parametersItem == null)
            {
                parametersItem = new Parameters
                {
                    Id = 1,
                    Comissao = 2.5,
                    Frete = 500,
                };

                await App.Database.InsertItemAsync(parametersItem);
            }
            SpanParameterComission.Text = parametersItem.Comissao.ToString("N", ptBR)+"%";
            SpanParameterFrete.Text = parametersItem.Frete.ToString("C", ptBR);
            btnCalc.IsEnabled = true;
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            try
            {
                checkFields();
                double valorCabeca = Double.Parse(EntryValorCabeca.Text); //R$
                double quantidade = Double.Parse(EntryQuantidade.Text); //num
                //int valorFrete = Int32.Parse(EntryValorFrete.Text); //R$ (int)
                //double comissao = Double.Parse(EntryComissao.Text.Replace(",","."), NumberStyles.Any, CultureInfo.InvariantCulture); //% (double)
                int valorFrete = parametersItem.Frete;
                double comissao = parametersItem.Comissao;

                double cabecaComissao = (comissao / 100) * valorCabeca; //((comissao / 100) * valorCabeca);
                SpanCabecaComissao.Text = cabecaComissao.ToString("0", ptBR);

                double cabecaFrete = valorFrete/quantidade;
                SpanCabecaFrete.Text = cabecaFrete.ToString("N", ptBR);

                SpanTotalDespesasCabeca.Text = (cabecaComissao + cabecaFrete).ToString("C", ptBR);

                //valor por cabeça
                double valorPorCabeca = cabecaComissao + cabecaFrete + valorCabeca;
                SpanValorTotalCabeca.Text = valorPorCabeca.ToString("C", ptBR);

                SpanValorFinal.Text = (valorPorCabeca * quantidade).ToString("C", ptBR);
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
            if (EntryValorCabeca.Text == null)
            {
                throw new System.InvalidOperationException("O campo Valor da Cabeça não pode ficar em branco!");
            }
            else if (EntryQuantidade.Text == null)
            {
                throw new System.InvalidOperationException("O campo Quantidade não pode ficar em branco!");
            }
        }
    }
}