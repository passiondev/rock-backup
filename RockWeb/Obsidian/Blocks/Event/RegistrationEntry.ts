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

import { defineComponent, inject, provide, reactive, ref } from 'vue';
import RockButton from '../../Elements/RockButton';
import { Guid, newGuid } from '../../Util/Guid';
import RegistrationEntryIntro from './RegistrationEntry/Intro';
import RegistrationEntryRegistrants from './RegistrationEntry/Registrants';
import { RegistrantInfo, RegistrarInfo, RegistrationEntryBlockFormViewModel, RegistrationEntryBlockSuccessViewModel, RegistrationEntryBlockViewModel, RegistrationPersonFieldType } from './RegistrationEntry/RegistrationEntryBlockViewModel';
import RegistrationEntryRegistrationStart from './RegistrationEntry/RegistrationStart';
import RegistrationEntryRegistrationEnd from './RegistrationEntry/RegistrationEnd';
import RegistrationEntrySummary from './RegistrationEntry/Summary';
import Registrants from './RegistrationEntry/Registrants';
import ProgressTracker, { ProgressTrackerItem } from '../../Elements/ProgressTracker';
import NumberFilter, { toWord } from '../../Services/Number';
import StringFilter, { isNullOrWhitespace, toTitleCase } from '../../Services/String';
import Alert from '../../Elements/Alert';
import CountdownTimer from '../../Elements/CountdownTimer';
import RegistrationEntrySuccess from './RegistrationEntry/Success';
import Page from '../../Util/Page';
import { RegistrationEntryBlockArgs } from './RegistrationEntry/RegistrationEntryBlockArgs';
import { InvokeBlockActionFunc } from '../../Controls/RockBlock';
import JavaScriptAnchor from '../../Elements/JavaScriptAnchor';

export enum Step
{
    'intro' = 'intro',
    'registrationStartForm' = 'registrationStartForm',
    'perRegistrantForms' = 'perRegistrantForms',
    'registrationEndForm' = 'registrationEndForm',
    'reviewAndPayment' = 'reviewAndPayment',
    'success' = 'success'
}

export type RegistrantBasicInfo = {
    FirstName: string;
    LastName: string;
    Email: string;
    Guid: Guid;
};

export type RegistrationEntryState = {
    Steps: Record<Step, Step>;
    ViewModel: RegistrationEntryBlockViewModel;
    CurrentStep: string;
    FirstStep: string;
    CurrentRegistrantIndex: number;
    CurrentRegistrantFormIndex: number;
    Registrants: RegistrantInfo[];
    RegistrationFieldValues: Record<Guid, unknown>;
    Registrar: RegistrarInfo;
    GatewayToken: string;
    DiscountCode: string;
    SuccessViewModel: RegistrationEntryBlockSuccessViewModel | null;
    AmountToPayToday: number;
    SessionExpirationDate: Date | null;
    RegistrationSessionGuid: Guid;
};

export function getDefaultRegistrantInfo ()
{
    const ownFamilyGuid = newGuid();

    return {
        IsOnWaitList: false,
        FamilyGuid: ownFamilyGuid,
        FieldValues: {},
        FeeItemQuantities: {},
        Guid: newGuid(),
        PersonGuid: '',
        OwnFamilyGuid: ownFamilyGuid
    } as RegistrantInfo;
}

export function getRegistrantBasicInfo ( registrant: RegistrantInfo, registrantForms: RegistrationEntryBlockFormViewModel[] ): RegistrantBasicInfo
{
    const fields = registrantForms?.flatMap( f => f.Fields ) || [];

    const firstNameGuid = fields.find( f => f.PersonFieldType === RegistrationPersonFieldType.FirstName )?.Guid || '';
    const lastNameGuid = fields.find( f => f.PersonFieldType === RegistrationPersonFieldType.LastName )?.Guid || '';
    const emailGuid = fields.find( f => f.PersonFieldType === RegistrationPersonFieldType.Email )?.Guid || '';

    return {
        FirstName: ( registrant?.FieldValues[ firstNameGuid ] || '' ) as string,
        LastName: ( registrant?.FieldValues[ lastNameGuid ] || '' ) as string,
        Email: ( registrant?.FieldValues[ emailGuid ] || '' ) as string,
        Guid: registrant?.Guid
    };
}

