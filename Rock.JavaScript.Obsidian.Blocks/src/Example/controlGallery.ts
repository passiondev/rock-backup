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

import { Component, computed, defineComponent, getCurrentInstance, onMounted, onUnmounted, PropType, ref, useAttrs, watch } from "vue";
import HighlightJs from "@Obsidian/Libs/highlightJs";
import FieldFilterEditor from "@Obsidian/Controls/fieldFilterEditor";
import AttributeValuesContainer from "@Obsidian/Controls/attributeValuesContainer";
import TextBox from "@Obsidian/Controls/textBox";
import EmailBox from "@Obsidian/Controls/emailBox";
import CurrencyBox from "@Obsidian/Controls/currencyBox";
import DatePicker from "@Obsidian/Controls/datePicker";
import DateRangePicker from "@Obsidian/Controls/dateRangePicker";
import DateTimePicker from "@Obsidian/Controls/dateTimePicker";
import ListBox from "@Obsidian/Controls/listBox";
import BirthdayPicker from "@Obsidian/Controls/birthdayPicker";
import NumberUpDown from "@Obsidian/Controls/numberUpDown";
import AddressControl, { getDefaultAddressControlModel } from "@Obsidian/Controls/addressControl";
import InlineSwitch from "@Obsidian/Controls/inlineSwitch";
import Switch from "@Obsidian/Controls/switch";
import Toggle from "@Obsidian/Controls/toggle";
import ItemsWithPreAndPostHtml, { ItemWithPreAndPostHtml } from "@Obsidian/Controls/itemsWithPreAndPostHtml";
import StaticFormControl from "@Obsidian/Controls/staticFormControl";
import ProgressTracker, { ProgressTrackerItem } from "@Obsidian/Controls/progressTracker";
import RockForm from "@Obsidian/Controls/rockForm";
import RockButton from "@Obsidian/Controls/rockButton";
import RadioButtonList from "@Obsidian/Controls/radioButtonList";
import DropDownList from "@Obsidian/Controls/dropDownList";
import Dialog from "@Obsidian/Controls/dialog";
import InlineCheckBox from "@Obsidian/Controls/inlineCheckBox";
import CheckBox from "@Obsidian/Controls/checkBox";
import PhoneNumberBox from "@Obsidian/Controls/phoneNumberBox";
import HelpBlock from "@Obsidian/Controls/helpBlock";
import DatePartsPicker, { DatePartsPickerValue } from "@Obsidian/Controls/datePartsPicker";
import ColorPicker from "@Obsidian/Controls/colorPicker";
import NumberBox from "@Obsidian/Controls/numberBox";
import NumberRangeBox from "@Obsidian/Controls/numberRangeBox";
import GenderDropDownList from "@Obsidian/Controls/genderDropDownList";
import SocialSecurityNumberBox from "@Obsidian/Controls/socialSecurityNumberBox";
import TimePicker from "@Obsidian/Controls/timePicker";
import UrlLinkBox from "@Obsidian/Controls/urlLinkBox";
import CheckBoxList from "@Obsidian/Controls/checkBoxList";
import Rating from "@Obsidian/Controls/rating";
import Fullscreen from "@Obsidian/Controls/fullscreen";
import Panel from "@Obsidian/Controls/panel";
import PersonPicker from "@Obsidian/Controls/personPicker";
import FileUploader from "@Obsidian/Controls/fileUploader";
import ImageUploader from "@Obsidian/Controls/imageUploader";
import EntityTypePicker from "@Obsidian/Controls/entityTypePicker";
import SlidingDateRangePicker from "@Obsidian/Controls/slidingDateRangePicker";
import DefinedValuePicker from "@Obsidian/Controls/definedValuePicker";
import CategoryPicker from "@Obsidian/Controls/categoryPicker";
import LocationPicker from "@Obsidian/Controls/locationPicker";
import CopyButton from "@Obsidian/Controls/copyButton";
import EntityTagList from "@Obsidian/Controls/entityTagList";
import Following from "@Obsidian/Controls/following";
import DetailBlock from "@Obsidian/Templates/detailBlock";
import { toNumber } from "@Obsidian/Utility/numberUtils";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";
import { PublicAttributeBag } from "@Obsidian/ViewModels/Utility/publicAttributeBag";
import { newGuid } from "@Obsidian/Utility/guid";
import { FieldFilterGroupBag } from "@Obsidian/ViewModels/Reporting/fieldFilterGroupBag";
import { BinaryFiletype, DefinedType, EntityType, FieldType } from "@Obsidian/SystemGuids";
import { SlidingDateRange, slidingDateRangeToString } from "@Obsidian/Utility/slidingDateRange";
import { PanelAction } from "@Obsidian/Types/Controls/panelAction";
import { sleep } from "@Obsidian/Utility/promiseUtils";
import { upperCaseFirstCharacter } from "@Obsidian/Utility/stringUtils";
import TransitionVerticalCollapse from "@Obsidian/Controls/transitionVerticalCollapse";


import SectionContainer from "@Obsidian/Controls/sectionContainer";
import SectionHeader from "@Obsidian/Controls/sectionHeader";
import { FieldFilterSourceBag } from "@Obsidian/ViewModels/Reporting/fieldFilterSourceBag";
import { PickerDisplayStyle } from "@Obsidian/Types/Controls/pickerDisplayStyle";
import { useStore } from "@Obsidian/PageState";

// #region Gallery Support

const displayStyleItems: ListItemBag[] = [
    {
        value: PickerDisplayStyle.Auto,
        text: "Auto"
    },
    {
        value: PickerDisplayStyle.List,
        text: "List"
    },
    {
        value: PickerDisplayStyle.Condensed,
        text: "Condensed"
    }
];

/**
 * Takes a gallery component's name and converts it to a name that is useful for the header and
 * sidebar by adding spaces and stripping out the "Gallery" suffix
 *
 * @param name Name of the control
 * @returns A string of code that can be used to import the given control file
 */
function convertComponentName(name: string | undefined | null): string {
    if (!name) {
        return "Unknown Component";
    }

    return name.replace(/[A-Z]/g, " $&").replace(/Gallery$/, "").trim();
}

/**
 * A wrapper component that describes the template used for each of the controls
 * within this control gallery
 */
// eslint-disable-next-line @typescript-eslint/naming-convention
export const GalleryAndResult = defineComponent({
    name: "GalleryAndResult",
    inheritAttrs: false,
    components: {
        Switch,
        SectionHeader,
        TransitionVerticalCollapse,
        CopyButton
    },
    props: {
        // The value passed into/controlled by the component, if any
        value: {
            required: false
        },
        // If true, the provided value is a map of multiple values
        hasMultipleValues: {
            type: Boolean as PropType<boolean>,
            default: false
        },
        // Show another copy of the component so you can see that the value is reflected across them
        enableReflection: {
            type: Boolean as PropType<boolean>,
            default: false
        },
        // Code snippet showing how to import the component
        importCode: {
            type: String as PropType<string>
        },
        // Code snippet of the component being used
        exampleCode: {
            type: String as PropType<string>
        },
        // Describe what this component is/does
        description: {
            type: String as PropType<string>,
            default: ""
        }
    },

    setup(props) {
        // Calculate a header based on the name of the component, adding spaces and stripping out the "Gallery" suffix
        const componentName = convertComponentName(getCurrentInstance()?.parent?.type?.name);

        const formattedValue = computed(() => {
            if (!props.hasMultipleValues) {
                return JSON.stringify(props.value, null, 4);
            }
            else {
                return Object.fromEntries(
                    Object.entries(props.value as Record<string, unknown>).map(([key, val]) => {
                        return [
                            key,
                            JSON.stringify(val, null, 4)
                        ];
                    })
                );
            }
        });

        const styledImportCode = computed((): string | undefined => {
            if (!props.importCode) {
                return undefined;
            }

            return HighlightJs.highlight(props.importCode, {
                language: "typescript"
            })?.value;
        });

        const styledExampleCode = computed((): string | undefined => {
            if (!props.exampleCode) {
                return undefined;
            }

            return HighlightJs.highlight(props.exampleCode, {
                language: "html"
            })?.value;
        });

        const showReflection = ref(false);

        return {
            componentName,
            formattedValue,
            showReflection,
            styledExampleCode,
            styledImportCode,
        };
    },

    template: `
<v-style>
.galleryContent-mainRow > div.well {
    overflow-x: auto;
}

.galleryContent-reflectionToggle {
    display: flex;
    justify-content: flex-end;
}

.galleryContent-valueBox {
    max-height: 300px;
    overflow: auto;
}

.galleryContent-codeSampleWrapper {
    position: relative;
}

.galleryContent-codeSample {
    padding-right: 3rem;
    overflow-x: auto;
}

.galleryContent-codeCopyButton {
    position: absolute;
    top: 1.4rem;
    transform: translateY(-50%);
    right: .5rem;
    z-index: 1;
}

.galleryContent-codeCopyButton::before {
    content: "";
    position: absolute;
    top: -0.3rem;
    right: -0.5rem;
    bottom: -0.3rem;
    left: -0.5rem;
    background: linear-gradient(to left, #f5f5f4, #f5f5f4 80%, #f5f5f500);
    z-index: -1;
}
</v-style>

<SectionHeader :title="componentName" :description="description" />
<div class="galleryContent-mainRow mb-4 row">
    <div v-if="$slots.default" class="mb-4" :class="value === void 0 ? 'col-sm-12' : 'col-sm-6'">
        <h4 class="mt-0">Test Control</h4>
        <slot name="default" />

        <div v-if="enableReflection" class="mt-3">
            <div class="mb-3 galleryContent-reflectionToggle">
                <Switch v-model="showReflection" text="Show Reflection" />
            </div>
            <TransitionVerticalCollapse>
                <div v-if="showReflection">
                    <h4 class="mt-0">Control Reflection</h4>
                    <slot name="default" />
                </div>
            </TransitionVerticalCollapse>
        </div>
    </div>
    <div v-if="value !== void 0" class="well mb-4 col-sm-6">
        <h4>Current Value</h4>
        <template v-if="hasMultipleValues" v-for="value, key in formattedValue">
            <h5><code>{{ key }}</code></h5>
            <pre class="m-0 p-0 border-0 galleryContent-valueBox">{{ value }}</pre>
        </template>
        <pre v-else class="m-0 p-0 border-0 galleryContent-valueBox">{{ formattedValue }}</pre>
    </div>
</div>
<div v-if="$slots.settings" class="mb-4">
    <h4 class="mt-0">Settings</h4>
    <slot name="settings" />
</div>
<div v-if="importCode || exampleCode || $slots.usage" class="mb-4">
    <h4 class="mt-0 mb-3">Usage Notes</h4>
    <slot name="usage">
        <h5 v-if="importCode" class="mt-3 mb-2">Import</h5>
        <div v-if="importCode" class="galleryContent-codeSampleWrapper">
            <pre class="galleryContent-codeSample"><code v-html="styledImportCode"></code></pre>
            <CopyButton :value="importCode" class="galleryContent-codeCopyButton" btnSize="sm" btnType="link" />
        </div>
        <h5 v-if="exampleCode" class="mt-3 mb-2">Template Syntax</h5>
        <div v-if="exampleCode" class="galleryContent-codeSampleWrapper">
            <pre class="galleryContent-codeSample"><code v-html="styledExampleCode"></code></pre>
            <CopyButton :value="exampleCode" class="galleryContent-codeCopyButton" btnSize="sm" btnType="link" />
        </div>
    </slot>
</div>

<div v-if="$slots.header">
    <p class="text-semibold font-italic">The <code>header</code> slot is no longer supported.</p>
</div>

<div v-if="$attrs.splitWidth !== void 0">
    <p class="text-semibold font-italic">The <code>splitWidth</code> prop is no longer supported.</p>
</div>

<div v-if="$slots.gallery">
    <p class="text-semibold font-italic">The <code>gallery</code> slot is deprecated. Please update to the newest Control Gallery template.</p>
    <slot name="gallery" />
</div>
<div v-if="$slots.result">
    <p class="text-semibold font-italic">The <code>result</code> slot is deprecated. Please update to the newest Control Gallery template.</p>
    <slot name="result" />
</div>
`
});

