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

        private Personagem personagemSelecionado;

        public Personagem PersonagemSelecionado
        {
            get
            {
                return personagemSelecionado;
            }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;

                    Shell.Current
                        .GoToAsync($"cadPersonagemView?pId={personagemSelecionado.Id}");
                }
            }
        }

        public ObservableCollection<TipoClasse> ListaTiposClasse { get; set; }

        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            pService = new PersonagemService(token);

            Personagens = new ObservableCollection<Personagem>();

            ListaTiposClasse = new ObservableCollection<TipoClasse>();

            _ = ObterPersonagens();

            _ = ObterClasses();

            NovoPersonagemCommand = new Command(async () => { await ExibirCadastroPersonagem(); });
            RemoverPersonagemCommand = new Command<Personagem>(async (Personagem p ) => { await RemoverPersonagem(p); });

        }
        public ICommand NovoPersonagemCommand { get; }

        public ICommand RemoverPersonagemCommand { get; set; }

        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();

                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert( "Ops",ex.Message + " Detalhes: " + ex.InnerException,"Ok" );
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
                await Application.Current.MainPage.DisplayAlert("Ops",ex.Message + " Detalhes: " + ex.InnerException, "Ok");
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
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message  + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        public async Task RemoverPersonagem(Personagem p)
        {
            try
            {
                if (await Application.Current.MainPage
                     .DisplayAlert("Confirmação", $"Confirma a remoção de {p.Nome}?", "Sim", "Não"))
                {

                    await pService.DeletePersonagemAsync(p.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Personagem removido com sucesso!", "Ok");

                    await ObterPersonagens();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}