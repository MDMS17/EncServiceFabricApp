using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Action : BackboneElement
    {
        public string prefix { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string textEquivalent { get; set; }
        public string priority { get; set; }
        public List<CodeableConcept> code { get; set; }
        public List<CodeableConcept> reason { get; set; }
        public List<RelatedArtifact> documentation { get; set; }
        public List<string> goalId { get; set; }
        public Subject subject { get; set; }
        public List<TriggerDefinition> trigger { get; set; }
        public List<ConditionAction> condition { get; set; }
        public List<DataRequirement> input { get; set; }
        public List<DataRequirement> output { get; set; }
        public List<RelatedAction> relatedAction { get; set; }
        public TimingAction timing { get; set; }
        public List<ParticipantAction> participant { get; set; }
        public CodeableConcept type { get; set; }
        public string groupingBehavior { get; set; }
        public string selectionBehavior { get; set; }
        public string requiredBehavior { get; set; }
        public string precheckBehavior { get; set; }
        public string cardinalityBehavior { get; set; }
        public Definition definition { get; set; }
        public string transform { get; set; }
        public List<DynamicValue> dynamicValue { get; set; }
        public List<Action> action { get; set; }
    }
}
