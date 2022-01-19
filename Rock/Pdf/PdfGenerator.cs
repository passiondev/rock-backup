using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuppeteerSharp;

namespace Rock.Pdf
{
    /// <summary>
    /// Class PDFGenerator.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class PdfGenerator : IDisposable
    {
        private Browser _puppeteerBrowser;
        private Page _puppeteerPage;

        private static int _lastProgressPercentage = 0;

        public static void EnsureChromeEngineInstalled()
        {
            var browserDownloadPath = System.Web.Hosting.HostingEnvironment.MapPath( "~/App_Data/ChromeEngine" );
            Directory.CreateDirectory( browserDownloadPath );

            var browserFetcherOptions = new BrowserFetcherOptions
            {
                Product = Product.Chrome,
                Path = browserDownloadPath
            };

            _lastProgressPercentage = 0;

            using ( var browserFetcher = new BrowserFetcher( browserFetcherOptions ) )
            {
                browserFetcher.DownloadProgressChanged += BrowserFetcher_DownloadProgressChanged;
                browserFetcher.DownloadAsync().Wait();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfGenerator"/> class.
        /// </summary>
        public PdfGenerator()
        {
            InitializeChromeEngine();
        }

        /// <summary>
        /// Initializes the chrome engine.
        /// </summary>
        private void InitializeChromeEngine()
        {
            EnsureChromeEngineInstalled();

            var launchOptions = new LaunchOptions
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions { Width = 1280, Height = 1024, DeviceScaleFactor = 1 },
                //    ExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
            };

            _puppeteerBrowser = Puppeteer.LaunchAsync( launchOptions ).Result;
            _puppeteerPage = _puppeteerBrowser.NewPageAsync().Result;
            //_puppeteerPage.EmulateMediaTypeAsync( PuppeteerSharp.Media.MediaType.Screen ).Wait();
        }

        /// <summary>
        /// Occurs when [download progress changed].
        /// </summary>
        //public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        private static void BrowserFetcher_DownloadProgressChanged( object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            if ( e.ProgressPercentage != _lastProgressPercentage )
            {
                _lastProgressPercentage = e.ProgressPercentage;
                System.Diagnostics.Debug.WriteLine( $"Downloading PdfGenerator ChromeEngine:  {e.ProgressPercentage}%" );
            }
        }

        /// <summary>
        /// Renders the PDF document.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Stream.</returns>
        public Stream RenderPDFDocument( string html )
        {
            _puppeteerPage.SetContentAsync( html ).Wait();

            /*MarginOptions marginOptions = new MarginOptions
            {
                Bottom = $"{_reportSettings.PDFSettings.MarginBottomMillimeters ?? 15}mm",
                Left = $"{_reportSettings.PDFSettings.MarginLeftMillimeters ?? 10}mm",
                Top = $"{_reportSettings.PDFSettings.MarginTopMillimeters ?? 10}mm",
                Right = $"{_reportSettings.PDFSettings.MarginRightMillimeters ?? 10}mm",
            };*/

            var pdfOptions = new PdfOptions();

            //pdfOptions.MarginOptions = marginOptions;
            pdfOptions.PrintBackground = true;
            pdfOptions.DisplayHeaderFooter = true;
            //pdfOptions.FooterTemplate = financialStatementGeneratorRecipientResult.FooterHtmlFragment;

            // set HeaderTemplate to something so that it doesn't end up using the default
            /*pdfOptions.HeaderTemplate = "<!-- -->";

            switch ( _reportSettings.PDFSettings.PaperSize )
            {
                case FinancialStatementTemplatePDFSettingsPaperSize.A4:
                    pdfOptions.Format = PaperFormat.A4;
                    break;
                case FinancialStatementTemplatePDFSettingsPaperSize.Legal:
                    pdfOptions.Format = PaperFormat.Legal;
                    break;
                case FinancialStatementTemplatePDFSettingsPaperSize.Letter:
                default:
                    pdfOptions.Format = PaperFormat.Letter;
                    break;
            }

            */

            var pdfStream = _puppeteerPage.PdfStreamAsync( pdfOptions ).Result;

            return pdfStream;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _puppeteerPage?.Dispose();
            _puppeteerBrowser?.Dispose();
        }
    }
}
