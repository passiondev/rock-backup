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

/** An enum that represents when a Job notification status should be sent. */
export const enum JobNotificationStatus {
    /** Notifications should be sent when a job completes with any notification status. */
    All = 1,

    /** Notification should be sent when the job has completed successfully. */
    Success = 2,

    /** Notification should be sent when the job has completed with an error status. */
    Error = 3,

    /** Notifications should not be sent when this job completes with any status. */
    None = 4
}
