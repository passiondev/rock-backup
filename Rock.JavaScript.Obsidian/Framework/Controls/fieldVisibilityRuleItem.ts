import { defineComponent, PropType } from "vue";

export type FieldVisibilityRuleItem = {
    key?: "value"
};

export const FieldVisibilityRuleItemComponent = defineComponent({
    name: "FieldVisibilityRuleItemComponent",

    props: {
        rule: {
            type: Object as PropType<FieldVisibilityRuleItem>,
            required: true
        }
    },

    emits: [
        "removeRule"
    ],

    setup(props, { emit }) {
        function removeRule(): void {
            console.log("REMOVE THIS");
            emit("removeRule", props.rule);
        }
        return {
            removeRule
        };
    },

    template: `
<div class="filter-rule row form-row">
    <div class="col-xs-10 col-sm-11">
        <div class="row form-row">
            <div class="filter-rule-comparefield col-md-4">
                <select class="form-control">
                    <option value="6af9bf76-9e43-49f5-ac77-b02f59c65549" selected="selected">Position Description</option>
                    <option value="3703041b-2f0d-49b2-8c45-be0769802c7e">Type</option>
                </select>
            </div>
            <div class="filter-rule-fieldfilter col-md-8">
                <div id="ctl00_main_ctl35_ctl01_ctl06_5ce3e346336540e48c88ef1d352ffc76_99eda7dc483a497785420016403d896c_99eda7dc483a497785420016403d896c_formEditor__mdFieldVisibilityRules_fvreWorkflowFields__filterControl_49afa9b409ad4ab49d4865cf041e8eeb" class="row form-row field-criteria">
                    <div id="ctl00_main_ctl35_ctl01_ctl06_5ce3e346336540e48c88ef1d352ffc76_99eda7dc483a497785420016403d896c_99eda7dc483a497785420016403d896c_formEditor__mdFieldVisibilityRules_fvreWorkflowFields__filterControl_49afa9b409ad4ab49d4865cf041e8eeb_col1" class="col-md-4">
                        <select class="js-filter-compare form-control">
                            <option value="1" selected="selected">Equal To</option>
                            <option value="2">Not Equal To</option>
                            <option value="8">Contains</option>
                            <option value="16">Does Not Contain</option>
                            <option value="32">Is Blank</option>
                            <option value="64">Is Not Blank</option>
                            <option value="4">Starts With</option>
                            <option value="2048">Ends With</option>
                        </select>
                    </div>
                    <div id="ctl00_main_ctl35_ctl01_ctl06_5ce3e346336540e48c88ef1d352ffc76_99eda7dc483a497785420016403d896c_99eda7dc483a497785420016403d896c_formEditor__mdFieldVisibilityRules_fvreWorkflowFields__filterControl_49afa9b409ad4ab49d4865cf041e8eeb_col2" class="col-md-8">
                        <input name="ctl00$main$ctl35$ctl01$ctl06$5ce3e346336540e48c88ef1d352ffc76$99eda7dc483a497785420016403d896c$99eda7dc483a497785420016403d896c_formEditor$_mdFieldVisibilityRules$fvreWorkflowFields$_filterControl_49afa9b409ad4ab49d4865cf041e8eeb_ctlCompareValue" type="text" id="ctl00_main_ctl35_ctl01_ctl06_5ce3e346336540e48c88ef1d352ffc76_99eda7dc483a497785420016403d896c_99eda7dc483a497785420016403d896c_formEditor__mdFieldVisibilityRules_fvreWorkflowFields__filterControl_49afa9b409ad4ab49d4865cf041e8eeb_ctlCompareValue" class="js-filter-control form-control">
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
