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

import { computed, defineComponent, onMounted, reactive, Ref, ref } from "vue";
import { useConfigurationValues } from "../../Util/block";
import DropDownList from "../../Elements/dropDownList";
import Modal from "../../Controls/modal";
import Panel from "../../Controls/panel";
import RockButton from "../../Elements/rockButton";
import RockLabel from "../../Elements/rockLabel";
import RockForm from "../../Controls/rockForm";
import Switch from "../../Elements/switch";
import TextBox from "../../Elements/textBox";
import ConfigurableZone from "./FormBuilderDetail/configurableZone";
import FieldEditAside from "./FormBuilderDetail/fieldEditAside";
import GeneralAside from "./FormBuilderDetail/generalAside";
import SectionZone from "./FormBuilderDetail/sectionZone";
import { DragSource, DragTarget, IDragSourceOptions } from "../../Directives/dragDrop";
import { newGuid } from "../../Util/guid";
import { List } from "../../Util/linq";
import { FormField, FormFieldType, FormSection, GeneralAsideSettings } from "./FormBuilderDetail/types";
import { FieldType } from "../../SystemGuids";

function getSectionDragSourceOptions(sections: FormSection[]): IDragSourceOptions {
    return {
        id: newGuid(),
        copyElement: true,
        dragDrop(operation) {
            operation.element.remove();

            if (operation.targetIndex !== undefined) {
                sections.splice(operation.targetIndex, 0, {
                    guid: newGuid(),
                    title: "",
                    description: "",
                    showHeadingSeparator: false,
                    type: "",
                    fields: []
                });
            }
        }
    };
}

function getFieldDragSourceOptions(sections: FormSection[], availableFieldTypes: Ref<FormFieldType[]>): IDragSourceOptions {
    return {
        id: newGuid(),
        copyElement: true,
        dragOver(operation) {
            if (operation.targetContainer && operation.targetContainer instanceof HTMLElement) {
                operation.targetContainer.closest(".field-zone")?.classList.add("highlight");
            }
        },
        dragOut(operation) {
            if (operation.targetContainer && operation.targetContainer instanceof HTMLElement) {
                operation.targetContainer.closest(".field-zone")?.classList.remove("highlight");
            }
        },
        dragShadow(operation) {
            if (operation.shadow) {
                operation.shadow.classList.remove("col-xs-6");
                operation.shadow.classList.add("flex-col", "flex-col-12");
            }
        },
        dragDrop(operation) {
            operation.element.remove();

            const fieldTypeGuid = (operation.element as HTMLElement).dataset.fieldType;
            const sectionGuid = (operation.targetContainer as HTMLElement).dataset.sectionId;
            const section = new List(sections).firstOrUndefined(s => s.guid === sectionGuid);
            const fieldType = new List(availableFieldTypes.value).firstOrUndefined(f => f.guid === fieldTypeGuid);

            if (section && fieldType && operation.targetIndex !== undefined) {
                section.fields.splice(operation.targetIndex, 0, {
                    guid: newGuid(),
                    fieldTypeGuid: fieldType.guid,
                    label: fieldType.text,
                    size: 12
                });
            }
        }
    };
}

function getFieldReorderDragSourceOptions(sections: FormSection[]): IDragSourceOptions {
    return {
        id: newGuid(),
        copyElement: false,
        handleSelector: ".zone-actions",
        dragDrop(operation) {
            const sourceSectionGuid = (operation.sourceContainer as HTMLElement).dataset.sectionId;
            const sourceSection = new List(sections).firstOrUndefined(s => s.guid === sourceSectionGuid);
            const targetSectionGuid = (operation.targetContainer as HTMLElement).dataset.sectionId;
            const targetSection = new List(sections).firstOrUndefined(s => s.guid === targetSectionGuid);

            if (sourceSection && targetSection && operation.targetIndex !== undefined) {
                const field = sourceSection.fields[operation.sourceIndex];

                sourceSection.fields.splice(operation.sourceIndex, 1);
                targetSection.fields.splice(operation.targetIndex, 0, field);
            }
        }
    };
}

const formHeaderZoneGuid = "C7D522D0-A18C-4CB0-B604-B2E9727E9E33";
const formFooterZoneGuid = "317E5892-C156-4614-806F-BE4CAB67AC10";
const personEntryZoneGuid = "5257312E-102C-4026-B558-10184AFEAC4D";

