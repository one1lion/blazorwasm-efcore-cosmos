using System;

namespace BlazorWasmEfCoreCosmos.Models {
  public class Comment {
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  }
}