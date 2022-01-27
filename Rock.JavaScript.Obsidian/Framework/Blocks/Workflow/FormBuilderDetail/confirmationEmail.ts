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
import DropDownList from "../../../Elements/dropDownList";
import InlineSwitch from "../../../Elements/inlineSwitch";
import TransitionVerticalCollapse from "../../../Elements/transitionVerticalCollapse";
import { ListItem } from "../../../ViewModels";
import EmailSource from "./emailSource";
import { FormConfirmationEmail, FormEmailSource } from "./types";

/**
 * Displays the UI for the Confirmation Email component in the Communications
 * screen.
 */
export default defineComponent({
    name: "Workflow.FormBuilderDetail.ConfirmationEmail",

    components: {
        DropDownList,
        EmailSource,
        InlineSwitch,
        TransitionVerticalCollapse
    },

    props: {
        modelValue: {
            type: Object as PropType<FormConfirmationEmail>,
            required: true
        },

        recipientOptions: {
            type: Array as PropType<ListItem[]>,
            default: []
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        /** True if the confirmation e-mail is enabled and the rest of the UI should be shown. */
        const enabled = ref(props.modelValue.enabled ?? false);

        /** The unique identifier of the attribute used for the reciepient. */
        const recipientAttributeGuid = ref(props.modelValue.recipientAttributeGuid ?? null);

        /** The e-mail source information on how the e-mail is generated. */
        const source = ref<FormEmailSource>(props.modelValue.source ?? {});

        // Watch for changes in our modelValue and then update all our internal values.
        watch(() => props.modelValue, () => {
            enabled.value = props.modelValue.enabled ?? false;
            recipientAttributeGuid.value = props.modelValue.recipientAttributeGuid ?? null;
            source.value = props.modelValue.source ?? {};
        });

        // Watch for changes on any of our internal values and then update the modelValue.
        watch([enabled, recipientAttributeGuid, source], () => {
            const newValue: FormConfirmationEmail = {
                ...props.modelValue,
                enabled: enabled.value,
                recipientAttributeGuid: recipientAttributeGuid.value,
                source: source.value
            };

            emit("update:modelValue", newValue);
        });

        return {
            enabled,
            recipientAttributeGuid,
            source
        };
    },

    template: `
<div class="well">
    <div class="d-flex">
        <div style="flex-grow: 1;">
            <h3>Confirmation Email</h3>
            <p>
                The following settings will be used to send an email to the individual who submitted the form.
            </p>
        </div>

        <div style="align-self: end;">
            <InlineSwitch v-model="enabled" label="Enable" />
        </div>
    </div>

    <TransitionVerticalCollapse>
        <div v-if="enabled">
            <hr />

            <div style="margin: 12px;">
                <div class="row">
                    <div class="col-md-4">
                        <DropDownList v-model="recipientAttributeGuid"
                            label="Recipient"
                            rules="required"
                            :options="recipientOptions" />
                    </div>
                </div>

                <div class="mt-3">
                    <EmailSource v-model="source" />
                </div>
            </div>
        </div>
    </TransitionVerticalCollapse>
</div>
`
});
