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

import { computed, defineComponent, PropType, ref, watch } from "vue";
import RockField from "../../../../Controls/rockField";
import { DragSource, IDragSourceOptions } from "../../../../Directives/dragDrop";
import DropDownList from "../../../../Elements/dropDownList";
import Switch from "../../../../Elements/switch";
import { toNumberOrNull } from "../../../../Services/number";
import ConfigurableZone from "./configurableZone";
import { FormFieldType, GeneralAsideSettings } from "./types";

export default defineComponent({
    name: "Workflow.FormBuilderDetail.GeneralAside",
    components: {
        ConfigurableZone,
        DropDownList,
        RockField,
        Switch
    },

    directives: {
        DragSource
    },

    props: {
        modelValue: {
            type: Object as PropType<GeneralAsideSettings>,
            required: true
        },

        fieldTypes: {
            type: Array as PropType<FormFieldType[]>,
            required: true
        },

        sectionDragOptions: {
            type: Object as PropType<IDragSourceOptions>,
            required: true
        },

        fieldDragOptions: {
            type: Object as PropType<IDragSourceOptions>,
            required: true
        }
    },

    emits: [
        "update:modelValue"
    ],

    methods: {
        /**
         * Checks if this aside is safe to close or if there are errors that
         * must be corrected first.
         */
        isSafeToClose(): boolean {
            return true;
        }
    },

    setup(props, { emit }) {
        /**
         * The value for the drop down that specifies where to set the campus
         * context from for this form.
         */
        const campusSetFrom = ref(props.modelValue.campusSetFrom?.toString() ?? "");

        /** True if the form has a person entry section. */
        const hasPersonEntry = ref(props.modelValue.hasPersonEntry ?? false);

        /** The field types to display in the common field types section. */
        const commonFieldTypes = computed((): FormFieldType[] => {
            return props.fieldTypes.filter(f => f.isCommon);
        });

        /** Used to temporarily disable emitting the modelValue when something changes. */
        let autoSyncModelValue = true;

        // Watch for changes in the model value and update our internal values.
        watch(() => props.modelValue, () => {
            autoSyncModelValue = false;
            campusSetFrom.value = props.modelValue.campusSetFrom?.toString() ?? "";
            hasPersonEntry.value = props.modelValue.hasPersonEntry ?? false;
            autoSyncModelValue = true;
        });

        // Watch for changes in our internal values and update the modelValue.
        watch([campusSetFrom, hasPersonEntry], () => {
            if (!autoSyncModelValue) {
                return;
            }

            const value: GeneralAsideSettings = {
                campusSetFrom: toNumberOrNull(campusSetFrom.value) ?? undefined,
                hasPersonEntry: hasPersonEntry.value
            };

            emit("update:modelValue", value);
        });

        return {
            campusSetFrom,
            hasPersonEntry,
            commonFieldTypes
        };
    },

    template: `
<div class="d-flex flex-column" style="overflow-y: hidden; flex-grow: 1;">
    <div class="p-2 aside-header" style="border-right: 1px solid #dfe0e1; border-bottom: 1px solid #dfe0e1;">
        <span class="title">Field List</span>
    </div>

    <div class="aside-body d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
        <div class="mt-3" v-drag-source="sectionDragOptions">
            <div class="form-template-item form-template-item-section">
                <i class="fa fa-expand fa-fw"></i>
                Section
            </div>
        </div>

        <div class="mt-3" style="flex-grow: 1;">
            <RockLabel>Field Types</RockLabel>

            <div class="d-flex flex-wrap" style="overflow-x: clip; margin-right: -5px;" v-drag-source="fieldDragOptions">
                <div v-for="field in commonFieldTypes" class="form-template-item form-template-item-field" :data-field-type="field.guid">
                    <i :class="field.icon + ' fa-fw'"></i>
                    <div class="text">{{ field.text }}</div>
                </div>
            </div>
        </div>

        <div class="mt-3">
            <DropDownList v-model="campusSetFrom" label="Campus Set From" />
            <Switch v-model="hasPersonEntry" text="Enable Person Entry" />
        </div>
    </div>
</div>
`
});
