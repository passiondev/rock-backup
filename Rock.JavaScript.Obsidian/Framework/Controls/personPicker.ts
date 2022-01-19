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

import { computed, defineComponent, PropType, Ref, ref, watch } from "vue";
import { ListItem } from "../ViewModels";
import RockFormField from "../Elements/rockFormField";
import Panel from "./panel";
import TextBox from "../Elements/textBox";
import { nextTick } from "vue";
import { doApiCall } from "../Util/http";

type PersonSearchResult = {
    guid?: string | null;

    name?: string | null;

    isActive?: boolean;

    isDeceased?: boolean;

    imageUrl?: string | null;

    age?: number | null;

    formattedAge?: string | null;

    ageClassification?: number;

    gender?: string | null;

    connectionStatus?: string | null;

    spouseName?: string | null;

    spouseNickName?: string | null;
};

export default defineComponent({
    name: "PersonPicker",

    components: {
        RockFormField,
        Panel,
        TextBox
    },

    props: {
        modelValue: {
            type: Object as PropType<ListItem>
        }
    },

    setup(props, { emit }) {
        const internalValue = ref(props.modelValue);

        /** Determines if the clear button should be shown. */
        const showClear = computed(() => props.modelValue?.value);

        /** True if the popup person picker should be visible. */
        const showPopup = ref(false);

        const searchPanel = ref<HTMLElement | null>(null);

        const panelTabIndex = computed((): string | undefined => {
            return showPopup.value ? "0" : undefined;
        });

        const searchText = ref("");

        const searchResults = ref<PersonSearchResult[]>([]);
        const selectedSearchResult = ref("");

        let searchCancelToken: Ref<boolean> | null = null;

        /**
         * Event handler for when the clear button is clicked by the user.
         */
        const onClear = (): void => {
            emit("update:modelValue", undefined);
        };

        /**
         * Event handler for when the user clicks on the picker. Show/hide the
         * popup.
         */
        const onPickerClick = (): void => {
            showPopup.value = !showPopup.value;

            if (showPopup.value) {
                nextTick(() => {
                    if (searchPanel.value) {
                        searchPanel.value.focus();
                    }
                });
            }
        };

        /**
         * Event handler for when the user clicks the cancel button. Hide the
         * popup.
         */
        const onCancel = (): void => {
            showPopup.value = false;
        };

        /**
         * Event handler for when the user clicks the select button. Save the
         * current selection and close the popup.
         */
        const onSelect = (): void => {
            const newModelValue = {};

            // Emit the new value and close the popup.
            emit("update:modelValue", newModelValue);
            showPopup.value = false;
        };

        const onKeyDown = (ev: KeyboardEvent): void => {
            if (ev.keyCode === 27 && showPopup.value) {
                onCancel();
            }
        };

        const updateSearchResults = async (cancellationToken: Ref<boolean>, text: string): Promise<void> => {
            if (text.length < 3) {
                return;
            }

            const params = {
                name: text,
                includeDetails: true
            };

            const result = await doApiCall<PersonSearchResult[]>("POST", "/api/v2/Controls/PersonPicker/Search", undefined, params);

            if (cancellationToken.value) {
                return;
            }

            if (result.isSuccess && result.data) {
                searchResults.value = result.data;
                selectedSearchResult.value = "";
            }
            else {
                console.warn(result.errorMessage);
            }
        };

        const getNameAdditionalText = (result: PersonSearchResult): string => {
            if (result.spouseNickName && result.formattedAge) {
                return `Age: ${result.formattedAge}; Spouse: ${result.spouseNickName}`;
            }
            else if (result.formattedAge) {
                return `Age: ${result.formattedAge}`;
            }
            else if (result.spouseNickName) {
                return `Spouse: ${result.spouseNickName}`;
            }
            else {
                return "";
            }
        };

        watch(searchText, () => {
            if (searchCancelToken) {
                searchCancelToken.value = true;
            }

            searchCancelToken = ref(false);

            updateSearchResults(searchCancelToken, searchText.value);
        });


        watch(() => props.modelValue, () => internalValue.value = props.modelValue);

        return {
            getNameAdditionalText,
            internalValue,
            onClear,
            onPickerClick,
            onCancel,
            onKeyDown,
            onSelect,
            panelTabIndex,
            searchPanel,
            searchResults,
            searchText,
            selectedSearchResult,
            showClear,
            showPopup
        };
    },

    template: `
<RockFormField
    :modelValue="internalValue"
    formGroupClasses="person-picker"
    name="personpicker">
    <template #default="{uniqueId, field}">
        <div class="control-wrapper">
            <div class="picker picker-select person-picker">
                <a class="picker-label" href="#" @click.prevent.stop="onPickerClick">
                    <i class="fa fa-user fa-fw"></i>
                    <span class="selected-names" v-text="selectedNames"></span>
                    <i class="fa fa-caret-down pull-right"></i>
                </a>

                <a v-if="showClear" class="picker-select-none" @click.prevent.stop="onClear">
                    <i class="fa fa-times"></i>
                </a>

                <Panel v-if="showPopup" isFullscreen title="Person Search">
                    <template #actionAside>
                        <span class="panel-action" @click.prevent.stop="onCancel">
                            <i class="fa fa-times"></i>
                        </span>
                    </template>

                    <div ref="searchPanel" @keydown.stop="onKeyDown" :tabindex="panelTabIndex">
                        <TextBox v-model="searchText" label="Search" />

                        <div style="display: flex;">
                            <div v-for="result in searchResults" :key="result.guid" class="well">
                                <div>
                                    {{ result.name }}
                                    <small v-if="getNameAdditionalText(result)" class="text-muted">{{ getNameAdditionalText(result) }}</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </Panel>
            </div>
        </div>
    </template>
</RockFormField>
`
});
