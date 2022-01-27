// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.IO;

using PuppeteerSharp;
using PuppeteerSharp.Media;

using Rock.Utility;
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

        private Browser _puppeteerBrowser = null;
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

            try
            {
                AsyncHelper.RunSync( () => browserFetcher.DownloadAsync() );
            }
            catch ( IOException ioException )
            {
                throw new PdfGeneratorException( "PDF Engine is not available. Please try again later.", ioException );
            }
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
        /// Gets or sets CSS @media. Defaults to <see cref="MediaType.Print"/>
        /// </summary>
        /// <value>The type of the PDF media.</value>
        public MediaType PDFMediaType { get; set; } = MediaType.Screen;

        /// <summary>
        /// Gets or sets the paper format (Letter, Legal, A4)
        /// </summary>
        /// <value>The paper format.</value>
        public PaperFormat PaperFormat { get; set; } = PaperFormat.Letter;

        /// <summary>
        /// Gets or sets the margin options.
        /// </summary>
        /// <value>The margin options.</value>
        public MarginOptions MarginOptions { get; set; }

        /// <summary>
        /// Print background graphics. Defaults to true.
        /// </summary>
        /// <value><c>true</c> if [print background]; otherwise, <c>false</c>.</value>
        public bool PrintBackground { get; set; } = true;

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
            _puppeteerPage.EmulateMediaTypeAsync( this.PDFMediaType ).Wait();
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
        public Stream GetPDFDocumentFromURL( string url )
        {
            return GetPDFDocument( null, url );
        }

        /// <summary>
        /// Renders the PDF document from HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Stream.</returns>
        public Stream GetPDFDocumentFromHtml( string html )
        {
            return GetPDFDocument( html, null );
        }

        /// <summary>
        /// Gets the PDF document from a URL as base64 Data URL
        /// For example:
        /// <br/>
        /// <c>data:application/pdf;base64,JVBERi0xLjMK...</c>
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        public string GetPDFDocumentFromUrlAsBase64DataUrl( string url )
        {
            return GetPDFDocumentAsBase64DataUrl( null, url );
        }

        /// <summary>
        /// Gets the PDF document from HTML as base64 Data URL. For example:
        /// <br/>
        /// <c>data:application/pdf;base64,JVBERi0xLjMK...</c>
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>System.String.</returns>
        public string GetPDFDocumentFromHtmlAsBase64DataUrl( string html )
        {
            return GetPDFDocumentAsBase64DataUrl( html, null );
        }

        /// <summary>
        /// Gets the PDF document in the form of
        /// <br/>
        /// <c>data:application/pdf;base64,JVBERi0xLjMK...</c>
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        private string GetPDFDocumentAsBase64DataUrl( string html, string url )
        {
            using ( var pdfStream = GetPDFDocument( html, url ) )
            {
                byte[] bytes;
                using ( var memoryStream = new MemoryStream() )
                {
                    pdfStream.CopyTo( memoryStream );
                    bytes = memoryStream.ToArray();
                }

                string base64 = Convert.ToBase64String( bytes );

                var base64Url = $"data:application/pdf;base64,{base64}";
                return base64Url;
            }
        }

        /// <summary>
        /// Renders the PDF document.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="url">The URL.</param>
        /// <returns>Stream.</returns>
        private Stream GetPDFDocument( string html, string url )
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

            var pdfOptions = new PdfOptions();

            if ( this.MarginOptions != null )
            {
                pdfOptions.MarginOptions = this.MarginOptions;
            }

            pdfOptions.PrintBackground = this.PrintBackground;

            pdfOptions.DisplayHeaderFooter = true;

            // set HeaderTemplate to something so that it doesn't end up using the default, which is Page Title and Date
            pdfOptions.HeaderTemplate = "<!-- -->";

            // Set footer template to show pageNumber/totalPages on the bottom right.
            // See chromium source code at  https://source.chromium.org/chromium/chromium/src/+/main:components/printing/resources/print_header_footer_template_page.html
            pdfOptions.FooterTemplate = @"
<div class='text left grow'></div>
<div class='text right'>
    <span class='pageNumber'></span>/<span class='totalPages'></span>
</div>;";

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

    public class PdfGeneratorException : Exception
    {
        public PdfGeneratorException( string message ) : base( message )
        {

        }

        public PdfGeneratorException( string message, Exception innerException ) : base( message, innerException )
        {

        }
    }
}
