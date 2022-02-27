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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.CheckIn;
using Rock.CheckIn.Registration;
using Rock.Data;
using Rock.Model;
using Rock.Transactions;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

/// <summary>
///
/// </summary>
namespace RockWeb.Blocks.CheckIn
{
    /// <summary>
    ///
    /// </summary>
    [DisplayName("Passion - Edit Family")]
    [Category("Check-in")]
    [Description("Block to Add or Edit a Family during the Check-in Process.")]
    public partial class EditFamily : CheckInEditFamilyBlock
    {
        #region private fields

        /// <summary>
        /// A hash of EditFamilyState before any editing. Use this to determine if there were any changes
        /// </summary>
        /// <value>
        /// The initial state hash.
        /// </value>
        private int _initialEditFamilyStateHash
        {
            get
            {
                return ViewState["_initialEditFamilyStateHash"] as int? ?? 0;
            }
            set
            {
                ViewState["_initialEditFamilyStateHash"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the state of the edit family.
        /// </summary>
        /// <value>
        /// The state of the edit family.
        /// </value>
        public FamilyRegistrationState EditFamilyState { get; set; }

        /// <summary>
        /// Gets or sets the required attributes for adults.
        /// </summary>
        /// <value>
        /// The required attributes for adults.
        /// </value>
        private List<AttributeCache> RequiredAttributesForAdults { get; set; }

        /// <summary>
        /// Gets or sets the optional attributes for adults.
        /// </summary>
        /// <value>
        /// The optional attributes for adults.
        /// </value>
        private List<AttributeCache> OptionalAttributesForAdults { get; set; }

        /// <summary>
        /// Gets or sets the required attributes for children.
        /// </summary>
        /// <value>
        /// The required attributes for children.
        /// </value>
        private List<AttributeCache> RequiredAttributesForChildren { get; set; }

        /// <summary>
        /// Gets or sets the optional attributes for children.
        /// </summary>
        /// <value>
        /// The optional attributes for children.
        /// </value>
        private List<AttributeCache> OptionalAttributesForChildren { get; set; }

        /// <summary>
        /// Gets or sets the required attributes for families.
        /// </summary>
        /// <value>
        /// The required attributes for families.
        /// </value>
        private List<AttributeCache> RequiredAttributesForFamilies { get; set; }

        /// <summary>
        /// Gets or sets the optional attributes for families.
        /// </summary>
        /// <value>
        /// The optional attributes for families.
        /// </value>
        private List<AttributeCache> OptionalAttributesForFamilies { get; set; }

        /// <summary>
        /// The group type role adult identifier
        /// </summary>
        private static int _groupTypeRoleAdultId = GroupTypeCache.GetFamilyGroupType().Roles.FirstOrDefault(a => a.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid()).Id;
        private static int _groupTypeRoleChildId = GroupTypeCache.GetFamilyGroupType().Roles.FirstOrDefault(a => a.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid()).Id;

        /// <summary>
        /// The person record status active identifier
        /// </summary>
        private static int _personRecordStatusActiveId = DefinedValueCache.Get(Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE.AsGuid()).Id;

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (CurrentCheckInState == null)
            {
                return;
            }

            gFamilyMembers.DataKeyNames = new string[] { "GroupMemberGuid" };
            gFamilyMembers.GridRebind += gFamilyMembers_GridRebind;

            RequiredAttributesForAdults = CurrentCheckInState.CheckInType.Registration.RequiredAttributesForAdults.Where(a => a.IsAuthorized(Rock.Security.Authorization.EDIT, this.CurrentPerson)).ToList();
            OptionalAttributesForAdults = CurrentCheckInState.CheckInType.Registration.OptionalAttributesForAdults.Where(a => a.IsAuthorized(Rock.Security.Authorization.EDIT, this.CurrentPerson)).ToList();
            RequiredAttributesForChildren = CurrentCheckInState.CheckInType.Registration.RequiredAttributesForChildren.Where(a => a.IsAuthorized(Rock.Security.Authorization.EDIT, this.CurrentPerson)).ToList();
            OptionalAttributesForChildren = CurrentCheckInState.CheckInType.Registration.OptionalAttributesForChildren.Where(a => a.IsAuthorized(Rock.Security.Authorization.EDIT, this.CurrentPerson)).ToList();
            RequiredAttributesForFamilies = CurrentCheckInState.CheckInType.Registration.RequiredAttributesForFamilies.Where(a => a.IsAuthorized(Rock.Security.Authorization.EDIT, this.CurrentPerson)).ToList();
            OptionalAttributesForFamilies = CurrentCheckInState.CheckInType.Registration.OptionalAttributesForFamilies.Where(a => a.IsAuthorized(Rock.Security.Authorization.EDIT, this.CurrentPerson)).ToList();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (CurrentCheckInState == null)
            {
                NavigateToHomePage();
            }

            if (this.IsPostBack)
            {
                // make sure the ShowAddFamilyPrompt is disabled so that it doesn't show again until explicitly enabled after doing a Search
                hfShowCancelEditPrompt.Value = "0";

                if (this.Request.Params["__EVENTARGUMENT"] == "ConfirmCancelFamily")
                {
                    CancelFamilyEdit(false);
                }
            }
        }

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (CurrentCheckInState == null)
            {
                return;
            }

            EditFamilyState = (this.ViewState["EditFamilyState"] as string).FromJsonOrNull<FamilyRegistrationState>();

            CreateDynamicFamilyControls(FamilyRegistrationState.FromGroup(new Group() { GroupTypeId = GroupTypeCache.GetFamilyGroupType().Id }), false);

            CreateDynamicPersonControls(FamilyRegistrationState.FamilyPersonState.FromTemporaryPerson(), false);
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            this.ViewState["EditFamilyState"] = EditFamilyState.ToJson();
            return base.SaveViewState();
        }

        /// <summary>
        /// Creates the dynamic family controls.
        /// </summary>
        /// <param name="editFamilyState">State of the edit family.</param>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        private void CreateDynamicFamilyControls(FamilyRegistrationState editFamilyState, bool setValues)
        {
            phFamilyAttributes.Controls.Clear();

            var fakeFamily = new Group() { GroupTypeId = GroupTypeCache.GetFamilyGroupType().Id };
            var attributeList = editFamilyState.FamilyAttributeValuesState.Select(a => AttributeCache.Get(a.Value.AttributeId)).ToList();
            fakeFamily.Attributes = attributeList.ToDictionary(a => a.Key, v => v);
            fakeFamily.AttributeValues = editFamilyState.FamilyAttributeValuesState;
            var familyAttributeKeysToEdit = this.RequiredAttributesForFamilies.OrderBy(a => a.Order).Select(a => a.Key).ToList();
            familyAttributeKeysToEdit.AddRange(this.OptionalAttributesForFamilies.OrderBy(a => a.Order).Select(a => a.Key).ToList());

            Rock.Attribute.Helper.AddEditControls(
                string.Empty,
                familyAttributeKeysToEdit,
                fakeFamily,
                phFamilyAttributes,
                btnSaveFamily.ValidationGroup,
                setValues,
                new List<string>(),
                2);

            // override the attribute's IsRequired and set Required based on whether the attribute is part of the Required or Optional set of attributes for the Registration
            foreach (Control attributeControl in phFamilyAttributes.ControlsOfTypeRecursive<Control>().OfType<Control>())
            {
                if (attributeControl is IHasRequired && attributeControl.ID.IsNotNullOrWhiteSpace())
                {
                    int? attributeControlAttributeId = attributeControl.ID.Replace("attribute_field_", string.Empty).AsIntegerOrNull();
                    if (attributeControlAttributeId.HasValue)
                    {
                        (attributeControl as IHasRequired).Required = this.RequiredAttributesForFamilies.Any(a => a.Id == attributeControlAttributeId.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the dynamic person controls.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        private void CreateDynamicPersonControls(FamilyRegistrationState.FamilyPersonState familyPersonState, bool setValues)
        {
            phAdultAttributes.Controls.Clear();
            phChildAttributes.Controls.Clear();
            var fakePerson = new Person();
            var attributeList = familyPersonState.PersonAttributeValuesState.Select(a => AttributeCache.Get(a.Value.AttributeId)).ToList();
            fakePerson.Attributes = attributeList.ToDictionary(a => a.Key, v => v);
            fakePerson.AttributeValues = familyPersonState.PersonAttributeValuesState;

            var adultAttributeKeysToEdit = this.RequiredAttributesForAdults.OrderBy(a => a.Order).Select(a => a.Key).ToList();
            adultAttributeKeysToEdit.AddRange(this.OptionalAttributesForAdults.OrderBy(a => a.Order).Select(a => a.Key).ToList());

            Rock.Attribute.Helper.AddEditControls(
                string.Empty,
                adultAttributeKeysToEdit,
                fakePerson,
                phAdultAttributes,
                btnDonePerson.ValidationGroup,
                setValues,
                new List<string>(),
                2);

            var childAttributeKeysToEdit = this.RequiredAttributesForChildren.OrderBy(a => a.Order).Select(a => a.Key).ToList();
            childAttributeKeysToEdit.AddRange(this.OptionalAttributesForChildren.OrderBy(a => a.Order).Select(a => a.Key).ToList());

            Rock.Attribute.Helper.AddEditControls(
                string.Empty,
                childAttributeKeysToEdit,
                fakePerson,
                phChildAttributes,
                btnDonePerson.ValidationGroup,
                setValues,
                new List<string>(),
                2);

            // override the attribute's IsRequired and set Required based on whether the attribute is part of the Required or Optional set of attributes for the Registration
            foreach (Control attributeControl in phAdultAttributes.ControlsOfTypeRecursive<Control>().OfType<Control>())
            {
                if (attributeControl is IHasRequired && attributeControl.ID.IsNotNullOrWhiteSpace())
                {
                    int? attributeControlAttributeId = attributeControl.ID.Replace("attribute_field_", string.Empty).AsIntegerOrNull();
                    if (attributeControlAttributeId.HasValue)
                    {
                        (attributeControl as IHasRequired).Required = this.RequiredAttributesForAdults.Any(a => a.Id == attributeControlAttributeId.Value);
                    }
                }
            }

            foreach (Control attributeControl in phChildAttributes.ControlsOfTypeRecursive<Control>().OfType<Control>())
            {
                if (attributeControl is IHasRequired && attributeControl.ID.IsNotNullOrWhiteSpace())
                {
                    int? attributeControlAttributeId = attributeControl.ID.Replace("attribute_field_", string.Empty).AsIntegerOrNull();
                    if (attributeControlAttributeId.HasValue)
                    {
                        (attributeControl as IHasRequired).Required = this.RequiredAttributesForChildren.Any(a => a.Id == attributeControlAttributeId.Value);
                    }
                }
            }
        }

        #endregion Methods

        #region Edit Family

        /// <summary>
        /// Shows the Edit Family block in Edit Family mode
        /// </summary>
        /// <param name="checkInFamily">The check in family.</param>
        public override void ShowEditFamily(CheckInFamily checkInFamily)
        {
            ShowFamilyDetail(checkInFamily);
        }

        /// <summary>
        /// Shows the Edit Family block in Add Family mode
        /// </summary>
        public override void ShowAddFamily()
        {
            ShowFamilyDetail(null);
        }

        /// <summary>
        /// Shows edit UI fo the family (or null adding a new family)
        /// </summary>
        /// <param name="checkInFamily">The check in family.</param>
        private void ShowFamilyDetail(CheckInFamily checkInFamily)
        {
            if (checkInFamily != null && checkInFamily.Group != null)
            {
                this.EditFamilyState = FamilyRegistrationState.FromGroup(checkInFamily.Group);
                hfGroupId.Value = checkInFamily.Group.Id.ToString();
                mdEditFamily.Title = checkInFamily.Group.Name;

                int groupId = hfGroupId.Value.AsInteger();
                var rockContext = new RockContext();
                var groupMemberService = new GroupMemberService(rockContext);
                var groupMembersQuery = groupMemberService.Queryable(false)
                    .Include(a => a.Person)
                    .Where(a => a.GroupId == groupId)
                    .OrderBy(m => m.GroupRole.Order)
                    .ThenBy(m => m.Person.BirthYear)
                    .ThenBy(m => m.Person.BirthMonth)
                    .ThenBy(m => m.Person.BirthDay)
                    .ThenBy(m => m.Person.Gender);

                var groupMemberList = groupMembersQuery.ToList();

                foreach (var groupMember in groupMemberList)
                {
                    var familyPersonState = FamilyRegistrationState.FamilyPersonState.FromPerson(groupMember.Person, 0, true);
                    familyPersonState.GroupMemberGuid = groupMember.Guid;
                    familyPersonState.GroupId = groupMember.GroupId;
                    familyPersonState.IsAdult = groupMember.GroupRoleId == _groupTypeRoleAdultId;
                    this.EditFamilyState.FamilyPersonListState.Add(familyPersonState);
                }

                var adultIds = this.EditFamilyState.FamilyPersonListState.Where(a => a.IsAdult && a.PersonId.HasValue).Select(a => a.PersonId.Value).ToList();
                var roleIds = CurrentCheckInState.CheckInType.Registration.KnownRelationships.Where(a => a.Key != 0).Select(a => a.Key).ToList();
                IEnumerable<GroupMember> personRelationships = new PersonService(rockContext).GetRelatedPeople(adultIds, roleIds);
                foreach (GroupMember personRelationship in personRelationships)
                {
                    if (!this.EditFamilyState.FamilyPersonListState.Any(a => a.PersonId == personRelationship.Person.Id))
                    {
                        var familyPersonState = FamilyRegistrationState.FamilyPersonState.FromPerson(personRelationship.Person, personRelationship.GroupRoleId, false);
                        familyPersonState.GroupMemberGuid = Guid.NewGuid();
                        var relatedFamily = personRelationship.Person.GetFamily();
                        if (relatedFamily != null)
                        {
                            familyPersonState.GroupId = relatedFamily.Id;
                        }

                        //familyPersonState.IsAdult = false;
                        familyPersonState.ChildRelationshipToAdult = personRelationship.GroupRoleId;
                        familyPersonState.CanCheckIn = CurrentCheckInState.CheckInType.Registration.KnownRelationshipsCanCheckin.Any(k => k.Key == familyPersonState.ChildRelationshipToAdult);
                        this.EditFamilyState.FamilyPersonListState.Add(familyPersonState);
                    }
                }

                BindFamilyMembersGrid();
                CreateDynamicFamilyControls(EditFamilyState, true);

                ShowFamilyView();
            }
            else
            {
                this.EditFamilyState = FamilyRegistrationState.FromGroup(new Group() { GroupTypeId = GroupTypeCache.GetFamilyGroupType().Id });
                CreateDynamicFamilyControls(EditFamilyState, true);
                hfGroupId.Value = "0";
                mdEditFamily.Title = "Add Family";
                EditGroupMember(null);
            }

            _initialEditFamilyStateHash = this.EditFamilyState.GetStateHash();

            // disable any idle redirect blocks that are on the page when the mdEditFamily modal is open
            DisableIdleRedirectBlocks(true);

            upContent.Update();
            mdEditFamily.Show();
        }

        /// <summary>
        /// Handles the GridRebind event of the gFamilyMembers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.GridRebindEventArgs"/> instance containing the event data.</param>
        private void gFamilyMembers_GridRebind(object sender, Rock.Web.UI.Controls.GridRebindEventArgs e)
        {
            BindFamilyMembersGrid();
        }

        /// <summary>
        /// The index of the 'DeleteField' column in the grid for gFamilyMembers_RowDataBound
        /// </summary>
        private int _deleteFieldIndex;

        /// <summary>
        /// The known relationship lookup for gFamilyMembers_RowDataBound
        /// </summary>
        private Dictionary<int, string> _knownRelationshipLookup = null;

        /// <summary>
        /// Handles the RowDataBound event of the gFamilyMembers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gFamilyMembers_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            var familyPersonState = e.Row.DataItem as FamilyRegistrationState.FamilyPersonState;
            if (familyPersonState != null)
            {
                Literal lGroupRoleAndRelationship = e.Row.FindControl("lGroupRoleAndRelationship") as Literal;
                if (lGroupRoleAndRelationship != null)
                {
                    lGroupRoleAndRelationship.Text = familyPersonState.GroupRole;
                    if (familyPersonState.ChildRelationshipToAdult > 0 && _knownRelationshipLookup != null)
                    {
                        var relationshipText = _knownRelationshipLookup.GetValueOrNull(familyPersonState.ChildRelationshipToAdult);
                        lGroupRoleAndRelationship.Text = string.Format("{0}<br/>{1}", familyPersonState.GroupRole, relationshipText);
                    }
                }

                Literal lRequiredAttributes = e.Row.FindControl("lRequiredAttributes") as Literal;
                if (lRequiredAttributes != null)
                {
                    List<AttributeCache> requiredAttributes;
                    if (familyPersonState.IsAdult)
                    {
                        requiredAttributes = RequiredAttributesForAdults;
                    }
                    else
                    {
                        requiredAttributes = RequiredAttributesForChildren;
                    }

                    if (requiredAttributes.Any())
                    {
                        DescriptionList descriptionList = new DescriptionList();
                        foreach (var requiredAttribute in requiredAttributes)
                        {
                            var attributeValue = familyPersonState.PersonAttributeValuesState.GetValueOrNull(requiredAttribute.Key);
                            var requiredAttributeDisplayValue = requiredAttribute.FieldType.Field.FormatValue(lRequiredAttributes, attributeValue != null ? attributeValue.Value : null, requiredAttribute.QualifierValues, true);
                            descriptionList.Add(requiredAttribute.Name, requiredAttributeDisplayValue);
                        }

                        lRequiredAttributes.Text = descriptionList.Html;
                    }
                }

                var deleteCell = (e.Row.Cells[_deleteFieldIndex] as DataControlFieldCell).Controls[0];
                if (deleteCell != null)
                {
                    // only support deleting people that haven't been saved to the database yet
                    deleteCell.Visible = !familyPersonState.PersonId.HasValue;
                }
            }
        }

        /// <summary>
        /// Binds the family members grid.
        /// </summary>
        private void BindFamilyMembersGrid()
        {
            var deleteField = gFamilyMembers.ColumnsOfType<DeleteField>().FirstOrDefault();
            _deleteFieldIndex = gFamilyMembers.Columns.IndexOf(deleteField);

            _knownRelationshipLookup = CurrentCheckInState.CheckInType.Registration.KnownRelationships.ToDictionary(k => k.Key, v => v.Value);
            gFamilyMembers.DataSource = this.EditFamilyState.FamilyPersonListState.Where(a => a.IsDeleted == false).ToList();
            gFamilyMembers.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the btnSaveFamily control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSaveFamily_Click(object sender, EventArgs e)
        {
            var currentFamily = CurrentCheckInState.CheckIn.Families.FirstOrDefault(a => a.Group.Id == EditFamilyState.GroupId);

            if (CurrentCheckInState == null)
            {
                // OnLoad would have started a 'NavigateToHomePage', so just jump out
                return;
            }

            if (!EditFamilyState.FamilyPersonListState.Any(x => !x.IsDeleted))
            {
                // Saving a new family, but nobody added to family, so just exit
                CancelFamilyEdit(false);
            }

            //if (!EditFamilyState.FamilyPersonListState.Any(x => x.IsAdult))
            //{
            //    // testing to see if we can not allow only children to be saved to a family
            //    lblAdultWarning.Visible = true;

            //    return;
            //}

            if (!this.Page.IsValid)
            {
                return;
            }

            var rockContext = new RockContext();

            // Set the Campus for new families to the Campus of this Kiosk
            int? kioskCampusId = null;
            var deviceLocation = new DeviceService(rockContext).GetSelect(CurrentCheckInState.Kiosk.Device.Id, a => a.Locations.FirstOrDefault());
            if (deviceLocation != null)
            {
                kioskCampusId = deviceLocation.CampusId;
            }

            UpdateFamilyAttributesState();

            SetEditableStateAttributes();

            FamilyRegistrationState.SaveResult saveResult = null;

            rockContext.WrapTransaction(() =>
           {
               saveResult = EditFamilyState.SaveFamilyAndPersonsToDatabase(kioskCampusId, rockContext);
           });

            // Queue up any Workflows that are configured to fire after a new person and/or family is added
            if (saveResult.NewFamilyList.Any())
            {
                var addFamilyWorkflowTypes = CurrentCheckInState.CheckInType.Registration.AddFamilyWorkflowTypes;

                // only fire a NewFamily workflow if the Primary family is new (don't fire workflows for any 'Can Checkin' families that were created)
                var newPrimaryFamily = saveResult.NewFamilyList.FirstOrDefault(a => a.Id == EditFamilyState.GroupId.Value);
                if (newPrimaryFamily != null)
                {
                    foreach (var addFamilyWorkflowType in addFamilyWorkflowTypes)
                    {
                        LaunchWorkflowTransaction launchWorkflowTransaction = new LaunchWorkflowTransaction<Group>(addFamilyWorkflowType.Id, newPrimaryFamily.Id);
                        launchWorkflowTransaction.Enqueue();
                    }
                }
            }

            if (saveResult.NewPersonList.Any())
            {
                var addPersonWorkflowTypes = CurrentCheckInState.CheckInType.Registration.AddPersonWorkflowTypes;
                foreach (var newPerson in saveResult.NewPersonList)
                {
                    foreach (var addPersonWorkflowType in addPersonWorkflowTypes)
                    {
                        LaunchWorkflowTransaction launchWorkflowTransaction = new LaunchWorkflowTransaction<Person>(addPersonWorkflowType.Id, newPerson.Id);
                        launchWorkflowTransaction.Enqueue();
                    }
                }
            }

            if (CurrentCheckInState.CheckInType.Registration.EnableCheckInAfterRegistration)
            {
                upContent.Update();
                mdEditFamily.Hide();

                // un-disable any IdleRedirect blocks
                DisableIdleRedirectBlocks(false);

                

                if (currentFamily == null)
                {
                    // if this is a new family, add it to the Checkin.Families so that the CurrentFamily wil be set to the new family
                    currentFamily = new CheckInFamily() { Selected = true };
                    currentFamily.Group = new GroupService(rockContext).GetNoTracking(EditFamilyState.GroupId.Value).Clone(false);
                    CurrentCheckInState.CheckIn.Families.Add(currentFamily);
                }

                if (currentFamily.Selected)
                {
                    // Save the family address
                    var _rockContext = new RockContext();
                    var groupService = new GroupService(_rockContext);
                    Group primaryFamily = null;
                    Guid? familyGuid = currentFamily.Group.Guid;
                    primaryFamily = groupService.Get(familyGuid.Value);
                    bool saveEmptyValues = true;
                    var homeLocationType = DefinedValueCache.Get(Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME.AsGuid());
                    if (homeLocationType != null)
                    {
                        // Find a location record for the address that was entered
                        var loc = new Location();
                        adrStreetAddress.GetValues(loc);
                        if (adrStreetAddress.Street1.IsNotNullOrWhiteSpace() && loc.City.IsNotNullOrWhiteSpace())
                        {
                            loc = new LocationService(_rockContext).Get(
                                loc.Street1, loc.Street2, loc.City, loc.State, loc.PostalCode, loc.Country, primaryFamily, true);
                        }
                        else
                        {
                            loc = null;
                        }

                        // Check to see if family has an existing home address;
                        var groupLocation = primaryFamily.GroupLocations
                            .FirstOrDefault(l =>
                               l.GroupLocationTypeValueId.HasValue &&
                               l.GroupLocationTypeValueId.Value == homeLocationType.Id);

                        if (loc != null)
                        {
                            if (groupLocation == null || groupLocation.LocationId != loc.Id)
                            {
                                // If family does not currently have a home address or it is different than the one entered, add a new address (move old address to prev)
                                GroupService.AddNewGroupAddress(_rockContext, primaryFamily, homeLocationType.Guid.ToString(), loc, true, "Check-In", true, true);
                            }
                        }
                        else
                        {
                            if (groupLocation != null && saveEmptyValues)
                            {
                                // If an address was not entered, and family has one on record, update it to be a previous address
                                var prevLocationType = DefinedValueCache.Get(Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_PREVIOUS.AsGuid());
                                if (prevLocationType != null)
                                {
                                    groupLocation.GroupLocationTypeValueId = prevLocationType.Id;
                                }
                            }
                        }
                    }

                    currentFamily.People.Clear();

                    // execute the workflow activity that is configured for this block (probably 'Person Search') so that
                    // the checkin state gets updated with any changes we made in Edit Family
                    string workflowActivity = GetAttributeValue("WorkflowActivity");
                    List<string> errorMessages;
                    if (!string.IsNullOrEmpty(workflowActivity))
                    {
                        // just in case this is a new family, or family name or phonenumber was changed, update the search to match the updated values
                        if (CurrentCheckInState.CheckIn.SearchType.Guid == Rock.SystemGuid.DefinedValue.CHECKIN_SEARCH_TYPE_NAME.AsGuid())
                        {
                            var firstFamilyPerson = EditFamilyState.FamilyPersonListState.OrderBy(a => a.IsAdult).FirstOrDefault();
                            if (firstFamilyPerson != null)
                            {
                                CurrentCheckInState.CheckIn.SearchValue = firstFamilyPerson.FullNameForSearch;
                            }
                        }

                        if (CurrentCheckInState.CheckIn.SearchType.Guid == Rock.SystemGuid.DefinedValue.CHECKIN_SEARCH_TYPE_PHONE_NUMBER.AsGuid())
                        {
                            var firstFamilyPersonWithPhone = EditFamilyState.FamilyPersonListState.Where(a => a.MobilePhoneNumber.IsNotNullOrWhiteSpace()).OrderBy(a => a.IsAdult).FirstOrDefault();
                            if (firstFamilyPersonWithPhone != null)
                            {
                                CurrentCheckInState.CheckIn.SearchValue = firstFamilyPersonWithPhone.MobilePhoneNumber;
                            }
                        }


                        ProcessActivity(workflowActivity, out errorMessages);
                    }
                }

                // if the searchBlock is on this page, have it re-search using the person's updated full name
                var searchBlock = this.RockPage.ControlsOfTypeRecursive<CheckInSearchBlock>().FirstOrDefault();

                if (searchBlock != null)
                {
                    var firstFamilyPerson = EditFamilyState.FamilyPersonListState.OrderBy(a => a.IsAdult).FirstOrDefault();
                    string searchString;
                    if (firstFamilyPerson != null)
                    {
                        searchString = firstFamilyPerson.FullNameForSearch;
                    }
                    else
                    {
                        searchString = CurrentCheckInState.CheckIn.SearchValue;
                    }

                    searchBlock.ProcessSearch(searchString);
                }
                else
                {
                    // reload the current page so that other blocks will get updated correctly
                    NavigateToCurrentPageReference();
                }
            }
            else
            {
                upContent.Update();
                NavigateToHomePage();
            }
        }

        /// <summary>
        /// Updates the EditFamilyState.FamilyAttributeValuesState from any values that were viewable/editable in the UI
        /// </summary>
        private void UpdateFamilyAttributesState()
        {
            var fakeFamily = new Group() { GroupTypeId = GroupTypeCache.GetFamilyGroupType().Id, Id = EditFamilyState.GroupId ?? 0 };
            fakeFamily.LoadAttributes();
            Rock.Attribute.Helper.GetEditValues(phFamilyAttributes, fakeFamily);

            EditFamilyState.FamilyAttributeValuesState = fakeFamily.AttributeValues.ToDictionary(k => k.Key, v => v.Value);
        }

        /// <summary>
        /// Sets the editable state attributes for the EditFamilyState and each FamilyPersonState, so that only Editable attributes are saved to the database
        /// </summary>
        private void SetEditableStateAttributes()
        {

            EditFamilyState.EditableFamilyAttributes = RequiredAttributesForFamilies.Union(OptionalAttributesForFamilies).Select(a => a.Id).ToList();

            // only include the attributes for each person that are included in Required/Optional attributes so that we don't accidentally delete values for attributes are aren't included
            foreach (var familyPersonState in EditFamilyState.FamilyPersonListState)
            {
                if (familyPersonState.IsAdult)
                {
                    familyPersonState.EditableAttributes = RequiredAttributesForAdults.Union(OptionalAttributesForAdults).Select(a => a.Id).ToList();
                }
                else
                {
                    familyPersonState.EditableAttributes = RequiredAttributesForChildren.Union(OptionalAttributesForChildren).Select(a => a.Id).ToList();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancelFamily control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelFamily_Click(object sender, EventArgs e)
        {
            CancelFamilyEdit(true);
        }

        /// <summary>
        /// Cancels the family edit.
        /// </summary>
        /// <param name="promptIfChangesMade">if set to <c>true</c> [prompt if changes made].</param>
        private void CancelFamilyEdit(bool promptIfChangesMade)
        {
            if (promptIfChangesMade)
            {
                UpdateFamilyAttributesState();
                int currentEditFamilyStateHash = EditFamilyState.GetStateHash();
                if (_initialEditFamilyStateHash != currentEditFamilyStateHash)
                {
                    hfShowCancelEditPrompt.Value = "1";
                    upContent.Update();
                    return;
                }
            }

            upContent.Update();
            mdEditFamily.Hide();

            // un-disable any IdleRedirect blocks
            DisableIdleRedirectBlocks(false);
        }

        /// <summary>
        /// Handles the Click event of the DeleteFamilyMember control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.RowEventArgs"/> instance containing the event data.</param>
        protected void DeleteFamilyMember_Click(object sender, Rock.Web.UI.Controls.RowEventArgs e)
        {
            if (CurrentCheckInState == null)
            {
                // OnLoad would have started a 'NavigateToHomePage', so just jump out
                return;
            }

            var familyPersonState = EditFamilyState.FamilyPersonListState.FirstOrDefault(a => a.GroupMemberGuid == (Guid)e.RowKeyValue);
            familyPersonState.IsDeleted = true;
            BindFamilyMembersGrid();
        }

        /// <summary>
        /// Handles the Click event of the EditFamilyMember control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.RowEventArgs"/> instance containing the event data.</param>
        protected void EditFamilyMember_Click(object sender, Rock.Web.UI.Controls.RowEventArgs e)
        {
            EditGroupMember((Guid)e.RowKeyValue);
        }

        /// <summary>
        /// Handles the Click event of the btnAddPerson control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnAddPerson_Click(object sender, System.EventArgs e)
        {
            EditGroupMember(null);
        }

        /// <summary>
        /// Shows the family view.
        /// </summary>
        private void ShowFamilyView()
        {
            pnlEditPerson.Visible = false;
            pnlEditFamily.Visible = true;
            mdEditFamily.Title = EditFamilyState.FamilyName;
            // Set the address from the family
            var homeLocationType = DefinedValueCache.Get(Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME.AsGuid());
            if (homeLocationType != null)
            {
                RockContext _rockContext = new RockContext();
                var currentFamily = CurrentCheckInState.CheckIn.Families.FirstOrDefault(a => a.Group.Id == EditFamilyState.GroupId);
                if(currentFamily != null)
                {
                    var familyAddress = new GroupLocationService(_rockContext).Queryable()
                                            .Where(l => l.Group.GroupTypeId == currentFamily.Group.GroupTypeId
                                                   && l.GroupLocationTypeValueId == homeLocationType.Id
                                                   && l.Group.Id == currentFamily.Group.Id)
                                            .FirstOrDefault();

                    if (familyAddress != null)
                    {
                        adrStreetAddress.SetValues(familyAddress.Location);
                    }
                }
            }
            else
            {
                adrStreetAddress.SetValues(null);
            }
            upContent.Update();

            
        }

        #endregion Edit Family

        #region Edit Person

        /// <summary>
        /// Edits the group member.
        /// </summary>
        /// <param name="groupMemberId">The group member identifier.</param>
        private void EditGroupMember(Guid? groupMemberGuid)
        {
            var rockContext = new RockContext();

            FamilyRegistrationState.FamilyPersonState familyPersonState = null;

            if (groupMemberGuid.HasValue)
            {
                familyPersonState = EditFamilyState.FamilyPersonListState.FirstOrDefault(a => a.GroupMemberGuid == groupMemberGuid);
            }

            if (familyPersonState == null)
            {
                // create a new temp record so we can set the defaults for the new person
                familyPersonState = FamilyRegistrationState.FamilyPersonState.FromTemporaryPerson();
                familyPersonState.GroupMemberGuid = Guid.NewGuid();

                // default Gender to Unknown so that it'll prompt to select gender if it hasn't been selected yet
                familyPersonState.Gender = Gender.Unknown;
                familyPersonState.IsAdult = false;
                familyPersonState.IsMarried = false;
                familyPersonState.RecordStatusValueId = DefinedValueCache.Get(Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE.AsGuid()).Id;
                familyPersonState.ConnectionStatusValueId = CurrentCheckInState.CheckInType.Registration.DefaultPersonConnectionStatusId;

                var firstFamilyMember = EditFamilyState.FamilyPersonListState.FirstOrDefault();
                if (firstFamilyMember != null)
                {
                    // if this family already has a person, default the LastName to the first person
                    familyPersonState.LastName = firstFamilyMember.LastName;
                }
            }

            hfGroupMemberGuid.Value = familyPersonState.GroupMemberGuid.ToString();
            tglAdultChild.Checked = familyPersonState.IsAdult;

            // only allow Adult/Child and Relationship to be changed for newly added people
            //tglAdultChild.Visible = !familyPersonState.PersonId.HasValue;

            ddlChildRelationShipToAdult.Visible = !familyPersonState.PersonId.HasValue;
            lChildRelationShipToAdultReadOnly.Visible = familyPersonState.PersonId.HasValue;

            ShowControlsForRole(tglAdultChild.Checked);
            if (familyPersonState.Gender == Gender.Unknown)
            {
                bgGender.SelectedValue = null;
            }
            else
            {
                bgGender.SetValue(familyPersonState.Gender.ConvertToInt());
            }
            tglAdultMaritalStatus.Checked = familyPersonState.IsMarried;

            ddlChildRelationShipToAdult.Items.Clear();

            foreach (var relationShipType in CurrentCheckInState.CheckInType.Registration.KnownRelationships)
            {
                ddlChildRelationShipToAdult.Items.Add(new ListItem(relationShipType.Value, relationShipType.Key.ToString()));
            }

            ddlChildRelationShipToAdult.SetValue(familyPersonState.ChildRelationshipToAdult);
            lChildRelationShipToAdultReadOnly.Text = CurrentCheckInState.CheckInType.Registration.KnownRelationships.GetValueOrNull(familyPersonState.ChildRelationshipToAdult);

            // Only show the RecordStatus if they aren't currently active
            dvpRecordStatus.Visible = false;
            if (familyPersonState.PersonId.HasValue)
            {
                var personRecordStatusValueId = new PersonService(rockContext).GetSelect(familyPersonState.PersonId.Value, a => a.RecordStatusValueId);
                if (personRecordStatusValueId.HasValue)
                {
                    dvpRecordStatus.Visible = personRecordStatusValueId != _personRecordStatusActiveId;
                }
            }

            tbFirstName.Focus();
            tbFirstName.Text = familyPersonState.FirstName;
            tbLastName.Text = familyPersonState.LastName;
            tbAlternateID.Text = familyPersonState.AlternateID;

            dvpSuffix.DefinedTypeId = DefinedTypeCache.Get(Rock.SystemGuid.DefinedType.PERSON_SUFFIX.AsGuid()).Id;
            dvpSuffix.SetValue(familyPersonState.SuffixValueId);

            dvpRecordStatus.DefinedTypeId = DefinedTypeCache.Get(Rock.SystemGuid.DefinedType.PERSON_RECORD_STATUS.AsGuid()).Id;
            dvpRecordStatus.SetValue(familyPersonState.RecordStatusValueId);
            hfConnectionStatus.Value = familyPersonState.ConnectionStatusValueId.ToString();

            var mobilePhoneNumber = familyPersonState.MobilePhoneNumber;
            if (mobilePhoneNumber != null)
            {
                pnMobilePhone.CountryCode = familyPersonState.MobilePhoneCountryCode;
                pnMobilePhone.Number = mobilePhoneNumber;
            }
            else
            {
                pnMobilePhone.CountryCode = string.Empty;
                pnMobilePhone.Number = string.Empty;
            }

            tbEmail.Text = familyPersonState.Email;
            dpBirthDate.SelectedDate = familyPersonState.BirthDate;
            if (familyPersonState.GradeOffset.HasValue)
            {
                gpGradePicker.SetValue(familyPersonState.GradeOffset);
            }
            else
            {
                gpGradePicker.SelectedValue = null;
            }

            CreateDynamicPersonControls(familyPersonState, true);

            ShowPersonView(familyPersonState);
        }

        /// <summary>
        /// Shows the person view.
        /// </summary>
        /// <param name="familyPersonState">State of the family person.</param>
        private void ShowPersonView(FamilyRegistrationState.FamilyPersonState familyPersonState)
        {
            pnlEditFamily.Visible = false;
            pnlEditPerson.Visible = true;
            mdEditFamily.Title = familyPersonState.FullName;
            upContent.Update();
        }

        /// <summary>
        /// Shows the controls for role.
        /// </summary>
        /// <param name="isAdult">if set to <c>true</c> [is adult].</param>
        private void ShowControlsForRole(bool isAdult)
        {
            if (CurrentCheckInState == null)
            {
                // OnLoad would have started a 'NavigateToHomePage', so just jump out
                return;
            }

            tglAdultMaritalStatus.Visible = isAdult;
            //dpBirthDate.Visible = !isAdult;
            dpBirthDate.Required = !isAdult;
            gpGradePicker.Visible = !isAdult;
            tbEmail.Visible = isAdult;
            tbEmail.Required = isAdult;
            pnMobilePhone.Visible = isAdult;
            pnMobilePhone.Required = isAdult;
            pnlChildRelationshipToAdult.Visible = !isAdult;

            tbAlternateID.Visible = (isAdult && CurrentCheckInState.CheckInType.Registration.DisplayAlternateIdFieldforAdults) || (!isAdult && CurrentCheckInState.CheckInType.Registration.DisplayAlternateIdFieldforChildren);
            phAdultAttributes.Visible = isAdult;
            phChildAttributes.Visible = !isAdult;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the tglAdultChild control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void tglAdultChild_CheckedChanged(object sender, EventArgs e)
        {
            ShowControlsForRole(tglAdultChild.Checked);
        }

        /// <summary>
        /// Handles the Click event of the btnCancelPerson control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelPerson_Click(object sender, EventArgs e)
        {
            if (CurrentCheckInState == null)
            {
                // OnLoad would have started a 'NavigateToHomePage', so just jump out
                return;
            }

            ShowFamilyView();

            if (!EditFamilyState.FamilyPersonListState.Any())
            {
                // cancelling on adding first person to family, so cancel adding the family too
                CancelFamilyEdit(false);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDonePerson control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnDonePerson_Click(object sender, EventArgs e)
        {
            if (CurrentCheckInState == null)
            {
                // OnLoad would have started a 'NavigateToHomePage', so just jump out
                return;
            }

            Guid groupMemberGuid = hfGroupMemberGuid.Value.AsGuid();
            var familyPersonState = EditFamilyState.FamilyPersonListState.FirstOrDefault(a => a.GroupMemberGuid == groupMemberGuid);
            if (familyPersonState == null)
            {
                // new person added
                familyPersonState = FamilyRegistrationState.FamilyPersonState.FromTemporaryPerson();
                familyPersonState.GroupMemberGuid = groupMemberGuid;
                familyPersonState.PersonId = null;
                EditFamilyState.FamilyPersonListState.Add(familyPersonState);
            }

            familyPersonState.RecordStatusValueId = dvpRecordStatus.SelectedValue.AsIntegerOrNull();
            familyPersonState.ConnectionStatusValueId = hfConnectionStatus.Value.AsIntegerOrNull();
            familyPersonState.IsAdult = tglAdultChild.Checked;
            

            familyPersonState.Gender = bgGender.SelectedValueAsEnumOrNull<Gender>() ?? Gender.Unknown;
            familyPersonState.ChildRelationshipToAdult = ddlChildRelationShipToAdult.SelectedValue.AsInteger();

            familyPersonState.InPrimaryFamily = CurrentCheckInState.CheckInType.Registration.KnownRelationshipsSameFamily.Any(k => k.Key == familyPersonState.ChildRelationshipToAdult);
            familyPersonState.CanCheckIn = (familyPersonState.InPrimaryFamily == false)
                && CurrentCheckInState.CheckInType.Registration.KnownRelationshipsCanCheckin.Any(k => k.Key == familyPersonState.ChildRelationshipToAdult);

            familyPersonState.IsMarried = tglAdultMaritalStatus.Checked;
            familyPersonState.FirstName = tbFirstName.Text.FixCase();
            familyPersonState.LastName = tbLastName.Text.FixCase();
            familyPersonState.SuffixValueId = dvpSuffix.SelectedValue.AsIntegerOrNull();

            familyPersonState.MobilePhoneNumber = pnMobilePhone.Number;
            familyPersonState.MobilePhoneCountryCode = pnMobilePhone.CountryCode;
            familyPersonState.BirthDate = dpBirthDate.SelectedDate;
            familyPersonState.Email = tbEmail.Text;

           

                if (gpGradePicker.SelectedGradeValue != null)
                {
                    familyPersonState.GradeOffset = gpGradePicker.SelectedGradeValue.Value.AsIntegerOrNull();
                }
                else
                {
                    familyPersonState.GradeOffset = null;
                }

                familyPersonState.AlternateID = tbAlternateID.Text;
                var fakePerson = new Person() { Id = familyPersonState.PersonId ?? 0 };
                fakePerson.LoadAttributes();

                if (familyPersonState.IsAdult)
                {
                    Rock.Attribute.Helper.GetEditValues(phAdultAttributes, fakePerson);
                }
                else
                {
                    Rock.Attribute.Helper.GetEditValues(phChildAttributes, fakePerson);
                }

                familyPersonState.PersonAttributeValuesState = fakePerson.AttributeValues.ToDictionary(k => k.Key, v => v.Value);

                ShowFamilyView();

                BindFamilyMembersGrid();
            }
        }

            #endregion Edit Person
        }