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
using System.ComponentModel;
using System.ComponentModel.Composition;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// </summary>
    [ActionCategory( "Utility" )]
    [Description( "Allows for e-signing a document based on a workflow template." )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "eSignature" )]

    [SignatureDocumentTemplateField(
        "Signature Document Template",
        Description = "The template to use for the signature document.",
        Key = AttributeKey.SignatureDocumentTemplate,
        ShowTemplatesThatHaveExternalProviders = false,
        IsRequired = true,
        Order = 1 )]

    [WorkflowAttribute(
        "Applies to Person",
        Description = "The attribute that represents the person that the document applies to.",
        Key = AttributeKey.AppliesToPerson,
        IsRequired = true,
        FieldTypeClassNames = new string[] { "Rock.Field.Types.PersonFieldType", "Rock.Field.Types.TextFieldType" },
        Order = 2 )]

    [WorkflowAttribute(
        "Assigned To Person",
        Description = "The attribute that represents the person that will be signing the document. This is only needed if the signature will be completed via an email",
        Key = AttributeKey.AssignedToPerson,
        IsRequired = false,
        FieldTypeClassNames = new string[] { "Rock.Field.Types.PersonFieldType", "Rock.Field.Types.TextFieldType" },
        Order = 3 )]

    [WorkflowAttribute(
        "Signed by Person",
        Description = "The attribute that represents the person that signed the document. If a person is logged in that person will override this value.",
        Key = AttributeKey.SignedByPerson,
        IsRequired = false,
        FieldTypeClassNames = new string[] { "Rock.Field.Types.PersonFieldType", "Rock.Field.Types.TextFieldType" },
        Order = 4 )]

    [WorkflowAttribute(
        "Signature Document",
        Description = "The workflow attribute to place the PDF document in.",
        Key = AttributeKey.SignatureDocument,
        IsRequired = false,
        FieldTypeClassNames = new string[] { "Rock.Field.Types.TextFieldType", "Rock.Field.Types.BinaryFileFieldType" },
        Order = 5 )]

    public class ElectronicSignature : ActionComponent
    {
        /// <summary>
        /// Keys to use for Attributes
        /// </summary>
        private static class AttributeKey
        {
            public const string SignatureDocumentTemplate = "SignatureDocumentTemplate";
            public const string AppliesToPerson = "AppliesToPerson";
            public const string AssignedToPerson = "AssignedToPerson";
            public const string SignedByPerson = "SignedByPerson";
            public const string SignatureDocument = "SignatureDocument";
        }

        /// <summary>
        /// Gets the signature document template.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="workflowAction">The workflow action.</param>
        /// <returns>SignatureDocumentTemplate.</returns>
        public SignatureDocumentTemplate GetSignatureDocumentTemplate( RockContext rockContext, WorkflowAction workflowAction )
        {
            var templateGuid = this.GetAttributeValue( workflowAction, AttributeKey.SignatureDocumentTemplate ).AsGuidOrNull();
            if ( templateGuid.HasValue )
            {
                return new SignatureDocumentTemplateService( rockContext ).Get( templateGuid.Value );
            }

            return null;
        }

        /// <summary>
        /// Saves the signature signature document values to the workflow attribute.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="workflowAction">The workflow action.</param>
        /// <param name="signatureDocument">The signature document.</param>
        public void SaveSignatureDocumentValuesToAttributes( RockContext rockContext, WorkflowAction workflowAction, SignatureDocument signatureDocument )
        {
            var signatureTemplate = new SignatureDocumentTemplateService( rockContext ).Get( signatureDocument.SignatureDocumentTemplateId );
            this.SetWorkflowAttributeValue( workflowAction, AttributeKey.SignatureDocumentTemplate, signatureTemplate?.Guid );

            var personAliasService = new PersonAliasService( rockContext );

            Guid? appliesToPersonAliasGuid = null;
            Guid? assignedToPersonAliasGuid = null;
            Guid? signedByPersonAliasGuid = null;
            if ( signatureDocument.AppliesToPersonAliasId.HasValue )
            {
                appliesToPersonAliasGuid = personAliasService.GetGuid( signatureDocument.AppliesToPersonAliasId.Value );
            }

            if ( signatureDocument.AssignedToPersonAliasId.HasValue )
            {
                assignedToPersonAliasGuid = personAliasService.GetGuid( signatureDocument.AssignedToPersonAliasId.Value );
            }

            if ( signatureDocument.SignedByPersonAliasId.HasValue )
            {
                signedByPersonAliasGuid = personAliasService.GetGuid( signatureDocument.SignedByPersonAliasId.Value );
            }

            if ( signatureDocument.BinaryFileId.HasValue )
            {
                var binaryFileGuid = new BinaryFileService( rockContext ).GetSelect( signatureDocument.BinaryFileId.Value, s => s.Guid );
                this.SetWorkflowAttributeValue( workflowAction, AttributeKey.SignatureDocument, signatureDocument.Guid );
            }

            this.SetWorkflowAttributeValue( workflowAction, AttributeKey.AppliesToPerson, appliesToPersonAliasGuid );
            this.SetWorkflowAttributeValue( workflowAction, AttributeKey.AssignedToPerson, assignedToPersonAliasGuid );
            this.SetWorkflowAttributeValue( workflowAction, AttributeKey.SignedByPerson, signedByPersonAliasGuid );

        }

        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The workflow action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public override bool Execute( RockContext rockContext, WorkflowAction action, object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            var actionType = action.ActionTypeCache;

            // Always return false. Special logic for e-Signature will be handled in the WorkflowEntry block
            return false;
        }
    }
}