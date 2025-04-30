using CropDeals.Models;

public class CropCreateDto
{
    public string Name { get; set; }
    public CropTypeEnum Type { get; set; }
}

public class CropUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CropTypeEnum Type { get; set; }
}

public class CropDto
{
    public Guid Id { get; set; }          // Include Crop ID
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
}