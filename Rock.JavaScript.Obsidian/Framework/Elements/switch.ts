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
import { defineComponent, PropType } from "vue";
import { useVModelPassthrough } from "../Util/component";
import InlineSwitch from "./inlineSwitch";
import RockFormField from "./rockFormField";

export default defineComponent({
    name: "CheckBox",

    components: {
        InlineSwitch,
        RockFormField
    },

    props: {
        modelValue: {
            type: Boolean as PropType<boolean>,
            required: true
        },

        text: {
            type: String as PropType<string>,
            default: ""
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        const internalValue = useVModelPassthrough(props, "modelValue", emit);

        return {
            internalValue
        };
    },

    template: `
<RockFormField
    :modelValue="internalValue"
    formGroupClasses="rock-switch"
    name="switch">
    <template #default="{uniqueId, field}">
        <div class="control-wrapper">
            <InlineSwitch v-model="internalValue" :label="text" :uniqueId="uniqueId" v-bind="field" />
        </div>
    </template>
</RockFormField>
`
});
