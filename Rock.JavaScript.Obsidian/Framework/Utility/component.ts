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

import { AsyncComponentLoader, Component, ComponentPublicInstance, defineAsyncComponent as vueDefineAsyncComponent, ExtractPropTypes, PropType, reactive, ref, Ref, watch, WatchOptions } from "vue";
import { deepEqual } from "./util";
import { useSuspense } from "./suspense";
import { newGuid } from "./guid";
import { ControlLazyMode, ControlLazyModeType } from "@Obsidian/Types/Controls/controlLazyMode";
import type { RulesPropType, ValidationRule } from "@Obsidian/Types/validationRules";
import { containsRequiredRule } from "./validationRules";

type Prop = { [key: string]: unknown };
type PropKey<T extends Prop> = Extract<keyof T, string>;
// eslint-disable-next-line @typescript-eslint/no-explicit-any
type EmitFn<E extends `update:${string}`> = E extends Array<infer EE> ? (event: EE, ...args: any[]) => void : (event: E, ...args: any[]) => void;

/**
 * Utility function for when you are using a component that takes a v-model
 * and uses that model as a v-model in that component's template. It creates
 * a new ref that keeps itself up-to-date with the given model and fires an
 * 'update:MODELNAME' event when it gets changed.
 *
 * Ensure the related `props` and `emits` are specified to ensure there are
 * no type issues.
 */
export function useVModelPassthrough<T extends Prop, K extends PropKey<T>, E extends `update:${K}`>(props: T, modelName: K, emit: EmitFn<E>, options?: WatchOptions): Ref<T[K]> {
    const internalValue = ref(props[modelName]) as Ref<T[K]>;

    watch(() => props[modelName], val => internalValue.value = val, options);
    watch(internalValue, val => emit(`update:${modelName}`, val), options);

    return internalValue;
}

/**
 * Updates the Ref value, but only if the new value is actually different than
 * the current value. A strict comparison is performed.
 * 
 * @param target The target Ref object to be updated.
 * @param value The new value to be assigned to the target.
 *
 * @returns True if the target was updated, otherwise false.
 */
export function updateRefValue<T>(target: Ref<T>, value: T): boolean {
    if (deepEqual(target.value, value, true)) {
        return false;
    }

    target.value = value;

    return true;
}

/**
 * Defines a component that will be loaded asynchronously. This contains logic
 * to properly work with the RockSuspense control.
 * 
 * @param source The function to call to load the component.
 *
 * @retunrs The component that was loaded.
 */
export function defineAsyncComponent<T extends Component = { new(): ComponentPublicInstance }>(source: AsyncComponentLoader<T>): T {
    return vueDefineAsyncComponent(async () => {
        const suspense = useSuspense();
        const operationKey = newGuid();

        suspense?.startAsyncOperation(operationKey);
        const component = await source();
        suspense?.completeAsyncOperation(operationKey);

        return component;
    });
}

// #region Standard Form Field

type StandardRockFormFieldProps = {
    label: {
        type: PropType<string>,
        default: ""
    },

    help: {
        type: PropType<string>,
        default: ""
    },

    rules: RulesPropType,

    formGroupClasses: {
        type: PropType<string>,
        default: ""
    },

    validationTitle: {
        type: PropType<string>,
        default: ""
    }
};

/** The standard component props that should be included when using RockFormField. */
export const standardRockFormFieldProps: StandardRockFormFieldProps = {
    label: {
        type: String as PropType<string>,
        default: ""
    },

    help: {
        type: String as PropType<string>,
        default: ""
    },

    rules: {
        type: [Array, Object, String] as PropType<ValidationRule | ValidationRule[]>,
        default: ""
    },

    formGroupClasses: {
        type: String as PropType<string>,
        default: ""
    },

    validationTitle: {
        type: String as PropType<string>,
        default: ""
    }
};

/**
 * Copies the known properties for the standard rock form field props from
 * the source object to the destination object.
 * 
 * @param source The source object to copy the values from.
 * @param destination The destination object to copy the values to.
 */
function copyStandardRockFormFieldProps(source: ExtractPropTypes<StandardRockFormFieldProps>, destination: ExtractPropTypes<StandardRockFormFieldProps>): void {
    destination.formGroupClasses = source.formGroupClasses;
    destination.help = source.help;
    destination.label = source.label;
    destination.rules = source.rules;
    destination.validationTitle = source.validationTitle;
}

