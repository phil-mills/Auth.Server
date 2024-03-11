namespace Server.Data.Entities
{
    using System;
    
    public class User
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        public IEnumerable<Guid> Roles { get; set; }
    }
}