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
using System.IO;

namespace Rock.Cms.StructuredContent.BlockTypes
{
    /// <summary>
    /// The Note block type used in the structured content system.
    /// </summary>
    /// <seealso cref="StructuredContentBlockRenderer{TData}" />
    [StructuredContentBlock( "note" )]
    public class NoteRenderer : StructuredContentBlockRenderer<NoteData>
    {
        /// <inheritdoc/>
        protected override void Render( TextWriter writer, NoteData data )
        {
            var placeholder = data.Note ?? string.Empty;

            writer.WriteLine( $"<textarea placeholder=\"{placeholder.EncodeXml( true )}\"></textarea>" );
        }
    }
}
