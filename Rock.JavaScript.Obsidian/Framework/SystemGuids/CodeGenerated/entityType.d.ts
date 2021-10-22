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

export const enum EntityType {
    /** The achievement attempt */
    AchievementAttempt = "5C144B51-3D2E-4BC2-B6C7-7E4CB890E15F",
    /** The achievement type */
    AchievementType = "0E99356C-0DEA-4F24-944E-21CD5FA83B9E",
    /** The achievement type prerequisite */
    AchievementTypePrerequisite = "5362DB19-B8E1-4378-A66A-FB097CE3AB90",
    /** The guid for the Rock.Model.Attendance entity. */
    Attendance = "4CCB856F-51E0-4E48-B94A-1705EFBA6C9E",
    /** The guid for the Rock.Model.Attribute entity. */
    Attribute = "5997C8D3-8840-4591-99A5-552919F90CBD",
    /** The database authentication provider */
    AuthenticationDatabase = "4E9B798F-BB68-4C0E-9707-0928D15AB020",
    /** The pin authentication provider */
    AuthenticationPin = "1FB5A259-F45C-4857-AF3D-3B9E32DB0EEE",
    /** The guid for the Rock.Model.Badge entity */
    Badge = "99300129-6F4C-45B2-B486-71123F046289",
    /** The benevolence request */
    BenevolenceRequest = "CF0CE5C1-9286-4310-9B50-10D040F8EBD2",
    /** The Block entity type */
    Block = "D89555CA-9AE4-4D62-8AF1-E5E463C1EF65",
    /** The campus */
    Campus = "00096BED-9587-415E-8AD4-4E076AE8FBF0",
    /** The checkr provider */
    CheckrProvider = "8D9DE88A-C649-47B2-BA5C-92A24F60AE61",
    /** The content channel type */
    ContentChannel = "44484685-477E-4668-89A6-84F29739EB68",
    /** The content channel item type */
    ContentChannelItem = "BF12AE64-21FB-433B-A8A4-E40E8C426DDA",
    /** The guid for the email communication medium */
    CommunicationMediumEmail = "5A653EBE-6803-44B4-85D2-FB7B8146D55D",
    /** The guid for the push notification communication medium */
    CommunicationMediumPushNotification = "3638C6DF-4FF3-4A52-B4B8-AFB754991597",
    /** The guid for the email communication medium */
    CommunicationMediumSms = "4BC02764-512A-4A10-ACDE-586F71D8A8BD",
    /** The guid for Rock.Model.CommunicationTemplate */
    CommunicationTemplate = "A9493AFE-4316-4651-800D-5028E4C7444D",
    /** The guid for the Rock.Model.ConnectionActivityType entity */
    ConnectionActivityType = "97B143F0-CB9D-4652-8FF1-FF2FA1EA4945",
    /** The guid for the Rock.Model.ConnectionOpportunity entity */
    ConnectionOpportunity = "79F64363-BC90-4109-9D31-A5EEB397CB2F",
    /** The guid for the Rock.Model.ConnectionOpportunityCampus entity */
    ConnectionOpportunityCampus = "E656E8B3-12AB-476E-AA63-5F9B76F64A08",
    /** The guid for the Rock.Model.ConnectionOpportunityGroup entity */
    ConnectionOpportunityGroup = "CD3F425C-9B36-4433-9C38-D58DE42C9F65",
    /** The guid for the Rock.Model.ConnectionOpportunityConnectorGroup entity */
    ConnectionOpportunityConnectorGroup = "4CB430B1-0F32-482F-9C95-164A09332CC1",
    /** The guid for the Rock.Model.ConnectionRequest entity */
    ConnectionRequest = "36B0D0C7-8125-48FA-9DA2-729AAA65F718",
    /** The guid for the Rock.Model.ConnectionRequestActivity entity */
    ConnectionRequestActivity = "3248F40D-7661-42CC-AD9B-EF63322937B7",
    /** The guid for the Rock.Model.ConnectionRequestWorkflow entity */
    ConnectionRequestWorkflow = "C69D1C9F-5521-4C83-8FE9-5044ECC2CE65",
    /** The guid for the Rock.Model.ConnectionStatus entity */
    ConnectionStatus = "F3840C8B-63BF-4F98-AC4A-9336896E589B",
    /** The guid for the Rock.Model.ConnectionType entity */
    ConnectionType = "B1E52EAD-65BD-4C4D-BCCD-73368067621D",
    /** The guid for the Rock.Model.ConnectionWorkflow entity */
    ConnectionWorkflow = "4EB8711F-7301-4699-A223-0505A7CEB20A",
    /** The guid for the Rock.Model.DataView entity. */
    Dataview = "57F8FA29-DCF1-4F74-8553-87E90F234139",
    /** The guid for the Rock.Model.DefinedType entity. */
    DefinedType = "6028D502-79F4-4A74-9323-525E90F900C7",
    /** The guid for Rock.Model.DefinedValue entity */
    DefinedValue = "53D4BF38-C49E-4A52-8B0E-5E016FB9574E",
    /** The guid for the Rock.Model.FinancialAccount entity. */
    FinancialAccount = "798BCE48-6AA7-4983-9214-F9BCEFB4521D",
    /** The guid for the Rock.Model.FinancialBatch entity. */
    FinancialBatch = "BDD09C8E-2C52-4D08-9062-BE7D52D190C2",
    /** The guid for the Rock.Model.FinancialScheduledTransaction entity. */
    FinancialScheduledTransaction = "76824E8A-CCC4-4085-84D9-8AF8C0807E20",
    /** The guid for the Rock.Model.FinancialTransaction entity. */
    FinancialTransaction = "2C1CB26B-AB22-42D0-8164-AEDEE0DAE667",
    /** The guid for the Rock.Model.FinancialTransactionDetail entity. */
    FinancialTransactionDetail = "AC4AC28B-8E7E-4D7E-85DB-DFFB4F3ADCCE",
    /** The guid for the Rock.Model.Group entity. */
    Group = "9BBFDA11-0D22-40D5-902F-60ADFBC88987",
    /** The guid for the Rock.Model.GroupMember entity. */
    GroupMember = "49668B95-FEDC-43DD-8085-D2B0D6343C48",
    /** The HTTP module component */
    HttpModuleComponent = "EDE69F48-5E05-4260-B360-DA37DFD1AB83",
    /** The guid for  */
    Interaction = "3BB4B095-2DE4-4009-8FA2-705BF284F7B7",
    /** The guid for the Rock.Model.MetricCategory entity */
    Metriccategory = "3D35C859-DF37-433F-A20A-0FFD0FCB9862",
    /** The guid for the Rock.Model.MergeTemplate entity */
    MergeTemplate = "CD1DB988-6891-4B0F-8D1B-B0A311A3BC3E",
    /** The GUID for the entity . */
    MobileAnswerToPrayerBlockType = "759AFCA0-9E0B-4A22-A402-CD4499F2A457",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Content */
    MobileContentBlockType = "B9ADB0A5-62B0-4D74-BDFF-1AA959788602",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.ContentChannelItemList */
    MobileContentChannelItemListBlockType = "6DBF59D6-EB40-43C8-8859-F38254EC3F6D",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.ContentChannelItemView */
    MobileContentChannelItemViewBlockType = "44A8B647-E0A7-42E7-9A75-276310F7E7BB",
    /** The GUID for the entity  */
    MobileCmsDailyChallengeEntry = "E9BC058A-CFE4-498B-A7E7-DD38DC74B30E",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.LavaItemList */
    MobileLavaItemListBlockType = "60AD6D70-8A2A-4CC1-97D5-199300AF77EE",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Login */
    MobileLoginBlockType = "6CE2D3D7-18D8-49FF-8C39-0CA98EB5DEB4",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Core.Notes */
    MobileCoreNotesBlockType = "2FED71D1-4A60-4EB5-B971-530B5D1FC041",
    /** The GUID for the entity . */
    MobileMyPrayerRequestsBlockType = "E644DE6A-44CA-48AC-BF33-5429DA8052C6",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.ProfileDetails */
    MobileProfileDetailsBlockType = "A1ED4948-0778-4E13-B434-E97795DDB68B",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Register */
    MobileRegisterBlockType = "4459357F-E422-45D1-855D-C4681101F848",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.WorkflowEntry */
    MobileWorkflowEntryBlockType = "02D2DBA8-5300-4367-B15B-E37DFB3F7D1E",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Cms.Hero */
    MobileCmsHeroBlockType = "49BE78CD-2D19-44C4-A6BF-4F3B5D3F97C8",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Cms.StructuredContentView */
    MobileCmsStructuredcontentviewBlockType = "219660C4-8F32-46DA-B8E3-A7A6FA0D6B76",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.CalendarEventList */
    MobileEventsCalendareventlistBlockType = "6FB9F1F4-5F24-4A22-A6EB-A7FA499179A9",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.CalendarView */
    MobileEventsCalendarviewBlockType = "5A26F32F-892E-4E76-B64A-0F54A77C863D",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Communication.CommunicationView */
    MobileCommunicationCommunicationviewBlockType = "4AF5FCEF-CBF6-486B-A04D-920E31356B7F",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.CalendarEventItemOccurrenceView */
    MobileEventsCalendareventitemoccurrenceviewBlockType = "04C43693-C524-4679-9F65-047F94A74CAB",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.CommunicationListSubscribe */
    MobileEventsCommunicationListSubscribeBlockType = "C4B81A58-6380-4C38-85E8-0536E584310E",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.EventItemOccurrenceListByAudienceLava */
    MobileEventsEventitemoccurrencelistbyaudiencelavaBlockType = "95BAF1B3-5B4B-430C-BDCC-268142C708BD",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.PrayerSession */
    MobileEventsPrayerSessionBlockType = "BCAF9B7B-2ADE-496B-9303-150F495851FC",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Events.PrayerSessionSetup */
    MobileEventsPrayerSessionSetupBlockType = "51431866-FF92-433C-8B0F-0F6BBAD9BCE7",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupRegistration */
    MobileGroupsGroupAddToGroup = "E0664BDC-9583-44F2-AC8D-23AE48603EAB",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupAttendanceEntry */
    MobileGroupsGroupAttendanceEntryBlockType = "1655E6A9-2BD6-4FA0-8886-D64DCA177FBB",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupEdit */
    MobileGroupsGroupEditBlockType = "DE46759A-CE15-4F27-9FC8-154CD30D4637",
    /** The GUID for the entity  */
    MobileGroupsGroupFinderBlockType = "15492F6A-344A-484E-AA26-A5E667CBD502",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupMemberEdit */
    MobileGroupsGroupMemberEditBlockType = "61208516-9051-4E0E-AC46-6C8E1F104F3A",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupMemberList */
    MobileGroupsGroupMemberListBlockType = "70652D98-9285-4707-8F46-B7FC48B6503D",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupMemberView */
    MobileGroupsGroupMemberViewBlockType = "3213DCBC-C5EC-4DD2-BB78-19B3636AE842",
    /** The GUID for the entity  */
    MobileGroupsGroupRegistrationBlockType = "E0664BDC-9583-44F2-AC8D-23AE48603EAB",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Groups.GroupView */
    MobileGroupsGroupViewBlockType = "564C4D86-C9DF-48D0-84B6-DD3FCC1A5158",
    /** The GUID for the entity Rock.Blocks.Types.Mobile.Prayer.PrayerRequestDetails */
    MobilePrayerPrayerRequestDetailsBlockType = "F8E56BC0-E9D1-44A4-9900-46589A1FB784",
    /** The GUID for the entity . */
    MobilePrayerPrayerCardViewBlockType = "0D0F1D7E-2D75-451B-95EE-0610B8F26BBF",
    /** The GUID for the entity . */
    MobileSecurityOnboardPerson = "C9B7F36A-F70A-4ABF-9422-B18E579F927F",
    /** The obsidian event registration entry */
    ObsidianEventRegistrationEntry = "06AAC065-BF89-483D-B671-80F0F72779A6",
    /** The obsidian event control gallery */
    ObsidianExampleControlGallery = "7B916FEC-9395-4877-9856-427419C50AB5",
    /** The obsidian event field type gallery */
    ObsidianExampleFieldTypeGallery = "82F9C803-C998-46B2-B354-783D4D1E3B43",
    /** The guid for the Rock.Model.Note entity */
    Note = "53DC1E78-14A5-44DE-903F-6A2CB02164E7",
    /** The guid for the Rock.Model.Page entity */
    Page = "E104DCDF-247C-4CED-A119-8CC51632761F",
    /** The guid for the Rock.Model.Person entity */
    Person = "72657ED8-D16E-492E-AC12-144C5E7567E7",
    /** The guid for the Rock.Model.PersonAlias entity */
    PersonAlias = "90F5E87B-F0D5-4617-8AE9-EB57E673F36F",
    /** The guid for the Rock.Workflow.Action.PersonGetCampusTeamMember entity */
    PersonGetCampusTeamMember = "6A4F7FEC-3D49-4A31-882C-2D10DB84231E",
    /** The guid for the Rock.Model.PersonSignal entity */
    PersonSignal = "0FFF77A1-E92D-4A05-8B36-1D2B6D46660F",
    /** The protect my ministry provider */
    ProtectMyMinistryProvider = "C16856F4-3C6B-4AFB-A0B8-88A303508206",
    /** The guid for the Rock.Model.Registration entity */
    Registration = "D2F294C6-E161-4A56-85C7-CD74D535F61A",
    /** The guid for the Rock.Model.RegistrationTemplate entity */
    RegistrationTemplate = "A01E3E99-A8AD-4C6C-BAAC-98795738BA70",
    /** The LiquidSelect DataSelect field for Reporting */
    ReportingDataselectLiquidselect = "C130DC52-CA31-45EE-A4F2-6C53A838EF3D",
    /** The guid for the Rock.Model.Schedule entity */
    Schedule = "0B2C38A7-D79C-4F85-9757-F1B045D32C8A",
    /** The guid for the Rock.Workflow.Action.SendEmail entity */
    SendEmail = "66197B01-D1F0-4924-A315-47AD54E030DE",
    /** The Service Job entity type */
    ServiceJob = "52766196-A72F-4F60-997A-78E19508843D",
    /** The Signal Type entity type */
    SignalType = "0BA03B9B-E974-4526-9B21-5037424B6D16",
    /** The guid for the database storage provider entity */
    StorageProviderDatabase = "0AA42802-04FD-4AEC-B011-FEB127FC85CD",
    /** The guid for  */
    Streak = "D953B0A5-0065-4624-8844-10010DE01E5C",
    /** The guid for the system communication entity */
    SystemCommunication = "D0CAD7C0-10FE-41EF-B89D-E6F0D22456C4",
    /** The guid for the file-system storage provider entity (Rock.Storage.Provider.FileSystem) */
    StorageProviderFilesystem = "A97B6002-454E-4890-B529-B99F8F2F376A",
    /** The asset storage 'Amazon S3' component (Rock.Storage.AssetStorage.AmazonS3Component) */
    StorageAssetstorageAmazons3 = "FFE9C4A0-7AB7-48CA-8938-EC73DEC134E8",
    /** The asset storage 'Azure Cloud Storage' component (Rock.Storage.AssetStorage.AzureCloudStorageComponent) */
    StorageAssetstorageAzurecloud = "1576800F-BFD2-4309-A2C9-AE6DF6C0A1A5",
    /** The asset storage 'Google Cloud Storage' component (Rock.Storage.AssetStorage.GoogleCloudStorageComponent) */
    StorageAssetstorageGooglecloud = "71344FA8-4210-4B6C-ADC1-9F63C4CA15CA",
    /** The asset storage file-system component (Rock.Storage.AssetStorage.FileSystemComponent) */
    StorageAssetstorageFilesystem = "FFEA94EA-D394-4C1A-A3AE-23E6C50F047A",
    /** The EntityType Guid for  */
    Workflow = "3540E9A7-FE30-43A9-8B0A-A372B63DFC93",
    /** The EntityType Guid for  */
    WorkflowType = "C9F3C4A5-1526-474D-803F-D6C7A45CBBAE",
    /** The EntityType Guid for  */
    WorkflowActionType = "23E3273A-B137-48A3-9AFF-C8DC832DDCA6",
    /** The guid for the Test Financial Gateway entity type */
    FinancialGatewayTestGateway = "C22B0247-7C9F-411B-A1F5-0051FCBAC199",
    /** The guid for the Step entity type */
    Step = "8EADB0DC-17F4-4541-A46E-53F89E21A622",
    /** The guid for the Step program entity */
    StepProgram = "E89F9528-A74E-41B7-8B65-B56B4CE7A122",
    /** The MyWell financial gateway */
    MywellFinancialGateway = "E81ED723-E807-4BDE-ADF1-AB9686241637",
    /** The SMS Conversation Action */
    SmsActionConversation = "E808A9FD-06A7-4FB2-AD01-C826A53B0ABB",
    /** Rock.Model.Site EntityType guid */
    Site = "7244C10B-5D87-467B-A7F5-12DC29910CA8",
    /** The EntityType Guid for   */
    AccumulativeAchievementComponent = "05D8CD17-E07D-4927-B9C4-5018F7C4B715",
    /** The EntityType Guid for   */
    StreakAchievementComponent = "174F0AFF-3A5E-4A20-AE8B-D8D83D43BACD",
    /** The EntityType Guid for   */
    StepProgramAchievementComponent = "7140BAE3-89E9-423E-A691-6E13544203CA",
    /** The EntityType Guid for   */
    InteractionSourcedAchievementComponent = "1F2B13BE-EFAA-4D4E-B2D2-D221B51AEA67",
}

