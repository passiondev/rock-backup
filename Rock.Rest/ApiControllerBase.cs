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
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web.Http;
using System.Web.Http.OData;

#if NET5_0_OR_GREATER
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
#endif

using Rock.Data;
using Rock.Model;

namespace Rock.Rest
{
    /*
     * NOTE: We could have inherited from System.Web.Http.OData.ODataController, but that changes 
     * the response format from vanilla REST to OData format. That breaks existing Rock Rest clients.
     * 
     */

    /// <summary>
    /// Base ApiController for Rock REST endpoints
    /// Supports ODataV3 Queries and ODataRouting
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [ODataRouting]
#if NET5_0_OR_GREATER
    public class ApiControllerBase : ControllerBase
#else
    public class ApiControllerBase : ApiController
#endif
    {
        /// <summary>
        /// Gets the currently logged in Person
        /// </summary>
        /// <returns></returns>
        protected virtual Rock.Model.Person GetPerson()
        {
            return GetPerson( null );
        }

        /// <summary>
        /// Gets the currently logged in Person
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        protected virtual Rock.Model.Person GetPerson( RockContext rockContext )
        {
#if NET5_0_OR_GREATER
            if ( HttpContext.Items.ContainsKey( "Person" ) )
            {
                return HttpContext.Items["Person"] as Person;
            }

            var principal = User;
#else
            if ( Request.Properties.Keys.Contains( "Person" ) )
            {
                return Request.Properties["Person"] as Person;
            }

            var principal = ControllerContext.Request.GetUserPrincipal();
#endif
            if ( principal != null && principal.Identity != null )
            {
                if ( principal.Identity.Name.StartsWith( "rckipid=" ) )
                {
                    var personService = new Model.PersonService( rockContext ?? new RockContext() );
                    Rock.Model.Person impersonatedPerson = personService.GetByImpersonationToken( principal.Identity.Name.Substring( 8 ), false, null );
                    if ( impersonatedPerson != null )
                    {
                        return impersonatedPerson;
                    }
                }
                else
                {
                    var userLoginService = new Rock.Model.UserLoginService( rockContext ?? new RockContext() );
                    var userLogin = userLoginService.GetByUserName( principal.Identity.Name );

                    if ( userLogin != null )
                    {
                        var person = userLogin.Person;
#if NET5_0_OR_GREATER
                        HttpContext.Items.Add( "Person", person );
#else
                        Request.Properties.Add( "Person", person );
#endif
                        return userLogin.Person;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Ensures that the HttpContext has a CurrentPerson value.
        /// </summary>
        protected virtual void EnsureHttpContextHasCurrentPerson()
        {
#if NET5_0_OR_GREATER
            if ( !HttpContext.Items.ContainsKey( "CurrentPerson" ) )
            {
                HttpContext.Items.Add( "CurrentPerson", GetPerson() );
            }
#else
            System.Web.HttpContext.Current.AddOrReplaceItem( "CurrentPerson", GetPerson() );
#endif
        }

        /// <summary>
        /// Gets the primary person alias of the currently logged in person
        /// </summary>
        /// <returns></returns>
        protected virtual Rock.Model.PersonAlias GetPersonAlias()
        {
            return GetPersonAlias( null );
        }

        /// <summary>
        /// Gets the primary person alias of the currently logged in person
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        protected virtual Rock.Model.PersonAlias GetPersonAlias( RockContext rockContext )
        {
            var person = GetPerson( rockContext );
            if ( person != null )
            {
                return person.PrimaryAlias;
            }

            return null;
        }

        /// <summary>
        /// Gets the primary person alias ID of the currently logged in person
        /// </summary>
        /// <returns></returns>
        protected virtual int? GetPersonAliasId()
        {
            return GetPersonAliasId( null );
        }

        /// <summary>
        /// Gets the primary person alias ID of the currently logged in person
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        protected virtual int? GetPersonAliasId( RockContext rockContext )
        {
            var currentPersonAlias = GetPersonAlias( rockContext );
            return currentPersonAlias == null ? ( int? ) null : currentPersonAlias.Id;
        }

#if !NET5_0_OR_GREATER
        /// <summary>
        /// Creates a response with the NoContent status code.
        /// </summary>
        /// <returns>The response.</returns>
        protected IHttpActionResult NoContent()
        {
            return StatusCode( System.Net.HttpStatusCode.NoContent );
        }

        /// <summary>
        /// Creates a response with the Accepted status code.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>The response</returns>
        protected IHttpActionResult Accepted<T>( T content )
        {
            return Content( HttpStatusCode.Accepted, content );
        }

        internal IHttpActionResult BadRequest( RockApiError error )
        {
            return BadRequest( error.Message );
        }

        internal IHttpActionResult NotFound( RockApiError error )
        {
            return ResponseMessage( ControllerContext.Request.CreateErrorResponse( HttpStatusCode.NotFound, error.Message ) );
        }
#endif
    }
}
