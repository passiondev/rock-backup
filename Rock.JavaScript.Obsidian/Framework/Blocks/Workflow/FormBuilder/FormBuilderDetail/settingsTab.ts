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

import { defineComponent, PropType, ref, watch } from "vue";
import RockForm from "../../../../Controls/rockForm";
import CompletionSettings from "./completionSettings";
import GeneralSettings from "./generalSettings";

export default defineComponent({
    name: "Workflow.FormBuilderDetail.CommunicationsTab",

    components: {
        GeneralSettings,
        CompletionSettings,
        RockForm
    },

    props: {
        modelValue: {
            type: Object as PropType<Record<string, unknown>>,
            required: true
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        const generalSettings = ref(props.modelValue.generalSettings ?? {});

        const completionSettings = ref(props.modelValue.completionSettings ?? {});

        watch(() => props.modelValue, () => {
            generalSettings.value = props.modelValue.generalSettings ?? {};
            completionSettings.value = props.modelValue.completionSettings ?? {};
        });

        watch([generalSettings, completionSettings], () => {
            const newValue: Record<string, unknown> = {
                ...props.modelValue,
                generalSettings: generalSettings.value,
                completionSettings: completionSettings.value
            };

            emit("update:modelValue", newValue);
        });

        return {
            generalSettings,
            completionSettings
        };
    },

    template: `
<div class="d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
    <div class="panel-body">
        <RockForm>
            <GeneralSettings v-model="generalSettings" />

            <CompletionSettings v-model="completionSettings" />
        </RockForm>
    </div>
</div>
`
});
