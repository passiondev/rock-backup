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

import { computed, defineComponent, PropType } from "vue";
import { useVModelPassthrough } from "../../../Util/component";
import { DragSource, DragTarget, IDragSourceOptions } from "../../../Directives/dragDrop";
import { Guid, newGuid } from "../../../Util/guid";
import ConfigurableZone from "./configurableZone";
import RockField from "../../../Controls/rockField";
import { FormField, FormSection } from "./types";
import { ClientEditableAttributeValue } from "../../../ViewModels";

export default defineComponent({
    name: "Workflow.FormBuilderDetail.SectionZone",
    components: {
        ConfigurableZone,
        RockField
    },

    directives: {
        DragSource,
        DragTarget
    },

    props: {
        modelValue: {
            type: Object as PropType<FormSection>,
            required: true
        },

        dragTargetId: {
            type: String as PropType<Guid>,
            required: true
        },

        reorderDragOptions: {
            type: Object as PropType<IDragSourceOptions>,
            required: true
        },

        activeZone: {
            type: String as PropType<string>,
            required: false
        }
    },

    emits: [
        "update:modelValue",
        "configureField"
    ],

    setup(props, { emit }) {
        const section = useVModelPassthrough(props, "modelValue", emit);

        const getFieldColumnSize = (field: FormField): string => `flex-col flex-col-${field.size}`;

        const getAttributeValue = (field: FormField): ClientEditableAttributeValue => {
            return {
                attributeGuid: newGuid(),
                fieldTypeGuid: field.fieldTypeGuid,
                name: !(field.isHideLabel ?? false) ? field.label : "",
                key: "SampleValue",
                isRequired: field.isRequired ?? false,
                description: "",
                order: 0,
                categories: []
            };
        };

        const isSectionActive = computed((): boolean => props.activeZone === section.value.guid);

        const isFieldActive = (field: FormField): boolean => {
            return field.guid === props.activeZone;
        };

        const onConfigureField = (field: FormField): void => {
            emit("configureField", field);
        };

        return {
            getAttributeValue,
            getFieldColumnSize,
            isFieldActive,
            isSectionActive,
            onConfigureField,
            section
        };
    },

    template: `
<ConfigurableZone class="field-zone" :modelValue="isSectionActive">
    <div class="form-section" style="padding: 20px;" v-drag-source="reorderDragOptions" v-drag-target="reorderDragOptions.id" v-drag-target:2="dragTargetId" :data-section-id="section.guid">
        <ConfigurableZone v-for="field in section.fields" :key="field.guid" :modelValue="isFieldActive(field)" :class="getFieldColumnSize(field)" :data-field-id="field.guid" @configure="onConfigureField(field)">
            <div style="padding: 20px;">
                <RockField :attributeValue="getAttributeValue(field)" isEditMode />
            </div>
        </ConfigurableZone>
    </div>
</ConfigurableZone>
`
});
