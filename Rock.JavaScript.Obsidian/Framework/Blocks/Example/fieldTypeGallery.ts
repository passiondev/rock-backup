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

import PaneledBlockTemplate from "../../Templates/paneledBlockTemplate";
import { Component, computed, defineComponent, nextTick, PropType, reactive, ref, watch } from "vue";
import PanelWidget from "../../Elements/panelWidget";
import AttributeValuesContainer from "../../Controls/attributeValuesContainer";
import { Guid } from "../../Util/guid";
import TextBox from "../../Elements/textBox";
import { FieldType as FieldTypeGuids } from "../../SystemGuids";
import { ClientEditableAttributeValue, ListItem } from "../../ViewModels";
import { DefinedValueFieldType } from "../../Fields/definedValueField";
import { useInvokeBlockAction } from "../../Util/block";
import DropDownList, { DropDownListOption } from "../../Elements/dropDownList";
import CheckBoxList from "../../Elements/checkBoxList";
import CheckBox from "../../Elements/checkBox";
import RockField from "../../Controls/rockField";
import NumberBox from "../../Elements/numberBox";
import Alert from "../../Elements/alert";
import { asTrueFalseOrNull } from "../../Services/boolean";

/**
 * Convert a simpler set of parameters into AttributeValueData
 * @param name
 * @param fieldTypeGuid
 * @param configValues
 */
const getAttributeValueData = (name: string, initialValue: string, fieldTypeGuid: Guid, configValues: Record<string, string>): Array<ClientEditableAttributeValue> => {
    const configurationValues = configValues;

    return [reactive({
            fieldTypeGuid: fieldTypeGuid,
            name: `${name} 1`,
            key: name,
            description: `This is the description of the ${name} without an initial value`,
            configurationValues,
            isRequired: false,
            textValue: "",
            value: "",
            attributeGuid: "",
            order: 0,
            categories: []
        }),
        reactive({
            fieldTypeGuid: fieldTypeGuid,
            name: `${name} 2`,
            key: name,
            description: `This is the description of the ${name} with an initial value`,
            configurationValues,
            isRequired: false,
            textValue: initialValue,
            value: initialValue,
            attributeGuid: "",
            order: 0,
            categories: []
        })
    ];
};

/** An inner component that describes the template used for each of the controls
 *  within this field type gallery */
const galleryAndResult = defineComponent({
    name: "GalleryAndResult",
    components: {
        PanelWidget,
        AttributeValuesContainer
    },
    props: {
        title: {
            type: String as PropType<string>,
            required: true
        },
        attributeValues: {
            type: Array as PropType<ClientEditableAttributeValue[]>,
            required: true
        }
    },
    computed: {
        value1Json(): string {
            return this.attributeValues[0].value ?? "";
        },
        value2Json(): string {
            return this.attributeValues[1].value ?? "";
        }
    },
    template: `
<PanelWidget>
    <template #header>{{title}}</template>
    <div class="row">
        <div class="col-md-6">
            <h4>Qualifier Values</h4>
            <slot />
            <hr />
            <h4>Attribute Values Container (edit)</h4>
            <AttributeValuesContainer :attributeValues="attributeValues" :isEditMode="true" />
        </div>
        <div class="col-md-6">
            <h4>Attribute Values Container (view)</h4>
            <AttributeValuesContainer :attributeValues="attributeValues" :isEditMode="false" />
            <hr />
            <h4>Values</h4>
            <p>
                <strong>Value 1</strong>
                <pre>{{value1Json}}</pre>
            </p>
            <p>
                <strong>Value 2</strong>
                <pre>{{value2Json}}</pre>
            </p>
        </div>
    </div>
</PanelWidget>`
});

/**
 * Generate a gallery component for a specific field type
 * @param name
 * @param fieldTypeGuid
 * @param configValues
 */