export default defineComponent({
    name: "Workflow.FormBuilderDetail",
    components: {
        ConfigurableZone,
        DropDownList,
        FieldEditAside,
        GeneralAside,
        Modal,
        Panel,
        RockButton,
        RockForm,
        RockLabel,
        SectionZone,
        Switch,
        TextBox
    },

    directives: {
        DragSource,
        DragTarget,
    },

    setup() {
        const config = useConfigurationValues<Record<string, string> | null>();

        const bodyElement = ref<HTMLElement | null>(null);

        const availableFieldTypes = ref<FormFieldType[]>([
            {
                guid: FieldType.Text,
                text: "Text",
                icon: "fa fa-font",
                isCommon: true
            },
            {
                guid: FieldType.Memo,
                text: "Memo",
                icon: "fa fa-align-left",
                isCommon: true
            },
            {
                guid: FieldType.SingleSelect,
                text: "Single Select",
                icon: "fa fa-dot-circle",
                isCommon: true
            },
            {
                guid: FieldType.MultiSelect,
                text: "Multi Select",
                icon: "fa fa-check-circle",
                isCommon: true
            },
            {
                guid: FieldType.Decimal,
                text: "Decimal",
                icon: "fa fa-font",
                isCommon: true
            },
            {
                guid: FieldType.Date,
                text: "Date",
                icon: "fa fa-calendar",
                isCommon: true
            },
            {
                guid: FieldType.Time,
                text: "Time",
                icon: "fa fa-clock",
                isCommon: true
            },
            {
                guid: FieldType.DateTime,
                text: "Date Time",
                icon: "fa fa-calendar-alt",
                isCommon: true
            },
            {
                guid: FieldType.Boolean,
                text: "Boolean",
                icon: "fa fa-check",
                isCommon: true
            }
        ]);

        const activeZone = ref("");

        const sections = reactive<FormSection[]>([
            {
                guid: newGuid(),
                title: "My Favorite Things",
                description: "Below is a form that helps us get to know you a bit more. Please complete it and we'll keep it on record.",
                showHeadingSeparator: true,
                type: "",
                fields: [
                    {
                        guid: newGuid(),
                        fieldTypeGuid: FieldType.SingleSelect,
                        label: "Single Select",
                        size: 6
                    },
                    {
                        guid: newGuid(),
                        fieldTypeGuid: FieldType.Memo,
                        label: "Memo",
                        size: 6
                    },
                    {
                        guid: newGuid(),
                        fieldTypeGuid: FieldType.MultiSelect,
                        label: "Multi Select",
                        size: 12
                    }
                ]
            }
        ]);

        const sectionDragSourceOptions = getSectionDragSourceOptions(sections);
        const fieldDragSourceOptions = getFieldDragSourceOptions(sections, availableFieldTypes);
        const fieldReorderDragSourceOptions = getFieldReorderDragSourceOptions(sections);

        const generalAsideSettings = ref<GeneralAsideSettings>({
            campusSetFrom: undefined,
            hasPersonEntry: true
        });

        const showGeneralAside = computed((): boolean => {
            return editField.value === null;
        });

        const hasPersonEntry = computed((): boolean => generalAsideSettings.value.hasPersonEntry ?? false);

        const isFormHeaderActive = computed({
            get: (): boolean => {
                return activeZone.value === formHeaderZoneGuid;
            },
            set(value: boolean) {
                if (!value && activeZone.value === formHeaderZoneGuid) {
                    activeZone.value = "";
                }
            }
        });

        const isFormFooterActive = computed({
            get: (): boolean => {
                return activeZone.value === formFooterZoneGuid;
            },
            set(value: boolean) {
                if (!value && activeZone.value === formFooterZoneGuid) {
                    activeZone.value = "";
                }
            }
        });
        const isPersonEntryActive = computed((): boolean => activeZone.value === personEntryZoneGuid);

        const configureFormHeader = (): void => {
            formHeaderEditContent.value = "";
            activeZone.value = formHeaderZoneGuid;
        };

        const configureFormFooter = (): void => {
            activeZone.value = formFooterZoneGuid;
        };

        const configurePersonEntry = (): void => {
            activeZone.value = personEntryZoneGuid;
        };

        const configureSection = (section: FormSection): void => {
            activeZone.value = section.guid;
        };

        const configureField = (field: FormField): void => {
            activeZone.value = field.guid;
            editField.value = field;
        };

        const submitFormHeader = ref(false);
        const formHeaderEditContent = ref("");
        const startSaveFormHeader = (): void => {
            submitFormHeader.value = true;
        };
        const saveFormHeader = (): void => {
            // TODO: Store this somewhere.
            activeZone.value = "";
        };

        const editField = ref<FormField | null>(null);

        const onAsideClose = (): void => {
            // TODO: Check if the aside can be closed.
            activeZone.value = "";
            editField.value = null;
        };

        onMounted(() => {
            sectionDragSourceOptions.mirrorContainer = bodyElement.value ?? undefined;
            fieldDragSourceOptions.mirrorContainer = bodyElement.value ?? undefined;
            fieldReorderDragSourceOptions.mirrorContainer = bodyElement.value ?? undefined;
        });

        return {
            activeZone,
            availableFieldTypes,
            bodyElement,
            configureField,
            configureFormHeader,
            configureFormFooter,
            configurePersonEntry,
            configureSection,
            editField,
            fieldDragSourceOptions,
            fieldDragTargetId: fieldDragSourceOptions.id,
            fieldReorderDragSourceOptions,
            fieldReorderDragTargetId: fieldReorderDragSourceOptions.id,
            generalAsideSettings,
            hasPersonEntry,
            isFormFooterActive,
            isFormHeaderActive,
            isPersonEntryActive,
            onAsideClose,
            saveFormHeader,
            sectionDragSourceOptions,
            sectionDragTargetId: sectionDragSourceOptions.id,
            sections,
            showGeneralAside,
            startSaveFormHeader,
            submitFormHeader
        };
    },

    template: `
<Panel type="block" hasFullscreen title="Workflow Form Builder" titleIconClass="fa fa-hammer">
    <template #default>
        <v-style>
            /*** Overrides for theme CSS ***/
            .form-builder-detail .form-section {
                margin-bottom: 0px;
            }

            /*** Style Variables ***/
            .form-builder-detail {
                --zone-color: #e1e1e1;
                --zone-action-text-color: #a7a7a7;
                --zone-active-color: #c9eaf9;
                --zone-active-action-text-color: #83bad3;
                --zone-highlight-color: #ee7725;
                --zone-highlight-action-text-color: #e4bda2;
                --flex-col-gutter: 30px;
            }

            /*** Form Template Items ***/
            .form-builder-detail .form-template-item {
                display: flex;
                align-items: center;
                background-color: #ffffff;
                border: 1px solid #e1e1e1;
                border-left: 3px solid #e1e1e1;
                border-radius: 3px;
                padding: 6px;
                font-size: 13px;
                font-weight: 600;
                cursor: grab;
            }

            .form-builder-detail .form-template-item > .fa {
                margin-right: 6px;
            }

            .form-builder-detail .form-template-item > .text {
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }

            .form-builder-detail .form-template-item.form-template-item-section {
                border-left-color: #009ce3;
            }

            .form-builder-detail .form-template-item.form-template-item-field {
                border-left-color: #ee7725;
                margin-right: 5px;
                margin-bottom: 5px;
                flex-basis: calc(50% - 5px);
            }

            /*** Configuration Asides ***/
            .form-builder-detail .aside-header {
                border-right: 1px solid #dfe0e1;
                border-bottom: 1px solid #dfe0e1;
            }

            .form-builder-detail .aside-header .fa {
                margin-right: 4px;
            }

            .form-builder-detail .aside-header .title {
                font-size: 85%;
                font-weight: 600;
            }

            .form-builder-detail .aside-body {
                padding: 15px;
            }

            /*** Configurable Zones ***/
            .form-builder-detail .configurable-zone {
                display: flex;
                margin-bottom: 12px;
            }

            .form-builder-detail .configurable-zone.field-zone {
                flex-grow: 1;
            }

            .form-builder-detail .configurable-zone > .zone-content-container {
                display: flex;
                flex-grow: 1;
                border: 2px solid var(--zone-color);
            }

            .form-builder-detail .configurable-zone.field-zone > .zone-content-container {
                border-style: dashed;
                border-right-style: solid;
            }

            .form-builder-detail .configurable-zone > .zone-content-container > .zone-content {
                flex-grow: 1;
            }

            .form-builder-detail .configurable-zone > .zone-actions {
                background-color: var(--zone-color);
                border: 2px solid var(--zone-color);
                border-left: 0px;
                width: 40px;
                flex-shrink: 0;
                display: flex;
                justify-content: center;
                align-items: center;
                color: var(--zone-action-text-color);
                cursor: pointer;
            }

            .form-builder-detail .configurable-zone.active > .zone-content-container {
                border-color: var(--zone-active-color);
            }

            .form-builder-detail .configurable-zone.active > .zone-actions {
                background-color: var(--zone-active-color);
                border-color: var(--zone-active-color);
                color: var(--zone-active-action-text-color);
            }

            .form-builder-detail .configurable-zone.highlight > .zone-content-container {
                border-color: var(--zone-highlight-color);
                border-right-style: dashed;
            }

            /*** Form Sections ***/
            .form-builder-detail .form-section {
                display: flex;
                flex-wrap: wrap;
                align-content: flex-start;
                margin-right: calc(0px - var(--flex-col-gutter));
                min-height: 100%;
            }

            .form-builder-detail .form-section .form-template-item.form-template-item-field {
                margin: 0px 0px 12px 0px;
                flex-basis: calc(100% - var(--flex-col-gutter));
            }

            /*** Flex Column Sizes ***/
            .form-builder-detail .flex-col {
                margin-right: var(--flex-col-gutter);
            }

            .form-builder-detail .flex-col-1 {
                flex-basis: calc(8.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-2 {
                flex-basis: calc(16.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-3 {
                flex-basis: calc(25% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-4 {
                flex-basis: calc(33.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-5 {
                flex-basis: calc(41.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-6 {
                flex-basis: calc(50% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-7 {
                flex-basis: calc(58.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-8 {
                flex-basis: calc(66.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-9 {
                flex-basis: calc(75% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-10 {
                flex-basis: calc(83.3333% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-11 {
                flex-basis: calc(91.6666% - var(--flex-col-gutter));
            }

            .form-builder-detail .flex-col-12 {
                flex-basis: calc(100% - var(--flex-col-gutter));
            }
        </v-style>

        <div ref="bodyElement" class="form-builder-detail d-flex flex-column position-absolute inset-0" style="overflow-y: hidden;">
            <div class="p-2 d-flex" style="border-bottom: 1px solid #dfe0e1;">
                <ul class="nav nav-pills" style="flex-grow: 1;">
                    <li role="presentation"><a href="#">Submissions</a></li>
                    <li class="active" role="presentation"><a href="#">Form Builder</a></li>
                    <li role="presentation"><a href="#">Communications</a></li>
                    <li role="presentation"><a href="#">Settings</a></li>
                    <li role="presentation"><a href="#">Analytics</a></li>
                </ul>

                <div>
                    <RockButton btnType="primary">Save</RockButton>
                </div>
            </div>

            <div class="d-flex" style="flex-grow: 1; overflow-y: hidden;">
                <div class="d-flex flex-column" style="background-color: #f8f9fa; width: 320px; flex-shrink: 0; overflow-y: hidden;">
                    <GeneralAside v-if="showGeneralAside"
                        v-model="generalAsideSettings"
                        :fieldTypes="availableFieldTypes"
                        :sectionDragOptions="sectionDragSourceOptions"
                        :fieldDragOptions="fieldDragSourceOptions" />

                    <FieldEditAside v-else-if="editField"
                        v-model="editField"
                        :fieldTypes="availableFieldTypes"
                        @close="onAsideClose" />
                </div>

                <div class="p-3 d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
                    <ConfigurableZone :modelValue="isFormHeaderActive" iconCssClass="fa fa-pencil" @configure="configureFormHeader">
                        <div style="padding: 20px;">
                            <div class="text-center">Form Header</div>
                        </div>
                    </ConfigurableZone>

                    <ConfigurableZone v-if="hasPersonEntry" :modelValue="isPersonEntryActive" @configure="configurePersonEntry">
                        <div style="padding: 20px;">
                            <div class="text-center">Person Entry Form</div>
                        </div>
                    </ConfigurableZone>

                    <div style="flex-grow: 1; display: flex; flex-direction: column;" v-drag-target="sectionDragTargetId">
                        <SectionZone v-for="section in sections"
                            :key="section.guid"
                            v-model="section"
                            :activeZone="activeZone"
                            :dragTargetId="fieldDragTargetId"
                            :reorderDragOptions="fieldReorderDragSourceOptions"
                            @configure="configureSection(section)"
                            @configureField="configureField" />
                    </div>

                    <ConfigurableZone :modelValue="isFormFooterActive" iconCssClass="fa fa-pencil" @configure="configureFormFooter">
                        <div style="padding: 20px;">
                            <div class="text-center">Form Footer</div>
                        </div>
                    </ConfigurableZone>
                </div>
            </div>
        </div>
    </template>
</Panel>

<Modal v-model="isFormHeaderActive" title="Form Header">
    <RockForm v-model:submit="submitFormHeader" @submit="saveFormHeader">
        <TextBox v-model="formHeaderEditContent" label="Content" textMode="multiline" />
    </RockForm>

    <template #customButtons>
        <RockButton btnType="primary" @click="startSaveFormHeader">Update</RockButton>
    </template>
</Modal>
`
});
