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
using Rock.Model;

namespace Rock.Migrations
{
    /// <summary>
    ///
    /// </summary>
    public partial class Rollup_20220118 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            UpdateStatementGeneratorDownloadLinkUp();
            UpdatePersonalDevices();
            AddMissingRoutes();
            FullWorkSurface();
            SetConnectionWebViewBlockTypes();
            UpdateBackgroundCheckWorkflowInjectCheckEmailActions();
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            UpdateStatementGeneratorDownloadLinkDown();
        }


        /// <summary>
        /// DV: Statement Generator Download Location - Updates the statement generator download link up.
        /// </summary>
        private void UpdateStatementGeneratorDownloadLinkUp()
        {
            Sql( @"
				DECLARE @statementGeneratorDefinedValueId INT = (SELECT Id FROM [DefinedValue] WHERE [Guid] = '54E1EBCC-5A5A-4B26-9CCB-36E7CEB49C3C')
				DECLARE @downloadUrlAttributeId INT = (SELECT [Id] FROM [Attribute] WHERE [Guid] = 'E0AF9B30-15EA-413B-BAC4-25B286D91FD9')

				UPDATE [AttributeValue]
				SET [Value] = 'https://storage.rockrms.com/externalapplications/sparkdevnetwork/statementgenerator/1.13.1/statementgenerator.msi'
				WHERE AttributeId = @downloadUrlAttributeId and EntityId = @statementGeneratorDefinedValueId" );
        }

        /// <summary>
        /// DV: Statement Generator Download Location - Updates the statement generator download link down.
        /// </summary>
        private void UpdateStatementGeneratorDownloadLinkDown()
        {
            Sql( @"
				DECLARE @statementGeneratorDefinedValueId INT = (SELECT Id FROM [DefinedValue] WHERE [Guid] = '54E1EBCC-5A5A-4B26-9CCB-36E7CEB49C3C')
				DECLARE @downloadUrlAttributeId INT = (SELECT [Id] FROM [Attribute] WHERE [Guid] = 'E0AF9B30-15EA-413B-BAC4-25B286D91FD9')

				UPDATE [AttributeValue]
				SET [Value] = 'https://storage.rockrms.com/externalapplications/sparkdevnetwork/statementgenerator/1.12.8/statementgenerator.msi'
				WHERE AttributeId = @downloadUrlAttributeId and EntityId = @statementGeneratorDefinedValueId" );
        }

        /// <summary>
        /// GJ: Update Personal Devices
        /// </summary>
        private void UpdatePersonalDevices()
        {
            // Add Block Attribute Value
            //   Block: Personal Devices
            //   BlockType: Personal Devices
            //   Category: CRM
            //   Block Location: Page=Personal Devices, Site=Rock RMS
            //   Attribute: Lava Template
            /*   Attribute Value: ... */
            RockMigrationHelper.AddBlockAttributeValue( "B5A94C63-869C-4B4C-B129-9E098EF5537C", "24CAD424-3DAD-407C-9EA5-90FAD6293F81", @"<div class=""panel panel-block"">
	<div class=""panel-heading"">
		<h4 class=""panel-title"">
			<i class=""fa fa-mobile""></i>
			{{ Person.FullName }}
		</h4>
	</div>
	<div class=""panel-body"">
		<div class=""row d-flex flex-wrap"">
			{% for item in PersonalDevices %}
				<div class=""col-xs-6 col-sm-4 col-md-3 mb-4"">
					<div class=""well h-100 mb-0 rollover-container"">
						<a class=""pull-right rollover-item btn btn-xs btn-danger"" href=""#"" onclick=""Rock.dialogs.confirm('Are you sure you want to delete this Device?', function (result) { if (result ){{ item.PersonalDevice.Id | Postback:'DeleteDevice' }}}) ""><i class=""fa fa-times""></i></a>
						<div style=""min-height: 120px;"">
							<h3 class=""margin-v-none"">
								{% if item.DeviceIconCssClass != '' %}
									<i class=""fa {{ item.DeviceIconCssClass }}""></i>
								{% endif %}
								{% if item.PersonalDevice.NotificationsEnabled == true %}
									<i class=""fa fa-comment-o""></i>
								{% endif %}
							</h3>
							<dl>
								{% if item.PlatformValue != '' %}<dt>{{ item.PlatformValue }} {{ item.PersonalDevice.DeviceVersion }}</dt>{% endif %}
								{% if item.PersonalDevice.CreatedDateTime != null %}<dt>Discovered</dt><dd>{{ item.PersonalDevice.CreatedDateTime }}</dd>{% endif %}
								{% if item.PersonalDevice.MACAddress != '' and item.PersonalDevice.MACAddress != null %}<dt>MAC Address</dt><dd>{{ item.PersonalDevice.MACAddress }}</dd>{% endif %}
							</dl>
						</div>
						{% if LinkUrl != '' %}
							<a href=""{{ LinkUrl | Replace:'[Id]',item.PersonalDevice.Id }}"" class=""btn btn-default btn-xs""> Interactions</a>
						{% endif %}
					</div>
				</div>
			{% endfor %}
		</div>
	</div>
</div>" );
        }

        /// <summary>
        /// GJ: Add Missing Routes
        /// </summary>
        private void AddMissingRoutes()
        {
            Sql( @"
				DECLARE @PageId int

				-- Page: Benevolence Type Detail
				SET @PageId = (SELECT [Id] FROM [Page] WHERE [Guid] = 'DBFC432E-F0A4-457E-BA5B-572C49B899D1')
				IF @PageId IS NOT NULL AND NOT EXISTS(SELECT [Id] FROM [PageRoute] WHERE [PageId] = @PageId AND [Route] = 'finance/benevolence/types/{BenevolenceTypeId}')
									INSERT INTO [PageRoute] (
										[IsSystem],[PageId],[Route],[Guid])
									VALUES(
										1, @PageId, 'finance/benevolence/types/{BenevolenceTypeId}', '95DEE816-F3BF-4FA0-941B-937F8A2734CB' )

				-- Page: Security Settings
				SET @PageId = (SELECT [Id] FROM [Page] WHERE [Guid] = '0EF3DE1C-CB97-431E-A066-DDF8BD2D8283')
				IF @PageId IS NOT NULL AND NOT EXISTS(SELECT [Id] FROM [PageRoute] WHERE [PageId] = @PageId AND [Route] = 'admin/security/security-settings')
									INSERT INTO [PageRoute] (
										[IsSystem],[PageId],[Route],[Guid])
									VALUES(
										1, @PageId, 'admin/security/security-settings', '3A3C2C78-C758-4B02-9C95-C8D201F83E6C' )

				-- Page: Security Change Audit
				SET @PageId = (SELECT [Id] FROM [Page] WHERE [Guid] = '340C6E2C-7006-4490-9FD4-14D58784519B')
				IF @PageId IS NOT NULL AND NOT EXISTS(SELECT [Id] FROM [PageRoute] WHERE [PageId] = @PageId AND [Route] = 'admin/security/change-audit')
									INSERT INTO [PageRoute] (
										[IsSystem],[PageId],[Route],[Guid])
									VALUES(
										1, @PageId, 'admin/security/change-audit', '25A24576-F2BA-41DB-AB62-387748B31EF0' )" );
        }

        /// <summary>
        /// GJ: Full Worksurface Migrations
        /// </summary>
        private void FullWorkSurface()
        {
            Sql( @"
				-- Page: Connections Board to Full Worksurface
				UPDATE [Page]
				SET LayoutId = (
						SELECT TOP 1 Id
						FROM Layout
						WHERE Guid = 'C2467799-BB45-4251-8EE6-F0BF27201535'
						)
				WHERE [Guid] = '4FBCEB52-8892-4035-BDEA-112A494BE81F'

				-- Page: SMS Conversations to Full Worksurface
				UPDATE [Page]
				SET LayoutId = (
						SELECT TOP 1 Id
						FROM Layout
						WHERE Guid = 'C2467799-BB45-4251-8EE6-F0BF27201535'
						)
				WHERE [Guid] = '275A5175-60E0-40A2-8C63-4E9D9CD39036'

				-- Page: Group Scheduler to Full Worksurface
				UPDATE [Page]
				SET LayoutId = (
						SELECT TOP 1 Id
						FROM Layout
						WHERE Guid = 'C2467799-BB45-4251-8EE6-F0BF27201535'
						)
				WHERE [Guid] = '1815D8C6-7C4A-4C05-A810-CF23BA937477'" );
        }

        /// <summary>
        /// CH: Add Connection WebView BlockTypes
        /// </summary>
        private void SetConnectionWebViewBlockTypes()
        {

            // Add/Update WebView : Connection Opportunity List
            RockMigrationHelper.UpdateBlockType( "Connection Opportunity List", "Displays the connection type opportunities in a lava formatted block.", "~/Blocks/Connection/WebConnectionOpportunityListLava.ascx", "Connection > WebView", "B2E0E4E3-30B1-45BD-B808-C55BCD540894" );

            // Add/Update WebView : Connection Request List
            RockMigrationHelper.UpdateBlockType( "Connection Request List", "Displays the connection request in a lava formatted block.", "~/Blocks/Connection/WebConnectionRequestListLava.ascx", "Connection > WebView", "E6BAA42C-D799-4189-ABC9-4A8CA1B91D5A" );

            // Add/Update WebView : Connection Type List
            RockMigrationHelper.UpdateBlockType( "Connection Type List", "Displays the connection types in a lava formatted block.", "~/Blocks/Connection/WebConnectionTypeListLava.ascx", "Connection > WebView", "887F66AF-944F-4959-87F0-087E3999BAC3" );
        }

        private void UpdateBackgroundCheckWorkflowInjectCheckEmailActions()
        {
            try
            {
                const string myMinistry_ActivityTypeGuid = "2950B120-7BB5-46B5-93D0-26D3936F1894";
                const string checkr_ActivityTypeGuid = "9BC07356-3B2F-4BFF-9320-FA8F3A28FC39";
                // Background check [Initial Request] activity types: First entry [0] is My Ministry Guid Type, the second [1] is Checkr
                var backgroundCheck_WorkflowActivityTypeGuids = new[] { myMinistry_ActivityTypeGuid };

                foreach ( var activityTypeGuid in backgroundCheck_WorkflowActivityTypeGuids )
                {
                    var bgCheck_ActivityTypeIdSql = $"SELECT TOP 1 [Id] FROM WorkflowActivityType WHERE [Guid] = '{activityTypeGuid}'";
                    var bgCheck_ActivityTypeId = SqlScalar( bgCheck_ActivityTypeIdSql ).ToIntSafe();

                    if ( bgCheck_ActivityTypeId > 0 )
                    {
                        // Attribute Guids
                        const string personEmailValid_AttributeGuid = "0C611ABB-C3AF-495E-8454-C12FBAFB8847";

                        // Action EntityType Attributes
                        const string workflowActionShowHtml_EntityTypeGuid = "FDDAE78D-B7B3-4DA2-9A92-CC129AAF15DE";
                        const string workflowActionRunLava_EntityTypeGuid = "BC21E57A-1477-44B3-A7C2-61A806118945";
                        const string workflowActionCompleteWorkflow_EntityTypeGuid = "EEDA4318-F014-4A46-9C76-4C052EF81AA1";

                        // WorkflowEventItemOccurrence EntityType 
                        const string workflowEventItemOccurrence_EntityTypeGuid = "71632E1A-1E7F-42B9-A630-EC99F375303A";
                        var workflowEventItemOccurrence_EntityTypeIdSql = $"SELECT TOP 1 [Id] FROM EntityType WHERE [Guid] = '{workflowEventItemOccurrence_EntityTypeGuid}'";
                        var workflowEventItemOccurrence_EntityTypeId = SqlScalar( workflowEventItemOccurrence_EntityTypeIdSql ).ToIntSafe();

                        // WorkflowActionType EntityType 
                        const string workflowActionType_EntityTypeGuid = "23E3273A-B137-48A3-9AFF-C8DC832DDCA6";
                        var workflowActionType_EntityTypeIdSql = $"SELECT TOP 1 [Id] FROM EntityType WHERE [Guid] = '{workflowActionType_EntityTypeGuid}'";
                        var workflowActionType_EntityTypeId = SqlScalar( workflowActionType_EntityTypeIdSql ).ToIntSafe();

                        // Add Activity PersonEmailValid Entity Attribute
                        RockMigrationHelper.AddOrUpdateEntityAttribute( "Rock.Model.WorkflowActivity",
                            "9C204CD0-1233-41C5-818A-C5DA439445AA", "ActivityTypeId",
                            bgCheck_ActivityTypeId.ToString(),
                            "PersonEmailValid",
                            "PersonEmailValid",
                            "Used to indicate if a person has a valid and active email before executing a background check.", 0, "No",
                            personEmailValid_AttributeGuid, "PersonEmailValid" );


                        // Action Entity Attribute Value
                        const string lavaAttribute_AttributeGuid = "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4";
                        const string htmlAttribute_AttributeGuid = "DCC5F049-8BDB-4DBF-B07A-081C0B772473";

                        #region Add [Set PersonEmailValid Attribute] Value
                        // Add [Set PersonEmailValid Attribute] Value
                        const string setPersonEmailValidLava = @"{% assign person = Workflow | Attribute:''Person'',''Object'' %}
{% assign emailLength = person.Email | Size %}
{% assign emailActive = person.IsEmailActive | AsBoolean %}
{% if emailLength > 0 and emailActive %}
Yes
{% else %}
No
{% endif %}";
                        #endregion Add [Set PersonEmailValid Attribute] Value

                        RockMigrationHelper.AddAttributeValue( lavaAttribute_AttributeGuid, workflowEventItemOccurrence_EntityTypeId,
                            setPersonEmailValidLava, "6DC21662-3F1C-4118-9E2A-3D20AC9DE865" );

                        #region [Show Email Not Found Message] Value
                        // [Show Email Not Found Message] Value
                        const string setPersonEmailNotFoundMessage = @"{% assign person = Workflow | Attribute:''Person'',''Object'' %}
{% assign personProfilePage = ''~/Person/'' %}
{% assign personUrl = personProfilePage | ResolveRockUrl person.Id %}

<div class=''alert alert-danger margin-t-lg js-workflow-entry-message-notification-box''>
    <strong> Error: Email Required</strong>
    <span class=''js-notification-text''>
	    <ul>
	        <li><strong><a href=''{{ personProfilePage | ResolveRockUrl }}{{ person.Id }}'' target=''_blank''>{{ Workflow | Attribute:''Person'',''FullName'' }}</a></strong> has an empty or inactive email address. A background check can not be run unless a valid email address is provided.</li>
				    <ul>
                        <li>Enter a valid email address and\or set it to active for <strong></strong>{{ Workflow | Attribute:''Person'',''FullName'' }}</strong>.</li>
                        <li>Then run the background check again.</li>
                    </ul>
	    </ul>
	</span>
</div>";
                        #endregion [Show Email Not Found Message] Value

                        RockMigrationHelper.AddAttributeValue( htmlAttribute_AttributeGuid, workflowActionType_EntityTypeId,
                            setPersonEmailNotFoundMessage, "1F9C3491-C165-4307-881D-06B8716146C9" );

                        // Reorder Action entries
                        if ( activityTypeGuid == myMinistry_ActivityTypeGuid )
                        {
                            // Set Status
                            Sql( "Update [WorkflowActionType] SET [Order] = 4 WHERE [Guid] = '10E8C6CB-A72C-4254-A1F5-E43B6C7B404B'" );

                            // Set Requester
                            Sql( "Update [WorkflowActionType] SET [Order] = 5 WHERE [Guid] = '3136A135-4836-4C09-BD81-326CA21C6AA5'" );

                            // Set Name
                            Sql( "Update [WorkflowActionType] SET [Order] = 6 WHERE [Guid] = '6A779AB3-3223-411B-9AEE-87A5EE1EDF12'" );

                            // Set Warning
                            Sql( "Update [WorkflowActionType] SET [Order] = 7 WHERE [Guid] = '81D1FB3E-5017-4A53-A4EF-F6618F782935'" );

                            // Get Details
                            Sql( "Update [WorkflowActionType] SET [Order] = 8 WHERE [Guid] = 'A3EAF2A3-97FB-47A6-9844-F7F0755FC5BE'" );

                        }

                        // Add [Set PersonEmailValid Attribute] Action - Order 1
                        RockMigrationHelper.UpdateWorkflowActionType( activityTypeGuid, "Set PersonEmailValid Attribute", 1,
                            workflowActionRunLava_EntityTypeGuid, true, false, "",
                            "", ComparisonType.EqualTo.ConvertToInt(), "",
                            "47352363-74CC-477C-B7A1-56939B18B1E2" );

                        // Add [Show Email Not Found Message] Action - Order 2
                        RockMigrationHelper.UpdateWorkflowActionType( activityTypeGuid, "Show Email Not Found Message", 2,
                            workflowActionShowHtml_EntityTypeGuid, true, false, "",
                            personEmailValid_AttributeGuid, ComparisonType.EqualTo.ConvertToInt(), "No",
                            "F7706A78-A603-435E-8905-4FCF50AA2765" );

                        // Add [Set Email Not Found Status] Action - Order 3
                        RockMigrationHelper.UpdateWorkflowActionType( activityTypeGuid, "Set Email Not Found Status", 3,
                            workflowActionCompleteWorkflow_EntityTypeGuid, true, true, "",
                            personEmailValid_AttributeGuid, ComparisonType.EqualTo.ConvertToInt(), "No",
                            "4FE1A541-123C-4F16-B0DE-EA90640CDE07" );
                    }

                } //foreach
            }
            catch(Exception ex )
            {
                throw new Exception( $"Error in {nameof( UpdateBackgroundCheckWorkflowInjectCheckEmailActions )}\r\n{ex}", ex.InnerException );
            }
        }
    }
}