const getFieldTypeGalleryComponent = (name: string, initialValue: string, fieldTypeGuid: Guid, initialConfigValues: Record<string, string>): Component => {
    return defineComponent({
        name: `${name}Gallery`,
        components: {
            GalleryAndResult: galleryAndResult,
            TextBox
        },
        data () {
            return {
                name,
                configValues: { ...initialConfigValues } as Record<string, string>,
                attributeValues: getAttributeValueData(name, initialValue, fieldTypeGuid, initialConfigValues)
            };
        },
        computed: {
            configKeys(): string[] {
                const keys: string[] = [];

                for (const attributeValue of this.attributeValues) {
                    for (const key in attributeValue.configurationValues) {
                        if (keys.indexOf(key) === -1) {
                            keys.push(key);
                        }
                    }
                }

                return keys;
            }
        },
        watch: {
            configValues: {
                deep: true,
                handler() {
                    for (const attributeValue of this.attributeValues) {
                        for (const key in attributeValue.configurationValues) {
                            const value = this.configValues[key] || "";
                            attributeValue.configurationValues[key] = value;
                        }
                    }
                }
            }
        },
        template: `
<GalleryAndResult :title="name" :attributeValues="attributeValues">
    <TextBox v-for="configKey in configKeys" :key="configKey" :label="configKey" v-model="configValues[configKey]" />
</GalleryAndResult>`
    });
};

