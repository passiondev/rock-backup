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
import CheckBox from "../../../Elements/checkBox";
import DropDownList from "../../../Elements/dropDownList";
import TextBox from "../../../Elements/textBox";
import RockForm from "../../../Controls/rockForm";
import { ListItem } from "../../../ViewModels";
import { toNumberOrNull } from "../../../Services/number";
import { FormPersonEntrySettings, WorkflowActionFormPersonEntryOption } from "./types";

export default defineComponent({
    name: "Workflow.FormBuilderDetail.PersonEntrySettings",
    components: {
        CheckBox,
        DropDownList,
        Panel,
        RockForm,
        TextBox
    },

    props: {
        modelValue: {
            type: Object as PropType<FormPersonEntrySettings>,
            default: {}
        },

        recordStatusOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        },

        connectionStatusOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        },

        campusTypeOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        },

        campusStateOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        },

        addressTypeOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        },

        isVertical: {
            type: Boolean as PropType<boolean>,
            default: false
        }
    },

    emits: [
        "update:modelValue",
        "close"
    ],

    setup(props, { emit }) {
        const autofillCurrentPerson = ref(props.modelValue.autofillCurrentPerson ?? false);
        const hideIfCurrentPersonKnown = ref(props.modelValue.hideIfCurrentPersonKnown ?? false);
        const recordStatus = ref(props.modelValue.recordStatus ?? "");
        const connectionStatus = ref(props.modelValue.connectionStatus ?? "");
        const showCampus = ref(props.modelValue.showCampus ?? false);
        const campusType = ref(props.modelValue.campusType ?? "");
        const campusState = ref(props.modelValue.campusState ?? "");
        const gender = ref(props.modelValue.gender?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const email = ref(props.modelValue.gender?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const mobilePhone = ref(props.modelValue.mobilePhone?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const birthdate = ref(props.modelValue.birthdate?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const address = ref(props.modelValue.address?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const addressType = ref(props.modelValue.addressType ?? "");
        const maritalStatus = ref(props.modelValue.maritalStatus?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const spouseEntry = ref(props.modelValue.spouseEntry?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString());
        const spouseLabel = ref(props.modelValue.spouseLabel ?? "");

        const triStateOptions: ListItem[] = [
            {
                value: WorkflowActionFormPersonEntryOption.Hidden.toString(),
                text: "Hidden"
            },
            {
                value: WorkflowActionFormPersonEntryOption.Optional.toString(),
                text: "Optionsl"
            },
            {
                value: WorkflowActionFormPersonEntryOption.Required.toString(),
                text: "Required"
            }
        ];

        /** The column span class to apply to the columns. */
        const columnClass = computed((): string => props.isVertical ? "col-xs-12" : "col-md-3");

        // Watch for any changes in our simple field values and update the
        // modelValue.
        watch([autofillCurrentPerson, hideIfCurrentPersonKnown, recordStatus, connectionStatus, showCampus, campusType, campusState, gender, email, mobilePhone, birthdate, address, addressType, maritalStatus, spouseEntry, spouseLabel], () => {
            const newValue: FormPersonEntrySettings = {
                ...props.modelValue,
                autofillCurrentPerson: autofillCurrentPerson.value,
                hideIfCurrentPersonKnown: hideIfCurrentPersonKnown.value,
                recordStatus: recordStatus.value,
                connectionStatus: connectionStatus.value,
                showCampus: showCampus.value,
                campusType: campusType.value,
                campusState: campusState.value,
                gender: toNumberOrNull(gender.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                email: toNumberOrNull(email.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                mobilePhone: toNumberOrNull(mobilePhone.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                birthdate: toNumberOrNull(birthdate.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                address: toNumberOrNull(address.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                addressType: addressType.value,
                maritalStatus: toNumberOrNull(maritalStatus.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                spouseEntry: toNumberOrNull(spouseEntry.value) ?? WorkflowActionFormPersonEntryOption.Hidden,
                spouseLabel: spouseLabel.value
            };

            emit("update:modelValue", newValue);
        });

        // Watch for any incoming changes from the parent component and update
        // all our individual field values.
        watch(() => props.modelValue, () => {
            autofillCurrentPerson.value = props.modelValue.autofillCurrentPerson ?? false;
            hideIfCurrentPersonKnown.value = props.modelValue.hideIfCurrentPersonKnown ?? false;
            recordStatus.value = props.modelValue.recordStatus ?? "";
            connectionStatus.value = props.modelValue.connectionStatus ?? "";
            showCampus.value = props.modelValue.showCampus ?? false;
            campusType.value = props.modelValue.campusType ?? "";
            campusState.value = props.modelValue.campusState ?? "";
            gender.value = props.modelValue.gender?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            email.value = props.modelValue.gender?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            mobilePhone.value = props.modelValue.mobilePhone?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            birthdate.value = props.modelValue.birthdate?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            address.value = props.modelValue.address?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            addressType.value = props.modelValue.addressType ?? "";
            maritalStatus.value = props.modelValue.maritalStatus?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            spouseEntry.value = props.modelValue.spouseEntry?.toString() ?? WorkflowActionFormPersonEntryOption.Hidden.toString();
            spouseLabel.value = props.modelValue.spouseLabel ?? "";
        });

        return {
            address,
            addressType,
            autofillCurrentPerson,
            birthdate,
            campusState,
            campusType,
            columnClass,
            connectionStatus,
            email,
            gender,
            hideIfCurrentPersonKnown,
            maritalStatus,
            mobilePhone,
            recordStatus,
            showCampus,
            spouseEntry,
            spouseLabel,
            triStateOptions
        };
    },

    template: `
<div>
    <div class="row">
        <div :class="columnClass">
            <CheckBox v-model="autofillCurrentPerson"
                label="Autofill Current Person" />
        </div>

        <div :class="columnClass">
            <CheckBox v-model="hideIfCurrentPersonKnown"
                label="Hide if Current Person Known" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="recordStatus"
                label="Record Status"
                :options="recordStatusOptions"
                rules="required" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="connectionStatus"
                label="Connection Status"
                :options="connectionStatusOptions"
                rules="required" />
        </div>

        <div :class="columnClass">
            <CheckBox v-model="showCampus"
                label="Show Campus" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="campusType"
                label="Campus Type"
                :options="campusTypeOptions" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="campusStatus"
                label="Campus Status"
                :options="campusStatusOptions" />
        </div>
    </div>

    <div class="row">
        <div :class="columnClass">
            <DropDownList v-model="gender"
                label="Gender"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="email"
                label="Email"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="mobilePhone"
                label="Mobile Phone"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="birthdate"
                label="Birthdate"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="address"
                label="Address"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="addressType"
                label="Address Type"
                :options="addressTypeOptions"
                rules="required" />
        </div>

        <div :class="columnClass">
            <DropDownList v-model="maritalStatus"
                label="Marital Status"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>
    </div>

    <div class="row">
        <div :class="columnClass">
            <DropDownList v-model="spouseEntry"
                label="Spouse Entry"
                :showBlankValue="false"
                :options="triStateOptions" />
        </div>

        <div :class="columnClass">
            <TextBox v-model="spouseLabel"
                label="Spouse Label" />
        </div>
    </div>
</div>
`
});
