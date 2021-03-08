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
System.register(["vue", "../../../Elements/Alert", "./RegistrationEntryBlockViewModel"], function (exports_1, context_1) {
    "use strict";
    var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
        function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
            function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
            function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    };
    var __generator = (this && this.__generator) || function (thisArg, body) {
        var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f) throw new TypeError("Generator is already executing.");
            while (_) try {
                if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
                if (y = 0, t) op = [op[0] & 2, t.value];
                switch (op[0]) {
                    case 0: case 1: t = op; break;
                    case 4: _.label++; return { value: op[1], done: false };
                    case 5: _.label++; y = op[1]; op = [0]; continue;
                    case 7: op = _.ops.pop(); _.trys.pop(); continue;
                    default:
                        if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                        if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                        if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                        if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                        if (t[2]) _.ops.pop();
                        _.trys.pop(); continue;
                }
                op = body.call(thisArg, _);
            } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
            if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
        }
    };
    var vue_1, Alert_1, RegistrationEntryBlockViewModel_1;
    var __moduleName = context_1 && context_1.id;
    return {
        setters: [
            function (vue_1_1) {
                vue_1 = vue_1_1;
            },
            function (Alert_1_1) {
                Alert_1 = Alert_1_1;
            },
            function (RegistrationEntryBlockViewModel_1_1) {
                RegistrationEntryBlockViewModel_1 = RegistrationEntryBlockViewModel_1_1;
            }
        ],
        execute: function () {
            exports_1("default", vue_1.defineComponent({
                name: 'Event.RegistrationEntry.RegistrantPersonField',
                components: {
                    Alert: Alert_1.default
                },
                props: {
                    field: {
                        type: Object,
                        required: true
                    }
                },
                data: function () {
                    return {
                        fieldControlComponent: null,
                        fieldControlComponentProps: {},
                        loading: true,
                        value: ''
                    };
                },
                watch: {
                    field: {
                        immediate: true,
                        handler: function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var componentPath, props, componentModule, _a, component;
                                return __generator(this, function (_b) {
                                    switch (_b.label) {
                                        case 0:
                                            this.loading = true;
                                            componentPath = '';
                                            props = {
                                                rules: this.field.IsRequired ? 'required' : ''
                                            };
                                            switch (this.field.PersonFieldType) {
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.FirstName:
                                                    componentPath = 'Elements/TextBox';
                                                    props.label = 'First Name';
                                                    break;
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.LastName:
                                                    componentPath = 'Elements/TextBox';
                                                    props.label = 'Last Name';
                                                    break;
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.MiddleName:
                                                    componentPath = 'Elements/TextBox';
                                                    props.label = 'Middle Name';
                                                    break;
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.Campus:
                                                    componentPath = 'Components/CampusPicker';
                                                    props.label = 'Campus';
                                                    break;
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.Email:
                                                    componentPath = 'Elements/EmailBox';
                                                    props.label = 'Email';
                                                    break;
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.Gender:
                                                    componentPath = 'Elements/GenderDropDownList';
                                                    break;
                                                case RegistrationEntryBlockViewModel_1.RegistrationPersonFieldType.Birthdate:
                                                    props.label = 'Birthday';
                                                    componentPath = 'Elements/BirthdayPicker';
                                                    break;
                                            }
                                            if (!componentPath) return [3 /*break*/, 2];
                                            return [4 /*yield*/, context_1.import("../../../" + componentPath)];
                                        case 1:
                                            _a = (_b.sent());
                                            return [3 /*break*/, 3];
                                        case 2:
                                            _a = null;
                                            _b.label = 3;
                                        case 3:
                                            componentModule = _a;
                                            component = componentModule ? (componentModule.default || componentModule) : null;
                                            if (component) {
                                                this.fieldControlComponentProps = props;
                                                this.fieldControlComponent = vue_1.markRaw(component);
                                            }
                                            else {
                                                this.fieldControlComponentProps = {};
                                                this.fieldControlComponent = null;
                                            }
                                            this.loading = false;
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        }
                    }
                },
                template: "\n<component v-if=\"fieldControlComponent\" :is=\"fieldControlComponent\" v-bind=\"fieldControlComponentProps\" v-model=\"value\" />\n<Alert v-else-if=\"!loading\" alertType=\"danger\">Could not resolve person field</Alert>"
            }));
        }
    };
});
//# sourceMappingURL=RegistrantPersonField.js.map