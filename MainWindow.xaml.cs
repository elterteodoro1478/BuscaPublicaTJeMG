using BuscaPublicaTJeMG.Models;
using BuscaPublicaTJeMG.Repository;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BuscaPublicaTJeMG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int QtdeRegistros = 0;
        int QtdeSucesso = 0;
        int QtdeErro = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
        }


        private void MensagemProcessamento(bool IsEnabled, string Detalhes)
        {
            for (int i = 1; i < 30; i++)
            {
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        btnProcesso.IsEnabled = IsEnabled;
                        txt_CNPJ.IsEnabled = IsEnabled;

                        lbl_qtde_Total.Content = "Total a processar: " + QtdeRegistros.ToString();
                        lbl_qtde_sucesso.Content = "Lançados com sucesso:  " + QtdeSucesso.ToString();
                        lbl_qtde_erro.Content = "Erros ao lançar: " + QtdeErro.ToString();
                        lbl_Detalhe.Content = Detalhes.Replace("VER DETALHES DO PROCESSO", "");

                    }), DispatcherPriority.ContextIdle);
                }
                catch { }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string resposta = "OK";

            MensagemProcessamento(false, "Iniciando o browser");

            ConexaoBrowser conn = new ConexaoBrowser();
            IWebDriver browser = conn.NavegarChrome("https://pje-consulta-publica.tjmg.jus.br/");

            resposta = browser == null ? "Erro ao criar instancia do browser" : "OK";

            MensagemProcessamento(false, "Realizando a busca de dados");
            resposta = resposta == "OK" ? ConsultaByCNPJ(browser, 300, 1000, txt_CNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", "")) : resposta;

            MensagemProcessamento(false, "Realizador a contagem de registros");
            QtdeRegistros = 0;
            resposta = resposta == "OK" ? GetQtdeRegistros(browser, 60, 1000, out QtdeRegistros) : resposta;

            MensagemProcessamento(false, "Incluindo processos");
            int QtdeRegistrosCad = 0;
            resposta = resposta == "OK" ? SetJanelas(browser, 3, 1000, out QtdeRegistrosCad) : resposta;

            MensagemProcessamento(true, "Fim do processo!!!");
            MessageBox.Show("Fim do processo!");

            try
            {
                browser.Quit();
            }
            catch { }
        }

        private string ConsultaByCNPJ(IWebDriver browser, int qtdeTentativas, int TempoEspera, string CNPJ)
        {
            string retorno = "Erro";

            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();


                    IWebElement documentoParte = browser.FindElement(By.Id("fPP:dpDec:documentoParte"));
                    documentoParte.Click();

                    List<IWebElement> rdDocs = browser.FindElements(By.Name("tipoMascaraDocumento")).ToList();
                    rdDocs[1].Click();
                    Task.Delay(TempoEspera).Wait();

                    documentoParte.Clear();

                    for (int i = 0; i < CNPJ.Length; i++)
                    {
                        documentoParte.SendKeys(CNPJ[i].ToString());
                        Task.Delay(300).Wait();
                    }



                    Task.Delay(TempoEspera).Wait();
                    IWebElement searchProcessos = browser.FindElement(By.Id("fPP:searchProcessos"));
                    searchProcessos.Click();


                    Tentativas = qtdeTentativas;
                    retorno = "OK";
                }
                catch (Exception ex)
                {

                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }

        private string GetQtdeRegistros(IWebDriver browser, int qtdeTentativas, int TempoEspera, out int QtdeRegistros)
        {
            string retorno = "Erro";

            QtdeRegistros = 0;

            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();

                    List<IWebElement> footer1 = browser.FindElements(By.ClassName("rich-table-footer")).ToList();

                    List<IWebElement> span1 = footer1[0].FindElements(By.TagName("span")).ToList();

                    string[] subs = span1[0].Text.Split(' ');
                    string qtde = subs[0];
                    QtdeRegistros = Convert.ToInt32(qtde.Trim());

                    retorno = QtdeRegistros > 0 ? "OK" : "0 resultados encontrados";
                    Tentativas = qtdeTentativas;
                }
                catch (Exception ex)
                {

                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                    QtdeRegistros = 0;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }

        private string SetJanelas(IWebDriver browser, int qtdeTentativas, int TempoEspera, out int QtdeReg)
        {
            string retorno = "Erro";

            QtdeReg = 0;
            QtdeSucesso = 0;
            QtdeErro = 0;
            string Detalhes = "";

            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();

                    Detalhes = "";

                    IWebElement Tabela1 = browser.FindElement(By.Id("fPP:processosTable:tb"));
                    List<IWebElement> linhas1 = Tabela1.FindElements(By.TagName("tr")).ToList();

                    int QtdeLinhas = linhas1.Count();

                    int RegistroAtual = 0;

                    do
                    {
                        Contexto _contexto = new Contexto();
                        FechaJanelas(browser, 5, 100);

                        IWebElement Tabela2 = browser.FindElement(By.Id("fPP:processosTable:tb"));
                        List<IWebElement> linhas2 = Tabela2.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> lnk1 = linhas2[RegistroAtual].FindElements(By.TagName("a")).ToList();

                        Detalhes = "Processando: " + linhas2[RegistroAtual].Text;
                        MensagemProcessamento(false, Detalhes);

                        lnk1[0].Click();

                        try
                        {
                            EsperaAbrirJanela(browser, 30, 1000, 1);

                            // 
                            int IdProcesso = 0;
                            string ret1 = IncluirProcesso(browser, 30, 1000, _contexto, out IdProcesso);

                            if (ret1.Contains("OK"))
                            {
                                // Pega polos os ativos
                                IncluirParticanteAtivo(browser, 5, 1000, _contexto, IdProcesso);

                                // Pega polos os Passivos
                                IncluirParticantePassivo(browser, 5, 1000, _contexto, IdProcesso);

                                // Pega as Movimentações/Andamentos
                                IncluirMovimentacao(browser, 5, 1000, _contexto, IdProcesso);
                            }

                            if (ret1.Contains("OK"))
                            {
                                QtdeSucesso++;
                            }
                            else
                            {
                                QtdeErro++;
                            }

                            _contexto.Dispose();

                            FechaJanelas(browser, 5, 100);
                            QtdeReg++;
                        }
                        catch (Exception ex)
                        {
                            QtdeErro++;
                            FechaJanelas(browser, 5, 100);
                        }

                        MensagemProcessamento(false, Detalhes);

                        RegistroAtual++;

                    } while (RegistroAtual < QtdeLinhas);


                    Tentativas = qtdeTentativas;

                    retorno = "OK";
                }
                catch (Exception ex)
                {
                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }

        private void FechaJanelas(IWebDriver browser, int qtdeTentativas, int TempoEspera)
        {
            int c = browser.WindowHandles.Count();
            string Janela = "";
            do
            {
                try
                {
                    Janela = browser.WindowHandles[c];
                    browser.SwitchTo().Window(Janela).Close();
                }
                catch { }
                c--;
            } while (c > 0);

            Task.Delay(2000).Wait();
            browser.SwitchTo().Window(browser.WindowHandles[0]);
        }

        private void EsperaAbrirJanela(IWebDriver browser, int qtdeTentativas, int TempoEspera, int Index = 1)
        {
            int ContaJanelas = browser.WindowHandles.Count();
            string Janela = "";
            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();

                    Janela = browser.WindowHandles[Index];
                    browser.SwitchTo().Window(Janela);
                    Tentativas = qtdeTentativas;
                    ContaJanelas = browser.WindowHandles.Count();
                }
                catch
                {
                    Tentativas++;
                    ContaJanelas = browser.WindowHandles.Count();
                }


            } while (ContaJanelas > 1 && Tentativas < qtdeTentativas);

            Task.Delay(2000).Wait();

        }

        private string IncluirProcesso(IWebDriver browser, int qtdeTentativas, int TempoEspera, Contexto _contexto, out int IdProcesso)
        {
            string retorno = "Erro";
            IdProcesso = 0;
            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();

                    //rich - stglpanel
                    List<IWebElement> pnDadosProcesso = browser.FindElements(By.ClassName("rich-stglpanel-body")).ToList();

                    List<IWebElement> dados = pnDadosProcesso[0].FindElements(By.ClassName("propertyView")).ToList();

                    CultureInfo regiaoInfo = new CultureInfo("pt-BR");

                    List<IWebElement> dNumeroProcesso = dados[0].FindElements(By.TagName("div")).ToList();
                    string NumeroProcesso = dNumeroProcesso[dNumeroProcesso.Count - 1].Text;

                    ProcessoRep repProcesso = new ProcessoRep(_contexto);
                    Processo proc = repProcesso.SelecionarByNumeroProcesso(NumeroProcesso);

                    if (proc == null)
                    {
                        proc = new Processo();

                        proc.NumeroProcesso = NumeroProcesso;

                        List<IWebElement> dDataDistribuicao = dados[1].FindElements(By.TagName("div")).ToList();
                        proc.DataDistribuicao = Convert.ToDateTime(dDataDistribuicao[dDataDistribuicao.Count - 1].Text, regiaoInfo);

                        List<IWebElement> dClasseJuridicial = dados[2].FindElements(By.TagName("div")).ToList();
                        proc.ClasseJuridicial = dClasseJuridicial[dClasseJuridicial.Count - 1].Text;

                        List<IWebElement> dAssunto = dados[3].FindElements(By.TagName("div")).ToList();
                        proc.Assunto = dAssunto[dAssunto.Count - 1].Text;

                        List<IWebElement> dJurisdicao = dados[4].FindElements(By.TagName("div")).ToList();
                        proc.Jurisdicao = dJurisdicao[dJurisdicao.Count - 1].Text;

                        List<IWebElement> dOrgaoJulgador = dados[6].FindElements(By.TagName("div")).ToList();
                        proc.OrgaoJulgador = dOrgaoJulgador[dOrgaoJulgador.Count - 1].Text;

                        List<IWebElement> dNumeroProcessoReferencia = dados[7].FindElements(By.TagName("div")).ToList();
                        proc.NumeroProcessoReferencia = dNumeroProcessoReferencia[dNumeroProcessoReferencia.Count - 1].Text;

                        retorno = repProcesso.Incluir(proc);
                        retorno = retorno.Contains("OK") ? "OK" : retorno;
                        IdProcesso = !retorno.Contains("OK") ? 0 : repProcesso.SelecionarByNumeroProcesso(proc.NumeroProcesso).Id;

                    }
                    else
                    {
                        retorno = "OK";
                        IdProcesso = proc.Id;
                    }
                    Tentativas = qtdeTentativas;
                }
                catch (Exception ex)
                {

                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                    IdProcesso = 0;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }

        private string IncluirParticanteAtivo(IWebDriver browser, int qtdeTentativas, int TempoEspera, Contexto _contexto, int IdProcesso)
        {
            string retorno = "Erro";

            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();

                    // Pega polos os ativos
                    ParticipanteRep repParticanteAtivo = new ParticipanteRep(_contexto);
                    List<IWebElement> ltbodyAtivo = browser.FindElements(By.TagName("tbody")).ToList();

                    string TabelaPoloAtivo = "";
                    foreach (IWebElement item in ltbodyAtivo)
                    {
                        if (item.GetAttribute("id").ToString().Contains(":processoPartesPoloAtivoResumidoList:tb"))
                        {
                            TabelaPoloAtivo = item.GetAttribute("id").ToString();
                            break;
                        }
                    }

                    IWebElement ListaPoloAtivo = browser.FindElement(By.Id(TabelaPoloAtivo));
                    List<IWebElement> ItemsPolosAtivo = ListaPoloAtivo.FindElements(By.TagName("tr")).ToList();

                    foreach (IWebElement item in ItemsPolosAtivo)
                    {
                        Participante particante = new Participante();
                        List<IWebElement> spans1 = item.FindElements(By.TagName("span")).ToList();
                        string TipoDocto = spans1[0].Text.ToUpper().Contains("CPF") ? "CPF" : "CNPJ";
                        int TamanhoDocto = TipoDocto == "CPF" ? 14 : 18;
                        string[] ativoList = spans1[0].Text.Replace("- CPF", ";").Replace("- CNPJ", ";").Split(";");
                        particante.Nome = ativoList[0].TrimStart().TrimEnd();
                        particante.Documento = ativoList[1].Replace(":", "").TrimStart().TrimEnd().Substring(0, TamanhoDocto);
                        particante.TipoDocumento = TipoDocto;
                        particante.Polo = "Ativo";
                        particante.IdProcesso = IdProcesso;
                        retorno = repParticanteAtivo.Incluir(particante);
                    }

                    Tentativas = qtdeTentativas;
                }
                catch (Exception ex)
                {

                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }

        private string IncluirParticantePassivo(IWebDriver browser, int qtdeTentativas, int TempoEspera, Contexto _contexto, int IdProcesso)
        {
            string retorno = "Erro";

            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();

                    // Pega polos os Passivos
                    ParticipanteRep repParticantePassivo = new ParticipanteRep(_contexto);
                    List<IWebElement> ltbodyPassivo = browser.FindElements(By.TagName("tbody")).ToList();

                    string TabelaPoloPassivo = "";
                    foreach (IWebElement item in ltbodyPassivo)
                    {
                        if (item.GetAttribute("id").ToString().Contains("processoPartesPoloPassivoResumidoList:tb"))
                        {
                            TabelaPoloPassivo = item.GetAttribute("id").ToString();
                            break;
                        }
                    }

                    IWebElement ListaPoloPassivo = browser.FindElement(By.Id(TabelaPoloPassivo));
                    List<IWebElement> ItemsPolosPassivo = ListaPoloPassivo.FindElements(By.TagName("tr")).ToList();

                    foreach (IWebElement item in ItemsPolosPassivo)
                    {
                        Participante particante = new Participante();
                        List<IWebElement> spans1 = item.FindElements(By.TagName("span")).ToList();
                        string TipoDocto = spans1[0].Text.ToUpper().Contains("CPF") ? "CPF" : "CNPJ";
                        int TamanhoDocto = TipoDocto == "CPF" ? 14 : 18;
                        string[] PassivoList = spans1[0].Text.Replace("- CPF", ";").Replace("- CNPJ", ";").Split(";");
                        particante.Nome = PassivoList[0].TrimStart().TrimEnd();
                        particante.Documento = PassivoList[1].Replace(":", "").TrimStart().TrimEnd().Substring(0, TamanhoDocto);
                        particante.TipoDocumento = TipoDocto;
                        particante.Polo = "Passivo";
                        particante.IdProcesso = IdProcesso;
                        retorno = repParticantePassivo.Incluir(particante);
                    }

                    Tentativas = qtdeTentativas;
                }
                catch (Exception ex)
                {

                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }

        private string IncluirMovimentacao(IWebDriver browser, int qtdeTentativas, int TempoEspera, Contexto _contexto, int IdProcesso)
        {
            string retorno = "Erro";

            int Tentativas = 0;
            do
            {
                try
                {
                    Task.Delay(TempoEspera).Wait();
                    CultureInfo regiaoInfo = new CultureInfo("pt-BR");
                    // Pega as Movimentaçoes/Andamentos  
                    MovimentacaoRep repMovimentacao = new MovimentacaoRep(_contexto);
                    List<IWebElement> ltbodyMovimentacao = browser.FindElements(By.TagName("tbody")).ToList();
                    string TabelaMovimentacao = "";
                    foreach (IWebElement item in ltbodyMovimentacao)
                    {
                        if (item.GetAttribute("id").ToString().Contains(":processoEvento:tb"))
                        {
                            TabelaMovimentacao = item.GetAttribute("id").ToString();
                            break;
                        }
                    }

                    IWebElement ListaMovimentacao = browser.FindElement(By.Id(TabelaMovimentacao));
                    List<IWebElement> ItemsMovimentacao = ListaMovimentacao.FindElements(By.TagName("tr")).ToList();

                    foreach (IWebElement item in ItemsMovimentacao)
                    {
                        Movimentacao movimentacao = new Movimentacao();
                        List<IWebElement> spans1 = item.FindElements(By.TagName("span")).ToList();

                        string[] MovimentacaoList = spans1[0].Text.Split("-");


                        movimentacao.DataMovimentacao = Convert.ToDateTime(MovimentacaoList[0].TrimStart().TrimEnd(), regiaoInfo);
                        movimentacao.Descricao = MovimentacaoList[1].TrimStart().TrimEnd();

                        movimentacao.IdProcesso = IdProcesso;
                        string ret3 = repMovimentacao.Incluir(movimentacao);
                    }

                    Tentativas = qtdeTentativas;
                }
                catch (Exception ex)
                {

                    Tentativas++;
                    retorno = "Erro: " + ex.Message;
                }
            }
            while (Tentativas < qtdeTentativas);

            return retorno;
        }


    }
}

