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

import { defineComponent, inject } from 'vue';
import Alert from '../../../Elements/Alert';
import NumberUpDown from '../../../Elements/NumberUpDown';
import RockButton from '../../../Elements/RockButton';
import { toTitleCase, pluralConditional } from '../../../Services/String';
import { getDefaultRegistrantInfo, RegistrationEntryState } from '../RegistrationEntry';
import { RegistrationEntryBlockViewModel } from './RegistrationEntryBlockViewModel';

export default defineComponent( {
    name: 'Event.RegistrationEntry.Intro',
    components: {
        NumberUpDown,
        RockButton,
        Alert
    },
    data()
    {
        const registrationEntryState = inject( 'registrationEntryState' ) as RegistrationEntryState;

        return {
            /** The number of registrants that this registrar is going to input */
            numberOfRegistrants: registrationEntryState.Registrants.length || 1,

            /** The shared state among all the components that make up this block */
            registrationEntryState,

            /** Should the remaining capacity warning be shown? */
            showRemainingCapacity: false
        };
    },
    computed: {
        /** The view model sent by the C# code behind. This is just a convenient shortcut to the shared object. */
        viewModel(): RegistrationEntryBlockViewModel
        {
            return this.registrationEntryState.ViewModel;
        },

        /** The number of these registrants that will be placed on a waitlist because of capacity rules */
        numberToAddToWaitlist(): number
        {
            if ( this.viewModel.SpotsRemaining === null || !this.viewModel.WaitListEnabled )
            {
                // There is no waitlist or no cap on number of attendees
                return 0;
            }

            if ( this.viewModel.SpotsRemaining >= this.numberOfRegistrants )
            {
                // There is enough capacity left for all of these registrants
                return 0;
            }

            // Some or all need to go on the waitlist
            return this.numberOfRegistrants - this.viewModel.SpotsRemaining;
        },

        /** The capacity left phrase: Ex: 1 more camper */
        remainingCapacityPhrase(): string
        {
            const spots = this.viewModel.SpotsRemaining;

            if ( spots === null )
            {
                return '';
            }

            return pluralConditional( spots, `1 more ${this.registrantTerm}`, `${spots} more ${this.registrantTermPlural}` );
        },

        /** Is this instance full and no one else can register? */
        isFull(): boolean
        {
            if ( this.viewModel.SpotsRemaining === null )
            {
                return false;
            }

            return this.viewModel.SpotsRemaining < 1;
        },

        registrantTerm(): string
        {
            this.viewModel.InstanceName;
            return ( this.viewModel.RegistrantTerm || 'registrant' ).toLowerCase();
        },
        registrantTermPlural(): string
        {
            return ( this.viewModel.PluralRegistrantTerm || 'registrants' ).toLowerCase();
        },
        registrationTerm(): string
        {
            return ( this.viewModel.RegistrationTerm || 'registration' ).toLowerCase();
        },
        registrationTermPlural(): string
        {
            return ( this.viewModel.PluralRegistrationTerm || 'registrations' ).toLowerCase();
        },
        registrationTermTitleCase(): string
        {
            return toTitleCase( this.registrationTerm );
        }
    },
    methods: {
        pluralConditional,
        onNext()
        {
            // Resize the registrant array to match the selected number
            while ( this.numberOfRegistrants > this.registrationEntryState.Registrants.length )
            {
                const registrant = getDefaultRegistrantInfo();
                this.registrationEntryState.Registrants.push( registrant );
            }

            this.registrationEntryState.Registrants.length = this.numberOfRegistrants;
            const firstWaitListIndex = this.numberOfRegistrants - this.numberToAddToWaitlist;

            for ( let i = firstWaitListIndex; i < this.numberOfRegistrants; i++ )
            {
                this.registrationEntryState.Registrants[ i ].IsOnWaitList = true;
            }

            this.$emit( 'next' );
        },
    },
    watch: {
        numberOfRegistrants()
        {
            if ( !this.viewModel.WaitListEnabled && this.viewModel.SpotsRemaining !== null && this.viewModel.SpotsRemaining < this.numberOfRegistrants )
            {
                this.showRemainingCapacity = true;
                const spotsRemaining = this.viewModel.SpotsRemaining;

                // Do this on the next tick to allow the events to finish. Otherwise the component tree doesn't have time
                // to respond to this, since the watch was triggered by the numberOfRegistrants change
                this.$nextTick( () => this.numberOfRegistrants = spotsRemaining );
            }
        }
    },
    template: `
<div class="registrationentry-intro">
    <Alert v-if="isFull" class="text-left" alertType="warning">
        <strong>{{registrationTermTitleCase}} Full</strong>
        <p>
            There are not any more {{registrationTermPlural}} available for {{viewModel.InstanceName}}. 
        </p>
    </Alert>
    <Alert v-if="showRemainingCapacity" class="text-left" alertType="warning">
        <strong>{{registrationTermTitleCase}} Full</strong>
        <p>
            This {{registrationTerm}} only has capacity for {{remainingCapacityPhrase}}.
        </p>
    </Alert>
    <div class="text-left" v-html="viewModel.InstructionsHtml">
    </div>
    <div v-if="viewModel.MaxRegistrants > 1" class="registrationentry-intro">
        <h1>How many {{viewModel.PluralRegistrantTerm}} will you be registering?</h1>
        <NumberUpDown v-model="numberOfRegistrants" class="margin-t-sm" numberIncrementClasses="input-lg" :max="viewModel.MaxRegistrants" />
    </div>
    <Alert v-if="viewModel.TimeoutMinutes" alertType="info" class="text-left">
        Due to a high-volume of expected interest your {{registrationTerm}} session will expire after
        {{pluralConditional(viewModel.TimeoutMinutes, 'a minute', viewModel.TimeoutMinutes + ' minutes')}}
        of inactivity.
    </Alert>
    <Alert v-if="numberToAddToWaitlist === numberOfRegistrants" class="text-left" alertType="warning">
        This {{registrationTerm}} has reached its capacity. Complete the registration below to be added to the waitlist.
    </Alert>
    <Alert v-else-if="numberToAddToWaitlist" class="text-left" alertType="warning">
        This {{registrationTerm}} only has capacity for {{remainingCapacityPhrase}}.
        The first {{pluralConditional(viewModel.SpotsRemaining, registrantTerm, viewModel.SpotsRemaining + ' ' + registrantTermPlural)}} you add will be registered for {{viewModel.InstanceName}}.
        The remaining {{pluralConditional(numberToAddToWaitlist, registrantTerm, numberToAddToWaitlist + ' ' + registrantTermPlural)}} will be added to the waitlist. 
    </Alert>
    <div class="actions text-right">
        <RockButton btnType="primary" @click="onNext">
            Next
        </RockButton>
    </div>
</div>`
} );
