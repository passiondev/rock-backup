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
            const string pmm_ActivityTypeGuid = "2950B120-7BB5-46B5-93D0-26D3936F1894";
            const string checkr_ActivityTypeGuid = "CB30F298-7532-446C-949E-2FEC156CE700";
            // Background check [Initial Request] activity types: First entry [0] is Protect My Ministry Guid Type, the second [1] is Checkr
            var backgroundCheck_WorkflowActivityTypeGuids = new[] { pmm_ActivityTypeGuid, checkr_ActivityTypeGuid };

            foreach ( var activityTypeGuid in backgroundCheck_WorkflowActivityTypeGuids )
            {
                var bgCheck_ActivityTypeIdSql = $"SELECT TOP 1 [Id] FROM WorkflowActivityType WHERE [Guid] = '{activityTypeGuid}'";
                var bgCheck_ActivityTypeId = SqlScalar( bgCheck_ActivityTypeIdSql ).ToIntSafe();

                if ( bgCheck_ActivityTypeId > 0 )
                {
                    // PersonEmailValid - Attribute - Guid
                    var personEmailValid_AttributeGuid = Guid.NewGuid().ToString().ToUpper();

                    // Add Activity [PersonEmailValid] Entity Attribute
                    RockMigrationHelper.AddOrUpdateEntityAttribute( "Rock.Model.WorkflowActivity",
                        "9C204CD0-1233-41C5-818A-C5DA439445AA", "ActivityTypeId",
                        bgCheck_ActivityTypeId.ToString(),
                        "PersonEmailValid",
                        "PersonEmailValid",
                        "Used to indicate if a person has a valid and active email before executing a background check.", 0, "No",
                        personEmailValid_AttributeGuid, "PersonEmailValid" );

                    // Reorder Workflow Activity - Action entries
                    if ( activityTypeGuid == pmm_ActivityTypeGuid )
                    {
                        // Set Person is already order 0

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
                    else if ( activityTypeGuid == checkr_ActivityTypeGuid )
                    {
                        // Set Person 
                        Sql( "Update [WorkflowActionType] SET [Order] = 0 WHERE [Guid] = '407C6B9C-2E89-400F-8345-A062B1083F66'" );

                        // Set Status
                        Sql( "Update [WorkflowActionType] SET [Order] = 4 WHERE [Guid] = '5556D7BA-4EFF-400A-A7A9-E8EFE4519985'" );

                        // Set Requester 
                        Sql( "Update [WorkflowActionType] SET [Order] = 5 WHERE [Guid] = '46BF8484-6386-4A45-A332-A107185E7353'" );

                        // Set Name
                        Sql( "Update [WorkflowActionType] SET [Order] = 6 WHERE [Guid] = '8C59CA7A-4FF1-410F-9CBC-81FA7BDEE610'" );

                        // Set Warning
                        Sql( "Update [WorkflowActionType] SET [Order] = 7 WHERE [Guid] = '2EE62F7C-EABE-4C9C-B00B-045984C462E9'" );

                        // Get Details
                        Sql( "Update [WorkflowActionType] SET [Order] = 8 WHERE [Guid] = '9751CA4C-A648-4567-B31E-4B077DDFF124'" );

                    }

                    // Add/Inject new Workflow Activity - Action entries

                    // Add [Set PersonEmailValid Attribute] Action - Order 1
                    var setPersonEmailValidAttributeActionTypeGuid = Guid.NewGuid().ToString().ToUpper();
                    RockMigrationHelper.UpdateWorkflowActionType( activityTypeGuid, "Set PersonEmailValid Attribute", 1,
                        "BC21E57A-1477-44B3-A7C2-61A806118945", true, false, "",
                        "", ComparisonType.EqualTo.ConvertToInt(), "",
                        setPersonEmailValidAttributeActionTypeGuid );

                    // Add [Show Email Not Found Message] Action - Order 2
                    var showEmailNotFoundMessageActionTypeGuid = Guid.NewGuid().ToString().ToUpper();
                    RockMigrationHelper.UpdateWorkflowActionType( activityTypeGuid, "Show Email Not Found Message", 2,
                        "FDDAE78D-B7B3-4DA2-9A92-CC129AAF15DE", true, false, "",
                        personEmailValid_AttributeGuid, ComparisonType.EqualTo.ConvertToInt(), "No",
                        showEmailNotFoundMessageActionTypeGuid );

                    // Add [Set Email Not Found Status] Action - Order 3
                    var setEmailNotFoundStatusActionTypeGuid = Guid.NewGuid().ToString().ToUpper();
                    RockMigrationHelper.UpdateWorkflowActionType( activityTypeGuid, "Set Email Not Found Status", 3,
                        "EEDA4318-F014-4A46-9C76-4C052EF81AA1", true, true, "",
                        personEmailValid_AttributeGuid, ComparisonType.EqualTo.ConvertToInt(), "No",
                        setEmailNotFoundStatusActionTypeGuid );

                    #region Add [Set PersonEmailValid Attribute] Value
                    // Add [Set PersonEmailValid Attribute] Value
                    const string setPersonEmailValidLava = @"{% assign person = Workflow | Attribute:'Person','Object' %}
{% assign emailLength = person.Email | Size %}
{% assign emailActive = person.IsEmailActive | AsBoolean %}
{% if emailLength > 0 and emailActive %}
Yes
{% else %}
No
{% endif %}";
                    #endregion Add [Set PersonEmailValid Attribute] Lava Value

                    // Set PersonEmailValid Attribute - Action - Lava
                    RockMigrationHelper.AddActionTypeAttributeValue( setPersonEmailValidAttributeActionTypeGuid, "F1F6F9D6-FDC5-489C-8261-4B9F45B3EED4",
                        setPersonEmailValidLava );

                    // Set PersonEmailValid Attribute - Action - Select Attribute
                    RockMigrationHelper.AddActionTypeAttributeValue( setPersonEmailValidAttributeActionTypeGuid, "431273C6-342D-4030-ADC7-7CDEDC7F8B27",
                 personEmailValid_AttributeGuid );

                    #region [Show Email Not Found Message] Lava Value
                    // [Show Email Not Found Message] Value
                    const string personEmailNotFoundMessage = @"{% assign person = Workflow | Attribute:'Person','Object' %}
{% assign personProfilePage = '~/Person/' %}
{% assign personUrl = personProfilePage | ResolveRockUrl person.Id %}

<div class='alert alert-danger margin-t-lg js-workflow-entry-message-notification-box'>
    <strong> Error: Email Required</strong>
    <span class='js-notification-text'>
	    <ul>
	        <li><strong><a href='{{ personProfilePage | ResolveRockUrl }}{{ person.Id }}' target='_blank'>{{ Workflow | Attribute:'Person','FullName' }}</a></strong> has an empty or inactive email address. A background check can not be run unless a valid email address is provided.</li>
				    <ul>
                        <li>Enter a valid email address and/or set it to active.</li>
                    </ul>
	    </ul>
	</span>
</div>";
                    #endregion [Show Email Not Found Message] Value

                    // Set Email Not Found Message - Action - HTML\Lava
                    RockMigrationHelper.AddActionTypeAttributeValue( showEmailNotFoundMessageActionTypeGuid, "DCC5F049-8BDB-4DBF-B07A-081C0B772473",
                        personEmailNotFoundMessage );

                    // Set Email Not Found Message - Action - Hide Status Message
                    RockMigrationHelper.AddActionTypeAttributeValue( showEmailNotFoundMessageActionTypeGuid, "4F9306CC-A50F-4A9A-B7A5-BCC2C1263EE2",
                        "True" );

                    // Set Email Not Found Status - Action
                    RockMigrationHelper.AddActionTypeAttributeValue( setEmailNotFoundStatusActionTypeGuid, "07CB7DBC-236D-4D38-92A4-47EE448BA89A",
                        "Person Email Not Found" );

                    // Fix Lava in the My Ministry background check [Get Details] - Action
                    if ( activityTypeGuid == pmm_ActivityTypeGuid )
                    {
                        const string header = @"{% assign WarnOfRecent = Workflow | Attribute:'WarnOfRecent' %}
<h1>Background Request Details</h1>
<p>{{ CurrentPerson.NickName }}, please complete the form below to start the background request process.</p>
{% if WarnOfRecent == 'Yes' %}
    <div class='alert alert-warning'>
        Notice: It's been less than a year since this person's last background check was processed.
        Please make sure you want to continue with this request!
    </div>
{% endif %}
<hr/>";

                        const string actions = "Submit^fdc397cd-8b4a-436e-bea1-bce2e6717c03^342BCBFC-2CA7-426E-ABBB-A7C461A05736^Your request has been submitted successfully.|Cancel^5683E775-B9F3-408C-80AC-94DE0E51CF3A^F47C3F69-4485-4A6A-BFCE-C44FE628DF3E^The request has been cancelled.|";

                        // Update using the fixed lava and the current Action values for the Action Form
                        RockMigrationHelper.UpdateWorkflowActionForm( header, "", actions, "", false, "", "328B74E5-6058-4C4E-9EF8-EC10985F18A8" );
                    }
                }

            } //foreach
        }
    }
}
