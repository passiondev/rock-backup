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
System.register(["vue", "../Util/Guid", "vee-validate", "./RockLabel"], function (exports_1, context_1) {
    "use strict";
    var vue_1, Guid_1, vee_validate_1, RockLabel_1;
    var __moduleName = context_1 && context_1.id;
    return {
        setters: [
            function (vue_1_1) {
                vue_1 = vue_1_1;
            },
            function (Guid_1_1) {
                Guid_1 = Guid_1_1;
            },
            function (vee_validate_1_1) {
                vee_validate_1 = vee_validate_1_1;
            },
            function (RockLabel_1_1) {
                RockLabel_1 = RockLabel_1_1;
            }
        ],
        execute: function () {
            exports_1("default", vue_1.defineComponent({
                name: 'RockFormField',
                components: {
                    Field: vee_validate_1.Field,
                    RockLabel: RockLabel_1.default
                },
                props: {
                    modelValue: {
                        required: true
                    },
                    name: {
                        type: String,
                        required: true
                    },
                    label: {
                        type: String,
                        default: ''
                    },
                    help: {
                        type: String,
                        default: ''
                    },
                    rules: {
                        type: String,
                        default: ''
                    },
                    disabled: {
                        type: Boolean,
                        default: false
                    },
                    formGroupClasses: {
                        type: String,
                        default: ''
                    },
                    validationTitle: {
                        type: String,
                        default: ''
                    }
                },
                emits: [
                    'update:modelValue'
                ],
                data: function () {
                    return {
                        uniqueId: "rock-" + this.name + "-" + Guid_1.newGuid(),
                        internalValue: this.modelValue
                    };
                },
                computed: {
                    isRequired: function () {
                        return this.rules.includes('required');
                    }
                },
                watch: {
                    internalValue: function () {
                        this.$emit('update:modelValue', this.internalValue);
                    },
                    modelValue: function () {
                        this.internalValue = this.modelValue;
                    }
                },
                template: "\n<Field v-model=\"internalValue\" :name=\"validationTitle || label\" :rules=\"rules\" #default=\"{field, errors}\">\n    <slot name=\"pre\" />\n    <div class=\"form-group\" :class=\"[formGroupClasses, isRequired ? 'required' : '', Object.keys(errors).length ? 'has-error' : '']\">\n        <RockLabel v-if=\"label || help\" :for=\"uniqueId\" :help=\"help\">\n            {{label}}\n        </RockLabel>\n        <slot v-bind=\"{uniqueId, field, errors, disabled}\" />\n    </div>\n    <slot name=\"post\" />\n</Field>"
            }));
        }
    };
});
//# sourceMappingURL=RockFormField.js.map