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
import { computed, defineComponent, inject, PropType, ref, watch } from "vue";
import CheckBox from "../Elements/checkBox";
import CheckBoxList from "../Elements/checkBoxList";
import DropDownList, { DropDownListOption } from "../Elements/dropDownList";
import NumberBox from "../Elements/numberBox";
import { asBoolean, asTrueFalseOrNull } from "../Services/boolean";
import { toNumber, toNumberOrNull } from "../Services/number";
import { ListItem } from "../ViewModels";
import { ClientValue, ConfigurationValueKey, ValueItem } from "./definedValueField";
import { getFieldEditorProps } from "./utils";

function parseModelValue(modelValue: string | undefined): string {
    try {
        const clientValue = JSON.parse(modelValue ?? "") as ClientValue;

        return clientValue.value;
    }
    catch {
        return "";
    }
}

function getClientValue(value: string | string[], valueOptions: ValueItem[]): ClientValue {
    const values = Array.isArray(value) ? value : [value];
    const selectedValues = valueOptions.filter(v => values.includes(v.value));

    if (selectedValues.length >= 1) {
        return {
            value: selectedValues.map(v => v.value).join(","),
            text: selectedValues.map(v => v.text).join(", "),
            description: selectedValues.map(v => v.description).join(", ")
        };
    }
    else {
        return {
            value: "",
            text: "",
            description: ""
        };
    }
}

export const EditComponent = defineComponent({
    name: "DefinedValueField.Edit",

    components: {
        DropDownList,
        CheckBoxList
    },

    props: getFieldEditorProps(),

    setup(props, { emit }) {
        const internalValue = ref(parseModelValue(props.modelValue));
        const internalValues = ref(parseModelValue(props.modelValue).split(",").filter(v => v !== ""));

        const valueOptions = computed((): ValueItem[] => {
            try {
                return JSON.parse(props.configurationValues[ConfigurationValueKey.Values] ?? "[]") as ValueItem[];
            }
            catch {
                return [];
            }
        });

        const displayDescription = computed((): boolean => asBoolean(props.configurationValues[ConfigurationValueKey.DisplayDescription]));

        /** The options to choose from in the drop down list */
        const options = computed((): DropDownListOption[] => {
            const providedOptions: DropDownListOption[] = valueOptions.value.map(v => {
                return {
                    text: displayDescription.value ? v.description : v.text,
                    value: v.value
                };
            });

            return providedOptions;
        });

        /** The options to choose from in the drop down list */
        const optionsMultiple = computed((): ListItem[] => {
            return valueOptions.value.map(v => {
                return {
                    text: displayDescription.value ? v.description : v.text,
                    value: v.value
                } as ListItem;
            });
        });

        const isMultiple = computed((): boolean => asBoolean(props.configurationValues[ConfigurationValueKey.AllowMultiple]));

        const configAttributes = computed((): Record<string, unknown> => {
            const attributes: Record<string, unknown> = {};

            const enhancedConfig = props.configurationValues[ConfigurationValueKey.EnhancedSelection];
            if (enhancedConfig) {
                attributes.enhanceForLongLists = asBoolean(enhancedConfig);
            }

            return attributes;
        });

        /** The number of columns wide the checkbox list will be. */
        const repeatColumns = computed((): number => toNumber(props.configurationValues[ConfigurationValueKey.RepeatColumns]));

        watch(() => props.modelValue, () => {
            internalValue.value = parseModelValue(props.modelValue);
            internalValues.value = parseModelValue(props.modelValue).split(",").filter(v => v !== "");
        });

        watch(() => internalValue.value, () => {
            if (!isMultiple.value) {
                const clientValue = getClientValue(internalValue.value, valueOptions.value);

                emit("update:modelValue", JSON.stringify(clientValue));
            }
        });

        watch(() => internalValues.value, () => {
            if (isMultiple.value) {
                const clientValue = getClientValue(internalValues.value, valueOptions.value);

                emit("update:modelValue", JSON.stringify(clientValue));
            }
        });

        return {
            configAttributes,
            internalValue,
            internalValues,
            isMultiple,
            isRequired: inject("isRequired") as boolean,
            options,
            optionsMultiple,
            repeatColumns
        };
    },

    template: `
<DropDownList v-if="!isMultiple" v-model="internalValue" v-bind="configAttributes" :options="options" :showBlankItem="!isRequired" />
<CheckBoxList v-else v-model="internalValues" :options="optionsMultiple" horizontal :repeatColumns="repeatColumns" />
`
});

const enum ConfigurationPropertyKey {
    DefinedTypes = "definedTypes",
    DefinedValues = "definedValues"
}

