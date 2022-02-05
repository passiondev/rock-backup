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
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class ElectronicSignature : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            // Update EntityType so that Workflow and Registration have LinkUrlLavaTemplate's
            Sql( @"UPDATE [EntityType]
SET [LinkUrlLavaTemplate] = '~/Workflow/{{ Entity.Id }}'
WHERE [Guid] = '" + Rock.SystemGuid.EntityType.WORKFLOW + "'" );


            Sql( @"UPDATE [EntityType]
SET [LinkUrlLavaTemplate] = '~/web/event-registrations/{{ Entity.RegistrationInstanceId }}/registration/{{ Entity.Id }}'
WHERE [Guid] = '" + Rock.SystemGuid.EntityType.REGISTRATION + "'" );


            // Check if the Rock.SignNow.SignNow signature provider is active.
            // If so, show a warning on the Signature Providers block. Otherwise, delete the signature providers block.
            Sql( @"
DECLARE @signNowEntityTypeId INT = (
        SELECT TOP 1 Id
        FROM EntityType
        WHERE [Name] LIKE 'Rock.SignNow.SignNow'
        )
DECLARE @providerIsActive BIT = (
        SELECT count(*)
        FROM AttributeValue
        WHERE AttributeId IN (
                SELECT Id
                FROM Attribute
                WHERE EntityTypeId = @signNowEntityTypeId
                    AND [Key] = 'Active'
                )
            AND ValueAsBoolean = 5
        )
DECLARE @usedbySignatureDocumentTemplate BIT = (
        SELECT count(*)
        FROM SignatureDocumentTemplate
        WHERE ProviderEntityTypeId = @signNowEntityTypeId
        )
DECLARE @stillUsed BIT = @providerIsActive | @usedbySignatureDocumentTemplate
DECLARE @warningHtml NVARCHAR(max) = '<div class=""alert alert-warning"">Support for signature document providers will be fully removed in the next full release.</div>';

IF (@stillUsed = 0)
BEGIN
    DELETE FROM [Block]
    WHERE [Guid] = '8690831c-d48a-48a7-bba7-5bc496e493f2'
    
    DELETE
    FROM [Page]
    WHERE [Guid] = 'FAA6A2F2-4CFD-4B97-A0C2-8F4F9CE841F3'
END
ELSE
BEGIN
    UPDATE [Block]
    SET PreHtml = @warningHtml
    WHERE [Guid] = '8690831c-d48a-48a7-bba7-5bc496e493f2'
END
" );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }
    }
}
