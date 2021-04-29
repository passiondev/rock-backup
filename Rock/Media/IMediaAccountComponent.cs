﻿// <copyright>
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
using System.Threading;
using System.Threading.Tasks;

using Rock.Model;

namespace Rock.Media
{
    /// <summary>
    /// Component that provides functionality for syncing to a media storage
    /// provider.
    /// </summary>
    public interface IMediaAccountComponent
    {
        /// <summary>
        /// Gets a value if this account allows the individual to add/edit/delete folders and media files.
        /// </summary>
        /// <value>
        ///   <c>true</c> if manual entry of folders and media is allowed; otherwise, <c>false</c>.
        /// </value>
        bool AllowsManualEntry { get; }

        /// <summary>
        /// Gets the attribute value for the media account.
        /// </summary>
        /// <param name="mediaAccount">The media account.</param>
        /// <param name="key">The key.</param>
        /// <returns>The attribute value.</returns>
        string GetAttributeValue( MediaAccount mediaAccount, string key );

        /// <summary>
        /// Gets the additional HTML to display on media account detail page.
        /// </summary>
        /// <param name="mediaAccount">The media account.</param>
        /// <returns>A string of HTML content.</returns>
        /// <remarks>This is normally any metrics data that should be displayed.</remarks>
        string GetAccountHtmlSummary( MediaAccount mediaAccount );

        /// <summary>
        /// Gets the additional HTML to display on media folder detail page.
        /// </summary>
        /// <param name="mediaFolder">The media folder.</param>
        /// <returns>A string of HTML content.</returns>
        /// <remarks>This is normally any metrics data that should be displayed.</remarks>
        string GetFolderHtmlSummary( MediaFolder mediaFolder );

        /// <summary>
        /// Gets the additional HTML to display on media element detail page.
        /// </summary>
        /// <param name="mediaElement">The media element.</param>
        /// <returns>A string of HTML content.</returns>
        /// <remarks>This is normally any metrics data that should be displayed.</remarks>
        string GetMediaElementHtmlSummary( MediaElement mediaElement );

        /// <summary>
        /// Performs a full synchronization of folders and media content for the account. 
        /// </summary>
        /// <param name="mediaAccount">The media account to be synchronized.</param>
        /// <param name="cancellationToken">Indicator that the operation should be stopped.</param>
        /// <returns>A <see cref="SyncOperationResult"/> object with the result of the operation.</returns>
        /// <remarks>
        /// The <see cref="MediaAccount.LastRefreshDateTime"/> is updated when
        /// <see cref="SyncOperationResult.IsSuccess"/> is <c>true</c>.
        /// </remarks>
        Task<SyncOperationResult> SyncMediaAsync( MediaAccount mediaAccount, CancellationToken cancellationToken );

        /// <summary>
        /// Performs a synchronization of all analytics data for the media account.
        /// </summary>
        /// <param name="mediaAccount">The media account to be synchronized.</param>
        /// <param name="cancellationToken">Indicator that the operation should be stopped.</param>
        /// <returns>A <see cref="SyncOperationResult"/> object with the result of the operation.</returns>
        Task<SyncOperationResult> SyncAnalyticsAsync( MediaAccount mediaAccount, CancellationToken cancellationToken );

        /// <summary>
        /// Performs a partial synchronization of folders and media content for
        /// the account. This should be a very fast operation. As such it is
        /// normal to only pull in newly created folders or media elements.
        /// </summary>
        /// <param name="mediaAccount">The media account to be refreshed.</param>
        /// <param name="cancellationToken">Indicator that the operation should be stopped.</param>
        /// <returns>A <see cref="SyncOperationResult"/> object with the result of the operation.</returns>
        /// <remarks>
        /// The <see cref="MediaAccount.LastRefreshDateTime"/> is updated when
        /// <see cref="SyncOperationResult.IsSuccess"/> is <c>true</c>.
        /// </remarks>
        Task<SyncOperationResult> RefreshAccountAsync( MediaAccount mediaAccount, CancellationToken cancellationToken );
    }
}
