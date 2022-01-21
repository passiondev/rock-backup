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

import { defineComponent, PropType } from "vue";
import { useVModelPassthrough } from "../../../Util/component";
import { DragTarget } from "../../../Directives/dragDrop";
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
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        const section = useVModelPassthrough(props, "modelValue", emit);

        const getFieldColumnSize = (field: FormField): string => `col-xs-${field.size}`;

        const getAttributeValue = (field: FormField): ClientEditableAttributeValue => {
            return {
                attributeGuid: newGuid(),
                fieldTypeGuid: field.fieldTypeGuid,
                name: field.label,
                key: "SampleValue",
                isRequired: false,
                description: "",
                order: 0,
                categories: []
            };
        };

        return {
            getAttributeValue,
            getFieldColumnSize,
            section
        };
    },

    template: `
<ConfigurableZone class="field-zone">
    <div class="row form-section" style="min-height: 100%;" v-drag-target="dragTargetId" :data-section-id="section.guid">
        <div v-for="field in section.fields" :class="getFieldColumnSize(field)">
            <ConfigurableZone>
                <RockField :attributeValue="getAttributeValue(field)" isEditMode />
            </ConfigurableZone>
        </div>
    </div>

</ConfigurableZone>
`
});
