using userManagement;

namespace t9n.api.Controllers
{
   public class CreateProjectRequest
   {
      public Tenant tenant { get; set; }
      public string projectName { get; set; }
   }
}