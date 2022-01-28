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

import { defineComponent, PropType, Ref, ref, watch } from "vue";
import CategoryPicker from "../../../Controls/categoryPicker";
import DateTimePicker from "../../../Elements/dateTimePicker";
import DropDownList from "../../../Elements/dropDownList";
import InlineSwitch from "../../../Elements/inlineSwitch";
import TextBox from "../../../Elements/textBox";
import TransitionVerticalCollapse from "../../../Elements/transitionVerticalCollapse";
import { EntityType } from "../../../SystemGuids";
import { ListItem } from "../../../ViewModels";
import EmailSource from "./emailSource";
import SettingsWell from "./settingsWell";
import { FormGeneralSettings } from "./types";

/**
 * Compares two values for equality by performing deep nested comparisons
 * if the values are objects or arrays.
 *
 * This function should be moved to the util namespace eventually.
 * 
 * @param a The first value to compare.
 * @param b The second value to compare.
 * @param strict True if strict comparision is required (meaning 1 would not equal "1").
 *
 * @returns True if the two values are equal to each other.
 */
function deepEqual(a: unknown, b: unknown, strict: boolean): boolean {
    // Catches everything but objects.
    if (strict && a === b) {
        return true;
    }
    else if (!strict && a == b) {
        return true;
    }

    // NaN never equals another NaN, but functionally they are the same.
    if (typeof a === "number" && typeof b === "number" && isNaN(a) && isNaN(b)) {
        return true;
    }

    // Remaining value types must both be of type object
    if (a && b && typeof a === "object" && typeof b === "object") {
        // Array status must match.
        if (Array.isArray(a) !== Array.isArray(b)) {
            return false;
        }

        if (Array.isArray(a) && Array.isArray(b)) {
            // Array lengths must match.
            if (a.length !== b.length) {
                return false;
            }

            // Each element in the array must match.
            for (let i = 0; i < a.length; i++) {
                if (!deepEqual(a[i], b[i], strict)) {
                    return false;
                }
            }

            return true;
        }
        else {
            // NOTE: There are a few edge cases not accounted for here, but they
            // are rare and unusual:
            // Map, Set, ArrayBuffer, RegExp

            // The objects must be of the same "object type".
            if (a.constructor !== b.constructor) {
                return false;
            }

            // Get all the key/value pairs of each object and sort them so they
            // are in the same order as each other.
            const aEntries = Object.entries(a).sort((a, b) => a[0] < b[0] ? -1 : (a[0] > b[0] ? 1 : 0));
            const bEntries = Object.entries(b).sort((a, b) => a[0] < b[0] ? -1 : (a[0] > b[0] ? 1 : 0));

            // Key/value count must be identical.
            if (aEntries.length !== bEntries.length) {
                return false;
            }

            for (let i = 0; i < aEntries.length; i++) {
                const aEntry = aEntries[i];
                const bEntry = bEntries[i];

                // Ensure the keys are equal, must always be strict.
                if (!deepEqual(aEntry[0], bEntry[0], true)) {
                    return false;
                }

                // Ensure the values are equal.
                if (!deepEqual(aEntry[1], bEntry[1], strict)) {
                    return false;
                }
            }

            return true;
        }
    }

    return false;
}

/**
 * Updates the Ref value, but only if the new value is actually different than
 * the current value. A strict comparison is performed.
 * 
 * @param target The target Ref object to be updated.
 * @param value The new value to be assigned to the target.
 *
 * @returns True if the target was updated, otherwise false.
 */
function updateRefValue<T>(target: Ref<T>, value: T): boolean {
    if (deepEqual(target.value, value, true)) {
        return false;
    }

    target.value = value;

    return true;
}

/**
 * Displays the UI for the General Settings section of the Settings screen.
 */
export default defineComponent({
    name: "Workflow.FormBuilderDetail.GeneralSettings",

    components: {
        CategoryPicker,
        DateTimePicker,
        DropDownList,
        EmailSource,
        InlineSwitch,
        SettingsWell,
        TextBox,
        TransitionVerticalCollapse
    },

    props: {
        modelValue: {
            type: Object as PropType<FormGeneralSettings>,
            required: true
        },

        templateOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        /** The name of the form. */
        const name = ref(props.modelValue.name ?? "");

        /** The text that describes the forms purpose. */
        const description = ref(props.modelValue.description ?? "");

        /** The form template to use to provide default values. */
        const template = ref(props.modelValue.template ?? "");

        /** The category this form is attached to. */
        const category = ref(props.modelValue.category ? [props.modelValue.category] : []);

        /** The date and time the form beings to allow entries. */
        const entryStarts = ref(props.modelValue.entryStarts ?? "");

        /** The date and time after which the form no longer allows entries. */
        const entryEnds = ref(props.modelValue.entryEnds ?? "");

        // Watch for changes in our modelValue and then update all our internal values.
        watch(() => props.modelValue, () => {
            updateRefValue(name, props.modelValue.name ?? "");
            updateRefValue(description, props.modelValue.description ?? "");
            updateRefValue(template, props.modelValue.template ?? "");
            updateRefValue(category, props.modelValue.category ? [props.modelValue.category] : []);
            updateRefValue(entryStarts, props.modelValue.entryStarts ?? "");
            updateRefValue(entryEnds, props.modelValue.entryEnds ?? "");
        });

        // Watch for changes on any of our internal values and then update the modelValue.
        watch([name, description, template, category, entryStarts, entryEnds], (a, b) => {
            const newValue: FormGeneralSettings = {
                ...props.modelValue,
                name: name.value,
                description: description.value,
                template: template.value,
                category: category.value.length > 0 ? category.value[0] : null,
                entryStarts: entryStarts.value,
                entryEnds: entryEnds.value,
            };

            emit("update:modelValue", newValue);
        });

        return {
            category,
            description,
            entryStarts,
            entryEnds,
            name,
            template,
            workflowTypeEntityTypeGuid: EntityType.WorkflowType
        };
    },

    template: `
<SettingsWell title="General Settings"
    description="Update the general settings for the form below.">
    <div class="row">
        <div class="col-md-6">
            <div>
                <TextBox v-model="name"
                    label="Form Name"
                    rules="required" />

                <TextBox v-model="description"
                    label="Description"
                    textMode="multiline" />

                <DropDownList v-model="template"
                    label="Template"
                    :options="templateOptions"
                    />

                <CategoryPicker v-model="category"
                    label="Category"
                    rules="required"
                    :entityTypeGuid="workflowTypeEntityTypeGuid" />
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <DateTimePicker v-model="entryStarts"
                label="Form Entry Starts" />
        </div>

        <div class="col-md-6">
            <DateTimePicker v-model="entryEnds"
                label="Form Entry Ends" />
        </div>
    </div>
</Settingswell>
`
});
