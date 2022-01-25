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

import { computed, defineComponent, onMounted, reactive, Ref, ref, watch } from "vue";
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
import { areEqual, newGuid } from "../../Util/guid";
import { List } from "../../Util/linq";
import { FormField, FormFieldType, FormSection, GeneralAsideSettings, IAsideProvider } from "./FormBuilderDetail/types";
import { FieldType } from "../../SystemGuids";

/**
 * Get the drag source options for the section zones. This allows the user to
 * drag a zone placeholder into the form to add a new zone.
 * 
 * @param sections The (reactive) array of sections to update.
 *
 * @returns The IDragSourceOptions object to use for drag operations.
 */
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

/**
 * Get the drag source options for the field types. This allows the user to
 * drag a new field type placeholder into the form to add a new field.
 *
 * @param sections The (reactive) array of sections to update.
 * @param availableFieldTypes The list of field types that are available to be used.
 *
 * @returns The IDragSourceOptions object to use for drag operations.
 */
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
                    name: fieldType.text,
                    key: "TODO:GenerateDefaultKeyFromName",
                    size: 12,
                    configurationValues: {},
                    defaultValue: ""
                });
            }
        }
    };
}

/**
 * Get the drag source options for re-ordering the fields. This allows the user
 * to drag and drop existing fields to move them around the form.
 *
 * @param sections The (reactive) array of sections to update.
 *
 * @returns The IDragSourceOptions object to use for drag operations.
 */
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

// Unique identifiers for the standard zones.
const formHeaderZoneGuid = "C7D522D0-A18C-4CB0-B604-B2E9727E9E33";
const formFooterZoneGuid = "317E5892-C156-4614-806F-BE4CAB67AC10";
const personEntryZoneGuid = "5257312E-102C-4026-B558-10184AFEAC4D";

// Temporary placeholder for available form field types to use.
const temporaryFieldTypes: FormFieldType[] = [
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
];

