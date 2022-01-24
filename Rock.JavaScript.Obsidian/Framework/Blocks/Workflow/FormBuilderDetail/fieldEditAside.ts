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

import { computed, defineComponent, PropType, ref, watch } from "vue";
import Panel from "../../../Controls/panel";
import InlineSwitch from "../../../Elements/switch";
import NumberBox from "../../../Elements/numberBox";
import Slider from "../../../Elements/slider";
import { List } from "../../../Util/linq";
import ConfigurableZone from "./configurableZone";
import { FormField, FormFieldType } from "./types";

export default defineComponent({
    name: "Workflow.FormBuilderDetail.FieldEditAside",
    components: {
        ConfigurableZone,
        Panel,
        InlineSwitch,
        NumberBox,
        Slider
    },

    props: {
        modelValue: {
            type: Object as PropType<FormField>,
            required: true
        },

        fieldTypes: {
            type: Array as PropType<FormFieldType[]>,
            required: true
        }
    },

    emits: [
        "update:modelValue",
        "close"
    ],

    setup(props, { emit }) {
        const fieldType = computed((): FormFieldType | null => {
            return new List(props.fieldTypes).firstOrUndefined(f => f.guid === props.modelValue.fieldTypeGuid) ?? null;
        });

        const asideTitle = computed((): string => props.modelValue.label);

        const asideIconClass = computed((): string => fieldType.value?.icon ?? "");

        const fieldSize = computed({
            get: (): number => props.modelValue.size,
            set: (value: number): void => {
                props.modelValue.size = value;
            }
        });

        const isFieldRequired = computed({
            get: (): boolean => props.modelValue.isRequired ?? false,
            set: (value: boolean): void => {
                props.modelValue.isRequired = value;
            }
        });

        const isFieldLabelHidden = computed({
            get: (): boolean => props.modelValue.isHideLabel ?? false,
            set: (value: boolean): void => {
                props.modelValue.isHideLabel = value;
            }
        });

        const onBackClick = (): void => emit("close");

        return {
            asideIconClass,
            asideTitle,
            fieldSize,
            isFieldLabelHidden,
            isFieldRequired,
            onBackClick
        };
    },

    template: `
<div class="d-flex flex-column" style="overflow-y: hidden; flex-grow: 1;">
    <div class="d-flex">
        <div class="d-flex clickable" style="background-color: #484848; color: #fff; align-items: center; justify-content: center; width: 40px;" @click="onBackClick">
            <i class="fa fa-chevron-left"></i>
        </div>

        <div class="p-2 aside-header" style="flex-grow: 1;">
            <i v-if="asideIconClass" :class="asideIconClass"></i>
            <span class="title">{{ asideTitle }}</span>
        </div>
    </div>

    <div class="aside-body d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
        <Panel title="Field Type" :hasCollapse="true">
        </Panel>

        <Panel title="Conditionals" :hasCollapse="true">
        </Panel>

        <Panel title="Format" :hasCollapse="true">
            <Slider v-model="fieldSize" label="Column Span" :min="1" :max="12" isIntegerOnly showValueBar/>
            <InlineSwitch v-model="isFieldRequired" text="Required" />
            <InlineSwitch v-model="isFieldLabelHidden" text="Hide Label" />
        </Panel>

        <Panel title="Advanced" :hasCollapse="true">
        </Panel>
    </div>
</div>
`
});
