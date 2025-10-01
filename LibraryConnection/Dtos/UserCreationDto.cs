using LibraryConnection.DbSet;

namespace LibraryConnection.Dtos
{
    public class UserCreationDto
    {
        public Client user { get; set; }

        public string bd { get; set; }

        public string sede { get; set; }

        public string environment { get; set; }
    }
}
