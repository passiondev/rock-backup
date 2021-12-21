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

import { Component, computed, defineComponent, PropType, ref, watch } from "vue";
import TextBox from "../Elements/textBox";
import CheckBox from "../Elements/checkBox";
import FieldTypeEditor from "./fieldTypeEditor";

export default defineComponent({
    name: "AttributeEditor",

    components: {
        TextBox,
        CheckBox,
        FieldTypeEditor
    },

    props: {
    },

    setup(props, { emit }) {
        const attributeName = ref("");
        const abbreviatedName = ref("");
        const attributeKey = ref("");
        const isActive = ref(false);
        const isPublic = ref(false);
        const isRequired = ref(false);
        const isShowOnBulk = ref(false);
        const isShowInGrid = ref(false);

        return {
            abbreviatedName,
            attributeName,
            attributeKey,
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
            <TextBox label="Categories" />

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
            <FieldTypeEditor />
        </div>
    </div>
</fieldset>
`
});
