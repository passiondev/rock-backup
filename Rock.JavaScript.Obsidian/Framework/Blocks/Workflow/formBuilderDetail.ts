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

import { defineComponent } from "vue";
import { useConfigurationValues } from "../../Util/block";
import DropDownList from "../../Elements/dropDownList";
import Panel from "../../Controls/panel";
import RockButton from "../../Elements/rockButton";
import Switch from "../../Elements/switch";
import { DragSource, DragTarget, IDragSourceOptions } from "../../Directives/dragDrop";
import { newGuid } from "../../Util/guid";

export default defineComponent({
    name: "Workflow.FormBuilderDetail",
    components: {
        DropDownList,
        Panel,
        RockButton,
        Switch
    },

    directives: {
        DragSource,
        DragTarget,
    },

    setup() {
        const config = useConfigurationValues<Record<string, string> | null>();

        const dragSourceOptions: IDragSourceOptions = {
            id: newGuid(),
            copyElement: true,
            dragOver(operation) {
                if (operation.targetContainer && operation.targetContainer instanceof HTMLElement) {
                    operation.targetContainer.style.border = "2px solid var(--brand-primary)";
                }
            },
            dragOut(operation) {
                if (operation.targetContainer && operation.targetContainer instanceof HTMLElement) {
                    operation.targetContainer.style.border = "2px dashed #dfe0e1";
                }
            }
        };

        return {
            commonFieldTypes: ["Text", "Memo", "Single Select", "Multi Select", "Number", "Date", "Time", "Date Time", "Boolean", "Fixed Message"],
            dragSourceOptions
        };
    },

    template: `
<Panel type="block" hasFullscreen title="Workflow Form Builder" titleIconClass="fa fa-hammer">
    <template #default>
        <v-style>
            .form-builder-detail .form-template-item,
            .gu-mirror.form-template-item {
                font-size: 13px;
                font-weight: 600;
                padding: 6px;
                background-color: #ffffff;
                border: 1px solid #e1e1e1;
                border-left: 3px solid #e1e1e1;
                border-radius: 3px;
                cursor: grab;
            }

            .form-builder-detail .form-template-item.form-template-item-section,
            .gu-mirror.form-template-item.form-template-item-section {
                border-left-color: #009ce3;
            }

            .form-builder-detail .form-template-item.form-template-item-field,
            .gu-mirror.form-template-item.form-template-item-field {
                border-left-color: #ee7725;
                margin-right: 5px;
                margin-bottom: 5px;
                flex-basis: calc(50% - 5px);
            }
        </v-style>

        <div class="form-builder-detail d-flex flex-column position-absolute inset-0" style="overflow-y: hidden;">
            <div class="p-2 d-flex" style="border-bottom: 1px solid #dfe0e1;">
                <ul class="nav nav-pills" style="flex-grow: 1;">
                    <li role="presentation"><a href="#">Submissions</a></li>
                    <li class="active" role="presentation"><a href="#">Form Builder</a></li>
                    <li role="presentation"><a href="#">Communications</a></li>
                    <li role="presentation"><a href="#">Settings</a></li>
                    <li role="presentation"><a href="#">Analytics</a></li>
                </ul>

                <div>
                    <RockButton btnType="primary">Save</RockButton>
                </div>
            </div>

            <div class="d-flex" style="flex-grow: 1; overflow-y: hidden;">
                <div class="d-flex flex-column" style="background-color: #f8f9fa; width: 320px; overflow-y: hidden;">
                    <div class="p-2" style="border-right: 1px solid #dfe0e1; border-bottom: 1px solid #dfe0e1;">
                        <strong>Field List</strong>
                    </div>

                    <div class="p-2 d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
                        <div class="mt-3" v-drag-source="dragSourceOptions">
                            <div class="form-template-item form-template-item-section">
                                <i class="fa fa-expand fa-fw"></i>
                                Section
                            </div>
                        </div>

                        <div class="mt-3" style="flex-grow: 1;">
                            <div><strong>Field Types</strong></div>

                            <div class="d-flex flex-wrap" v-drag-source="dragSourceOptions">
                                <div v-for="field in commonFieldTypes" class="form-template-item form-template-item-field">
                                    <i class="fa fa-font fa-fw"></i>
                                    {{ field }}
                                </div>
                            </div>
                        </div>

                        <div class="mt-3">
                            <DropDownList label="Campus Set From" />
                            <Switch text="Enable Person Entry" />
                        </div>
                    </div>
                </div>

                <div class="p-3 d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
                    <div style="border: 1px solid #dfe0e1; padding: 20px; text-align: center;">
                        Form Header
                    </div>

                    <div class="mt-3" style="flex-grow: 1; border: 2px dashed #dfe0e1;" v-drag-target="dragSourceOptions">
                        
                    </div>

                    <div class="mt-3" style="border: 1px solid #dfe0e1; padding: 20px; text-align: center;">
                        Form Footer
                    </div>
                </div>
            </div>
        </div>
    </template>
</Panel>`
});
