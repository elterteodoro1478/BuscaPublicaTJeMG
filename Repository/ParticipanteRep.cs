using BuscaPublicaTJeMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BuscaPublicaTJeMG.Repository
{
    public class ParticipanteRep
    {

        private readonly Contexto _contexto;

        public ParticipanteRep(Contexto context)
        {
            _contexto = context;
        }


        public Participante SelecionarById(int Id)
        {
            return _contexto.Participante.Where(c => c.Id == Id).FirstOrDefault();
        }

        public Participante SelecionarByDocumento(string Documento)
        {
            return _contexto.Participante.Where(c => c.Documento == Documento).FirstOrDefault();
        }

        public Participante SelecionarByNome(string Nome)
        {
            return _contexto.Participante.Where(c => c.Nome.ToUpper().Contains(Nome.ToUpper())).FirstOrDefault();
        }


        public Participante SelecionarByIdProcesso(int IdProcesso)
        {
            return _contexto.Participante.Where(c => c.IdProcesso == IdProcesso).FirstOrDefault();
        }


        public string Incluir(Participante participante)
        {
            int id = 0;

            try
            {
                if (participante.IdProcesso == null || participante.Nome == null)
                {
                    return "Informe o Id.Processo e data de Movimentação";
                }
                else
                {
                    var novo = _contexto.Participante.Where(c => c.IdProcesso == participante.IdProcesso && c.Documento == participante.Documento && c.Documento == participante.Documento).FirstOrDefault();

                    if (novo != null)
                    {
                        return "Já cadastrado";
                    }
                    else
                    {
                        _contexto.Participante.Add(participante);
                        _contexto.SaveChanges();

                        var novoParticipante = _contexto.Participante.Where(c => c.IdProcesso == participante.IdProcesso && c.Documento == participante.Documento && c.Documento == participante.Documento).FirstOrDefault();

                        return "OK:" + novoParticipante.Id.ToString();
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
