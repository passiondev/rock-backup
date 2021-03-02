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
import { Field } from '../Vendor/VeeValidate/vee-validate.js';
import { asFormattedString, toNumberOrNull } from '../Filters/Number.js';
import { defineComponent, PropType } from '../Vendor/Vue/vue.js';
import RockLabel from './RockLabel.js';
import { newGuid } from '../Util/Guid.js';

export default defineComponent({
    name: 'CurrencyBox',
    components: {
        RockLabel,
        Field
    },
    props: {
        modelValue: {
            type: Number as PropType<number | null>,
            default: null
        },
        label: {
            type: String as PropType<string>,
            required: true
        },
        help: {
            type: String as PropType<string>,
            default: ''
        },
        rules: {
            type: String as PropType<string>,
            default: ''
        },
        disabled: {
            type: Boolean as PropType<boolean>,
            default: false
        }
    },
    emits: [
        'update:modelValue'
    ],
    data: function () {
        return {
            uniqueId: `rock-currencybox-${newGuid()}`,
            internalValue: ''
        };
    },
    methods: {
        onChange() {
            this.internalValue = asFormattedString(this.modelValue);
        }
    },
    computed: {
        isRequired(): boolean {
            return this.rules.includes('required');
        }
    },
    watch: {
        internalValue() {
            this.$emit('update:modelValue', toNumberOrNull(this.internalValue));
        },
        modelValue: {
            immediate: true,
            handler() {
                this.internalValue = asFormattedString(this.modelValue);
            }
        }
    },
    template: `
<Field
    v-model.lazy="internalValue"
    @change="onChange"
    :name="label"
    :rules="rules"
    #default="{field, errors}">
    <div class="form-group rock-currency-box" :class="{required: isRequired, 'has-error': Object.keys(errors).length}">
        <RockLabel :for="uniqueId" :help="help">
            {{label}}
        </RockLabel>
        <div class="input-group">
            <span class="input-group-addon">$</span>
            <input :id="uniqueId" type="text" class="form-control" v-bind="field" :disabled="disabled" />
        </div>
    </div>
</Field>`
});