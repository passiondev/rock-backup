﻿// <copyright>
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
import { newGuid } from "../Util/guid";
import { ruleStringToArray } from "../Rules/index";
import RockFormField from "./rockFormField";

export default defineComponent({
    name: "CheckBox",

    components: {
        RockFormField
    },

    props: {
        modelValue: {
            type: Boolean as PropType<boolean>,
            required: true
        },
        label: {
            type: String as PropType<string>,
            required: true
        },
        inline: {
            type: Boolean as PropType<boolean>,
            default: true
        },
        rules: {
            type: String as PropType<string>,
            default: ""
        }
    },
    data: function () {
        return {
            uniqueId: `rock-checkbox-${newGuid()}`,
            internalValue: this.modelValue
        };
    },
    methods: {
        toggle() {
            this.internalValue = !this.internalValue;
        }
    },
    computed: {
        isRequired() {
            const rules = ruleStringToArray(this.rules);
            return rules.indexOf("required") !== -1;
        }
    },
    watch: {
        modelValue() {
            this.internalValue = this.modelValue;
        },
        internalValue() {
            this.$emit("update:modelValue", this.internalValue);
        }
    },
    template: `
<div v-if="inline" class="checkbox">
    <label title="">
        <input type="checkbox" v-model="internalValue" />
        <span class="label-text ">{{label}}</span>
    </label>
</div>
<RockFormField
    v-else
    :modelValue="modelValue"
    :label="label"
    formGroupClasses="rock-check-box"
    name="checkbox">
    <template #default="{uniqueId, field, errors, disabled}">
    <div class="control-wrapper">
        <div class="rock-checkbox-icon" @click="toggle">
            <i v-if="modelValue" class="fa fa-check-square-o fa-lg"></i>
            <i v-else class="fa fa-square-o fa-lg"></i>
        </div>
    </div>
    </template>
</RockFormField>
`
});
