//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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

using Rock.Model;
using Rock.SystemGuid;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// FinancialTransactionImages REST API
    /// </summary>
    [RestControllerGuid( "46233633-4A40-43EF-86BF-7A8AFA7C68ED" )]
    public partial class FinancialTransactionImagesController : Rock.Rest.ApiController<Rock.Model.FinancialTransactionImage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialTransactionImagesController"/> class.
        /// </summary>
        public FinancialTransactionImagesController() : base( new Rock.Model.FinancialTransactionImageService( new Rock.Data.RockContext() ) ) { } 
    }
}