const galleryComponents: Record<string, Component> = {
    AddressGallery: getFieldTypeGalleryComponent("Address", '{"street1": "3120 W Cholla St", "city": "Phoenix", "state": "AZ", "postalCode": "85029-4113", "country": "US"}', FieldTypeGuids.Address, {
    }),

    BooleanGallery: getFieldTypeGalleryComponent("Boolean", "t", FieldTypeGuids.Boolean, {
        truetext: "This is true",
        falsetext: "This is false",
        BooleanControlType: "2"
    }),

    CampusGallery: getFieldTypeGalleryComponent("Campus", "", FieldTypeGuids.Campus, {
        values: JSON.stringify([
            { value: "069D4509-398A-4E08-8225-A0658E8A51E8", text: "Main Campus" },
            { value: "0D8B2F85-5DC2-406E-8A7D-D435F3153C58", text: "Secondary Campus" },
            { value: "8C99160C-D0FC-49E4-AA9D-87EAE7297AF1", text: "Tertiary Campus" }
        ] as ListItem[])
    }),

    CampusesGallery: getFieldTypeGalleryComponent("Campuses", "", FieldTypeGuids.Campuses, {
        repeatColumns: "4",
        values: JSON.stringify([
            { value: "069D4509-398A-4E08-8225-A0658E8A51E8", text: "Main Campus" },
            { value: "0D8B2F85-5DC2-406E-8A7D-D435F3153C58", text: "Secondary Campus" },
            { value: "8C99160C-D0FC-49E4-AA9D-87EAE7297AF1", text: "Tertiary Campus" }
        ] as ListItem[])
    }),

    ColorGallery: getFieldTypeGalleryComponent("Color", "#ee7725", FieldTypeGuids.Color, {
        selectiontype: "Color Picker"
    }),

    CurrencyGallery: getFieldTypeGalleryComponent("Currency", "4.70", FieldTypeGuids.Currency, {
    }),

    DateGallery: getFieldTypeGalleryComponent("Date", "2009-02-11", FieldTypeGuids.Date, {
        format: "MMM yyyy",
        displayDiff: "true",
        displayCurrentOption: "true",
        datePickerControlType: "Date Parts Picker",
        futureYearCount: "2"
    }),

    DateRangeGallery: getFieldTypeGalleryComponent("DateRange", "2021-07-25T00:00:00.0000000,2021-07-29T00:00:00.0000000", FieldTypeGuids.DateRange, {
    }),

    DateTimeGallery: getFieldTypeGalleryComponent("DateTime", "2009-02-11T14:23:00", FieldTypeGuids.DateTime, {
        format: "MMM dd, yyyy h:mm tt",
        displayDiff: "false",
        displayCurrentOption: "true",
    }),

    DayOfWeekGallery: getFieldTypeGalleryComponent("DayOfWeek", "2", FieldTypeGuids.DayOfWeek, {
    }),

    DaysOfWeekGallery: getFieldTypeGalleryComponent("DaysOfWeek", "2,5", FieldTypeGuids.DaysOfWeek, {
    }),

    DecimalGallery: getFieldTypeGalleryComponent("Decimal", "18.283", FieldTypeGuids.Decimal, {
    }),

    DecimalRangeGallery: getFieldTypeGalleryComponent("DecimalRange", "18.283,100", FieldTypeGuids.DecimalRange, {
    }),

    DefinedValueGallery: getFieldTypeGalleryComponent("DefinedValue", '{ "value": "F19FC180-FE8F-4B72-A59C-8013E3B0EB0D", "text": "Single", "description": "Used when the individual is single." }', FieldTypeGuids.DefinedValue, {
        values: JSON.stringify([
            { value: "5FE5A540-7D9F-433E-B47E-4229D1472248", text: "Married", description: "Used when an individual is married." },
            { value: "F19FC180-FE8F-4B72-A59C-8013E3B0EB0D", text: "Single", description: "Used when the individual is single." },
            { value: "3B689240-24C2-434B-A7B9-A4A6CBA7928C", text: "Divorced", description: "Used when the individual is divorced." },
            { value: "AE5A0228-9910-4505-B3C6-E6C98BEE2E7F", text: "Unknown", description: "" }
        ]),
        allowmultiple: "",
        displaydescription: "true",
        enhancedselection: "",
        includeInactive: "",
        AllowAddingNewValues: "",
        RepeatColumns: ""
    }),

    DefinedValueRangeGallery: getFieldTypeGalleryComponent("DefinedValueRange", '{ "value": "F19FC180-FE8F-4B72-A59C-8013E3B0EB0D,3B689240-24C2-434B-A7B9-A4A6CBA7928C", "text": "Single to Divorced", "description": "Used when the individual is single. to Used when the individual is divorced." }', FieldTypeGuids.DefinedValueRange, {
        values: JSON.stringify([
            { value: "5FE5A540-7D9F-433E-B47E-4229D1472248", text: "Married", description: "Used when an individual is married." },
            { value: "F19FC180-FE8F-4B72-A59C-8013E3B0EB0D", text: "Single", description: "Used when the individual is single." },
            { value: "3B689240-24C2-434B-A7B9-A4A6CBA7928C", text: "Divorced", description: "Used when the individual is divorced." },
            { value: "AE5A0228-9910-4505-B3C6-E6C98BEE2E7F", text: "Unknown", description: "" }
        ]),
        displaydescription: "false"
    }),

    EmailGallery: getFieldTypeGalleryComponent("Email", "ted@rocksolidchurchdemo.com", FieldTypeGuids.Email, {
    }),

    GenderGallery: getFieldTypeGalleryComponent("Gender", "2", FieldTypeGuids.Gender, {
    }),

    IntegerGallery: getFieldTypeGalleryComponent("Integer", "20", FieldTypeGuids.Integer, {
    }),

    IntegerRangeGallery: getFieldTypeGalleryComponent("IntegerRange", "0,100", FieldTypeGuids.IntegerRange, {
    }),

    KeyValueListGallery: getFieldTypeGalleryComponent("KeyValueList", `[{"key":"One","value":"Two"},{"key":"Three","value":"Four"}]`, FieldTypeGuids.KeyValueList, {
        keyprompt: "Enter Key",
        valueprompt: "Enter Value",
        displayvaluefirst: "false",
        allowhtml: "false",
        values: JSON.stringify([])
    }),

    MemoGallery: getFieldTypeGalleryComponent("Memo", "This is a memo", FieldTypeGuids.Memo, {
        numberofrows: "10",
        maxcharacters: "100",
        showcountdown: "true",
        allowhtml: "true"
    }),

    MonthDayGallery: getFieldTypeGalleryComponent("MonthDay", "7/4", FieldTypeGuids.MonthDay, {
    }),

    MultiSelectGallery: getFieldTypeGalleryComponent("MultiSelect", "pizza", FieldTypeGuids.MultiSelect, {
        repeatColumns: "4",
        repeatDirection: "Horizontal",
        enhancedselection: "false",
        values: '[{"value": "pizza", "text": "Pizza"}, {"value": "sub", "text": "Sub"}, {"value": "bagel", "text": "Bagel"}]'
    }),

    PhoneNumberGallery: getFieldTypeGalleryComponent("PhoneNumber", "(321) 456-7890", FieldTypeGuids.PhoneNumber, {
    }),

    RatingGallery: getFieldTypeGalleryComponent("Rating", '{"value":3,"maxValue":5}', FieldTypeGuids.Rating, {
        max: "5"
    }),

    SingleSelectGallery: getFieldTypeGalleryComponent("SingleSelect", "pizza", FieldTypeGuids.SingleSelect, {
        repeatColumns: "4",
        fieldtype: "rb",
        values: '[{"value": "pizza", "text": "Pizza"}, {"value": "sub", "text": "Sub"}, {"value": "bagel", "text": "Bagel"}]'
    }),

    SSNGallery: getFieldTypeGalleryComponent("SSN", "123-45-6789", FieldTypeGuids.Ssn, {
    }),

    TextGallery: getFieldTypeGalleryComponent("Text", "Hello", FieldTypeGuids.Text, {
        ispassword: "false",
        maxcharacters: "10",
        showcountdown: "true"
    }),

    TimeGallery: getFieldTypeGalleryComponent("Time", "13:15:00", FieldTypeGuids.Time, {
    }),
};