// Temporary palceholder for initial sections to display.
const temporarySections: FormSection[] = [
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
                name: "Single Select",
                key: "SingleSelect1",
                size: 6
            },
            {
                guid: newGuid(),
                fieldTypeGuid: FieldType.Memo,
                name: "Memo",
                key: "Memo1",
                size: 6
            },
            {
                guid: newGuid(),
                fieldTypeGuid: FieldType.MultiSelect,
                name: "Multi Select",
                key: "MultiSelect1",
                size: 12
            }
        ]
    }
];

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
        // #region Values

        const config = useConfigurationValues<Record<string, string> | null>();

        /** The body element that will be used for drag and drop operations. */
        const bodyElement = ref<HTMLElement | null>(null);

        /** The component instance that is displaying the general form settings. */
        const generalAsideComponentInstance = ref<IAsideProvider | null>(null);

        /** The component instance that is displaying the person entry editor. */
        const personEntryAsideComponentInstance = ref<IAsideProvider | null>(null);

        /** The component instance that is displaying the section editor. */
        const sectionEditAsideComponentInstance = ref<IAsideProvider | null>(null);

        /** The component instance that is displaying the field editor. */
        const fieldEditAsideComponentInstance = ref<IAsideProvider | null>(null);

        /** All the field types that are available for use when designing a form. */
        const availableFieldTypes = ref<FormFieldType[]>(temporaryFieldTypes);

        /** The identifier of the zone currently being edited. */
        const activeZone = ref("");

        /** The form field that is currently being edited in the aside. */
        const editField = ref<FormField | null>(null);

        /**
         * The section that are currently displayed on the form. This is reactive
         * since we make live updates to it in various places.
         */
        const sections = reactive<FormSection[]>(temporarySections);

        /** The settings object used by the general aside form settings. */
        const generalAsideSettings = ref<GeneralAsideSettings>({
            campusSetFrom: undefined,
            hasPersonEntry: true
        });

        // #endregion

        // #region Computed Values

        /** The current aside being displayed. */
        const activeAside = computed((): IAsideProvider | null => {
            if (showGeneralAside.value) {
                return generalAsideComponentInstance.value;
            }
            else if (personEntryAsideComponentInstance.value) {
                return personEntryAsideComponentInstance.value;
            }
            else if (sectionEditAsideComponentInstance.value) {
                return sectionEditAsideComponentInstance.value;
            }
            else if (fieldEditAsideComponentInstance.value) {
                return fieldEditAsideComponentInstance.value;
            }
            else {
                return null;
            }
        });

        /** True if the general aside should be currently displayed. */
        const showGeneralAside = computed((): boolean => {
            return editField.value === null;
        });

        /** True if the form has a person entry section. */
        const hasPersonEntry = computed((): boolean => generalAsideSettings.value.hasPersonEntry ?? false);

        /** True if the form header model should be open. */
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

        /** True if the form header model should be open. */
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

        /** True if the person entry zone is currently active. */
        const isPersonEntryActive = computed((): boolean => activeZone.value === personEntryZoneGuid);

        // #endregion

        // #region Functions

        /**
         * Checks if we can safely close the current aside panel.
         *
         * @returns true if the aside can be closed, otherwise false.
         */
        const canCloseAside = (): boolean => {
            if (activeAside.value) {
                return activeAside.value.isSafeToClose();
            }
            else {
                return true;
            }
        };

        /**
         * Closes any currently open aside and returns control to the general
         * form settings aside.
         */
        const closeAside = (): void => {
            editField.value = null;
            activeZone.value = "";
        };

        // #endregion

        // #region Event Handlers

        /**
         * Event handler for when the form header section wants to configure itself.
         */
        const onConfigureFormHeader = (): void => {
            if (!canCloseAside()) {
                return;
            }

            closeAside();

            formHeaderEditContent.value = "";
            activeZone.value = formHeaderZoneGuid;
        };

        /**
         * Event handler for when the form footer section wants to configure itself.
         */
        const onConfigureFormFooter = (): void => {
            if (!canCloseAside()) {
                return;
            }

            closeAside();

            activeZone.value = formFooterZoneGuid;
        };

        /**
         * Event handler for when the person entry section wants to configure itself.
         */
        const onConfigurePersonEntry = (): void => {
            if (!canCloseAside()) {
                return;
            }

            closeAside();

            activeZone.value = personEntryZoneGuid;
        };

        /**
         * Event handler for when any field section wants to configure itself.
         * 
         * @param section The section that is requesting to start configuration.
         */
        const onConfigureSection = (section: FormSection): void => {
            if (!canCloseAside()) {
                return;
            }

            closeAside();

            activeZone.value = section.guid;
        };

        /**
         * Event handler for when any field wants to configure itself.
         * 
         * @param field The field that is requesting to start configuration.
         */
        const onConfigureField = (field: FormField): void => {
            if (!canCloseAside()) {
                return;
            }

            closeAside();

            for (const section of sections) {
                for (const existingField of section.fields) {
                    if (areEqual(existingField.guid, field.guid)) {
                        activeZone.value = existingField.guid;
                        editField.value = existingField;

                        return;
                    }
                }
            }
        };

        /**
         * Event handler for when any aside wants to close itself.
         */
        const onAsideClose = (): void => {
            if (!canCloseAside()) {
                return;
            }

            activeZone.value = "";
            editField.value = null;
        };

        /**
         * Event handler for when the edit field aside has updated the field
         * values or configuration.
         * 
         * @param value The new form field details.
         */
        const onEditFieldUpdate = (value: FormField): void => {
            editField.value = value;

            // Find the original value that was just updated and replace it with
            // the new value.
            for (const section of sections) {
                const existingFieldIndex = section.fields.findIndex(f => areEqual(f.guid, value.guid));

                if (existingFieldIndex !== -1) {
                    section.fields.splice(existingFieldIndex, 1, value);
                    return;
                }
            }
        };

        // #endregion

        // TODO: Move these to new component.
        const submitFormHeader = ref(false);
        const formHeaderEditContent = ref("");
        const startSaveFormHeader = (): void => {
            submitFormHeader.value = true;
        };
        const saveFormHeader = (): void => {
            // TODO: Store this somewhere.
            activeZone.value = "";
        };

        // Wait for the body element to load and then update the drag options.
        watch(bodyElement, () => {
            sectionDragSourceOptions.mirrorContainer = bodyElement.value ?? undefined;
            fieldDragSourceOptions.mirrorContainer = bodyElement.value ?? undefined;
            fieldReorderDragSourceOptions.mirrorContainer = bodyElement.value ?? undefined;
        });

        // Generate all the drag options.
        const sectionDragSourceOptions = getSectionDragSourceOptions(sections);
        const fieldDragSourceOptions = getFieldDragSourceOptions(sections, availableFieldTypes);
        const fieldReorderDragSourceOptions = getFieldReorderDragSourceOptions(sections);

        return {
            activeZone,
            availableFieldTypes,
            bodyElement,
            editField,
            fieldDragSourceOptions,
            fieldDragTargetId: fieldDragSourceOptions.id,
            fieldEditAsideComponentInstance,
            fieldReorderDragSourceOptions,
            fieldReorderDragTargetId: fieldReorderDragSourceOptions.id,
            generalAsideComponentInstance,
            generalAsideSettings,
            hasPersonEntry,
            isFormFooterActive,
            isFormHeaderActive,
            isPersonEntryActive,
            onAsideClose,
            onConfigureField,
            onConfigureFormHeader,
            onConfigureFormFooter,
            onConfigurePersonEntry,
            onConfigureSection,
            onEditFieldUpdate,
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

            .custom-switch {
                position: relative;
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

            .form-builder-detail .configurable-zone > .zone-content-container > .zone-content > .zone-body {
                padding: 20px;
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
                        ref="generalAsideComponentInstance"
                        :fieldTypes="availableFieldTypes"
                        :sectionDragOptions="sectionDragSourceOptions"
                        :fieldDragOptions="fieldDragSourceOptions" />

                    <FieldEditAside v-else-if="editField"
                        :modelValue="editField"
                        ref="fieldEditAsideComponentInstance"
                        :fieldTypes="availableFieldTypes"
                        @update:modelValue="onEditFieldUpdate"
                        @close="onAsideClose" />
                </div>

                <div class="p-3 d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
                    <ConfigurableZone :modelValue="isFormHeaderActive" iconCssClass="fa fa-pencil" @configure="onConfigureFormHeader">
                        <div class="zone-body">
                            <div class="text-center">Form Header</div>
                        </div>
                    </ConfigurableZone>

                    <ConfigurableZone v-if="hasPersonEntry" :modelValue="isPersonEntryActive" @configure="onConfigurePersonEntry">
                        <div class="zone-body">
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
                            @configure="onConfigureSection(section)"
                            @configureField="onConfigureField" />
                    </div>

                    <ConfigurableZone :modelValue="isFormFooterActive" iconCssClass="fa fa-pencil" @configure="onConfigureFormFooter">
                        <div class="zone-body">
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
