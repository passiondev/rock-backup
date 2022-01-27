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
import Alert from "../../../Elements/alert";
import DropDownList from "../../../Elements/dropDownList";
import InlineCheckbox from "../../../Elements/inlineCheckBox";
import InlineSwitch from "../../../Elements/inlineSwitch";
import RockButton from "../../../Elements/rockButton";
import RockForm from "../../../Controls/rockForm";
import TextBox from "../../../Elements/textBox";
import TransitionVerticalCollapse from "../../../Elements/transitionVerticalCollapse";
import { ListItem } from "../../../ViewModels";
import { useVModelPassthrough } from "../../../Util/component";
import { Guid } from "../../../Util/guid";

export const SegmentedPicker = defineComponent({
    name: "SegmentedPicker",

    props: {
        modelValue: {
            type: String as PropType<string>,
            default: ""
        },

        options: {
            type: Array as PropType<ListItem[]>,
            default: []
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        const internalValue = useVModelPassthrough(props, "modelValue", emit);

        const getButtonClass = (item: ListItem): string[] => {
            return ["btn", item.value === internalValue.value ? "btn-primary" : "btn-default"];
        };

        const onItemClick = (item: ListItem): void => {
            internalValue.value = item.value;
        };

        return {
            getButtonClass,
            internalValue,
            onItemClick
        };
    },

    template: `
<div class="btn-group" role="group">
    <button v-for="item in options" :class="getButtonClass(item)" :key="item.value" type="button" @click="onItemClick(item)">{{ item.text }}</button>
</div>
`
});

const enum FormEmailSourceType {
    UseTemplate = 0,

    Custom = 1
}

type FormConfirmationEmail = {
    enabled?: boolean;

    recipientAttributeGuid?: ListItem | null;

    source?: FormEmailSource | null;
};

type FormEmailSource = {
    type?: FormEmailSourceType;

    template?: ListItem | null;

    subject?: string | null;

    replyTo?: string | null;

    body?: string | null;

    appendOrgHeaderAndFooter?: boolean;
};

const enum NotificationEmailDestination {
    SpecificIndividual = 0,

    EmailAddress = 1,

    CampusTopic = 2
}

type FormNotificationEmail= {
    enabled?: boolean;

    destination?: NotificationEmailDestination;

    recipientGuid?: Guid | null;

    emailAddress?: string | null;

    campusTopicGuid?: Guid | null;

    source?: FormEmailSource;

    subject?: string | null;

    replyTo?: string | null;

    body?: string | null;

    appendOrgHeaderAndFooter?: boolean;
};

type FormCommunication = {
    confirmationEmail?: FormConfirmationEmail;

    notificationEmail?: FormNotificationEmail;
};

const confirmationEmailOptions: ListItem[] = [
    {
        value: "0",
        text: "Use Email Template"
    },
    {
        value: "1",
        text: "Provide Custom Email"
    }
];

export const EmailSource = defineComponent({
    name: "Workflow.FormBuilderDetail.EmailSource",

    components: {
        DropDownList,
        InlineCheckbox,
        SegmentedPicker,
        TextBox
    },

    props: {
        modelValue: {
            type: Object as PropType<FormEmailSource>,
            default: {}
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        const type = ref(props.modelValue.type?.toString() ?? "0");

        const template = ref(props.modelValue.template ?? "");

        const subject = ref(props.modelValue.subject ?? "");

        const replyTo = ref(props.modelValue.replyTo ?? "");

        const body = ref(props.modelValue.body ?? "");

        return {
            confirmationEmailOptions
        };
    },

    template: `
<div>
    <SegmentedPicker v-model="type" :options="confirmationEmailOptions" />
</div>
`
});

export const ConfirmationEmail = defineComponent({
    name: "Workflow.FormBuilderDetail.ConfirmationEmail",

    components: {
        DropDownList,
        InlineSwitch,
        SegmentedPicker,
        TextBox,
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
        const enabled = ref(props.modelValue.enabled ?? false);

        const recipientAttributeGuid = ref(props.modelValue.recipientAttributeGuid ?? null);

        const source = ref<FormEmailSource>(props.modelValue.source ?? {});

        watch(() => props.modelValue, () => {
            enabled.value = props.modelValue.enabled ?? false;
            recipientAttributeGuid.value = props.modelValue.recipientAttributeGuid ?? null;
            source.value = props.modelValue.source ?? {};
        });

        watch([enabled, recipientAttributeGuid, source], () => {
            const newValue: FormConfirmationEmail = {
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

            </div>
        </div>
    </TransitionVerticalCollapse>
</div>
`
});

export default defineComponent({
    name: "Workflow.FormBuilderDetail.CommunicationsTab",

    components: {
        Alert,
        ConfirmationEmail,
        DropDownList,
        InlineSwitch,
        RockForm,
        RockButton,
        SegmentedPicker
    },

    props: {
        modelValue: {
            type: Object as PropType<FormCommunication>,
            required: true
        },

        confirmationEmailForced: {
            type: Boolean as PropType<boolean>,
            default: false
        },

        notificationEmailForced: {
            type: Boolean as PropType<boolean>,
            default: false
        }
    },

    emits: [
        "update:modelValue"
    ],

    setup(props, { emit }) {
        const confirmationEmail = ref(props.modelValue.confirmationEmail ?? {});

        const notificationEmail = ref(props.modelValue.notificationEmail ?? {});

        watch(() => props.modelValue, () => {
            confirmationEmail.value = props.modelValue.confirmationEmail ?? {};
            notificationEmail.value = props.modelValue.notificationEmail ?? {};
        });

        watch([confirmationEmail, notificationEmail], () => {
            const newValue: FormCommunication = {
                ...props.modelValue,
                confirmationEmail: confirmationEmail.value,
                notificationEmail: notificationEmail.value
            };

            emit("update:modelValue", newValue);
        });

        return {
            confirmationEmail,
            notificationEmail,
            onSubmit: () => alert("save")
        };
    },

    template: `
<div class="d-flex flex-column" style="flex-grow: 1; overflow-y: auto;">
    <div class="panel-body">
        <RockForm @submit="onSubmit">

            <ConfirmationEmail v-if="!confirmationEmailForced" v-model="confirmationEmail" />
            <Alert v-else alertType="info">
                <h4 class="alert-heading">Confirmation Email</h4>
                <p>
                    The confirmation e-mail is defined on the template and cannot be changed.
                </p>
            </Alert>

            <div class="well">
                <div class="d-flex">
                    <div style="flex-grow: 1;">
                        <h3>Notification Email</h3>
                        <p>
                            Notification emails can be sent to specified individuals when each form is completed.
                        </p>
                    </div>

                    <div style="align-self: end;">
                        <InlineSwitch v-model="hasNotificationEmail" label="Enable" />
                    </div>
                </div>

                <hr />

                <div v-if="hasNotificationEmail" style="margin: 12px;">
                    <div class="row">
                        <div class="col-md-4">
                            <DropDownList label="Recipient" rules="required" />
                        </div>
                    </div>
                </div>
            </div>

            <RockButton type="submit" btnType="primary">Submit</RockButton>
        </RockForm>
    </div>
</div>
`
});
