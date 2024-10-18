using MahApps.Metro.IconPacks;

namespace WpfApp2.Models
{
    public class CardModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PackIconMaterialKind IconKind { get; set; } = PackIconMaterialKind.None;
    }
}
