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

import { Component } from "vue";
import { Guid } from "../../../Util/guid";
import { ListItem } from "../../../ViewModels";

export type FormSection = {
    guid: Guid;

    title: string;

    description: string;

    showHeadingSeparator: boolean;

    type: SectionStyleType;

    fields: FormField[];
};

export type FormField = {
    guid: Guid;

    fieldTypeGuid: Guid;

    name: string;

    description?: string;

    key: string;

    size: number;

    isRequired?: boolean;

    isHideLabel?: boolean;

    isShowOnGrid?: boolean;

    configurationValues?: Record<string, string>;

    defaultValue?: string;
};

export type FormFieldType = {
    text: string;

    guid: Guid;

    icon: string;

    isCommon: boolean;
}

export type GeneralAsideSettings = {
    campusSetFrom?: number;

    hasPersonEntry?: boolean;
};

export type SectionAsideSettings = {
    guid: Guid;

    title: string;

    description: string;

    showHeadingSeparator: boolean;

    type: SectionStyleType;
};

export const enum SectionStyleType {
    None = 0,

    Well = 1
}

export interface IAsideProvider {
    isSafeToClose: () => boolean;
}

export const enum FormEmailSourceType {
    UseTemplate = 0,

    Custom = 1
}

export type FormConfirmationEmail = {
    enabled?: boolean;

    recipientAttributeGuid?: ListItem | null;

    source?: FormEmailSource | null;
};

export type FormEmailSource = {
    type?: FormEmailSourceType;

    template?: ListItem | null;

    subject?: string | null;

    replyTo?: string | null;

    body?: string | null;

    appendOrgHeaderAndFooter?: boolean;
};

export const enum NotificationEmailDestination {
    SpecificIndividual = 0,

    EmailAddress = 1,

    CampusTopic = 2
}

export type FormNotificationEmail = {
    enabled?: boolean;

    destination?: NotificationEmailDestination;

    recipient?: ListItem | null;

    emailAddress?: string | null;

    campusTopicGuid?: Guid | null;

    source?: FormEmailSource;
};

export type FormCommunication = {
    confirmationEmail?: FormConfirmationEmail;

    notificationEmail?: FormNotificationEmail;
};

export type FormGeneralSettings = {
    name?: string | null;

    description?: string | null;

    template?: string | null;

    category?: ListItem | null;

    entryStarts?: string | null;

    entryEnds?: string | null;
};

export type FormCompletionSettings = {
    type?: CompletionResponseType;

    message?: string | null;

    redirectUrl?: string | null;
};

export const enum CompletionResponseType {
    DisplayMessage = 0,

    Redirect = 1
}

export type FormSettings = {
    general?: FormGeneralSettings | null;

    completion?: FormCompletionSettings | null;
};

export const enum WorkflowActionFormPersonEntryOption {
    /** Don't show the control. */
    Hidden = 0,

    /** Control is visible, but a value is not required. */
    Optional = 1,

    /** Control is visible, and a value is required. */
    Required = 2
}

export type FormPersonEntrySettings = {
    autofillCurrentPerson?: boolean;

    hideIfCurrentPersonKnown?: boolean;

    recordStatus?: string | null;

    connectionStatus?: string | null;

    showCampus?: boolean;

    campusType?: string | null;

    campusState?: string | null;

    gender?: WorkflowActionFormPersonEntryOption;

    email?: WorkflowActionFormPersonEntryOption;

    mobilePhone?: WorkflowActionFormPersonEntryOption;

    birthdate?: WorkflowActionFormPersonEntryOption;

    address?: WorkflowActionFormPersonEntryOption;

    addressType?: string | null;

    maritalStatus?: WorkflowActionFormPersonEntryOption;

    spouseEntry?: WorkflowActionFormPersonEntryOption;

    spouseLabel?: string | null;
};
