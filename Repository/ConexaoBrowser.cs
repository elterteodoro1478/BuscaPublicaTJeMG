using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BuscaPublicaTJeMG.Repository
{
    public class ConexaoBrowser
    {

        public IWebDriver? CriaChrome(IWebDriver driverAtual, string pathChorme, string pathDownload, string Extension)
        {
            try
            {
                Directory.CreateDirectory(pathDownload);
                Directory.CreateDirectory(pathChorme);

                var chromeOptions = new ChromeOptions();

                chromeOptions.AddUserProfilePreference("block_third_party_cookies", true);
                chromeOptions.AddUserProfilePreference("download.default_directory", @"" + pathDownload + "");
                chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddUserProfilePreference("plugins.plugins_disabled", new[] { "Chrome PDF Viewer" });
                chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                chromeOptions.AddUserProfilePreference("download.directory_upgrade", true);

                chromeOptions.AddArgument("--start-maximized");
                chromeOptions.AddExcludedArgument("enable-automation");
                //chromeOptions.AddAdditionalCapability("useAutomationExtension", false);

                if (!String.IsNullOrWhiteSpace(Extension))
                    chromeOptions.AddExtension($@"{Extension}");

                var chromeDriverService = ChromeDriverService.CreateDefaultService(pathChorme);
                chromeDriverService.HideCommandPromptWindow = false;

                try
                {
                    driverAtual = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromSeconds(3600));
                }
                catch (Exception EX)
                {
                }
            }
            catch (Exception EX) { }

            return driverAtual;
        }


        public IWebDriver?  NavegarChrome(string link)
        {
            string CaminhoDriver = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            IWebDriver? retorno = CriaChrome(null, $@"{CaminhoDriver}", $@"{CaminhoDriver}\download", null);
            try
            {
                if (retorno != null)
                {
                    retorno.Navigate().GoToUrl(link);
                    Task.Delay(5000).Wait();
                }
            }
            catch(Exception ex)
            {
                retorno = null;
            }            

            return retorno;

        }

    }
}
