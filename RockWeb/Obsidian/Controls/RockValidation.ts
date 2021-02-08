﻿// <copyright>
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
import Alert from '../Elements/Alert.js';
import { defineComponent, PropType } from '../Vendor/Vue/vue.js';

export default defineComponent({
    name: 'RockValidation',
    components: {
        Alert
    },
    props: {
        errors: {
            type: Object as PropType<Record<string, string>>,
            required: true
        }
    },
    computed: {
        hasErrors(): boolean {
            return Object.keys(this.errors).length > 0;
        }
    },
    template: `
<Alert v-if="hasErrors" alertType="validation">
    Please correct the following:
    <ul>
        <li v-for="(error, fieldLabel) of errors">
            <strong>{{fieldLabel}}</strong>
            {{error}}
        </li>
    </ul>
</Alert>`
});