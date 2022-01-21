using System;
using System.IO;

using PuppeteerSharp;
using PuppeteerSharp.Media;

using Rock.Web.Cache;

namespace Rock.Pdf
{
    /// <summary>
    /// Class PDFGenerator.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class PdfGenerator : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfGenerator"/> class.
        /// </summary>
        public PdfGenerator()
        {
            InitializeChromeEngine();
        }

        // #TODO#. Seems to be quite a bit faster as a singleton, so this could be static, but Page would be trickier 
        private Browser _puppeteerBrowser = null;

        // #TODO#. If we do a singleton, we would have to be careful with Page since it isn't thread-safe. StatementGenerater has some tricks to keep page instances around in a thread-safe way. 
        private Page _puppeteerPage;

        private static int _lastProgressPercentage = 0;

        /// <summary>
        /// Ensures the chrome engine is downloaded and installed.
        /// Note this could take several minutes if the chrome engine hasn't been downloaded yet.
        /// </summary>
        public static void EnsureChromeEngineInstalled()
        {
            using ( var browserFetcher = GetBrowserFetcher() )
            {
                EnsureChromeEngineInstalled( browserFetcher );
            }
        }

        /// <inheritdoc cref="PdfGenerator.EnsureChromeEngineInstalled()"/>
        private static void EnsureChromeEngineInstalled( BrowserFetcher browserFetcher )
        {
            _lastProgressPercentage = 0;
            browserFetcher.DownloadProgressChanged += BrowserFetcher_DownloadProgressChanged;
            browserFetcher.DownloadAsync().Wait();
        }

        /// <summary>
        /// Gets the browser fetcher.
        /// </summary>
        /// <returns>BrowserFetcher.</returns>
        private static BrowserFetcher GetBrowserFetcher()
        {
            var browserDownloadPath = System.Web.Hosting.HostingEnvironment.MapPath( "~/App_Data/ChromeEngine" );
            Directory.CreateDirectory( browserDownloadPath );

            var browserFetcherOptions = new BrowserFetcherOptions
            {
                Product = Product.Chrome,
                Path = browserDownloadPath,
            };

            return new BrowserFetcher( browserFetcherOptions );
        }

        /// <summary>
        /// Initializes the chrome engine.
        /// </summary>
        private void InitializeChromeEngine()
        {
            if ( _puppeteerBrowser == null )
            {
                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    DefaultViewport = new ViewPortOptions { Width = 1280, Height = 1024, DeviceScaleFactor = 1 },

                };

                using ( var browserFetcher = GetBrowserFetcher() )
                {
                    // should have already been installed, but just in case it hasn't, download it now.
                    EnsureChromeEngineInstalled( browserFetcher );
                    launchOptions.ExecutablePath = browserFetcher.RevisionInfo( BrowserFetcher.DefaultChromiumRevision ).ExecutablePath;
                }

                _puppeteerBrowser = Puppeteer.LaunchAsync( launchOptions ).Result;
            }

            _puppeteerPage = _puppeteerBrowser.NewPageAsync().Result;
            _puppeteerPage.EmulateMediaTypeAsync( PuppeteerSharp.Media.MediaType.Screen ).Wait();
        }

        /// <summary>
        /// Handles the DownloadProgressChanged event of the BrowserFetcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Net.DownloadProgressChangedEventArgs"/> instance containing the event data.</param>
        private static void BrowserFetcher_DownloadProgressChanged( object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            if ( e.ProgressPercentage != _lastProgressPercentage )
            {
                _lastProgressPercentage = e.ProgressPercentage;
                System.Diagnostics.Debug.WriteLine( $"Downloading PdfGenerator ChromeEngine:  {e.ProgressPercentage}%" );
            }
        }


        /// <summary>
        /// Renders the PDF document from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Stream.</returns>
        public Stream RenderPDFDocumentFromURL( string url )
        {
            return RenderPDFDocument( null, url );
        }

        /// <summary>
        /// Renders the PDF document from HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Stream.</returns>
        public Stream RenderPDFDocumentFromHtml( string html )
        {
            return RenderPDFDocument( html, null );
        }

        /// <summary>
        /// Renders the PDF document.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="url">The URL.</param>
        /// <returns>Stream.</returns>
        private Stream RenderPDFDocument( string html, string url )
        {
            if ( html.IsNotNullOrWhiteSpace() )
            {
                var pdfHtml = html;

                // update all relative urls to absolute url
                string publicAppRoot = GlobalAttributesCache.Get().GetValue( "PublicApplicationRoot" );
                if ( publicAppRoot.IsNotNullOrWhiteSpace() )
                {
                    pdfHtml = pdfHtml.Replace( "~/", publicAppRoot );
                    pdfHtml = pdfHtml.Replace( @" src=""/", @" src=""" + publicAppRoot );
                    pdfHtml = pdfHtml.Replace( @" src='/", @" src='" + publicAppRoot );
                    pdfHtml = pdfHtml.Replace( @" href=""/", @" href=""" + publicAppRoot );
                    pdfHtml = pdfHtml.Replace( @" href='/", @" href='" + publicAppRoot );
                }

                _puppeteerPage.SetContentAsync( pdfHtml ).Wait();
            }
            else if ( url.IsNotNullOrWhiteSpace() )
            {
                _puppeteerPage.GoToAsync( url ).Wait();
            }
            else
            {
                return null;
            }

            MarginOptions marginOptions = new MarginOptions
            {
                Bottom = $"15mm",
                Left = $"10mm",
                Top = $"10mm",
                Right = $"10mm",
            };

            var pdfOptions = new PdfOptions();

            // set HeaderTemplate to something so that it doesn't end up using the default, which is Page Title and Date
            pdfOptions.HeaderTemplate = "<!-- -->";

            // let Footer Template be the default, which is Page/PageCount
            pdfOptions.FooterTemplate = null;

            pdfOptions.MarginOptions = marginOptions;
            pdfOptions.PrintBackground = false;
            pdfOptions.DisplayHeaderFooter = true;
            //pdfOptions.FooterTemplate = financialStatementGeneratorRecipientResult.FooterHtmlFragment;
            
            /*
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
            _puppeteerPage.Dispose();
            _puppeteerBrowser.Dispose();
        }
    }
}
