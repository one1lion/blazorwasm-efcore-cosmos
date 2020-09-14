using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWasmEfCoreCosmos.Models {
  /// <summary>
  /// Represents a project
  /// </summary>
  /// <remarks>
  /// This model specifies the shape of the data to be stored as
  /// a Project document in the Portfolio collection.
  /// </remarks>
  public partial class Project {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Url { get; set; }

    public List<Comment> Comments { get; set; }
  }
}
