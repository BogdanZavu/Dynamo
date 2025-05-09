using System;
using System.Collections.Generic;
using System.Linq;
using Dynamo.Configuration;
using Dynamo.Graph.Nodes;
using Dynamo.Properties;

namespace Dynamo.Search.SearchElements
{
    /// <summary>
    ///     Base class for all Dynamo Node search elements.
    /// </summary>
    public abstract class NodeSearchElement : ISearchEntry, ISource<NodeModel>, ICloneable
    {
        protected string iconName;

        private readonly HashSet<string> keywords = new HashSet<string>();
        protected readonly List<double> keywordWeights = new List<double>();
        private string description;        
        private SearchElementGroup group;
        private string assembly;
        private bool isVisibleInSearch = true;
        internal virtual bool IsExperimental { get; set; }  

        internal AutoCompletionNodeElementInfo AutoCompletionNodeElementInfo { get; set; } = new AutoCompletionNodeElementInfo();

        /// <summary>
        /// Event is fired when a node visibility in library search was changed.
        /// </summary>
        public Action VisibilityChanged;
        private void OnVisibilityChanged()
        {
            if (VisibilityChanged != null)
                VisibilityChanged();
        }

        /// <summary>
        ///     Specifies whether or not this entry should appear in search.
        /// </summary>
        public bool IsVisibleInSearch
        {
            get { return isVisibleInSearch; }
            set
            {
                if (value.Equals(isVisibleInSearch)) return;

                isVisibleInSearch = value;
                OnVisibilityChanged();
            }
        }

        /// <summary>
        /// The name that is used during node creation
        /// </summary>
        public virtual string CreationName { get { return this.Name; } }

        /// <summary>
        ///     List of nested categories this search element is contained in.
        /// </summary>
        public ICollection<string> Categories
        {
            get { return SplitCategoryName(FullCategoryName).ToList(); }
        }

        private const char CATEGORY_DELIMITER = '.';

        /// <summary>
        ///     Split a category name into individual category names splitting be DEFAULT_DELIMITER
        /// </summary>
        /// <param name="categoryName">The name</param>
        /// <returns>A list of output</returns>
        internal static IEnumerable<string> SplitCategoryName(string categoryName)
        {
            if (String.IsNullOrEmpty(categoryName))
                return Enumerable.Empty<string>();

            return
                categoryName.Split(CATEGORY_DELIMITER)
                    .Where(x => x != CATEGORY_DELIMITER.ToString() && !String.IsNullOrEmpty(x));
        }

        /// <summary>
        ///     The full name of entry which consists of category name and entry name.
        /// </summary>
        public virtual string FullName
        {
            get { return FullCategoryName + "." + Name; }
        }

        /// <summary>
        ///     The category name of this node.
        /// </summary>
        public string FullCategoryName
        {
            get;
            set;
        }

        /// <summary>
        ///     The name of this entry in search.
        /// </summary>
        string ISearchEntry.Name
        {
            get { return FullCategoryName + "." + Name; }
        }

        /// <summary>
        ///     The name of this entry as it appears in the library.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        ///     The parameters of this entry, used for overloaded nodes.
        /// </summary>
        public string Parameters
        {
            get;
            protected set;
        }

        /// <summary>
        ///     The search weight of this entry.
        /// </summary>
        public double Weight = 1;

        /// <summary>
        ///     Collection of keywords which can be used to search for this element.
        /// </summary>
        public ICollection<string> SearchKeywords
        {
            get { return keywords; }
        }

        /// <summary>
        ///     Description of the node.
        /// </summary>
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(description))
                {
                    return Configurations.NoDescriptionAvailable;
                }
                if (IsExperimental)
                {
                    return $"{Resources.DocsExperimentalPrefixMessage}" +
                        $"{Environment.NewLine}{Environment.NewLine}{description}"; 
                }
              

                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Returns the name of the node icon.
        /// </summary>
        public string IconName
        {
            get { return iconName; }
        }

        /// <summary>
        ///     Group to which Node belongs to 
        /// </summary>        
        public SearchElementGroup Group
        {
            get { return group; }
            set
            {
                if (value == group) return;
                group = value;
            }
        }

        /// <summary>
        ///     Assembly to which Node belongs to 
        /// </summary>        
        public string Assembly
        {
            get
            {
                if (!string.IsNullOrEmpty(assembly))
                    return assembly;

                // If there wasn't any assembly, then it's builtin function, operator or custom node.
                // Icons for these members are in DynamoCore project.
                return Configurations.DefaultAssembly;
            }
            protected set
            {
                assembly = value;
            }
        }

