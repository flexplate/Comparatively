namespace Comparatively
{
    public class Setting3Tiers
    {
        public string FolderName { get; set; }
        public string Key { get; set; }
        public string ValueDev { get; set; }
        public string ValueQa { get; set; }
        public string ValueProd { get; set; }
        public SettingComparisonResult Result { get; set; }

        public Setting3Tiers(string folderName, string key)
        {
            FolderName = folderName;
            Key = key;
        }
        public bool ValuesInternallyEqual
        {
            get { return ValueDev == ValueQa && ValueQa == ValueProd; }
        }

        public bool SameValuesAs(Setting3Tiers other)
        {
            return ValueDev == other.ValueDev && ValueQa == other.ValueQa && ValueProd == other.ValueProd;
        }
    }

}