/**
 * Configures the basic properties that should be passed to the RockFormField
 * component. The value returned by this function should be used with v-bind on
 * the RockFormField in order to pass all the defined prop values to it.
 * 
 * @param props The props of the component that will be using the RockFormField.
 *
 * @returns An object of prop values that can be used with v-bind.
 */
export function useStandardRockFormFieldProps(props: ExtractPropTypes<StandardRockFormFieldProps>): ExtractPropTypes<StandardRockFormFieldProps> {
    const propValues = reactive<ExtractPropTypes<StandardRockFormFieldProps>>({
        label: props.label,
        help: props.help,
        rules: props.rules,
        formGroupClasses: props.formGroupClasses,
        validationTitle: props.validationTitle
    });

    watch([() => props.formGroupClasses, () => props.help, () => props.label, () => props.rules, () => props.validationTitle], () => {
        copyStandardRockFormFieldProps(props, propValues);
    });

    return propValues;
}

// #endregion

// #region Standard Async Pickers

type StandardAsyncPickerProps = StandardRockFormFieldProps & {
    enhanceForLongLists: {
        type: PropType<boolean>,
        default: false
    },

    lazyMode: {
        type: PropType<ControlLazyModeType>,
        default: ControlLazyMode.OnDemand
    },

    multiple: {
        type: PropType<boolean>,
        default: false
    },

    showBlankItem: {
        type: PropType<boolean>,
        default: false
    }
};

/** The standard component props that should be included when using BaseAsyncPicker. */
export const standardAsyncPickerProps: StandardAsyncPickerProps = {
    ...standardRockFormFieldProps,

    enhanceForLongLists: {
        type: Boolean as PropType<boolean>,
        default: false
    },

    lazyMode: {
        type: String as PropType<ControlLazyModeType>,
        default: ControlLazyMode.OnDemand
    },

    multiple: {
        type: Boolean as PropType<boolean>,
        default: false
    },

    showBlankItem: {
        type: Boolean as PropType<boolean>,
        default: false
    }
};

/**
 * Copies the known properties for the standard async picker props from
 * the source object to the destination object.
 * 
 * @param source The source object to copy the values from.
 * @param destination The destination object to copy the values to.
 */
function copyStandardAsyncPickerProps(source: ExtractPropTypes<StandardAsyncPickerProps>, destination: ExtractPropTypes<StandardAsyncPickerProps>): void {
    copyStandardRockFormFieldProps(source, destination);

    destination.enhanceForLongLists = source.enhanceForLongLists;
    destination.lazyMode = source.lazyMode;
    destination.multiple = source.multiple;
    destination.showBlankItem = source.showBlankItem || !containsRequiredRule(source.rules);
}

/**
 * Configures the basic properties that should be passed to the BaseAsyncPicker
 * component. The value returned by this function should be used with v-bind on
 * the BaseAsyncPicker in order to pass all the defined prop values to it.
 * 
 * @param props The props of the component that will be using the BaseAsyncPicker.
 *
 * @returns An object of prop values that can be used with v-bind.
 */
export function useStandardAsyncPickerProps(props: ExtractPropTypes<StandardAsyncPickerProps>): ExtractPropTypes<StandardAsyncPickerProps> {
    const standardFieldProps = useStandardRockFormFieldProps(props);

    const propValues = reactive<ExtractPropTypes<StandardAsyncPickerProps>>({
        ...standardFieldProps,
        enhanceForLongLists: props.enhanceForLongLists,
        lazyMode: props.lazyMode,
        multiple: props.multiple,
        showBlankItem: props.showBlankItem || !containsRequiredRule(props.rules)
    });

    // Watch for changes in any of the standard props. Use deep for this so we
    // don't need to know which prop keys it actually contains.
    watch(() => standardFieldProps, () => {
        copyStandardRockFormFieldProps(props, propValues);
    }, {
        deep: true
    });

    // Watch for changes in our known list of props that might change.
    watch([() => props.enhanceForLongLists, () => props.lazyMode, () => props.multiple, () => props.showBlankItem], () => {
        copyStandardAsyncPickerProps(props, propValues);
    });

    return propValues;
}

// #endregion
