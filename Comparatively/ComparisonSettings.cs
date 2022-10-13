using System.Collections.Generic;

namespace Comparatively
{
    public class ComparisonSettings
    {
        public string DevPath { get; set; }
        public string QaPath { get; set; }
        public string ProdPath { get; set; }
        public string SortOrder { get; set; }
        public bool ClearOnGo { get; set; }
        public List<Setting3Tiers> Result { get; set; }
    }
}
