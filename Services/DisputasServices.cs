using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgApi.Data;
using RpgApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace RpgApi.Services
{
    public class DisputasServices
    {
        private readonly DataContext _context;
        public DisputasServices(DataContext context)
        {
            _context = context;
        }

        public async Task<List<DisputaDto>> ObterDisputas()
        {
            var resultado = _context.Database.SqlQueryRaw<DisputaDto>(
                @"SELECT
            D.Id,
            at.Nome [Atacante],
            op.Nome AS Oponente,
            d.Tx_Narracao Narracao,
            us.Username NomeUsuarioAtacante,
            usOp.Username NomeUsuarioOponente
            FROM tb_disputas d
            INNER JOIN TB_PERSONAGENS at ON d.AtacanteId = at.Id
            INNER JOIN TB_PERSONAGENS op ON d.OponenteId = op.Id
            LEFT JOIN TB_USUARIOS us ON at.UsuarioId = us.Id
            LEFT JOIN TB_USUARIOS usOp ON op.UsuarioId = usOp.Id"
                    );

            return resultado.ToList();
        }
    }
}