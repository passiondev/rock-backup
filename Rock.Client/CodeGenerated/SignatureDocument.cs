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
using System;
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Base client model for SignatureDocument that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class SignatureDocumentEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int? AppliesToPersonAliasId { get; set; }

        /// <summary />
        public int? AssignedToPersonAliasId { get; set; }

        /// <summary />
        public int? BinaryFileId { get; set; }

        /// <summary />
        public DateTime? CompletionEmailSentDateTime { get; set; }

        /// <summary />
        public string DocumentKey { get; set; }

        /// <summary />
        public int? EntityId { get; set; }

        /// <summary />
        public int? EntityTypeId { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public int InviteCount { get; set; }

        /// <summary />
        public DateTime? LastInviteDate { get; set; }

        /// <summary />
        public DateTime? LastStatusDate { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId is being set manually and should not be overwritten with current user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public string SignatureDataEncrypted { get; set; }

        /// <summary />
        public int SignatureDocumentTemplateId { get; set; }

        /// <summary />
        public string SignatureVerificationHash { get; set; }

        /// <summary />
        public string SignedByEmail { get; set; }

        /// <summary />
        public int? SignedByPersonAliasId { get; set; }

        /// <summary />
        public string SignedClientIp { get; set; }

        /// <summary />
        public string SignedClientUserAgent { get; set; }

        /// <summary />
        public DateTime? SignedDateTime { get; set; }

        /// <summary />
        public string SignedDocumentText { get; set; }

        /// <summary />
        public string SignedName { get; set; }

        /// <summary />
        public Rock.Client.Enums.SignatureDocumentStatus Status { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// This does not need to be set or changed. Rock will always set this to the current date/time when saved to the database.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source SignatureDocument object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( SignatureDocument source )
        {
            this.Id = source.Id;
            this.AppliesToPersonAliasId = source.AppliesToPersonAliasId;
            this.AssignedToPersonAliasId = source.AssignedToPersonAliasId;
            this.BinaryFileId = source.BinaryFileId;
            this.CompletionEmailSentDateTime = source.CompletionEmailSentDateTime;
            this.DocumentKey = source.DocumentKey;
            this.EntityId = source.EntityId;
            this.EntityTypeId = source.EntityTypeId;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.InviteCount = source.InviteCount;
            this.LastInviteDate = source.LastInviteDate;
            this.LastStatusDate = source.LastStatusDate;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.Name = source.Name;
            this.SignatureDataEncrypted = source.SignatureDataEncrypted;
            this.SignatureDocumentTemplateId = source.SignatureDocumentTemplateId;
            this.SignatureVerificationHash = source.SignatureVerificationHash;
            this.SignedByEmail = source.SignedByEmail;
            this.SignedByPersonAliasId = source.SignedByPersonAliasId;
            this.SignedClientIp = source.SignedClientIp;
            this.SignedClientUserAgent = source.SignedClientUserAgent;
            this.SignedDateTime = source.SignedDateTime;
            this.SignedDocumentText = source.SignedDocumentText;
            this.SignedName = source.SignedName;
            this.Status = source.Status;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for SignatureDocument that includes all the fields that are available for GETs. Use this for GETs (use SignatureDocumentEntity for POST/PUTs)
    /// </summary>
    public partial class SignatureDocument : SignatureDocumentEntity
    {
        /// <summary />
        public PersonAlias AppliesToPersonAlias { get; set; }

        /// <summary />
        public PersonAlias AssignedToPersonAlias { get; set; }

        /// <summary />
        public BinaryFile BinaryFile { get; set; }

        /// <summary />
        public EntityType EntityType { get; set; }

        /// <summary />
        public PersonAlias SignedByPersonAlias { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}
