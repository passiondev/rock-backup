//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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

/**
 * The options that can be passed to the GetFinancialGateways API action of
 * the inancialGatewayPicker control.
 */
export type FinancialGatewayPickerGetFinancialGatewaysOptionsBag = {
    /**
     * Whether or not to include payment gateways that are inactive or
     * don't support Rock-initiated transactions.
     */
    showAll: boolean;

    /**
     * The current selected payment gateway. By passing this in, we ensure
     * that this gateway is shown in the list even if it wouldn't normally.
     */
    selectedItem?: number | null;
};
