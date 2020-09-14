using System;

namespace BlazorWasmEfCoreCosmos.Models {
  public partial class Comment {
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  }
}