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

import { Component, computed, defineComponent, ref, watch } from "vue";
import { Guid } from "../Util/guid";
import { ClientEditableAttributeValue, ListItem } from "../ViewModels";
import Alert from "../Elements/alert";
import DropDownList, { DropDownListOption } from "../Elements/dropDownList";
import RockField from "../Controls/rockField";
import { get, post } from "../Util/http";
import { FieldType as FieldTypeGuids } from "../SystemGuids";
import { DefinedValueFieldType } from "../Fields/definedValueField";

/**
 * Describes a field type configuration state. This provides the information
 * required to edit a field type on a remote system.
 */
type AttributeConfigurationViewModel = {
    /**
     * Gets or sets the configuration properties that contain information
     * describing a field type edit operation.
     */
    configurationProperties: Record<string, string>;

    /**
     * Gets or sets the configuration options that describe the current
     * selections when editing a field type.
     */
    configurationOptions: Record<string, string>;

    /**
     * Gets or sets the default attribute value view model that corresponds
     * to the current configurationOptions.
     */
    defaultValue: ClientEditableAttributeValue;
};

/**
 * Contains information required to update a field type's configuration.
 */
type AttributeConfigurationUpdateViewModel = {
    /** Gets or sets the field type unique identifier. */
    fieldTypeGuid: Guid;

    /**
     * Gets or sets the configuration options that describe the current
     * selections when editing a field type.
     */
    configurationOptions: Record<string, string>;

    /** Gets or sets the default value currently set. */
    defaultValue: string;
};

const definedValueField = new DefinedValueFieldType();

export default defineComponent({
    name: "AttributeEditor",

    components: {
        Alert,
        DropDownList,
        RockField
    },

    setup() {
        /** The selected field type in the drop down list. */
        const fieldTypeValue = ref("");

        /** The details about the default value used for the attribute. */
        const defaultValue = ref<ClientEditableAttributeValue | null>(null);

        /** True if the default value component should be shown. */
        const hasDefaultValue = computed((): boolean => showConfigurationComponent.value && defaultValue.value ? true : false);

        /** The current configuration properties that describe the field type options. */
        const configurationProperties = ref<Record<string, string>>({});

        /** The current options selected in the configuration properties. */
        const configurationOptions = ref<Record<string, string>>({});

        /** True if the field types options are ready for display. */
        const isFieldTypesReady = ref(false);

        /** True if the configuration data has been retrieved and is ready for display. */
        const isConfigurationReady = ref(false);

        /** True if everything is loaded and ready for display. */
        const isReady = computed(() => isFieldTypesReady.value && isConfigurationReady.value);

        /** Contains any error message to be displayed to the user about the operation. */
        const fieldErrorMessage = ref("");

        /** The options to be shown in the field type drop down control. */
        const fieldTypeOptions = ref<DropDownListOption[]>([]);

        /** The UI component that will handle the configuration of the field type. */
        const configurationComponent = computed((): Component | null => {
            if (fieldTypeValue.value === FieldTypeGuids.DefinedValue) {
                return definedValueField.getConfigurationComponent();
            }
            return null;
        });

        /** True if the configuration component is ready to be displayed. */
        const showConfigurationComponent = computed((): boolean => {
            return configurationComponent && isReady.value;
        });

        /** Updates the attribute configuration from new data on the server. */
        const updateAttributeConfiguration = (): void => {
            const update: AttributeConfigurationUpdateViewModel = {
                fieldTypeGuid: fieldTypeValue.value,
                configurationOptions: configurationOptions.value,
                defaultValue: ""
            };

            post<AttributeConfigurationViewModel>("/api/v2/Controls/AttributeEditor/attributeConfiguration", null, update)
                .then(result => {
                    if (result.isSuccess && result.data && result.data.configurationProperties && result.data.configurationOptions && result.data.defaultValue) {
                        fieldErrorMessage.value = "";
                        isConfigurationReady.value = true;
                        configurationProperties.value = result.data.configurationProperties;
                        configurationOptions.value = result.data.configurationOptions;
                        defaultValue.value = result.data.defaultValue;
                    }
                    else {
                        fieldErrorMessage.value = result.errorMessage ?? "Encountered unknown error communicating with server.";
                    }
                });
        };

        // Called when the field type drop down value is changed.
        watch(fieldTypeValue, () => {
            updateAttributeConfiguration();
        });

        /**
         * Called when the field type configuration control requests that the
         * configuration properties be updated from the server.
         */
        const onUpdateConfiguration = (): void => {
            console.log("updateConfiguration");
            updateAttributeConfiguration();
        };

        /**
         * Called when the field type configuration control has updated one of
         * the configuration values.
         * 
         * @param key The key of the configuration value that was changed.
         * @param value The new value of the configuration value.
         */
        const onUpdateConfigurationValue = (key: string, value: string): void => {
            console.log("updateConfigurationValue", configurationOptions.value);
            if (defaultValue.value?.configurationValues) {
                defaultValue.value.configurationValues[key] = value;
            }
        };

        // Get all the available field types that the user is allowed to edit.
        get<ListItem[]>("/api/v2/Controls/AttributeEditor/availableFieldTypes")
            .then(result => {
                if (result.isSuccess && result.data) {
                    fieldTypeOptions.value = result.data.map((item): DropDownListOption => {
                        return {
                            value: item.value,
                            text: item.text
                        };
                    });

                    isFieldTypesReady.value = true;
                }
            });

        return {
            configurationComponent,
            configurationOptions,
            configurationProperties,
            defaultValue,
            hasDefaultValue,
            fieldErrorMessage,
            fieldTypeOptions,
            fieldTypeValue,
            isFieldTypesReady,
            onUpdateConfiguration,
            onUpdateConfigurationValue,
            showConfigurationComponent
        };
    },

    template: `
<div>
    <DropDownList v-if="isFieldTypesReady" label="Field Type" v-model="fieldTypeValue" :options="fieldTypeOptions" rules="required" />
    <Alert v-if="fieldErrorMessage" alertType="warning">
        {{ fieldErrorMessage }}
    </Alert>
    <component v-if="showConfigurationComponent" :is="configurationComponent" v-model="configurationOptions" :configurationProperties="configurationProperties" @updateConfiguration="onUpdateConfiguration" @updateConfigurationValue="onUpdateConfigurationValue" />
    <RockField v-if="hasDefaultValue" :attributeValue="defaultValue" isEditMode />
</div>
`
});