/**
 * Generate a string of an import statement that imports the control will the given file name.
 * The control's name will be based off the filename
 *
 * @param fileName Name of the control's file
 * @returns A string of code that can be used to import the given control file
 */
export function getControlImportPath(fileName: string): string {
    return `import ${upperCaseFirstCharacter(fileName)} from "@Obsidian/Controls/${fileName}";`;
}

/**
 * Generate a string of an import statement that imports the template will the given file name.
 * The template's name will be based off the filename
 *
 * @param fileName Name of the control's file
 * @returns A string of code that can be used to import the given control file
 */
export function getTemplateImportPath(fileName: string): string {
    return `import ${upperCaseFirstCharacter(fileName)} from "@Obsidian/Templates/${fileName}";`;
}

// #endregion

// #region Control Gallery

/** Demonstrates an attribute values container. */
const attributeValuesContainerGallery = defineComponent({
    name: "AttributeValuesContainerGallery",
    components: {
        GalleryAndResult,
        AttributeValuesContainer,
        CheckBox,
        NumberBox,
        TextBox
    },
    setup() {
        const isEditMode = ref(false);
        const showAbbreviatedName = ref(false);
        const showEmptyValues = ref(true);
        const displayAsTabs = ref(false);
        const showCategoryLabel = ref(true);
        const numberOfColumns = ref(2);
        const entityName = ref("Foo Entity");

        const categories = [{
            guid: newGuid(),
            name: "Cat A",
            order: 1
        },
        {
            guid: newGuid(),
            name: "Cat B",
            order: 2
        },
        {
            guid: newGuid(),
            name: "Cat C",
            order: 3
        }];

        const attributes = ref<Record<string, PublicAttributeBag>>({
            text: {
                attributeGuid: newGuid(),
                categories: [categories[0]],
                description: "A text attribute.",
                fieldTypeGuid: FieldType.Text,
                isRequired: false,
                key: "text",
                name: "Text Attribute",
                order: 2,
                configurationValues: {}
            },
            color: {
                attributeGuid: newGuid(),
                categories: [categories[0], categories[2]],
                description: "Favorite color? Or just a good one?",
                fieldTypeGuid: FieldType.Color,
                isRequired: false,
                key: "color",
                name: "Random Color",
                order: 4,
                configurationValues: {}
            },
            bool: {
                attributeGuid: newGuid(),
                categories: [categories[2]],
                description: "Are you foo?",
                fieldTypeGuid: FieldType.Boolean,
                isRequired: false,
                key: "bool",
                name: "Boolean Attribute",
                order: 3,
                configurationValues: {}
            },
            textagain: {
                attributeGuid: newGuid(),
                categories: [categories[1]],
                description: "Another text attribute.",
                fieldTypeGuid: FieldType.Text,
                isRequired: false,
                key: "textAgain",
                name: "Some Text",
                order: 5,
                configurationValues: {}
            },
            single: {
                attributeGuid: newGuid(),
                categories: [],
                description: "A single select attribute.",
                fieldTypeGuid: FieldType.SingleSelect,
                isRequired: false,
                key: "single",
                name: "Single Select",
                order: 1,
                configurationValues: {
                    values: JSON.stringify([{ value: "1", text: "One" }, { value: "2", text: "Two" }, { value: "3", text: "Three" }])
                }
            }
        });

        const attributeValues = ref<Record<string, string>>({
            "text": "Default text value",
            "color": "#336699",
            "bool": "N",
            "textAgain": "",
            single: "1"
        });

        return {
            attributes,
            attributeValues,
            isEditMode,
            showAbbreviatedName,
            showEmptyValues,
            displayAsTabs,
            showCategoryLabel,
            numberOfColumns,
            entityName,
            importCode: getControlImportPath("attributeValuesContainer"),
            exampleCode: `<AttributeValuesContainer v-model="attributeValues" :attributes="attributes" :isEditMode="false" :showAbbreviatedName="false" :showEmptyValues="true" :displayAsTabs="false" :showCategoryLabel="true" :numberOfColumns="1" :entityTypeName="entityName" />`
        };
    },
    template: `
<GalleryAndResult
    :value="{ attributes, modelValue: attributeValues }"
    hasMultipleValues
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <AttributeValuesContainer
        v-model="attributeValues"
        :attributes="attributes"
        :isEditMode="isEditMode"
        :showAbbreviatedName="showAbbreviatedName"
        :showEmptyValues="showEmptyValues"
        :displayAsTabs="displayAsTabs"
        :showCategoryLabel="showCategoryLabel"
        :numberOfColumns="numberOfColumns"
        :entityTypeName="entityName"
    />

    <template #settings>
        <div class="row">
            <CheckBox formGroupClasses="col-sm-6" v-model="isEditMode" label="Edit Mode" text="Enable" help="Default: false" />
            <CheckBox formGroupClasses="col-sm-6" v-model="showAbbreviatedName" label="Abbreviated Name" text="Show" help="Default: false" />
        </div>
        <div class="row">
            <CheckBox formGroupClasses="col-sm-6" v-model="showEmptyValues" label="Empty Values" text="Show" help="Default: true; Only applies if not in edit mode" />
            <CheckBox formGroupClasses="col-sm-6" v-model="displayAsTabs" label="Category Tabs" text="Show" help="Default: false; If any attributes are in a category, display each category as a tab. Not applicable while editing." />
        </div>
        <CheckBox v-model="showCategoryLabel" label="Category Labels" text="Show" help="Default: false; Only applies when not displaying tabs." />
        <div class="row">
            <NumberBox formGroupClasses="col-sm-6" v-model="numberOfColumns" label="Number of Columns" help="Default: 1; Only applies when not displaying tabs." />
            <TextBox formGroupClasses="col-sm-6" v-model="entityName" label="Entity Type" help="Default: ''; Appears in the heading when category labels are showing." />
        </div>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a field visibility rules editor. */
const fieldFilterEditorGallery = defineComponent({
    name: "FieldFilterEditorGallery",
    components: {
        GalleryAndResult,
        FieldFilterEditor,
        CheckBox,
        TextBox
    },
    setup() {

        const sources: FieldFilterSourceBag[] = [
            {
                guid: "2a50d342-3a0b-4da3-83c1-25839c75615c",
                type: 0,
                attribute: {
                    attributeGuid: "4eb1eb34-988b-4212-8c93-844fae61b43c",
                    fieldTypeGuid: "9C204CD0-1233-41C5-818A-C5DA439445AA",
                    name: "Text Field",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {
                        maxcharacters: "10"
                    }
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c0",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b0",
                    fieldTypeGuid: "A75DFC58-7A1B-4799-BF31-451B2BBE38FF",
                    name: "Integer Field",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {}
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c1",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b1",
                    fieldTypeGuid: "D747E6AE-C383-4E22-8846-71518E3DD06F",
                    name: "Color",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {
                        selectiontype: "Color Picker"
                    }
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c2",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b2",
                    fieldTypeGuid: "3EE69CBC-35CE-4496-88CC-8327A447603F",
                    name: "Currency",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {}
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c3",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b3",
                    fieldTypeGuid: "9C7D431C-875C-4792-9E76-93F3A32BB850",
                    name: "Date Range",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {}
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c4",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b4",
                    fieldTypeGuid: "7EDFA2DE-FDD3-4AC1-B356-1F5BFC231DAE",
                    name: "Day of Week",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {}
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c5",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b5",
                    fieldTypeGuid: "3D045CAE-EA72-4A04-B7BE-7FD1D6214217",
                    name: "Email",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {}
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c6",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b6",
                    fieldTypeGuid: "2E28779B-4C76-4142-AE8D-49EA31DDB503",
                    name: "Gender",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {
                        hideUnknownGender: "True"
                    }
                }
            },
            {
                guid: "6dbb47c4-5816-4110-8a52-92880d4d05c7",
                type: 0,
                attribute: {
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b7",
                    fieldTypeGuid: "C28C7BF3-A552-4D77-9408-DEDCF760CED0",
                    name: "Memo",
                    description: "",
                    order: 0,
                    isRequired: false,
                    configurationValues: {
                        numberofrows: "4",
                        allowhtml: "True",
                        maxcharacters: "5",
                        showcountdown: "True"
                    }
                }
            }
        ];

        const prefilled = (): FieldFilterGroupBag => ({
            guid: newGuid(),
            expressionType: 4,
            rules: [
                {
                    guid: "a81c3ef9-72a9-476b-8b88-b52f513d92e6",
                    comparisonType: 128,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b0",
                    value: "50"
                },
                {
                    guid: "74d34117-4cc6-4cea-92c5-8297aa693ba5",
                    comparisonType: 2,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b1",
                    value: "BlanchedAlmond"
                },
                {
                    guid: "0fa2b6ea-bc86-4fae-b0da-02e48fed8d96",
                    comparisonType: 8,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b5",
                    value: "@gmail.com"
                },
                {
                    guid: "434107e6-6c0c-4698-90ef-d615b1c2de4b",
                    comparisonType: 2,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b6",
                    value: "2"
                },
                {
                    guid: "706179b9-7518-4a74-8e0f-8a48016aec04",
                    comparisonType: 16,
                    sourceType: 0,
                    attributeGuid: "4eb1eb34-988b-4212-8c93-844fae61b43c",
                    value: "text"
                },
                {
                    guid: "4564eac2-15d9-48d9-b618-563523285af0",
                    comparisonType: 512,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b2",
                    value: "999"
                },
                {
                    guid: "e6c56d4c-7f63-44f9-8f07-1ea0860b605d",
                    comparisonType: 1,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b3",
                    value: "2022-02-01,2022-02-28"
                },
                {
                    guid: "0c27507f-9fb7-4f37-8026-70933bbf1398",
                    comparisonType: 0,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b4",
                    value: "3"
                },
                {
                    guid: "4f68fa2c-0942-4084-bb4d-3c045cef4551",
                    comparisonType: 8,
                    sourceType: 0,
                    attributeGuid: "c41817d8-be26-460c-9f89-a7059ae6a9b7",
                    value: "more text than I want to deal with...."
                }
            ]
        });

        const clean = (): FieldFilterGroupBag => ({
            guid: newGuid(),
            expressionType: 1,
            rules: []
        });

        const usePrefilled = ref(false);
        const value = ref(clean());

        watch(usePrefilled, () => {
            value.value = usePrefilled.value ? prefilled() : clean();
        });

        const title = ref("TEST PROPERTY");

        return {
            sources,
            value,
            title,
            usePrefilled,
            importCode: getControlImportPath("fieldFilterEditor"),
            exampleCode: `<FieldFilterEditor :sources="sources" v-model="value" :title="title" />`
        };
    },
    template: `
<GalleryAndResult
    :value="{ 'output:modelValue':value, 'input:sources':sources }"
    hasMultipleValues
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <FieldFilterEditor :sources="sources" v-model="value" :title="title" />

    <template #settings>
        <TextBox v-model="title" label="Attribute Name" />
        <CheckBox v-model="usePrefilled" text="Use prefilled data" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates a phone number box */
const phoneNumberBoxGallery = defineComponent({
    name: "PhoneNumberBoxGallery",
    components: {
        GalleryAndResult,
        PhoneNumberBox
    },
    setup() {
        return {
            phoneNumber: ref("8005551234"),
            importCode: getControlImportPath("phoneNumberBox"),
            exampleCode: `<PhoneNumberBox label="Phone 2" v-model="phoneNumber" />`
        };
    },
    template: `
<GalleryAndResult
    :value="phoneNumber"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <PhoneNumberBox label="Phone 1" v-model="phoneNumber" />

    <template #settings>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a help block */
const helpBlockGallery = defineComponent({
    name: "HelpBlockGallery",
    components: {
        GalleryAndResult,
        HelpBlock,
        TextBox
    },
    setup() {
        return {
            text: ref("This is some helpful text that explains something."),
            importCode: getControlImportPath("helpBlock"),
            exampleCode: `<HelpBlock text="text" />`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <HelpBlock :text="text" />
    Hover over the symbol to the left to view HelpBlock in action

    <template #settings>
        <TextBox label="Text" v-model="text" help="The text for the help tooltip to display" rules="required" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates a drop down list */
const dropDownListGallery = defineComponent({
    name: "DropDownListGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        DropDownList
    },
    setup() {
        const options: ListItemBag[] = [
            { text: "A Text", value: "a", category: "First" },
            { text: "B Text", value: "b", category: "First" },
            { text: "C Text", value: "c", category: "Second" },
            { text: "D Text", value: "d", category: "Second" }
        ];

        // This function can be used to demonstrate lazy loading of items.
        const loadOptionsAsync = async (): Promise<ListItemBag[]> => {
            await sleep(5000);

            return options;
        };

        return {
            enhanceForLongLists: ref(false),
            loadOptionsAsync,
            showBlankItem: ref(true),
            grouped: ref(false),
            multiple: ref(false),
            value: ref(null),
            options: options,
            importCode: getControlImportPath("dropDownList"),
            exampleCode: `<DropDownList label="Select" v-model="value" :items="options" :showBlankItem="true" :enhanceForLongLists="false" :grouped="false" :multiple="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="{'output:modelValue': value, 'input:items': options}"
    hasMultipleValues
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <DropDownList label="Select" v-model="value" :items="options" :showBlankItem="showBlankItem" :enhanceForLongLists="enhanceForLongLists" :grouped="grouped" :multiple="multiple" />

    <template #settings>
        <div class="row">
            <CheckBox formGroupClasses="col-sm-4" label="Show Blank Item" v-model="showBlankItem" />
            <CheckBox formGroupClasses="col-sm-4" label="Enhance For Long Lists" v-model="enhanceForLongLists" />
            <CheckBox formGroupClasses="col-sm-4" label="Grouped" v-model="grouped" />
            <CheckBox formGroupClasses="col-sm-4" label="Multiple" v-model="multiple" />
        </div>

        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a radio button list */
const radioButtonListGallery = defineComponent({
    name: "RadioButtonListGallery",
    components: {
        GalleryAndResult,
        RadioButtonList,
        Toggle,
        NumberUpDown
    },
    setup() {
        return {
            value: ref("a"),
            isHorizontal: ref(false),
            repeatColumns: ref(0),
            options: [
                { text: "A Text", value: "a" },
                { text: "B Text", value: "b" },
                { text: "C Text", value: "c" },
                { text: "D Text", value: "d" },
                { text: "E Text", value: "e" },
                { text: "F Text", value: "f" },
                { text: "G Text", value: "g" }
            ] as ListItemBag[],
            importCode: getControlImportPath("radioButtonList"),
            exampleCode: `<RadioButtonList label="Radio List" v-model="value" :items="options" :horizontal="false" :repeatColumns="0" />`
        };
    },
    template: `
<GalleryAndResult
    :value="{'output:modelValue': value, 'input:items': options}"
    hasMultipleValues
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <RadioButtonList label="Radio List" v-model="value" :items="options" :horizontal="isHorizontal" :repeatColumns="repeatColumns" />

    <template #settings>
        <div class="row">
            <NumberUpDown formGroupClasses="col-sm-6" label="Horizontal Columns" v-model="repeatColumns" :min="0" />
            <Toggle formGroupClasses="col-sm-6" label="Horizontal" v-model="isHorizontal" />
        </div>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a checkbox */
const checkBoxGallery = defineComponent({
    name: "CheckBoxGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        TextBox
    },
    setup() {
        return {
            isChecked: ref(false),
            importCode: getControlImportPath("checkBox"),
            exampleCode: `<CheckBox label="Check Box" text="Enable" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="isChecked"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <CheckBox label="Check Box" text="Enable" v-model="isChecked" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates an inline checkbox */
const inlineCheckBoxGallery = defineComponent({
    name: "InlineCheckBoxGallery",
    components: {
        GalleryAndResult,
        InlineCheckBox
    },
    data() {
        return {
            isChecked: false,
            inline: true,
            importCode: getControlImportPath("checkBox"),
            exampleCode: `<CheckBox label="Check Box" text="Enable" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="isChecked"
    :importCode="importCode"
    :exampleCode="exampleCode"
    description="Check Box with label that is displayed beside it instead of above it"
    enableReflection
>
    <InlineCheckBox label="Inline Label" v-model="isChecked" />
</GalleryAndResult>`
});

/** Demonstrates a modal / dialog / pop-up */
const dialogGallery = defineComponent({
    name: "DialogGallery",
    components: {
        GalleryAndResult,
        RockButton,
        Dialog,
        CheckBox
    },
    setup() {
        return {
            isDialogVisible: ref(false),
            isDismissible: ref(true),
            importCode: getControlImportPath("dialog"),
            exampleCode: `<Dialog v-model="isDialogVisible" :dismissible="true">
    <template #header>
        <h4>Dialog Header</h4>
    </template>
    <template #default>
        <p>Dialog Main Content</p>
    </template>
    <template #footer>
        <p>Dialog Footer (usually for buttons)</p>
    </template>
</Dialog>`
        };
    },
    template: `
<GalleryAndResult
    :value="isDialogVisible"
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <RockButton @click="isDialogVisible = true">Show</RockButton>

    <Dialog v-model="isDialogVisible" :dismissible="isDismissible">
        <template #header>
            <h4>Romans 11:33-36</h4>
        </template>
        <template #default>
            <p>
                Oh, the depth of the riches<br />
                and the wisdom and the knowledge of God!<br />
                How unsearchable his judgments<br />
                and untraceable his ways!<br />
                For who has known the mind of the Lord?<br />
                Or who has been his counselor?<br />
                And who has ever given to God,<br />
                that he should be repaid?<br />
                For from him and through him<br />
                and to him are all things.<br />
                To him be the glory forever. Amen.
            </p>
        </template>
        <template #footer>
            <RockButton @click="isDialogVisible = false" btnType="primary">OK</RockButton>
            <RockButton @click="isDialogVisible = false" btnType="default">Cancel</RockButton>
        </template>
    </Dialog>

    <template #settings>
        <CheckBox label="Dismissible" text="Show the close button" v-model="isDismissible" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates check box list */
const checkBoxListGallery = defineComponent({
    name: "CheckBoxListGallery",
    components: {
        GalleryAndResult,
        CheckBoxList,
        NumberUpDown,
        Toggle
    },
    setup() {
        return {
            items: ref(["green"]),
            options: [
                { value: "red", text: "Red" },
                { value: "green", text: "Green" },
                { value: "blue", text: "Blue" }
            ] as ListItemBag[],
            isHorizontal: ref(false),
            repeatColumns: ref(0),
            importCode: getControlImportPath("checkBoxList"),
            exampleCode: `<CheckBoxList label="CheckBoxList" v-model="value" :items="options" :horizontal="false" :repeatColumns="0" />`
        };
    },
    template: `
<GalleryAndResult
    :value="{'output:modelValue': items, 'input:items': options}"
    hasMultipleValues
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <CheckBoxList label="CheckBoxList" v-model="items" :items="options" :horizontal="isHorizontal" :repeatColumns="repeatColumns" />

    <template #settings>
        <div class="row">
            <NumberUpDown formGroupClasses="col-sm-6" label="Horizontal Columns" v-model="repeatColumns" :min="0" />
            <Toggle formGroupClasses="col-sm-6" label="Horizontal" v-model="isHorizontal" />
        </div>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a list box */
const listBoxGallery = defineComponent({
    name: "ListBoxGallery",
    components: {
        GalleryAndResult,
        ListBox,
        InlineCheckBox
    },
    setup() {
        return {
            value: ref(["a"]),
            options: [
                { text: "A Text", value: "a" },
                { text: "B Text", value: "b" },
                { text: "C Text", value: "c" },
                { text: "D Text", value: "d" }
            ] as ListItemBag[],
            enhanced: ref(false),
            importCode: getControlImportPath("listBox"),
            exampleCode: `<ListBox label="Select" v-model="value" :items="options" :enhanceForLongLists="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="{'output:modelValue': value, 'input:items': options}"
    hasMultipleValues
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <ListBox label="Select" v-model="value" :items="options" :enhanceForLongLists="enhanced" />

    <template #settings>
        <InlineCheckBox v-model="enhanced" label="Use Enhanced Functionality" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates date pickers */
const datePickerGallery = defineComponent({
    name: "DatePickerGallery",
    components: {
        GalleryAndResult,
        DatePicker,
        InlineCheckBox
    },
    setup() {
        return {
            date: ref<string | null>(null),
            displayCurrentOption: ref(false),
            isCurrentDateOffset: ref(false),
            importCode: getControlImportPath("datePicker"),
            exampleCode: `<DatePicker label="Date" v-model="date" :displayCurrentOption="false" :isCurrentDateOffset="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="date"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <DatePicker label="Date" v-model="date" :displayCurrentOption="displayCurrentOption" :isCurrentDateOffset="isCurrentDateOffset" />

    <template #settings>
        <div class="row">
            <div class="col-sm-4">
                <InlineCheckBox v-model="displayCurrentOption" label="Display Current Option" />
            </div>
            <div class="col-sm-4">
                <InlineCheckBox v-model="isCurrentDateOffset" label="Is Current Date Offset" />
            </div>
        </div>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates date range pickers */
const dateRangePickerGallery = defineComponent({
    name: "DateRangePickerGallery",
    components: {
        GalleryAndResult,
        DateRangePicker
    },
    setup() {
        return {
            date: ref({}),
            importCode: getControlImportPath("dateRangePicker"),
            exampleCode: `<DateRangePicker label="Date Range" v-model="date" />`
        };
    },
    template: `
<GalleryAndResult
    :value="date"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <DateRangePicker label="Date Range" v-model="date" />

    <template #settings>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates date time pickers */
const dateTimePickerGallery = defineComponent({
    name: "DateTimePickerGallery",
    components: {
        GalleryAndResult,
        DateTimePicker,
        InlineCheckBox
    },
    setup() {
        return {
            date: ref<string | null>(null),
            displayCurrentOption: ref(false),
            isCurrentDateOffset: ref(false),
            importCode: getControlImportPath("dateTimePicker"),
            exampleCode: `<DateTimePicker label="Date and Time" v-model="date" :displayCurrentOption="false" :isCurrentDateOffset="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="date"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>

    <DateTimePicker label="Date and Time" v-model="date" :displayCurrentOption="displayCurrentOption" :isCurrentDateOffset="isCurrentDateOffset" />

    <template #settings>
        <div class="row">
            <div class="col-sm-4">
                <InlineCheckBox v-model="displayCurrentOption" label="Display Current Option" />
            </div>
            <div class="col-sm-4">
                <InlineCheckBox v-model="isCurrentDateOffset" label="Is Current Date Offset" />
            </div>
        </div>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates date part pickers */
const datePartsPickerGallery = defineComponent({
    name: "DatePartsPickerGallery",
    components: {
        GalleryAndResult,
        Toggle,
        BirthdayPicker,
        DatePartsPicker
    },
    setup() {
        return {
            showYear: ref(true),
            datePartsModel: ref<DatePartsPickerValue>({
                month: 1,
                day: 1,
                year: 2020
            }),
            importCode: getControlImportPath("datePartsPicker"),
            exampleCode: `<DatePartsPicker label="Date" v-model="date" :requireYear="true" :showYear="true" :allowFutureDates="true" :futureYearCount="50" :startYear="1900" />`
        };
    },
    template: `
<GalleryAndResult
    :value="datePartsModel"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <DatePartsPicker label="Date" v-model="datePartsModel" :showYear="showYear" />

    <template #settings>
        <Toggle label="Show Year" v-model="showYear" />
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a textbox */
const textBoxGallery = defineComponent({
    name: "TextBoxGallery",
    components: {
        GalleryAndResult,
        TextBox
    },
    data() {
        return {
            text: "Some two-way bound text",
            importCode: getControlImportPath("textBox"),
            exampleCode: `<TextBox label="Text 1" v-model="text" :maxLength="50" showCountDown />`
        };
    },
    template: `
<GalleryAndResult
    :value="text"
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <TextBox label="Text 1" v-model="text" :maxLength="50" showCountDown />
    <TextBox label="Text 2" v-model="text" />
    <TextBox label="Memo" v-model="text" textMode="MultiLine" :rows="10" :maxLength="100" showCountDown />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a color picker */
const colorPickerGallery = defineComponent({
    name: "ColorPickerGallery",
    components: {
        GalleryAndResult,
        ColorPicker
    },
    setup() {
        return {
            value: ref("#ee7725"),
            importCode: getControlImportPath("colorPicker"),
            exampleCode: `<ColorPicker label="Color" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <ColorPicker label="Color" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a number box */
const numberBoxGallery = defineComponent({
    name: "NumberBoxGallery",
    components: {
        GalleryAndResult,
        RockForm,
        RockButton,
        TextBox,
        NumberBox
    },
    setup() {
        const minimumValue = ref("0");
        const maximumValue = ref("1");
        const value = ref(42);

        const numericMinimumValue = computed((): number => toNumber(minimumValue.value));
        const numericMaximumValue = computed((): number => toNumber(maximumValue.value));

        return {
            minimumValue,
            maximumValue,
            numericMinimumValue,
            numericMaximumValue,
            value,
            importCode: getControlImportPath("numberBox"),
            exampleCode: `<NumberBox label="Number" v-model="value" :minimumValue="minimumValue" :maximumValue="maximumValue" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <RockForm>
        <NumberBox label="Number" v-model="value" :minimumValue="numericMinimumValue" :maximumValue="numericMaximumValue" />
        <RockButton btnType="primary" type="submit">Test Validation</RockButton>
    </RockForm>

    <template #settings>
        <TextBox label="Minimum Value" v-model="minimumValue" />
        <TextBox label="Maximum Value" v-model="maximumValue" />

        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a number box */
const numberRangeBoxGallery = defineComponent({
    name: "NumberRangeBoxGallery",
    components: {
        GalleryAndResult,
        NumberRangeBox
    },
    setup() {
        return {
            value: ref({ lower: 0, upper: 100 }),
            importCode: getControlImportPath("numberRangeBox"),
            exampleCode: `<NumberRangeBox label="Number Range" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <NumberRangeBox label="Number Range" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a gender picker */
const genderDropDownListGallery = defineComponent({
    name: "GenderDropDownListGallery",
    components: {
        GalleryAndResult,
        GenderDropDownList
    },
    setup() {
        return {
            value: ref("1"),
            importCode: getControlImportPath("genderDropDownList"),
            exampleCode: `<GenderDropDownList label="Your Gender" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <GenderDropDownList label="Your Gender" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code> and <code>Drop Down List</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a social security number box */
const socialSecurityNumberBoxGallery = defineComponent({
    name: "SocialSecurityNumberBoxGallery",
    components: {
        GalleryAndResult,
        SocialSecurityNumberBox
    },
    setup() {
        return {
            value: ref("123456789"),
            importCode: getControlImportPath("socialSecurityNumberBox"),
            exampleCode: `<SocialSecurityNumberBox label="SSN" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <SocialSecurityNumberBox label="SSN" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code> and <code>Drop Down List</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a time picker */
const timePickerGallery = defineComponent({
    name: "TimePickerGallery",
    components: {
        GalleryAndResult,
        TimePicker
    },
    setup() {
        return {
            value: ref({ hour: 14, minute: 15 }),
            importCode: getControlImportPath("timePicker"),
            exampleCode: `<TimePicker label="Time" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <TimePicker label="Time" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code> and <code>Drop Down List</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a rating picker */
const ratingGallery = defineComponent({
    name: "RatingGallery",
    components: {
        GalleryAndResult,
        NumberBox,
        Rating
    },
    setup() {
        return {
            value: ref(3),
            maximumValue: ref(5),
            importCode: getControlImportPath("rating"),
            exampleCode: `<Rating label="Rating" v-model="value" :maxRating="5" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <Rating label="How Would You Rate God?" v-model="value" :maxRating="maximumValue || 5" />

    <template #settings>
        <NumberBox label="Maximum Rating" v-model="maximumValue" />
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a switch */
const switchGallery = defineComponent({
    name: "SwitchGallery",
    components: {
        GalleryAndResult,
        Switch
    },
    setup() {
        return {
            isChecked: ref(false),
            importCode: getControlImportPath("switch"),
            exampleCode: `<Switch text="Switch" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="isChecked"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <Switch text="Switch" v-model="isChecked" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates an inline switch */
const inlineSwitchGallery = defineComponent({
    name: "InlineSwitchGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        InlineSwitch
    },
    setup() {
        return {
            isBold: ref(false),
            isChecked: ref(false),
            importCode: getControlImportPath("inlineSwitch"),
            exampleCode: `<InlineSwitch label="Inline Switch" v-model="value" :isBold="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="isChecked"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <InlineSwitch label="Inline Switch" v-model="isChecked" :isBold="isBold" />

    <template #settings>
        <CheckBox label="Is Bold" v-model="isBold" />
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a currency box */
const currencyBoxGallery = defineComponent({
    name: "CurrencyBoxGallery",
    components: {
        GalleryAndResult,
        CurrencyBox
    },
    setup() {
        return {
            value: ref(1.23),
            importCode: getControlImportPath("currencyBox"),
            exampleCode: `<CurrencyBox label="Currency" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <CurrencyBox label="Currency" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code> and <code>Number Box</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates an email box */
const emailBoxGallery = defineComponent({
    name: "EmailBoxGallery",
    components: {
        GalleryAndResult,
        EmailBox
    },
    setup() {
        return {
            value: ref("ted@rocksolidchurchdemo.com"),
            importCode: getControlImportPath("emailBox"),
            exampleCode: `<EmailBox label="Email" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <EmailBox label="Email" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a number up down control */
const numberUpDownGallery = defineComponent({
    name: "NumberUpDownGallery",
    components: {
        GalleryAndResult,
        NumberUpDown
    },
    setup() {
        return {
            value: ref(1),
            importCode: getControlImportPath("numberUpDown"),
            exampleCode: `<NumberUpDown label="Number" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <NumberUpDown label="Number" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a static form control */
const staticFormControlGallery = defineComponent({
    name: "StaticFormControlGallery",
    components: {
        GalleryAndResult,
        StaticFormControl
    },
    setup() {
        return {
            value: ref("This is a static value"),
            importCode: getControlImportPath("staticFormControl"),
            exampleCode: `<StaticFormControl label="Static Value" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <StaticFormControl label="Static Value" v-model="value" />
</GalleryAndResult>`
});

/** Demonstrates an address control */
const addressControlGallery = defineComponent({
    name: "AddressControlGallery",
    components: {
        GalleryAndResult,
        AddressControl
    },
    setup() {
        return {
            value: ref(getDefaultAddressControlModel()),
            importCode: getControlImportPath("addressControl"),
            exampleCode: `<AddressControl label="Address" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <AddressControl label="Address" v-model="value" />

    <template #settings>
        <p>All props match that of a <code>Rock Form Field</code></p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a toggle button */
const toggleGallery = defineComponent({
    name: "ToggleGallery",
    components: {
        GalleryAndResult,
        TextBox,
        DropDownList,
        Toggle
    },
    setup() {
        return {
            trueText: ref("On"),
            falseText: ref("Off"),
            btnSize: ref("sm"),
            sizeOptions: [
                { value: "lg", text: "Large" },
                { value: "md", text: "Medium" },
                { value: "sm", text: "Small" },
                { value: "xs", text: "Extra Small" },
            ],
            value: ref(false),
            importCode: getControlImportPath("toggle"),
            exampleCode: `<Toggle label="Toggle" v-model="value" trueText="On" falseText="Off" :btnSize="btnSize" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <Toggle label="Toggle" v-model="value" :trueText="trueText" :falseText="falseText" :btnSize="btnSize" />

    <template #settings>
        <TextBox label="True Text" v-model="trueText" />
        <TextBox label="False Text" v-model="falseText" />
        <DropDownList label="Button Size" v-model="btnSize" :items="sizeOptions" />

        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a progress tracker */
const progressTrackerGallery = defineComponent({
    name: "ProgressTrackerGallery",
    components: {
        GalleryAndResult,
        NumberUpDown,
        ProgressTracker
    },
    setup() {
        return {
            value: ref(0),
            items: [
                { key: "S", title: "Start", subtitle: "The beginning" },
                { key: "1", title: "Step 1", subtitle: "The first step" },
                { key: "2", title: "Step 2", subtitle: "The second step" },
                { key: "3", title: "Step 3", subtitle: "The third step" },
                { key: "4", title: "Step 4", subtitle: "The fourth step" },
                { key: "5", title: "Step 5", subtitle: "The fifth step" },
                { key: "6", title: "Step 6", subtitle: "The sixth step" },
                { key: "7", title: "Step 7", subtitle: "The seventh step" },
                { key: "8", title: "Step 8", subtitle: "The eighth step" },
                { key: "F", title: "Finish", subtitle: "The finish" }
            ] as ProgressTrackerItem[],
            importCode: getControlImportPath("progressTracker"),
            exampleCode: `<ProgressTracker :items="items" :currentIndex="0" />`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <ProgressTracker :items="items" :currentIndex="value" />

    <template #settings>
        <NumberUpDown label="Index" v-model="value" :min="0" :max="10" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates an items with pre and post html control */
const itemsWithPreAndPostHtmlGallery = defineComponent({
    name: "ItemsWithPreAndPostHtmlGallery",
    components: {
        GalleryAndResult,
        TextBox,
        ItemsWithPreAndPostHtml
    },
    setup() {
        return {
            value: ref<ItemWithPreAndPostHtml[]>([
                { preHtml: '<div class="row"><div class="col-sm-6">', postHtml: "</div>", slotName: "item1" },
                { preHtml: '<div class="col-sm-6">', postHtml: "</div></div>", slotName: "item2" }
            ]),
            importCode: getControlImportPath("itemsWithPreAndPostHtml"),
            exampleCode: `<ItemsWithPreAndPostHtml :items="value">
    <template #item1>
        <div>This is item 1</div>
    </template>
    <template #item2>
        <div>This is item 2</div>
    </template>
</ItemsWithPreAndPostHtml>`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <ItemsWithPreAndPostHtml :items="value">
        <template #item1>
            <div class="padding-all-sm text-center bg-primary">This is item 1</div>
        </template>
        <template #item2>
            <div class="padding-all-sm text-center bg-primary">This is item 2</div>
        </template>
    </ItemsWithPreAndPostHtml>

    <template #settings>
        <TextBox label="Item 1 - Pre Html" v-model="value[0].preHtml" />
        <TextBox label="Item 1 - Post Html" v-model="value[0].postHtml" />
        <TextBox label="Item 2 - Pre Html" v-model="value[1].preHtml" />
        <TextBox label="Item 2 - Post Html" v-model="value[1].postHtml" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates a URL link box */
const urlLinkBoxGallery = defineComponent({
    name: "UrlLinkBoxGallery",
    components: {
        UrlLinkBox,
        RockForm,
        RockButton,
        GalleryAndResult
    },
    setup() {
        return {
            value: ref("/home/"),
            importCode: getControlImportPath("urlLinkBox"),
            exampleCode: `<UrlLinkBox label="URL" v-model="value" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <UrlLinkBox label="URL" v-model="value" />

    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});


/** Demonstrates the fullscreen component. */
const fullscreenGallery = defineComponent({
    name: "FullscreenGallery",
    components: {
        GalleryAndResult,
        InlineSwitch,
        CheckBox,
        Fullscreen
    },
    setup() {
        return {
            pageOnly: ref(true),
            value: ref(false),
            importCode: getControlImportPath("fullscreen"),
            exampleCode: `<Fullscreen v-model="value" :isPageOnly="true">
    <p>Content to make full screen</p>
</Fullscreen>`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <Fullscreen v-model="value" :isPageOnly="pageOnly">
        <div class="bg-info padding-all-md" style="width:100%; height: 100%; min-height: 300px; display: grid; place-content: center;">
            <InlineSwitch v-model="value" label="Fullscreen" :isBold="true" />
        </div>
    </Fullscreen>

    <template #settings>
        <CheckBox v-model="pageOnly" label="Is Page Only" help="If true, fills content window. If false, hides the browser chrome and fills entire screen." />
    </template>
</GalleryAndResult>`
});

/** Demonstrates the panel component. */
const panelGallery = defineComponent({
    name: "PanelGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        CheckBoxList,
        Panel,
        RockButton
    },

    setup() {
        const simulateValues = ref<string[]>([]);

        const headerSecondaryActions = computed((): PanelAction[] => {
            if (!simulateValues.value.includes("headerSecondaryActions")) {
                return [];
            }

            return [
                {
                    iconCssClass: "fa fa-user",
                    title: "Action 1",
                    type: "default",
                    handler: () => alert("Action 1 selected.")
                },
                {
                    iconCssClass: "fa fa-group",
                    title: "Action 2",
                    type: "default",
                    handler: () => alert("Action 2 selected.")
                }
            ];
        });

        return {
            colors: Array.apply(0, Array(256)).map((_: unknown, index: number) => `rgb(${index}, ${index}, ${index})`),
            collapsableValue: ref(true),
            drawerValue: ref(false),
            hasFullscreen: ref(false),
            headerSecondaryActions,
            simulateValues,
            simulateOptions: [
                {
                    value: "drawer",
                    text: "Drawer"
                },
                {
                    value: "headerActions",
                    text: "Header Actions"
                },
                {
                    value: "headerSecondaryActions",
                    text: "Header Secondary Actions"
                },
                {
                    value: "subheaderLeft",
                    text: "Subheader Left",
                },
                {
                    value: "subheaderRight",
                    text: "Subheader Right"
                },
                {
                    value: "footerActions",
                    text: "Footer Actions"
                },
                {
                    value: "footerSecondaryActions",
                    text: "Footer Secondary Actions"
                },
                {
                    value: "helpContent",
                    text: "Help Content"
                },
                {
                    value: "largeBody",
                    text: "Large Body"
                }
            ],
            simulateDrawer: computed((): boolean => simulateValues.value.includes("drawer")),
            simulateHeaderActions: computed((): boolean => simulateValues.value.includes("headerActions")),
            simulateSubheaderLeft: computed((): boolean => simulateValues.value.includes("subheaderLeft")),
            simulateSubheaderRight: computed((): boolean => simulateValues.value.includes("subheaderRight")),
            simulateFooterActions: computed((): boolean => simulateValues.value.includes("footerActions")),
            simulateFooterSecondaryActions: computed((): boolean => simulateValues.value.includes("footerSecondaryActions")),
            simulateLargeBody: computed((): boolean => simulateValues.value.includes("largeBody")),
            simulateHelp: computed((): boolean => simulateValues.value.includes("helpContent")),
            isFullscreenPageOnly: ref(true),
            value: ref(true),
            importCode: getControlImportPath("panel"),
            exampleCode: `<Panel v-model="isExanded" v-model:isDrawerOpen="false" title="Panel Title" :hasCollapse="true" :hasFullscreen="false" :isFullscreenPageOnly="true" :headerSecondaryActions="false">
    <template #helpContent>Help Content</template>
    <template #drawer>Drawer Content</template>
    <template #headerActions>Header Actions</template>
    <template #subheaderLeft>Sub Header Left</template>
    <template #subheaderRight>Sub Header Right</template>
    <template #footerActions>Footer Actions</template>
    <template #footerSecondaryActions>Footer Secondary Actions</template>

    Main Panel Content
</Panel>`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <Panel v-model="value" v-model:isDrawerOpen="drawerValue" :hasCollapse="collapsableValue" :hasFullscreen="hasFullscreen" :isFullscreenPageOnly="isFullscreenPageOnly" title="Panel Title" :headerSecondaryActions="headerSecondaryActions">
        <template v-if="simulateHelp" #helpContent>
            This is some help text.
        </template>

        <template v-if="simulateDrawer" #drawer>
            <div style="text-align: center;">Drawer Content</div>
        </template>

        <template v-if="simulateHeaderActions" #headerActions>
            <span class="action">
                <i class="fa fa-star-o"></i>
            </span>

            <span class="action">
                <i class="fa fa-user"></i>
            </span>
        </template>

        <template v-if="simulateSubheaderLeft" #subheaderLeft>
            <span class="label label-warning">Warning</span>&nbsp;
            <span class="label label-default">Default</span>
        </template>

        <template v-if="simulateSubheaderRight" #subheaderRight>
            <span class="label label-info">Info</span>&nbsp;
            <span class="label label-default">Default</span>
        </template>

        <template v-if="simulateFooterActions" #footerActions>
            <RockButton btnType="primary">Action 1</RockButton>
            <RockButton btnType="primary">Action 2</RockButton>
        </template>

        <template v-if="simulateFooterSecondaryActions" #footerSecondaryActions>
            <RockButton btnType="default"><i class="fa fa-lock"></i></RockButton>
            <RockButton btnType="default"><i class="fa fa-unlock"></i></RockButton>
        </template>


        <h4>Romans 11:33-36</h4>
        <p>
            Oh, the depth of the riches<br />
            and the wisdom and the knowledge of God!<br />
            How unsearchable his judgments<br />
            and untraceable his ways!<br />
            For who has known the mind of the Lord?<br />
            Or who has been his counselor?<br />
            And who has ever given to God,<br />
            that he should be repaid?<br />
            For from him and through him<br />
            and to him are all things.<br />
            To him be the glory forever. Amen.
        </p>
    </Panel>

    <template #settings>
        <div class="row">
            <CheckBox formGroupClasses="col-sm-3" v-model="collapsableValue" label="Collapsable" />
            <CheckBox formGroupClasses="col-sm-3" v-model="value" label="Panel Open" />
            <CheckBox formGroupClasses="col-sm-3" v-model="hasFullscreen" label="Has Fullscreen" />
            <CheckBox formGroupClasses="col-sm-3" v-model="isFullscreenPageOnly" label="Page Only Fullscreen" />
        </div>
        <CheckBoxList v-model="simulateValues" label="Simulate" :items="simulateOptions" />

        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates a person picker */
const personPickerGallery = defineComponent({
    name: "PersonPickerGallery",
    components: {
        GalleryAndResult,
        PersonPicker
    },
    setup() {
        return {
            value: ref(null),
            importCode: getControlImportPath("personPicker"),
            exampleCode: `<PersonPicker v-model="value" label="Person" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value ?? null"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <PersonPicker v-model="value" label="Person" />
    <template #settings>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates the file uploader component. */
const fileUploaderGallery = defineComponent({
    name: "FileUploaderGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        FileUploader,
        TextBox
    },
    setup() {
        return {
            binaryFileTypeGuid: ref(BinaryFiletype.Default),
            showDeleteButton: ref(true),
            uploadAsTemporary: ref(true),
            uploadButtonText: ref("Upload"),
            value: ref(null),
            importCode: getControlImportPath("fileUploader"),
            exampleCode: `<FileUploader v-model="value" label="File Uploader" :uploadAsTemporary="true" :binaryFileTypeGuid="BinaryFiletype.Default" uploadButtonText="Upload" :showDeleteButton="true" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <FileUploader v-model="value"
        label="File Uploader"
        :uploadAsTemporary="uploadAsTemporary"
        :binaryFileTypeGuid="binaryFileTypeGuid"
        :uploadButtonText="uploadButtonText"
        :showDeleteButton="showDeleteButton"
    />

    <template #settings>
        <div class="row">
            <CheckBox formGroupClasses="col-sm-4" v-model="uploadAsTemporary" label="Upload As Temporary" />
            <TextBox formGroupClasses="col-sm-8" v-model="binaryFileTypeGuid" label="Binary File Type Guid" />
        </div>
        <div class="row">
            <CheckBox formGroupClasses="col-sm-4" v-model="showDeleteButton" label="Show Delete Button" />
            <TextBox formGroupClasses="col-sm-8" v-model="uploadButtonText" label="Upload Button Text" />
        </div>

        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates the image uploader component. */
const imageUploaderGallery = defineComponent({
    name: "ImageUploaderGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        ImageUploader,
        TextBox
    },
    setup() {
        return {
            binaryFileTypeGuid: ref(BinaryFiletype.Default),
            showDeleteButton: ref(true),
            uploadAsTemporary: ref(true),
            uploadButtonText: ref("Upload"),
            value: ref(null),
            importCode: getControlImportPath("imageUploader"),
            exampleCode: `<ImageUploader v-model="value" label="Image Uploader" :uploadAsTemporary="true" :binaryFileTypeGuid="BinaryFiletype.Default" uploadButtonText="Upload" :showDeleteButton="true" />`
        };
    },
    template: `
    <GalleryAndResult
        :value="value"
        :importCode="importCode"
        :exampleCode="exampleCode"
        enableReflection
    >
        <ImageUploader v-model="value"
            label="Image Uploader"
            :uploadAsTemporary="uploadAsTemporary"
            :binaryFileTypeGuid="binaryFileTypeGuid"
            :uploadButtonText="uploadButtonText"
            :showDeleteButton="showDeleteButton"
        />

        <template #settings>
            <div class="row">
                <CheckBox formGroupClasses="col-sm-4" v-model="uploadAsTemporary" label="Upload As Temporary" />
                <TextBox formGroupClasses="col-sm-8" v-model="binaryFileTypeGuid" label="Binary File Type Guid" />
            </div>
            <div class="row">
                <CheckBox formGroupClasses="col-sm-4" v-model="showDeleteButton" label="Show Delete Button" />
                <TextBox formGroupClasses="col-sm-8" v-model="uploadButtonText" label="Upload Button Text" />
            </div>

            <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
        </template>
    </GalleryAndResult>`
});

/** Demonstrates a sliding date range picker */
const slidingDateRangePickerGallery = defineComponent({
    name: "SlidingDateRangePickerGallery",
    components: {
        GalleryAndResult,
        SlidingDateRangePicker
    },
    setup() {
        const value = ref<SlidingDateRange | null>(null);
        const valueText = computed((): string => value.value ? slidingDateRangeToString(value.value) : "");

        return {
            value,
            valueText,
            importCode: getControlImportPath("slidingDateRangePicker"),
            exampleCode: `<SlidingDateRangePicker v-model="value" label="Sliding Date Range" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <SlidingDateRangePicker v-model="value" label="Sliding Date Range" />

    <template #settings>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates defined value picker */
const definedValuePickerGallery = defineComponent({
    name: "DefinedValuePickerGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        DefinedValuePicker,
        TextBox
    },
    setup() {
        return {
            definedTypeGuid: ref(DefinedType.PersonConnectionStatus),
            enhanceForLongLists: ref(false),
            multiple: ref(false),
            value: ref({ "value": "b91ba046-bc1e-400c-b85d-638c1f4e0ce2", "text": "Visitor" }),
            importCode: getControlImportPath("definedValuePicker"),
            exampleCode: `<DefinedValuePicker label="Defined Value" v-model="value" :definedTypeGuid="definedTypeGuid" :multiple="false" :enhanceForLongLists="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <DefinedValuePicker label="Defined Value" v-model="value" :definedTypeGuid="definedTypeGuid" :multiple="multiple" :enhanceForLongLists="enhanceForLongLists" />

    <template #settings>
        <TextBox label="Defined Type" v-model="definedTypeGuid" />
        <CheckBox label="Multiple" v-model="multiple" />
        <CheckBox label="Enhance For Long Lists" v-model="enhanceForLongLists" />

        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates entity type picker */
const entityTypePickerGallery = defineComponent({
    name: "EntityTypePickerGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        DropDownList,
        EntityTypePicker,
        NumberUpDown
    },
    setup() {
        return {
            columnCount: ref(0),
            displayStyle: ref(PickerDisplayStyle.Auto),
            displayStyleItems,
            enhanceForLongLists: ref(false),
            includeGlobalOption: ref(false),
            multiple: ref(false),
            showBlankItem: ref(false),
            value: ref({ value: EntityType.Person, text: "Default Person" }),
            importCode: getControlImportPath("entityTypePicker"),
            exampleCode: `<EntityTypePicker label="Entity Type" v-model="value" :multiple="false" :includeGlobalOption="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <EntityTypePicker label="Entity Type"
        v-model="value"
        :multiple="multiple"
        :columnCount="columnCount"
        :includeGlobalOption="includeGlobalOption"
        :enhanceForLongLists="enhanceForLongLists"
        :displayStyle="displayStyle"
        :showBlankItem="showBlankItem" />

    <template #settings>
        <div class="row">
            <div class="col-md-3">
                <CheckBox label="Multiple" v-model="multiple" />
            </div>

            <div class="col-md-3">
                <CheckBox label="Include Global Option" v-model="includeGlobalOption" />
            </div>

            <div class="col-md-3">
                <CheckBox label="Enhance For Long Lists" v-model="enhanceForLongLists" />
            </div>

            <div class="col-md-3">
                <CheckBox label="Show Blank Item" v-model="showBlankItem" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-3">
                <DropDownList label="Display Style" v-model="displayStyle" :items="displayStyleItems" />
            </div>

            <div class="col-md-3">
                <NumberUpDown label="Column Count" v-model="columnCount" :min="0" />
            </div>
        </div>
    </template>
</GalleryAndResult>`
});

/** Demonstrates the SectionHeader component */
const sectionHeaderGallery = defineComponent({
    name: "SectionHeaderGallery",
    components: {
        GalleryAndResult,
        SectionHeader,
        CheckBox
    },
    setup() {
        const showSeparator = ref(true);
        const showDescription = ref(true);
        const showActionBar = ref(true);
        const showContent = ref(true);

        const description = computed(() => {
            return showDescription.value
                ? "You can use a Section Header to put a title and description above some content."
                : "";
        });

        return {
            showSeparator,
            showDescription,
            showActionBar,
            showContent,
            description,
            importCode: getControlImportPath("sectionHeader"),
            exampleCode: `<SectionHeader title="This is a SectionHeader" description="A Description" :isSeparatorHidden="false">
    <template #actions>Action Buttons</template>
</SectionHeader>`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>

    <SectionHeader
        title="This is a SectionHeader"
        :description="description"
        :isSeparatorHidden="!showSeparator"
    >
        <template v-if="showActionBar" #actions>
            <div>
                <a class="btn btn-default btn-xs btn-square"><i class="fa fa-lock"></i></a>
                <a class="btn btn-default btn-xs btn-square"><i class="fa fa-pencil"></i></a>
                <a class="btn btn-danger btn-xs btn-square"><i class="fa fa-trash-alt"></i></a>
            </div>
        </template>
    </SectionHeader>

    <template #settings>
        <div class="row">
            <CheckBox formGroupClasses="col-xs-4" v-model="showSeparator" label="Show Separator" />
            <CheckBox formGroupClasses="col-xs-4" v-model="showDescription" label="Show Description" />
            <CheckBox formGroupClasses="col-xs-4" v-model="showActionBar" label="Show Action Bar" />
        </div>
    </template>
</GalleryAndResult>`
});

/** Demonstrates the SectionContainer component */
const sectionContainerGallery = defineComponent({
    name: "SectionContainerGallery",
    components: {
        GalleryAndResult,
        SectionContainer,
        CheckBox
    },
    setup() {
        const showDescription = ref(true);
        const showActionBar = ref(true);
        const showContentToggle = ref(false);
        const showContent = ref(true);

        const description = computed(() => {
            return showDescription.value
                ? "The Section Container has a Section Header and a collapsible content section below it."
                : "";
        });

        return {
            showDescription,
            showActionBar,
            showContentToggle,
            showContent,
            description,
            importCode: getControlImportPath("sectionContainer"),
            exampleCode: `<SectionContainer title="This is a Section Container" description="A Description" v-model="showContent" toggleText="Show">
    <template #actions>Action Buttons</template>
    Main Content
</SectionContainer>`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>

    <SectionContainer
        title="This is a Section Container"
        :description="description"
        v-model="showContent"
        :toggleText="showContentToggle ? 'Show' : ''"
    >
        <template v-if="showActionBar" #actions>
            <div>
                <a class="btn btn-default btn-xs btn-square"><i class="fa fa-lock"></i></a>
                <a class="btn btn-default btn-xs btn-square"><i class="fa fa-pencil"></i></a>
                <a class="btn btn-danger btn-xs btn-square"><i class="fa fa-trash-alt"></i></a>
            </div>
        </template>
        Here's some content to put in here.
    </SectionContainer>

    <template #settings>
        <div class="row">
            <CheckBox formGroupClasses="col-xs-4" v-model="showDescription" label="Show Description" />
            <CheckBox formGroupClasses="col-xs-4" v-model="showActionBar" label="Show Action Bar" />
            <CheckBox formGroupClasses="col-xs-4" v-model="showContentToggle" label="Show Content Toggle" />
        </div>
        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates category picker */
const categoryPickerGallery = defineComponent({
    name: "CategoryPickerGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        CategoryPicker,
        TextBox
    },
    setup() {
        return {
            entityTypeGuid: ref(EntityType.DefinedType),
            multiple: ref(false),
            value: ref(null),
            importCode: getControlImportPath("categoryPicker"),
            exampleCode: `<CategoryPicker label="Category Picker" v-model="value" :multiple="false" :entityTypeGuid="entityTypeGuid" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <CategoryPicker label="Category Picker" v-model="value" :multiple="multiple" :entityTypeGuid="entityTypeGuid" />

    <template #settings>
        <CheckBox label="Multiple" v-model="multiple" />
        <TextBox label="Entity Type Guid" v-model="entityTypeGuid" />

        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
        <p>Additional props extend and are passed to the underlying <code>Rock Form Field</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates location picker */
const locationPickerGallery = defineComponent({
    name: "LocationPickerGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        LocationPicker
    },
    setup() {
        return {
            multiple: ref(false),
            value: ref(null),
            importCode: getControlImportPath("locationPicker"),
            exampleCode: `<LocationPicker label="Location" v-model="value" :multiple="false" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode"
    enableReflection
>
    <LocationPicker label="Location" v-model="value" :multiple="multiple" />

    <template #settings>
        <CheckBox label="Multiple" v-model="multiple" />

        <p class="text-semibold font-italic">Not all options have been implemented yet.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates copy button */
const copyButtonGallery = defineComponent({
    name: "CopyButtonGallery",
    components: {
        GalleryAndResult,
        TextBox,
        DropDownList,
        CopyButton,
    },
    setup() {
        return {
            tooltip: ref("Copy"),
            value: ref("To God Be The Glory"),
            buttonSize: ref("md"),
            sizeOptions: [
                { value: "lg", text: "Large" },
                { value: "md", text: "Medium" },
                { value: "sm", text: "Small" },
                { value: "xs", text: "Extra Small" },
            ],
            importCode: getControlImportPath("copyButton"),
            exampleCode: `<CopyButton :value="value" tooltip="Copy" />`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode"
>
    <CopyButton :value="value" :tooltip="tooltip" :btnSize="buttonSize" />

    <template #settings>
        <div class="row">
            <TextBox formGroupClasses="col-sm-4" v-model="value" label="Value to Copy to Clipboard" />
            <TextBox formGroupClasses="col-sm-4" v-model="tooltip" label="Tooltip" />
            <DropDownList formGroupClasses="col-sm-4" label="Button Size" v-model="buttonSize" :items="sizeOptions" />
        </div>

        <p>Additional props extend and are passed to the underlying <code>Rock Button</code>.</p>
    </template>
</GalleryAndResult>`
});

/** Demonstrates entity tag list */
const entityTagListGallery = defineComponent({
    name: "EntityTagListGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        EntityTagList
    },
    setup() {
        const store = useStore();

        return {
            disabled: ref(false),
            entityTypeGuid: EntityType.Person,
            entityKey: store.state.currentPerson?.idKey ?? "",
            importCode: getControlImportPath("entityTagList"),
            exampleCode: `<EntityTagList :entityTypeGuid="entityTypeGuid" :entityKey="entityKey" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode">
    <EntityTagList :entityTypeGuid="entityTypeGuid" :entityKey="entityKey" :disabled="disabled" />

    <template #settings>
        <CheckBox label="Disabled" v-model="disabled" />
    </template>
</GalleryAndResult>`
});

/** Demonstrates following control. */
const followingGallery = defineComponent({
    name: "FollowingGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        Following,
        TextBox
    },
    setup() {
        const store = useStore();

        return {
            disabled: ref(false),
            entityTypeGuid: ref(EntityType.Person),
            entityKey: ref(store.state.currentPerson?.idKey ?? ""),
            importCode: getControlImportPath("following"),
            exampleCode: `<Following :entityTypeGuid="entityTypeGuid" :entityKey="entityKey" />`
        };
    },
    template: `
<GalleryAndResult
    :value="value"
    :importCode="importCode"
    :exampleCode="exampleCode">
    <Following :entityTypeGuid="entityTypeGuid" :entityKey="entityKey" :disabled="disabled" />

    <template #settings>
        <div class="row">
            <div class="col-md-4">
                <CheckBox label="Disabled" v-model="disabled" />
            </div>

            <div class="col-md-4">
                <TextBox label="Entity Type Guid" v-model="entityTypeGuid" />
            </div>

            <div class="col-md-4">
                <TextBox label="Entity Key" v-model="entityKey" />
            </div>
        </div>
    </template>
</GalleryAndResult>`
});



const controlGalleryComponents: Record<string, Component> = [
    attributeValuesContainerGallery,
    fieldFilterEditorGallery,
    textBoxGallery,
    datePickerGallery,
    dateRangePickerGallery,
    dateTimePickerGallery,
    datePartsPickerGallery,
    radioButtonListGallery,
    dialogGallery,
    checkBoxGallery,
    inlineCheckBoxGallery,
    switchGallery,
    inlineSwitchGallery,
    checkBoxListGallery,
    listBoxGallery,
    phoneNumberBoxGallery,
    dropDownListGallery,
    helpBlockGallery,
    colorPickerGallery,
    numberBoxGallery,
    numberRangeBoxGallery,
    genderDropDownListGallery,
    socialSecurityNumberBoxGallery,
    timePickerGallery,
    ratingGallery,
    currencyBoxGallery,
    emailBoxGallery,
    numberUpDownGallery,
    staticFormControlGallery,
    addressControlGallery,
    toggleGallery,
    progressTrackerGallery,
    itemsWithPreAndPostHtmlGallery,
    urlLinkBoxGallery,
    fullscreenGallery,
    panelGallery,
    personPickerGallery,
    fileUploaderGallery,
    imageUploaderGallery,
    slidingDateRangePickerGallery,
    definedValuePickerGallery,
    entityTypePickerGallery,
    sectionHeaderGallery,
    sectionContainerGallery,
    categoryPickerGallery,
    locationPickerGallery,
    copyButtonGallery,
    entityTagListGallery,
    followingGallery
]
    // Sort list by component name
    .sort((a, b) => a.name.localeCompare(b.name))
    // Convert list to an object where the key is the component name and the value is the component
    .reduce((newList, comp) => {
        newList[comp.name] = comp;
        return newList;
    }, {});

// #endregion

// #region Template Gallery

/** Demonstrates the detailPanel component. */
const detailBlockGallery = defineComponent({
    name: "DetailBlockGallery",
    components: {
        GalleryAndResult,
        CheckBox,
        CheckBoxList,
        DetailBlock
    },

    setup() {
        const simulateValues = ref<string[]>([]);

        const headerActions = computed((): PanelAction[] => {
            if (!simulateValues.value.includes("headerActions")) {
                return [];
            }

            return [
                {
                    iconCssClass: "fa fa-user",
                    title: "Action 1",
                    type: "default",
                    handler: () => alert("Action 1 selected.")
                },
                {
                    iconCssClass: "fa fa-group",
                    title: "Action 2",
                    type: "success",
                    handler: () => alert("Action 2 selected.")
                }
            ];
        });

        const labels = computed((): PanelAction[] => {
            if (!simulateValues.value.includes("labels")) {
                return [];
            }

            return [
                {
                    iconCssClass: "fa fa-user",
                    title: "Action 1",
                    type: "info",
                    handler: () => alert("Action 1 selected.")
                },
                {
                    iconCssClass: "fa fa-group",
                    title: "Action 2",
                    type: "success",
                    handler: () => alert("Action 2 selected.")
                }
            ];
        });

        const headerSecondaryActions = computed((): PanelAction[] => {
            if (!simulateValues.value.includes("headerSecondaryActions")) {
                return [];
            }

            return [
                {
                    iconCssClass: "fa fa-user",
                    title: "Action 1",
                    type: "default",
                    handler: () => alert("Action 1 selected.")
                },
                {
                    iconCssClass: "fa fa-group",
                    title: "Action 2",
                    type: "success",
                    handler: () => alert("Action 2 selected.")
                }
            ];
        });

        const footerActions = computed((): PanelAction[] => {
            if (!simulateValues.value.includes("footerActions")) {
                return [];
            }

            return [
                {
                    iconCssClass: "fa fa-user",
                    title: "Action 1",
                    type: "default",
                    handler: () => alert("Action 1 selected.")
                },
                {
                    iconCssClass: "fa fa-group",
                    title: "Action 2",
                    type: "success",
                    handler: () => alert("Action 2 selected.")
                }
            ];
        });

        const footerSecondaryActions = computed((): PanelAction[] => {
            if (!simulateValues.value.includes("footerSecondaryActions")) {
                return [];
            }

            return [
                {
                    iconCssClass: "fa fa-user",
                    title: "Action 1",
                    type: "default",
                    handler: () => alert("Action 1 selected.")
                },
                {
                    iconCssClass: "fa fa-group",
                    title: "Action 2",
                    type: "success",
                    handler: () => alert("Action 2 selected.")
                }
            ];
        });

        return {
            colors: Array.apply(0, Array(256)).map((_: unknown, index: number) => `rgb(${index}, ${index}, ${index})`),
            entityTypeGuid: EntityType.Group,
            footerActions,
            footerSecondaryActions,
            headerActions,
            headerSecondaryActions,
            isAuditHidden: ref(false),
            isBadgesVisible: ref(true),
            isDeleteVisible: ref(true),
            isEditVisible: ref(true),
            isFollowVisible: ref(true),
            isSecurityHidden: ref(false),
            isTagsVisible: ref(false),
            labels,
            simulateValues,
            simulateOptions: [
                {
                    value: "headerActions",
                    text: "Header Actions"
                },
                {
                    value: "headerSecondaryActions",
                    text: "Header Secondary Actions"
                },
                {
                    value: "labels",
                    text: "Labels",
                },
                {
                    value: "footerActions",
                    text: "Footer Actions"
                },
                {
                    value: "footerSecondaryActions",
                    text: "Footer Secondary Actions"
                },
                {
                    value: "helpContent",
                    text: "Help Content"
                }
            ],
            simulateHelp: computed((): boolean => simulateValues.value.includes("helpContent")),
            importCode: getTemplateImportPath("detailBlock"),
            exampleCode: `<DetailBlock name="Sample Entity" :entityTypeGuid="entityTypeGuid" entityTypeName="Entity Type" entityKey="57dc00a3-ff88-4d4c-9878-30ae309117e2" />`
        };
    },
    template: `
<GalleryAndResult
    :importCode="importCode"
    :exampleCode="exampleCode">
    <DetailBlock name="Sample Entity"
        :entityTypeGuid="entityTypeGuid"
        entityTypeName="Entity Type"
        entityKey="57dc00a3-ff88-4d4c-9878-30ae309117e2"
        :headerActions="headerActions"
        :headerSecondaryActions="headerSecondaryActions"
        :labels="labels"
        :footerActions="footerActions"
        :footerSecondaryActions="footerSecondaryActions"
        :isAuditHidden="isAuditHidden"
        :isEditVisible="isEditVisible"
        :isDeleteVisible="isDeleteVisible"
        :isFollowVisible="isFollowVisible"
        :isBadgesVisible="isBadgesVisible"
        :isSecurityHidden="isSecurityHidden"
        :isTagsVisible="isTagsVisible">
        <template v-if="simulateHelp" #helpContent>
            This is some help text.
        </template>
        <div v-for="c in colors" :style="{ background: c, height: '1px' }"></div>
    </DetailBlock>

    <template #settings>
        <div class="row">
            <div class="col-md-3">
                <CheckBox v-model="isAuditHidden" label="Is Audit Hidden" />
            </div>
            <div class="col-md-3">
                <CheckBox v-model="isBadgesVisible" label="Is Badges Visible" />
            </div>
            <div class="col-md-3">
                <CheckBox v-model="isDeleteVisible" label="Is Delete Visible" />
            </div>
            <div class="col-md-3">
                <CheckBox v-model="isEditVisible" label="Is Edit Visible" />
            </div>
            <div class="col-md-3">
                <CheckBox v-model="isFollowVisible" label="Is Follow Visible" />
            </div>
            <div class="col-md-3">
                <CheckBox v-model="isSecurityHidden" label="Is Security Hidden" />
            </div>
            <div class="col-md-3">
                <CheckBox v-model="isTagsVisible" label="Is Tags Visible" />
            </div>
        </div>

        <CheckBoxList v-model="simulateValues" label="Simulate" :items="simulateOptions" horizontal />
    </template>
</GalleryAndResult>`
});

const templateGalleryComponents = [
    detailBlockGallery
]
    .sort((a, b) => a.name.localeCompare(b.name))
    .reduce((newList, comp) => {
        newList[comp.name] = comp;
        return newList;
    }, {});

// #endregion

export default defineComponent({
    name: "Example.ControlGallery",
    components: {
        Panel,
        SectionHeader,
        ...controlGalleryComponents,
        ...templateGalleryComponents
    },

    setup() {
        const currentComponent = ref<Component>(Object.values(controlGalleryComponents)[0]);

        function getComponentFromHash(): void {
            const hashComponent = new URL(document.URL).hash.replace("#", "");
            const component = controlGalleryComponents[hashComponent] ?? templateGalleryComponents[hashComponent];

            if (component) {
                currentComponent.value = component;
            }
        }

        onMounted(() => {
            getComponentFromHash();

            window.addEventListener("hashchange", getComponentFromHash);
        });

        onUnmounted(() => {
            window.removeEventListener("hashchange", getComponentFromHash);
        });

        return {
            currentComponent,
            convertComponentName,
            controlGalleryComponents,
            templateGalleryComponents
        };
    },

    template: `
<v-style>
.galleryContainer {
    margin: -18px;
}

.galleryContainer > * {
    padding: 24px;
}

.gallerySidebar {
    border-radius: 0;
    margin: -1px 0 -1px -1px;
}

.gallerySidebar li.current {
    font-weight: 700;
}
</v-style>
<Panel type="block">
    <template #title>
        Obsidian Control Gallery
    </template>
    <template #default>
        <div class="galleryContainer row">

            <div class="gallerySidebar well col-sm-3">
                <h4>Components</h4>

                <ul class="list-unstyled mb-0">
                    <li v-for="(component, key) in controlGalleryComponents" :key="key" :class="{current: currentComponent.name === component.name}">
                        <a :href="'#' + key" @click="currentComponent = component">{{ convertComponentName(component.name) }}</a>
                    </li>
                </ul>

                <h4 class="mt-3">Templates</h4>

                <ul class="list-unstyled mb-0">
                    <li v-for="(component, key) in templateGalleryComponents" :key="key" :class="{current: currentComponent.name === component.name}">
                        <a :href="'#' + key" @click="currentComponent = component">{{ convertComponentName(component.name) }}</a>
                    </li>
                </ul>
            </div>

            <div class="galleryContent col-sm-9">
                <component :is="currentComponent" />
            </div>

        </div>
    </template>
</Panel>`
});
