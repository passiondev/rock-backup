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

import { defineComponent, PropType, ref, watch } from "vue";
import CheckBox from "../Elements/checkBox";
import TextBox from "../Elements/textBox";
import { FieldTypeConfigurationViewModel } from "../ViewModels/Controls/fieldTypeEditor";
import { PublicEditableAttributeViewModel } from "../ViewModels/publicEditableAttribute";
import CategoryPicker from "./categoryPicker";
import FieldTypeEditor from "./fieldTypeEditor";

export default defineComponent({
    name: "AttributeEditor",

    components: {
        TextBox,
        CategoryPicker,
        CheckBox,
        FieldTypeEditor
    },

    props: {
        modelValue: {
            type: Object as PropType<PublicEditableAttributeViewModel | null>,
            default: null
        }
    },

    setup(props, { emit }) {
        const attributeName = ref(props.modelValue?.name ?? "");
        const abbreviatedName = ref(props.modelValue?.abbreviatedName ?? "");
        const attributeKey = ref(props.modelValue?.key ?? "");
        const description = ref(props.modelValue?.description ?? "");
        const isActive = ref(props.modelValue?.isActive ?? true);
        const isPublic = ref(props.modelValue?.isPublic ?? false);
        const isRequired = ref(props.modelValue?.isRequired ?? false);
        const isShowOnBulk = ref(props.modelValue?.showOnBulk ?? false);
        const isShowInGrid = ref(props.modelValue?.showInGrid ?? false);
        const categories = ref([...(props.modelValue?.categories ?? [])]);
        const fieldTypeValue = ref<FieldTypeConfigurationViewModel>({
            fieldTypeGuid: props.modelValue?.fieldTypeGuid ?? "",
            configurationOptions: { ...(props.modelValue?.configurationOptions ?? {}) },
            defaultValue: props.modelValue?.defaultValue ?? ""
        });

        watch([
            attributeName,
            abbreviatedName,
            attributeKey,
            isActive,
            isPublic,
            isRequired,
            isShowOnBulk,
            isShowInGrid,
            categories,
            fieldTypeValue], () => {
                const newModelValue: PublicEditableAttributeViewModel = {
                    ...(props.modelValue ?? {}),
                    name: attributeName.value,
                    abbreviatedName: abbreviatedName.value,
                    key: attributeKey.value,
                    description: description.value,
                    isActive: isActive.value,
                    isPublic: isPublic.value,
                    isRequired: isRequired.value,
                    showOnBulk: isShowOnBulk.value,
                    showInGrid: isShowInGrid.value,
                    categories: [...categories.value],
                    fieldTypeGuid: fieldTypeValue.value.fieldTypeGuid,
                    configurationOptions: { ...fieldTypeValue.value.configurationOptions },
                    defaultValue: fieldTypeValue.value.defaultValue
                };

                emit("update:modelValue", newModelValue);
            });

        return {
            abbreviatedName,
            attributeName,
            attributeKey,
            categories,
            fieldTypeValue,
            isActive,
            isPublic,
            isRequired,
            isShowInGrid,
            isShowOnBulk
        };
    },

    template: `
<fieldset>
    <div class="row">
        <div class="col-md-6">
            <TextBox label="Name" v-model="attributeName" rules="required" />
        </div>

        <div class="col-md-6">
            <CheckBox label="Active" v-model="isActive" help="Set to Inactive to exclude this attribute from Edit and Display UIs." />
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <TextBox label="Abbreviated Name" v-model="abbreviatedName" />
        </div>

        <div class="col-md-6">
            <CheckBox label="Public" v-model="isPublic" help="Set to public if you want this attribute to be displayed in public contexts." />
        </div>
    </div>

    <TextBox label="Description" textMode="multiline" />

    <div class="row">
        <div class="col-md-6">
            <CategoryPicker label="Categories"
                v-model="categories"
                entityTypeGuid="5997C8D3-8840-4591-99A5-552919F90CBD"
                entityTypeQualifierColumn="EntityTypeId"
                entityTypeQualifierValueX="{EL:A2277FBA-D09F-4D07-B0AB-1C650C25A7A7:72657ED8-D16E-492E-AC12-144C5E7567E7}" />

            <TextBox label="Key" v-model="attributeKey" rules="required" />

            <div class="row">
                <div class="col-sm-6">
                    <CheckBox label="Required" v-model="isRequired" />
                </div>

                <div class="col-sm-6">
                    <CheckBox label="Show on Bulk" v-model="isShowOnBulk" help="If selected, this attribute will be shown with bulk update attributes." />
                </div>

                <div class="col-sm-6">
                    <CheckBox label="Show in Grid" v-model="isShowInGrid" help="If selected, this attribute will be included in a grid." />
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <FieldTypeEditor v-model="fieldTypeValue" />
        </div>
    </div>
</fieldset>
`
});
