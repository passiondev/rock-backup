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

import { computed, defineComponent, reactive, Ref, ref, watch } from "vue";
import { useConfigurationValues } from "../../Util/block";
import DropDownList from "../../Elements/dropDownList";
import Modal from "../../Controls/modal";
import Panel from "../../Controls/panel";
import RockButton from "../../Elements/rockButton";
import RockLabel from "../../Elements/rockLabel";
import RockForm from "../../Controls/rockForm";
import Switch from "../../Elements/switch";
import TextBox from "../../Elements/textBox";
import ConfigurableZone from "./FormBuilderDetail/configurableZone";
import FormBuilderTab from "./FormBuilderDetail/formBuilderTab";
import CommunicationsTab from "./FormBuilderDetail/communicationsTab";

export default defineComponent({
    name: "Workflow.FormBuilderDetail",

    components: {
        ConfigurableZone,
        CommunicationsTab,
        DropDownList,
        FormBuilderTab,
        Modal,
        Panel,
        RockButton,
        RockForm,
        RockLabel,
        Switch,
        TextBox
    },

    setup() {
        const selectedTab = ref(0);

        const isFormBuilderTabSelected = computed((): boolean => selectedTab.value === 0);
        const isCommunicationsTabSelected = computed((): boolean => selectedTab.value === 1);
        const isSettingsTabSelected = computed((): boolean => selectedTab.value === 2);

        const formBuilderContainerStyle = computed((): Record<string, string> => {
            return {
                display: isFormBuilderTabSelected.value ? "flex" : "none"
            };
        });

        const communicationsContainerStyle = computed((): Record<string, string> => {
            return {
                display: isCommunicationsTabSelected.value ? "flex" : "none"
            };
        });

        const onFormBuilderTab = (): void => {
            selectedTab.value = 0;
        };

        const onCommunicationsTab = (): void => {
            selectedTab.value = 1;
        };

        return {
            communicationsContainerStyle,
            formBuilderContainerStyle,
            isCommunicationsTabSelected,
            isFormBuilderTabSelected,
            isSettingsTabSelected,
            onCommunicationsTab,
            onFormBuilderTab
        };
    },

    template: `
<Panel type="block" hasFullscreen :isFullscreenPageOnly="true" title="Workflow Form Builder" titleIconClass="fa fa-hammer">
    <template #default>
        <v-style>
            /*** Overrides for theme CSS ***/
            .form-builder-detail .form-section {
                margin-bottom: 0px;
            }

            .custom-switch {
                position: relative;
            }

            /*** Style Variables ***/
            .form-builder-detail {
                --zone-color: #e1e1e1;
                --zone-action-text-color: #a7a7a7;
                --zone-active-color: #c9eaf9;
                --zone-active-action-text-color: #83bad3;
                --zone-highlight-color: #ee7725;
                --zone-highlight-action-text-color: #e4bda2;
                --flex-col-gutter: 30px;
            }

            /*** Form Template Items ***/
            .form-builder-detail .form-template-item {
                display: flex;
                align-items: center;
                background-color: #ffffff;
                border: 1px solid #e1e1e1;
                border-left: 3px solid #e1e1e1;
                border-radius: 3px;
                padding: 6px;
                font-size: 13px;
                font-weight: 600;
                cursor: grab;
            }

            .form-builder-detail .form-template-item > .fa {
                margin-right: 6px;
            }

            .form-builder-detail .form-template-item > .text {
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }

            .form-builder-detail .form-template-item.form-template-item-section {
                border-left-color: #009ce3;
            }

            .form-builder-detail .form-template-item.form-template-item-field {
                border-left-color: #ee7725;
                margin-right: 5px;
                margin-bottom: 5px;
                flex-basis: calc(50% - 5px);
            }

            /*** Configuration Asides ***/
            .form-builder-detail .aside-header {
                border-right: 1px solid #dfe0e1;
                border-bottom: 1px solid #dfe0e1;
            }

            .form-builder-detail .aside-header .fa {
                margin-right: 4px;
            }

            .form-builder-detail .aside-header .title {
                font-size: 85%;
                font-weight: 600;
            }

            .form-builder-detail .aside-body {
                padding: 15px;
            }

            /*** Configurable Zones ***/
            .form-builder-detail .configurable-zone {
                display: flex;
                margin-bottom: 12px;
            }

            .form-builder-detail .configurable-zone.field-zone {
                flex-grow: 1;
            }

            .form-builder-detail .configurable-zone > .zone-content-container {
                display: flex;
                flex-grow: 1;
                border: 2px solid var(--zone-color);
            }

            .form-builder-detail .configurable-zone.field-zone > .zone-content-container {
                border-style: dashed;
                border-right-style: solid;
            }

            .form-builder-detail .configurable-zone > .zone-content-container > .zone-content {
                flex-grow: 1;
            }

            .form-builder-detail .configurable-zone > .zone-content-container > .zone-content > .zone-body {
                padding: 20px;
            }

            .form-builder-detail .configurable-zone > .zone-actions {
                background-color: var(--zone-color);
                border: 2px solid var(--zone-color);
                border-left: 0px;
                width: 40px;
                flex-shrink: 0;
                display: flex;
                justify-content: center;
                align-items: center;
                color: var(--zone-action-text-color);
                cursor: pointer;
            }

            .form-builder-detail .configurable-zone.active > .zone-content-container {
                border-color: var(--zone-active-color);
            }

            .form-builder-detail .configurable-zone.active > .zone-actions {
                background-color: var(--zone-active-color);
                border-color: var(--zone-active-color);
                color: var(--zone-active-action-text-color);
            }

            .form-builder-detail .configurable-zone.highlight > .zone-content-container {
                border-color: var(--zone-highlight-color);
                border-right-style: dashed;
            }

            /*** Form Sections ***/
            .form-builder-detail .form-section {
                display: flex;
                flex-wrap: wrap;
                align-content: flex-start;
                margin-right: calc(0px - var(--flex-col-gutter));
                min-height: 50px;
                flex-grow: 1;
            }

            .form-builder-detail .form-section .form-template-item.form-template-item-field {
                margin: 0px 0px 12px 0px;
                flex-basis: calc(100% - var(--flex-col-gutter));
            }

            /*** Flex Column Sizes ***/
            .form-builder-detail .flex-col {
                margin-right: var(--flex-col-gutter);
            }

            .form-builder-detail .flex-col-1 {
                flex-basis: calc(8.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-2 {
                flex-basis: calc(16.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-3 {
                flex-basis: calc(25% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-4 {
                flex-basis: calc(33.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-5 {
                flex-basis: calc(41.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-6 {
                flex-basis: calc(50% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-7 {
                flex-basis: calc(58.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-8 {
                flex-basis: calc(66.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-9 {
                flex-basis: calc(75% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-10 {
                flex-basis: calc(83.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-11 {
                flex-basis: calc(91.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-12 {
                flex-basis: calc(100% - var(--flex-col-gutter));
            }
        </v-style>

        <div ref="bodyElement" class="form-builder-detail d-flex flex-column panel-flex-fill-body" style="overflow-y: hidden;">
            <div class="p-2 d-flex" style="border-bottom: 1px solid #dfe0e1;">
                <ul class="nav nav-pills" style="flex-grow: 1;">
                    <li role="presentation"><a href="#">Submissions</a></li>
                    <li :class="{ active: isFormBuilderTabSelected }" role="presentation"><a href="#" @click.prevent="onFormBuilderTab">Form Builder</a></li>
                    <li :class="{ active: isCommunicationsTabSelected }" role="presentation"><a href="#" @click.prevent="onCommunicationsTab">Communications</a></li>
                    <li :class="{ active: isSettingsTabSelected }" role="presentation"><a href="#">Settings</a></li>
                    <li role="presentation"><a href="#">Analytics</a></li>
                </ul>

                <div>
                    <RockButton btnType="primary">Save</RockButton>
                </div>
            </div>

            <div style="flex-grow: 1; overflow-y: hidden;" :style="formBuilderContainerStyle">
                <FormBuilderTab />
            </div>

            <div style="flex-grow: 1; overflow-y: hidden;" :style="communicationsContainerStyle">
                <CommunicationsTab />
            </div>
        </div>
    </template>
</Panel>
`
});
