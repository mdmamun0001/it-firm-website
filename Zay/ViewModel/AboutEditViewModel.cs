namespace Zay.ViewModel
{
    public class AboutEditViewModel:AboutCreateViewModel
    {
        public int Id { get; set; }
        public string? ExistingImage { get; set; }
        public string? ExistingLogo { get; set; }
    }
}
