using BuscaPublicaTJeMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BuscaPublicaTJeMG.Repository
{
    public class MovimentacaoRep
    {
        private readonly Contexto _contexto;

        public MovimentacaoRep(Contexto context)
        {
            _contexto = context;
        }

        public Movimentacao SelecionarById(int Id)
        {
            return _contexto.Movimentacao.Where(c => c.Id == Id).FirstOrDefault();
        }


        public Movimentacao SelecionarByIdProcessoDataMovimentacao(int IdProcesso, DateTime? DataMovimentacao)
        {
            return _contexto.Movimentacao.Where(c => c.IdProcesso == IdProcesso && c.DataMovimentacao == DataMovimentacao).FirstOrDefault();
        }


        public string Incluir(Movimentacao movimentacao)
        {
            int id = 0;

            try
            {
                if (movimentacao.IdProcesso == null || movimentacao.DataMovimentacao == null )
                {
                    return "Informe o Id.Processo e data de Movimentação";
                }
                else
                {
                    var novo = _contexto.Movimentacao.Where(c => c.IdProcesso == movimentacao.IdProcesso && c.DataMovimentacao == movimentacao.DataMovimentacao).FirstOrDefault();

                    if (novo != null)
                    {
                        return "Já cadastrado";
                    }
                    else
                    {
                        _contexto.Movimentacao.Add(movimentacao);
                        _contexto.SaveChanges();

                        var novaMovimentacao = _contexto.Movimentacao.Where(c => c.IdProcesso == movimentacao.IdProcesso && c.DataMovimentacao == movimentacao.DataMovimentacao).FirstOrDefault();

                        return "OK:" + novaMovimentacao.Id.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                return "Erro:" + ex.Message;
            }
        }

    }
}
