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
System.register(["vue", "./Registrant", "../../../Elements/Alert"], function (exports_1, context_1) {
    "use strict";
    var vue_1, Registrant_1, Alert_1;
    var __moduleName = context_1 && context_1.id;
    return {
        setters: [
            function (vue_1_1) {
                vue_1 = vue_1_1;
            },
            function (Registrant_1_1) {
                Registrant_1 = Registrant_1_1;
            },
            function (Alert_1_1) {
                Alert_1 = Alert_1_1;
            }
        ],
        execute: function () {
            exports_1("default", vue_1.defineComponent({
                name: 'Event.RegistrationEntry.Registrants',
                components: {
                    Registrant: Registrant_1.default,
                    Alert: Alert_1.default
                },
                setup: function () {
                    return {
                        registrationEntryState: vue_1.inject('registrationEntryState')
                    };
                },
                methods: {
                    onPrevious: function () {
                        if (this.registrationEntryState.CurrentRegistrantIndex <= 0) {
                            this.$emit('previous');
                            return;
                        }
                        var lastFormIndex = this.registrationEntryState.ViewModel.RegistrantForms.length - 1;
                        this.registrationEntryState.CurrentRegistrantIndex--;
                        this.registrationEntryState.CurrentRegistrantFormIndex = lastFormIndex;
                    },
                    onNext: function () {
                        var lastIndex = this.registrationEntryState.Registrants.length - 1;
                        if (this.registrationEntryState.CurrentRegistrantIndex >= lastIndex) {
                            this.$emit('next');
                            return;
                        }
                        this.registrationEntryState.CurrentRegistrantIndex++;
                        this.registrationEntryState.CurrentRegistrantFormIndex = 0;
                    }
                },
                computed: {
                    /** Will some of the registrants have to be added to a waitlist */
                    hasWaitlist: function () {
                        return this.registrationEntryState.Registrants.some(function (r) { return r.IsOnWaitList; });
                    },
                    /** Will this registrant be added to the waitlist? */
                    isOnWaitlist: function () {
                        var currentRegistrant = this.registrationEntryState.Registrants[this.registrationEntryState.CurrentRegistrantIndex];
                        return currentRegistrant.IsOnWaitList;
                    },
                    /** What are the registrants called? */
                    registrantTerm: function () {
                        return (this.registrationEntryState.ViewModel.RegistrantTerm || 'registrant').toLowerCase();
                    },
                    registrants: function () {
                        return this.registrationEntryState.Registrants;
                    },
                    currentRegistrantIndex: function () {
                        return this.registrationEntryState.CurrentRegistrantIndex;
                    }
                },
                template: "\n<div class=\"registrationentry-registrant\">\n    <Alert v-if=\"hasWaitlist && !isOnWaitlist\" alertType=\"success\">\n        This {{registrantTerm}} will be fully registered.\n    </Alert>\n    <Alert v-else-if=\"isOnWaitlist\" alertType=\"warning\">\n        This {{registrantTerm}} will be on the waiting list.\n    </Alert>\n    <template v-for=\"(r, i) in registrants\" :key=\"r.Guid\">\n        <Registrant v-show=\"currentRegistrantIndex === i\" :currentRegistrant=\"r\" :isWaitList=\"isOnWaitlist\" @next=\"onNext\" @previous=\"onPrevious\" />\n    </template>\n</div>"
            }));
        }
    };
});
//# sourceMappingURL=Registrants.js.map