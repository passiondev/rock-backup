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

import { IEntity, Person } from "../ViewModels";
import { Guid } from "./guid";

export type PageConfig = {
    executionStartTime: number;
    pageId: number;
    pageGuid: Guid;
    pageParameters: Record<string, unknown>;
    currentPerson: Person | null;
    contextEntities: Record<string, IEntity>;
    loginUrlWithReturnUrl: string;
};

export function smoothScrollToTop(): void {
    window.scrollTo({ top: 0, behavior: "smooth" });
}

export default {
    smoothScrollToTop
};


/*
 * Code to handle working with modals.
 */
let currentModalCount = 0;

/**
 * Track a modal being opened or closed. This is used to adjust the page in response
 * to any modals being visible.
 * 
 * @param state true if the modal is now open, false if it is now closed.
 */
export function trackModalState(state: boolean): void {
    const body = document.body;
    const cssClasses = ["modal-open"];

    if (state) {
        currentModalCount++;
    }
    else {
        currentModalCount = currentModalCount > 0 ? currentModalCount - 1 : 0;
    }

    if (currentModalCount > 0) {
        for (const cssClass of cssClasses) {
            body.classList.add(cssClass);
        }
    }
    else {
        for (const cssClass of cssClasses) {
            body.classList.remove(cssClass);
        }
    }
}

