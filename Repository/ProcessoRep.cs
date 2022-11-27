using BuscaPublicaTJeMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BuscaPublicaTJeMG.Repository
{
    public class ProcessoRep 
    {

        private readonly Contexto _contexto;

        public ProcessoRep(Contexto context)
        {
            _contexto = context;
        }

        public List<Processo> SelecionarTodos()
        {
           return  _contexto.Processo.ToList();
        }

        public Processo SelecionarByNumeroProcesso(string NumeroProcesso)
        {
            return _contexto.Processo.Where(c=> c.NumeroProcesso == NumeroProcesso).FirstOrDefault();
        }

        public Processo SelecionarById(int Id)
        {
            return _contexto.Processo.Where(c => c.Id == Id).FirstOrDefault();
        }


        public  string Incluir(Processo processo)
        {
            int id = 0;
            

            try
            {
                if (String.IsNullOrEmpty(processo.NumeroProcesso))
                {
                    return "Informe o Número Processo";
                }
                else
                {
                    var novo = _contexto.Processo.Where(c => c.NumeroProcesso == processo.NumeroProcesso ).FirstOrDefault();

                    if (novo != null)
                    {
                        return  "Já cadastrado";
                    }
                    else
                    {
                        _contexto.Processo.Add(processo);
                        _contexto.SaveChanges();

                        var novoProcesso = _contexto.Processo.Where(c => c.NumeroProcesso == processo.NumeroProcesso).FirstOrDefault();

                        return "OK:"+ novoProcesso.Id.ToString();
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
