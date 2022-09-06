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
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Displays a bootstrap badge
    /// </summary>
    public class PreRegistrationChildRow : CompositeControl
    {
        private RockLiteral _lNickName;
        private RockLiteral _lLastName;
        private RockTextBox _tbNickName;
        private RockTextBox _tbLastName;
        private DefinedValuePicker _ddlSuffix;
        private RockDropDownList _ddlGender;
        private BirthdayPicker _bpBirthdate;
        private GradePicker _ddlGradePicker;
        private PhoneNumberBox _pnbMobile;
        private EmailBox _ebEmail;
        private RockDropDownList _ddlRelationshipType;
        private PlaceHolder _phAttributes;
        private RockRadioButtonList _rblCommunicationPreference;
        private LinkButton _lbDelete;
        private ImageEditor _imgProfile;

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption
        {
            get { return ViewState["Caption"] as string ?? "Child"; }
            set { ViewState["Caption"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show suffix].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show suffix]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSuffix
        {
            get
            {
                EnsureChildControls();
                return _ddlSuffix.Visible;
            }
            set
            {
                EnsureChildControls();
                _ddlSuffix.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show gender].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show gender]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowGender
        {
            get
            {
                EnsureChildControls();
                return _ddlGender.Visible;
            }
            set
            {
                EnsureChildControls();
                _ddlGender.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require gender].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require gender]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireGender
        {
            get
            {
                EnsureChildControls();
                return _ddlGender.Required;
            }
            set
            {
                EnsureChildControls();
                _ddlGender.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show birth date].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show birth date]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowBirthDate
        {
            get
            {
                EnsureChildControls();
                return _bpBirthdate.Visible;
            }
            set
            {
                EnsureChildControls();
                _bpBirthdate.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require birth date].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require birth date]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireBirthDate
        {
            get
            {
                EnsureChildControls();
                return _bpBirthdate.Required;
            }
            set
            {
                EnsureChildControls();
                _bpBirthdate.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show grade].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show grade]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowGrade
        {
            get
            {
                EnsureChildControls();
                return _ddlGradePicker.Visible;
            }
            set
            {
                EnsureChildControls();
                _ddlGradePicker.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require grade].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require grade]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireGrade
        {
            get
            {
                EnsureChildControls();
                return _ddlGradePicker.Required;
            }
            set
            {
                EnsureChildControls();
                _ddlGradePicker.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show mobile phone].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show mobile phone]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowMobilePhone
        {
            get
            {
                EnsureChildControls();
                return _pnbMobile.Visible;
            }
            set
            {
                EnsureChildControls();
                _pnbMobile.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require mobile phone].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require mobile phone]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireMobilePhone
        {
            get
            {
                EnsureChildControls();
                return _pnbMobile.Required;
            }
            set
            {
                EnsureChildControls();
                _pnbMobile.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show email address].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show email address]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowEmailAddress
        {
            get
            {
                EnsureChildControls();
                return _ebEmail.Visible;
            }
            set
            {
                EnsureChildControls();
                _ebEmail.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require email address].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require email address]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireEmailAddress
        {
            get
            {
                EnsureChildControls();
                return _ebEmail.Required;
            }
            set
            {
                EnsureChildControls();
                _ebEmail.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the communication preference control.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the communication preference control should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowCommunicationPreference
        {
            get
            {
                EnsureChildControls();
                return _rblCommunicationPreference.Visible;
            }
            set
            {
                EnsureChildControls();
                _rblCommunicationPreference.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show profile photo].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show profile photo]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowProfilePhoto
        {
            get
            {
                EnsureChildControls();
                return _imgProfile.Visible;
            }
            set
            {
                EnsureChildControls();
                _imgProfile.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require profile photo].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [require profile photo]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireProfilePhoto
        {
            get
            {
                EnsureChildControls();
                return _imgProfile.Required;
            }
            set
            {
                EnsureChildControls();
                _imgProfile.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets the relationship type list.
        /// </summary>
        /// <value>
        /// The relationship type list.
        /// </value>
        public Dictionary<int, string> RelationshipTypeList
        {
            get
            {
                if ( _relationshipTypeList == null )
                {
                    _relationshipTypeList = ViewState["RelationshipTypeList"] as Dictionary<int, string>;
                    if ( _relationshipTypeList == null )
                    {
                        _relationshipTypeList = new Dictionary<int, string>();
                    }
                }
                return _relationshipTypeList;
            }
            set
            {
                _relationshipTypeList = value;
                ViewState["RelationshipTypeList"] = _relationshipTypeList;
                RecreateChildControls();
            }
        }
        /// <summary>
        /// The relationship type list
        /// </summary>
        private Dictionary<int, string> _relationshipTypeList = null;

        /// <summary>
        /// Gets or sets the attribute list.
        /// </summary>
        /// <value>
        /// The attribute list.
        /// </value>
        public List<AttributeCache> AttributeList
        {
            get
            {
                if ( _attributeList == null )
                {
                    _attributeList = ViewState["AttributeList"] as List<AttributeCache>;
                    if ( _attributeList == null )
                    {
                        _attributeList = new List<AttributeCache>();
                    }
                }
                return _attributeList;
            }
            set
            {
                _attributeList = value;
                ViewState["AttributeList"] = _attributeList;
                RecreateChildControls();
            }
        }
        private List<AttributeCache> _attributeList = null;

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        public int PersonId
        {
            get { return ViewState["PersonId"] as int? ?? 0; }
            set { ViewState["PersonId"] = value; }
        }

        /// <summary>
        /// Gets or sets the person GUID.
        /// </summary>
        /// <value>
        /// The person GUID.
        /// </value>
        public Guid? PersonGuid
        {
            get { return ViewState["PersonGuid"] as Guid?; }
            set { ViewState["PersonGuid"] = value; }
        }

        /// <summary>
        /// Gets or sets the nick name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string NickName
        {
            get
            {
                EnsureChildControls();
                return _tbNickName.Text;
            }

            set
            {
                EnsureChildControls();
                _lNickName.Text = value;
                _tbNickName.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get
            {
                EnsureChildControls();
                return _tbLastName.Text;
            }

            set
            {
                EnsureChildControls();
                _lLastName.Text = value;
                _tbLastName.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the suffix value id.
        /// </summary>
        /// <value>
        /// The suffix value id.
        /// </value>
        public int? SuffixValueId
        {
            get
            {
                EnsureChildControls();
                return _ddlSuffix.SelectedValueAsInt();
            }

            set
            {
                EnsureChildControls();
                _ddlSuffix.SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public Gender Gender
        {
            get
            {
                EnsureChildControls();
                return _ddlGender.SelectedValueAsEnum<Gender>( Gender.Unknown );
            }
            set
            {
                EnsureChildControls();
                _ddlGender.SetValue( value.ConvertToInt() );
            }
        }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime? BirthDate
        {
            get
            {
                EnsureChildControls();
                return _bpBirthdate.SelectedDate;
            }

            set
            {
                EnsureChildControls();
                _bpBirthdate.SelectedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the grade offset.
        /// </summary>
        /// <value>
        /// The grade offset.
        /// </value>
        public int? GradeOffset
        {
            get
            {
                EnsureChildControls();
                return _ddlGradePicker.SelectedValueAsInt( false );
            }

            set
            {
                EnsureChildControls();
                _ddlGradePicker.SetValue( value.HasValue ? value.Value.ToString() : "" );
            }
        }

        /// <summary>
        /// Gets or sets the relation to guardian value id.
        /// </summary>
        /// <value>
        /// The relation to guardian value id.
        /// </value>
        public int? RelationshipType
        {
            get
            {
                EnsureChildControls();
                return _ddlRelationshipType.SelectedValueAsInt();
            }

            set
            {
                EnsureChildControls();
                _ddlRelationshipType.SetValue( value );
            }
        }

        /// <summary>
        /// Gets or sets the mobile phone number.
        /// </summary>
        /// <value>
        /// The mobile phone number.
        /// </value>
        public string MobilePhone
        {
            get
            {
                EnsureChildControls();
                return _pnbMobile.Number;
            }
            set
            {
                EnsureChildControls();
                _pnbMobile.Number = value;
            }
        }

        /// <summary>
        /// Gets or sets the mobile phone country code.
        /// </summary>
        /// <value>
        /// The cell mobile country code.
        /// </value>
        public string MobilePhoneCountryCode
        {
            get
            {
                EnsureChildControls();
                return _pnbMobile.CountryCode;
            }
            set
            {
                _pnbMobile.CountryCode = value;
                EnsureChildControls();
            }
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress
        {
            get
            {
                EnsureChildControls();
                return _ebEmail.Text;
            }
            set
            {
                EnsureChildControls();
                _ebEmail.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the communication preference.
        /// </summary>
        /// <value>
        /// The communication preference.
        /// </value>
        public CommunicationType CommunicationPreference
        {
            get
            {
                EnsureChildControls();
                return ((CommunicationType?) _rblCommunicationPreference.SelectedValue.AsIntegerOrNull()) ?? CommunicationType.Email;
            }
            set
            {
                EnsureChildControls();
                _rblCommunicationPreference.SelectedValue = value.ConvertToInt().ToString();
            }
        }

        /// <summary>
        /// Gets or sets the profile photo binary identifier.
        /// </summary>
        /// <value>
        /// The profile photo binary identifier.
        /// </value>
        public int? ProfilePhotoId
        {
            get
            {
                EnsureChildControls();
                return _imgProfile.BinaryFileId;
            }
            set
            {
                EnsureChildControls();
                _imgProfile.BinaryFileId = value;
            }
        }

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get
            {
                return _tbNickName.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                _tbNickName.ValidationGroup = value;
                _tbLastName.ValidationGroup = value;
                _ddlSuffix.ValidationGroup = value;
                _ddlGender.ValidationGroup = value;
                _bpBirthdate.ValidationGroup = value;
                _ddlGradePicker.ValidationGroup = value;
                _pnbMobile.ValidationGroup = value;
                _ebEmail.ValidationGroup = value;
                _rblCommunicationPreference.ValidationGroup = value;
                _ddlRelationshipType.ValidationGroup = value;
                foreach ( var ctrl in _phAttributes.Controls )
                {
                    var rockCtrl = ctrl as IRockControl;
                    if ( rockCtrl != null )
                    {
                        rockCtrl.ValidationGroup = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        public List<string> ValidationErrors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public int Columns { get; set; } = 4;

        /// <summary>
        /// Returns true if all of the requires fields have a value and the birthdate is successfully parsed
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                EnsureChildControls();
                ValidationErrors.Clear();

                if( _tbNickName.Required && _tbNickName.Text.IsNullOrWhiteSpace())
                {
                    return false;
                }

                if( _tbLastName.Required && _tbLastName.Text.IsNullOrWhiteSpace())
                {
                    return false;
                }

                if ( _ddlGender.Required && _ddlGender.SelectedIndex == 0 )
                {
                    return false;
                }

                if ( _bpBirthdate.Required && _bpBirthdate.SelectedDate == null)
                {
                    return false;
                }

                if( _ddlGradePicker.Required && _ddlGradePicker.SelectedIndex == 0)
                {
                    return false;
                }

                if(  _ebEmail.Required && !_ebEmail.IsValid )
                {
                    return false;
                }

                if ( _ebEmail.Text.IsNotNullOrWhiteSpace() && !_ebEmail.IsValid )
                {
                    return false;
                }

                if ( !_pnbMobile.IsValid )
                {
                    return false;
                }

                if ( _imgProfile.Required && _imgProfile.BinaryFileId == null )
                {
                    return false;
                }

                var communicationPreference = (CommunicationType)_rblCommunicationPreference.SelectedValue.AsInteger();
                if (communicationPreference == CommunicationType.SMS && _pnbMobile.Visible && _pnbMobile.Number.IsNullOrWhiteSpace())
                {
                    ValidationErrors.Add( "SMS Number is required if SMS communication preference is selected." );
                    return false;
                }

                // This one will never have a null or blank value, so no need to validate it.
                //if( _ddlRelationshipType.Required && _ddlRelationshipType.SelectedIndex == 0 )
                //{
                //    return false;
                //}

                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewGroupMembersRow" /> class.
        /// </summary>
        public PreRegistrationChildRow()
            : base()
        {
            _lNickName = new RockLiteral();
            _lLastName = new RockLiteral();
            _tbNickName = new RockTextBox();
            _tbLastName = new RockTextBox();
            _ddlSuffix = new DefinedValuePicker();
            _ddlGender = new RockDropDownList();
            _bpBirthdate = new BirthdayPicker();
            _ddlGradePicker = new GradePicker { UseAbbreviation = true, UseGradeOffsetAsValue = true };
            _ddlGradePicker.Label = string.Empty;
            _pnbMobile = new PhoneNumberBox();
            _ebEmail = new EmailBox();
            _ddlRelationshipType = new RockDropDownList();
            _phAttributes = new PlaceHolder();
            _lbDelete = new LinkButton();
            _rblCommunicationPreference = new RockRadioButtonList();
            _imgProfile = new ImageEditor() { RequiredErrorMessage = "Profile photo is required for the child." };
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();

            _lNickName.ID = "_lNickName";
            _lLastName.ID = "_lLastName";
            _tbNickName.ID = "_tbNickName";
            _tbLastName.ID = "_tbLastName";
            _ddlSuffix.ID = "_ddlSuffix";
            _ddlGender.ID = "_ddlGender";
            _bpBirthdate.ID = "_bpBirthdate";
            _ddlGradePicker.ID = "_ddlGrade";
            _pnbMobile.ID = "_pnbPhone";
            _ebEmail.ID = "_ebEmail";
            _ddlRelationshipType.ID = "_ddlRelationshipType";
            _phAttributes.ID = "_phAttributes";
            _lbDelete.ID = "_lbDelete";
            _rblCommunicationPreference.ID = "_rblCommunicationPreference";
            _imgProfile.ID = "_imgProfile";

            Controls.Add( _lNickName );
            Controls.Add( _lLastName );
            Controls.Add( _tbNickName );
            Controls.Add( _tbLastName );
            Controls.Add( _ddlSuffix );
            Controls.Add( _bpBirthdate );
            Controls.Add( _ddlGender );
            Controls.Add( _ddlGradePicker );
            Controls.Add( _pnbMobile );
            Controls.Add( _ebEmail );
            Controls.Add( _rblCommunicationPreference );
            Controls.Add( _ddlRelationshipType );
            Controls.Add( _phAttributes );
            Controls.Add( _lbDelete );
            Controls.Add( _imgProfile );

            _lNickName.Label = "First Name";

            _lLastName.Label = "Last Name";

            _tbNickName.CssClass = "form-control";
            _tbNickName.Required = true;
            _tbNickName.RequiredErrorMessage = "First Name is required for all children";
            _tbNickName.Label = "First Name";

            _tbLastName.Required = true;
            _tbLastName.RequiredErrorMessage = "Last Name is required for all children";
            _tbLastName.Label = "Last Name";

            _ddlSuffix.CssClass = "form-control";
            _ddlSuffix.Label = "Suffix";
            string suffixValue = _ddlSuffix.SelectedValue;
            _ddlSuffix.Items.Clear();
            _ddlSuffix.DefinedTypeId = DefinedTypeCache.Get( Rock.SystemGuid.DefinedType.PERSON_SUFFIX.AsGuid() ).Id;
            if ( !string.IsNullOrEmpty( suffixValue ) )
            {
                _ddlSuffix.SelectedValue = suffixValue;
            }

            _ddlGender.RequiredErrorMessage = "Gender is required for all children";
            _ddlGender.Label = "Gender";
            string genderValue = _ddlGender.SelectedValue;
            _ddlGender.Items.Clear();
            _ddlGender.BindToEnum<Gender>( true, new Gender[] { Gender.Unknown } );
            if ( !string.IsNullOrEmpty( genderValue ) )
            {
                _ddlGender.SelectedValue = genderValue;
            }

            _bpBirthdate.RequiredErrorMessage = "Birth date is required for all children";
            _bpBirthdate.Label = "Birth Date";

            _ddlGradePicker.CssClass = "form-control";
            _ddlGradePicker.RequiredErrorMessage = "Grade is required for all children";
            _ddlGradePicker.Label = "Grade";

            /*
                    06/03/2020 - SK
                    This is not needed because the PhoneNumberBox control already adds form-control in the correct spot
             */
            //_pnbMobile.CssClass = "form-control";

            _pnbMobile.Label = "Mobile Phone";
            _pnbMobile.RequiredErrorMessage = "A valid phone number is required for all children.";

            _ebEmail.Label = "Email Address";

            _rblCommunicationPreference.Label = "Communication Preference";
            _rblCommunicationPreference.RepeatDirection = RepeatDirection.Horizontal;

            if ( _rblCommunicationPreference.Items.Count == 0 )
            {
                _rblCommunicationPreference.Items.Add( new ListItem( "Email", "1" ) );
                _rblCommunicationPreference.Items.Add( new ListItem( "SMS", "2" ) );
            }

            _ddlRelationshipType.CssClass = "form-control";
            _ddlRelationshipType.Required = true;
            _ddlRelationshipType.Label = "Relationship to Adult";
            _ddlRelationshipType.DataValueField = "Key";
            _ddlRelationshipType.DataTextField = "Value";
            string relationshipTypeValue = _ddlRelationshipType.SelectedValue;
            _ddlRelationshipType.Items.Clear();
            _ddlRelationshipType.DataSource = RelationshipTypeList;
            _ddlRelationshipType.DataBind();
            if ( !string.IsNullOrEmpty( relationshipTypeValue ) )
            {
                _ddlRelationshipType.SelectedValue = relationshipTypeValue;
            }

            foreach ( var attribute in AttributeList )
            {
                attribute.AddControl( _phAttributes.Controls, "", this.ValidationGroup, false, true, attribute.IsRequired );
            }

            _lbDelete.CssClass = "btn btn-xs btn-square btn-danger pull-right";
            _lbDelete.Click += lbDelete_Click;
            _lbDelete.CausesValidation = false;
            _lbDelete.Text = "<i class='fa fa-times'></i>";
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-right" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                _lbDelete.RenderControl( writer );
                writer.RenderEndTag();

                writer.RenderBeginTag( HtmlTextWriterTag.H4 );
                writer.Write( Caption );
                writer.RenderEndTag();

                // Relationship
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle(6) );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                _ddlRelationshipType.RenderControl( writer );
                writer.RenderEndTag();
                writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 6 ) ); // filler/blocker column
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                writer.RenderEndTag();
                writer.RenderEndTag(); // end Relationship row

                writer.AddAttribute( "rowid", ID );
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "row clearfix" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );

                bool existingPerson = ( this.PersonId > 0 );
                _lNickName.Visible = existingPerson;
                _lLastName.Visible = existingPerson;
                _tbNickName.Visible = !existingPerson;
                _tbLastName.Visible = !existingPerson;

                writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                _lNickName.RenderControl( writer );
                _tbNickName.RenderControl( writer );
                writer.RenderEndTag();

                writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                _lLastName.RenderControl( writer );
                _tbLastName.RenderControl( writer );
                writer.RenderEndTag();

                if ( this.ShowGender )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _ddlGender.RenderControl( writer );
                    writer.RenderEndTag();
                }

                if ( this.ShowSuffix )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _ddlSuffix.RenderControl( writer );
                    writer.RenderEndTag();
                }

                if ( this.ShowBirthDate )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 6 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _bpBirthdate.RenderControl( writer );
                    writer.RenderEndTag();
                }

                if ( this.ShowGrade )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _ddlGradePicker.RenderControl( writer );
                    writer.RenderEndTag();
                }

                if ( this.ShowMobilePhone )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _pnbMobile.RenderControl( writer );
                    writer.RenderEndTag();
                }

                if ( this.ShowEmailAddress )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 6 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _ebEmail.RenderControl( writer );
                    writer.RenderEndTag();
                }

                if ( this.ShowCommunicationPreference )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 6 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _rblCommunicationPreference.RenderControl( writer );
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();

                if ( this.ShowProfilePhoto )
                {
                    // Create row for profile photo
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 6 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    _imgProfile.RenderControl( writer );
                    writer.RenderEndTag();
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 6 ) ); // filler/blocker column
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    // end Relationship row
                }

                // Attributes row
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );

                foreach ( Control attributeCtrl in _phAttributes.Controls )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, GetColumnStyle( 3 ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    attributeCtrl.RenderControl( writer );
                    writer.RenderEndTag();
                }

                writer.RenderEndTag(); // End Attributes row

                writer.RenderBeginTag( HtmlTextWriterTag.Hr );
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Binds the gender.
        /// </summary>
        private void BindGender()
        {
            string selectedValue = _ddlGender.SelectedValue;

            _ddlGender.Items.Clear();
            _ddlGender.BindToEnum<Gender>( !RequireGender, new Gender[] { Gender.Unknown } );

            if ( !string.IsNullOrEmpty( selectedValue ) )
            {
                _ddlGender.SelectedValue = selectedValue;
            }
        }

        /// <summary>
        /// Sets the attribute values.
        /// </summary>
        /// <param name="child">The child.</param>
        public void SetAttributeValues( PreRegistrationChild child )
        {
            EnsureChildControls();

            int i = 0;
            foreach ( var attribute in AttributeList )
            {

                attribute.FieldType.Field.SetEditValue( attribute.GetControl( _phAttributes.Controls[i] ), attribute.QualifierValues, child.GetAttributeValue( attribute.Key ) );
                i++;
            }
        }

        /// <summary>
        /// Gets the attribute values.
        /// </summary>
        /// <param name="child">The child.</param>
        public void GetAttributeValues( PreRegistrationChild child )
        {
            EnsureChildControls();
            int i = 0;
            foreach ( var attribute in AttributeList )
            {
                child.SetAttributeValue( attribute.Key, attribute.FieldType.Field.GetEditValue( attribute.GetControl( _phAttributes.Controls[i] ), attribute.QualifierValues ) );
            }
        }

        private string GetColumnStyle( int columns )
        {
            if ( ( columns != 3 && columns != 6 ) || Columns == 4 )
            {
                return "col-sm-" + columns.ToString();
            }

            if ( columns == 6 )
            {
                return "col-sm-" + columns.ToString();
            }

            return "col-sm-" + ( columns * 2 ).ToString();
        }

        /// <summary>
        /// Handles the Click event of the lbDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void lbDelete_Click( object sender, EventArgs e )
        {
            if ( DeleteClick != null )
            {
                DeleteClick( this, e );
            }
        }

        /// <summary>
        /// Occurs when delete is clicked.
        /// </summary>
        public event EventHandler DeleteClick;
    }

    /// <summary>
    /// Helper Class for serializing child data in viewstate
    /// </summary>
    [Serializable]
    public class PreRegistrationChild
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the nick.
        /// </summary>
        /// <value>
        /// The name of the nick.
        /// </value>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the suffix value identifier.
        /// </summary>
        /// <value>
        /// The suffix value identifier.
        /// </value>
        public int? SuffixValueId { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the grade offset.
        /// </summary>
        /// <value>
        /// The grade offset.
        /// </value>
        public int? GradeOffset { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone number.
        /// </summary>
        /// <value>
        /// The mobile phone number.
        /// </value>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the mobile country code.
        /// </summary>
        /// <value>
        /// The mobile country code.
        /// </value>
        public string MobileCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The mobile phone number.
        /// </value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the communication preference.
        /// </summary>
        /// <value>
        /// The communication preference.
        /// </value>
        public CommunicationType CommunicationPreference { get; set; }

        /// <summary>
        /// Gets or sets the type of the relationship.
        /// </summary>
        /// <value>
        /// The type of the relationship.
        /// </value>
        public int? RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the attribute values.
        /// </summary>
        /// <value>
        /// The attribute values.
        /// </value>
        public Dictionary<string, string> AttributeValues { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the profile photo binary file identifier.
        /// </summary>
        /// <value>
        /// The profile photo binary file identifier.
        /// </value>
        public int? ProfilePhotoId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreRegistrationChild"/> class.
        /// </summary>
        /// <param name="person">The person.</param>
        public PreRegistrationChild( Person person )
        {
            Id = person.Id;
            Guid = person.Guid;
            NickName = person.NickName;
            LastName = person.LastName;
            SuffixValueId = person.SuffixValueId;
            Gender = person.Gender;
            BirthDate = person.BirthDate;
            GradeOffset = person.GradeOffset;
            Age = person.Age;
            CommunicationPreference = person.CommunicationPreference;
            ProfilePhotoId = person.PhotoId;

            var mobilePhone = person.GetPhoneNumber( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE.AsGuid() );
            MobilePhoneNumber = mobilePhone?.Number;
            MobileCountryCode = mobilePhone?.CountryCode;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetAttributeValue( string key )
        {
            return AttributeValues.ContainsKey( key ) ? AttributeValues[key] : string.Empty;
        }

        /// <summary>
        /// Sets the attribute value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetAttributeValue( string key, string value )
        {
            AttributeValues.AddOrReplace( key, value );
        }
    }
}