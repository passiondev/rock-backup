import { defineComponent, PropType, ref, watch, computed, defineAsyncComponent } from "vue";
import DropDownList from "../Elements/dropDownList";
import TextBox from "../Elements/textBox";
import { useVModelPassthrough } from "../Util/component";
import { newGuid } from "../Util/guid";

import { DateFieldType } from "../Fields/dateField";
import { IntegerFieldType } from "../Fields/integerField";
import { TextFieldType } from "../Fields/textField";
import { string } from "yup";

export type FieldVisibilityRuleItem = {
    guid?: string,
    comparedToFormFieldGuid?: string,
    comparisonType?: number,
    comparedToValue?: string
};

export type FieldVisibilityRuleAttributeOption = {
    name: string,
    comparators: string[],
    type: string,
    componentProps: Record<string, unknown>
};

const allComparisonTypes: Record<string, number> = {
    "Equal To": 0x1,
    "Not Equal To": 0x2,
    "Contains": 0x8,
    "Does Not Contain": 0x10,
    "Is Blank": 0x20,
    "Is Not Blank": 0x40,
    "Greater Than": 0x80,
    "Greater Than Or Equal To": 0x100,
    "Less Than": 0x200,
    "Less Than Or Equal To": 0x400,
    "Starts With": 0x4,
    "Ends With": 0x800,
    "Between": 0x1000,
    "Regular Expression": 0x2000,
};

export const FieldVisibilityRuleItemComponent = defineComponent({
    name: "FieldVisibilityRuleItemComponent",

    components: {
        DropDownList,
        TextBox
    },

    props: {
        modelValue: {
            type: Object as PropType<FieldVisibilityRuleItem>,
            required: true
        },
        attributeOptions: {
            type: Object as PropType<Record<string, FieldVisibilityRuleAttributeOption>>,
            required: true
        }
    },

    emits: [
        "update:modelValue",
        "removeRule"
    ],

    setup(props, { emit }) {
        const rule = useVModelPassthrough(props, "modelValue", emit);

        //#region GUID
        rule.value.guid = rule.value.guid ?? newGuid();
        //#endregion

        //#region Field List
        // This will need to be dynamically loaded from the server or from a parent component that already has them
        const fieldList = computed(() => {
            return Object.keys(props.attributeOptions).map(key => {
                return {
                    text: props.attributeOptions[key].name,
                    value: key
                };
            });
        });

        if (!rule.value.comparedToFormFieldGuid) {
            rule.value.comparedToFormFieldGuid = fieldList.value[0].value;
        }
        //#endregion

        // Current Selected Attribute/Property
        const currentAttribute = computed<FieldVisibilityRuleAttributeOption>(() => {
            return props.attributeOptions[rule.value.comparedToFormFieldGuid as string];
        });

        //#region Comparison Types
        const comparisonTypes = computed(() => {
            return currentAttribute.value.comparators.map(text => {
                return {
                    text,
                    value: allComparisonTypes[text]
                };
            })
        });

        if (!rule.value.comparisonType) {
            rule.value.comparisonType = comparisonTypes.value[0].value;
        }
        //#endregion

        //#region Comparison Value
        rule.value.comparedToValue = rule.value.comparedToValue ?? '';
        //#endregion

        const isFieldLoaded = ref(false);
        const fieldComponent = computed(() => {
            const fieldType = currentAttribute.value.type.toLowerCase();
            isFieldLoaded.value = false;

            return defineAsyncComponent(async () => {
                try {
                    // console.log('start import')
                    const components = await import(`../Fields/${fieldType}FieldComponents`);
                    // console.log('import finished')
                    
                    if (components && components.EditComponent) {
                        // console.log("is edit component")
                        isFieldLoaded.value = true;
                        return components.EditComponent;
                    }
                } catch(e) {}

                // console.log('component catchall')
                isFieldLoaded.value = true;
                return defineComponent({
                    template: `<div class="mt-2">Error: Failed to load the form field</div>`
                });
            });
        });

        function removeRule(): void {
            emit("removeRule", props.modelValue);
        }

        return {
            removeRule,
            fieldComponent,
            rule,
            fieldList,
            comparisonTypes,
            isFieldLoaded
        };
    },

    template: `
<div class="filter-rule row form-row">
    <div class="col-xs-10 col-sm-11">
        <div class="row form-row">
            <div class="filter-rule-comparefield col-md-4">
                <DropDownList :options="fieldList" v-model="rule.comparedToFormFieldGuid" />
            </div>
            <div class="filter-rule-fieldfilter col-md-8">
                <div class="row form-row field-criteria">
                    <div class="col-md-4" v-if="isFieldLoaded">
                        <DropDownList :options="comparisonTypes" v-model="rule.comparisonType" />
                    </div>
                    <div class="col-md-8">
                        <Suspense timeout="100">
                            <component :is="fieldComponent" v-bind="currentAttribute?.componentProps" v-model="rule.comparedToValue" />
                            <template #fallback>
                                <div class="mt-2">Loading...</div>
                            </template>
                        </Suspense>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xs-2 col-sm-1">
        <button class="btn btn-danger btn-square" @click.prevent="removeRule"><i class="fa fa-times"></i></button>
    </div>
</div>`
});
