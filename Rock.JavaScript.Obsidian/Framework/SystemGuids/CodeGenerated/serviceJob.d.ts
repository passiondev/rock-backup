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

export const enum ServiceJob {
    /** Gets the Job Pulse guid */
    JobPulse = "CB24FF2A-5AD3-4976-883F-DAF4EFC1D7C7",
    /** The Job for migrating pre-v8.0 attendance records to occurrence records */
    MigrateAttendanceOccurrence = "98A2DCA5-5E2E-482A-A7CA-15DAD5B4EA65",
    /** The Job for migrating family check-in identifiers to person alternate ids */
    MigrateFamilyCheckinIds = "E782C667-EF07-4AD2-86B7-01C1935AAF5B",
    /** The Job to run Post v8.0 Data Migrations */
    DataMigrations80 = "AF760EF9-66BD-4A4D-AF95-749AA789ACAA",
    /** The Job to run Post v8.4 Data Migrations */
    DataMigrations84 = "79FBDA04-ADFD-40D4-824F-E07D660F7858",
    /** The Job to run Post v7.4 Data Migrations */
    DataMigrations74 = "FF760EF9-66BD-4A4D-AF95-749AA789ACAF",
    /** The Job to run Post v9 Data Migrations for DISC data */
    DataMigrations90Disc = "A839DFEC-B1A3-499C-9BB3-03241E8E5305",
    /** The Job to run Post v10.0 Data Migrations for AttributeValue.ValueAsNumeric */
    DataMigrations100AttributevalueValueasnumeric = "0A7573C9-D977-4A7E-BDD6-66DD36CBF6F3",
    /** The Job to run Post v10.0 Data Migrations for SundayDate */
    DataMigrations100Sundaydate = "CC263453-B290-4393-BB91-1C1C87CAE291",
    /** The Job to run Post v9 Data Migrations to convert Scheduled Transaction Notes to History */
    DataMigrations90Scheduledtransactionnotestohistory = "6707AA98-7CF8-4258-A75A-0881CD68B0D9",
    /** The Job to run Post v9 Data Migrations */
    DataMigrations90 = "3F279016-C7D1-490F-835D-8FFE6D943A32",
    /** The Job to run Post v10.3 Data Migrations for Spiritual Gifts Assessment updates */
    DataMigrations103SpiritualGifts = "B16F889F-3349-4CA9-976D-7EF098DD8BC6",
    /** The Job to run Post v11 Data Migrations to update Date Keys on several tables */
    DataMigrations110PopulateDateKeys = "E56FD4FC-02F8-4A46-A91D-E86C2B635870",
    /** The job to run Post V11 to create an index on  */
    DataMigrations110CommunicationrecipientResponsecodeIndex = "131F9418-777B-4A34-A19B-EB9A65893602",
    /** The Job to run Post v11 Data Migrations to update Related DataView Id in DataView Filter table. */
    DataMigrations110PopulateRelatedDataviewId = "C3882742-714B-4E82-8894-4B944142CDC7",
    /** The Job to run Post v12 to update interaction indexes. */
    DataMigrations120UpdateInteractionIndexes = "090CB437-F74B-49B0-8B51-BF2A491DD36D",
    /** The data migrations 120 add communication recipient index */
    DataMigrations120AddCommunicationrecipientIndex = "AD7CAEAC-6C84-4B55-9E5A-FEE085C270E4",
    /** The data migrations 120 add communication get queued index */
    DataMigrations120AddCommunicationGetQueuedIndex = "BF3AADCC-B2A5-4EB9-A365-08C3F052A551",
    /** The Job to run Post v12.2 Data Migrations for adding PersonalDeviceId to Interaction index */
    DataMigrations122InteractionPersonalDeviceId = "6BEDCC6F-620B-4DE0-AE9F-F6DB0E0153E4",
    /** The Job to run Post v12.4 Data Migrations for Update Group Salutation fields on Rock.Model.Group. */
    DataMigrations124UpdateGroupSalutations = "584F899B-B974-4847-9473-15099AADD577",
    /** The Job to run Post v12.5 Data Migrations for Update Step Program Completion */
    DataMigrations125UpdateStepProgramCompletion = "E7C54AAB-451E-4E89-8083-CF398D37416E",
    /** The Job to run Post v12.5 Data Migrations for Add SystemCommunicationId index to Communication */
    DataMigrations125AddCommunicationSystemCommunicationIdIndex = "DA54E879-44CE-433C-A472-54B57B11CB7B",
    /** The Job to run Post v12.7 Data Migrations for Rebuild Group Salutation fields on Rock.Model.Group. */
    DataMigrations127RebuildGroupSalutations = "FD32833A-6FC8-43E6-8D36-0C840DBE99F8",
    /** The Job to run Post v13.0 Data Migrations for Add InteractionComponentId index to Interaction */
    DataMigrations130AddInteractionInteractionComponentIdIndex = "1D7FADEC-2A8A-46FD-898E-58544E7FD9F2",
    /** The Job to Migrate pre-v8.0 History Summary Data */
    MigrateHistorySummaryData = "CF2221CC-1E0A-422B-B0F7-5D81AF1DDB14",
    /** The Job to Migrate pre-v7.0 PageViews and Communication Recipient Activity to Interactions */
    MigrateInteractionsData = "189AE3F1-92E9-4394-ACC5-0F244967F32E",
    /** The job to migrate pre-v7.0 Communication Medium data from JSON to regular fields */
    MigrateCommunicationMediumData = "E7C54AAB-451E-4E89-8083-CF398D37416E",
    /** The Job to run Post v12.4 Data Migrations to decrypt the expiration month / year and the name on card fields. */
    DataMigrations124DecryptFinancialPaymentDetails = "6C795E61-9DD4-4BE8-B9EB-E662E43B5E12",
    /** The Job to get NCOA */
    GetNcoa = "D2D6EA6C-F94A-39A0-481B-A23D08B887D6",
    /** The Job to Rebuild a Sequence. This job has been deleted and replaced with
 */
    RebuildStreak = "BFBB9524-10E8-42CF-BCD3-0CC7D2B22C3A",
    /** The rock cleanup Job.  */
    RockCleanup = "1A8238B1-038A-4295-9FDE-C6D93002A5D7",
    /** The steps automation job - add steps based on people in a dataview */
    StepsAutomation = "97858941-0447-49D6-9E35-B03665FEE965",
    /** The collect hosting metrics job - collect metrics regarding database connections, Etc. */
    CollectHostingMetrics = "36FA38CA-9DB0-40A8-BABD-5411121B4809",
    /** The Job to send an email digest with an attendance summary of all child groups to regional group leaders */
    SendGroupAttendanceDigest = "9F9E9C3B-FC58-4939-A272-4FA86D44CE7B",
    /** A run once job after a new installation. The purpose of this job is to populate generated datasets after an initial installation using RockInstaller that are too large to include in the installer. */
    PostInstallDataMigrations = "322984F1-A7A0-4D1B-AE6F-D7F043F66EB3",
    /** The  job. */
    GivingAutomation = "B6DE0544-8C91-444E-B911-453D4CE71515",
    /** Use  instead */
    GivingAnalytics = "B6DE0544-8C91-444E-B911-453D4CE71515",
    /** The media synchronize job. */
    SyncMedia = "FB27C6DF-F8DB-41F8-83AF-BBE09E77A0A9",
    /** The Process Elevated Security Job.  */
    ProcessElevatedSecurity = "A1AF9D7D-E968-4AF6-B203-6BB4FD625714",
}