export default defineComponent( {
    name: 'Event.RegistrationEntry',
    components: {
        RockButton,
        Registrants,
        RegistrationEntryIntro,
        RegistrationEntryRegistrants,
        RegistrationEntryRegistrationStart,
        RegistrationEntryRegistrationEnd,
        RegistrationEntrySummary,
        RegistrationEntrySuccess,
        ProgressTracker,
        Alert,
        CountdownTimer,
        JavaScriptAnchor
    },
    setup ()
    {
        const steps: Record<Step, Step> = {
            [ Step.intro ]: Step.intro,
            [ Step.registrationStartForm ]: Step.registrationStartForm,
            [ Step.perRegistrantForms ]: Step.perRegistrantForms,
            [ Step.registrationEndForm ]: Step.registrationEndForm,
            [ Step.reviewAndPayment ]: Step.reviewAndPayment,
            [ Step.success ]: Step.success
        };

        const notFound = ref( false );
        const viewModel = inject( 'configurationValues' ) as RegistrationEntryBlockViewModel;
        const invokeBlockAction = inject( 'invokeBlockAction' ) as InvokeBlockActionFunc;

        if ( !viewModel?.RegistrationAttributesStart )
        {
            notFound.value = true;
        }

        const hasPreAttributes = viewModel.RegistrationAttributesStart?.length > 0;
        let currentStep = steps.intro;

        if ( viewModel.SuccessViewModel )
        {
            // This is after having paid via redirect gateway
            currentStep = steps.success;
        }
        else if ( viewModel.Session )
        {
            // This is an existing registration, start at the summary
            currentStep = steps.reviewAndPayment;
        }
        else if ( viewModel.MaxRegistrants === 1 && isNullOrWhitespace( viewModel.InstructionsHtml ) )
        {
            // There is no need to show the numer of registrants selector or instructions. Start at the second page.
            currentStep = hasPreAttributes ? steps.registrationStartForm : steps.perRegistrantForms;
        }

        const registrationEntryState = reactive( {
            Steps: steps,
            ViewModel: viewModel,
            FirstStep: currentStep,
            CurrentStep: currentStep,
            CurrentRegistrantFormIndex: 0,
            CurrentRegistrantIndex: 0,
            Registrants: viewModel.Session?.Registrants || [ getDefaultRegistrantInfo() ],
            RegistrationFieldValues: viewModel.Session?.FieldValues || {},
            Registrar: viewModel.Session?.Registrar || {
                NickName: '',
                LastName: '',
                Email: '',
                UpdateEmail: true
            },
            GatewayToken: '',
            DiscountCode: viewModel.Session?.DiscountCode || '',
            SuccessViewModel: viewModel.SuccessViewModel,
            AmountToPayToday: 0,
            SessionExpirationDate: null,
            RegistrationSessionGuid: viewModel.Session?.RegistrationSessionGuid || newGuid()
        } as RegistrationEntryState );

        provide( 'registrationEntryState', registrationEntryState );

        /** A method to get the args needed for persisting the session */
        const getRegistrationEntryBlockArgs: () => RegistrationEntryBlockArgs = () =>
        {
            return {
                RegistrationSessionGuid: registrationEntryState.RegistrationSessionGuid,
                GatewayToken: registrationEntryState.GatewayToken,
                DiscountCode: registrationEntryState.DiscountCode,
                FieldValues: registrationEntryState.RegistrationFieldValues,
                Registrar: registrationEntryState.Registrar,
                Registrants: registrationEntryState.Registrants,
                AmountToPayNow: registrationEntryState.AmountToPayToday
            };
        };

        provide( 'getRegistrationEntryBlockArgs', getRegistrationEntryBlockArgs );

        /** A method to persist the session */
        const persistSession: ( force: boolean ) => Promise<void> = async ( force = false ) =>
        {
            if ( !force && !viewModel.TimeoutMinutes )
            {
                return;
            }

            const response = await invokeBlockAction<{ ExpirationDateTime: string }>( 'PersistSession', {
                args: getRegistrationEntryBlockArgs()
            } );

            if ( response.data )
            {
                const asDate = new Date( response.data.ExpirationDateTime );
                registrationEntryState.SessionExpirationDate = asDate;
            }
        };

        provide( 'persistSession', persistSession );

        /** Expose these members and make them available within the rest of the component */
        return {
            viewModel,
            steps,
            registrationEntryState,
            notFound,
            persistSession
        };
    },
    data ()
    {
        return {
            secondsBeforeExpiration: -1
        };
    },
    computed: {
        /** Is the session expired? */
        isSessionExpired (): boolean
        {
            return this.secondsBeforeExpiration === 0 && this.currentStep !== this.steps.success;
        },

        viewModel (): RegistrationEntryBlockViewModel
        {
            return this.registrationEntryState.ViewModel;
        },
        mustLogin (): boolean
        {
            return !this.$store.state.currentPerson && ( this.viewModel.IsUnauthorized || this.viewModel.LoginRequiredToRegister );
        },
        isUnauthorized (): boolean
        {
            return this.viewModel.IsUnauthorized;
        },
        currentStep (): string
        {
            return this.registrationEntryState.CurrentStep;
        },
        registrants (): RegistrantInfo[]
        {
            return this.registrationEntryState.Registrants;
        },
        hasPreAttributes (): boolean
        {
            return this.viewModel.RegistrationAttributesStart.length > 0;
        },
        hasPostAttributes (): boolean
        {
            return this.viewModel.RegistrationAttributesEnd.length > 0;
        },
        progressTrackerIndex (): number
        {
            if ( this.currentStep === this.steps.intro )
            {
                return 0;
            }

            const stepsBeforePre = this.registrationEntryState.FirstStep === this.steps.intro ? 1 : 0;

            if ( this.currentStep === this.steps.registrationStartForm )
            {
                return stepsBeforePre;
            }

            const stepsBeforeRegistrants = stepsBeforePre + ( this.hasPreAttributes ? 1 : 0 );

            if ( this.currentStep === this.steps.perRegistrantForms )
            {
                return this.registrationEntryState.CurrentRegistrantIndex + stepsBeforeRegistrants;
            }

            const stepsToCompleteRegistrants = this.registrationEntryState.Registrants.length + stepsBeforeRegistrants;

            if ( this.currentStep === this.steps.registrationEndForm )
            {
                return stepsToCompleteRegistrants;
            }

            if ( this.currentStep === this.steps.reviewAndPayment )
            {
                return stepsToCompleteRegistrants + ( this.hasPostAttributes ? 1 : 0 );
            }

            return 0;
        },
        uppercaseRegistrantTerm (): string
        {
            return StringFilter.toTitleCase( this.viewModel.RegistrantTerm );
        },
        currentRegistrantTitle (): string
        {
            const ordinal = NumberFilter.toOrdinal( this.registrationEntryState.CurrentRegistrantIndex + 1 );
            let title = StringFilter.toTitleCase(
                this.registrants.length <= 1 ?
                    this.uppercaseRegistrantTerm :
                    ordinal + ' ' + this.uppercaseRegistrantTerm );

            if ( this.registrationEntryState.CurrentRegistrantFormIndex > 0 )
            {
                title += ' (cont)';
            }

            return title;
        },
        stepTitleHtml (): string
        {
            if ( this.currentStep === this.steps.registrationStartForm )
            {
                return this.viewModel.RegistrationAttributeTitleStart;
            }

            if ( this.currentStep === this.steps.perRegistrantForms )
            {
                return this.currentRegistrantTitle;
            }

            if ( this.currentStep === this.steps.registrationEndForm )
            {
                return this.viewModel.RegistrationAttributeTitleEnd;
            }

            if ( this.currentStep === this.steps.reviewAndPayment )
            {
                return 'Review Registration';
            }

            if ( this.currentStep === this.steps.success )
            {
                return this.registrationEntryState.SuccessViewModel?.TitleHtml || 'Congratulations';
            }

            return '';
        },

        /** The items to display in the progress tracker */
        progressTrackerItems (): ProgressTrackerItem[]
        {
            const items: ProgressTrackerItem[] = [];

            if ( this.registrationEntryState.FirstStep === this.steps.intro )
            {
                items.push( {
                    Key: 'Start',
                    Title: 'Start',
                    Subtitle: this.viewModel.RegistrationTerm
                } );
            }

            if ( this.hasPreAttributes )
            {
                items.push( {
                    Key: 'Pre',
                    Title: this.viewModel.RegistrationAttributeTitleStart,
                    Subtitle: this.viewModel.RegistrationTerm
                } );
            }

            if ( !this.registrationEntryState.Registrants.length )
            {
                items.push( {
                    Key: 'Registrant',
                    Title: toTitleCase( this.viewModel.RegistrantTerm ),
                    Subtitle: this.viewModel.RegistrationTerm
                } );
            }

            for ( let i = 0; i < this.registrationEntryState.Registrants.length; i++ )
            {
                const registrant = this.registrationEntryState.Registrants[ i ];
                const info = getRegistrantBasicInfo( registrant, this.viewModel.RegistrantForms );

                if ( info?.FirstName && info?.LastName )
                {
                    items.push( {
                        Key: `Registrant-${registrant.Guid}`,
                        Title: info.FirstName,
                        Subtitle: info.LastName
                    } );
                }
                else
                {
                    items.push( {
                        Key: `Registrant-${registrant.Guid}`,
                        Title: toTitleCase( this.viewModel.RegistrantTerm ),
                        Subtitle: toTitleCase( toWord( i + 1 ) )
                    } );
                }
            }

            if ( this.hasPostAttributes )
            {
                items.push( {
                    Key: 'Post',
                    Title: this.viewModel.RegistrationAttributeTitleEnd,
                    Subtitle: this.viewModel.RegistrationTerm
                } );
            }

            items.push( {
                Key: 'Finalize',
                Title: 'Finalize',
                Subtitle: this.viewModel.RegistrationTerm
            } );

            return items;
        }
    },
    methods: {
        /** Restart the registration */
        restart ()
        {
            location.reload();
        },

        async onIntroNext ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.hasPreAttributes ? this.steps.registrationStartForm : this.steps.perRegistrantForms;
            Page.smoothScrollToTop();
        },
        async onRegistrationStartPrevious ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.steps.intro;
            Page.smoothScrollToTop();
        },
        async onRegistrationStartNext ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.steps.perRegistrantForms;
            Page.smoothScrollToTop();
        },
        async onRegistrantPrevious ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.hasPreAttributes ? this.steps.registrationStartForm : this.steps.intro;
            Page.smoothScrollToTop();
        },
        async onRegistrantNext ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.hasPostAttributes ? this.steps.registrationEndForm : this.steps.reviewAndPayment;
            Page.smoothScrollToTop();
        },
        async onRegistrationEndPrevious ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.steps.perRegistrantForms;
            Page.smoothScrollToTop();
        },
        async onRegistrationEndNext ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.steps.reviewAndPayment;
            Page.smoothScrollToTop();
        },
        async onSummaryPrevious ()
        {
            await this.persistSession( false );
            this.registrationEntryState.CurrentStep = this.hasPostAttributes ? this.steps.registrationEndForm : this.steps.perRegistrantForms;
            Page.smoothScrollToTop();
        },
        async onSummaryNext ()
        {
            this.registrationEntryState.CurrentStep = this.steps.success;
            Page.smoothScrollToTop();
        }
    },
    watch: {
        'registrationEntryState.SessionExpirationDate': {
            immediate: true,
            handler ()
            {
                if ( !this.registrationEntryState.SessionExpirationDate )
                {
                    this.secondsBeforeExpiration = -1;
                    return;
                }

                const nowMs = new Date().getTime();
                const thenMs = this.registrationEntryState.SessionExpirationDate.getTime();
                const diffMs = thenMs - nowMs;
                this.secondsBeforeExpiration = diffMs / 1000;
            }
        }
    },
    mounted ()
    {
        if ( this.viewModel.LoginRequiredToRegister && !this.$store.state.currentPerson )
        {
            this.$store.dispatch( 'redirectToLogin' );
        }
    },
    template: `
<div>
    <Alert v-if="notFound" alertType="warning">
        <strong>Sorry</strong>
        <p>The selected registration could not be found or is no longer active.</p>
    </Alert>
    <Alert v-else-if="mustLogin" alertType="warning">
        <strong>Please log in</strong>
        <p>You must be logged in to access this registration.</p>
    </Alert>
    <Alert v-else-if="isUnauthorized" alertType="warning">
        <strong>Sorry</strong>
        <p>You are not allowed to view or edit the selected registration since you are not the one who created the registration.</p>
    </Alert>
    <Alert v-else-if="isSessionExpired" alertType="warning">
        Your registration has expired.
        <JavaScriptAnchor @click="restart">
            Restart registration
        </JavaScriptAnchor>
    </Alert>
    <template v-else>
        <h1 v-if="currentStep !== steps.intro" v-html="stepTitleHtml"></h1>
        <ProgressTracker v-if="currentStep !== steps.success" :items="progressTrackerItems" :currentIndex="progressTrackerIndex">
            <template #aside>
                <div v-if="secondsBeforeExpiration >= 0 && secondsBeforeExpiration <= (30 * 60)" class="remaining-time flex-grow-1 flex-md-grow-0">
                    <span class="remaining-time-title">Time left before timeout</span>
                    <p class="remaining-time-countdown">
                        <CountdownTimer v-model="secondsBeforeExpiration" />
                    </p>
                </div>
            </template>
        </ProgressTracker>
        <RegistrationEntryIntro v-if="currentStep === steps.intro" @next="onIntroNext" />
        <RegistrationEntryRegistrationStart v-else-if="currentStep === steps.registrationStartForm" @next="onRegistrationStartNext" @previous="onRegistrationStartPrevious" />
        <RegistrationEntryRegistrants v-else-if="currentStep === steps.perRegistrantForms" @next="onRegistrantNext" @previous="onRegistrantPrevious" />
        <RegistrationEntryRegistrationEnd v-else-if="currentStep === steps.registrationEndForm" @next="onRegistrationEndNext" @previous="onRegistrationEndPrevious" />
        <RegistrationEntrySummary v-else-if="currentStep === steps.reviewAndPayment" @next="onSummaryNext" @previous="onSummaryPrevious" />
        <RegistrationEntrySuccess v-else-if="currentStep === steps.success" />
        <Alert v-else alertType="danger">Invalid State: '{{currentStep}}'</Alert>
    </template>
</div>`
} );
