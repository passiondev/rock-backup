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
import PersonPicker from "../../../Controls/personPicker";
import DropDownList from "../../../Elements/dropDownList";
import EmailBox from "../../../Elements/emailBox";
import InlineSwitch from "../../../Elements/inlineSwitch";
import TextBox from "../../../Elements/textBox";
import TransitionVerticalCollapse from "../../../Elements/transitionVerticalCollapse";
import { toNumberOrNull } from "../../../Services/number";
import { ListItem } from "../../../ViewModels";
import EmailSource from "./emailSource";
import SegmentedPicker from "./segmentedPicker";
import { FormNotificationEmail, NotificationEmailDestination } from "./types";

const notificationDestinationOptions: ListItem[] = [
    {
        value: NotificationEmailDestination.SpecificIndividual.toString(),
        text: "Specific Individual"
    },
    {
        value: NotificationEmailDestination.EmailAddress.toString(),
        text: "Email Address"
    },
    {
        value: NotificationEmailDestination.CampusTopic.toString(),
        text: "Campus Topic Address"
    }
];

/**
 * Handles the UI for defining the Notification Email section of the Communications
 * tab.
 */
export default defineComponent({
    name: "Workflow.FormBuilderDetail.NotificationEmail",

    components: {
        DropDownList,
        EmailBox,
        EmailSource,
        InlineSwitch,
        PersonPicker,
        SegmentedPicker,
        TextBox,
        TransitionVerticalCollapse
    },

    props: {
        modelValue: {
            type: Object as PropType<FormNotificationEmail>,
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
        /** True if the notification e-mail is enabled and the rest of the UI should be shown. */
        const enabled = ref(props.modelValue.enabled ?? false);

        /** The currently selected destination option for where the e-mail will be sent. */
        const destination = ref(props.modelValue.destination?.toString() ?? NotificationEmailDestination.SpecificIndividual.toString());

        /** The recipient when destination is set to specific individual. */
        const recipient = ref(props.modelValue.recipient ?? null);

        /** The e-mail address(es) when destination is set to email address. */
        const emailAddress = ref(props.modelValue.emailAddress ?? "");

        /** The campus topic identifier when destination is set to campus topic. */
        const campusTopicGuid = ref(props.modelValue.campusTopicGuid ?? "");

        /** The source that will generate the e-mail to be sent. */
        const source = ref(props.modelValue.source ?? {});

        /** True if the selected destination is Specific Individual. */
        const isDestinationSpecificIndividual = computed((): boolean => destination.value === NotificationEmailDestination.SpecificIndividual.toString());

        /** True if the selected destination is Email Address. */
        const isDestinationEmailAddress = computed((): boolean => destination.value === NotificationEmailDestination.EmailAddress.toString());

        /** True if the selected destination is Campus Topic. */
        const isDestinationCampusTopic = computed((): boolean => destination.value === NotificationEmailDestination.CampusTopic.toString());

        // Watch for changes in the modelValue and update all our internal values.
        watch(() => props.modelValue, () => {
            enabled.value = props.modelValue.enabled ?? false;
            destination.value = props.modelValue.destination?.toString() ?? NotificationEmailDestination.SpecificIndividual.toString();
            recipient.value = props.modelValue.recipient ?? null;
            emailAddress.value = props.modelValue.emailAddress ?? "";
            campusTopicGuid.value = props.modelValue.campusTopicGuid ?? "";
            source.value = props.modelValue.source ?? {};
        });

        // Watch for changes in our internal values and update the modelValue.
        watch([enabled, destination, recipient, emailAddress, campusTopicGuid, source], () => {
            const newValue: FormNotificationEmail = {
                ...props.modelValue,
                enabled: enabled.value,
                destination: toNumberOrNull(destination.value) ?? NotificationEmailDestination.SpecificIndividual,
                recipient: props.modelValue.recipient,
                emailAddress: props.modelValue.emailAddress,
                campusTopicGuid: props.modelValue.campusTopicGuid,
                source: source.value
            };

            emit("update:modelValue", newValue);
        });

        return {
            campusTopicGuid,
            destination,
            destinationOptions: notificationDestinationOptions,
            emailAddress,
            enabled,
            isDestinationSpecificIndividual,
            isDestinationEmailAddress,
            isDestinationCampusTopic,
            recipient,
            source
        };
    },

    template: `
<div class="well">
    <div class="d-flex">
        <div style="flex-grow: 1;">
            <h3>Notification Email</h3>
            <p>
                Notification emails can be sent to specified individuals when each form is completed.
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
                <SegmentedPicker v-model="destination" :options="destinationOptions" />

                <div v-if="isDestinationSpecificIndividual">
                    <div class="row">
                        <div class="col-md-4">
                            <PersonPicker v-model="recipient"
                                label="Recipient"
                                rules="required" />
                        </div>
                    </div>
                </div>

                <div v-else-if="isDestinationEmailAddress">
                    <div class="row">
                        <div class="col-md-4">
                            <EmailBox v-model="emailAddress"
                                label="Recipient(s)"
                                rules="required"
                                allowMultiple />
                        </div>
                    </div>
                </div>

                <div v-else-if="isDestinationCampusTopic">
                    <div class="row">
                        <div class="col-md-4">
                            <DropDownList v-model="campusTopicGuid"
                                label="Topic"
                                rules="required"
                                :options="campusTopicOptions" />
                        </div>
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
