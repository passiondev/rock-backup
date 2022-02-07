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

import { PropType } from "vue";
import { defineComponent, ref } from "vue";
import { FieldVisibilityRuleItemComponent, FieldVisibilityRuleItem } from "./fieldVisibilityRuleItem";
import DropDownList from "../Elements/dropDownList";
import { ListItem } from "../ViewModels";

export default defineComponent({
    name: "FieldVisibilityRulesEditor",

    components: {
        DropDownList,
        FieldVisibilityRuleItemComponent
    },

    props: {
        fieldName: {
            type: String as PropType<string>,
            required: true
        }
    },

    emits: [],

    setup() {
        const showHide = ref<"Show" | "Hide">("Show");
        const showHideOptions: ListItem[] = [
            { text: "Show", value: "Show" },
            { text: "Hide", value: "Hide" }
        ];

        const allAny = ref<"All" | "Any">("All");
        const allAnyOptions: ListItem[] = [
            { text: "All", value: "All" },
            { text: "Any", value: "Any" }
        ];

        const rules = ref<FieldVisibilityRuleItem[]>([{}]);

        return {
            showHide,
            showHideOptions,
            allAny,
            allAnyOptions,
            rules
        };
    },

    template: `
<div class="filtervisibilityrules-container">
    <div class="filtervisibilityrules-rulesheader">
        <div class="filtervisibilityrules-type form-inline form-inline-all">
            <DropDownList v-model="showHide" :options="showHideOptions" :show-blank-item="false" formControlClasses="input-width-sm margin-r-sm" />
            <div class="form-control-static margin-r-sm">
                <span class="filtervisibilityrules-fieldname">{{ fieldName}}</span><span class="filtervisibilityrules-if"> if</span>
            </div>
            <DropDownList v-model="allAny" :options="allAnyOptions" :show-blank-item="false" formControlClasses="input-width-sm margin-r-sm" />
            <span class="form-control-static">of the following match:</span>
        </div>
    </div>

    <div class="filtervisibilityrules-ruleslist ">
        <FieldVisibilityRuleItemComponent v-for="rule in rules" v-model="rule" />
    </div>

    <div class="filter-actions">
        <button class="btn btn-xs btn-action add-action" @click="addCriteria"><i class="fa fa-filter"></i> Add Criteria</button>
    </div>
</div>
`
});
