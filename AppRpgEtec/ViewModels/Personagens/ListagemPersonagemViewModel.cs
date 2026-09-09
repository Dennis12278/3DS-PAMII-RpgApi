using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;

        public ObservableCollection<Personagem> Personagens { get; set; }

        public ObservableCollection<TipoClasse> ListaTiposClasse { get; set; }

        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            pService = new PersonagemService(token);

            Personagens = new ObservableCollection<Personagem>();

            ListaTiposClasse = new ObservableCollection<TipoClasse>();

            _ = ObterPersonagens();

            _ = ObterClasses();

            NovoPersonagem = new Command(async () => { await ExibirCadastroPersonagem(); });
        }
        public ICommand NovoPersonagem { get; }

        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();

                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert( "Ops",ex.Message + " Detalhes: " + ex.InnerException,"Ok" );
            }
        }

        public async Task ObterClasses()
        {
            try
            {
                ListaTiposClasse = new ObservableCollection<TipoClasse>();
                ListaTiposClasse.Add( new TipoClasse() { Id = 1,Descricao = "Cavaleiro" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 2,Descricao = "Mago" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 3,Descricao = "Clérigo" });
                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops",ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        public async Task ExibirCadastroPersonagem()
        {
            try
            {
                await Shell.Current.GoToAsync("cadPersonagemView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message  + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}