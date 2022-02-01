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

import { Guid } from "../../../../Util/guid";
import { ListItem } from "../../../../ViewModels";
import { FieldVisibilityRule } from "../../../../ViewModels/fieldVisibilityRule";

// #region Enums

export const enum FormEmailSourceType {
    UseTemplate = 0,

    Custom = 1
}

export const enum FormNotificationEmailDestination {
    SpecificIndividual = 0,

    EmailAddress = 1,

    CampusTopic = 2
}

export const enum FormCompletionActionType {
    DisplayMessage = 0,

    Redirect = 1
}

// #endregion

// #region Interfaces

export interface IAsideProvider {
    isSafeToClose: () => boolean;
}

// #endregion

// #region Types

export type FormSection = {
    guid: Guid;

    title?: string | null;

    description?: string | null;

    showHeadingSeparator?: boolean;

    type?: string | null;

    fields?: FormField[] | null;
};

export type FormField = {
    guid: Guid;

    fieldTypeGuid: Guid;

    name: string;

    description?: string | null;

    key: string;

    size: number;

    isRequired?: boolean;

    isHideLabel?: boolean;

    isShowOnGrid?: boolean;

    configurationValues?: Record<string, string> | null;

    defaultValue?: string | null;
};

export type FormFieldType = {
    text: string;

    guid: Guid;

    icon: string;

    isCommon: boolean;
}

export type GeneralAsideSettings = {
    campusSetFrom?: number; // TODO: Enum

    hasPersonEntry?: boolean;
};

export type SectionAsideSettings = {
    guid: Guid;

    title: string;

    description: string;

    showHeadingSeparator: boolean;

    type: Guid | null;
};

export type FormConfirmationEmail = {
    enabled?: boolean;

    recipientAttributeGuid?: string | null;

    source?: FormEmailSource | null;
};

export type FormEmailSource = {
    type?: FormEmailSourceType;

    template?: string | null;

    subject?: string | null;

    replyTo?: string | null;

    body?: string | null;

    appendOrgHeaderAndFooter?: boolean;
};

export type FormNotificationEmail = {
    enabled?: boolean;

    destination?: FormNotificationEmailDestination;

    recipient?: ListItem | null;

    emailAddress?: string | null;

    campusTopicGuid?: Guid | null;

    source?: FormEmailSource;
};

export type FormCommunication = {
    confirmationEmail?: FormConfirmationEmail;

    notificationEmail?: FormNotificationEmail;
};

export type FormGeneral = {
    name?: string | null;

    description?: string | null;

    template?: string | null;

    category?: ListItem | null;

    entryStarts?: string | null;

    entryEnds?: string | null;

    isLoginRequired?: boolean;
};

export type FormCompletionAction = {
    type?: FormCompletionActionType;

    message?: string | null;

    redirectUrl?: string | null;
};

export type FormSettings = {
    headerContent?: string | null;

    footerContent?: string | null;

    sections?: FormSection[] | null;

    general?: FormGeneral | null;

    confirmationEmail?: FormConfirmationEmail | null;

    notificationEmail?: FormNotificationEmail | null;

    completion?: FormCompletionAction | null;

    personEntry?: FormPersonEntry | null;
};

export type FormPersonEntry = {
    autofillCurrentPerson?: boolean;

    hideIfCurrentPersonKnown?: boolean;

    recordStatus?: string | null;

    connectionStatus?: string | null;

    showCampus?: boolean;

    campusType?: string | null;

    campusStatus?: string | null;

    gender?: FieldVisibilityRule;

    email?: FieldVisibilityRule;

    mobilePhone?: FieldVisibilityRule;

    birthdate?: FieldVisibilityRule;

    address?: FieldVisibilityRule;

    addressType?: string | null;

    maritalStatus?: FieldVisibilityRule;

    spouseEntry?: FieldVisibilityRule;

    spouseLabel?: string | null;
};

// #endregion