const galleryTemplate: string = Object.keys(galleryComponents).sort().map(g => `<${g} />`).join("");

const definedValueField = new DefinedValueFieldType();

type AttributeDesignerViewModel = {
    fieldTypeGuid?: Guid | null;

    designConfigurationValues?: Record<string, string> | null;

    designValues?: Record<string, string> | null;

    defaultValue?: string | null;

    editableValue?: ClientEditableAttributeValue | null;
};

type DesignConfigurationValues = {
    definedTypes?: ListItem[] | null;

    definedValues?: ListItem[] | null;
};

const attributeDesigner = defineComponent({
    name: "Example.AttributeDesigner",

    components: {
        Alert,
        DropDownList,
        RockField
    },

    setup() {
        const invokeBlockAction = useInvokeBlockAction();
        const fieldTypeValue = ref("");
        const defaultValue = ref<ClientEditableAttributeValue | null>(null);
        const hasDefaultControl = computed((): boolean => showDesignerControl.value && defaultValue.value ? true : false);
        const designConfigurationValues = ref<Record<string, string>>({});
        const designValues = ref<Record<string, string>>({});
        const isReady = ref(false);
        const fieldErrorMessage = ref("");

        const fieldTypeOptions = [
            {
                text: "Defined Value",
                value: FieldTypeGuids.DefinedValue
            }
        ] as DropDownListOption[];

        const designerControl = computed((): Component | null => {
            if (fieldTypeValue.value === FieldTypeGuids.DefinedValue) {
                return definedValueDesigner;
            }
            return null;
        });

        const showDesignerControl = computed((): boolean => {
            return designerControl && isReady.value;
        });



        const updateDesignValues = (): void => {
            invokeBlockAction<AttributeDesignerViewModel>("GetClientDesignValues", {
                designer: {
                    fieldTypeGuid: fieldTypeValue.value,
                    designValues: designValues.value,
                    defaultValue: ""
                } as AttributeDesignerViewModel
            }).then(result => {
                if (result.isSuccess && result.data && result.data.designConfigurationValues && result.data.designValues && result.data.editableValue) {
                    fieldErrorMessage.value = "";
                    isReady.value = true;
                    designConfigurationValues.value = result.data.designConfigurationValues;
                    designValues.value = result.data.designValues;
                    defaultValue.value = result.data.editableValue;
                }
                else {
                    fieldErrorMessage.value = result.errorMessage ?? "Encountered unknown error communicating with server.";
                }
            });
        };

        watch(fieldTypeValue, () => {
            updateDesignValues();
        });

        const onPostBack = (): void => {
            console.log("postback", designValues.value);
            updateDesignValues();
        };

        const onUpdateConfiguration = (key: string, value: string): void => {
            console.log("updateControl", designValues.value);
            if (defaultValue.value?.configurationValues) {
                defaultValue.value.configurationValues[key] = value;
            }
        };

        return {
            designerControl,
            designValues,
            designConfigurationValues,
            defaultValue,
            hasDefaultControl,
            fieldErrorMessage,
            fieldTypeOptions,
            fieldTypeValue,
            onPostBack,
            onUpdateConfiguration,
            showDesignerControl
        };
    },

    template: `
<div>
    <DropDownList label="Field Type" v-model="fieldTypeValue" :options="fieldTypeOptions" rules="required" />
    <Alert v-if="fieldErrorMessage" alertType="warning">
        {{ fieldErrorMessage }}
    </Alert>
    <component v-if="showDesignerControl" :is="designerControl" v-model="designValues" :designConfigurationValues="designConfigurationValues" @postback="onPostBack" @updateConfiguration="onUpdateConfiguration" />
    <RockField v-if="hasDefaultControl" :attributeValue="defaultValue" isEditMode />
</div>
`
});