        protected List<Tuple<string, string>> inputParameters;
        public IEnumerable<Tuple<string, string>> InputParameters
        {
            get
            {
                if (!inputParameters.Any())
                    GenerateInputParameters();

                return inputParameters;
            }
        }

        protected List<string> outputParameters;
        public IEnumerable<string> OutputParameters
        {
            get
            {
                if (!outputParameters.Any())
                    GenerateOutputParameters();

                return outputParameters;
            }
        }

        /// <summary>
        ///     Indicates whether it is custom node or zero-touch element.
        ///     And whether this element comes from package or not.
        /// </summary>
        public ElementTypes ElementType
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Event fired when this search element produces a new NodeModel. This typically
        ///     happens when it is selected in the library by the user.
        /// </summary>
        public event Action<NodeModel> ItemProduced;
        protected virtual void OnItemProduced(NodeModel obj)
        {
            var handler = ItemProduced;
            if (handler != null) handler(obj);
        }

        /// <summary>
        ///     Creates a new NodeModel to be inserted into the current Dynamo workspace.
        /// </summary>
        /// <returns></returns>
        protected abstract NodeModel ConstructNewNodeModel();

        /// <summary>
        ///     Produces a new Node, via the ItemProduced event.
        /// </summary>
        internal void ProduceNode()
        {
            OnItemProduced(ConstructNewNodeModel());
        }

        internal NodeModel CreateNode()
        {
            return ConstructNewNodeModel();
        }

        ICollection<string> ISearchEntry.SearchTags
        {
            get
            {
                return SearchKeywords.ToList();
            }
        }

        IEnumerable<double> ISearchEntry.SearchTagWeights
        {
            get
            {
                return keywordWeights;
            }
        }

        protected virtual IEnumerable<string> GenerateOutputParameters()
        {
            outputParameters.Add(Properties.Resources.NoneString);
            return outputParameters;
        }

        protected virtual IEnumerable<Tuple<string, string>> GenerateInputParameters()
        {
            inputParameters.Add(Tuple.Create(String.Empty, Properties.Resources.NoneString));
            return inputParameters;
        }

        /// <summary>
        ///    Clones the NodeSearchElement
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    ///      This class will contain the information related to the node elements of Auto-completion feature.
    /// </summary>
    internal class AutoCompletionNodeElementInfo
    {
        internal int PortToConnect { get; set; }
    }

    /// <summary>
    /// This class will support the Machine Learning info related to a node element of Auto-completion feature.
    /// </summary>
    public class AutoCompletionNodeMachineLearningInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public AutoCompletionNodeMachineLearningInfo()
        {
            DisplayIcon = false;
            IsByRecommendation = false;
            IsByUse = false;
        }

        /// <summary>
        /// Creates ML info object using DisplayIcon, IsByRecommendation and ConfidenceScore params
        /// </summary>
        /// <param name="displayIcon">True if the user can view the confidence score or the recent use icon, otherwise False</param>
        /// <param name="isByRecommendation">True if the Node is part of the ML recommendation, otherwise False</param>
        /// <param name="confidenceScore">Confidence Score</param>
        public AutoCompletionNodeMachineLearningInfo(bool displayIcon, bool isByRecommendation, double confidenceScore)
        {
            DisplayIcon = displayIcon;
            IsByRecommendation = isByRecommendation;
            ConfidenceScore = confidenceScore;
        }

        /// <summary>
        /// Indicates if the icon is displayed to show the condifent score or by-usage
        /// </summary>
        public bool DisplayIcon { get; set; }

        /// <summary>
        /// Confidence score in percentage
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Indicates if the Node is part of ML result per use
        /// </summary>
        public bool IsByUse { get; set; }
        /// <summary>
        /// Indicates if the Node is part of the ML result per recommendation
        /// </summary>
        public bool IsByRecommendation { get; set; }
    }

    /// <summary>
    /// This class returns <see cref="NodeSearchElement"/> which is used 
    /// for creating a node, when the element is drag and dropped.
    /// </summary>
    public class DragDropNodeSearchElementInfo
    {
        /// <summary>
        /// Returns <see cref="NodeSearchElement"/> to drag and drop.
        /// </summary>
        public NodeSearchElement SearchElement { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropNodeSearchElementInfo"/> class.
        /// </summary>
        /// <param name="searchElement">Element to drag on the canvas</param>
        public DragDropNodeSearchElementInfo(NodeSearchElement searchElement)
        {
            this.SearchElement = searchElement;
        }
    }
}
