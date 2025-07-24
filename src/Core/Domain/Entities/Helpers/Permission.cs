namespace Domain.Helpers
{
    public class Permission
    {
        public string RoleId { get; set; }
        public IList<RoleClaim> RoleClaims { get; set; }
    }

   
}
