using AppRpgEtec.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppRpgEtec.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;

        private const string apiUrlBase = "https://rpgapi3ds2026-2-fsf6e4d0b2hjh7bc.mexicocentral-01.azurewebsites.net/Usuarios";

        //ctor + TAB atalho para criar um construtor

        public UsuarioService()
        {
            _request = new Request();
        }

        public async Task<Usuario> PostRegistrarUsuarioAsync(Usuario u)
        {
            string urlComplementar = "/Registrar";
            u.Id = await _request.PostReturnIntAsync(apiUrlBase + urlComplementar, u, string.Empty);

            return u;
        }

        public async Task<Usuario> PostAutenticarUsuarioAsync(Usuario u)
        {
            string urlComplementar = "/Autenticar";
            u = await _request.PostAsync<Usuario>(apiUrlBase + urlComplementar, u, string.Empty);

            return u;
        }
    }
}