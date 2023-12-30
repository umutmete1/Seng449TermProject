using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

public class Stock {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("kod")]
    public string? Code { get; set; }
    [JsonProperty("ad")]
    public string? Name { get; set; }
    [JsonProperty("tip")]
    public string? Type { get; set; }
    
    // Price'yi tutmaya gerek yok bence, wssden canlı veriyi aktarırız
}