export const ConfigurationComponent = defineComponent({
    name: "DefinedValueField.Configuration",

    components: {
        DropDownList,
        CheckBoxList,
        CheckBox,
        NumberBox
    },

    props: {
        modelValue: {
            type: Object as PropType<Record<string, string>>,
            required: true
        },
        configurationProperties: {
            type: Object as PropType<Record<string, string>>,
            required: true
        }
    },

    setup(props, { emit }) {
        const definedTypeValue = ref(props.modelValue.definedtype ?? "");
        const allowMultipleValues = ref(asBoolean(props.modelValue[ConfigurationValueKey.AllowMultiple]));
        const displayDescriptions = ref(asBoolean(props.modelValue[ConfigurationValueKey.DisplayDescription]));
        const enhanceForLongLists = ref(asBoolean(props.modelValue[ConfigurationValueKey.EnhancedSelection]));
        const includeInactive = ref(asBoolean(props.modelValue[ConfigurationValueKey.IncludeInactive]));
        const repeatColumns = ref<number | null>(toNumberOrNull(props.modelValue[ConfigurationValueKey.RepeatColumns]));
        const selectableValues = ref<string[]>((props.modelValue["selectableValues"] ?? "").split(",").filter(s => s !== ""));
        const definedTypeItems = ref<ListItem[]>([]);
        const definedValueItems = ref<ListItem[]>([]);

        const definedTypeOptions = computed((): DropDownListOption[] => {
            return definedTypeItems.value.map((item): DropDownListOption => {
                return {
                    text: item.text,
                    value: item.value
                };
            });
        });

        const definedValueOptions = computed((): ListItem[] => definedValueItems.value);

        const hasValues = computed((): boolean => {
            return definedValueItems.value.length > 0;
        });

        watch(() => [props.modelValue, props.configurationProperties], () => {
            const definedTypes = props.configurationProperties[ConfigurationPropertyKey.DefinedTypes];
            const definedValues = props.configurationProperties[ConfigurationPropertyKey.DefinedValues];

            definedTypeItems.value = definedTypes ? JSON.parse(props.configurationProperties.definedTypes) as ListItem[] : [];
            definedValueItems.value = definedValues ? JSON.parse(props.configurationProperties.definedValues) as ListItem[] : [];

            definedTypeValue.value = props.modelValue.definedtype;
            selectableValues.value = (props.modelValue.selectableValues?.split(",") ?? []).filter(s => s !== "");
        }, {
            immediate: true
        });

        const maybeUpdateModelValue = (): boolean => {
            const newValue = {
                definedtype: definedTypeValue.value,
                selectableValues: selectableValues.value.join(","),
                allowmultiple: asTrueFalseOrNull(allowMultipleValues.value) ?? "False",
                displaydescription: asTrueFalseOrNull(displayDescriptions.value) ?? "False",
                enhancedselection: asTrueFalseOrNull(enhanceForLongLists.value) ?? "False",
                includeInactive: asTrueFalseOrNull(includeInactive.value) ?? "False",
                RepeatColumns: repeatColumns.value?.toString() ?? ""
            };

            const anyValueChanged = newValue.definedtype !== props.modelValue.definedtype
                || newValue.selectableValues !== (props.modelValue.selectableValues ?? "")
                || newValue.allowmultiple !== (props.modelValue.allowmultiple ?? "False")
                || newValue.displaydescription !== (props.modelValue.displaydescription ?? "False")
                || newValue.enhancedselection !== (props.modelValue.enhancedselection ?? "False")
                || newValue.includeInactive !== (props.modelValue.includeInactive ?? "False")
                || newValue.RepeatColumns !== (props.modelValue.RepeatColumns ?? "");

            if (anyValueChanged) {
                emit("update:modelValue", newValue);
                return true;
            }
            else {
                return false;
            }
        };

        watch([definedTypeValue, selectableValues, displayDescriptions, includeInactive], () => {
            if (maybeUpdateModelValue()) {
                emit("updateConfiguration");
            }
        });

        const maybeUpdateConfiguration = (key: string, value: string): void => {
            if (maybeUpdateModelValue()) {
                emit("updateConfigurationValue", key, value);
            }
        };

        watch(allowMultipleValues, () => maybeUpdateConfiguration("allowmultiple", asTrueFalseOrNull(allowMultipleValues.value) ?? "False"));
        watch(enhanceForLongLists, () => maybeUpdateConfiguration("enhancedselection", asTrueFalseOrNull(enhanceForLongLists.value) ?? "False"));
        watch(repeatColumns, () => maybeUpdateConfiguration("RepeatColumns", repeatColumns.value?.toString() ?? ""));

        return {
            allowMultipleValues,
            definedTypeValue,
            definedTypeOptions,
            definedValueOptions,
            displayDescriptions,
            enhanceForLongLists,
            hasValues,
            includeInactive,
            repeatColumns,
            selectableValues
        };
    },

    template: `
<div>
    <DropDownList v-model="definedTypeValue" label="Defined Type" :options="definedTypeOptions" :showBlankItem="false" />
    <CheckBox v-model="allowMultipleValues" label="Allow Multiple Values" text="Yes" help="When set, allows multiple defined type values to be selected." />
    <CheckBox v-model="displayDescriptions" label="Display Descriptions" text="Yes" help="When set, the defined value descriptions will be displayed instead of the values." />
    <CheckBox v-model="enhanceForLongLists" label="Enhance For Long Lists" text="Yes" />
    <CheckBox v-model="includeInactive" label="Include Inactive" text="Yes" />
    <NumberBox v-model="repeatColumns" label="Repeat Columns" />
    <CheckBoxList v-if="hasValues" v-model="selectableValues" label="Selectable Values" :options="definedValueOptions" :horizontal="true" />
</div>
`
});
