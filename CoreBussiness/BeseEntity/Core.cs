using DNTPersianUtils.Core;

namespace CoreBussiness.BeseEntity;

public class Core
{
    public Core()
    {
        CreationTime = DateTime.Now.ToShortPersianDateTimeString();
        DateTimeCreation = DateTime.Now;
    }
    public DateTime DateTimeCreation { get; set; }
    public int Id { get; set; }
  
    public string?CreationTime{ get; set; }
    public  string?ModificationTime { get; set; }
    
    public DateTime DateTimeModification { get; set; }
    public  bool IsDeleted { get; set; }
}