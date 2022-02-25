using System;
using System.Runtime.Serialization;

using Rock.Data;
using Rock.Field;
using Rock.Model;

namespace Rock.Web.Cache
{
    /// <summary>
    /// Cache for <see cref="Rock.Model.WorkflowActionFormSection"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public class WorkflowActionFormSectionCache : ModelCache<WorkflowActionFormSectionCache, WorkflowActionFormSection>
    {
        #region Properties

        /// <inheritdoc cref="WorkflowActionFormSection.Title"/>
        [DataMember]
        public string Title { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.Description"/>
        [DataMember]
        public string Description { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.ShowHeadingSeparator"/>
        [DataMember]
        public bool ShowHeadingSeparator { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.SectionVisibilityRulesJSON"/>
        [DataMember]
        public string SectionVisibilityRulesJSON { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.Order"/>
        [DataMember]
        public int Order { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.WorkflowActionFormId"/>
        [DataMember]
        public int WorkflowActionFormId { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.SectionTypeValueId"/>
        [DataMember]
        public int? SectionTypeValueId { get; private set; }

        /// <inheritdoc cref="WorkflowActionFormSection.WorkflowActionForm"/>
        public WorkflowActionFormCache WorkflowActionForm => WorkflowActionFormCache.Get( WorkflowActionFormId );

        /// <inheritdoc cref="WorkflowActionFormSection.SectionType"/>
        public DefinedValueCache SectionType => SectionTypeValueId.HasValue
            ? DefinedValueCache.Get( SectionTypeValueId.Value )
            : null;

        /// <inheritdoc cref="WorkflowActionFormSection.SectionVisibilityRulesJSON"/>
        public FieldVisibilityRules SectionVisibilityRules { get; private set; }

        #endregion Properties

        /// <summary>
        /// Set's the cached objects properties from the model/entities properties.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void SetFromEntity( IEntity entity )
        {
            base.SetFromEntity( entity );

            WorkflowActionFormSection workflowActionFormSection = entity as WorkflowActionFormSection;
            if ( workflowActionFormSection == null )
            {
                return;
            }
            
            this.Title = workflowActionFormSection.Title;
            this.Description = workflowActionFormSection.Description;
            this.ShowHeadingSeparator = workflowActionFormSection.ShowHeadingSeparator;
            this.SectionVisibilityRulesJSON = workflowActionFormSection.SectionVisibilityRulesJSON;
            this.Order = workflowActionFormSection.Order;
            this.WorkflowActionFormId = workflowActionFormSection.WorkflowActionFormId;
            this.SectionTypeValueId = workflowActionFormSection.SectionTypeValueId;

            this.SectionVisibilityRules = workflowActionFormSection.SectionVisibilityRulesJSON?.FromJsonOrNull<FieldVisibilityRules>() ?? new FieldVisibilityRules();
        }
    }
}
