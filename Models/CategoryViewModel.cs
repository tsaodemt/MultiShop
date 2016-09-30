using System.Collections.Generic;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string NameVN { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Icon { get; set; }
    public int ParentId { get; set; }
    public int DisplayOrder { get; set; }
    public IList<Category> SubCategory { get; set; }

    public CategoryViewModel()
    {
        SubCategory = new List<Category>();
    }
}