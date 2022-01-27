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
import RockForm from "../../../Controls/rockForm";
import Alert from "../../../Elements/alert";
import RockButton from "../../../Elements/rockButton";
import ConfirmationEmail from "./confirmationEmail";
import NotificationEmail from "./notificationEmail";
import { FormCommunication } from "./types";

export default defineComponent({
    name: "Workflow.FormBuilderDetail.CommunicationsTab",

    components: {
        Alert,
        ConfirmationEmail,
        NotificationEmail,
        RockForm,
        RockButton
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

            <NotificationEmail v-if="!notificationEmailForced" v-model="notificationEmail" />
            <Alert v-else alertType="info">
                <h4 class="alert-heading">Notification Email</h4>
                <p>
                    The notification e-mail is defined on the template and cannot be changed.
                </p>
            </Alert>

            <RockButton type="submit" btnType="primary">Submit</RockButton>
        </RockForm>
    </div>
</div>
`
});
