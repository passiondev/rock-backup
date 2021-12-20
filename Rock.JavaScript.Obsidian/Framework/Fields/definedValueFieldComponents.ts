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
import { getFieldEditorProps } from "./utils";
import CheckBoxList from "../Elements/checkBoxList";
import DropDownList, { DropDownListOption } from "../Elements/dropDownList";
import CheckBox from "../Elements/checkBox";
import NumberBox from "../Elements/numberBox";
import { asBoolean } from "../Services/boolean";
import { ClientValue, ConfigurationValueKey, ValueItem } from "./definedValueField";
import { ListItem } from "../ViewModels";
import { toNumber } from "../Services/number";
import { asTrueFalseOrNull } from "../Services/boolean";

/**
 * The field configuration values that */
type AttributeConfigurationValues = {
    definedTypes?: ListItem[] | null;

    definedValues?: ListItem[] | null;
};

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
        const definedTypeValue = ref("");
        const allowMultipleValues = ref(false);
        const displayDescriptions = ref(false);
        const enhanceForLongLists = ref(false);
        const includeInactive = ref(false);
        const repeatColumns = ref<number | null>(null);
        const selectableValues = ref<string[]>([]);
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
                allowmultiple: asTrueFalseOrNull(allowMultipleValues.value),
                displaydescription: asTrueFalseOrNull(displayDescriptions.value),
                enhancedselection: asTrueFalseOrNull(enhanceForLongLists.value),
                includeInactive: asTrueFalseOrNull(includeInactive.value),
                RepeatColumns: repeatColumns.value?.toString() ?? ""
            };

            const anyValueChanged = newValue.definedtype !== props.modelValue.definedtype
                || newValue.selectableValues !== props.modelValue.selectableValues
                || newValue.allowmultiple !== props.modelValue.allowmultiple
                || newValue.displaydescription !== props.modelValue.displaydescription
                || newValue.enhancedselection !== props.modelValue.enhancedselection
                || newValue.includeInactive !== props.modelValue.includeInactive
                || newValue.RepeatColumns !== props.modelValue.RepeatColumns;

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
    <CheckBox v-model="allowMultipleValues" label="Allow Multiple Values" help="When set, allows multiple defined type values to be selected." />
    <CheckBox v-model="displayDescriptions" label="Display Descriptions" help="When set, the defined value descriptions will be displayed instead of the values." />
    <CheckBox v-model="enhanceForLongLists" label="Enhance For Long Lists" />
    <CheckBox v-model="includeInactive" label="Include Inactive" />
    <NumberBox v-model="repeatColumns" label="Repeat Columns" />
    <CheckBoxList v-if="hasValues" v-model="selectableValues" label="Selectable Values" :options="definedValueOptions" :horizontal="true" />
</div>
`
});
