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

import { IEntity } from "../entity";

export type ConnectionRequest = IEntity & {
    assignedGroupId?: number | null;
    assignedGroupMemberAttributeValues?: string | null;
    assignedGroupMemberRoleId?: number | null;
    assignedGroupMemberStatus?: number | null;
    campusId?: number | null;
    comments?: string | null;
    connectionOpportunityId?: number;
    connectionState?: number;
    connectionStatusId?: number;
    connectorPersonAliasId?: number | null;
    followupDate?: string | null;
    order?: number;
    personAliasId?: number;
    createdDateTime?: string | null;
    modifiedDateTime?: string | null;
    createdByPersonAliasId?: number | null;
    modifiedByPersonAliasId?: number | null;
};
