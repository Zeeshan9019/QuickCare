namespace Common_QuickCare.ViewModel
{
    public class BaseViewModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class BaseOrgViewModel : BaseViewModel
    {
        public int OrganizationId { get; set; }
    }
}
