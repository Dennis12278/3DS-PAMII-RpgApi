using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Models;
using AppRpgEtec.Views.Usuarios;
using AppRpgEtec.Views.Personagens;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService _uService;
        public ICommand AutenticarCommand { get; set; }
        public ICommand RegistrarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }

        //ctor + TAB + TAB para criar o construtor
        public UsuarioViewModel()
        {
            _uService = new UsuarioService();
            InicializarCommand();
        }

        public void InicializarCommand()
        {
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }

        private Usuario usuario;

        #region AtributosPropriedades

        private string login = string.Empty;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string senha = string.Empty;

        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public async Task AutenticarUsuario()//Métodos para autentica um usuário
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uAutenticado = await _uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo(a) {uAutenticado.Username}.";

                    // Guardando dados do usuário para uso futuro
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    await Application.Current.MainPage
                        .DisplayAlert("Informação", mensagem, "OK");

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorretos :(", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task RegistrarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Senha;

                Usuario uRegistrado = await _uService.PostRegistrarUsuarioAsync(u);

                await Application.Current.MainPage
                    .DisplayAlert("Informação", "Usuário registrado com sucesso!", "OK");

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task DirecionarParaCadastro()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CadastroView());
        }
    }
}