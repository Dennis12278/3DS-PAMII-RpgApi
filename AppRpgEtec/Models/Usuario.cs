using System;
using System.Collections.Generic;
using System.Text;

namespace AppRpgEtec.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordString { get; set; } = string.Empty;
        public string Perfil {  get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[]? Foto { get; set; } 
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