const definedValueDesigner = defineComponent({
    name: "Example.DefinedValueDesigner",

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
        designConfigurationValues: {
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
        const designConfigurationValues = ref<DesignConfigurationValues>({});

        const definedTypeOptions = computed((): DropDownListOption[] => {
            if (!designConfigurationValues.value.definedTypes) {
                return [];
            }

            return designConfigurationValues.value.definedTypes.map((item): DropDownListOption => {
                return {
                    text: item.text,
                    value: item.value
                };
            });
        });

        const definedValueOptions = computed((): ListItem[] => designConfigurationValues.value?.definedValues ?? []);


        const hasValues = computed((): boolean => {
            return (designConfigurationValues.value?.definedValues?.length ?? 0) > 0;
        });

        watch(() => [props.modelValue, props.designConfigurationValues], () => {
            const definedTypes = props.designConfigurationValues.definedTypes ? JSON.parse(props.designConfigurationValues.definedTypes) as ListItem[] : [];
            const definedValues = props.designConfigurationValues.definedValues ? JSON.parse(props.designConfigurationValues.definedValues) as ListItem[] : [];

            designConfigurationValues.value = {
                definedTypes,
                definedValues
            };

            definedTypeValue.value = props.modelValue.definedtype;
            selectableValues.value = (props.modelValue.selectableValues?.split(",") ?? []).filter(s => s !== "");
        }, {
            immediate: true
        });

        const updateDesignValues = (): boolean => {
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
            if (updateDesignValues()) {
                emit("postback");
            }
        });

        const maybeUpdateConfiguration = (key: string, value: string): void => {
            if (updateDesignValues()) {
                emit("updateConfiguration", key, value);
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
    <CheckBox v-model="allowMultipleValues" label="Allow Multiple Values" help="When set, allows multiple defined type values to be selected." :inline="false" />
    <CheckBox v-model="displayDescriptions" label="Display Descriptions" help="When set, the defined value descriptions will be displayed instead of the values." :inline="false" />
    <CheckBox v-model="enhanceForLongLists" label="Enhance For Long Lists" :inline="false" />
    <CheckBox v-model="includeInactive" label="Include Inactive" :inline="false" />
    <NumberBox v-model="repeatColumns" label="Repeat Columns" />
    <CheckBoxList v-if="hasValues" v-model="selectableValues" label="Selectable Values" :options="definedValueOptions" :horizontal="true" />
</div>
`
});

export default defineComponent({
    name: "Example.FieldTypeGallery",
    components: {
        PaneledBlockTemplate,
        AttributeDesigner: attributeDesigner,
        ...galleryComponents
    },
    template: `
<PaneledBlockTemplate>
    <template v-slot:title>
        <i class="fa fa-flask"></i>
        Obsidian Field Type Gallery
    </template>
    <template v-slot:default>
        <AttributeDesigner />
        <div style="margin-top:60px;"></div>
        ${galleryTemplate}
    </template>
</PaneledBlockTemplate>`
});
